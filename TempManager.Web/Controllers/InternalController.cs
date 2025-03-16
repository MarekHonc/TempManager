using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TempManager.DL.Repositories;
using TempManager.Shibboleth;

namespace TempManager.Web.Controllers
{
	/// <summary>
	/// Interní kontroler pro testovací akce.
	/// </summary>
	public class InternalController : Controller
	{
		private readonly RepositoriesFactory repositoriesFactory;

		public InternalController(RepositoriesFactory repositoriesFactory)
		{
			this.repositoriesFactory = repositoriesFactory;
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
				"affiliation: " + ident.FindFirst(ShibbolethClaimsType.AFFILIATION).Value.Split(";"),
			};

			return Content(string.Join("<br>", values), "text/html");
		}

		public IActionResult TestHeaders()
		{
			var list = new List<string>();

			foreach (var header in Request.Headers)
			{
				list.Add($"{header.Key};{string.Join(",", header.Value.Select(v => v))}");
			}

			return Content(string.Join("<br>", list), "text/html");
		}

		public async Task<IActionResult> DetectGroups()
		{
			var rooms = await this.repositoriesFactory.RoomRepository.FetchAll();
			foreach (var room in rooms)
			{
				room.DetectGroups();
			}

			await this.repositoriesFactory.SaveChanges();
			return Content("OK");
		}
	}
}