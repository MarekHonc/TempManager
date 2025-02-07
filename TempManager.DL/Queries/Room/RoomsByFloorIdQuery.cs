using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání místností pro konkrétní podlaží.
	/// </summary>
	public class RoomsByFloorIdQuery : QueryObjectBase<Room>
	{
		private readonly int? floorId;

		public RoomsByFloorIdQuery(int? floorId)
		{
			this.floorId = floorId;
		}

		protected override IQueryable<Room> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.Rooms
				.Where(r => !r.Floor.IsHidden);

			if (this.floorId.HasValue)
			{
				query = query.Where(r => r.FloorId == this.floorId);
			}

			return query
				.OrderBy(r => r.Floor.Name)
				.ThenBy(r => r.Name);
		}
	}
}