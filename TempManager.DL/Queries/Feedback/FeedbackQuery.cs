using TempManager.DL.Entities;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Query pro stažení feedbacku.
	/// </summary>
	public class FeedbackQuery : QueryObjectBase<Feedback>
	{
		/// <inheritdoc />
		protected override IQueryable<Feedback> CreateQuery(TempManagerContext dbContext)
		{
			return dbContext.Feedbacks.OrderByDescending(f => f.RatedAt);
		}
	}
}