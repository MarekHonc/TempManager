using TempManager.Common;
using TempManager.DL.Repositories;
using TempManager.KNX.Api;
using TempManager.DL.Entities;
using TempManager.DL.Queries;
using Floor = TempManager.BL.Models.Floor;
using Room = TempManager.BL.Models.Room;

namespace TempManager.BL.SyncService
{
	/// <summary>
	/// Job na pozadí, který synchronizuje hodnoty z API vůči lokální databázi.
	/// </summary>
	public class ValueSyncService
	{
		private const double IsSameThreshold = 0.01;

		private readonly RepositoriesFactory repositoriesFactory;
		private readonly IApiService apiService;

		private Dictionary<string, DL.Entities.Floor> floorCache;

		public ValueSyncService(RepositoriesFactory repositoriesFactory, IApiSettings settings)
		{
			this.repositoriesFactory = repositoriesFactory;
			this.apiService = ApiServiceFactory.GetService(settings);
		}

		/// <summary>
		/// Zesynchronizuje všechny podlaží z API do aplikace.
		/// </summary>
		public async Task<Floor[]> SyncFloors()
		{
			// Api je offline -> neřeším nic.
			if (!await this.apiService.Ping())
				return Array.Empty<Floor>();

			// Stáhnu existující hodnoty z DB + nové hodnoty z API.
			this.floorCache = await this.repositoriesFactory.FloorRepository.GetExternalIdLookUp();
			var variables = await this.apiService.GetVariables();

			// Vrací vše, co získám z API.
			var result = new List<DL.Entities.Floor>();

			// A zesynchronizuji, situaci, kdy v API proměnná ubyla neřeším.
			using (this.repositoriesFactory.BulkChange())
			{
				foreach (var variable in variables)
				{
					// Pokud patro neexistuje, tak ho vytvořím.
					if (!this.floorCache.TryGetValue(variable.Name, out DL.Entities.Floor floor))
					{
						floor = await DL.Entities.Floor.Create(
							repositoriesFactory.FloorRepository,
							variable.Name,
							ignoreExternalIdCheck: false
						);

						await this.repositoriesFactory.FloorRepository.Add(floor);
					}

					result.Add(floor);
				}
			}

			// Uložím změny.
			await this.repositoriesFactory.SaveChanges();

			// A vrátím natažené podlaží.
			return result
				.Select(f => new Floor(f))
				.ToArray();
		}

