using Microsoft.AspNetCore.Authentication;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Eventy vyvolávané skrze <see cref="ShibbolethHandler"/> aby byla možná vytvořit kontrola nad celým přihlášením.
	/// </summary>
	public class ShibbolethEvents
	{
		/// <summary>
		/// Vrací nebo nastavuje funkci vyvolanou po ověření.
		/// </summary>
		public Func<ShibbolethCreatingTicketContext, Task> OnCreatingTicket
		{
			get;
			set;
		} = context => Task.CompletedTask;

		/// <summary>
		/// Vrací nebo nastavuje funkci, vyvolanou po vyvolání redirectu.
		/// </summary>
		public Func<RedirectContext<ShibbolethOptions>, Task> OnRedirectToAuthorizationEndpoint
		{
			get;
			set;
		} = context =>
		{
			context.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		};

		/// <summary>
		/// Vrací nebo nastavuje funkci vyvolanou při výběru atributu.
		/// </summary>
		public Func<ShibbolethProcessorSelectionContext, Task> OnSelectingProcessor
		{
			get;
			set;
		} = context => Task.CompletedTask;

		/// <summary>
		/// Vrací nebo nastavuje funkci, která se zavolá pokud při komunikaci se Shibbolethem vznikla výjimka.
		/// </summary>
		public Func<ShibbolethFailureContext, Task> OnShibbolethFailure
		{
			get;
			set;
		} = context => Task.CompletedTask;

		/// <summary>
		/// Vrací nebo nastavuje funkci, vyvolanou po obdržení ticketu.
		/// </summary>
		public Func<ShibbolethTicketReceivedContext, Task> OnTicketReceived
		{
			get;
			set;
		} = context => Task.CompletedTask;

		/// <summary>
		/// Funkce vyvolána po nalezení dat Shibbolethu v sessioně.
		/// </summary>
		public virtual Task CreatingTicket(ShibbolethCreatingTicketContext context)
			=> this.OnCreatingTicket(context);

		/// <summary>
		/// Funkce vyvolána po výběru atributu.
		/// </summary>
		public virtual Task ShibbolethProcessorSelection(ShibbolethProcessorSelectionContext context)
			=> this.OnSelectingProcessor(context);

		/// <summary>
		/// Funkce vyvolána po challangi, která vyvolá přesměrování na autorizaci.
		/// </summary>
		public virtual Task RedirectToAuthorizationEndpoint(RedirectContext<ShibbolethOptions> context)
			=> this.OnRedirectToAuthorizationEndpoint(context);

		/// <summary>
		/// Funkce vyvolána po selhání na straně Shibbolethu.
		/// </summary>
		public virtual Task ShibbolethFailure(ShibbolethFailureContext context)
			=> this.OnShibbolethFailure(context);


		/// <summary>
		/// Funkce vyvolána při přijmu autentizačního ticketu.
		/// </summary>
		public virtual Task TicketReceived(ShibbolethTicketReceivedContext context)
			=> this.OnTicketReceived(context);
	}
}