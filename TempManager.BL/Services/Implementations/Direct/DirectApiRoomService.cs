using System.Text.RegularExpressions;
using TempManager.BL.Models;
using TempManager.KNX.Api;

namespace TempManager.BL.Services.Implementations.Direct
{
	/// <summary>
	/// Implementace napojená přímo na API.
	/// </summary>
	public class DirectApiRoomService : IRoomService
	{
		private readonly IFloorService floorService;
		private readonly IApiService apiService;

		public DirectApiRoomService(IFloorService floorService, IApiSettings apiSettings)
		{
			this.floorService = floorService;
			apiService = ApiServiceFactory.GetService(apiSettings);
		}

		public async Task<Room[]> GetRooms(int floorId)
		{
			if (!await apiService.Ping())
				throw new Exception("Direct connection - api must be up!");

			var floors = await floorService.GetFloors();
			var floor = floors.Single(f => f.Id == floorId);

			var result = new List<Room>();
			var apiRooms = await apiService.GetValues(new ApiVariable() { Name = floor.FriendlyId });
			foreach (var room in apiRooms)
			{
				var r = new Room(
					int.Parse(Regex.Match(room.Name, @"\d+").Value),
					room.Name,
					room.Name,
					true,
					false,
					room.Temp,
					room.Rh,
					room.DesiredTemperature,
					room.ValveOpen
				);

				result.Add(r);
			}

			return result.OrderBy(r => r.Name).ToArray();
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