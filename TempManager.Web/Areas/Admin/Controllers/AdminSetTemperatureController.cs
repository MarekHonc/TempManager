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
	/// Kontroller zobrazující historii nastavování teploty.
	/// </summary>
	public class AdminSetTemperatureController : AdminBaseController
	{
		private const int pageSize = 20;

		public AdminSetTemperatureController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService)
			: base(repositoriesFactory, floorService, userService)
		{
		}

		/// <summary>
		/// Vrací hlavní stránku s přehledem historie nastavení teploty.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var model = await FetchModel(
				AdminWebLocation.SetTemperature,
				new AdminPagedListViewModel<SetTemperature>(await GetList())
			);

			return View(model);
		}

		/// <summary>
		/// Vrací konkrétní stránku historie.
		/// </summary>
		public async Task<IActionResult> List(int page, int? userId, int? roomId)
		{
			var result = await GetList(page, userId, roomId);
			return Json(result);
		}

		/// <summary>
		/// Vrací požadovanou stránku historie.
		/// </summary>
		private async Task<IPagedList<SetTemperature>> GetList(int page = 1, int? userId = null, int? roomId = null)
		{
			var query = new SetTemperatureHistoryQuery(userId, roomId);
			var result = await this.repositoriesFactory.SetTemperatureRepository.FetchPage(query, page, pageSize);

			return result;
		}
	}
}