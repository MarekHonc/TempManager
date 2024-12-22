using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kontext držený při zpracování atributu.
	/// </summary>
	public class ShibbolethProcessorSelectionContext : ResultContext<ShibbolethOptions>
	{
		public ShibbolethProcessorSelectionContext(HttpContext context, AuthenticationScheme scheme, ShibbolethOptions options)
			: base(context, scheme, options)
		{
		}

		/// <summary>
		/// Vrací nebo nastavuje processor hodnoty.
		/// </summary>
		public IShibbolethProcessor? Processor
		{
			get;
			set;
		}
	}
}