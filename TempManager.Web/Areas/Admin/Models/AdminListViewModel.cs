namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Základní model pro zobrazení listu entit.
	/// </summary>
	public class AdminListViewModel<T> : AdminBaseViewModel where T : class
	{
		public AdminListViewModel(IReadOnlyCollection<T> items)
		{
			this.Items = items;
		}

		/// <summary>
		/// Vrací položky pro zobrazení.
		/// </summary>
		public IReadOnlyCollection<T> Items
		{
			get;
		}
	}
}