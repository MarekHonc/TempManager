using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kontext držený po obdržení ticketu.
	/// </summary>
	public class ShibbolethTicketReceivedContext : ShibbolethAuthenticationContext
	{
		public ShibbolethTicketReceivedContext(HttpContext context, AuthenticationScheme scheme, ShibbolethOptions options, AuthenticationTicket ticket)
			: base(context, scheme, options, ticket?.Properties)
		{
			this.Principal = ticket?.Principal;
		}

		/// <summary>
		/// Vrací nebo nastavuje adresu na kterou bude přesměrováno po přihlášení.
		/// </summary>
		public string? ReturnUri
		{
			get;
			set;
		}
	}
}