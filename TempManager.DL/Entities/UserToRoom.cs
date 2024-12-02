using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Propojení uživatele s místností.
	/// </summary>
	[PrimaryKey(nameof(UserId), nameof(RoomId))]
	public class UserToRoom : IEntity
	{
		/// <summary>
		/// Vrací nebo nastavuje identifikátor uživatele.
		/// </summary>
		[ForeignKey(nameof(User))]
		public int UserId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor místnosti.
		/// </summary>
		[ForeignKey(nameof(Room))]
		public int RoomId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li má uživatel právo nastavit teplotu v místnosti.
		/// </summary>
		public bool HasRightToEdit
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li má uživatel označenou místnost jako oblíbenou.
		/// </summary>
		public bool IsFavorite
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatele.
		/// </summary>
		public User User
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje místnost.
		/// </summary>
		public Room Room
		{
			get;
			set;
		}

		/// <summary>
		/// Vytvoří entitu vhodnou k uložení.
		/// </summary>
		public static UserToRoom Create(int userId, int roomId, bool isFavorite)
		{
			return new UserToRoom()
			{
				UserId = userId,
				RoomId = roomId,
				IsFavorite = isFavorite
			};
		}
	}
}
