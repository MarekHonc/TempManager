namespace TempManager.Shibboleth
{
	/// <summary>
	/// Typy claimů z Shibbolethu.
	/// Interně jsou namapovány claim z asp net.
	/// </summary>
	public class ShibbolethClaimsType
	{
		/// <summary>
		/// Příslušnost k affiliaci z Shibbo je mapována na skupinu.
		/// </summary>
		public const string AFFILIATION = StandardClaimTypes.Group;

		/// <summary>
		/// Mapování pro EPPS z shibboleth je mapováno na UPN.
		/// </summary>
		public const string EPPN = StandardClaimTypes.UPN;

		/// <summary>
		/// Unikání id z shibboleth je namapování na unikátní název z ASP net.
		/// </summary>
		public const string UID = StandardClaimTypes.Name;

		/// <summary>
		/// Mapování pro křestní jméno.
		/// </summary>
		public const string FIRSTNAME = StandardClaimTypes.GivenName;

		/// <summary>
		/// Mapování pro příjmení.
		/// </summary>
		public const string LASTNAME = StandardClaimTypes.Surname;

		/// <summary>
		/// Mapování pro e.amil.
		/// </summary>
		public const string EMAIL = StandardClaimTypes.Email;
	}
}