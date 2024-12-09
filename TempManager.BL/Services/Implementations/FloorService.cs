using TempManager.BL.Models;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pracující s podlažími které stáhne z databáze.
	/// </summary>
	public class FloorService : IFloorService
	{
		private readonly RepositoriesFactory repositoriesFactory;
		private readonly IUserService userService;

		public FloorService(RepositoriesFactory repositoriesFactory, IUserService userService)
		{
			this.repositoriesFactory = repositoriesFactory;
			this.userService = userService;
		}
		
		/// <inheritdoc cref="GetFloors"/>
		public async Task<Floor[]> GetFloors()
		{
			var floorsQuery = new GetVisibleFloorsQuery();
			var floors = await this.repositoriesFactory.FloorRepository.Fetch(floorsQuery);

			return floors
				.Select(f => new Floor(f))
				.ToArray();
		}

		/// <inheritdoc cref="GetLastSelectedFloor"/>
		public async Task<Floor> GetLastSelectedFloor()
		{
			var user = await this.userService.GetCurrentUser();
			var dbUser = await this.repositoriesFactory.UserRepository.FetchById(user.Id);

			if (dbUser!.SelectedFloorId != null)
				return new Floor(dbUser.SelectedFloor!);

			var floorsQuery = new GetVisibleFloorsQuery();
			var floor = await this.repositoriesFactory.FloorRepository.FetchOne(floorsQuery);

			return new Floor(floor!);
		}

		/// <inheritdoc cref="SaveLastSelectedFloor"/>
		public async Task<bool> SaveLastSelectedFloor(int floorId)
		{
			var floor = await this.repositoriesFactory.FloorRepository.FetchById(floorId);
			if (floor == null)
				return false;

			var user = await this.userService.GetCurrentUser();
			var dbUser = await this.repositoriesFactory.UserRepository.FetchById(user.Id);

			dbUser!.SetSelectedFloor(floor);

			await this.repositoriesFactory.SaveChanges();

			return true;
		}

		/// <inheritdoc cref="GetByFriendlyId"/>
		public async Task<Floor?> GetByFriendlyId(string friendlyId)
		{
			var floorQuery = new FloorByFriendlyIdQuery(friendlyId);
			var floor = await this.repositoriesFactory.FloorRepository.FetchOne(floorQuery);

			return floor != null ?  new Floor(floor) : null;
		}
	}
}
