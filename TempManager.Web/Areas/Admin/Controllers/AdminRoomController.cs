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
	/// Kontroler pro práci s místnostmi.
	/// </summary>
	public class AdminRoomController : AdminBaseController
	{
		public AdminRoomController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService)
			: base(repositoriesFactory, floorService, userService)
		{
		}

		/// <summary>
		/// Zobrazí seznam všech místnosti.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var rooms = await this.repositoriesFactory.RoomRepository.FetchAll();
			var model = await FetchModel(AdminWebLocation.Rooms, new AdminListViewModel<Room>(rooms));

			return View(model);
		}

		/// <summary>
		/// Zobrazí editor pro místnost.
		/// </summary>
		public async Task<IActionResult> Edit(int id)
		{
			var room = await this.repositoriesFactory.RoomRepository.FetchById(id);
			if (room == null)
				return NotFound();

			var model = await FetchModel(AdminWebLocation.Rooms, new RoomEditorViewModel(room));
			return View(model);
		}

		/// <summary>
		/// Upraví konkrétní místnost.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Edit(RoomEditorViewModel postedModel)
		{
			var model = await FetchModel(AdminWebLocation.Floors, postedModel);
			if (!ModelState.IsValid)
				return View(model);

			// Updatuji záznam.
			var room = await this.repositoriesFactory.RoomRepository.FetchById(model.Id);
			model.Update(room!);

			// Uložím změny.
			await this.repositoriesFactory.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		/// <summary>
		/// Vrací uživatele, který se automaticky doplňují.
		/// </summary>
		public async Task<IActionResult> AutoCompleteUser(string search)
		{
			if (string.IsNullOrEmpty(search))
				return Json(Enumerable.Empty<object>());

			var query = new UserSearchQuery(search);
			var users = await this.repositoriesFactory.UserRepository.FetchCount(query, count: 10);

			return Json(users.Select(u => new
			{
				id = u.Id,
				name = u.UserName
			}));
		}
	}
}