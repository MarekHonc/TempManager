namespace TempManager.BL.Models
{
	/// <summary>
	/// Třída reprezentující konkrétní místnost.
	/// </summary>
	public class Room
	{
		public Room(int id, string name, bool hasRightToEdit, bool isFavorite, double temperature, double rh, double co2, double desiredTemperature, bool valveOpen)
		{
			Id = id;
			Name = name;
			HasRightToEdit = hasRightToEdit;
			IsFavorite = isFavorite;
			Temperature = temperature;
			Rh = rh;
			CO2 = co2;
			DesiredTemperature = desiredTemperature;
			ValveOpen = valveOpen;
		}

		/// <summary>
		/// Vrací id místnosti.
		/// </summary>
		public int Id
		{
			get;
		}

		/// <summary>
		/// Vrací název místnosti.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// Vrací, zda-li má aktuální uživatel právo editovat teplotu.
		/// </summary>
		public bool HasRightToEdit
		{
			get;
		}

		/// <summary>
		/// Vrací, zda-li má aktuální uživatel místnost přidanou v oblíbených položkách.
		/// </summary>
		public bool IsFavorite
		{
			get;
		}

		/// <summary>
		/// Vrací aktuální teplotu v místnosti.
		/// </summary>
		public double Temperature
		{
			get;
		}

		/// <summary>
		/// Vrací hodnotu rh v místnosti.
		/// </summary>
		public double Rh
		{
			get;
		}

		/// <summary>
		/// Vrací hodnotu CO2 v místnosti.
		/// </summary>
		public double CO2
		{
			get;
		}

		/// <summary>
		/// Vrací nastavenou teplotu.
		/// </summary>
		public double DesiredTemperature
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je ventil otevřen.
		/// </summary>
		public bool ValveOpen
		{
			get;
		}
	}
}
