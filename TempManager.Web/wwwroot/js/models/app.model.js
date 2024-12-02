/**
 * Hlavní view model aplikace.
 * @param {any} initData inicializační data modelu.
 */
function appModel(initData) {
	let self = this;

	if (!initData.roomsUrl) throw "Missing roomsUrl";
	if (!initData.saveFavoriteUrl) throw "Missing saveFavoriteUrl";

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
			self.rooms.set(room.name, new roomModel(room, initData));
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
}