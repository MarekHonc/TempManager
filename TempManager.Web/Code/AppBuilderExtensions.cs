using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using TempManager.BL.Interfaces;
using TempManager.BL.Services;
using TempManager.BL.SyncService;
using TempManager.DL;
using TempManager.DL.Repositories;
using TempManager.KNX.Api;
using TempManager.Shibboleth;
using TempManager.Web.Areas.Admin.Code;
using TempManager.Web.HostedServices;
using TempManager.Web.Models;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Třída s extension metodami pro konfiguraci aplikace.
	/// </summary>
	public static class AppBuilderExtensions
	{
		/// <summary>
		/// Zaregistruje služby aplikace do DI kontejneru.
		/// </summary>
		public static void RegisterServices(this WebApplicationBuilder builder)
		{
			// Služby (např. přihlašovací služba) potřebuje mít aktuální httpcontext.
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<CookieManager>();

			// Připojení k databázi
			builder.Services.AddDbContextPool<TempManagerContext>(opt =>
				opt.UseNpgsql(builder.Configuration.GetConnectionString("TempManagerContext")));

			// Přidám i repositories factory.
			builder.Services.AddScoped<RepositoriesFactory>();

			// Napojení na API -> je to služba, co čte z konfigu, stačí singleton.
			builder.Services.AddSingleton<IApiSettings, ApiSettings>();
			builder.Services.AddSingleton<ShibbolethAuthorizeSettings>();

			// Služby pro weby -> zapisují a čtou z lokální storage.
			builder.Services.AddScoped<IClaimsUser, ClaimsUser>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IFloorService, FloorService>();
			builder.Services.AddScoped<IRoomService, RoomService>();
			builder.Services.AddScoped<IFeedbackService, FeedbackService>();

			// Hangfire.
			builder.Services.AddHangfire(config =>
			{
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
					.UseSimpleAssemblyNameTypeSerializer()
					.UseRecommendedSerializerSettings()
					.UseMemoryStorage()
					.UseFilter(new AutomaticRetryAttribute { Attempts = 0 }) // Zakáže opakované pokusy
					.UseFilter(new DisableConcurrentExecutionAttribute(timeoutInSeconds: 60)); // Globální zákaz souběhu
			});
			builder.Services.AddHangfireServer();

			// Tasky na pozadí.
			builder.Services.AddScoped<IValueSyncService, ValueSyncService>();
			builder.Services.AddTransient<ISyncJob, SyncJob>();
			builder.Services.AddTransient<ICleanerJob, CleanerJob>();

			// Hub pro real time update hodnot.
			builder.Services.AddSignalR();
		}

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

		/// <summary>
		/// Zaregistruje do aplikace autorizaci pomocí Shibboleth.
		/// </summary>
		public static void AddShibboleth(this WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = ShibbolethDefaults.AuthenticationScheme;
			}).AddShibboleth(options =>
			{
				if (!builder.Environment.IsDevelopment())
					return;

				var attributes = new ShibbolethAttributeValueCollection()
				{
					new ShibbolethAttributeValue("uid", "uid1234"),
					new ShibbolethAttributeValue("givenName", "Marek"),
					new ShibbolethAttributeValue("sn", "Honc"),
					new ShibbolethAttributeValue("mail", "marek.honc@tul.cz"),
					new ShibbolethAttributeValue("affiliation", "employee@tul.cz;member@tul.cz;alum@tul.cz;faculty@tul.cz")
				};
				options.Events = new ShibbolethEvents
				{
					OnSelectingProcessor = ctx =>
					{
						ctx.Processor = new ShibbolethDevelopmentProcessor(attributes);
						return Task.CompletedTask;
					}
				};
			});
		}

		/// <summary>
		/// Naplánuje spuštění úloh na pozadí.
		/// </summary>
		public static void ScheduleJobs(this WebApplication app)
		{
			// TODO: padá to na 500 - nějaký content type
			app.UseHangfireDashboard("/services", new DashboardOptions()
			{
				Authorization = new[] { new AdminShibbolethAuthorizeAttribute() },
			});

			// Naplánování úloh přes DI
			using (var scope = app.Services.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;

				var syncJob = serviceProvider.GetRequiredService<ISyncJob>();
				var cleanerJob = serviceProvider.GetRequiredService<ICleanerJob>();

				RecurringJob.AddOrUpdate(nameof(SyncJob), () => syncJob.Run(), "* * * * *");
				RecurringJob.AddOrUpdate(nameof(CleanerJob), () => cleanerJob.Run(), "0 0 * * *");
			}
		}
	}
}