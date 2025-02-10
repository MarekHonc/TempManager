using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using TempManager.Web.Code;
using TempManager.Web.Hubs;

namespace TempManager.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Konfigurace MVC.
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

			// Potøebuji použít forwarded header, aby to fungovalo s reverse proxy.
			// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-9.0#other-proxy-server-and-load-balancer-scenarios
			builder.Services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders =
					ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

			// Zaregistruji služby.
			builder.RegisterServices();

			// Registrace Shibba.
			builder.AddShibboleth();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			else
			{
				app.UseHttpsRedirection();
			}

			app.UseForwardedHeaders();
			app.UseStaticFiles();
			
			app.UseRouting();

			app.UseAuthorization();

			app.MapHub<UpdateHub>("/updateHub");
			app.EnsureLatestDatabase();
			app.ScheduleJobs();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=AdminFloor}/{action=Index}/{id?}"
				);

				endpoints.MapHangfireDashboard("/services");
			});

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}