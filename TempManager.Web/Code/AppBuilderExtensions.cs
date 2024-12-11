using TempManager.DL;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Třída s extension metodami pro konfiguraci aplikace.
	/// </summary>
	public static class AppBuilderExtensions
	{
		/// <summary>
		/// Zajistí, aby databáze byla aktivní.
		/// </summary>
		public static void EnsureLatestDatabase(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<TempManagerContext>();
				context.ApplyMigrations();
			}
		}
	}
}
