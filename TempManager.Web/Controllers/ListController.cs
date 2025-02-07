using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Web.Code;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro poskytnutí dat pro zobrazení typu list.
	/// </summary>
	public class ListController : BaseController
	{
		protected readonly IRoomService roomService;
		protected readonly CookieManager cookieManager;

		/// <inheritdoc />
		public ListController(IRoomService roomService, CookieManager cookieManager, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(floorService, userService, syncService)
		{
			this.roomService = roomService;
			this.cookieManager = cookieManager;
		}

		/// <summary>
		/// Vrací seznam všech dostupných místností.
		/// </summary>
		/// <returns></returns>
		[Route("/List")]
		public async Task<IActionResult> Index()
		{
			this.cookieManager.Save(CookieManager.FloorViewTypeCookieName, FloorViewType.List);

			// A vrátím konkrétní view.
			return await GetMainView(WebLocation.List);
		}

		/// <summary>
		/// Vrací všechny místnosti na daném podlaží.
		/// </summary>
		public async Task<IActionResult> GetRooms()
		{
			// Stáhnu a vrátím všechny místnosti, na které má uživatel právo.
			var rooms = await this.roomService.GetRooms();
			return Json(rooms);
		}
	}
}