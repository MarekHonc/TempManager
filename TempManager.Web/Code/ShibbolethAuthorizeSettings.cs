namespace TempManager.Web.Code
{
	/// <summary>
	/// Třída nesoucí nastavení autorizace - oprávnění, tj. toho kdo má přístup.
	/// </summary>
	public class ShibbolethAuthorizeSettings
	{
		public ShibbolethAuthorizeSettings(IConfiguration configuration)
		{
			this.Mails = configuration.GetSection("AccessControl:Emails").Get<string[]>();
			this.Affiliations = configuration.GetSection("AccessControl:Affiliations").Get<string[]>();
		}

		/// <summary>
		/// Vrací povolené e-mailové adresy.
		/// </summary>
		public string[] Mails
		{
			get;
		}

		/// <summary>
		/// Vrací povolené affiliace.
		/// </summary>
		public string[] Affiliations
		{
			get;
		}
	}
}