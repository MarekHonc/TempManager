using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TempManager.BL.Services;
using TempManager.Shibboleth;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Interní kontroler pro testovací akce.
	/// </summary>
	public class InternalController : BaseController
	{
		public InternalController(IFloorService floorService, IUserService userService) : base(floorService, userService)
		{
		}

		public IActionResult TestShibboleth()
		{
			var ident = (ClaimsIdentity)HttpContext.User.Identity;

			var values = new string[]
			{
				"givenName: " + ident.FindFirst(ShibbolethClaimsType.FIRSTNAME).Value,
				"sn: " + ident.FindFirst(ShibbolethClaimsType.LASTNAME).Value,
				"eppn: " + ident.FindFirst(ShibbolethClaimsType.EPPN)?.Value,
				"uid: " + ident.FindFirst(ShibbolethClaimsType.UID).Value,
				"mail: " + ident.FindFirst(ShibbolethClaimsType.EMAIL).Value,
				"eduPersonScopedAffiliation: " + ident.FindFirst(ShibbolethClaimsType.AFFILIATION).Value,
			};

			return Content(string.Join("<br>", values), "text/html");
		}
	}
}
