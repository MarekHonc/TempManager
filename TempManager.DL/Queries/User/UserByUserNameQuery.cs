using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query, pro stažení uživatele na základě jeho unikátního identifikátoru.
	/// </summary>
	public class UserByUserNameQuery : QueryObjectBase<User>
	{
		private readonly string userName;

		public UserByUserNameQuery(string userName)
		{
			this.userName = userName;
		}
		protected override IQueryable<User> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Users.Where(u =>
				u.UserName == this.userName
			);
		}
	}
}