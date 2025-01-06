using TempManager.DL.Repositories;
using TempManager.KNX.Api;
using TempManager.BL.Models;

namespace TempManager.BL.SyncService
{
	/// <summary>
	/// Job na pozadí, který synchronizuje hodnoty z API vůči lokální databázi.
	/// </summary>
	public class ValueSyncService
	{
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

			// Pro každé patro co je v cahce.
			foreach (var floor in this.floorCache)
			{
				Room[] rooms;

				// Stáhnu existující hodnoty z DB + nové hodnoty z API.
				var existingRooms = await this.repositoriesFactory.RoomRepository.GetExternalIdLookUp();
				var apiRooms = await this.apiService.GetValues(new ApiVariable() { Name = floor.Key });

				// A zesynchronizuji, situaci, kdy v API hodnota ubyla neřeším.
				using (this.repositoriesFactory.BulkChange())
				{
					// Zjištěné hodnoty.
					var roomValues = new List<(DL.Entities.Room room, DL.Entities.JsonTypes.RoomValue value)>();

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
						roomValues.Add((room, roomValue));;
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
							isAdmin: true, // TODO: reálná hodnota
							userToRoom: null // TODO: reálná hodnota
						);
					}

					// Historii uložím.
					await this.repositoriesFactory.FloorHistoryRepository.Add(
						DL.Entities.FloorHistory.Create(floor.Value, syncTime, toSave)
					);
				}

				// Uložím změny.
				await this.repositoriesFactory.SaveChanges();

				// A doplním do dictionary.
				result.Add(new Floor(floor.Value), rooms);
			}

			// TODO: synchronizace nastavených hodnot

			// A vracím aktuální hodnoty.
			return result;
		}
	}
}