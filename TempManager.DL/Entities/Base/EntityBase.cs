using System.ComponentModel.DataAnnotations;

namespace TempManager.DL.Entities.Base
{
	/// <summary>
	/// Bázová třída pro entity v aplikaci.
	/// </summary>
	public class EntityBase
	{
		/// <summary>
		/// Vrací nebo nastavuje id entity.
		/// </summary>
		[Key]
		public int Id
		{
			get;
			set;
		}
	}
}
