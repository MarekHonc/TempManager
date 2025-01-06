using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Extension metody pro sestavení identity provideru ze shibboleth.
	/// </summary>
	public static class ShibbolethExtensions
	{
		/// <summary>
		/// Povolí autentizaci pomocí Shibboleth ve schématu <see cref="ShibbolethDefaults.AuthenticationScheme"/>
		/// </summary>
		public static AuthenticationBuilder AddShibboleth(this AuthenticationBuilder builder, Action<ShibbolethOptions> configureOptions)
			=> builder.AddShibboleth(ShibbolethDefaults.AuthenticationScheme, displayName: null, configureOptions);

		/// <summary>
		/// Povolí autentizaci pomocí Shibboleth ve schématu <see cref="ShibbolethDefaults.AuthenticationScheme"/>
		/// </summary>
		private static AuthenticationBuilder AddShibboleth(this AuthenticationBuilder builder,
			string authenticationScheme,
			string displayName,
			Action<ShibbolethOptions> configureOptions)
		{
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ShibbolethOptions>, ShibbolethPostConfigureOptions>());
			return builder.AddScheme<ShibbolethOptions, ShibbolethHandler>(authenticationScheme, displayName, configureOptions);
		}
	}
}