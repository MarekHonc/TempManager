using Microsoft.EntityFrameworkCore;
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
			this.FeedbackRepository = new BaseRepository<Feedback>(this.context);
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

		/// <summary>
		/// Vrací repozitář pro práci se zpětnou vazbou.
		/// </summary>
		public IRepository<Feedback> FeedbackRepository
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

		#region Retence

		/// <summary>
		/// Počet dní jak dlouho se uchovává historie nastavení teploty.
		/// </summary>
		private const int SetTemperatureHistoryKeepDays = 14;

		/// <summary>
		/// Počet dní jak dlouho se uchovávají naměřené hodnoty.
		/// </summary>
		private const int RoomValueKeepDays = 14;

		/// <summary>
		/// Smaže staré hodnoty.
		/// </summary>
		public async Task DeleteOldValues()
		{
			var now = DateTime.UtcNow;

			// 1. Nastavení teploty.
			var setTemperatureHistoryCutoffDate = now.AddDays(-SetTemperatureHistoryKeepDays);
			await context.SetTemperatures
				.Where(r => r.Date < setTemperatureHistoryCutoffDate)
				.ExecuteDeleteAsync();

			// 2. Naměřené hodnoty.
			var roomValueCutoffDate = now.AddDays(-RoomValueKeepDays);
			await context.FloorHistories
				.Where(r => r.Date < roomValueCutoffDate)
				.ExecuteDeleteAsync();
		}

		#endregion

		/// <summary>
		/// Uloží změny do databáze.
		/// </summary>
		public Task<int> SaveChanges()
		{
			return this.context.SaveChangesAsync();
		}
	}
}