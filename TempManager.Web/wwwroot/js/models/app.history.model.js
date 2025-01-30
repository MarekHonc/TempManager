/**
 * Zobrazení histori nastavení podlaží.
 */
function historyModel(data) {
	var self = this;

	/**
	 * Aktuální stránka.
	 */
	self.page = ko.observable(1);

	/**
	 * Existuje následujícící stránka.
	 */
	self.hasNext = ko.observable(false);

	/**
	 * Id uživatele z filtru.
	 */
	self.userId = ko.observable();

	/**
	 * Id místnosti z filtru.
	 */
	self.roomId = ko.observable();

	/**
	 * Položky k zobrazení.
	 */
	self.rows = ko.observableArray([]);

	/**
	 * Načte data - ať už další nebo nové z filtru.
	 */
	const loadData = function () {
		$.ajax({
			method: "GET",
			url: data.url,
			data: {
				page: self.page(),
				userId: self.userId(),
				roomId: self.roomId()
			},
			success: function (data) {
				self.page(data.page);
				self.hasNext(data.hasNext);

				for (let i = 0; i < data.items.length; i++) {
					self.rows.push(data.items[i]);
				}
			}
		})
	}

	/**
	 * Načte další stránku.
	 */
	self.loadNext = function () {
		self.page(self.page() + 1);
		loadData();
	}

	/**
	 * Autocomplete pro uživatele.
	 */
	self.userAutoComplete = new autoCompleteViewModel({
		values: {},
		autoComplete: "#auto-complete-user",
		autoCompleteHolder: "#auto-complete-holder",
		autoCompleteUrl: data.userAutoCompleteUrl,
		selectCallback: function (id) { setUserId(id); },
		removeCallback: function (id) { setUserId(); }
	});

	const setUserId = function (id) {
		self.page(1);
		self.rows([]);
		self.userId(id);
		loadData();
	}

	/**
	 * Autocomplete pro místnosti.
	 */
	self.roomAutoComplete = new autoCompleteViewModel({
		values: {},
		autoComplete: "#auto-complete-room",
		autoCompleteHolder: "#auto-complete-holder",
		autoCompleteUrl: data.roomAutoCompleteUrl,
		selectCallback: function (id) { setRoomId(id); },
		removeCallback: function (id) { setRoomId(); }
	});

	const setRoomId = function (id) {
		self.page(1);
		self.rows([]);
		self.roomId(id);
		loadData();
	}

	// Načtu data.
	loadData();
}