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
	/// Kontroler pro nastavování uživatelských oprávnění.
	/// </summary>
	public class AdminUserRightsController : AdminBaseController
	{
		public AdminUserRightsController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService)
			: base(repositoriesFactory, floorService, userService)
		{
		}

		/// <summary>
		/// Zobrazí seznam všech uživatelů.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var users = await repositoriesFactory.UserRepository.FetchAll();
			var model = await FetchModel(AdminWebLocation.UserRights, new AdminListViewModel<User>(users));

			return View(model);
		}

		/// <summary>
		/// Vrací editor konkrétního uživatele.
		/// </summary>
		public async Task<IActionResult> Edit(int id)
		{
			var user = await this.repositoriesFactory.UserRepository.FetchById(id);
			if (user == null)
				return NotFound();

			var model = await FetchModel(AdminWebLocation.UserRights, new UserRightsEditorViewModel(user));
			return View(model);
		}

		/// <summary>
		/// Upraví konkrétního uživatele.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Edit(UserRightsEditorViewModel postedModel)
		{
			var model = await FetchModel(AdminWebLocation.UserRights, postedModel);
			if (!ModelState.IsValid)
				return View(model);

			// Updatuji záznam.
			var user = await this.repositoriesFactory.UserRepository.FetchById(model.Id);
			model.Update(user);

			// Uložím změny.
			await this.repositoriesFactory.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		/// <summary>
		/// Vrací místnosti, které se automaticky doplňují.
		/// </summary>
		public async Task<IActionResult> AutoCompleteRoom(string search)
		{
			if (string.IsNullOrEmpty(search))
				return Json(Enumerable.Empty<object>());

			var query = new RoomsSearchQuery(search);
			var rooms = await this.repositoriesFactory.RoomRepository.FetchCount(query, count: 10);

			return Json(rooms.Select(r => new
			{
				id = r.Id,
				name = r.Name
			}));
		}
	}
}