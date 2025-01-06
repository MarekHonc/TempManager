using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání oprávnění konkrétního uživatele.
	/// </summary>
	public class UserToRoomForUserQuery : QueryObjectBase<UserToRoom>
	{
		private readonly int userId;
		private readonly int? roomId;

		public UserToRoomForUserQuery(int userId, int? roomId = null)
		{
			this.userId = userId;
			this.roomId = roomId;
		}

		protected override IQueryable<UserToRoom> CreateQuery(TempManagerContext dbContext)
		{
			var query =  dbContext.UsersToRooms.Where(utr => utr.UserId == this.userId);

			if (this.roomId.HasValue)
			{
				query = query.Where(utr => utr.RoomId == this.roomId.Value);
			}

			return query;
		}
	}
}