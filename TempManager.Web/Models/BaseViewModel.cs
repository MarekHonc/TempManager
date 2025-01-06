using TempManager.BL.Models;
using TempManager.Common;

namespace TempManager.Web.Models
{
	/// <summary>
	/// Bázový model pro všechny stránky.
	/// </summary>
	public class BaseViewModel
	{
		/// <summary>
		/// Vrací aktuálně vybrané podlaží.
		/// </summary>
		public WebLocation WebLocation
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací aktuálně vybrané patro, pokud nějaké je.
		/// </summary>
		public Floor SelectedFloor
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací všechny definované podlaží.
		/// </summary>
		public Floor[] Floors
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací aktuálně přihlášeného užiatele.
		/// </summary>
		public User CurrentUser
		{
			get;
			set;
		}
	}
}