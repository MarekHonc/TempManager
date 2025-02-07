using TempManager.BL.Models;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pracující s místnostmi které stáhne z databáze.
	/// </summary>
	public class RoomService : IRoomService
	{
		private readonly RepositoriesFactory repositoriesFactory;
		private readonly IUserService userService;

		public RoomService(RepositoriesFactory repositoriesFactory, IUserService userService)
		{
			this.repositoriesFactory = repositoriesFactory;
			this.userService = userService;
		}

		/// <inheritdoc cref="GetRooms"/>
		public async Task<Room[]> GetRooms(int? floorId = null)
		{
			var user = await this.userService.GetCurrentUser();

			// Mapování oprávění.
			var userRightsQuery = new UserToRoomForUserQuery(user.Id);
			var userRights = await this.repositoriesFactory.UserToRoomRepository.Fetch(userRightsQuery);
			var userRightDictionary = userRights.ToDictionary(k => k.RoomId);

			// Získám místnosti.
			var roomsQuery = new RoomsByFloorIdQuery(floorId)
				.Include(nameof(DL.Entities.Room.Floor));

			var rooms = (await this.repositoriesFactory.RoomRepository.Fetch(roomsQuery))
				.ToDictionary(k => k.ExternalId, v => v);

			// Získám poslední hodnoty.
			var historyQuery = new FloorHistoryLatestQuery(floorId);
			var historyResult = await this.repositoriesFactory.FloorHistoryRepository.Fetch(historyQuery);
			var history = historyResult
				.SelectMany(r => r.RoomValues)
				.ToArray();

			if (history.Length == 0)
				return [];

			// A jdu poskládat výsledek.
			var result = new Room[history.Length];
			var index = 0;

			foreach (var roomValue in history)
			{
				// Našel jsem místnost.
				if (rooms.TryGetValue(roomValue.ExternalRoomId, out var room))
				{
					// Kouknu i na práva.
					userRightDictionary.TryGetValue(room.Id, out var userToRoom);
					result[index] = Room.Create(room, roomValue, isAdmin: user.IsAdmin, userToRoom: userToRoom);
				}
				else
				{
					result[index] = Room.Create(roomValue);
				}

				index++;
			}

			return result;
		}

		/// <summary>
		/// Nastaví teplotu v místnosti.
		/// </summary>
		public async Task<bool> SetTemperature(int roomId, double newTemperature)
		{
			var user = await this.userService.GetCurrentUser();
			var room = await this.repositoriesFactory.RoomRepository.FetchById(roomId);

			// Pokud není admin.
			if (!user.IsAdmin)
			{
				var userRightsQuery = new UserToRoomForUserQuery(user.Id, room.Id);
				var userRights = await this.repositoriesFactory.UserToRoomRepository.FetchOne(userRightsQuery);

				// Žádný záznam neexistuje nebo nemá právo.
				if (userRights == null || !userRights.HasRightToEdit)
					return false;
			}

			// Stáhnu poslední hodnoty pro celé podlaží.
			var values = await this.repositoriesFactory.FloorHistoryRepository.FetchOne(
				new FloorHistoryLatestQuery(room.FloorId)
			);

			var latestTemp = values?.RoomValues.FirstOrDefault(r => r.ExternalRoomId == room.ExternalId);

			// Vytvořím záznam o uložení teploty.
			var temperature = DL.Entities.SetTemperature.Create(
				user.Id,
				roomId,
				latestTemp?.DesiredTemperature ?? 0,
				newTemperature
			);

			// Uložím do databáze.
			await this.repositoriesFactory.SetTemperatureRepository.Add(temperature);
			await this.repositoriesFactory.SaveChanges();

			return true;
		}

		/// <inheritdoc cref="SetFavorite"/>
		public async Task<bool> SetFavorite(int roomId, bool isFavorite)
		{
			// Místnost musí existovat.
			var room = await this.repositoriesFactory.RoomRepository.FetchById(roomId);
			if (room == null)
				return false;

			// Vytáhnu usera.
			var user = await this.userService.GetCurrentUser();

			// Kouknu, jestli existuje vazba.
			var userToRoom = await this.repositoriesFactory.UserToRoomRepository.FetchById(user.Id, roomId);

			// Existuje -> nastavím
			if (userToRoom != null)
			{
				userToRoom.SetFavorite(isFavorite);
			}
			else
			{
				userToRoom = DL.Entities.UserToRoom.Create(user.Id, roomId, isFavorite, hasRight: false);
				await this.repositoriesFactory.UserToRoomRepository.Add(userToRoom);
			}

			// Uložím změny.
			await this.repositoriesFactory.SaveChanges();

			return true;
		}
	}
}