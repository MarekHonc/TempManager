using Microsoft.EntityFrameworkCore;
using TempManager.BL.Services;
using TempManager.BL.Services.Implementations;
using TempManager.BL.SyncService;
using TempManager.DL;
using TempManager.DL.Repositories;
using TempManager.KNX.Api;
using TempManager.Web.Code;
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

			// Konfigurace MVC.
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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
			// Pøipojení k databázi
			builder.Services.AddDbContextPool<TempManagerContext>(opt =>
				opt.UseNpgsql(builder.Configuration.GetConnectionString("TempManagerContext")));

			// Pøidám i repositories factory.
			builder.Services.AddScoped<RepositoriesFactory>();

			// Napojení na API -> je to služba, co ète z konfigu, staèí singleton.
			builder.Services.AddSingleton<IApiSettings, ApiSettings>();

			// Služby pro weby -> zapisují a ètou z lokální storage.
			builder.Services.AddScoped<IFloorService, DirectApiFloorService>();
			builder.Services.AddScoped<IRoomService, DirectApiRoomService>();

			// Background task, který synchronizuje lokální storage s/do KNX.
			builder.Services.AddHostedService<ValueSyncServiceWrapper>();
			builder.Services.AddScoped<ValueSyncService>();
		}
	}
}
