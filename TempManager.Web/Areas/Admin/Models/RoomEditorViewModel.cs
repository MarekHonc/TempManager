using System.ComponentModel.DataAnnotations;
using TempManager.DL.Entities;
using TempManager.Web.Areas.Admin.Localization;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro editaci místnosti.
	/// </summary>
	public class RoomEditorViewModel : AdminBaseViewModel
	{
		public RoomEditorViewModel()
		{
		}

		public RoomEditorViewModel(Room room)
		{
			this.Id = room.Id;
			this.Name = room.Name;
			this.AuthorizedUsers = room.UsersToRoom
				.Where(utr => utr.HasRightToEdit)
				.ToDictionary(k => k.UserId, v => v.User.UserName);
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
		/// Vrací nebo nastavuje uživatele, kteří mají oprávnění nastavovat teploty v místnosti.
		/// </summary>
		public Dictionary<int, string> AuthorizedUsers
		{
			get;
			set;
		}

		/// <summary>
		/// Promítne změny z modelu do databáze.
		/// </summary>
		public void Update(Room room)
		{
			room.SetName(this.Name);
			
			// Oprávnění vůči místnostem.
			foreach (var userToRoom in room.UsersToRoom)
			{
				if (this.AuthorizedUsers.TryGetValue(userToRoom.UserId, out _))
				{
					userToRoom.SetHasRight(true);
					this.AuthorizedUsers.Remove(userToRoom.UserId);
				}
				else
				{
					userToRoom.SetHasRight(false);
				}
			}

			// Tady mi zbydou pouze nově přidaní uživatelé.
			foreach (var userId in this.AuthorizedUsers)
			{
				var userToRoom = UserToRoom.Create(userId.Key, this.Id, isFavorite: false, hasRight: true);

				room.UsersToRoom.Add(userToRoom);
			}
		}
	}
}
