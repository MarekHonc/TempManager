using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro stažení viditelných podlaží.
	/// </summary>
	public class GetVisibleFloorsQuery : QueryObjectBase<Floor>
	{
		protected override IQueryable<Floor> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Floors.Where(f => !f.IsHidden);
		}
	}
}