using Newtonsoft.Json;

namespace TempManager.KNX.Api
{
	/// <summary>
	/// Model reprezentující hodnotu z API.
	/// </summary>
	public class ApiValue
	{
		/// <summary>
		/// Vrací název místnosti.
		/// </summary>
		[JsonProperty("name")]
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// Vrací teplotu místnosti.
		/// </summary>
		[JsonProperty("temp")]
		public double Temp
		{
			get;
			internal set;
		}

		/// <summary>
		/// Vrací hodnotu rh v místnosti.
		/// </summary>
		[JsonProperty("rh")]
		public double Rh
		{
			get;
			internal set;
		}

		/// <summary>
		/// Vrací hodnotu CO2 v místnosti.
		/// </summary>
		[JsonProperty("co2")]
		public double CO2
		{
			get;
			internal set;
		}

		/// <summary>
		/// Vrací nastavenou teplotu.
		/// </summary>
		[JsonProperty("tempW")]
		public double DesiredTemperature
		{
			get;
			internal set;
		}

		/// <summary>
		/// Vrací zda-li je ventil otevřen.
		/// </summary>
		[JsonProperty("ventil")]
		public bool ValveOpen
		{
			get;
			internal set;
		}
	}
}
