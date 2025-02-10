namespace TempManager.Web.HostedServices
{
	/// <summary>
	/// Rozhraní pro běh úlohy na pozadí.
	/// </summary>
	public interface IBackgroundJob
	{
		/// <summary>
		/// Spustí úlohu na pozadí.
		/// </summary>
		Task Run();
	}

	/// <summary>
	/// Job pro synchronizaci hodnot.
	/// </summary>
	public interface ISyncJob : IBackgroundJob { }

	/// <summary>
	/// Job s čistícímu úlohami.
	/// </summary>
	public interface ICleanerJob : IBackgroundJob { }
}