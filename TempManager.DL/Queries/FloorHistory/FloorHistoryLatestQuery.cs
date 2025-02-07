using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání posledních hodnot v podlaží.
	/// </summary>
	public class FloorHistoryLatestQuery : QueryObjectBase<FloorHistory>
	{
		private readonly int? floorId;

		public FloorHistoryLatestQuery(int? floorId = null)
		{
			this.floorId = floorId;
		}

		protected override IQueryable<FloorHistory> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.FloorHistories
				.Where(r => !r.Floor.IsHidden);

			if (this.floorId.HasValue)
			{
				query = query.Where(f => f.FloorId == this.floorId);
			}

			var grouped = query
				.GroupBy(h => h.FloorId)
				.Select(g => g.OrderByDescending(f => f.Date).FirstOrDefault());

			return grouped;
		}
	}
}