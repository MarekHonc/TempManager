using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání posledních hodnot v podlaží.
	/// </summary>
	public class FloorHistoryLatestQuery : QueryObjectBase<FloorHistory>
	{
		private readonly int? floorId;
		private readonly bool onlyLatest;

		public FloorHistoryLatestQuery(int? floorId, bool onlyLatest)
		{
			this.floorId = floorId;
			this.onlyLatest = onlyLatest;
		}

		protected override IQueryable<FloorHistory> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.FloorHistories
				.Where(r => !r.Floor.IsHidden);

			if (this.floorId.HasValue)
			{
				query = query.Where(f => f.FloorId == this.floorId);
			}

			// Pokud chci pouze poslední hodnoty pro podlaží.
			// Kvůli dělení po podlaží nelze použít .FetchOne
			if (onlyLatest)
			{
				var grouped = query
					.GroupBy(h => h.FloorId)
					.Select(g => g.OrderByDescending(f => f.Date).FirstOrDefault());

				return grouped;
			}

			return query.OrderByDescending(f => f.Date);
		}
	}
}