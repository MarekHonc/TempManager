using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http.Features;
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

			// Kvůli skupinovému editoru práv musím zvýšit limit na počet prvků ve formuláři.
			builder.Services.Configure<FormOptions>(options =>
			{
				options.ValueCountLimit = int.MaxValue;
			});
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
			app.UseHangfireDashboard("/services", new DashboardOptions()
			{
				Authorization = new[] { new AdminShibbolethAuthorizeAttribute() },
				IgnoreAntiforgeryToken = true
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

		/// <summary>
		/// Zaregistruje všechny JS/CSS bundly pro aplikaci.
		/// </summary>
		public static void RegisterBundles(this WebApplicationBuilder builder)
		{
			builder.Services.AddWebOptimizer(pipeline =>
			{
				pipeline.AddCssBundle(
					"/css/bundle.css",
					"/lib/bootstrap/css/bootstrap.min.css",
					"/lib/toastr.js/toastr.min.css",
					"/css/site.min.css"
				);

				pipeline.AddJavaScriptBundle(
					"/js/libraries",
					"/lib/jquery/jquery.min.js",
					"/lib/toastr.js/toastr.min.js",
					"/lib/bootstrap/js/bootstrap.bundle.min.js",
					"/lib/underscore.js/underscore.min.js",
					"/lib/knockout/knockout-latest.min.js",
					"/lib/microsoft-signalr/signalr.min.js",
					"/lib/numeral.js/numeral.min.js",
					"/lib/numeral.js/locales.min.js",
					"/lib/jqueryui/jquery-ui.js",
					"/js/observable.dictionary.js",
					"/js/knockout.custom.js"
				);

				pipeline.AddJavaScriptBundle(
					"/js/front-end",
					"/js/pan.and.zoom.js",
					"/js/models/room.model.js",
					"/js/models/app.model.js"
				);

				// Admin
				pipeline.AddCssBundle(
					"/css/bundle-admin.css",
					"/css/admin.min.css"
				);

				pipeline.AddJavaScriptBundle(
					"/js/autocomplete",
					"/js/models/auto.complete.model.js",
					"/js/models/auto.complete.rights.model.js"
				);

				pipeline.AddJavaScriptBundle(
					"/js/front-end-admin",
					"/js/models/app.feedback.model.js",
					"/js/models/app.history.model.js",
					"/js/models/app.room.values.model.js",
					"/js/models/app.group.rights.editor.js"
				);
			});
		}
	}
}