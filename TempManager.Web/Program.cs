using TempManager.Web.Code;

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

			// Zaregistruji služby.
			builder.RegisterServices();

			// Registrace Shibba.
			builder.AddShibboleth();

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

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=AdminFloor}/{action=Index}/{id?}"
				);
			});

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.EnsureLatestDatabase();
			app.Run();
		}
	}
}