using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Propojení uživatele s místností.
	/// </summary>
	[PrimaryKey(nameof(UserId), nameof(RoomId))]
	public class UserToRoom : IEntity
	{
		private Room room;
		private User user;

		protected UserToRoom()
		{
		}

		protected UserToRoom(ILazyLoader lazyLoader)
		{
			this.LazyLoader = lazyLoader;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor uživatele.
		/// </summary>
		[ForeignKey(nameof(User))]
		public int UserId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor místnosti.
		/// </summary>
		[ForeignKey(nameof(Room))]
		public int RoomId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li má uživatel právo nastavit teplotu v místnosti.
		/// </summary>
		public bool HasRightToEdit
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li má uživatel právo vidět teplotu v místnosti.
		/// </summary>
		public bool HasRightToView
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li má uživatel označenou místnost jako oblíbenou.
		/// </summary>
		public bool IsFavorite
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatele.
		/// </summary>
		public User User
		{
			get => this.LazyLoader.Load(this, ref this.user);
			set => this.user = value;
		}

		/// <summary>
		/// Vrací nebo nastavuje místnost.
		/// </summary>
		public Room Room
		{
			get => this.LazyLoader.Load(this, ref this.room);
			set => this.room = value;
		}

		/// <summary>
		/// Vrací Lazy loader pro dodatečné načítání entit.
		/// </summary>
		private ILazyLoader LazyLoader
		{
			get;
		}

		/// <summary>
		/// Nastaví, zda-li má uživatel místnost uloženou v oblíbených.
		/// </summary>
		public void SetFavorite(bool isFavorite)
		{
			this.IsFavorite = isFavorite;
		}

		/// <summary>
		/// Nastaví, zda-li má uživatel místnost právo editovat.
		/// </summary>
		public void SetHasRight(bool hasRightToEdit, bool hasRightToView)
		{
			if (hasRightToEdit && !hasRightToView)
				throw new Exception("Cannot have right to edit without right to view!");

			this.HasRightToEdit = hasRightToEdit;
			this.HasRightToView = hasRightToView;
		}

		/// <summary>
		/// Vytvoří entitu vhodnou k uložení.
		/// </summary>
		public static UserToRoom Create(int userId, int roomId, bool isFavorite, bool hasRightToEdit, bool hasRightToView)
		{
			if (hasRightToEdit && !hasRightToView)
				throw new Exception("Cannot have right to edit without right to view!");

			return new UserToRoom()
			{
				UserId = userId,
				RoomId = roomId,
				IsFavorite = isFavorite,
				HasRightToEdit = hasRightToEdit,
				HasRightToView = hasRightToView
			};
		}
	}
}