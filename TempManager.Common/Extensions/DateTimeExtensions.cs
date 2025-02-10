namespace TempManager.Common.Extensions
{
	/// <summary>
	/// Extension metody pro práci s datetime
	/// </summary>
	public static class DateTimeExtensions
	{
		public static string ToShortDateTime(this DateTimeOffset dateTime)
		{
			return dateTime.ToLocalTime().ToString("dd. MM. yyyy HH:mm");
		}
	}
}