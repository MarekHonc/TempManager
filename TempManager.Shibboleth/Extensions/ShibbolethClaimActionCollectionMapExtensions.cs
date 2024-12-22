using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Třída s extension metodami pro <see cref="ShibbolethClaimActionCollection"/>.
	/// </summary>
	/// <remarks>
	/// Inspirace zde:
	/// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OAuth/src/ClaimActionCollectionMapExtensions.cs
	/// </remarks>
	internal static class ShibbolethClaimActionCollectionMapExtensions
	{
		/// <summary>
		/// Vezme hodnotu atributu a vytvoří z ní claim.
		/// </summary>
		public static void MapAttribute(this ShibbolethClaimActionCollection collection, string claimType, string attributeName)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			collection.Add(new ShibbolethAttributeClaimAction(claimType, ClaimValueTypes.String, attributeName));
		}

		/// <summary>
		/// Vezme hodnotu atributu a vytvoří z ní claim.
		/// </summary>
		public static void MapCustomAttribute(this ShibbolethClaimActionCollection collection, string claimType, string attributeName, Func<string, string> processor)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			collection.Add(new ShibbolethCustomClaimAction(claimType, ClaimValueTypes.String, attributeName, processor));
		}

		/// <summary>
		/// Vezme hodnotu atributu a vytvoří z ní claim, který může uchovávat více hodnot.
		/// </summary>
		public static void MapCustomMultiValueAttribute(this ShibbolethClaimActionCollection collection, string claimType, string attributeName, Func<string, IEnumerable<string>> processor)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			
			collection.Add(new ShibbolethCustomMultiValueClaimAction(claimType, ClaimValueTypes.String, attributeName, processor));
		}
	}
}