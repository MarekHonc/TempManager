using TempManager.BL.Models;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pro čtení podlaží.
	/// </summary>
	public interface IFloorService
	{
		/// <summary>
		/// Vrací všechny podlaží.
		/// </summary>
		Task<Floor[]> GetFloors();

		/// <summary>
		/// Vrací poslední vybrané podlaží.
		/// </summary>
		Task<Floor> GetLastSelectedFloor();

		/// <summary>
		/// Uloží poslední zobrazené podlaží.
		/// </summary>
		Task<bool> SaveLastSelectedFloor(int floorId);

		/// <summary>
		/// Vrací podlaží podle jeho id.
		/// </summary>
		Task<Floor> GetByFriendlyId(string floorId);
	}
}