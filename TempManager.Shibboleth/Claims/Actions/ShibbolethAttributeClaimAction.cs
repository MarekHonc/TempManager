using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Mapování dat z atributù z <see cref="ShibbolethAttributeValueCollection"/> na claimy z ClaimsIdentity.
	/// </summary>
	/// <remarks>
	/// Inspirace zde:
	/// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OAuth/src/ClaimAction.cs
	/// </remarks>
	public class ShibbolethAttributeClaimAction : ShibbolethClaimAction
	{
		/// <summary>
		/// Vytvoøí novou mapovací akci.
		/// </summary>
		public ShibbolethAttributeClaimAction(string claimType, string valueType, string attributeName)
			: base(claimType, valueType)
		{
			this.AttributeName = attributeName;
		}

		/// <summary>
		/// Vrací název atributu.
		/// </summary>
		public string AttributeName
		{
			get;
		}

		/// <inheritdoc cref="Run"/>
		public override void Run(ShibbolethAttributeValueCollection userData, ClaimsIdentity identity, string issuer)
		{
			string? value = GetValue(userData, this.AttributeName);

			if (!string.IsNullOrEmpty(value))
				identity.AddClaim(new Claim(ClaimType, value, ValueType, issuer));
		}
	}
}