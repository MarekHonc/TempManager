using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání místností pro konkrétní podlaží.
	/// </summary>
	public class RoomsByFloorIdQuery : QueryObjectBase<Room>
	{
		private readonly int floorId;

		public RoomsByFloorIdQuery(int floorId)
		{
			this.floorId = floorId;
		}

		protected override IQueryable<Room> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Rooms.Where(r => r.FloorId == this.floorId);
		}
	}
}