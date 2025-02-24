using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro stažení uživatelů, kteří se mají zobrazit v administraci.
	/// </summary>
	public class AdminListUsersQuery : QueryObjectBase<User>
	{
		/// <inheritdoc />
		protected override IQueryable<User> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Users.Where(u => !u.IsDeleted && u.FirstName != string.Empty);
		}
	}
}
