namespace TempManager.BL.Models
{
	/// <summary>
	/// Model reprezentující patro budovy.
	/// </summary>
	public class Floor
	{
		internal Floor(DL.Entities.Floor floor)
		{
			this.Id = floor.Id;
			this.FriendlyId = floor.FriendlyId;
			this.Name = floor.Name;
			this.MapViewName = floor.MapViewName;
		}

		internal Floor(int id, string friendlyId, string name, string mapViewName)
		{
			this.Id = id;
			this.FriendlyId = friendlyId;
			this.Name = name;
			this.MapViewName = mapViewName;
		}

		/// <summary>
		/// Vrací unikátní identifikátor podlaží.
		/// </summary>
		public int Id
		{
			get;
		}

		/// <summary>
		/// Vrací nebo nastavuje interní název podlaží.
		/// </summary>
		public string FriendlyId
		{
			get;
		}

		/// <summary>
		/// Vrací název podlaží.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// Vrací název šablony, která se má zobrazit při zobrazení typu "mapa".
		/// </summary>
		public string? MapViewName
		{
			get;
		}
	}
}
