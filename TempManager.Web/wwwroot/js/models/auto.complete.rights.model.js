/**
 * Autocomplete modelu.
 * @param {any} initData inicializační data modelu.
 */
function autoCompleteRightViewModel(initData) {
	let self = this;

	/**
	 * Aktuálně zobrazené položky.
	 */
	self.data = ko.observableDictionary();

	/**
	 * Odebere záznam ze slovníku.
	 */
	self.remove = function (entry) {
		self.data.remove(entry.key());

		if (initData.removeCallback) {
			initData.removeCallback(entry.key());
		}
	}

	// Inicializace slovníku.
	for (const [key, value] of Object.entries(initData.values)) {
		self.data.set(key, new userRightModel(value));
	}

	/**
	 * Autocomplete na vyplnění prvků.
	 */
	$(initData.autoComplete).autocomplete({
		appendTo: initData.autoCompleteHolder,
		source: function (request, response) {
			$.ajax({
				url: initData.autoCompleteUrl,
				data: {
					search: request.term
				},
				success: function (data) {
					response(data);
				}
			});
		}, autoFocus: true,
		select: function (event, ui) {
			event.preventDefault();
			self.data.set(
				ui.item.id,
				new userRightModel({
					Title: ui.item.name,
					CanEdit: true,
					CanView: true
				})
			);

			if (initData.selectCallback) {
				initData.selectCallback(ui.item.id);
			}
		}
	}).each(function (i, input) {
		$(input).data("ui-autocomplete")._renderItem = function (ul, item) {
			return $("<li>")
				.append("<div>" + item.name + "</div>")
				.appendTo(ul);
		};
	});
}

/**
 * Model pro zobrazení konkrétního práva.
 */
function userRightModel(initData) {
	var self = this;

	/**
	 * Zobrazení titulek.
	 */
	self.title = ko.observable(initData.Title);

	/**
	 * Může editovat.
	 */
	self.canEdit = ko.observable(initData.CanEdit);

	/**
	 * Může číst.
	 */
	self.canView = ko.observable(initData.CanView);
}