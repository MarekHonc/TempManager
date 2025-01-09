namespace TempManager.Common
{
	/// <summary>
	/// Rozhraní pro list, který je možné stránkovat.
	/// </summary>
	public interface IPagedList<T> : IList<T>
	{
		/// <summary>
		/// Vrací celkový počet stránek.
		/// </summary>
		int PageCount
		{
			get;
		}

		/// <summary>
		/// Vrací celkový počet položek.
		/// </summary>
		int TotalItemCount
		{
			get;
		}

		/// <summary>
		/// Vrací index aktuálně zobrazované stránky.
		/// </summary>
		int PageIndex
		{
			get;
		}

		/// <summary>
		/// Vrací číslo aktuálně zobrazené stránky.
		/// </summary>
		int PageNumber
		{
			get;
		}

		/// <summary>
		/// Vrací velikost stránky.
		/// </summary>
		int PageSize
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li existuje předchozí stránka.
		/// </summary>
		bool HasPreviousPage
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li existuje následující stránky.
		/// </summary>
		bool HasNextPage
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je aktuální stránka první.
		/// </summary>
		bool IsFirstPage
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je aktuální stránka poslední.
		/// </summary>
		bool IsLastPage
		{
			get;
		}

		/// <summary>
		/// Vrací nový <see cref="IPagedList{TNew}"/>, který je naplněn položkami nového typu.
		/// </summary>
		/// <param name="select">Select, pomocí kterého se provede konverze položek stávajícího typu do nového.</param>
		IPagedList<TNew> SelectPagedList<TNew>(Func<T, TNew> select);
	}
}
