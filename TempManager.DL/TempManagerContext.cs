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

		/// <summary>
		/// Vrací nebo nastavuje tabulku s historií nastavování teplot.
		/// </summary>
		internal DbSet<SetTemperature> SetTemperatures
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
			Floor.CreateBindings(modelBuilder);
			FloorHistory.CreateBindings(modelBuilder);
			Room.CreateBindings(modelBuilder);
			User.CreateBindings(modelBuilder);

			// TODO: retance nad floor history + set temperature
		}

		/// <summary>
		/// Aplikuje všechny potřebné migrace, aby databáze byla aktuální.
		/// </summary>
		public void ApplyMigrations()
		{
			if (this.Database.GetPendingMigrations().Any())
			{
				this.Database.Migrate();
			}
		}
	}
}