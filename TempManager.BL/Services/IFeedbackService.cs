namespace TempManager.BL.Services
{
	/// <summary>
	/// Služba starající se o sběr hodnocení od uživatelů.
	/// </summary>
	public interface IFeedbackService
	{
		/// <summary>
		/// Vytvoří nové hodnocení od uživatele.
		/// </summary>
		Task<bool> CreateFeedback(int rating, string feedback);
	}
}