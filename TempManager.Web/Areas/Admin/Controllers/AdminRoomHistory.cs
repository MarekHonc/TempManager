using Microsoft.AspNetCore.Mvc;

namespace TempManager.Web.Areas.Admin.Controllers
{
	public class AdminRoomHistory : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}