using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Repositories
{
	/// <summary>
	/// Tovární třída pro získání repozitářů.
	/// </summary>
	public class RepositoriesFactory
	{
		private readonly TempManagerContext context;

		public RepositoriesFactory(TempManagerContext context)
		{
			this.context = context;

			this.FloorRepository = new ExternalIdBaseRepository<Floor>(this.context);
			this.FloorHistoryRepository = new BaseRepository<FloorHistory>(this.context);
			this.RoomRepository = new ExternalIdBaseRepository<Room>(this.context);
			this.UserRepository = new BaseRepository<User>(this.context);
			this.UserToRoomRepository = new BaseRepository<UserToRoom>(this.context);
			this.SetTemperatureRepository = new BaseRepository<SetTemperature>(this.context);
		}

		#region Repositories

		/// <summary>
		/// Vrací repozitář pro práci s podlažími.
		/// </summary>
		public IExternalIdRepository<Floor> FloorRepository
		{
			get;
		}

		/// <summary>
		/// Vrací repozitář pro práci s historií podlaží.
		/// </summary>
		public IRepository<FloorHistory> FloorHistoryRepository
		{
			get;
		}

		/// <summary>
		/// Vrací repozitář pro práci s místnostmi.
		/// </summary>
		public IExternalIdRepository<Room> RoomRepository
		{
			get;
		}

		/// <summary>
		/// Vrací repozitář pro práci s uživateli.
		/// </summary>
		public IRepository<User> UserRepository
		{
			get;
		}

		/// <summary>
		/// Vrací repozitář pro práci s podlažími.
		/// </summary>
		public IRepository<UserToRoom> UserToRoomRepository
		{
			get;
		}

		/// <summary>
		/// Vrací repozitář pro práci s historíí nastavení teplot.
		/// </summary>
		public IRepository<SetTemperature> SetTemperatureRepository
		{
			get;
		}

		#endregion

		/// <summary>
		/// Tato metoda vypne automatickou detekci změn na entitách, takže je možné je změnit všechny velmi rychle a až potom se všechny změny nadetekují najednou.
		/// Použití pomocí using(BulkChange()){...}
		/// </summary>
		public IDisposable BulkChange()
		{
			var temp = this.context.ChangeTracker.AutoDetectChangesEnabled;
			this.context.ChangeTracker.AutoDetectChangesEnabled = false;
			return new SimpleDisposable(() => this.context.ChangeTracker.AutoDetectChangesEnabled = temp);
		}

		/// <summary>
		/// Uloží změny do databáze.
		/// </summary>
		public Task<int> SaveChanges()
		{
			return this.context.SaveChangesAsync();
		}
	}
}