using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.Web.Models;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Hlavní controller aplikace.
	/// </summary>
	public class HomeController : BaseController
	{
		public HomeController(IFloorService floorService, IUserService userService)
			: base(floorService, userService)
		{
		}

		/// <summary>
		/// Vrací úvodní stránku aplikace - s posledním zobrazeným podlažím.
		/// </summary>
		[Route("/")]
		public async Task<IActionResult> Index()
		{
			return await GetView();
		}

		/// <summary>
		/// Vrací stránku pro zobrazení konkrétního podlaží.
		/// </summary>
		/// <param name="friendlyId"></param>
		/// <returns></returns>
		[Route("/Floor/{friendlyId}")]
		public async Task<IActionResult> Floor(string friendlyId)
		{
			// Vytáhnu podlaží.
			var floor = await this.floorService.GetByFriendlyId(friendlyId);
			if (floor == null)
				return NotFound($"Floor {friendlyId} does not exist!");

			// Uložím jako poslední zobrazené.
			await this.floorService.SaveLastSelectedFloor(floor.Id);

			// A vrátím konkrétní view.
			return await GetView();
		}

		/// <summary>
		/// Vrací konkrétní zobrazení pro aplikaci.
		/// </summary>
		private async Task<IActionResult> GetView()
		{
			var model = await FetchModel(WebLocation.Floor, new MainViewModel());

			// Pokud nemám ani jedno podlaží, apka není inicializovaná.
			if (model.Floors.Length == 0)
				return View("NotInitialized");

			// Inicializace modelu.
			model.Init();

			return View("Index", model);
		}
	}
}
