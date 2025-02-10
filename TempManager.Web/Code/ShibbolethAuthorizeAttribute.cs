using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TempManager.BL.Interfaces;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Atribut který se stará o ověření práv vůči hodnotám ze shibboleth.
	/// </summary>
	public class ShibbolethAuthorizeAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Při vykonávání akce kontroluji hodnoty přihlášeného uživatele.
		/// </summary>
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			var claimsUser = actionContext.HttpContext.RequestServices.GetService<IClaimsUser>();
			var settings = actionContext.HttpContext.RequestServices.GetService<ShibbolethAuthorizeSettings>();

			// Aktuální uživatel je null -> neřeším.
			if (claimsUser == null)
			{
				actionContext.Result = new RedirectResult("NotAuthorized");
				return;
			}

			// Tady vím, že uživatel není null, tak kontroluji povolené emailové adresy a affiliace.
			var authorized = settings.Mails.Contains(claimsUser.UserName) ||
							 settings.Affiliations.Any(a => claimsUser.Affiliations.Contains(a));

			// Není autorizováno.
			if (!authorized)
				actionContext.Result = new RedirectResult("NotAuthorized");
		}
	}
}