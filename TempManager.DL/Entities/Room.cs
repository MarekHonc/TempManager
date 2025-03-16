using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TempManager.Common;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující místnost.
	/// </summary>
	public class Room : EntityBase, IExternalId
	{
		private ICollection<UserToRoom> usersToRoom;
		private Floor floor;

		protected Room()
		{
		}

		protected Room(ILazyLoader lazyLoader) : base(lazyLoader)
		{
		}

		/// <summary>
		/// Vrací nebo nastavuje id místnosti v externím systému.
		/// </summary>
		[Required]
		[StringLength(30)]
		[Column(TypeName = "varchar(30)")]
		public string ExternalId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název místnosti.
		/// </summary>
		[Required]
		[StringLength(50)]
		[Column(TypeName = "varchar(50)")]
		public string Name
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje id podlaží, ve kterém místnost je.
		/// </summary>
		[ForeignKey(nameof(Floor))]
		public int FloorId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje x pozici na mapě.
		/// </summary>
		public double? XPosition
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje y pozici na mapě.
		/// </summary>
		public double? YPosition
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací zda-li je místnost skrytá.
		/// </summary>
		public bool IsHidden
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací, zda-li na místnost nelze nastavit žádné oprávnění.
		/// </summary>
		public bool NoRights
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje skupiny, do kterých místnost spadá.
		/// </summary>
		public Groups Groups
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje podlaží, ve kterém se místnost nachází.
		/// </summary>
		public Floor Floor
		{
			get => this.LazyLoader.Load(this, ref this.floor);
			protected set => this.floor = value;
		}

		/// <summary>
		/// Vrací nebo nastavuje všechny uživatele, kteří jsou provázáni s touto místností.
		/// </summary>
		public virtual ICollection<UserToRoom> UsersToRoom
		{
			get => this.LazyLoader.Load(this, ref this.usersToRoom);
			protected set => this.usersToRoom = value;
		}

		/// <summary>
		/// Nastaví název místnosti.
		/// </summary>
		public void SetName(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Nastaví novou pozici na mapě.
		/// </summary>
		public void SetPosition(double? x, double? y)
		{
			if ((x.HasValue && !y.HasValue) || (!x.HasValue && y.HasValue))
				throw new ArgumentException("X and Y must be set or unset!");

			this.XPosition = x;
			this.YPosition = y;
		}

		/// <summary>
		/// Updatuje viditelnost místnosti.
		/// </summary>
		public void SetIsVisible(bool isVisible)
		{
			this.IsHidden = !isVisible;
		}

		/// <summary>
		/// Updateuje příznak, zda-li se na místnosti mohou vázat oprávnění.
		/// </summary>
		public void SetNoRights(bool noRights)
		{
			this.NoRights = noRights;
		}

		/// <summary>
		/// Detekuje skupiny na místnosti.
		/// </summary>
		public void DetectGroups()
		{
			this.Groups = this.ExternalId.DetectGroups();
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
				Floor = floor,
				Groups = externalId.DetectGroups()
			};

			// Vracím novou místnost.
			return room;
		}

		/// <summary>
		/// Nastaví dodatečné bindingy v tabulce.
		/// </summary>
		public static void CreateBindings(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Room>()
				.HasIndex(r => r.ExternalId)
				.IsUnique();

			modelBuilder.Entity<Room>()
				.Property(r => r.Groups)
				.HasConversion<int>()
				.HasDefaultValue(Groups.None);
		}
	}
}