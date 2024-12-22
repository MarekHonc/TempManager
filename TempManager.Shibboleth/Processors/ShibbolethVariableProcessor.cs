using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Shibboleth autentizace v módu "proměnných".
	/// </summary>
	public class ShibbolethVariableProcessor : IShibbolethProcessor
	{
		public ShibbolethVariableProcessor(IShibbolethAttributeCollection attributes)
		{
			this.Attributes = attributes;
		}

		/// <summary>
		/// Vrací Shibboleth atributy z IDP.
		/// </summary>
		protected IShibbolethAttributeCollection Attributes
		{
			get;
		}

		/// <inheritdoc cref="IShibbolethProcessor.IsShibbolethSession"/>
		public bool IsShibbolethSession(HttpContext context)
		{
			// Hledám Shib-Session-Index - tím se indikuje aktivní session.
			return !string.IsNullOrEmpty(context.GetServerVariable(ShibbolethDefaults.VariableShibIndexName));
		}

		/// <inheritdoc cref="IShibbolethProcessor.ExtractAttributeValues"/>
		public ShibbolethAttributeValueCollection ExtractAttributeValues(HttpContext context)
		{
			var attributeValues = new ShibbolethAttributeValueCollection();

			foreach (string attribute in Attributes)
			{
				string value = context.GetServerVariable(attribute);
				if (!string.IsNullOrEmpty(value))
				{
					attributeValues.Add(new ShibbolethAttributeValue(attribute, value));
				}
			}

			return attributeValues;
		}
	}
}