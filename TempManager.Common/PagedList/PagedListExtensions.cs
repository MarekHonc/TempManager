namespace TempManager.Common
{
	/// <summary>
	/// Extension metody pro práci s paged listem.
	/// </summary>
	public static class PagedListExtensions
	{
		/// <summary>
		/// Vrací paged list vytvořený na základě konkrétního IQueryable objektu.
		/// </summary>
		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
		{
			return new PagedList<T>(source, pageNumber, pageSize);
		}
	}
}
