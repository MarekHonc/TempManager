namespace TempManager.KNX.Api
{
	/// <summary>
	/// Interface, nesoucí nastavení pro komunikaci s API, ze kterého se tahají data.
	/// </summary>
	public interface IApiSettings
	{
		/// <summary>
		/// Vrací URL adresu, na které se API nachází.
		/// </summary>
		string Url
		{
			get;
		}

		/// <summary>
		/// Vrací uživatelské jméno.
		/// </summary>
		string UserName
		{
			get;
		}

		/// <summary>
		/// Vrací heslo.
		/// </summary>
		string Password
		{
			get;
		}
	}
}
