using TempManager.Common;
using TempManager.Web.Code;

namespace TempManager.Web.Models
{
	/// <summary>
	/// Hlavní model pro zobrazení aplikace.
	/// </summary>
	public class MainViewModel : BaseViewModel
	{
		/// <summary>
		/// Vrací, zda-li aplikace aktivně přijímá data.
		/// </summary>
		public bool IsOnline
		{
			get;
			private set;
		}

		/// <summary>
		/// Vrací aktuální typ zobrazení.
		/// </summary>
		public FloorViewType FloorViewType
		{
			get;
			private set;
		}

		/// <summary>
		/// Vrací zda-li šablona existuje.
		/// </summary>
		public bool FloorViewExists => !string.IsNullOrEmpty(this.SelectedFloor.MapName);

		/// <summary>
		/// Inicializuje model.
		/// </summary>
		public void Init(CookieManager cookieManager)
		{
			// TODO: reálná kontrola.
			this.IsOnline = true;

			// Načtu správné zobrazení podle cookies.
			this.FloorViewType = cookieManager.Get(CookieManager.FloorViewTypeCookieName, FloorViewType.List);
		}
	}
}