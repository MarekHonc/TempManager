using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Mapování claimù s pøidaným "pøed procesore" který nejprve hodnotu upraví.
	/// </summary>
	public class ShibbolethCustomClaimAction : ShibbolethAttributeClaimAction
	{
		/// <summary>
		/// Vytvoøí novou mapovací akci s konverzní funkcí.
		/// </summary>
		public ShibbolethCustomClaimAction(string claimType, string valueType, string attributeName,
			Func<string, string?> processor)
			: base(claimType, valueType, attributeName)
		{
			this.Processor = processor;
		}

		/// <summary>
		/// Vrací funkci, která zpracuje data ze Shibbolethu.
		/// </summary>
		public Func<string, string?> Processor
		{
			get;
		}

		/// <inheritdoc cref="Run"/>
		public override void Run(ShibbolethAttributeValueCollection userData, ClaimsIdentity identity, string issuer)
		{
			var value = GetValue(userData, AttributeName);

			if (string.IsNullOrEmpty(value))
				return;
			
			var processedValue = this.Processor(value!);
			if (!string.IsNullOrEmpty(processedValue))
				identity.AddClaim(new Claim(ClaimType, processedValue, ValueType, issuer));
		}
	}
}