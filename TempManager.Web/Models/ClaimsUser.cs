using System.Security.Claims;
using TempManager.BL.Interfaces;
using TempManager.Shibboleth;

namespace TempManager.Web.Models
{
	/// <summary>
	/// Aktuálně přihlášený uživatel, který byl rozpoznán z claimů.
	/// </summary>
	public class ClaimsUser : IClaimsUser
	{
		public ClaimsUser(IHttpContextAccessor httpContextAccessor)
		{
			var claims = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;

			this.Uid = claims.FindFirst(ShibbolethClaimsType.UID).Value;
			this.UserName = claims.FindFirst(ShibbolethClaimsType.EMAIL).Value;
			this.FirstName = claims.FindFirst(ShibbolethClaimsType.FIRSTNAME).Value;
			this.LastName = claims.FindFirst(ShibbolethClaimsType.LASTNAME).Value;
			// TODO: Todle je array
			this.Affiliations = new string[]
			{
				claims.FindFirst(ShibbolethClaimsType.AFFILIATION).Value,
			};
		}

		/// <inheritdoc cref="IClaimsUser.Uid"/>
		public string Uid
		{
			get;
		}

		/// <inheritdoc cref="IClaimsUser.UserName"/>
		public string UserName
		{
			get;
		}

		/// <inheritdoc cref="IClaimsUser.FirstName"/>
		public string FirstName
		{
			get;
		}

		/// <inheritdoc cref="IClaimsUser.LastName"/>
		public string LastName
		{
			get;
		}

		/// <inheritdoc cref="IClaimsUser.Affiliations" />
		public string[] Affiliations
		{
			get;
		}
	}
}