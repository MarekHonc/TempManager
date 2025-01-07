using System.ComponentModel.DataAnnotations;
using TempManager.Web.Localization;

namespace TempManager.Web.Models
{
	/// <summary>
	/// Model pro získání zpětné vazby od uživatele.
	/// </summary>
	public class FeedbackViewModel : BaseViewModel
	{
		/// <summary>
		/// Vrací nebo nastavuje hodnocení.
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
		[Display(Name = "Rating", ResourceType = typeof(Resources))]
		[Range(DL.Entities.Feedback.MinRating, DL.Entities.Feedback.MaxRating, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Range")]
		public int Rating
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje slovní hodnocení.
		/// </summary>
		[Display(Name = "Feedback", ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
		public string Feedback
		{
			get;
			set;
		}
	}
}