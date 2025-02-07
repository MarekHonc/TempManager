using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Web.Code;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro poskytování dat pro zobrazení typu mapa.
	/// </summary>
	public class MapController : BaseController
	{
		protected readonly IRoomService roomService;
		protected readonly CookieManager cookieManager;

		/// <inheritdoc />
		public MapController(IRoomService roomService, CookieManager cookieManager, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(floorService, userService, syncService)
		{
			this.roomService = roomService;
			this.cookieManager = cookieManager;
		}

		/// <summary>
		/// Vrací stránku pro zobrazení konkrétního podlaží.
		/// </summary>
		[Route("/Map/{friendlyId}")]
		public async Task<IActionResult> Index(string friendlyId)
		{
			this.cookieManager.Save(CookieManager.FloorViewTypeCookieName, FloorViewType.Map);

			// Vytáhnu podlaží.
			var floor = await this.floorService.GetByFriendlyId(friendlyId);
			if (floor == null)
				return NotFound($"Floor {friendlyId} does not exist!");

			// Uložím jako poslední zobrazené.
			await this.floorService.SaveLastSelectedFloor(floor.Id);

			// A vrátím konkrétní view.
			return await GetMainView(WebLocation.Map);
		}

		/// <summary>
		/// Vrací všechny místnosti na daném podlaží.
		/// </summary>
		[Route("/Rooms/{friendlyId}")]
		public async Task<IActionResult> GetRooms(string friendlyId)
		{
			// Kontrola existence podlaží.
			var floor = await this.floorService.GetByFriendlyId(friendlyId);
			if (floor == null)
				return NotFound();

			// Stáhnu a vrátím podlaží.
			var rooms = await this.roomService.GetRooms(floor.Id);
			return Json(rooms);
		}
	}
}