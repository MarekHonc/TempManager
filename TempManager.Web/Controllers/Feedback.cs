using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Services;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro sběr zpětné vazby ohledně aplikace.
	/// </summary>
	public class Feedback : BaseController
	{
		public Feedback(IFloorService floorService, IUserService userService)
			: base(floorService, userService)
		{
		}

		/// <summary>
		/// Formulář pro zaslání zpětné vazby.
		/// </summary>
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Odešle formulář se zpětnou vazbou.
		/// </summary>
		[HttpPost]
		public IActionResult SubmitForm()
		{
			return View();
		}
	}
}