using System.ComponentModel.DataAnnotations.Schema;
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

		protected Feedback()
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
		public string Note
		{
			get;
			protected set;
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
				UserId = userId,
				Rating = rating,
				Note = note
			};
		}
	}
}