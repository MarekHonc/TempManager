using TempManager.Common.Enums;

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
		/// Inicializuje model.
		/// </summary>
		public void Init()
		{
			// TODO reálná kontrola.
			this.IsOnline = true;

			// TODO: reálné poslední zobrazení.
			this.FloorViewType = FloorViewType.List;
		}
	}
}
