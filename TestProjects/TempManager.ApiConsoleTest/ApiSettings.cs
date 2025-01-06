using TempManager.KNX.Api;

namespace TempManager.ApiConsoleTest
{
	internal class ApiSettings : IApiSettings
	{
		public ApiSettings(string url, string userName, string password)
		{
			Url = url;
			UserName = userName;
			Password = password;
		}

		public string Url
		{
			get;
		}


		public string UserName
		{
			get;
		}


		public string Password
		{
			get;
		}
	}
}