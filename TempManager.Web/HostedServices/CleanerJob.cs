using Hangfire;
using TempManager.DL.Repositories;

namespace TempManager.Web.HostedServices
{
	/// <summary>
	/// Job, starající se o čištění starých hodnot.
	/// </summary>
	public class CleanerJob : ICleanerJob
	{
		/// <summary>
		/// Počet dní jak dlouho se uchovává historie provedených tasků.
		/// </summary>
		private const int JobHistoryKeepDays = 3;

		private readonly ILogger<CleanerJob> logger;
		private readonly IServiceProvider serviceProvider;

		public CleanerJob(IServiceProvider serviceProvider, ILogger<CleanerJob> logger)
		{
			this.logger = logger;
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public async Task Run()
		{
			this.logger.LogInformation("Cleaning old data");

			// Službu používám v usingu.
			using (var scope = this.serviceProvider.CreateScope())
			{
				// Získání závislostí.
				var repositoriesFactory = scope.ServiceProvider.GetService<RepositoriesFactory>();
				await repositoriesFactory.DeleteOldValues();
			}

			this.logger.LogInformation("Cleaning old jobs");

			var monitoringApi = JobStorage.Current.GetMonitoringApi();
			var jobs = monitoringApi.SucceededJobs(0, int.MaxValue);

			foreach (var job in jobs)
			{
				if (job.Value.SucceededAt < DateTime.UtcNow.AddDays(-3))
				{
					BackgroundJob.Delete(job.Key);
					this.logger.LogInformation($"Job {job.Key} deleted");
				}
			}
		}
	}
}
