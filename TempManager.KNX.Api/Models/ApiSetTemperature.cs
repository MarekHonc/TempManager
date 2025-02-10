using System.Globalization;

namespace TempManager.KNX.Api
{
	/// <summary>
	/// Třída pro nastavení hodnoty na API.
	/// </summary>
	public class ApiSetTemperature
	{
		private readonly string roomExternalId;

		public ApiSetTemperature(double desiredTemperature, string roomExternalId)
		{
			this.roomExternalId = roomExternalId;
			this.DesiredTemperature = desiredTemperature.ToString("0.00", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Vrací kódové označení místnosti.
		/// </summary>
		public string RoomCode
		{
			get
			{
				// Pokud je prázdné -> vracím null.
				if (string.IsNullOrEmpty(this.roomExternalId))
					return null;

				// Rozdělím po mezerách, pokud nevyšlo, vracím null.
				var split = this.roomExternalId.Split(" ", StringSplitOptions.RemoveEmptyEntries);
				if (split.Length == 0)
					return null;

				// Jinak kód = místnost.
				return split[0];
			}
		}

		/// <summary>
		/// Vrací teplotu, která bude nastavena.
		/// </summary>
		public string DesiredTemperature
		{
			get;
		}
	}
}