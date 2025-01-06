using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kontext pro udržení chybového hlášení z Shibbolethu.
	/// </summary>
	public class ShibbolethFailureContext : HandleRequestContext<ShibbolethOptions>
	{
		public ShibbolethFailureContext(HttpContext context, AuthenticationScheme scheme, ShibbolethOptions options, Exception failure)
			: base(context, scheme, options)
		{
			Failure = failure;
		}

		/// <summary>
		/// Vrací nebo nastavuje výjimku, která nastala.
		/// </summary>
		public Exception Failure
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo natavuje stavové proměnné z přihlašovací sesion.
		/// </summary>
		public AuthenticationProperties Properties
		{
			get;
			set;
		}
	}
}