using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace TempManager.KNX.Api
{
	/// <summary>
	/// Služba která se stará o připojení k API.
	/// </summary>
	internal class ApiService : HttpClient, IApiService
	{
		private readonly IApiSettings apiSettings;

		/// <summary>
		/// Vytvoří novou instanci napojení na API.
		/// </summary>
		public ApiService(IApiSettings settings)
		{
			this.apiSettings = settings;
		}

		/// <summary>
		/// Vyzkouší napojení na API.
		/// </summary>
		public async Task<bool> Ping()
		{
			return await GetResponse<bool>(ApiConstants.Ping, json => true);
		}

		/// <summary>
		/// Přečte všechny proměnné z API.
		/// </summary>
		public async Task<ApiVariable[]> GetVariables()
		{
			var endPoint = ApiConstants.GetVariables;
			Func<string, ApiVariable[]> parseResponse = (json) =>
			{
				var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
				var variables = new ApiVariable[data.Count];

				var i = 0;

				foreach (var kvp in data)
				{
					variables[i] = new ApiVariable() { Name = kvp.Key };
					i++;
				}

				return variables;
			};

			// Stáhnu výsledek a zkontroluji, zda api něco vrátilo.
			var result = await GetResponse<ApiVariable[]>(endPoint, parseResponse);
			if (result == null)
				throw new NullReferenceException("Connect to API first!");

			return result;
		}

		/// <summary>
		/// Vrací hodnoty proměnné z API.
		/// </summary>
		public async Task<ApiValue[]> GetValues(ApiVariable variable, bool filterEmpty = true)
		{
			var endPoint = string.Format(ApiConstants.GetVariableValue, variable.Name);
			Func<string, ApiValue[]> parseResponse = (json) =>
			{
				try
				{
					var data = JsonConvert.DeserializeObject<Dictionary<string, ApiValue[]>>(json);

					if (filterEmpty)
					{
						var filteredCollection =
							data[variable.Name].Where(d => !string.IsNullOrEmpty(d.Name)).ToArray();
						return filteredCollection;
					}

					return data[variable.Name] ?? Array.Empty<ApiValue>();
				}
				catch
				{
					return Array.Empty<ApiValue>();
				}
			};

			// Stáhnu výsledek a zkontroluji, zda api něco vrátilo.
			var result = await GetResponse<ApiValue[]>(endPoint, parseResponse);
			if (result == null)
				throw new NullReferenceException("Connect to API first!");

			return result;
		}

		/// <summary>
		/// Promítne změny do API.
		/// </summary>
		public async Task<bool> SetTemperatures(ApiVariable variable, ApiSetTemperature newTemperature)
		{
			// Poskládám endpoint.
			var endpoint = string.Format(
				ApiConstants.SetTemperature,
				variable.Name,
				(newTemperature.ArrayIndex + 1),
				Math.Round(newTemperature.DesiredTemperature, 2)
			);

			// Udělám GET -> chci nahrát pouze 1 hodnotu, při úspěchu vrací prázdnou 200.
			var result = await GetResponse<bool>(endpoint, (json) => true);
			return result;
		}

		#region private helpers

		/// <summary>
		/// Provede GET request na určitý endpoint.
		/// </summary>
		private async Task<T> GetResponse<T>(string endPoint, Func<string, T> parseResponse)
		{
			using (var httpClient = new HttpClient())
			{
				// Header
				var authorization = new AuthenticationHeaderValue(
					"Basic", Base64Encode($"{this.apiSettings.UserName}:{this.apiSettings.Password}")
				);

				// Výchozí nastavení.
				httpClient.BaseAddress = new Uri(this.apiSettings.Url);
				httpClient.DefaultRequestHeaders.Authorization = authorization;

				HttpResponseMessage response;

				// Samotné provedení požadavku.
				try
				{
					response = await httpClient.GetAsync(endPoint);
				}
				catch
				{
					return default(T);
				}

				// Cokoli jiného než 200 vracím výchozí hodnotu.
				if (!response.IsSuccessStatusCode)
					return default(T);

				// Přečtu odpověď jako string.
				var jsonResult = await response.Content.ReadAsStringAsync();

				// A transformuji na požadovaný objekt.
				return parseResponse(jsonResult);
			}
		}

		/// <summary>
		/// Vrací předaný string konvertovaný do base 64.
		/// </summary>
		private static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		#endregion
	}
}