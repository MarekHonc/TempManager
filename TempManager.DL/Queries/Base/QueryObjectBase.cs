using Microsoft.EntityFrameworkCore;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Queries
{
	/// <summary>
	/// Bázová implementace třídy, která realizuje dotazy nad databází.
	/// </summary>
	/// <typeparam name="T">Typ entity, nad kterou jsou realizovány dotazy.</typeparam>
	public abstract class QueryObjectBase<T> : IQueryObjectBase<T> where T : class, IEntity
	{
		/// <summary>
		/// Vazby, které se při vyhodnocování query stáhnou také.
		/// </summary>
		public List<string> Includes
		{
			get;
		} = new List<string>();

		/// <summary>
		/// Příznak, zda-li má EF trackovat změny na stažených objektech.
		/// </summary>
		public bool ReadOnly
		{
			get;
			protected set;
		}

		/// <summary>
		/// Nastaví tuto query jako readonly, takže výsledky vrácené touto query nebudou trackované EF.
		/// (Dotaz bude rychlejší a objekty se nepodaří omylem změnit.)
		/// </summary>
		/// <returns>Vrací sama sebe.</returns>
		public QueryObjectBase<T> SetReadonly()
		{
			this.ReadOnly = true;
			return this;
		}

		/// <summary>
		/// Umožní includovat další objekty součástí této jedné property.
		/// (aby se nemusel používat lazy loading pro každou entitu, když je potřeba)
		/// </summary>
		/// <param name="include">cesta(y) k navigačním properties, které se mají načíst už v této query.</param>
		/// <returns>Vrací sama sebe.</returns>
		public QueryObjectBase<T> Include(params string[] include)
		{
			this.Includes.AddRange(include);
			return this;
		}

		/// <summary>
		/// Vrací počet řádků odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>Počet řádků odpovídajících dotazu.</returns>
		Task<int> IQueryObjectBase<T>.Count(TempManagerContext dbContext)
		{
			return Query(dbContext).CountAsync();
		}

		/// <summary>
		/// Vrací, zda existuje nějaký záznam odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns><c>true</c> pokud existuje nějaký záznam, jinak <c>false</c>.</returns>
		Task<bool> IQueryObjectBase<T>.IsAny(TempManagerContext dbContext)
		{
			return Query(dbContext).AnyAsync();
		}

		/// <summary>
		/// Vrací záznamy odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>Záznamy odpovídající dotazu.</returns>
		async Task<IReadOnlyCollection<T>> IQueryObjectBase<T>.Fetch(TempManagerContext dbContext)
		{
			return await Query(dbContext).ToListAsync();
		}

		/// <summary>
		/// Vrací první záznam odpovídající dotazu.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>
		/// Záznam odpovídající dotazu, nebo null pokud dotazu neodpovídá žádný záznam.
		/// </returns>
		Task<T> IQueryObjectBase<T>.FetchOne(TempManagerContext dbContext)
		{
			return Query(dbContext).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Vrací záznamy odpovídající dotazu omezené daným počtem.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <param name="count">Počet záznamů (nebo méně pokud jich tolik není) kolik se vrátí.</param>
		/// <returns>Záznamy odpovídající dotazu.</returns>
		async Task<IReadOnlyCollection<T>> IQueryObjectBase<T>.FetchCount(TempManagerContext dbContext, int count)
		{
			return await Query(dbContext).Take(count).ToListAsync();
		}


		/// <summary>
		/// Do vytvoří LINQ dotaz, který získá požadovaná data.
		/// </summary>
		/// <param name="dbContext">Databázový kontext pro provedení query.</param>
		/// <returns>Dotaz, který získá z databáze požadovaná data.</returns>
		protected abstract IQueryable<T> CreateQuery(TempManagerContext dbContext);

		/// <summary>
		/// Vytvoří query s ohledem na další parametry v tomto objektu <see cref="ReadOnly"/>, <see cref="Includes"/>.
		/// </summary>
		/// <param name="dbContext">Databázový kontext.</param>
		/// <returns>Dotaz, který získá z databáze požadovaná data.</returns>
		private IQueryable<T> Query(TempManagerContext dbContext)
		{
			// Nechám poskládat dotaz.
			var query = CreateQuery(dbContext);

			// Nakonec ještě doupravím (Include, Readonly atd...).
			query = PrepareQuery(query);

			// A vratím.
			return query;
		}

		/// <summary>
		/// Finálně Připraví(upraví) předanou query na provedení dotazu do DB.
		/// (S použitím ReadOnly a Include)
		/// </summary>
		protected IQueryable<T> PrepareQuery(IQueryable<T> query)
		{
			if (this.Includes.Count > 0)
			{
				foreach (var path in this.Includes)
				{
					query = query.Include(path);
				}
			}
			if (this.ReadOnly)
			{
				query = query.AsNoTracking();
			}
			return query;
		}
	}
}