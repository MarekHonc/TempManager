using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Bázový objekt který se využívá pro mapování claimů v <see cref="ShibbolethAttributeValueCollection"/> to claims on the ClaimsIdentity.
	/// </summary>
	/// <remarks>
	/// Inspirace:
	/// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OAuth/src/ClaimAction.cs
	/// </remarks>
	public abstract class ShibbolethClaimAction
	{
		/// <summary>
		/// Vytvoří novou akci pro mapování claimu.
		/// </summary>
		public ShibbolethClaimAction(string claimType, string valueType)
		{
			ClaimType = claimType;
			ValueType = valueType;
		}

		/// <summary>
		/// Vrací typ claimu.
		/// </summary>
		public string ClaimType
		{
			get;
		}

		/// <summary>
		/// Vrací typ hodnoty claimu,
		/// </summary>
		public string ValueType
		{
			get;
		}

		/// <summary>
		/// Zkontroluje, zda-li se <see cref="ShibbolethAttributeValueCollection"/>, nachází hodnota, případně ji přidá.
		/// </summary>
		public abstract void Run(ShibbolethAttributeValueCollection userData, ClaimsIdentity identity, string issuer);

		/// <summary>
		/// Vrací hodnotu z atributů na základě názvu.
		/// </summary>
		protected static string GetValue(ShibbolethAttributeValueCollection userData, string attributeName)
		{
			if (!userData.ContainsAttribute(attributeName))
				return null;

			if (!userData.ValueIsNullOrEmpty(attributeName))
				return userData[attributeName].Value.ToString();

			return null;
		}
	}
}