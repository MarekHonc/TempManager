using TempManager.DL.Entities;
using TempManager.DL.Repositories;

namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba pro získání zpětné vazby.
	/// </summary>
	public class FeedbackService : IFeedbackService
	{
		private readonly RepositoriesFactory repositoriesFactory;
		private readonly IUserService userService;

		public FeedbackService(RepositoriesFactory repositoriesFactory, IUserService userService)
		{
			this.repositoriesFactory = repositoriesFactory;
			this.userService = userService;
		}

		/// <inheritdoc cref="IFeedbackService.CreateFeedback"/>
		public async Task<bool> CreateFeedback(int rating, string feedback)
		{
			// Hodnocení musí být mezi 1 a 10.
			if (rating < Feedback.MinRating || rating > Feedback.MaxRating)
				throw new ArgumentOutOfRangeException(nameof(rating));

			var user = await this.userService.GetCurrentUser();

			// Vytvořím entitu.
			var entity = Feedback.Create(
				user.Id,
				rating,
				feedback
			);

			// Přidám a uložím.
			await repositoriesFactory.FeedbackRepository.Add(entity);
			await repositoriesFactory.SaveChanges();

			// Vrátím, že je vše uloženo.
			return true;
		}
	}
}