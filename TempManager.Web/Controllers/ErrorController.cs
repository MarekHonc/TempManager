using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Kontroler pro práci s errory.
	/// </summary>
	[AllowAnonymous]
	public class ErrorController : Controller
	{
		/// <summary>
		/// Uživatel není autorizovaný pro přístup.
		/// </summary>
		[Route("NotAuthorized")]
		public IActionResult NotAuthorized()
		{
			return View();
		}
	}
}