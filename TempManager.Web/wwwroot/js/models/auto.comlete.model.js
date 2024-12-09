/**
 * Autocomplete modelu.
 * @param {any} initData inicializační data modelu.
 */
function autoCompleteViewModel(initData) {
	let self = this;

	/**
	 * Aktuálně zobrazené položky.
	 */
	self.data = ko.observableDictionary();

	for (const [key, value] of Object.entries(initData.values)) {
		self.data.set(key, value);
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
			self.data.set(ui.item.id, ui.item.userName);
		}
	}).each(function (i, input) {
		$(input).data("ui-autocomplete")._renderItem = function (ul, item) {
			console.log(item);
			return $("<li>")
				.append("<div>" + item.userName + "</div>")
				.appendTo(ul);
		};
	});
}