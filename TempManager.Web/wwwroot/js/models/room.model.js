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
	 * Příznak, zda-li při získávání dat v místnosti nastal error.
	 */
	self.isError = ko.observable(room.isError);

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

	/**
	 * Nastaví novou teplotu -> přidá nebo odebere krok podle předaného směru.
	 */
	self.changeTemperature = function (direction, useCallback) {
		var odlTemperature = self.desiredTemperature().inputValue.number();
		var newValue = odlTemperature + direction * initData.step;

		if (newValue > initData.maxValue || newValue < initData.minValue)
			return;

		self.desiredTemperature().inputValue(newValue);

		if (useCallback) {
			debouncedhandleTemperatureChange()
		}
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
		$.ajax({
			method: "POST",
			url: initData.setTemperatureUrl,
			data: {
				roomId: self.id(),
				desiredTemperature: self.desiredTemperature().inputValue()
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
	self.valveOpen = ko.numericObservable(room.valveOpen, "0");

	/**
	 * X pozice na mapě.
	 */
	self.x = ko.observable(room.x);

	/**
	 * Y pozice na mapě.
	 */
	self.y = ko.observable(room.y);

	/**
	 * Spustí interval pro držení tlačítka přidání teploty.
	 */
	self.startIncrement = function () {
		self.startChanging(1);
	};

	/**
	 * Spustí interval pro držení tlačítka snížení teploty.
	 */
	self.startDecrement = function () {
		self.startChanging(-1);
	};

	/**
	 * Spuštění intervalu snižování.
	 */
	self.startChanging = function (direction) {
		if (self.interval)
			return;

		self.interval = setInterval(function () {
			self.changeTemperature(direction, false);
		}, 200);
	};

	/**
	 * Zastavení změny při uvolnění tlačítka.
	 */
	self.stopChange = function () {
		if (self.interval) {
			clearInterval(self.interval);
			self.interval = null;

			debouncedhandleTemperatureChange();
		}
	};

	/**
	 * Updatuje hodnoty pro zobrazení.
	 */
	self.update = function (room) {
		self.temperature(room.temperature);
		self.rh(room.rh);
		self.valveOpen(room.valveOpen);
		self.isError(room.isError);

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