namespace TempManager.KNX.Api
{
	/// <summary>
	/// Služba která se stará o připojení k API.
	/// </summary>
	public interface IApiService
	{
		/// <summary>
		/// Vyzkouší napojení na API.
		/// </summary>
		Task<bool> Ping();

		/// <summary>
		/// Přečte všechny proměnné z API.
		/// </summary>
		Task<ApiVariable[]> GetVariables();

		/// <summary>
		/// Vrací hodnoty proměnné z API.
		/// </summary>
		Task<ApiValue[]> GetValues(ApiVariable variable, bool filterEmpty = true);

		/// <summary>
		/// Nastaví novou teplotu na PLC.
		/// </summary>
		Task<bool> SetTemperatures(ApiVariable variable, ApiSetTemperature newTemperatures);
	}
}