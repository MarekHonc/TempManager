namespace TempManager.Common
{
	/// <summary>
	/// Výsledek nastavení teploty.
	/// </summary>
	public enum SetTemperatureResult
	{
		/// <summary>
		/// Čeká na zpracování.
		/// </summary>
		Pending = 0,

		/// <summary>
		/// Nastavení proběhlo úspěšně.
		/// </summary>
		Success = 1,

		/// <summary>
		/// Teplotu se nepovedlo změnit, jelikož byla následně ještě jednou změněna v rámci aplikace.
		/// </summary>
		FailedTemperatureOverridenInternally = 2,

		/// <summary>
		/// Teplotu se nepovedlo změnit, jelikož byla změněna z externího zdroje.
		/// </summary>
		FailedTemperatureOverridenExternally = 3,

		/// <summary>
		/// Teplotu se nepodařilo změnit.
		/// </summary>
		Failed = 4,
	}
}