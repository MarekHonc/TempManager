using TempManager.Common;
using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání místností pro konkrétní podlaží.
	/// </summary>
	public class RoomsByFloorIdQuery : QueryObjectBase<Room>
	{
		private readonly int userId;
		private readonly Groups groups;
		private readonly bool canViewAll;
		private readonly int? floorId;

		public RoomsByFloorIdQuery(int userId, Groups groups, bool canViewAll, int? floorId)
		{
			this.userId = userId;
			this.groups = groups;
			this.canViewAll = canViewAll;
			this.floorId = floorId;
		}

		protected override IQueryable<Room> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.Rooms
				.Where(r => !r.Floor.IsHidden && !r.IsHidden);

			if (this.floorId.HasValue)
			{
				query = query.Where(r => r.FloorId == this.floorId);
			}

			if (!this.canViewAll)
			{
				query = query.Where(r => 
					r.UsersToRoom.Any(utr => utr.UserId == this.userId && utr.HasRightToView)
					||
					(this.groups != Groups.None && (r.Groups & this.groups) != 0)
				);
			}

			return query
				.OrderBy(r => r.Floor.Name)
				.ThenBy(r => r.Name);
		}
	}
}