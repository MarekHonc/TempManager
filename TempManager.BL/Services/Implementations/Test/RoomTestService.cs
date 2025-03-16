using TempManager.BL.Models;
using TempManager.Common;

namespace TempManager.BL.Services.Implementations.Test
{
	/// <summary>
	/// Testovací služba pro práci s místnostmi.
	/// </summary>
	public class RoomTestService : IRoomService
	{
		private readonly Room[] rooms = new Room[]
		{
			new Room(1, "A1042 MTI Diblik", "1NP", "A1042 MTI Diblik", true, false, 21.26, 41, 21, 0, Groups.MTI),
			new Room(2, "A1043 MTI Tyl", "1NP", "A1043 MTI Tyl", true, false, 22.98, 25, 23, 100, Groups.MTI),
			new Room(3, "A1044 MTI Rous", "1NP", "A1044 MTI Rous", true, false, 23.48, 28, 24, 100, Groups.MTI),
			new Room(4, "A1045 MTI rekr", "1NP", "A1045 MTI rekr", true, false, 23.280001, 33, 23.5, 0, Groups.MTI)
		};

		public Task<Room[]> GetRooms(int? floorId)
		{
			if (floorId == 2)
				return Task.FromResult(rooms);

			return Task.FromResult(Array.Empty<Room>());
		}

		public Task<bool> SetTemperature(int roomId, double newTemperature)
		{
			return Task.FromResult(true);
		}

		public Task<bool> SetFavorite(int roomId, bool isFavorite)
		{
			return Task.FromResult(true);
		}
	}
}