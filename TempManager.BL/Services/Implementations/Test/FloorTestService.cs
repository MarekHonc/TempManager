using TempManager.BL.Models;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Testovací služba pro práci s podlažími.
	/// </summary>
	public class FloorTestService : IFloorService
	{
		private static int? selectedFloorId = null;

		private readonly Floor[] floors = new Floor[]
		{
			new Floor(1, "0NP", "nulté patro", "Floor0"),
			new Floor(2, "1NP", "první patro", "Floor2"),
			new Floor(3, "2NP", "druhé patro", "Floor3"),
			new Floor(4, "3NP", "třetí patro", "Floor4"),
			new Floor(5, "4NP", "čtvrté patro", "Floor5"),
		};

		public Task<Floor[]> GetFloors()
		{
			return Task.FromResult(floors);
		}

		public Task<Floor> GetLastSelectedFloor()
		{
			return Task.FromResult(floors.FirstOrDefault(f => f.Id == selectedFloorId) ?? floors.First());
		}

		public Task<bool> SaveLastSelectedFloor(int floorId)
		{
			selectedFloorId = floorId;
			return Task.FromResult(true);
		}

		public Task<Floor> GetByFriendlyId(string floorId)
		{
			return Task.FromResult(this.floors.FirstOrDefault(f => f.FriendlyId == floorId));
		}
	}
}