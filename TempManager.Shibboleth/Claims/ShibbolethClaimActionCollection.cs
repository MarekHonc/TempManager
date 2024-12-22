using System.Collections;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kolekce shibboleth atributů, používaná pro mapování do ASP NET.
	/// </summary>
	/// <remarks>Inspirace zde:
	/// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OAuth/src/ClaimActionCollection.cs
	/// </remarks>
	public class ShibbolethClaimActionCollection : IEnumerable<ShibbolethClaimAction>
	{
		/// <summary>
		/// Vrací seznam všech akcí pro mapování.
		/// </summary>
		private IList<ShibbolethClaimAction> Actions
		{
			get;
		} = new List<ShibbolethClaimAction>();

		/// <summary>
		/// Přidá mapovací akci.
		/// </summary>
		public void Add(ShibbolethClaimAction action)
		{
			this.Actions.Add(action);
		}

		/// <inheritdoc cref="GetEnumerator"/>
		public IEnumerator<ShibbolethClaimAction> GetEnumerator()
		{
			return this.Actions.GetEnumerator();
		}

		/// <inheritdoc cref="IEnumerable.GetEnumerator"/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}