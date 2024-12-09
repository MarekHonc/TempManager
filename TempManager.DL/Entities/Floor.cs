using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita reprezentující podlaží.
	/// </summary>
	public class Floor : EntityBase, IExternalId
	{
		protected Floor()
		{
			this.Rooms = new HashSet<Room>();
		}

		/// <summary>
		/// Vrací nebo nastavuje id podlaží v externím systému.
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "varchar(30)")]
		public string ExternalId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje interní název podlaží.
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "varchar(30)")]
		public string FriendlyId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací název podlaží.
		/// </summary>
		[StringLength(50)]
		[Column(TypeName = "varchar(50)")]
		public string Name
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací název šablony, která se má zobrazit při zobrazení typu "mapa".
		/// </summary>
		[StringLength(30)]
		[Column(TypeName = "varchar(30)")]
		public string? MapViewName
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje, zda-li je podlaží skryté.
		/// </summary>
		public bool IsHidden
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje seznam místností v podlaží.
		/// </summary>
		public ICollection<Room> Rooms
		{
			get;
			protected set;
		}

		/// <summary>
		/// Updatuje hodnoty podlaží.
		/// </summary>
		public void Update(string friendlyId, string name, string? mapViewName, bool isVisible)
		{
			this.FriendlyId = friendlyId;
			this.Name = name;
			this.MapViewName = mapViewName;
			SetIsVisible(isVisible);
		}

		/// <summary>
		/// Updatuje viditelnost podlaží.
		/// </summary>
		public void SetIsVisible(bool isVisible)
		{
			this.IsHidden = !isVisible;
		}

		/// <summary>
		/// Vytvoří novou entitu podlaží.
		/// </summary>
		public static async Task<Floor> Create(IExternalIdRepository<Floor> repository, string externalId, bool ignoreExternalIdCheck = false)
		{
			if (!ignoreExternalIdCheck && await repository.GetByExternalId(externalId) != null)
				throw new ArgumentException($"Floor with external id: '{externalId}' already exists!");

			// Vrátí nové podlaží.
			var floor = new Floor()
			{
				ExternalId = externalId,
				FriendlyId = externalId,
				Name = externalId,
				IsHidden = false
			};

			// Vracím nové podlaží.
			return floor;
		}
	}
}
