using Microsoft.EntityFrameworkCore;
using TempManager.DL.Entities;

namespace TempManager.DL
{
	/// <summary>
	/// Přístup do databáze.
	/// </summary>
	public class TempManagerContext : DbContext
	{
		public TempManagerContext(DbContextOptions<TempManagerContext> options) : base(options)
		{
		}

		#region DbSets

		/// <summary>
		/// Vrací nebo nastavuje seznam všech podlaží v aplikaci.
		/// </summary>
		internal DbSet<Floor> Floors
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje historii všech podlaží.
		/// </summary>
		internal DbSet<FloorHistory> FloorHistories
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje seznam všech místností.
		/// </summary>
		internal DbSet<Room> Rooms
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatele aplikace.
		/// </summary>
		internal DbSet<User> Users
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje vazbu mezi uživatelem a místností.
		/// Evidence práva zápisu, případně ukládání do oblíbených.
		/// </summary>
		internal DbSet<UserToRoom> UsersToRooms
		{
			get;
			set;
		}

		#endregion

		/// <summary>
		/// Vytvoří vazby v databázi.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// TODO: Rzházet aby se definovalo na entitách -> možná zkusit static metodu na interface?

			modelBuilder.Entity<Floor>()
				.HasIndex(f => f.ExternalId)
				.IsUnique();

			modelBuilder.Entity<Floor>()
				.HasIndex(f => f.FriendlyId)
				.IsUnique();

			modelBuilder.Entity<Room>()
				.HasIndex(r => r.ExternalId)
				.IsUnique();

			modelBuilder.Entity<FloorHistory>()
				.HasIndex(nameof(FloorHistory.Date), nameof(FloorHistory.FloorId))
				.IsUnique();

			modelBuilder.Entity<FloorHistory>()
				.OwnsMany(fh => fh.RoomValues, builder => { builder.ToJson(); });
		}
	}
}
