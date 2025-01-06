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
	 * Hodnota RH.
	 */
	self.rh = ko.observable(room.rh);

	/**
	 * Hodnota CO2.
	 */
	self.co2 = ko.observable(room.cO2);

	/**
	 * Nastavená teplota.
	 */
	self.desiredTemperature = ko.observable(room.desiredTemperature);
	self.desiredTemperature.subscribe(function (newValue) {
		debouncedhandleTemperatureChange();
	});

	const handleTemperatureChange = function () {
		$.ajax({
			method: "POST",
			url: initData.setTemperatureUrl,
			data: {
				roomId: self.id(),
				desiredTemperature: self.desiredTemperature()
			}
			// TODO: success + fail
		});
	}

	const debouncedhandleTemperatureChange = _.debounce(handleTemperatureChange, 500, false);

	/**
	 * Příznak - vytápí se nebo ne.
	 */
	self.valveOpen = ko.observable(room.valveOpen);
}