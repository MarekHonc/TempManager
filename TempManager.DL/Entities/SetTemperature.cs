using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using TempManager.Common;
using TempManager.DL.Entities.Base;
using TempManager.DL.Interfaces;

namespace TempManager.DL.Entities
{
	/// <summary>
	/// Entita pro uchování nastavení teploty.
	/// </summary>
	public class SetTemperature : EntityBase, IEntity
	{
		private Room room;
		private User user;

		protected SetTemperature()
		{
		}

		protected SetTemperature(ILazyLoader lazyLoader)
		{
			this.LazyLoader = lazyLoader;
		}

		/// <summary>
		/// Vrací nebo nastavuje datum a čas, kdy došlo ke změně teploty.
		/// </summary>
		public DateTimeOffset Date
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje starou teplotu.
		/// </summary>
		public double OldTemperature
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje novou teplotu.
		/// </summary>
		public double NewTemperature
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje 
		/// </summary>
		public SetTemperatureResult Result
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor uživatele.
		/// </summary>
		[ForeignKey(nameof(User))]
		public int UserId
		{
			get;
			protected set;
		}

		/// <summary>
		/// Vrací nebo nastavuje identifikátor místnosti.
		/// </summary>
		[ForeignKey(nameof(Room))]
		public int RoomId
		{
			get;
			protected set;
		}


		/// <summary>
		/// Vrací nebo nastavuje uživatele.
		/// </summary>
		public User User
		{
			get => this.LazyLoader.Load(this, ref this.user);
			set => this.user = value;
		}

		/// <summary>
		/// Vrací nebo nastavuje místnost.
		/// </summary>
		public Room Room
		{
			get => this.LazyLoader.Load(this, ref this.room);
			set => this.room = value;
		}

		/// <summary>
		/// Vrací Lazy loader pro dodatečné načítání entit.
		/// </summary>
		private ILazyLoader LazyLoader
		{
			get;
		}

		/// <summary>
		/// Nastaví nový výsledek nastavení teploty.
		/// </summary>
		public void SetResult(SetTemperatureResult result)
		{
			// Pokud není nastavení ve stavu pending, vyhodím výjimku.
			if (this.Result != SetTemperatureResult.Pending)
				throw new ArgumentException($"Cannot change result of entity in state {this.Result}", nameof(result));

			this.Result = result;
		}

		/// <summary>
		/// Vrací entitu reprezentující nastavení nové teploty, vhodnou pro uložení do databáze.
		/// </summary>
		public static SetTemperature Create(int userId, int roomId, double oldTemperature, double newTemperature)
		{
			return new SetTemperature()
			{
				UserId = userId,
				RoomId = roomId,
				OldTemperature = oldTemperature,
				NewTemperature = newTemperature,
				Result = SetTemperatureResult.Pending
			};
		}
	}
}