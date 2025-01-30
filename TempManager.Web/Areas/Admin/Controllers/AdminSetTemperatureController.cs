using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Localization;
using TempManager.Web.Areas.Admin.Models;

namespace TempManager.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Kontroller zobrazující historii nastavování teploty.
	/// </summary>
	public class AdminSetTemperatureController : AdminBaseController
	{
		private const int pageSize = 4;

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
				new AdminBaseViewModel()
			);

			return View(model);
		}

		/// <summary>
		/// Vrací konkrétní stránku historie.
		/// </summary>
		public async Task<IActionResult> List(int page, int? userId, int? roomId)
		{
			var history = await GetList(page, userId, roomId);

			return Json(new
			{
				items = history.Select(r => new
				{
					date = r.Date.ToLocalTime().ToString("dd. MM. yyyy hh:mm"),
					userName = r.User.UserName,
					roomName = r.Room.Name,
					newTemperature = r.NewTemperature,
					result = GetLocalizedResult(r.Result)
				}),
				hasNext = history.HasNextPage,
				page = history.PageNumber
			});
		}

		/// <summary>
		/// Vrací požadovanou stránku historie.
		/// </summary>
		private async Task<IPagedList<SetTemperature>> GetList(int page = 1, int? userId = null, int? roomId = null)
		{
			var query = new SetTemperatureHistoryQuery(userId, roomId).Include(nameof(DL.Entities.User), nameof(Room));
			var result = await this.repositoriesFactory.SetTemperatureRepository.FetchPage(query, page, pageSize);

			return result;
		}

		/// <summary>
		/// Vrací lokalizovaný výsledek operace.
		/// </summary>
		private string GetLocalizedResult(SetTemperatureResult result)
		{
			switch (result)
			{
				case SetTemperatureResult.Pending:
					return AdminResources.SetTemperatureResult_Pending;
				case SetTemperatureResult.Success:
					return AdminResources.SetTemperatureResult_Success;
				case SetTemperatureResult.FailedTemperatureOverridenInternally:
					return AdminResources.SetTemperatureResult_FailedTemperatureOverridenInternally;
				case SetTemperatureResult.FailedTemperatureOverridenExternally:
					return AdminResources.SetTemperatureResult_FailedTemperatureOverridenExternally;
				case SetTemperatureResult.Failed:
					return AdminResources.SetTemperatureResult_Failed;
				default:
					throw new Exception($"Localization of result {result} is not supported");
			}
		}
	}
}