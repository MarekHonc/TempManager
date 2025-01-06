using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Nastavení shibbolethu, stará se o mapování shibbo atributů do Claimů.
	/// </summary>
	public class ShibbolethOptions : AuthenticationSchemeOptions
	{
		public ShibbolethOptions()
		{
			Func<string, string> toLower = (s) => s.ToLower();
			Func<string, IEnumerable<string>> toArray = (s) => new[] { s.ToLower() };

			// Identifikátor názvu je uid z Shibba.
			ClaimActions.MapCustomAttribute(ClaimTypes.NameIdentifier, "uid", toLower);
			ClaimActions.MapAttribute(ShibbolethClaimsType.FIRSTNAME, "givenName");
			ClaimActions.MapAttribute(ShibbolethClaimsType.LASTNAME, "sn");
			ClaimActions.MapAttribute(ShibbolethClaimsType.EPPN, "eppn");
			ClaimActions.MapCustomAttribute(ShibbolethClaimsType.UID, "uid", toLower);
			ClaimActions.MapCustomAttribute(ShibbolethClaimsType.EMAIL, "mail", toLower);

			// TODO: nějak string rozparsovat a vytáhnout z toho seznam
			ClaimActions.MapCustomMultiValueAttribute(ShibbolethClaimsType.AFFILIATION, "eduPersonScopedAffiliation", toArray);
		}

		/// <summary>
		/// Vrací kolekci Shibboleth claimů, využivá se pro vytváření dat.
		/// </summary>
		public ShibbolethClaimActionCollection ClaimActions
		{
			get;
		} = new ShibbolethClaimActionCollection();

		/// <summary>
		/// Vrací nebo nastavuje zabezpečovací schéma, které je shodné s middlewarem.
		/// </summary>
		public string SignInScheme
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje formát, který zabezpečuje data zpracované middlewarem.
		/// </summary>
		public ISecureDataFormat<AuthenticationProperties> StateDataFormat
		{
			get;
			set;
		} = default!;

		/// <summary>
		/// Vrací nebo nastavuje provider, který zabezpečuje data.
		/// </summary>
		public IDataProtectionProvider DataProtectionProvider
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje <see cref="IShibbolethAttributeCollection"/> tobsahující všechny extraktovatelné hodnoty z Shibbolethu.
		/// </summary>
		public IShibbolethAttributeCollection ShibbolethAttributes { get; set; } = ShibbolethAttributeCollection.DefaultAttributes;

		#region Challenge

		/// <summary>
		/// Vrací nebo nastavuje url, na které proběhne přesměrování po dodatečném přihlášení.
		/// </summary>
		public PathString CallbackPath
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li se má používat challenge typ přihlášení.
		/// To je vhodné, pokud aplikace má stránku pro veřejnost.
		/// </summary>
		public bool UseChallenge
		{
			get;
			set;
		} = false;

		/// <summary>
		/// Vrací nebo nastavuje parametr, do kterého se uloží aktuálně zobrazená stránka.
		/// </summary>
		public string ReturnUrlParameter
		{
			get;
			set;
		} = "ReturnUrl";

		#endregion

		/// <summary>
		/// Zpracování eventů od Shibba.
		/// </summary>
		public new ShibbolethEvents Events
		{
			get
			{
				return (ShibbolethEvents)base.Events;
			}
			set
			{
				base.Events = value;
			}
		}

		/// <summary>
		/// Checks that the options are valid for a specific scheme
		/// </summary>
		/// <param name="scheme">The scheme being validated.</param>
		public override void Validate(string scheme)
		{
			base.Validate(scheme);
			if (this.UseChallenge && string.Equals(scheme, SignInScheme, StringComparison.Ordinal))
			{
				throw new InvalidOperationException("The SignInScheme for a Shibboleth authentication handler cannot be set to itself.  If it was not explicitly set, the AuthenticationOptions.DefaultSignInScheme or DefaultScheme is used.");
			}
		}
	}
}