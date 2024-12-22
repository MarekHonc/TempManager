using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Obsahuje informace o přihlášení v session a uživateli <see cref="ClaimsIdentity"/>.
	/// </summary>
	public class ShibbolethCreatingTicketContext : ResultContext<ShibbolethOptions>
	{
		public ShibbolethCreatingTicketContext(HttpContext context, AuthenticationScheme scheme, ShibbolethOptions options, ClaimsPrincipal principal, AuthenticationProperties properties, ShibbolethAttributeValueCollection userData)
			: base(context, scheme, options)
		{
			this.Principal = principal;
			this.Properties = properties;
			this.UserData = userData;
		}

		/// <summary>
		/// Vrací kolekci atributů z Shibbolethu, které se nacházejí v Session.
		/// </summary>
		public ShibbolethAttributeValueCollection UserData
		{
			get;
		}
	}
}