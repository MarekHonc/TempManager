using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query, pro stažení uživatele na základě jeho unikátního identifikátoru.
	/// </summary>
	public class UserByUidQuery : QueryObjectBase<User>
	{
		private readonly string uid;

		public UserByUidQuery(string uid)
		{
			this.uid = uid;
		}
		protected override IQueryable<User> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Users.Where(u =>
				u.Uid == this.uid
			);
		}
	}
}