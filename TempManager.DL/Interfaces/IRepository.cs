namespace TempManager.DL.Interfaces
{
	/// <summary>
	/// Interface pro repozitáře.
	/// </summary>
	public interface IRepository<T> : IQueryExecutor<T> where T : class, IEntity
	{
		/// <summary>
		/// Vrátí záznam podle jeho id.
		/// </summary>
		Task<T?> FetchById(params object[] id);

		/// <summary>
		/// Vrací všechny záznamy z databáze.
		/// </summary>
		Task<IReadOnlyCollection<T>> FetchAll();

		/// <summary>
		/// Přidá záznam do databáze.
		/// </summary>
		Task Add(T entity);

		/// <summary>
		/// Přidá záznamy do databáze.
		/// </summary>
		Task AddRange(IEnumerator<T> entity);

		/// <summary>
		/// Smaže záznam z databáze.
		/// </summary>
		Task Remove(T entity);
	}
}
