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
			this.IsAdmin = user.IsAdmin;
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
		/// Vrací zda-li je uživatel admin.
		/// </summary>
		public bool IsAdmin
		{
			get;
		}
	}
}
