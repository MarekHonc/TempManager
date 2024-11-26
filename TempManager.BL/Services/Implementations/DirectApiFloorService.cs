using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using TempManager.BL.Models;
using TempManager.KNX.Api;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Implementace napojená přímo na API.
	/// </summary>
	public class DirectApiFloorService : IFloorService
	{
		private static ConcurrentDictionary<string, Floor> floors = new ConcurrentDictionary<string, Floor>();
		private readonly IApiService apiService;
		private static int? selectedFloor = null;

		public DirectApiFloorService(IApiSettings settings)
		{
			this.apiService = ApiServiceFactory.GetService(settings);
		}

		public async Task<Floor[]> GetFloors()
		{
			if (!await this.apiService.Ping())
				throw new Exception("Direct connection - api must be up!");

			var apiFloors = await this.apiService.GetVariables();
			var id = 0;

			foreach (var apiFloor in apiFloors.OrderBy(f => f.Name))
			{
				floors.AddOrUpdate(apiFloor.Name, s =>
				{
					return new Floor(id++, apiFloor.Name, apiFloor.Name, "Floor" + id);
				}, (s, floor) => floor);
			}

			return floors.Values.OrderBy(v => v.Id).ToArray();
		}

		public Task<Floor> GetLastSelectedFloor()
		{
			if (selectedFloor == null)
				return Task.FromResult(floors.Values.OrderBy(v => v.Id).First());

			return Task.FromResult(floors.Values.Single(f => f.Id == selectedFloor));
		}

		public Task<bool> SaveLastSelectedFloor(int floorId)
		{
			selectedFloor = floorId;

			return Task.FromResult(true);
		}

		public Task<Floor?> GetByFriendlyId(string friendlyId)
		{
			return Task.FromResult(floors.Single(f => f.Value.FriendlyId == friendlyId).Value);
		}
	}
}
