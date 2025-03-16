using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.DL.Entities.JsonTypes;

namespace TempManager.BL.Models
{
	/// <summary>
	/// Třída reprezentující konkrétní místnost.
	/// </summary>
	public class Room
	{
		internal Room(int id, string name, string floorName,
			string externalId, bool hasRightToEdit, bool isFavorite,
			double temperature, double rh, double desiredTemperature,
			double valveOpen, Groups groups, double? x = null, double? y = null,
			bool isError = false
		)
		{
			this.Id = id;
			this.Name = name;
			this.FloorName = floorName;
			this.ExternalId = externalId;
			this.HasRightToEdit = hasRightToEdit;
			this.IsFavorite = isFavorite;
			this.Temperature = temperature;
			this.Rh = rh;
			this.DesiredTemperature = desiredTemperature;
			this.ValveOpen = valveOpen;
			this.Groups = groups;
			this.X = x;
			this.Y = y;
			this.IsError = isError;
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
		/// Vrací název podlaží.
		/// </summary>
		public string FloorName
		{
			get;
		}

		/// <summary>
		/// Vrací nebo nastavuje externí identifikátor místnosti.
		/// </summary>
		public string ExternalId
		{
			get;
			set;
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
		/// Vrací nastavenou teplotu.
		/// </summary>
		public double DesiredTemperature
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je ventil otevřen.
		/// </summary>
		public double ValveOpen
		{
			get;
		}

		/// <summary>
		/// Vrací X pozici na mapě.
		/// </summary>
		public double? X
		{
			get;
		}

		/// <summary>
		/// Vrací Y pozici na mapě.
		/// </summary>
		public double? Y
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li při získání hodnot místnosti nastal error.
		/// </summary>
		public bool IsError
		{
			get;
		}

		/// <summary>
		/// Vrací skupiny, ve kterých se místnost nachází.
		/// </summary>
		public Groups Groups
		{
			get;
		}

		/// <summary>
		/// Vrací dto pro místnost, která rovnou nese všechny potřebné informace (tj. hodnoty, oprávnění, ...).
		/// </summary>
		internal static Room Create(DL.Entities.Room room, RoomValue roomValue, bool canViewAllRooms, UserToRoom userToRoom)
		{
			// Rychlá kontrola, že data jsou spolu svázané.
			if (room.ExternalId != roomValue.ExternalRoomId)
				throw new ArgumentException($"Room id: {room.ExternalId} cannot have values from room id: {roomValue.ExternalRoomId}!", nameof(room));

			// Rychlá kontrola i na provázání práv.
			if (!canViewAllRooms && userToRoom != null && room.Id != userToRoom.RoomId)
				throw new ArgumentException($"Room id: {room.ExternalId} cannot have values from room id: {roomValue.ExternalRoomId}!", nameof(room));

			// Vracím novou instanci objektu.
			return new Room(
				room.Id,
				room.Name,
				room.Floor.Name,
				room.ExternalId,
				canViewAllRooms || userToRoom?.HasRightToEdit == true,
				userToRoom?.IsFavorite == true,
				roomValue.Temperature,
				roomValue.Rh,
				roomValue.DesiredTemperature,
				roomValue.ValveOpen,
				room.Groups,
				room.XPosition,
				room.YPosition
			);
		}

		/// <summary>
		/// Vytvoří místnost s "chybou" tj. bez hodnot.
		/// </summary>
		internal static Room Error(DL.Entities.Room room)
		{
			// Vracím novou instanci objektu.
			return new Room(
				room.Id,
				room.Name,
				room.Floor.Name,
				room.ExternalId,
				hasRightToEdit: false,
				isFavorite: false,
				temperature: 0,
				rh: 0,
				desiredTemperature: 0,
				valveOpen: 0,
				room.Groups,
				room.XPosition,
				room.YPosition,
				isError: true
			);
		}

		/// <summary>
		/// Vrací dto pro místnost, která rovnou nese všechny potřebné informace (tj. hodnoty, oprávnění, ...).
		/// </summary>
		internal static Room Create(RoomValue roomValue)
		{
			// Vracím novou instanci objektu.
			return new Room(
				id: 0,
				roomValue.ExternalRoomId,
				string.Empty,
				roomValue.ExternalRoomId,
				hasRightToEdit: false,
				isFavorite: false,
				roomValue.Temperature,
				roomValue.Rh,
				roomValue.DesiredTemperature,
				roomValue.ValveOpen,
				Groups.None
			);
		}
	}
}