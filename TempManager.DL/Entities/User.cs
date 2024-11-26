using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující uživatele aplikace.
	/// </summary>
	public class User : EntityBase, IEntity
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
		/// Vrací nebo nastavuje id aktuálně vybraného podlaží.
		/// </summary>
		[ForeignKey(nameof(SelectedFloor))]
		public int? SelectedFloorId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje aktuálně vybrané podlaží.
		/// </summary>
		public Floor? SelectedFloor
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
