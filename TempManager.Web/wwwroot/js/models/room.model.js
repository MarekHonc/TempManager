/**
 * View model reprezentující místnost.
 */
function roomModel(room, initData) {
	let self = this;

	/**
	 * Id místnosti.
	 */
	self.id = ko.observable(room.id);

	/**
	 * Název místnosti.
	 */
	self.name = ko.observable(room.name);

	/**
	 * Externí identifikátor místnosti.
	 */
	self.externalId = ko.observable(room.externalId);

	/**
	 * Příznak - právo k nastavení temploty.
	 */
	self.hasRightToEdit = ko.observable(room.hasRightToEdit);

	/**
	 * Uložení v oblíbených.
	 */
	self.isFavorite = ko.observable(room.isFavorite);
	self.isFavorite.subscribe(function (newValue) {
		debouncedHandleFavoriteChange();
	});

	const handleFavoriteChange = function () {
		$.ajax({
			method: "POST",
			url: initData.saveFavoriteUrl,
			data: {
				roomId: self.id(),
				isFavorite: self.isFavorite()
			}
			// TODO: success + fail
		});
	}

	const debouncedHandleFavoriteChange = _.debounce(handleFavoriteChange, 500, false);

	/**
	 * Aktuální templota.
	 */
	self.temperature = ko.observable(room.temperature);

	/**
	 * Zformátovaná teplota.
	 */
	self.temperatureFormatted = ko.observable(room.temperatureFormatted);

	/**
	 * Hodnota RH.
	 */
	self.rh = ko.observable(room.rh);

	/**
	 * Nastavená teplota.
	 */
	self.desiredTemperature = ko.observable(new valueHolder(room.desiredTemperature, room.desiredTemperature));
	self.desiredTemperature().inputValue.subscribe(function (newValue) {
		debouncedhandleTemperatureChange();
	});

	/**
	 * Zpracování změny teploty.
	 */
	const handleTemperatureChange = function () {
		if (self.desiredTemperature().dsValue() == self.desiredTemperature().inputValue())
			return;

		$.ajax({
			method: "POST",
			url: initData.setTemperatureUrl,
			data: {
				roomId: self.id(),
				desiredTemperature: self.desiredTemperature().inputValue()
			}
			// TODO: success + fail
		});
	}

	const debouncedhandleTemperatureChange = _.debounce(handleTemperatureChange, 500, false);

	/**
	 * Příznak - vytápí se nebo ne.
	 */
	self.valveOpen = ko.observable(room.valveOpen);

	/**
	 * X pozice na mapě.
	 */
	self.x = ko.observable(room.x);

	/**
	 * Y pozice na mapě.
	 */
	self.y = ko.observable(room.y);

	/**
	 * Updatuje hodnoty pro zobrazení.
	 */
	self.update = function (room) {
		self.temperature(room.temperature);
		self.temperatureFormatted(room.temperatureFormatted);
		self.rh(room.rh);
		self.valveOpen(room.valveOpen);

		self.desiredTemperature().dsValue(room.desiredTemperature);
		self.desiredTemperature().inputValue(room.desiredTemperature);
	}
}

/**
 * Value holder, pro správý update hodnoty input <--> server.
 */
function valueHolder(dsValue, inputValue) {
	var self = this;

	/**
	 * Hodnota ze serveru.
	 */
	self.dsValue = ko.observable(dsValue);

	/**
	 * Hodnota z klienta.
	 */
	self.inputValue = ko.observable(inputValue);
}