using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Mapování na standardní ASP net claimy.
	/// </summary>
	internal class StandardClaimTypes
	{
		/// <summary>
		/// Mapování skupiny/affiliace -> jde to na roli?
		/// TODO: zkontrolovat.
		/// </summary>
		public const string Group = ClaimTypes.Role;

		/// <summary>
		/// Mapování pro unikátní název principal.
		/// </summary>
		public const string UPN = ClaimTypes.Upn;

		/// <summary>
		/// Mapování pro unikátní název uživatele.
		/// </summary>
		public const string Name = ClaimTypes.Name;

		/// <summary>
		/// Mapování pro křestní jméno.
		/// </summary>
		public const string GivenName = ClaimTypes.GivenName;

		/// <summary>
		/// Mapování pro příjmení.
		/// </summary>
		public const string Surname = ClaimTypes.Surname;

		/// <summary>
		/// Mapování pro e.amil.
		/// </summary>
		public const string Email = ClaimTypes.Email;
	}
}