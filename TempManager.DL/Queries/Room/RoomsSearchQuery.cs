using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro vyhledávání místností.
	/// </summary>
	public class RoomsSearchQuery : QueryObjectBase<Room>
	{
		private readonly string search;

		public RoomsSearchQuery(string search)
		{
			this.search = search;
		}

		/// <inheritdoc cref="QueryObjectBase{T}.CreateQuery" />
		protected override IQueryable<Room> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Rooms.Where(r =>
				r.Name.Contains(this.search) ||
				r.ExternalId.Contains(this.search)
			);
		}
	}
}
