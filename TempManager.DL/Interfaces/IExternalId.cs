namespace TempManager.DL.Interfaces
{
	/// <summary>
	/// Interface označující entitu s externím identifikátorem.
	/// </summary>
	public interface IExternalId : IEntity
	{
		/// <summary>
		/// Vrací externí identifikátor.
		/// </summary>
		string ExternalId
		{
			get;
		}
	}
}