using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
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
		public bool FloorViewExists
		{
			get;
			private set;
		}

		/// <summary>
		/// Inicializuje model.
		/// </summary>
		public void Init(CookieManager cookieManager, ICompositeViewEngine viewEngine)
		{
			// TODO: reálná kontrola.
			this.IsOnline = true;

			// Načtu správné zobrazení podle cookies.
			this.FloorViewType = cookieManager.Get(CookieManager.FloorViewTypeCookieName, FloorViewType.List);

			// Kontrola, jestli view existuje.
			// Mapa je prázdná, nemusím kontrolovat.
			if (string.IsNullOrEmpty(this.SelectedFloor.MapViewName))
			{
				this.FloorViewExists = false;
			}
			else
			{
				// Zkusím najít view.
				var viewResult = viewEngine.GetView(null,  $"~/Views/Floor/{this.SelectedFloor.MapViewName}.cshtml", true);
				this.FloorViewExists = viewResult.Success;
			}
		}
	}
}