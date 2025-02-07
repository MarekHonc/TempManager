using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro práci s místnostmi.
	/// </summary>
	public class RoomController : BaseController
	{
		protected readonly IRoomService roomService;

		/// <inheritdoc />
		public RoomController(IRoomService roomService, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(floorService, userService, syncService)
		{
			this.roomService = roomService;
		}

		/// <summary>
		/// Uloží místnost do oblíbených.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> SaveToFavorite(int roomId, bool isFavorite)
		{
			var result = await this.roomService.SetFavorite(roomId, isFavorite);
			return Json(result);
		}

		/// <summary>
		/// Nastaví novou požadovanou teplotu dané místnosti.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> SetTemperature(int roomId, double desiredTemperature)
		{
			var result = await this.roomService.SetTemperature(roomId, desiredTemperature);
			return Json(result);
		}
	}
}
