using Microsoft.EntityFrameworkCore;
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
		protected User()
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
			get;
			protected set;
		}

		/// <summary>
		/// Nastaví vybrané podlaží.
		/// </summary>
		public void SetSelectedFloor(Floor floor)
		{
			this.SelectedFloorId = floor.Id;
		}

		/// <summary>
		/// Vrací všechny místnosti, na které má uživatel vazbu.
		/// </summary>
		public virtual ICollection<UserToRoom> UserToRooms
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací uživatele vhodného k uložení do databáze.
		/// </summary>
		public static async Task<User> Create(IRepository<User> repository, string uid, string userName, string firstName, string lastName)
		{
			// Kouknu, jestli uživatel existuje.
			var existingUser = await repository.FetchOne(new UserByUidQuery(uid));
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
				.HasIndex(u => u.Uid)
				.IsUnique();
		}
	}
}