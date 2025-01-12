using Microsoft.AspNetCore.SignalR;

namespace TempManager.Web.Hubs
{
	/// <summary>
	/// Hub pro real-time komunikaci s klienty.
	/// </summary>
	public class UpdateHub : Hub
	{
		/// <summary>
		/// Přidání připojení do skupiny.
		/// </summary>
		public async Task Join(string group)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, group);
		}

		/// <summary>
		/// Odebrání připojení ze skupiny.
		/// </summary>
		public async Task Leave(string group)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
		}

		/// <summary>
		/// Při připojení do skupiny kontroluji, zda-li je uživatel přihlášen.
		/// Aby mi tam nechodil kde kdo.
		/// </summary>
		public override Task OnConnectedAsync()
		{
			var userName = Context?.User?.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
				Context.Abort();

			return base.OnConnectedAsync();
		}
	}
}