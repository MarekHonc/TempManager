using TempManager.BL.Interfaces;
using TempManager.BL.Models;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba, pro práci s uživateli z Shibboleth přihlášení.
	/// </summary>
	public class UserService : IUserService
	{
		private readonly RepositoriesFactory repositoriesFactory;
		private readonly IClaimsUser claimsUser;

		public UserService(IClaimsUser claimsUser, RepositoriesFactory repositoriesFactory)
		{
			this.claimsUser = claimsUser;
			this.repositoriesFactory = repositoriesFactory;
		}

		/// <inheritdoc cref="GetCurrentUser"/>
		public async Task<User> GetCurrentUser()
		{
			var user = this.claimsUser;

			// Kontrola existujícího.
			var query = new UserByUidQuery(user.Uid);
			var existing = await this.repositoriesFactory.UserRepository.FetchOne(query);

			// Pokud již existuje, vracím existujícího.
			if (existing != null)
				return new User(existing);

			// Jinak zakládám nového.
			var newUser = await DL.Entities.User.Create(
				this.repositoriesFactory.UserRepository,
				user.Uid,
				user.UserName,
				user.FirstName,
				user.LastName
			);

			// První vytvořený uživatel je vždy admin.
			if (!await this.repositoriesFactory.UserRepository.IsAny())
				newUser.SetIsAdmin(isAdmin: true);

			await this.repositoriesFactory.UserRepository.Add(newUser);
			await this.repositoriesFactory.SaveChanges();

			// A vracím správného uživatele.
			return new User(newUser);
		}
	}
}