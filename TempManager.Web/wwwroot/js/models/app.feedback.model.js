/**
 * Zobrazení histori nastavení podlaží.
 */
function feedbackModel(data) {
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
				page: self.page()
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

	// Načtu data.
	loadData();
}