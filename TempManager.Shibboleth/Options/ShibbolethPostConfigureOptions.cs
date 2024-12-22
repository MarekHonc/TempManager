using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Pomocná třída pro správnou inicializaci <see cref="ShibbolethOptions"/>.
	/// </summary>
	public class ShibbolethPostConfigureOptions : IPostConfigureOptions<ShibbolethOptions>
	{
		private readonly IDataProtectionProvider dp;
		private readonly AuthenticationOptions authOptions;

		/// <summary>
		/// Vytvoří třídu pro dokončení konfigurace <see cref="ShibbolethOptions"/>.
		/// </summary>
		public ShibbolethPostConfigureOptions(IOptions<AuthenticationOptions> authOptions, IDataProtectionProvider dataProtection)
		{
			this.dp = dataProtection;
			this.authOptions = authOptions.Value;
		}

		/// <summary>
		/// Dokončí konfiguraci <see cref="ShibbolethOptions"/>.
		/// </summary>
		public void PostConfigure(string? name, ShibbolethOptions options)
		{
			ArgumentNullException.ThrowIfNull(name);
			
			options.SignInScheme ??= this.authOptions.DefaultSignInScheme ?? this.authOptions.DefaultScheme;
			options.DataProtectionProvider ??= this.dp;

			if (options.StateDataFormat == null)
			{
				IDataProtector dataProtector = options.DataProtectionProvider.CreateProtector(typeof(ShibbolethHandler).FullName!, name, "v1");
				options.StateDataFormat = new PropertiesDataFormat(dataProtector);
			}
		}
	}
}