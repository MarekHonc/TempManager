using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Rozhraní pro Shibboleth processor.
	/// </summary>
	public interface IShibbolethProcessor
	{
		/// <summary>
		/// Vrací kolekci atributů sessiony od Shibbolethu - header/proměnná.
		/// </summary>
		ShibbolethAttributeValueCollection ExtractAttributeValues(HttpContext context);

		/// <summary>
		/// Vrací zda-li je uživatel ověřený a je dostupná Shibboleth session.
		/// </summary>
		bool IsShibbolethSession(HttpContext context);
	}
}