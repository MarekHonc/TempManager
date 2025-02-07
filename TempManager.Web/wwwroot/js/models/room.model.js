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
	 * Název podlaží.
	 */
	self.floorName = ko.observable(room.floorName);

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
	 * Přidá / odebere danou místnost z oblíbených.
	 */
	self.toggleFavorite = function () {
		self.isFavorite(!self.isFavorite());
	}

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
	self.desiredTemperature = ko.observable(new valueHolder(room.desiredTemperatureFormatted, room.desiredTemperatureFormatted));
	self.desiredTemperature().inputValue.subscribe(function (newValue) {
		debouncedhandleTemperatureChange();
	});

	/**
	 * Nastaví novou teplotu -> přidá nebo odebere krok podle předaného směru.
	 */
	self.changeTemperature = function (direction) {
		let odlTemperature = parseFloat(self.desiredTemperature().inputValue());
		self.desiredTemperature().inputValue(odlTemperature + direction * 0.5);
	}

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
		self.temperatureFormatted(room.temperatureFormatted);
		self.rh(room.rh);
		self.valveOpen(room.valveOpen);

		self.desiredTemperature().dsValue(room.desiredTemperatureFormatted);
		self.desiredTemperature().inputValue(room.desiredTemperatureFormatted);
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