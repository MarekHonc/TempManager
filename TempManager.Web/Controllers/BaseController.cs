using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.Web.Models;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Bázový kontroler pro aplikaci.
	/// </summary>
	public class BaseController : Controller
	{
		protected IFloorService floorService;
		protected IUserService userService;

		public BaseController(IFloorService floorService, IUserService userService)
		{
			this.floorService = floorService;
			this.userService = userService;
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
	}
}
