using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TempManager.DL.Entities.Base;
using TempManager.DL.Entities.JsonTypes;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující historii podlaží.
	/// Historii eviduji po podlaží, využívám možnosti ukládat jako jsonb do postgre,
	/// kdybych ukládal po místnostech, tak by dat byl zbytečně moc a databáze by se rychle zvětšovala.
	/// </summary>
	public class FloorHistory : EntityBase, IEntity
	{
		private Floor floor;

		protected FloorHistory()
		{
		}

		protected FloorHistory(ILazyLoader lazyLoader) : base(lazyLoader)
		{
		}

		/// <summary>
		/// Vrací nebo nastavuje id podlaží.
		/// </summary>
		[ForeignKey(nameof(Floor))]
		public int FloorId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje datum a čas, ze kterého hodnoty pochází.
		/// </summary>
		public DateTimeOffset Date
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje všechny naměřené hodnoty.
		/// </summary>
		[Column(TypeName = "jsonb")]
		public ICollection<RoomValue> RoomValues
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje podlaží.
		/// </summary>
		public Floor Floor
		{
			get => this.LazyLoader.Load(this, ref this.floor);
			protected set => this.floor = value;
		}

		/// <summary>
		/// Vytvoří nový záznam o místnosti.
		/// </summary>
		public static FloorHistory Create(Floor floor, DateTimeOffset date, RoomValue[] values)
		{
			return new FloorHistory()
			{
				Floor = floor,
				Date = date,
				RoomValues = values
			};
		}

		/// <summary>
		/// Nastaví dodatečné bindingy v tabulce.
		/// </summary>
		public static void CreateBindings(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FloorHistory>()
				.HasIndex(nameof(FloorHistory.Date), nameof(FloorHistory.FloorId))
				.IsUnique();

			modelBuilder.Entity<FloorHistory>()
				.OwnsMany(fh => fh.RoomValues, builder => { builder.ToJson(); });
		}
	}
}