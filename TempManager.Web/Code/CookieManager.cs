using Newtonsoft.Json;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Cookie manager pro práci s cookies.
	/// </summary>
	public class CookieManager
	{
		#region Cookies

		/// <summary>
		/// Název cookie, do které se uchová poslední zobrazený typ podlaží.
		/// </summary>
		public const string FloorViewTypeCookieName = "fvt";

		#endregion

		private readonly IHttpContextAccessor httpContextAccessor;

		public CookieManager(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Uloží hodnotu do cookie.
		/// </summary>
		/// <typeparam name="T">Typ hodnoty</typeparam>
		/// <param name="key">Klíč cookie</param>
		/// <param name="value">Hodnota, která se má uložit</param>
		/// <param name="expires">Volitelná doba platnosti</param>
		public void Save<T>(string key, T value, DateTimeOffset? expires = null)
		{
			var options = new CookieOptions
			{
				Expires = expires ?? DateTimeOffset.UtcNow.AddDays(7),
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict
			};

			var serializedValue = JsonConvert.SerializeObject(value);
			this.httpContextAccessor.HttpContext.Response.Cookies.Append(key, serializedValue, options);
		}

		/// <summary>
		/// Získá hodnotu z cookie.
		/// </summary>
		/// <typeparam name="T">Typ hodnoty</typeparam>
		/// <param name="key">Klíč cookie</param>
		/// <param name="defaultValue">Výchozí hodnota pokud cookie není nalezena.</param>
		/// <returns>Hodnota cookie, nebo výchozí hodnota daného typu</returns>
		public T Get<T>(string key, T defaultValue)
		{
			if (this.httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out var serializedValue))
			{
				return JsonConvert.DeserializeObject<T>(serializedValue);
			}

			return defaultValue;
		}

		/// <summary>
		/// Smaže cookie podle klíče.
		/// </summary>
		/// <param name="key">Klíč cookie</param>
		public void Delete(string key)
		{
			this.httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
		}
	}
}