using TempManager.BL.Models;

namespace TempManager.BL.Services.Implementations.Test
{
	/// <summary>
	/// Testovací služba pro práci s místnostmi.
	/// </summary>
	public class RoomTestService : IRoomService
	{
		private readonly Room[] rooms = new Room[]
		{
			new Room(1, "A1042 MTI Diblik", true, false, 21.26, 41, 700.799988, 21, false),
			new Room(2, "A1043 MTI Tyl", true, false, 22.98, 25, 446.720001, 23, true),
			new Room(3, "A1044 MTI Rous", true, false, 23.48, 28, 438.720001, 24, true),
			new Room(4, "A1045 MTI rekr", true, false, 23.280001, 33, 461.76001, 23.5, true),
			new Room(5, "A1046 MTI Richter", true, false, 21.540001, 34, 398.720001, 21, false),
			new Room(6, "A1047 MTI Nosek", true, false, 22.040001, 41, 392.959991, 22, true),
			new Room(7, "A1048 MTI Kolar", true, false, 22.059999, 40, 609.919983, 22, false),
			new Room(8, "A1050 A103/TK4", true, false, 21.98, 39, 428.799988, 22, true),
			new Room(9, "A1051 A110/AP12", true, false, 21.879999, 38, 505.920013, 22, false),
			new Room(10, "A1056 A109/AP11", true, false, 21.940001, 36, 449.920013, 22, false),
			new Room(11, "A1057 A107/AP9", true, false, 21.940001, 31, 454.720001, 22, true),
			new Room(12, "A1059 WC z", true, false, 19, 34, 0, 19, false),
			new Room(13, "A1062 WC m", true, false, 18.84, 31, 0, 19, true),
			new Room(14, "A1065 chodba", true, false, 18.620001, 40, 0, 18, false),
			new Room(15, "A1069 MTI Hernych", true, false, 23.139999, 33, 419.839996, 24, true)
		};

		public Task<Room[]> GetRooms(int floorId)
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