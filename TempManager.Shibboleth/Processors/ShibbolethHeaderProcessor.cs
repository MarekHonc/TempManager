using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Shibboleth autentizace v módu "hlaviček" přímo v požadavku.
	/// </summary>
	public class ShibbolethHeaderProcessor : IShibbolethProcessor
	{
		public ShibbolethHeaderProcessor(IShibbolethAttributeCollection attributes)
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
			// Hladám hlavičku ShibSessionIndex - tím se indikuje aktivní session.
			if (context.Request.Headers.TryGetValue(ShibbolethDefaults.HeaderShibIndexName, out var shibIndex))
				return !StringValues.IsNullOrEmpty(shibIndex);

			return false;
		}

		/// <inheritdoc cref="IShibbolethProcessor.ExtractAttributeValues"/>
		public ShibbolethAttributeValueCollection ExtractAttributeValues(HttpContext context)
		{
			IHeaderDictionary headers = context.Request.Headers;

			var attributeValues = new ShibbolethAttributeValueCollection();

			foreach (string attribute in Attributes)
			{
				if (headers.ContainsKey(attribute))
				{
					attributeValues.Add(new ShibbolethAttributeValue(attribute, headers[attribute]));
				}
			}

			return attributeValues;
		}
	}
}