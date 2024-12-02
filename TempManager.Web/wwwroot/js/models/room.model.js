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
		$.ajax({
			method: "POST",
			url: initData.saveFavoriteUrl,
			data: {
				roomId: self.id(),
				isFavorite: newValue
			}
		});
	});

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

	/**
	 * Příznak - vytápí se nebo ne.
	 */
	self.valveOpen = ko.observable(room.valveOpen);
}