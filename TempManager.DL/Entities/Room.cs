using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující místnost.
	/// </summary>
	public class Room : EntityBase, IEntity
	{
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
		public virtual ICollection<UserToRoom> UsersToRoom
		{
			get;
			set;
		}
	}
}
