using Microsoft.EntityFrameworkCore;
using TempManager.Common;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Repositories
{
	/// <summary>
	/// Generický repozitář pro konkrétní tabulku.
	/// </summary>
	/// <typeparam name="T">Tabulka, se kterou se pracuje.</typeparam>
	public class BaseRepository<T> : IRepository<T> where T : class, IEntity
	{
		protected readonly TempManagerContext context;

		public BaseRepository(TempManagerContext context)
		{
			this.context = context;
		}

		/// <inheritdoc cref="FetchById"/>
		public async Task<T> FetchById(params object[] id)
		{
			return await this.context.Set<T>().FindAsync(id);
		}

		/// <inheritdoc cref="FetchAll"/>
		public async Task<IReadOnlyCollection<T>> FetchAll()
		{
			return await this.context.Set<T>().ToListAsync();
		}

		/// <inheritdoc cref="IRepository{T}.IsAny()"/>
		public async Task<bool> IsAny()
		{
			return await this.context.Set<T>().AnyAsync();
		}

		/// <inheritdoc cref="Add"/>
		public async Task Add(T entity)
		{
			await this.context.AddAsync(entity);
		}

		/// <inheritdoc cref="AddRange"/>
		public Task AddRange(IEnumerator<T> entities)
		{
			return this.context.AddRangeAsync(entities);
		}

		/// <inheritdoc cref="Remove"/>
		public Task Remove(T entity)
		{
			this.context.Remove(entity);
			return Task.CompletedTask;
		}

		/// <inheritdoc cref="Count"/>
		public Task<int> Count(IQueryObjectBase<T> query)
		{
			return query.Count(this.context);
		}

		/// <inheritdoc cref="IsAny"/>
		public Task<bool> IsAny(IQueryObjectBase<T> query)
		{
			return query.IsAny(this.context);
		}

		/// <inheritdoc cref="Fetch"/>
		public Task<IReadOnlyCollection<T>> Fetch(IQueryObjectBase<T> query)
		{
			return query.Fetch(this.context);
		}

		/// <inheritdoc cref="FetchOne"/>
		public Task<T> FetchOne(IQueryObjectBase<T> query)
		{
			return query.FetchOne(this.context);
		}

		/// <inheritdoc cref="FetchCount"/>
		public Task<IReadOnlyCollection<T>> FetchCount(IQueryObjectBase<T> query, int count)
		{
			return query.FetchCount(this.context, count);
		}

		/// <inheritdoc cref="FetchPage"/>
		public Task<IPagedList<T>> FetchPage(IQueryObjectBase<T> query, int pageNumber, int pageSize)
		{
			return query.FetchPage(this.context, pageNumber, pageSize);
		}
	}
}