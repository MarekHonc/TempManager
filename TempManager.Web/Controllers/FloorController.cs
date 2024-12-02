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

		public FloorController(IFloorService floorService, IRoomService roomService) : base(floorService)
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
			var rooms = await this.roomService.GetRooms(floor.Id);
			return Json(rooms);
		}

		/// <summary>
		/// Uloží místnost do oblíbených.
		/// </summary>
		[Route("Rooms/SaveToFavorite")]
		public async Task<IActionResult> SaveToFavorite(int roomId, bool isFavorite)
		{
			var result = await this.roomService.SetFavorite(roomId, isFavorite);
			return Json(result);
		}
	}
}
