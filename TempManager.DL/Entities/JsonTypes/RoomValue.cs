namespace TempManager.DL.Entities.JsonTypes
{
	/// <summary>
	/// Entita, reprezentující naměřené hodnoty v dané místnosti.
	/// </summary>
	public class RoomValue
	{
		/// <summary>
		/// Vrací nebo nastuvje id místnosti.
		/// </summary>
		public int RoomId
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
