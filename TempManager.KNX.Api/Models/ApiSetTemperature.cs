namespace TempManager.KNX.Api
{
	/// <summary>
	/// Třída pro nastavení hodnoty na API.
	/// </summary>
	public class ApiSetTemperature
	{
		public ApiSetTemperature(int arrayIndex, double desiredTemperature)
		{
			this.ArrayIndex = arrayIndex;
			this.DesiredTemperature = desiredTemperature;
		}

		/// <summary>
		/// Vrací index v poli na kterém se teplota nachází.
		/// </summary>
		public int ArrayIndex
		{
			get;
		}

		/// <summary>
		/// Vrací teplotu, která bude nastavena.
		/// </summary>
		public double DesiredTemperature
		{
			get;
		}
	}
}