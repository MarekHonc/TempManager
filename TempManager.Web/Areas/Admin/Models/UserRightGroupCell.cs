using TempManager.DL.Entities;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model reprezentující jednotlivou buňku editoru.
	/// </summary>
	public class UserRightGroupCell
	{
		public UserRightGroupCell()
		{
		}

		public UserRightGroupCell(User user, Room room, UserToRoom userToRoom)
		{
			this.UserIsDeleted = user.IsDeleted;
			this.UserName = user.UserName;
			this.ExternalRoomId = room.ExternalId;
			this.IsFavorite = userToRoom?.IsFavorite ?? false;
			this.CanEdit = userToRoom?.HasRightToEdit ?? false;
			this.CanView = userToRoom?.HasRightToView ?? false;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li je uživatel smazaný.
		/// </summary>
		public bool UserIsDeleted
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje uživatelské jméno.
		/// </summary>
		public string UserName
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor místnosti.
		/// </summary>
		public string ExternalRoomId
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo nastavuje zda-li má uživatel označenou místnost jako oblíbenou.
		/// </summary>
		public bool IsFavorite
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo zobrazuje zda-li si uživatel může editovat místnost.
		/// </summary>
		public bool CanEdit
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací nebo zobrazuje zda-li si uživatel může zobrazit místnost.
		/// </summary>
		public bool CanView
		{
			get;
			set;
		}
	}
}
