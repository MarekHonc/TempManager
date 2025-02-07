using TempManager.BL.SyncService;

namespace TempManager.Web.Models
{
	/// <summary>
	/// Hlavní model pro zobrazení aplikace.
	/// </summary>
	public class MainViewModel : BaseViewModel
	{
		private const int isOnlineThreshHoldMinutes = 10;

		/// <summary>
		/// Vrací, zda-li aplikace aktivně přijímá data.
		/// </summary>
		public bool IsOnline
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
		public async Task Init(IValueSyncService syncService)
		{
			var latestSync = await syncService.GetLatestSyncTime();

			// Online jsem pokud je spolední synchronizace před měně než 10 minutami.
			this.IsOnline = latestSync >= DateTimeOffset.UtcNow.AddMinutes(isOnlineThreshHoldMinutes);
		}
	}
}