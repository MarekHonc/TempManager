using Microsoft.EntityFrameworkCore;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Repositories
{
	/// <summary>
	/// Generický repozitář pro konkrétní tabulku.
	/// </summary>
	/// <typeparam name="T">Tabulka, se kterou se pracuje.</typeparam>
	public class BaseRepository<T> : IRepository<T> where T : class, IEntity
	{
		private readonly TempManagerContext context;

		public BaseRepository(TempManagerContext context)
		{
			this.context = context;
		}

		/// <inheritdoc cref="FetchById"/>
		public async Task<T?> FetchById(params object[] id)
		{
			return await this.context.Set<T>().FindAsync(id);
		}

		/// <inheritdoc cref="FetchAll"/>
		public async Task<IReadOnlyCollection<T>> FetchAll()
		{
			return await this.context.Set<T>().ToListAsync();
		}

		/// <inheritdoc cref="Add"/>
		public async Task Add(T entity)
		{
			await this.context.AddAsync(entity);
		}

		/// <inheritdoc cref="AddRange"/>
		public async Task AddRange(IEnumerator<T> entities)
		{
			await this.context.AddRangeAsync(entities);
		}

		/// <inheritdoc cref="Remove"/>
		public Task Remove(T entity)
		{
			this.context.Remove(entity);
			return Task.CompletedTask;
		}
	}
}
