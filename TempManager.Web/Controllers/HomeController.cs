using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.Web.Code;
using TempManager.Web.Models;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Hlavní controller aplikace.
	/// </summary>
	public class HomeController : BaseController
	{
		private readonly ICompositeViewEngine viewEngine;
		private readonly CookieManager cookieManager;

		public HomeController(IFloorService floorService, IUserService userService, ICompositeViewEngine viewEngine, CookieManager cookieManager)
			: base(floorService, userService)
		{
			this.cookieManager = cookieManager;
			this.viewEngine = viewEngine;
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
		/// Pøepne zobrazení v daném prohlížeèi pro klienta.
		/// </summary>
		public IActionResult SwitchView(FloorViewType viewType)
		{
			this.cookieManager.Save(CookieManager.FloorViewTypeCookieName, viewType);
			return RedirectToAction(nameof(Index));
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
			model.Init(this.cookieManager, this.viewEngine);

			return View("Index", model);
		}
	}
}