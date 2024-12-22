using Microsoft.Extensions.Logging;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Rozšíření pro logování chování Shibbolethu.
	/// </summary>
	public static partial class ShibbolethLoggingExtensions
	{
		[LoggerMessage(990, LogLevel.Information, "Error from ShibbolethAuthentication: {ErrorMessage}.", EventName = "ShibbolethAuthenticationFailed")]
		public static partial void ShibbolethAuthenticationFailed(this ILogger logger, string errorMessage);

		[LoggerMessage(5, LogLevel.Debug, "The SigningIn event returned Handled.", EventName = "SignInHandled")]
		public static partial void SignInHandled(this ILogger logger);

		[LoggerMessage(6, LogLevel.Debug, "The SigningIn event returned Skipped.", EventName = "SignInSkipped")]
		public static partial void SignInSkipped(this ILogger logger);
	}
}