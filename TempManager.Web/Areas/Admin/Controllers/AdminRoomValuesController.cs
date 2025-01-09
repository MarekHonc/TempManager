using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Models;

namespace TempManager.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Kontroler pro zobrazení historie naměřených hodnot.
	/// </summary>
	public class AdminRoomValuesController : AdminBaseController
	{
		private const int pageSize = 10;

		public AdminRoomValuesController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService) : base(repositoriesFactory, floorService, userService)
		{
		}

		/// <summary>
		/// Vrací hlavní stránku s historíí naměřených hodnot.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var floor = await this.floorService.GetLastSelectedFloor();

			// Vytvořím model.
			var model = await FetchModel(
				AdminWebLocation.RoomValues,
				new AdminPagedListViewModel<FloorHistory>(await GetList(floor.Id))
			);

			return View(model);
		}

		/// <summary>
		/// Vrací požadovanou stránku historie hodnot.
		/// </summary>
		private async Task<IPagedList<FloorHistory>> GetList(int floorId, int page = 1)
		{
			var query = new FloorHistoryLatestQuery(floorId);
			var result = await this.repositoriesFactory.FloorHistoryRepository.FetchPage(query, page, pageSize);

			return result;
		}
	}
}