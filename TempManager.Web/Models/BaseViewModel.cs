using TempManager.BL.Models;
using TempManager.Common.Enums;

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
	}
}
