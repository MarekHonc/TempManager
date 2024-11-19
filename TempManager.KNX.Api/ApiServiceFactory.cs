namespace TempManager.KNX.Api
{
	/// <summary>
	/// Factory třída pro získání služby pro práci s API.
	/// </summary>
	public static class ApiServiceFactory
	{
		/// <summary>
		/// Vrací instanci služby pro připojení k API.
		/// </summary>
		public static IApiService GetService(IApiSettings settings)
		{
			return new ApiService(settings);
		}
	}
}
