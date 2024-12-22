using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Základní kontext pro ověření skrze Shibboleth.
	/// </summary>
	public abstract class ShibbolethAuthenticationContext : HandleRequestContext<ShibbolethOptions>
	{
		protected ShibbolethAuthenticationContext(HttpContext context, AuthenticationScheme scheme, ShibbolethOptions options, AuthenticationProperties? properties)
			: base(context, scheme, options)
		{
			this.Properties = properties ?? new AuthenticationProperties();
		}

		/// <summary>
		/// Vrací nebo nastavuje <see cref="ClaimsPrincipal"/> obsahující claimy od uživatele.
		/// </summary>
		public ClaimsPrincipal? Principal
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje <see cref="AuthenticationProperties"/> aktuální uživatele.
		/// </summary>
		public virtual AuthenticationProperties? Properties
		{
			get;
			set;
		}
	}
}