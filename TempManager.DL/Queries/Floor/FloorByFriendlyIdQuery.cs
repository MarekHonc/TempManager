using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání podlaží, na základě jeho friendly identifikátoru.
	/// </summary>
	public class FloorByFriendlyIdQuery : QueryObjectBase<Floor>
	{
		private readonly string friendlyId;

		public FloorByFriendlyIdQuery(string friendlyId)
		{
			this.friendlyId = friendlyId;
		}

		protected override IQueryable<Floor> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Floors.Where(f => f.FriendlyId == this.friendlyId);
		}
	}
}
