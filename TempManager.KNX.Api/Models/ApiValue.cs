using Newtonsoft.Json;

namespace TempManager.KNX.Api
{
	/// <summary>
	/// Model reprezentující hodnotu z API.
	/// </summary>
	public class ApiValue
	{
		private double valveOpen;

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
		/// Vrací na kolik % je ventil otevřen.
		/// </summary>
		[JsonProperty("ventil")]
		public double ValveOpen
		{
			get => this.valveOpen;
			internal set => this.valveOpen = value > 100 ? 100 : value;
		}

		/// <summary>
		/// Nastaví novou hodnotu teploty.
		/// </summary>
		public void SetDesiredTemperature(double newValue)
		{
			this.DesiredTemperature = newValue;
		}
	}
}