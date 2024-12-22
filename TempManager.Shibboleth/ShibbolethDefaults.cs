using Microsoft.AspNetCore.Authentication;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Hodnoty pro napojení autorizace pomocí shibboleth.
	/// </summary>
	public class ShibbolethDefaults
	{
		/// <summary>
		/// Výchozí název pro autentizační schéma <see cref="AuthenticationScheme.Name"/>.
		/// </summary>
		public const string AuthenticationScheme = "Shibboleth";

		/// <summary>
		/// Hodnota identifikující že Shibbo session je přítomná.
		/// </summary>
		/// <remarks>Pro Linux (apache) nebo IIS v "hlavičkovém" módu.</remarks>
		public const string HeaderShibIndexName = "ShibSessionIndex";

		/// <summary>
		/// Hodnota identifikující že Shibbo session je přítomná.
		/// </summary>
		/// <remarks>Pro IIS v módu "proměnných" - výchozí nastavení.</remarks>
		public const string VariableShibIndexName = "Shib-Session-ID";
	}
}