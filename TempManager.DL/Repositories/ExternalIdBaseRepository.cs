using Microsoft.EntityFrameworkCore;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Repositories
{
	/// <summary>
	/// Generický repozitář pro konkrétní tabulku s entitou, která má externí id.
	/// </summary>
	/// <typeparam name="T">Tabulka, se kterou se pracuje.</typeparam>
	internal class ExternalIdBaseRepository<T> : BaseRepository<T>, IExternalIdRepository<T> where T : class, IExternalId
	{
		public ExternalIdBaseRepository(TempManagerContext context) : base(context)
		{
		}

		/// <inheritdoc cref="GetByExternalId"/>
		public Task<T> GetByExternalId(string externalId)
		{
			return this.context.Set<T>().FirstOrDefaultAsync(e => e.ExternalId == externalId);
		}

		/// <inheritdoc cref="GetExternalIdLookUp"/>
		public Task<Dictionary<string, T>> GetExternalIdLookUp()
		{
			return this.context.Set<T>().ToDictionaryAsync(k => k.ExternalId, v => v);
		}
	}
}
