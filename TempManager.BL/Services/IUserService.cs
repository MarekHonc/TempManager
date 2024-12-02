using TempManager.BL.Models;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pro práci s uživatelem.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Vrací aktuálního uživatele aplikace.
		/// </summary>
		Task<User> GetCurrentUser();
	}
}
