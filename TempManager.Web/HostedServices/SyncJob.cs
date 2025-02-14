using Microsoft.AspNetCore.SignalR;
using TempManager.BL.Models;
using TempManager.BL.SyncService;
using TempManager.Web.Hubs;

namespace TempManager.Web.HostedServices
{
	/// <summary>
	/// Job na synchronizaci hodnot z PLC.
	/// </summary>
	public class SyncJob : ISyncJob
	{
		private readonly ILogger<SyncJob> logger;
		private readonly IServiceProvider serviceProvider;
		private readonly IHubContext<UpdateHub> hubContext;

		public SyncJob(IServiceProvider serviceProvider,
			ILogger<SyncJob> logger,
			IHubContext<UpdateHub> hubContext
		)
		{
			this.logger = logger;
			this.serviceProvider = serviceProvider;
			this.hubContext = hubContext;
		}

		/// <inheritdoc />
		public async Task Run()
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
				await this.hubContext.Clients.All.SendAsync("ReceiveMessage", value.Value);
			}
		}
	}
}
