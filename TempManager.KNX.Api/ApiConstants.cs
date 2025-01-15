namespace TempManager.KNX.Api
{
	/// <summary>
	/// Konstanty, které využívá API - např. end-pointy.
	/// </summary>
	internal class ApiConstants
	{
		/// <summary>
		/// End-point vracejí info o API.
		/// </summary>
		public const string Ping = "GetInfo";

		/// <summary>
		/// End-point vracející všechny proměnné z API.
		/// </summary>
		public const string GetVariables = "GetList";

		/// <summary>
		/// End-point vracející všechny 
		/// </summary>
		public const string GetVariableValue = "GetObject?{0}";

		/// <summary>
		/// End-point pro nastavení hodnoty v PLC.
		/// </summary>
		public const string PutObject = "PutObject";
	}
}