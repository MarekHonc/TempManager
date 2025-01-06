using TempManager.BL.Models;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pro práci s místnostmi.
	/// </summary>
	public interface IRoomService
	{
		/// <summary>
		/// Vrací místnosti pro dané podlaží.
		/// </summary>
		Task<Room[]> GetRooms(int floorId);

		/// <summary>
		/// Nastaví teplotu v dané místnosti.
		/// </summary>
		Task<bool> SetTemperature(int roomId, double desiredTemperature);

		/// <summary>
		/// Uloží místnost do oblíbených.
		/// </summary>
		Task<bool> SetFavorite(int roomId, bool isFavorite);
	}
}