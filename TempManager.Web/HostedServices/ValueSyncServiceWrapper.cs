using Microsoft.AspNetCore.SignalR;
using TempManager.BL.Models;
using TempManager.BL.SyncService;
using TempManager.Web.Hubs;

namespace TempManager.Web.HostedServices
{
	/// <summary>
	/// Wraper, spouštějící synchronizaci mezi API a lokální storage.
	/// </summary>
	public class ValueSyncServiceWrapper : IHostedService, IDisposable
	{
		private readonly ILogger<ValueSyncServiceWrapper> logger;
		private readonly IServiceProvider serviceProvider;
		private readonly IHubContext<UpdateHub> hubContext;

		private Timer timer = null;

		public ValueSyncServiceWrapper(IServiceProvider serviceProvider,
			ILogger<ValueSyncServiceWrapper> logger, IHubContext<UpdateHub> hubContext)
		{
			this.logger = logger;
			this.serviceProvider = serviceProvider;
			this.hubContext = hubContext;
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

			// TODO Hangfire? todle je punk

			// Nastavím timeru že se již neopakuje.
			this.timer?.Change(Timeout.Infinite, 0);

			// Task byl ukončen.
			return Task.CompletedTask;
		}

		#region Private helpers
		
		/// <summary>
		/// Provede synchronizaci a všechny data pošle na klienty přes web sockety.
		/// </summary>
		private async void Sync(object state)
		{
			this.logger.LogInformation("Sync started");

			Dictionary<Floor, Room[]> latestValues;

			// Službu používám v usingu.
			using (var scope = this.serviceProvider.CreateScope())
			{
				// Získání závislostí.
				var syncService = scope.ServiceProvider.GetService<IValueSyncService>();

				// 1. synchronizuji podlaží.
				var floors = await syncService.SyncFloors();
				this.logger.LogInformation($"{floors.Length} floors synced.");

				// 2. synchronizuji naměřené teploty.
				latestValues = await syncService.SyncRooms();
			}

			// A všechny natažené hodnoty projedu.
			foreach (var value in latestValues)
			{
				// Lognu i počet natažených místností.
				this.logger.LogInformation($"Floor {value.Key.FriendlyId}: {value.Value.Length} rooms synced.");

				// A pošlu na web sockety.
				// TODO: nefunguje na produkci.
				// TODO: Teď to nebude fungovat protože groups.... možná roomy?
				await this.hubContext.Clients.Group(value.Key.FriendlyId).SendAsync("ReceiveMessage", value.Value);
			}
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