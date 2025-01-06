using TempManager.KNX.Api;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Nastavení připojení na API.
	/// </summary>
	public class ApiSettings : IApiSettings
	{
		public ApiSettings(IConfiguration configuration)
		{
			this.Url = configuration["ApiSettings:Url"];
			this.UserName = configuration["ApiSettings:UserName"];
			this.Password = configuration["ApiSettings:Password"];
		}

		/// <summary>
		/// Vrací url napojení na API.
		/// </summary>
		public string Url
		{
			get;
		}

		/// <summary>
		/// Vrací přihlašovací jméno k API.
		/// </summary>
		public string UserName
		{
			get;
		}

		/// <summary>
		/// Vrací heslo k API.
		/// </summary>
		public string Password
		{
			get;
		}
	}
}