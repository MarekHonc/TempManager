using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TempManager.Shibboleth
{
	/// <summary>
	/// Handler starající se o autentizaci skrze Shibbo.
	/// </summary>
	public class ShibbolethHandler : AuthenticationHandler<ShibbolethOptions>, IAuthenticationRequestHandler, IAuthenticationSignInHandler
	{
		private Task<HandleRequestResult> extractShibbolethDataTask;
		private const string AuthSchemeKey = ".AuthScheme";

		/// <summary>
		/// Vytvoří novou instanci handleru pro autentizaci.
		/// </summary>
		public ShibbolethHandler(IOptionsMonitor<ShibbolethOptions> options, ILoggerFactory logger, UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		/// <summary>
		/// The authentication scheme used by default for signin.
		/// </summary>
		protected string SignInScheme => Options.SignInScheme;

		/// <summary>
		/// Zpracování eventů od Shibba.
		/// </summary>
		protected new ShibbolethEvents Events
		{
			get
			{
				return (ShibbolethEvents)base.Events!;
			}
			set
			{
				base.Events = value;
			}
		}

		/// <summary>
		/// Vytvoří všechny potřebné eventy.
		/// </summary>
		/// <returns></returns>
		protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new ShibbolethEvents());

		/// <summary>
		/// Vrací, zda-li by měl být požadavek zpracován pomocí <see cref="HandleRequestAsync" />.
		/// </summary>
		public virtual Task<bool> ShouldHandleRequestAsync()
			=> Task.FromResult(!Options.UseChallenge || (Options.UseChallenge && Options.CallbackPath == Request.Path));

		/// <summary>
		/// Zpracuje aktuální požadavek do aplikace.
		/// Vrací true, pokud autentizace je zpracována.
		/// </summary>
		public virtual async Task<bool> HandleRequestAsync()
		{
			if (!await ShouldHandleRequestAsync())
				return false;

			AuthenticationTicket ticket = null;
			Exception exception = null;
			AuthenticationProperties properties = null;
			try
			{
				// RemoteAuthenticationHandler
				// https://github.com/dotnet/aspnetcore/blob/3f16e780a3a708d1d63fb4458157178c7fab5f39/src/Security/Authentication/Core/src/RemoteAuthenticationHandler.cs#L87
				HandleRequestResult authResult = await EnsureShibbolethSession();
				if (authResult == null)
				{
					exception = new InvalidOperationException("Invalid return state, unable to redirect.");
				}
				else if (authResult.Handled)
				{
					return true;
				}
				else if (authResult.Skipped || authResult.None)
				{
					return false;
				}
				else if (!authResult.Succeeded)
				{
					exception = authResult.Failure ?? new InvalidOperationException("Invalid return state, unable to redirect.");
					properties = authResult.Properties;
				}

				ticket = authResult?.Ticket;
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			if (exception != null)
			{
				Logger.ShibbolethAuthenticationFailed(exception.Message);
				var errorContext = new ShibbolethFailureContext(Context, Scheme, Options, exception)
				{
					Properties = properties
				};
				await Events.ShibbolethFailure(errorContext);

				if (errorContext.Result != null)
				{
					if (errorContext.Result.Handled)
					{
						return true;
					}
					else if (errorContext.Result.Skipped)
					{
						return false;
					}
					else if (errorContext.Result.Failure != null)
					{
						throw new AuthenticationFailureException("An error was returned from the ShibbolethFailure event.", errorContext.Result.Failure);
					}
				}

				if (errorContext.Failure != null)
				{
					throw new AuthenticationFailureException("An error was encountered while handling the Shibboleth login.", errorContext.Failure);
				}
			}

			// Tady vím že mám ticket.
			Debug.Assert(ticket != null);
			var ticketContext = new ShibbolethTicketReceivedContext(Context, Scheme, Options, ticket)
			{
				ReturnUri = Request.Query[Options.ReturnUrlParameter]
			};

			ticket.Properties.RedirectUri = null;

			// Uchovávám i název schématu, to se ověřuje později.
			ticketContext.Properties!.Items[AuthSchemeKey] = Scheme.Name;

			await Events.TicketReceived(ticketContext);

			if (ticketContext.Result != null)
			{
				if (ticketContext.Result.Handled)
				{
					Logger.SignInHandled();
					return true;
				}
				else if (ticketContext.Result.Skipped)
				{
					Logger.SignInSkipped();
					return false;
				}
			}

			await Context.SignInAsync(SignInScheme, ticketContext.Principal!, ticketContext.Properties);

			// Výchozí redirect je "/".
			if (string.IsNullOrEmpty(ticketContext.ReturnUri))
			{
				ticketContext.ReturnUri = BuildRedirectUri("/");
			}

			// Pokud je celá stránka chráněna, podstrčím že výsledkem je false, aby proběhla i metoda AuthenticateAsync().
			if (!Options.UseChallenge)
				return false;

			Response.Redirect(ticketContext.ReturnUri);
			return true;
		}

		/// <summary>
		/// Zajistí že Shibboleth session existuje.
		/// </summary>
		/// <remarks>
		/// CookieAuthenticationHandler
		/// https://github.com/dotnet/aspnetcore/blob/3f16e780a3a708d1d63fb4458157178c7fab5f39/src/Security/Authentication/Cookies/src/CookieAuthenticationHandler.cs#L82
		/// </remarks>
		private Task<HandleRequestResult> EnsureShibbolethSession()
		{
			// Hodnoty stačí získat pouze jednou.
			this.extractShibbolethDataTask ??= ExamineForShibbolethSession();
			return this.extractShibbolethDataTask;
		}

		/// <summary>
		/// Prozkoumá session z Shibbolethu a vytáhne všechny hodnoty.
		/// </summary>
		private async Task<HandleRequestResult> ExamineForShibbolethSession()
		{
			IShibbolethProcessor shibbolethProcessor;
			IShibbolethAttributeCollection shibbolethAttributes = Options.ShibbolethAttributes;

			// Manuální výběr procesoru hodnot - typicky pro debug.
			var processorSelectionContext = new ShibbolethProcessorSelectionContext(Context, Scheme, Options);
			await Events.ShibbolethProcessorSelection(processorSelectionContext);

			// Externí procesor dat -> použiji ten.
			shibbolethProcessor = processorSelectionContext.Processor;

			if (shibbolethProcessor == null || !shibbolethProcessor.IsShibbolethSession(Context))
			{
				// Nejprve hledám proměnné, je to bezpečnější a více doporučované.
				// https://wiki.shibboleth.net/confluence/display/SP3/AttributeAccess#AttributeAccess-ServerVariables
				shibbolethProcessor = new ShibbolethVariableProcessor(shibbolethAttributes);

				// Je Shibbo povolené? Případně v header módu?
				if (!shibbolethProcessor.IsShibbolethSession(Context))
				{
					// Shibbo je v header módu?
					shibbolethProcessor = new ShibbolethHeaderProcessor(shibbolethAttributes);

					if (!shibbolethProcessor.IsShibbolethSession(Context))
					{
						// Shibbo neposílá ani headery ani proměnné - musí to odchytit něco dalšího?
						return HandleRequestResult.NoResult();
					}
				}
			}

			var identity = new ClaimsIdentity(ClaimsIssuer);
			ShibbolethAttributeValueCollection userData = shibbolethProcessor.ExtractAttributeValues(Context);

			IQueryCollection query = Request.Query;

			StringValues state = query["state"];
			AuthenticationProperties properties = Options.StateDataFormat.Unprotect(state);
			properties ??= new AuthenticationProperties();

			return HandleRequestResult.Success(await CreateTicketAsync(identity, properties, userData));
		}

		/// <summary>
		/// Prohledá headery zda-li obsahují atributy z Shibba, pokud obsahuje bude vytvořena příslušná identita.
		/// </summary>
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var endpoint = Context.GetEndpoint();
			if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
			{
				return AuthenticateResult.NoResult();
			}

			// Při využívání challenge se o udržení hodnot stará jiný handler, typicky cookie.
			if (Options.UseChallenge)
			{
				// https://github.com/dotnet/aspnetcore/blob/f98b1df4aca8b0919173a4a96f0b094a0c011308/src/Security/Authentication/Core/src/RemoteAuthenticationHandler.cs#L195
				AuthenticateResult result = await Context.AuthenticateAsync(SignInScheme);
				if (result != null)
				{
					if (result.Failure != null)
					{
						return result;
					}

					// Schéma může být sdílený.
					AuthenticationTicket ticket = result.Ticket;
					if (ticket != null && ticket.Principal != null && ticket.Properties != null &&
					    ticket.Properties.Items.TryGetValue(AuthSchemeKey, out string authenticatedScheme) &&
						string.Equals(Scheme.Name, authenticatedScheme, StringComparison.Ordinal))
					{
						return AuthenticateResult.Success(new AuthenticationTicket(ticket.Principal,
							ticket.Properties, Scheme.Name));
					}

					return AuthenticateResult.NoResult();
				}
			}

			// Kontrola platné shibboleth session.
			try
			{
				HandleRequestResult result = await EnsureShibbolethSession();
				return result;
			}
			catch (Exception ex)
			{
				var authenticationFailedContext = new ShibbolethFailureContext(Context, Scheme, Options, ex)
				{
					Failure = ex
				};

				await Events.ShibbolethFailure(authenticationFailedContext);
				if (authenticationFailedContext.Result != null)
				{
					return authenticationFailedContext.Result;
				}

				throw;
			}
		}

		/// <summary>
		/// Vytvoří <see cref="AuthenticationTicket"/> s hodnotami a atributy z Shibbolethu.
		/// </summary>
		protected virtual async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, ShibbolethAttributeValueCollection userData)
		{
			foreach (var action in Options.ClaimActions)
			{
				action.Run(userData, identity, Options.ClaimsIssuer ?? Scheme.Name);
			}

			var context = new ShibbolethCreatingTicketContext(Context, Scheme, Options, new ClaimsPrincipal(identity), properties, userData);
			await Events.CreatingTicket(context);

			return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
		}

		/// <summary>
		/// Zpracuje challenge po přihlášení.
		/// </summary>
		protected override Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			// Pokud challenge nepouživám, nebo nemám callback tak neřeším nic.
			if (!Options.UseChallenge || !Options.CallbackPath.HasValue)
				return base.HandleChallengeAsync(properties);

			// Pokud mám redirect url, tak poskládám to.
			if (string.IsNullOrEmpty(properties.RedirectUri))
				properties.RedirectUri = BuildRedirectUri(OriginalPath + Request.QueryString);

			var authorizationEndpoint = BuildChallengeUrl(properties, properties.RedirectUri);

			var redirectContext = new RedirectContext<ShibbolethOptions>(
				Context, Scheme, Options,
				properties, authorizationEndpoint
			);

			return Events.RedirectToAuthorizationEndpoint(redirectContext);
		}

		/// <summary>
		/// Vytvoří challenge URL - obdobný mechanismus jako např. OAuth.
		/// </summary>
		/// <remarks>Inspirace OAuthHandler.cs
		/// https://github.com/dotnet/aspnetcore/blob/6196f76672ed4a4415f7a12e8ae17b8212ebf462/src/Security/Authentication/OAuth/src/OAuthHandler.cs#L300
		/// </remarks>
		protected string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
		{
			var parameters = new Dictionary<string, string>()
			{
				{ Options.ReturnUrlParameter, redirectUri }
			};

			parameters["state"] = Options.StateDataFormat.Protect(properties);
			var authorizationEndpoint = OriginalPathBase + Options.CallbackPath;

			return QueryHelpers.AddQueryString(authorizationEndpoint, parameters!);
		}

		/// <inheritdoc cref="SignInAsync"/>
		public virtual Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
		{
			// forward the sign-in if this is a challenge
			if (Options.UseChallenge)
				return Context.SignInAsync(Options.SignInScheme, user, properties);

			// Stánky jsou celé zabezpečené - přihlášení je "fake"
			return Task.CompletedTask;
		}

		/// <inheritdoc cref="SignOutAsync"/>
		public Task SignOutAsync(AuthenticationProperties properties)
		{
			if (Options.UseChallenge)
				return Context.SignOutAsync(Options.SignInScheme, properties);

			// Stánky jsou celé zabezpečené - odhlášení je "fake"
			return Task.CompletedTask;
		}
	}
}