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
			},
			success: function () {
				toastr["success"](initData.localization.successDescription, initData.localization.success);
			},
			error: function () {
				toastr["error"](initData.localization.errorDescription, initData.localization.error);
			}
		});
	}

	const debouncedHandleFavoriteChange = _.debounce(handleFavoriteChange, 200, false);

	/**
	 * Přidá / odebere danou místnost z oblíbených.
	 */
	self.toggleFavorite = function () {
		self.isFavorite(!self.isFavorite());
	}

	/**
	 * Zformátovaná teplota.
	 */
	self.temperature = ko.numericObservable(room.temperature);

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
	 * Nastaví novou teplotu -> přidá nebo odebere krok podle předaného směru.
	 */
	self.changeTemperature = function (direction) {
		var odlTemperature = self.desiredTemperature().inputValue.number();
		var newValue = odlTemperature + direction * initData.step;

		if (newValue > initData.maxValue || newValue < initData.minValue)
			return;

		self.desiredTemperature().inputValue(newValue);
	}

	/**
	 * Vrací zda-li teplota může být snížena.
	 */
	self.canDecreaseTemperature = ko.pureComputed(function () {
		return self.desiredTemperature().inputValue.number() > initData.minValue;
	});

	/**
	 * Vrací zda-li teplota může být zvýšena.
	 */
	self.canIncreaseTemperature = ko.pureComputed(function () {
		return self.desiredTemperature().inputValue.number() < initData.maxValue;
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
				desiredTemperature: self.desiredTemperature().inputValue.number()
			},
			success: function (data) {
				if (data) {
					toastr["success"](initData.localization.successDescription, initData.localization.success);
				}
				else {
					toastr["error"](initData.localization.errorDescription, initData.localization.error);
				}
			},
			error: function () {;
				toastr["error"](initData.localization.errorDescription, initData.localization.error);
			}
		});
	}

	const debouncedhandleTemperatureChange = _.debounce(handleTemperatureChange, 200, false);

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
	self.dsValue = ko.numericObservable(dsValue);

	/**
	 * Hodnota z klienta.
	 */
	self.inputValue = ko.numericObservable(inputValue);
}