using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání oprávnění konkrétního uživatele.
	/// </summary>
	public class UserToRoomForUserQuery : QueryObjectBase<UserToRoom>
	{
		private readonly int userId;

		public UserToRoomForUserQuery(int userId)
		{
			this.userId = userId;
		}

		protected override IQueryable<UserToRoom> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.UsersToRooms.Where(utr => utr.UserId == this.userId);
		}
	}
}
