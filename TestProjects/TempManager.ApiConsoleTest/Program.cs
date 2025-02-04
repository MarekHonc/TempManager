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

			var setTemperatures = new List<ApiSetTemperature>();
			ApiVariable toSet = null;

			foreach (var variable in variables)
			{
				Console.WriteLine($"Found variable: {variable.Name}");

				var useSetTemperature = false;
				var values = await api.GetValues(variable, filterEmpty: false);

				if (variable.Name == "_TEST_")
				{
					useSetTemperature = true;
					toSet = variable;
				}

				for (var index = 0; index < values.Length; index++)
				{
					var value = values[index];

					if (useSetTemperature)
					{
						setTemperatures.Add(new ApiSetTemperature(index, (index % 10) + 20));
					}


					Console.WriteLine($"Name: {value.Name}, Temp: {value.Temp}, RH: {value.Rh} CO2: {value.CO2}, Desired Temp: {value.DesiredTemperature}, Valve Open: {value.ValveOpen}");
				}
			}

			foreach (var setTemperature in setTemperatures)
			{
				Console.WriteLine($"Set temperature at {setTemperature.ArrayIndex} to {setTemperature.DesiredTemperature}");
				await api.SetTemperatures(toSet, setTemperature);
			}
		}
	}
}