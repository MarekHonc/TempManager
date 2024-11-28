using TempManager.DL.Entities;

namespace TempManager.BL.Models
{
	/// <summary>
	/// Třída reprezentující konkrétní místnost.
	/// </summary>
	public class Room
	{
		internal Room(int id, string name, bool hasRightToEdit, bool isFavorite, double temperature, double rh, double co2, double desiredTemperature, bool valveOpen)
		{
			this.Id = id;
			this.Name = name;
			this.HasRightToEdit = hasRightToEdit;
			this.IsFavorite = isFavorite;
			this.Temperature = temperature;
			this.Rh = rh;
			this.CO2 = co2;
			this.DesiredTemperature = desiredTemperature;
			this.ValveOpen = valveOpen;
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

		/// <summary>
		/// Vrací dto pro místnost, která rovnou nese všechny potřebné informace (tj. hodnoty, oprávnění, ...).
		/// </summary>
		internal static Room Create(DL.Entities.Room room, DL.Entities.JsonTypes.RoomValue roomValue, bool isAdmin, UserToRoom? userToRoom)
		{
			// Rychlá kontrola, že data jsou spolu svázané.
			if (room.ExternalId != roomValue.ExternalRoomId)
				throw new ArgumentException($"Room id: {room.ExternalId} cannot have values from room id: {roomValue.ExternalRoomId}!", nameof(room));

			// Rychlá kontrola i na provázání práv.
			if (!isAdmin && userToRoom != null && room.Id != userToRoom.RoomId)
				throw new ArgumentException($"Room id: {room.ExternalId} cannot have values from room id: {roomValue.ExternalRoomId}!", nameof(room));

			// Vracím novou instanci objektu.
			return new Room(
				room.Id,
				room.Name,
				isAdmin || userToRoom?.HasRightToEdit == true,
				userToRoom?.IsFavorite == true,
				roomValue.Temperature,
				roomValue.Rh,
				roomValue.CO2,
				roomValue.DesiredTemperature,
				roomValue.ValveOpen
			);
		}
	}
}
