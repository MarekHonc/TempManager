namespace TempManager.DL.Entities.JsonTypes
{
	/// <summary>
	/// Entita, reprezentující naměřené hodnoty v dané místnosti.
	/// </summary>
	public class RoomValue
	{
		internal RoomValue()
		{
		}

		public RoomValue(string externalRoomId, double temperature, double rh, double co2, double desiredTemperature, bool valveOpen)
		{
			this.ExternalRoomId = externalRoomId;
			this.Temperature = temperature;
			this.Rh = rh;
			this.CO2 = co2;
			this.DesiredTemperature = desiredTemperature;
			this.ValveOpen = valveOpen;
		}

		/// <summary>
		/// Vrací nebo nastavuje externí místnosti.
		/// </summary>
		public string ExternalRoomId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje aktuální teplotu v místnosti.
		/// </summary>
		public double Temperature
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje hodnotu rh v místnosti.
		/// </summary>
		public double Rh
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje hodnotu CO2 v místnosti.
		/// </summary>
		public double CO2
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje nastavenou teplotu.
		/// </summary>
		public double DesiredTemperature
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li je ventil otevřen.
		/// </summary>
		public bool ValveOpen
		{
			get;
			set;
		}
	}
}