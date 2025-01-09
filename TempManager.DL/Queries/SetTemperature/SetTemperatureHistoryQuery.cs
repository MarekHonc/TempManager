using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro získání historie nastavování teploty.
	/// </summary>
	public class SetTemperatureHistoryQuery : QueryObjectBase<SetTemperature>
	{
		private readonly int? userId;
		private readonly int? roomId;

		public SetTemperatureHistoryQuery(int? userId, int? roomId)
		{
			this.userId = userId;
			this.roomId = roomId;
		}

		/// <inheritdoc />
		protected override IQueryable<SetTemperature> CreateQuery(TempManagerContext dbContext)
		{
			var query = dbContext.SetTemperatures.AsQueryable();

			if (this.userId.HasValue)
			{
				query = query.Where(st => st.UserId == this.userId);
			}

			if (this.roomId.HasValue)
			{
				query = query.Where(st => st.RoomId == this.roomId);
			}

			return query.OrderByDescending(st => st.Date);
		}
	}
}