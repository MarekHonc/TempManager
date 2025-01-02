namespace TempManager.BL.Interfaces
{
	/// <summary>
	/// Interface reprezentující model vytvoření z claimů.
	/// </summary>
	public interface IClaimsUser
	{
		/// <summary>
		/// Vrací název aktuálního uživatele.
		/// </summary>
		public string UserName
		{
			get;
		}

		/// <summary>
		/// Vrací křestní jméno uživatele.
		/// </summary>
		public string FirstName
		{
			get;
		}

		/// <summary>
		/// Vrací příjmení uživatele.
		/// </summary>
		public string LastName
		{
			get;
		}

		/// <summary>
		/// Vrací unikátní identifikátor uživatele.
		/// </summary>
		public string Uid
		{
			get;
		}

		/// <summary>
		/// Vrací affiliace aktuálně přihlášeného uživatele.
		/// </summary>
		public string[] Affiliations
		{
			get;
		}
	}
}