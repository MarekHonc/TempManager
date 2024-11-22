using TempManager.BL.Services;
using TempManager.Web.HostedServices;

namespace TempManager.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			// Zaregistruji služby.
			RegisterServices(builder);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}

		/// <summary>
		/// Zaregistruje služby aplikace do DI kontejneru.
		/// </summary>
		private static void RegisterServices(WebApplicationBuilder builder)
		{
			// Služby pro weby -> zapisují a ètou z lokální storage.
			builder.Services.AddScoped<IFloorService, FloorTestService>();
			builder.Services.AddScoped<IRoomService, RoomTestService>();

			// Background task, který synchronizuje lokální storage s/do KNX.
			builder.Services.AddHostedService<ValueSyncServiceWrapper>();
		}
	}
}
