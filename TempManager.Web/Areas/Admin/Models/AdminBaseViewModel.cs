using TempManager.Common;
using TempManager.Web.Models;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Základní model admin rozhraní.
	/// </summary>
	public class AdminBaseViewModel : BaseViewModel
	{
		/// <summary>
		/// Vrací nebo nastavuje záložku v adminu, kde se uživatel nachází.
		/// </summary>
		public AdminWebLocation AdminWebLocation
		{
			get;
			set;
		}
	}
}