using TempManager.Common;
using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro stažení hodnot, které čekají na promítnutí do PLC.
	/// </summary>
	public class PendingChangesQuery : QueryObjectBase<SetTemperature>
	{
		/// <inheritdoc />
		protected override IQueryable<SetTemperature> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.SetTemperatures.Where(st => st.Result == SetTemperatureResult.Pending);
		}
	}
}