/**
 * Hlavní view model aplikace.
 * @param {any} initData inicializační data modelu.
 */
function appModel(initData) {
	let self = this;

	if (!initData.roomsUrl) throw "Missing roomsUrl";
	if (!initData.saveFavoriteUrl) throw "Missing saveFavoriteUrl";
	if (!initData.setTemperatureUrl) throw "Missing setTemperatureUrl";
	if (!initData.group) throw "Missing group";
	if (!initData.step) initData.step = 0;
	if (!initData.minValue) initData.minValue = 0;
	if (!initData.maxValue) initData.maxValue = 0;

	/**
	 * Příznak, zda-li probíhá načítání.
	 */
	self.loading = ko.observable(true);

	/**
	 * Aktuální vyhledávávní.
	 */
	self.searchTerm = ko.observable();

	/**
	 * Zobrazení pouze oblíbených.
	 */
	self.onlyFavorites = ko.observable(false);

	/**
	 * Všechny místnosti v aplikaci.
	 */
	self.rooms = ko.observableDictionary();

	/**
	 * AKtuální vyfiltrované místnosti.
	 */
	self.filteredRooms = ko.pureComputed(function () {
		const isSearch = !!self.searchTerm();
		const onlyFavorites = self.onlyFavorites();

		let rooms;
		if (onlyFavorites) {
			rooms = _.filter(self.rooms.values(), function (r) {
				return r.isFavorite();
			})
		}
		else {
			rooms = self.rooms.values();
		}

		if (!isSearch)
			return rooms;

		return _.filter(rooms, function (r) {
			return r.name().includes(self.searchTerm());
		});
	});

	let loadData = function (data) {
		data.forEach(function (room) {
			self.rooms.set(room.externalId, new roomModel(room, initData));
		});
	}

	/**
	 * Inicializační metoda modelu.
	 */
	let init = function () {
		$.ajax({
			url: initData.roomsUrl,
			method: "GET",
			success: function (data) {
				// Načtu vše do view.
				loadData(data);

				// Nakonec vypnu načítání.
				self.loading(false);
			}
		});
	}

	// Inicializace.
	init();

	// SignalR
	const connection = new signalR.HubConnectionBuilder()
		.withUrl("/updateHub")
		.build();

	connection.on("ReceiveMessage", (data) => {
		const values = self.rooms.values();

		for (let i = 0; i < data.length; i++) {
			const current = data[i];
			const room = _.find(values, function (r) { return r.externalId() == current.externalId });

			if (room)
				room.update(current);
		}
	});

	/**
	 * Nastartuje SignalR připojení.
	 */
	async function start() {
		try {
			await connection.start();
			await connection.invoke("Join", initData.group);
		} catch (err) {
			console.error(err);
			setTimeout(start, 5000);
		}
	}

	connection.onclose(start);
	start();

	/**
	 * Na zavření okna odpojím.
	 */
	window.addEventListener("unload", async () => await connection.invoke("Leave", initData.group));
	//window.onbeforeunload = async () => {
	//	await connection.invoke("Leave", initData.group);
	//}
}