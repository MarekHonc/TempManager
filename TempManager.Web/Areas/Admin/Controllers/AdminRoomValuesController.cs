using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Common.Extensions;
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

		public AdminRoomValuesController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(repositoriesFactory, floorService, userService, syncService)
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
				new AdminBaseViewModel()
			);

			return View(model);
		}

		/// <summary>
		/// Vrací konkrétní stránku historie.
		/// </summary>
		public async Task<IActionResult> List(int floorId, int page)
		{
			var history = await GetList(floorId, page);
			var rooms = await this.repositoriesFactory.RoomRepository.GetExternalIdLookUp();
			var result = new List<object>();

			// "Zplošním" kolekci.
			foreach (var measure in history)
			{
				foreach (var roomValue in measure.RoomValues)
				{
					rooms.TryGetValue(roomValue.ExternalRoomId, out var room);

					result.Add(new
					{
						date = measure.Date.ToShortDateTime(),
						room = room?.Name,
						id = room?.Id,
						temperature = roomValue.Temperature,
						co2 = roomValue.CO2,
						rh = roomValue.Rh
					});
				}
			}

			return Json(new
			{
				items = result,
				hasNext = history.HasNextPage,
				page = history.PageNumber
			});
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