using TempManager.KNX.Api;

namespace TempManager.ApiConsoleTest
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var settings = new ApiSettings(@"http://147.230.77.197/TecoApi/", "test", "test");
			var api = ApiServiceFactory.GetService(settings);

			if (!await api.Ping())
			{
				Console.WriteLine("Api is not available!");
				return;
			}

			var variables = await api.GetVariables();

			foreach (var variable in variables)
			{
				Console.WriteLine($"Found variable: {variable.Name}");

				var values = await api.GetValues(variable);

				foreach (var value in values)
				{
					Console.WriteLine($"Name: {value.Name}, Temp: {value.Temp}, RH: {value.Rh} CO2: {value.CO2}, Desired Temp: {value.DesiredTemperature}, Valve Open: {value.ValveOpen}");
				}
			}
		}
	}
}