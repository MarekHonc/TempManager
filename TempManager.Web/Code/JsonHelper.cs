using Newtonsoft.Json;

namespace TempManager.Web.Code
{
	/// <summary>
	/// Pomocné metody pro práci s JSONem.
	/// </summary>
	public static class JsonHelper
	{
		/// <summary>
		/// Převede objekt do formátu json.
		/// </summary>
		public static string ToJson(this object obj)
		{
			var settings = new JsonSerializerSettings();

			return JsonConvert.SerializeObject(obj, settings);
		}
	}
}
