using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Entities.JsonTypes;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující historii podlaží.
	/// Historii eviduji po podlaží, využívám možnosti ukládat jako jsonb do postgre,
	/// kdybych ukládal po místnostech, tak by dat byl zbytečně moc a databáze by se rychle zvětšovala.
	/// </summary>
	public class FloorHistory : EntityBase
	{
		/// <summary>
		/// Vrací nebo nastavuje id podlaží.
		/// </summary>
		[ForeignKey(nameof(Floor))]
		public int FloorId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje datum a čas, ze kterého hodnoty pochází.
		/// </summary>
		public DateTime Date
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje všechny naměřené hodnoty.
		/// </summary>
		[Column(TypeName = "JSONB")]
		public RoomValue[] RoomValues
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje podlaží.
		/// </summary>
		public Floor Floor
		{
			get;
			set;
		}
	}
}
