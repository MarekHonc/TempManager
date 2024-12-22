using System.Security.Claims;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Mapování claimù s pøidaným "pøed procesore" který nejprve hodnotu upraví a možností mít vícero hodnot.
	/// </summary>
	public class ShibbolethCustomMultiValueClaimAction : ShibbolethAttributeClaimAction
	{
		/// <summary>
		/// Vytvoøí novou mapovací akci s konverzní funkcí a možností mít více záznamù.
		/// </summary>
		public ShibbolethCustomMultiValueClaimAction(string claimType, string valueType, string attributeName, Func<string, IEnumerable<string>> processor)
			: base(claimType, valueType, attributeName)
		{
			this.Processor = processor;
		}

		/// <summary>
		/// The Func that will be called to process a value from the given Shibboleth user data
		/// </summary>
		public Func<string, IEnumerable<string>> Processor
		{
			get;
		}

		/// <inheritdoc cref="Run"/>
		public override void Run(ShibbolethAttributeValueCollection userData, ClaimsIdentity identity, string issuer)
		{
			var value = GetValue(userData, AttributeName);

			if (string.IsNullOrEmpty(value))
				return;
			
			var processedValues = this.Processor(value!);
			foreach (string processedValue in processedValues)
			{
				if (!string.IsNullOrEmpty(processedValue))
					identity.AddClaim(new Claim(ClaimType, processedValue, ValueType, issuer));
			}
		}
	}
}