using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro vyhledávání mezi uživateli.
	/// </summary>
	public class UserSearchQuery : QueryObjectBase<User>
	{
		private readonly string search;

		public UserSearchQuery(string search)
		{
			this.search = search;
		}

		protected override IQueryable<User> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Users.Where(u =>
				u.UserName.Contains(this.search) &&
				!u.IsAdmin
			);
		}
	}
}
