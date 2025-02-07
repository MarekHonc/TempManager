using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.Common;
using TempManager.Common.Extensions;
using TempManager.DL.Entities;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Models;

namespace TempManager.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Kontroler pro zobrazení posbírané zpětné vazby.
	/// </summary>
	public class AdminFeedbackController : AdminBaseController
	{
		private readonly int pageSize = 20;

		public AdminFeedbackController(RepositoriesFactory repositoriesFactory, IFloorService floorService, IUserService userService, IValueSyncService syncService)
			: base(repositoriesFactory, floorService, userService, syncService)
		{
		}

		/// <summary>
		/// Vrací hlavní stránku s přehledem zpětné vazby.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var model = await FetchModel(
				AdminWebLocation.Feedback,
				new AdminBaseViewModel()
			);

			return View(model);
		}

		/// <summary>
		/// Vrací konkrétní stránku zpětné vazby.
		/// </summary>
		public async Task<IActionResult> List(int page)
		{
			var feedbacks = await GetList(page);

			return Json(new
			{
				items = feedbacks.Select(f => new
				{
					date = f.RatedAt.ToShortDateTime(),
					userName = f.User.UserName,
					rating = f.Rating,
					note = f.Note,
				}),
				hasNext = feedbacks.HasNextPage,
				page = feedbacks.PageNumber
			});
		}

		/// <summary>
		/// Vrací požadovanou stránku zpětné vazby.
		/// </summary>
		private async Task<IPagedList<Feedback>> GetList(int page = 1)
		{
			var query = new FeedbackQuery().Include(nameof(DL.Entities.User));
			var result = await this.repositoriesFactory.FeedbackRepository.FetchPage(query, page, pageSize);

			return result;
		}
	}
}