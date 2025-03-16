using TempManager.Common;

namespace TempManager.BL.Models
{
	/// <summary>
	/// Model uživatele aplikace.
	/// </summary>
	public class User
	{
		public User(DL.Entities.User user)
		{
			this.Id = user.Id;
			this.UserName = user.UserName;
			this.FirstName = user.FirstName;
			this.LastName = user.LastName;
			this.Uid = user.Uid;
			this.IsAdmin = user.IsAdmin;
			this.CanViewAllRooms = user.CanViewAllRooms;
			this.IsDeleted = user.IsDeleted;
			this.Groups = user.Groups;
		}

		/// <summary>
		/// Vrací identifikátor uživatele.
		/// </summary>
		public int Id
		{
			get;
		}

		/// <summary>
		/// Vrací název aktuálního uživatele.
		/// </summary>
		public string UserName
		{
			get;
		}

		/// <summary>
		/// Vrací křestní jméno uživatele.
		/// </summary>
		public string FirstName
		{
			get;
		}

		/// <summary>
		/// Vrací příjmení uživatele.
		/// </summary>
		public string LastName
		{
			get;
		}

		/// <summary>
		/// Vrací unikátní identifikátor uživatele.
		/// </summary>
		public string Uid
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je uživatel admin.
		/// </summary>
		public bool IsAdmin
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li uživatel vidí všechny místnosti.
		/// </summary>
		public bool CanViewAllRooms
		{
			get;
		}

		/// <summary>
		/// Vrací zda-li je uživatel smazaný.
		/// </summary>
		public bool IsDeleted
		{
			get;
		}

		/// <summary>
		/// Vrací skupiny, na které má uživatel právo.
		/// </summary>
		public Groups Groups
		{
			get;
		}
	}
}