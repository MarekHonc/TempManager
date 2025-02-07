using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Web.Code;
using TempManager.Web.Models;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Bázový kontroler pro aplikaci.
	/// </summary>
	[Authorize]
	[ShibbolethAuthorize]
	public class BaseController : Controller
	{
		protected readonly IFloorService floorService;
		protected readonly IUserService userService;
		protected readonly IValueSyncService syncService;

		public BaseController(IFloorService floorService, IUserService userService, IValueSyncService syncService)
		{
			this.floorService = floorService;
			this.userService = userService;
			this.syncService = syncService;
		}

		/// <summary>
		/// Naplní model základními hodnotami pro zobrazení aplikace.
		/// </summary>
		protected async Task<T> FetchModel<T>(WebLocation webLocation, T model) where T : BaseViewModel
		{
			// Naplním podlaží.
			model.Floors = await this.floorService.GetFloors();
			model.SelectedFloor = await this.floorService.GetLastSelectedFloor();

			// Naplním uživatele
			model.CurrentUser = await this.userService.GetCurrentUser();

			// Naplním, kde se uživatel na webu nachází.
			model.WebLocation = webLocation;

			return model;
		}

		/// <summary>
		/// Vrací konkrétní zobrazení pro aplikaci.
		/// </summary>
		protected async Task<IActionResult> GetMainView(WebLocation webLocation, string viewName = "Index")
		{
			var model = await FetchModel(webLocation, new MainViewModel());

			// Pokud nemám ani jedno podlaží, apka není inicializovaná.
			if (model.Floors.Length == 0)
				return View("../Home/NotInitialized");

			// Inicializace modelu.
			await model.Init(syncService);

			// Vracím požadované view.
			return View(viewName, model);
		}
	}
}