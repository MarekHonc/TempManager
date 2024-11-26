using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující podlaží.
	/// </summary>
	public class Floor : EntityBase
	{
		/// <summary>
		/// Vrací nebo nastavuje id podlaží v externím systému.
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "VARCHAR")]
		public string ExternalId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje interní název podlaží.
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "VARCHAR")]
		public string FriendlyId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací název podlaží.
		/// </summary>
		[StringLength(50)]
		[Column(TypeName = "VARCHAR")]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací název šablony, která se má zobrazit při zobrazení typu "mapa".
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "VARCHAR")]
		public string MapViewName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje seznam místností v podlaží.
		/// </summary>
		public virtual ICollection<Room> Rooms
		{
			get;
			set;
		}
	}
}
