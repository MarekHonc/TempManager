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

			this.FloorRepository = new BaseRepository<Floor>(this.context);
			this.FloorHistoryRepository = new BaseRepository<FloorHistory>(this.context);
			this.RoomRepository = new BaseRepository<Room>(this.context);
			this.UserRepository = new BaseRepository<User>(this.context);
			this.UserToRoomRepository = new BaseRepository<UserToRoom>(this.context);
		}

		#region Repositories

		/// <summary>
		/// Vrací repozitář pro práci s podlažími.
		/// </summary>
		public IRepository<Floor> FloorRepository
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
		public IRepository<Room> RoomRepository
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

		#endregion

		/// <summary>
		/// Uloží změny do databáze.
		/// </summary>
		public async Task<int> SaveChanges()
		{
			return await this.context.SaveChangesAsync();
		}
	}
}