		/// <summary>
		/// Zesynchronizuje naměřené hodnoty v místnostech z API do aplikace.
		/// </summary>
		public async Task<Dictionary<Floor, Room[]>> SyncRooms()
		{
			var result = new Dictionary<Floor, Room[]>();
			var syncTime = DateTimeOffset.UtcNow;

			// Api je offline -> neřeším nic.
			if (!await this.apiService.Ping())
				return result;

			// Stáhnu si všechny místnosti z DB.
			var existingRooms = await this.repositoriesFactory.RoomRepository.GetExternalIdLookUp();

			// Synchronizace do PLC.
			var pendingChangesQuery = new PendingChangesQuery().Include(nameof(SetTemperature.Room));
			var pendingChanges = await this.repositoriesFactory.SetTemperatureRepository.Fetch(pendingChangesQuery);

			// Roztřídím podle podlaží.
			var pendingChangesForFloor = pendingChanges
				.GroupBy(p => p.Room.FloorId)
				.ToDictionary(k => k.Key, v => v);

			// Pro každé patro co je v cahce.
			foreach (var floor in this.floorCache)
			{
				Room[] rooms;
				Dictionary<int, List<SetTemperature>> pendingChangesForRoom = null;
				var apiVariable = new ApiVariable() { Name = floor.Key };

				// Kouknu, jestli mám něco pro podlaží.
				if (pendingChangesForFloor.TryGetValue(floor.Value.Id, out var changes))
				{
					pendingChangesForRoom = changes
						.GroupBy(p => p.RoomId)
						.ToDictionary(
							k => k.Key,
							v => v.OrderBy(st => st.Date).ToList()
						);
				}

				// Stáhnu nové hodnoty z API.
				var apiRooms = await this.apiService.GetValues(apiVariable);

				// A zesynchronizuji, situaci, kdy v API hodnota ubyla neřeším.
				using (this.repositoriesFactory.BulkChange())
				{
					// Zjištěné hodnoty.
					var roomValues = new List<(DL.Entities.Room room, DL.Entities.JsonTypes.RoomValue value)>();
					var changesToPromote = new List<SetTemperature>();

					// Každou místnost synchronizuji.
					foreach (var apiRoom in apiRooms)
					{
						// Pokud patro neexistuje, tak ho vytvořím.
						if (!existingRooms.TryGetValue(apiRoom.Name, out DL.Entities.Room room))
						{
							room = await DL.Entities.Room.Create(
								repositoriesFactory.RoomRepository,
								floor.Value,
								apiRoom.Name,
								ignoreExternalIdCheck: false
							);

							// Vytvořím místnost.
							await this.repositoriesFactory.RoomRepository.Add(room);
						}
						// Pokud existuje, mohu se pokusit do něj promítnou nový stav
						else if (pendingChangesForRoom?.TryGetValue(room.Id, out var roomChanges) == true)
						{
							// Zpracování změn.
							for (var i = 0; i < roomChanges.Count; i++)
							{
								var current = roomChanges[i];

								// Pokud se jedná o poslední, tak změnu můžu zkusit promotnout.
								if (i == roomChanges.Count - 1)
								{
									// Teplota je +- 0.01 stejná, pokusím se udělat promote změny.
									if (Math.Abs(current.OldTemperature - apiRoom.DesiredTemperature) < IsSameThreshold)
									{
										changesToPromote.Add(current);
										apiRoom.SetDesiredTemperature(current.NewTemperature);
									}
									// Někdo mi změnil externě, ignoruji.
									else
									{
										current.SetResult(SetTemperatureResult.FailedTemperatureOverridenExternally);
									}
								}
								// Někdo další změnil teplotu, změnu "ignoruji".
								else
								{
									current.SetResult(SetTemperatureResult.FailedTemperatureOverridenInternally);
								}
							}
						}

						// Ještě připravím hodnoty pro uložení.
						var roomValue = new DL.Entities.JsonTypes.RoomValue(
							room.ExternalId,
							apiRoom.Temp,
							apiRoom.Rh,
							apiRoom.CO2,
							apiRoom.DesiredTemperature,
							apiRoom.ValveOpen
						);

						// A vložím do hotnot.
						roomValues.Add((room, roomValue));
					}

					// Inicializuji prázné pole, které níže naplním.
					rooms = new Room[roomValues.Count];
					var toSave = new DL.Entities.JsonTypes.RoomValue[roomValues.Count];

					// Nakonec vytvořím dto + uložím hodnoty do DB.
					for (var i = 0; i < roomValues.Count; i++)
					{
						var current = roomValues[i];

						// 1. K uložení.
						toSave[i] = current.value;

						// 2. K navrácení z metody.
						rooms[i] = Room.Create(
							current.room,
							current.value,
							isAdmin: false,
							userToRoom: null
						);
					}

					// Historii uložím.
					await this.repositoriesFactory.FloorHistoryRepository.Add(
						DL.Entities.FloorHistory.Create(floor.Value, syncTime, toSave)
					);

					// Pokud mám promítnout změny.
					if (changesToPromote.Count > 0)
					{
						var success = await this.apiService.PutValues(apiVariable, apiRooms);
						var desiredState = success ? SetTemperatureResult.Success : SetTemperatureResult.Failed;
						changesToPromote.ForEach(ch => ch.SetResult(desiredState));
					}
				}

				// Uložím změny.
				await this.repositoriesFactory.SaveChanges();

				// A doplním do dictionary.
				result.Add(new Floor(floor.Value), rooms);
			}

			// A vracím aktuální hodnoty.
			return result;
		}
	}
}