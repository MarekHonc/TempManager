using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Code;
using TempManager.Web.Areas.Admin.Models;
using TempManager.Web.Controllers;

namespace TempManager.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Bázový kontroler pro admin akce.
	/// </summary>
	[Area("Admin")]
	[AdminShibbolethAuthorize]
	public class AdminBaseController : BaseController
	{
		protected readonly RepositoriesFactory repositoriesFactory;

		public AdminBaseController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(floorService, userService, syncService)
		{
			this.repositoriesFactory = repositoriesFactory;
		}

		/// <summary>
		/// Naplní model základními hodnotami pro zobrazení aplikace.
		/// </summary>
		protected async Task<T> FetchModel<T>(AdminWebLocation webLocation, T model) where T : AdminBaseViewModel
		{
			model = await base.FetchModel(WebLocation.Admin, model);
			model.AdminWebLocation = webLocation;

			return model;
		}
	}
}