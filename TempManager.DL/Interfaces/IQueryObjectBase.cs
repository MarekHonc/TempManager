namespace TempManager.DL.Interfaces
{
	/// <summary>
	/// Rozhraní definující dotazovací objekt.
	/// </summary>
	/// <typeparam name="T">Entity objekt, který je výsledkem dotazu.</typeparam>
	public interface IQueryObjectBase<T> where T : class, IEntity
	{
		/// <summary>
		/// Příznak, zda-li má EF trackovat změny na stažených objektech.
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// Vazby, které se při vyhodnocování query stáhnou také.
		/// </summary>
		List<string> Includes { get; }

		/// <summary>
		/// Vrací počet řádků odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>Počet řádků odpovídajících dotazu.</returns>
		Task<int> Count(TempManagerContext dbContext);

		/// <summary>
		/// Vrací, zda existuje nějaký záznam odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns><c>true</c> pokud existuje nějaký záznam, jinak <c>false</c>.</returns>
		Task<bool> IsAny(TempManagerContext dbContext);

		/// <summary>
		/// Vrací záznamy odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>Záznamy odpovídající dotazu.</returns>
		Task<IReadOnlyCollection<T>> Fetch(TempManagerContext dbContext);

		/// <summary>
		/// Vrací první záznam odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>
		/// Záznam odpovídající dotazu, nebo null pokud dotazu neodpovídá žádný záznam.
		/// </returns>
		Task<T?> FetchOne(TempManagerContext dbContext);
	}
}
