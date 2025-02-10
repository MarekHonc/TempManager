using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TempManager.BL.Services;

namespace TempManager.Web.Areas.Admin.Code
{
	/// <summary>
	/// Atribut, který umožní přístup na danou stránku pouze admin uživatelům.
	/// </summary>
	public class AdminShibbolethAuthorizeAttribute : ActionFilterAttribute, IDashboardAuthorizationFilter
	{
		/// <inheritdoc />
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			if (!IsUserAdmin(actionContext.HttpContext))
				actionContext.Result = new RedirectResult("/");
		}

		/// <inheritdoc />
		public bool Authorize(DashboardContext context)
		{
			return IsUserAdmin(context.GetHttpContext());
		}

		/// <summary>
		/// Vrací zda-li je aktuální uživatel admin.
		/// </summary>
		private static bool IsUserAdmin(HttpContext httpContext)
		{
			var userService = httpContext.RequestServices.GetService<IUserService>();
			var currentUser = userService.GetCurrentUser().Result;

			return currentUser?.IsAdmin == true;
		}
	}
}