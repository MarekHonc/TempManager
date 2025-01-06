using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání posledních hodnot v podlaží.
	/// </summary>
	public class FloorHistoryLatestQuery : QueryObjectBase<FloorHistory>
	{
		private readonly int floorId;

		public FloorHistoryLatestQuery(int floorId)
		{
			this.floorId = floorId;
		}

		protected override IQueryable<FloorHistory> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.FloorHistories
				.Where(f => f.FloorId == this.floorId)
				.OrderByDescending(f => f.Date);
		}
	}
}