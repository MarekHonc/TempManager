using System.ComponentModel.DataAnnotations;
using TempManager.Common;
using TempManager.DL.Entities;
using TempManager.Web.Areas.Admin.Localization;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro editaci uživatele.
	/// </summary>
	public class UserRightsEditorViewModel : AdminBaseViewModel
	{
		public UserRightsEditorViewModel()
		{
		}

		public UserRightsEditorViewModel(User user)
		{
			this.Id = user.Id;
			this.UserName = user.UserName;
			this.FullName = $"{user.FirstName} {user.LastName}";
			this.IsAdmin = user.IsAdmin;
			this.CanViewAllRooms = user.CanViewAllRooms;
			this.Groups = user.Groups;
			this.EditableRooms = user.UserToRooms.ToDictionary(
					k => k.RoomId,
					v => new UserRightEditorViewModel(v.Room.Name, v.HasRightToEdit, v.HasRightToView)
			);
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor uživatele.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje název uživatele.
		/// </summary>
		public string UserName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje celé jméno uživatele.
		/// </summary>
		public string FullName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li je uživatel admin.
		/// </summary>
		[Display(Name = "IsAdmin", ResourceType = typeof(AdminResources))]
		public bool IsAdmin
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li uživatel může vidět všechny místnosti.
		/// </summary>
		[Display(Name = "CanViewAllRooms", ResourceType = typeof(AdminResources))]
		public bool CanViewAllRooms
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje skupiny oprávnění.
		/// </summary>
		[Display(Name = "Groups", ResourceType = typeof(AdminResources))]
		public Groups Groups
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje oprávnění uživatele.
		/// </summary>
		public int[] GroupsSetter
		{
			get
			{
				var result = new List<int>();

				foreach (var group in Enum.GetValues<Groups>())
				{
					if (this.Groups.HasFlag(group))
					{
						result.Add((int) group);
					}
				}

				return result.ToArray();
			}
			set
			{
				this.Groups = (Groups) value.Sum();
			}
		}

		/// <summary>
		/// Vrací nebo nastavuje místnosti, které mý daný uživatel právo editovat.
		/// </summary>
		public Dictionary<int, UserRightEditorViewModel> EditableRooms
		{
			get;
			set;
		}

		/// <summary>
		/// Promítne změny z modelu do databáze.
		/// </summary>
		public void Update(User user)
		{
			this.EditableRooms ??= new Dictionary<int, UserRightEditorViewModel>();

			user.SetIsAdmin(this.IsAdmin);
			user.SetCanViewAllRooms(this.CanViewAllRooms);
			user.SetGroups(this.Groups);

			// Oprávnění vůči místnostem.
			foreach (var userToRoom in user.UserToRooms)
			{
				if (this.EditableRooms.TryGetValue(userToRoom.RoomId, out var editable))
				{
					userToRoom.SetHasRight(editable.CanEdit, editable.CanView);
					this.EditableRooms.Remove(userToRoom.RoomId);
				}
				else
				{
					userToRoom.SetHasRight(false, false);
				}
			}

			// Tady mi zbydou pouze nově přidaní uživatelé.
			foreach (var roomId in this.EditableRooms)
			{
				var userToRoom = UserToRoom.Create(
					this.Id,
					roomId.Key,
					isFavorite: false,
					hasRightToEdit: roomId.Value.CanEdit,
					hasRightToView: roomId.Value.CanView
				);

				user.UserToRooms.Add(userToRoom);
			}
		}
	}
}