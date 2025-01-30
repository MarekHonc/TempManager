/**
 * Model pro zobrazení naměřených hodnot.
 */
function roomValuesModel(data) {
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
	 * Id místnosti z filtru.
	 */
	self.roomId = ko.observable();

	/**
	 * Položky k zobrazení.
	 */
	self.rows = ko.observableArray([]);

	/**
	 * Všechny podlaží.
	 */
	self.floors = ko.observableArray(data.floors);

	/**
	 * Aktuálně vybrané podlaží.
	 */
	self.selectedFloor = ko.observable(_.find(floors, function (f) { return f.floorId() == data.selectedFloorId; }));
	self.selectedFloor.subscribe(function () {
		self.page(1);
		self.rows([]);
		loadData();
	});

	/**
	 * Aktuální vyfiltrované místnosti.
	 */
	self.filteredRows = ko.pureComputed(function () {
		if (!!self.roomId()) {
			return _.filter(self.rows(), function (r) {
				return r.id == self.roomId();
			})
		}

		return self.rows();
	});

	/**
	 * Načte data - ať už další nebo nové z filtru.
	 */
	const loadData = function () {
		$.ajax({
			method: "GET",
			url: data.url,
			data: {
				page: self.page(),
				floorId: self.selectedFloor().floorId()
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
	 * Autocomplete pro místnosti.
	 */
	self.roomAutoComplete = new autoCompleteViewModel({
		values: {},
		autoComplete: "#auto-complete-room",
		autoCompleteHolder: "#auto-complete-holder",
		autoCompleteUrl: data.roomAutoCompleteUrl,
		selectCallback: function (id) { self.roomId(id); },
		removeCallback: function (id) { self.roomId(null); }
	});

	// Načtu data.
	loadData();
}

/**
 * Model podlaží.
 */
function floorModel(initData) {
	var self = this;

	/**
	 * Identifikátor podlaží.
	 */
	self.floorId = ko.observable(initData.id);

	/**
	 * Název podlaží.
	 */
	self.floorName = ko.observable(initData.name);
}