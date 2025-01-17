using System.ComponentModel.DataAnnotations;
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
			this.IsAdmin = user.IsAdmin;
			this.EditableRooms = user.UserToRooms
				.Where(utr => utr.HasRightToEdit)
				.ToDictionary(k => k.RoomId, v => v.Room.Name);
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
		/// Vrací nebo nastavuje, zda-li je uživatel admin.
		/// </summary>
		[Display(Name = "IsAdmin", ResourceType = typeof(AdminResources))]
		public bool IsAdmin
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje místnosti, které mý daný uživatel právo editovat.
		/// </summary>
		public Dictionary<int, string> EditableRooms
		{
			get;
			set;
		}

		/// <summary>
		/// Promítne změny z modelu do databáze.
		/// </summary>
		public void Update(User user)
		{
			this.EditableRooms ??= new Dictionary<int, string>();

			user.SetIsAdmin(this.IsAdmin);

			// Oprávnění vůči místnostem.
			foreach (var userToRoom in user.UserToRooms)
			{
				if (this.EditableRooms.TryGetValue(userToRoom.RoomId, out _))
				{
					userToRoom.SetHasRight(true);
					this.EditableRooms.Remove(userToRoom.RoomId);
				}
				else
				{
					userToRoom.SetHasRight(false);
				}
			}

			// Tady mi zbydou pouze nově přidaní uživatelé.
			foreach (var roomId in this.EditableRooms)
			{
				var userToRoom = UserToRoom.Create(this.Id, roomId.Key, isFavorite: false, hasRight: true);

				user.UserToRooms.Add(userToRoom);
			}
		}
	}
}