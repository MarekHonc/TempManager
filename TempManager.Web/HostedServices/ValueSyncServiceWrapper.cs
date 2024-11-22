using TempManager.BL.SyncService;

namespace TempManager.Web.HostedServices
{
	/// <summary>
	/// Wraper, spouštějící synchronizaci mezi API a lokální storage.
	/// </summary>
	public class ValueSyncServiceWrapper : IHostedService, IDisposable
	{
		private readonly ILogger<ValueSyncServiceWrapper> logger;
		private Timer? timer = null;

		public ValueSyncServiceWrapper(ILogger<ValueSyncServiceWrapper> logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Spustí službu na pozadí.
		/// </summary>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			this.logger.LogInformation($"{nameof(ValueSyncService)} service started.");

			// Zložím timer, který se stará o spouštění služby.
			this.timer = new Timer(Sync, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

			// Task byl započat.
			return Task.CompletedTask;
		}

		/// <summary>
		/// Zastaví službu na pozadí.
		/// </summary>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			this.logger.LogInformation($"{nameof(ValueSyncService)} service stopped.");

			// Nastavím timeru že se již neopakuje.
			this.timer?.Change(Timeout.Infinite, 0);

			// Task byl ukončen.
			return Task.CompletedTask;
		}

		#region Private helpers
		
		/// <summary>
		/// Provede synchronizaci a všechny data pošle na klienty přes web sockety.
		/// </summary>
		/// <param name="state"></param>
		private void Sync(object? state)
		{
			this.logger.LogInformation($"Sync started");

			// TODO sync
			// TODO rozeslat na web sockety.
		}

		#endregion

		/// <summary>
		/// Uvolní službu z paměti.
		/// </summary>
		public void Dispose()
		{
			this.timer?.Dispose();
		}
	}
}
