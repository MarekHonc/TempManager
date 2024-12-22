using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Procesor pro zpracování debugovacího přihlášení.
	/// </summary>
	public class ShibbolethDevelopmentProcessor : IShibbolethProcessor
	{
		public ShibbolethDevelopmentProcessor(ShibbolethAttributeValueCollection attributes, bool isSession = true)
		{
			this.Attributes = attributes;
			this.IsSession = isSession;
		}

		/// <summary>
		/// Vrací atributy přihlášení.
		/// </summary>
		public ShibbolethAttributeValueCollection Attributes
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je aktivní session.
		/// </summary>
		public bool IsSession
		{
			get;
		}

		/// <inheritdoc cref="IShibbolethProcessor.IsShibbolethSession"/>
		public bool IsShibbolethSession(HttpContext context) => this.IsSession;

		/// <inheritdoc cref="IShibbolethProcessor.ExtractAttributeValues"/>
		public ShibbolethAttributeValueCollection ExtractAttributeValues(HttpContext context)
		{
			return this.Attributes;
		}
	}
}