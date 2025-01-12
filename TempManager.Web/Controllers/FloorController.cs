using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro práci s místnostmi.
	/// </summary>
	public class FloorController : BaseController
	{
		private readonly IRoomService roomService;

		public FloorController(IFloorService floorService, IUserService userService, IRoomService roomService)
			: base(floorService, userService)
		{
			this.roomService = roomService;
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
			// TODO: Odebrat hodnotu CO2 - odebráno + možná i další?
			var rooms = await this.roomService.GetRooms(floor.Id);
			return Json(rooms);
		}

		/// <summary>
		/// Uloží místnost do oblíbených.
		/// </summary>
		[HttpPost]
		[Route("Rooms/SaveToFavorite")]
		public async Task<IActionResult> SaveToFavorite(int roomId, bool isFavorite)
		{
			var result = await this.roomService.SetFavorite(roomId, isFavorite);
			return Json(result);
		}

		/// <summary>
		/// Nastaví novou požadovanou teplotu dané místnosti.
		/// </summary>
		[HttpPost]
		[Route("Rooms/SetTemperature")]
		public async Task<IActionResult> SetTemperature(int roomId, double desiredTemperature)
		{
			var result = await this.roomService.SetTemperature(roomId, desiredTemperature);
			return Json(result);
		}
	}
}