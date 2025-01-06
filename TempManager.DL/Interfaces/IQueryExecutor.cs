namespace TempManager.DL.Interfaces
{
	/// <summary>
	/// Rozhraní pro spouštění dotazů nad databází.
	/// </summary>
	/// <typeparam name="T">Typ entity, nad kterou se budou spouštět dotazy.</typeparam>
	public interface IQueryExecutor<T> where T : class, IEntity
	{
		/// <summary>
		/// Vrací počet záznamů, které odpovídají dotazu.
		/// </summary>
		/// <param name="query">
		/// Dotaz, pro který se bude zjišťovat počet odpovídajících záznamů.
		/// </param>
		/// <returns>Počet záznamů odpovídajících dotazu.</returns>
		Task<int> Count(IQueryObjectBase<T> query);

		/// <summary>
		/// Vrací, zda existuje nějaký záznam odpovídající dotazu.
		/// </summary>
		/// <param name="query">Dotaz, pro získání záznamů.</param>
		/// <returns>Vrací <c>true</c> pokud záznam existuje, jinak vrací <c>false</c>.</returns>
		Task<bool> IsAny(IQueryObjectBase<T> query);

		/// <summary>
		/// Získává všechny záznamu, které vyhovují dotazu.
		/// </summary>
		/// <param name="query">Dotaz, pro získání záznamů.</param>
		/// <returns>Záznamy odpovídající dotazu.</returns>
		Task<IReadOnlyCollection<T>> Fetch(IQueryObjectBase<T> query);

		/// <summary>
		/// Vrací první záznam odpovídající dotazu.
		/// </summary>
		/// <param name="query">Dotaz, pro získání záznamu.</param>
		/// <returns>První dotaz, který odpovídá dotazu, nebo null.</returns>
		Task<T> FetchOne(IQueryObjectBase<T> query);
	}
}