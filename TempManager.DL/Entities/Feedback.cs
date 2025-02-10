using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující feedback od uživatele.
	/// </summary>
	public class Feedback : EntityBase, IEntity
	{
		public const int MinRating = 1;
		public const int MaxRating = 10;

		private User user;

		protected Feedback()
		{
		}

		protected Feedback(ILazyLoader lazyLoader) : base(lazyLoader)
		{
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor uživatele.
		/// </summary>
		[ForeignKey(nameof(User))]
		public int UserId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje kdy byla zpětná vazba pořízena.
		/// </summary>
		[Required]
		public DateTimeOffset RatedAt
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje hodnocení na škále 1-10.
		/// </summary>
		public int Rating
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje text hodnocení.
		/// </summary>
		[Required]
		public string Note
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatele, který vytvořil hodnocení.
		/// </summary>
		public User User
		{
			get => this.LazyLoader.Load(this, ref this.user);
			set => this.user = value;
		}

		/// <summary>
		/// Vytvoří novou entitu hodnocení vhodnou pro uložení do databáze.
		/// </summary>
		public static Feedback Create(int userId, int rating, string note)
		{
			// Hodnocení musí být mezi 1 a 10.
			if (rating < MinRating || rating > MaxRating)
				throw new ArgumentOutOfRangeException(nameof(rating));

			return new Feedback()
			{
				// TODO: Hvězdičky v editoru zadávání + děkovná stránka lepší
				RatedAt = DateTimeOffset.UtcNow,
				UserId = userId,
				Rating = rating,
				Note = note
			};
		}
	}
}