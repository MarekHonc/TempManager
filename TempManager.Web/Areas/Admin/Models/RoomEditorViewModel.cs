using System.ComponentModel.DataAnnotations;
using TempManager.DL.Entities;
using TempManager.Web.Areas.Admin.Localization;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro editaci místnosti.
	/// </summary>
	public class RoomEditorViewModel : AdminBaseViewModel, IValidatableObject
	{
		public RoomEditorViewModel()
		{
		}

		public RoomEditorViewModel(Room room)
		{
			this.Id = room.Id;
			this.Name = room.Name;
			this.X = room.XPosition;
			this.Y = room.YPosition;
			this.AuthorizedUsers = room.UsersToRoom.ToDictionary(
				k => k.UserId,
				v => new UserRightEditorViewModel(v.User.UserName, v.HasRightToEdit, v.HasRightToView)
			);
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor místnosti.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název místnosti.
		/// </summary>
		[Display(Name = "Name", ResourceType = typeof(AdminResources))]
		[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdminResources))]
		[StringLength(50, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(AdminResources))]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje X pozici na mapě.
		/// </summary>
		[Display(Name = "X", ResourceType = typeof(AdminResources))]
		public double? X
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje Y pozici na mapě.
		/// </summary>
		[Display(Name = "Y", ResourceType = typeof(AdminResources))]
		public double? Y
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatele, kteří mají oprávnění nastavovat teploty v místnosti.
		/// </summary>
		public Dictionary<int, UserRightEditorViewModel> AuthorizedUsers
		{
			get;
			set;
		}

		/// <summary>
		/// Promítne změny z modelu do databáze.
		/// </summary>
		public void Update(Room room)
		{
			this.AuthorizedUsers ??= new Dictionary<int, UserRightEditorViewModel>();

			room.SetName(this.Name);
			room.SetPosition(this.X, this.Y);

			// Oprávnění vůči místnostem.
			foreach (var userToRoom in room.UsersToRoom)
			{
				if (this.AuthorizedUsers.TryGetValue(userToRoom.UserId, out var editable))
				{
					userToRoom.SetHasRight(editable.CanEdit, editable.CanView);
					this.AuthorizedUsers.Remove(userToRoom.UserId);
				}
				else
				{
					userToRoom.SetHasRight(false, false);
				}
			}

			// Tady mi zbydou pouze nově přidaní uživatelé.
			foreach (var userId in this.AuthorizedUsers)
			{
				var userToRoom = UserToRoom.Create(
					userId.Key,
					this.Id,
					isFavorite: false,
					hasRightToEdit: userId.Value.CanEdit,
					hasRightToView: userId.Value.CanView
				);

				room.UsersToRoom.Add(userToRoom);
			}
		}

		/// <inheritdoc />
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if ((this.X.HasValue && !this.Y.HasValue) || (!this.X.HasValue && this.Y.HasValue))
				yield return new ValidationResult(AdminResources.PositionMustBeSet, new[] { nameof(this.X), nameof(this.Y) });
		}
	}
}