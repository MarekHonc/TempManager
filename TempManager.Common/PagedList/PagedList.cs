namespace TempManager.Common
{
	/// <summary>
	/// Model pro list, který je možné stránkovat.
	/// </summary>
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		private PagedList()
		{
		}
		public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
			: this(source, pageNumber, pageSize, null)
		{
		}

		public PagedList(IQueryable<T> source, int pageNumber, int pageSize, int? totalCount)
		{
			Initialize(source, pageNumber, pageSize, totalCount);
		}

		#region IPagedList Members

		/// <inheritdoc cref="IPagedList{T}.PageCount"/>
		public int PageCount
		{
			get
			{
				if (this.TotalItemCount > 0)
				{
					return (int)Math.Ceiling(this.TotalItemCount / (double)this.PageSize);
				}

				return 0;
			}
		}

		/// <inheritdoc cref="IPagedList{T}.TotalItemCount"/>
		public int TotalItemCount
		{
			get;
			private set;
		}

		/// <inheritdoc cref="IPagedList{T}.PageIndex"/>
		public int PageIndex
		{
			get;
			private set;
		}

		/// <inheritdoc cref="IPagedList{T}.PageNumber"/>
		public int PageNumber => this.PageIndex + 1;

		/// <inheritdoc cref="IPagedList{T}.PageSize"/>
		public int PageSize
		{
			get;
			private set;
		}

		/// <inheritdoc cref="IPagedList{T}.HasPreviousPage"/>
		public bool HasPreviousPage => this.PageIndex > 0;

		/// <inheritdoc cref="IPagedList{T}.HasNextPage"/>
		public bool HasNextPage => this.PageIndex < this.PageCount - 1;

		/// <inheritdoc cref="IPagedList{T}.IsFirstPage"/>
		public bool IsFirstPage => this.PageIndex <= 0;

		/// <inheritdoc cref="IPagedList{T}.IsLastPage"/>
		public bool IsLastPage => this.PageIndex >= this.PageCount - 1;

		/// <summary>
		/// Vrací nový <see cref="PagedList{T}"/>, který je naplněn položkami nového typu.
		/// </summary>
		/// <param name="select">Select, pomocí kterého se provede konverze položek stávajícího typu do nového.</param>
		public IPagedList<TNew> SelectPagedList<TNew>(Func<T, TNew> select)
		{
			var list = new PagedList<TNew>()
			{
				PageIndex = this.PageIndex,
				PageSize = this.PageSize,
				TotalItemCount = this.TotalItemCount
			};

			list.AddRange(this.Select(select));

			return list;
		}

		#endregion

		protected void Initialize(IQueryable<T> source, int pageNumber, int pageSize, int? totalCount)
		{
			// for using it externally with 1 based page number
			pageNumber = pageNumber - 1;

			//### argument checking
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("PageIndex cannot be below 0.");
			}
			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException("PageSize cannot be less than 1.");
			}

			//### set source to blank list if source is null to prevent exceptions
			if (source == null)
			{
				source = new List<T>().AsQueryable();
			}

			//### set properties
			this.PageSize = pageSize;
			this.PageIndex = pageNumber;

			// query na získání dat
			var items = source.Skip((pageNumber) * pageSize).Take(pageSize).ToList();

			//### add items to internal list
			AddRange(items);

			// Pokud je stažený počet menší než pagesize, tak asi další page není a tak znám TotalCount
			if (this.Count < pageSize) // je to asi poslední stránka
			{
				this.TotalItemCount = (this.PageIndex * this.PageSize) + this.Count;
			}
			else
			{
				this.TotalItemCount = totalCount ?? source.Count();
			}
		}
	}
}