using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Models;

namespace TempManager.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Controler pro editaci podlaží.
	/// </summary>
	public class AdminFloorController : AdminBaseController
	{
		public AdminFloorController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService)
			: base(repositoriesFactory, floorService, userService)
		{
		}

		/// <summary>
		/// Zobrazí seznam všech podlaží.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var floors = await repositoriesFactory.FloorRepository.FetchAll();
			var model = await FetchModel(AdminWebLocation.Floors, new AdminListViewModel<Floor>(floors));
			
			return View(model);
		}

		/// <summary>
		/// Vrací editor konkrétního podlaží.
		/// </summary>
		public async Task<IActionResult> Edit(int id)
		{
			var floor = await this.repositoriesFactory.FloorRepository.FetchById(id);
			if (floor == null)
				return NotFound();

			var model = await FetchModel(AdminWebLocation.Floors, new FloorEditorViewModel(floor));
			return View(model);
		}

		/// <summary>
		/// Upraví konkrétní podlaží.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Edit(FloorEditorViewModel postedModel)
		{
			var model = await FetchModel(AdminWebLocation.Floors, postedModel);
			if(!ModelState.IsValid)
				return View(model);

			// Updatuji záznam.
			var floor = await this.repositoriesFactory.FloorRepository.FetchById(model.Id);
			model.Update(floor!);

			// Uložím změny.
			await this.repositoriesFactory.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		/// <summary>
		/// Nastaví viditelnost podlaží.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> SetVisible(int floorId, bool isVisible)
		{
			var floor = await this.repositoriesFactory.FloorRepository.FetchById(floorId);
			if (floor == null)
				return NotFound();

			floor.SetIsVisible(isVisible);
			await this.repositoriesFactory.SaveChanges();

			return Content("OK");
		}
	}
}