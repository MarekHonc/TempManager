using TempManager.Common;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Základní model pro zobrazení stránkovaného listu entit.
	/// </summary>
	public class AdminPagedListViewModel<T> : AdminBaseViewModel where T : class
	{
		public AdminPagedListViewModel(IPagedList<T> items)
		{
			this.Items = items;
		}

		/// <summary>
		/// Vrací položky pro zobrazení.
		/// </summary>
		public IPagedList<T> Items
		{
			get;
		}
	}
}