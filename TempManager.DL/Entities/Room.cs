using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující místnost.
	/// </summary>
	public class Room : EntityBase, IExternalId
	{
		protected Room()
		{
			this.UsersToRoom = new List<UserToRoom>();
		}

		/// <summary>
		/// Vrací nebo nastavuje id místnosti v externím systému.
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "varchar(30)")]
		public string ExternalId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název místnosti.
		/// </summary>
		[StringLength(50)]
		[Column(TypeName = "varchar(50)")]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje id podlaží, ve kterém místnost je.
		/// </summary>
		[ForeignKey(nameof(Floor))]
		public int FloorId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje podlaží, ve kterém se místnost nachází.
		/// </summary>
		public Floor Floor
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje všechny uživatele, kteří jsou provázáni s touto místností.
		/// </summary>
		public ICollection<UserToRoom> UsersToRoom
		{
			get;
			set;
		}

		/// <summary>
		/// Vytvoří novou entitu místnosti.
		/// </summary>
		public static async Task<Room> Create(IExternalIdRepository<Room> repository, Floor floor, string externalId, bool ignoreExternalIdCheck = false)
		{
			if (!ignoreExternalIdCheck && await repository.GetByExternalId(externalId) != null)
				throw new ArgumentException($"Floor with external id: '{externalId}' already exists!");

			// Vytvořím místnost.
			var room = new Room()
			{
				ExternalId = externalId,
				Name = externalId,
				Floor = floor
			};

			// Vracím novou místnost.
			return room;
		}
	}
}
