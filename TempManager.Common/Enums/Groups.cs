namespace TempManager.Common
{
	/// <summary>
	/// Skupiny místností.
	/// </summary>
	[Flags]
	public enum Groups
	{
		/// <summary>
		/// Místnost nepatří do žádné skupiny.
		/// </summary>
		None = 0,

		/// <summary>
		/// Učebna.
		/// </summary>
		Room = 1,

		/// <summary>
		/// Pracoviště MTI.
		/// </summary>
		MTI = 1 << 1,
		
		/// <summary>
		/// Pracoviště NTI.
		/// </summary>
		NTI = 1 << 2,
		
		/// <summary>
		/// Pracoviště ITE.
		/// </summary>
		ITE = 1 << 3,

		/// <summary>
		/// Pracoviště OIS
		/// </summary>
		OIS = 1 << 4,

		/// <summary>
		/// Děkanát + sekretariát.
		/// </summary>
		FM = 1 << 5,
	}
}
