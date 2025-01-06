using Microsoft.AspNetCore.Mvc;

namespace TempManager.Web.Areas.Admin.Controllers
{
	public class AdminUserRightsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}