using TempManager.DL.Entities;
using TempManager.DL.Queries;
using TempManager.DL.Repositories;

namespace TempManager.Web.Areas.Admin.Models
{
	/// <summary>
	/// Model pro hromadnou editaci uživatelských práv.
	/// </summary>
	public class UserRightsGroupEditorViewModel : AdminBaseViewModel
	{
		/// <summary>
		/// Vrací nebo nastavuje buňky tabulky.
		/// </summary>
		public UserRightGroupCell[][] Cells
		{
			get;
			set;
		}

		/// <summary>
		/// Vrací sloupce pro editor.
		/// </summary>
		public string[] Columns
		{
			get
			{
				var result = new string[this.Cells[0].Length];

				for (var i = 0; i < result.Length; i++)
				{
					result[i] = this.Cells[0][i].ExternalRoomId;
				}

				return result;
			}
		}

		/// <summary>
		/// Vrací všechny řádky pro editor.
		/// </summary>
		public string[] Rows
		{
			get
			{
				var result = new string[this.Cells.Length];

				for (var i = 0; i < result.Length; i++)
				{
					result[i] = this.Cells[i][0].UserName;
				}

				return result;
			}
		}

		/// <summary>
		/// Načte všechny potřená data pro model.
		/// </summary>
		public async Task Load(RepositoriesFactory repositoriesFactory)
		{
			var usersToRooms =
				(await repositoriesFactory.UserToRoomRepository.FetchAll())
				.ToDictionary(k => (k.UserId, k.RoomId));

			var users = (await repositoriesFactory.UserRepository.FetchAll()).ToList();
			var rooms = (await repositoriesFactory.RoomRepository.FetchAll()).ToList();

			this.Cells = new UserRightGroupCell[users.Count][];

			for (var userIndex = 0; userIndex < users.Count; userIndex++)
			{
				this.Cells[userIndex] = new UserRightGroupCell[rooms.Count];

				var user = users[userIndex];

				for (var roomsIndex = 0; roomsIndex < rooms.Count; roomsIndex++)
				{
					var room = rooms[roomsIndex];
					usersToRooms.TryGetValue((user.Id, room.Id), out var userToRoom);

					this.Cells[userIndex][roomsIndex] = new UserRightGroupCell(user, room, userToRoom);
				}
			}
		}

		/// <summary>
		/// Promítne změny z modelu do databáze.
		/// </summary>
		public async Task Update(RepositoriesFactory repositoriesFactory)
		{
			var users = (await repositoriesFactory.UserRepository.FetchAll()).ToDictionary(k => k.UserName);
			var rooms = (await repositoriesFactory.RoomRepository.FetchAll()).ToDictionary(r => r.ExternalId);
			var floor = await repositoriesFactory.FloorRepository.FetchOne(new GetVisibleFloorsQuery());

			// Nejprve synchronizace uživatelů + místností.
			using (repositoriesFactory.BulkChange())
			{
				foreach (var cell in this.Cells)
				{
					var value = cell[0];

					// Už mám uživatele.
					if (!users.TryGetValue(value.UserName, out var user))
					{
						user = await User.Create(repositoriesFactory.UserRepository, uid: string.Empty, value.UserName,
							firstName: string.Empty, lastName: string.Empty);
						await repositoriesFactory.UserRepository.Add(user);
						users[value.UserName] = user;
					}

					// Smazané smažu.
					if (value.UserIsDeleted)
					{
						user.DeleteUser();
					}
				}

				foreach (var cell in this.Cells[0])
				{
					// Už mám místnost.
					if (!rooms.TryGetValue(cell.ExternalRoomId, out var room))
					{
						room = await Room.Create(repositoriesFactory.RoomRepository, floor, cell.ExternalRoomId);
						await repositoriesFactory.RoomRepository.Add(room);
						rooms[cell.ExternalRoomId] = room;
					}
				}
			}

			// Uložím změny.
			await repositoriesFactory.SaveChanges();

			var usersToRooms =
				(await repositoriesFactory.UserToRoomRepository.FetchAll())
				.ToDictionary(k => (k.UserId, k.RoomId));

			// A nakonec ještě vyřeším práva.
			using (repositoriesFactory.BulkChange())
			{
				for (var r = 0; r < this.Cells.Length; r++)
				{
					for (var c = 0; c < this.Cells[r].Length; c++)
					{
						var value = this.Cells[r][c];
						var user = users[value.UserName];
						var room = rooms[value.ExternalRoomId];

						if (usersToRooms.TryGetValue((user.Id, room.Id), out var existing))
						{
							existing.SetHasRight(value.CanEdit, value.CanView);
						}
						else if (value.CanEdit || value.CanView)
						{
							var newEntity = UserToRoom.Create(
								user.Id,
								room.Id,
								isFavorite: false,
								value.CanEdit,
								value.CanView
							);

							await repositoriesFactory.UserToRoomRepository.Add(newEntity);
						}
					}
				}
			}

			// Uložím změny.
			await repositoriesFactory.SaveChanges();
		}
	}
}
