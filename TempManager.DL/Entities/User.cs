using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;
using TempManager.DL.Queries;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující uživatele aplikace.
	/// </summary>
	public class User : EntityBase, IEntity
	{
		private ICollection<UserToRoom> userToRooms;
		private Floor selectedFloor;

		protected User()
		{
		}

		protected User(ILazyLoader lazyLoader) : base(lazyLoader)
		{
		}

		/// <summary>
		/// Vrací nebo nastavuje název uživatele.
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(150)")]
		public string UserName
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje křestní jméno.
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(150)")]
		public string FirstName
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje příjmení.
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(150)")]
		public string LastName
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje unikátní identifikátor uživatele.
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(150)")]
		public string Uid
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li je uživatel admin.
		/// </summary>
		public bool IsAdmin
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje id aktuálně vybraného podlaží.
		/// </summary>
		[ForeignKey(nameof(SelectedFloor))]
		public int? SelectedFloorId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje aktuálně vybrané podlaží.
		/// </summary>
		public Floor SelectedFloor
		{
			get => this.LazyLoader.Load(this, ref this.selectedFloor);
			protected set => this.selectedFloor = value;
		}

		/// <summary>
		/// Vrací všechny místnosti, na které má uživatel vazbu.
		/// </summary>
		public virtual ICollection<UserToRoom> UserToRooms
		{
			get => this.LazyLoader.Load(this, ref this.userToRooms);
			protected set => this.userToRooms = value;
		}

		/// <summary>
		/// Nastaví vybrané podlaží.
		/// </summary>
		public void SetSelectedFloor(Floor floor)
		{
			this.SelectedFloorId = floor.Id;
		}

		/// <summary>
		/// Nastaví danému uživateli zda-li má administrátorská práva.
		/// </summary>
		public void SetIsAdmin(bool isAdmin)
		{
			this.IsAdmin = isAdmin;
		}

		/// <summary>
		/// Vrací uživatele vhodného k uložení do databáze.
		/// </summary>
		public static async Task<User> Create(IRepository<User> repository, string uid, string userName, string firstName, string lastName)
		{
			// Kouknu, jestli uživatel existuje.
			var existingUser = await repository.FetchOne(new UserByUserNameQuery(uid));
			if (existingUser != null)
				throw new ArgumentException($"Uid {uid} already exists!", nameof(uid));

			var user = new User()
			{
				Uid = uid,
				UserName = userName,
				FirstName = firstName,
				LastName = lastName
			};

			return user;
		}

		/// <summary>
		/// Nastaví dodatečné bindingy v tabulce.
		/// </summary>
		public static void CreateBindings(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(u => u.UserName)
				.IsUnique();
		}
	}
}