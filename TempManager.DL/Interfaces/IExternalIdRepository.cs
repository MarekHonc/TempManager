namespace TempManager.DL.Interfaces
{
	/// <summary>
	/// Repozitář, pro entity které obsahují externí id.
	/// </summary>
	public interface IExternalIdRepository<T> : IRepository<T> where T : class, IExternalId
	{
		/// <summary>
		/// Vrací entitu na základě jejího externího id.
		/// </summary>
		Task<T?> GetByExternalId(string externalId);

		/// <summary>
		/// Vrací slovník všech získaných entit, roztříděných podle externího id.
		/// </summary>
		Task<Dictionary<string, T>> GetExternalIdLookUp();
	}
}
