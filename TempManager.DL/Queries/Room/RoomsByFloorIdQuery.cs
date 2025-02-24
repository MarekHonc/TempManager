using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání místností pro konkrétní podlaží.
	/// </summary>
	public class RoomsByFloorIdQuery : QueryObjectBase<Room>
	{
		private readonly int? floorId;
		private readonly int? userId;

		public RoomsByFloorIdQuery(int? floorId, int? userId)
		{
			this.floorId = floorId;
			this.userId = userId;
		}

		protected override IQueryable<Room> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.Rooms
				.Where(r => !r.Floor.IsHidden && !r.IsHidden);

			if (this.floorId.HasValue)
			{
				query = query.Where(r => r.FloorId == this.floorId);
			}

			if (this.userId.HasValue)
			{
				query = query.Where(r => 
					r.UsersToRoom.Any(utr => utr.UserId == this.userId && utr.HasRightToView)
				);
			}

			return query
				.OrderBy(r => r.Floor.Name)
				.ThenBy(r => r.Name);
		}
	}
}