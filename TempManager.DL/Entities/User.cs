using System.ComponentModel.DataAnnotations;
using TempManager.DL.Entities.Base;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující uživatele aplikace.
	/// </summary>
	public class User : EntityBase
	{
		/// <summary>
		/// Vrací nebo nastavuje název uživatele.
		/// </summary>
		[Required]
		public string UserName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li je uživatel admin.
		/// </summary>
		public bool IsAdmin
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací všechny místnosti, na které má uživatel vazbu.
		/// </summary>
		public virtual ICollection<UserToRoom> UserToRooms
		{
			get;
			set;
		}
	}
}
