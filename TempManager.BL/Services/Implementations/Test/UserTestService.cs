using TempManager.BL.Models;
using TempManager.DL.Repositories;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Testovací služba pro práci s uživateli.
	/// </summary>
	public class UserTestService : IUserService
	{
		private readonly RepositoriesFactory repositoriesFactory;

		public UserTestService(RepositoriesFactory repositoriesFactory)
		{
			this.repositoriesFactory = repositoriesFactory;
		}

		/// <inheritdoc cref="GetCurrentUser"/>
		public async Task<User> GetCurrentUser()
		{
			var users = await repositoriesFactory.UserRepository.FetchAll();
			return new User(users.OrderBy(u => u.Id).First());
		}
	}
}