using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Web.Code;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Hlavní controller aplikace.
	/// </summary>
	public class HomeController : BaseController
	{
		private readonly CookieManager cookieManager;

		public HomeController(IFloorService floorService, IUserService userService, CookieManager cookieManager, IValueSyncService syncService)
			: base(floorService, userService, syncService)
		{
			this.cookieManager = cookieManager;
		}

		/// <summary>
		/// Vrací úvodní stránku aplikace - s posledním zobrazeným podlažím.
		/// </summary>
		[Route("/")]
		public async Task<IActionResult> Index()
		{
			// Podle poslední zobrazené hodnoty natáhnu model.
			var lastValue = this.cookieManager.Get(CookieManager.FloorViewTypeCookieName, FloorViewType.List);
			IActionResult result = null;

			// Podle typu inicializuji model.
			switch (lastValue)
			{
				case FloorViewType.List:
					result = await GetMainView(WebLocation.List);
					break;
				case FloorViewType.Map:
					result = await GetMainView(WebLocation.Map);
					break;
				default:
					throw new Exception($"{lastValue} view type is not supported!");
			}

			return result;
		}
	}
}