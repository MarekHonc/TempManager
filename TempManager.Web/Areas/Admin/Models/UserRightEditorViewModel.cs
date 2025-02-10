namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro editaci práv uživatele.
	/// </summary>
	public class UserRightEditorViewModel
	{
		public UserRightEditorViewModel()
		{
		}

		public UserRightEditorViewModel(string title, bool canEdit, bool canView)
		{
			this.Title = title;
			this.CanEdit = canEdit;
			this.CanView = canView;
		}

		/// <summary>
		/// Vrací nebo nastavuje titulek záznamu ke kterému se práva váží.
		/// </summary>
		public string Title
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