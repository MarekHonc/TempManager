using System.ComponentModel.DataAnnotations;
using TempManager.DL.Entities;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;
using TempManager.Web.Areas.Admin.Localization;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro editaci podlaží.
	/// </summary>
	public class FloorEditorViewModel : AdminBaseViewModel, IValidatableObject
	{
		public FloorEditorViewModel()
		{
		}

		public FloorEditorViewModel(Floor floor)
		{
			this.Id = floor.Id;
			this.FriendlyId = floor.FriendlyId;
			this.Name = floor.Name;
			this.IsVisible = !floor.IsHidden;
			this.MapViewName = floor.MapViewName;
		}

		/// <summary>
		/// Vrací nebo nastauvje id podlaží.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje user-friendly id podlaží.
		/// </summary>
		[Display(Name = "FriendlyId", ResourceType = typeof(AdminResources))]
		[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdminResources))]
		[StringLength(30, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(AdminResources))]
		public string FriendlyId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název podlaží.
		/// </summary>
		[Display(Name = "Name", ResourceType = typeof(AdminResources))]
		[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdminResources))]
		[StringLength(30, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(AdminResources))]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název šablony s mapou.
		/// </summary>
		[Display(Name = "MapViewName", ResourceType = typeof(AdminResources))]
		[StringLength(30, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(AdminResources))]
		public string MapViewName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li je poldaží viditelné.
		/// </summary>
		[Display(Name = "Visible", ResourceType = typeof(AdminResources))]
		public bool IsVisible
		{
			get;
			set;
		}

		/// <summary>
		/// Updatuje podlaží.
		/// </summary>
		public void Update(Floor floor)
		{
			floor.Update(
				this.FriendlyId,
				this.Name,
				this.MapViewName,
				this.IsVisible
			);
		}

		/// <summary>
		/// Dodatečná validace modelu.
		/// </summary>
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var repositoriesFactory = validationContext.GetService<RepositoriesFactory>()!;
			var query = new FloorByFriendlyIdQuery(this.FriendlyId);
			var floor = (repositoriesFactory.FloorRepository.FetchOne(query)).Result;

			// Našel jsem jiné podlaží než aktuální.
			if (floor != null && floor.Id != this.Id)
				yield return new ValidationResult(string.Format(AdminResources.FloorDuplicate, this.FriendlyId), new[] { nameof(this.FriendlyId) });
		}
	}
}