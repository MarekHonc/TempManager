using TempManager.BL.Models;

namespace TempManager.BL.SyncService
{
	/// <summary>
	/// Job na pozadí, který synchronizuje hodnoty z API vůči lokální databázi.
	/// </summary>
	public interface IValueSyncService
	{
		/// <summary>
		/// Zesynchronizuje všechny podlaží z API do aplikace.
		/// </summary>
		Task<Floor[]> SyncFloors();

		/// <summary>
		/// Zesynchronizuje naměřené hodnoty v místnostech z API do aplikace.
		/// </summary>
		Task<Dictionary<Floor, Room[]>> SyncRooms();


		/// <summary>
		/// Vrací datum a čas poslední synchronizace.
		/// </summary>
		Task<DateTimeOffset> GetLatestSyncTime();
	}
}