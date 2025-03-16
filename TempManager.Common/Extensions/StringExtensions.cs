namespace TempManager.Common
{
	/// <summary>
	/// Extension metody pro práci se stringy.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Detekuje skupiny místností z názvu místnosti.
		/// </summary>
		public static Groups DetectGroups(this string text)
		{
			// Prázdný string -> není žádná skupina.
			if (string.IsNullOrEmpty(text))
				return Groups.None;

			text = text.ToLower();
			var result = Groups.None;

			// Pokud obsahuje lomítko, jedná se o učebnu.
			if (text.Contains("/"))
			{
				result |= Groups.Room;
			}

			// A teď detekce ze stringu.
			foreach (var group in Enum.GetValues<Groups>())
			{
				// Schodu na room, nebo none přeskakuji.
				if (group == Groups.None || group == Groups.Room)
					continue;

				// Koukám na stringovou shodu.
				var groupLower = group.ToString().ToLower();
				if (text.Contains(groupLower))
				{
					result |= group;
				}
			}

			return result;
		}
	}
}
