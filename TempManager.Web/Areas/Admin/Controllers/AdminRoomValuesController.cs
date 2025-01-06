using Microsoft.AspNetCore.Mvc;

namespace TempManager.Web.Areas.Admin.Controllers
{
	public class AdminRoomValuesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}