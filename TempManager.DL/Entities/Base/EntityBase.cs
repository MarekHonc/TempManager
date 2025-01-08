using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace TempManager.DL.Entities.Base
{
	/// <summary>
	/// Bázová třída pro entity v aplikaci.
	/// </summary>
	public class EntityBase
	{
		protected EntityBase()
		{
		}

		protected EntityBase(ILazyLoader lazyLoader)
		{
			this.LazyLoader = lazyLoader;
		}

		/// <summary>
		/// Vrací nebo nastavuje id entity.
		/// </summary>
		[Key]
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací Lazy loader pro dodatečné načítání entit.
		/// </summary>
		protected ILazyLoader LazyLoader
		{
			get;
		}
	}
}