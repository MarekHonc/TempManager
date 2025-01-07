using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;
using TempManager.Common;
using TempManager.Web.Models;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro sběr zpětné vazby ohledně aplikace.
	/// </summary>
	public class FeedbackController : BaseController
	{
		private readonly IFeedbackService feedbackService;

		public FeedbackController(IFloorService floorService, IUserService userService, IFeedbackService feedbackService)
			: base(floorService, userService)
		{
			this.feedbackService = feedbackService;
		}

		/// <summary>
		/// Formulář pro zaslání zpětné vazby.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			var model = await FetchModel(WebLocation.FeedBackForm, new FeedbackViewModel());
			return View("CreateFeedback", model);
		}

		/// <summary>
		/// Odešle formulář se zpětnou vazbou.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> SubmitForm(FeedbackViewModel postedModel)
		{
			var model = await FetchModel(WebLocation.FeedBackForm, postedModel);

			if (!ModelState.IsValid)
				return View("CreateFeedback", model);

			await feedbackService.CreateFeedback(model.Rating, model.Feedback);

			return View("ThankYou", model);
		}
	}
}