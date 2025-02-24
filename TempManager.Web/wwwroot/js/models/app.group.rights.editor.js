function userRightsGroupModel(data) {
	var self = this;

	self.columns = ko.observableArray(_.map(data.columns, function (c) { return new editableHeader(c); }));

	self.rows = ko.observableArray(_.map(data.rows, function (r) { return new editableHeader(r); }));

	self.cells = ko.observableDictionary();

	self.getKey = function (userName, roomId) {
		return userName + ";" + roomId;
	}

	self.selectedRow = ko.observable(0);
	self.selectedColumn = ko.observable(0);

	self.getSelectedCell = function () {
		var userName = self.rows()[self.selectedRow()].title();
		var room = self.columns()[self.selectedColumn()].title();

		var key = self.getKey(userName, room);
		var value = self.cells.get(key)();

		if (!value) {
			value = new cellModel({
				UserName: userName,
				ExternalRoomId: room
			});
			self.cells.set(key, value);
		}

		return value;
	}

	self.selectCell = function (column, row) {
		var selected = self.selectedRow() == row && self.selectedColumn() == column;

		if (selected) {
			self.getSelectedCell().toggle();
		}
		else {
			self.selectedColumn(column);
			self.selectedRow(row);
		}
	}

	self.add = function (isColumn) {
		var newHeader = new editableHeader();
		newHeader.isEdit(true);

		if (isColumn) {
			self.columns.push(newHeader);
		}
		else {
			self.rows.push(newHeader);
		}
	}

	window.addEventListener("keydown", function (event) {
		var row = self.selectedRow();
		var col = self.selectedColumn();
		var preventEvents = false;

		switch (event.key) {
			case "ArrowUp":
				if (row > 0)
					self.selectedRow(row - 1);

				preventEvents = true;
				break;
			case "ArrowDown":
				if (row < self.rows().length - 1)
					self.selectedRow(row + 1);

				preventEvents = true;
				break;
			case "ArrowLeft":
				if (col > 0)
					self.selectedColumn(col - 1);

				preventEvents = true;
				break;
			case "ArrowRight":
				if (col < self.columns().length - 1)
					self.selectedColumn(col + 1);

				preventEvents = true;
				break;
			case " ":
				if (event.target.tagName.toUpperCase() !== 'INPUT') {
					self.getSelectedCell().toggle();
					preventEvents = true;
				}

				break;
		}

		if (preventEvents) {
			var element = document.getElementsByClassName("outline")[0];
			element.scrollIntoView();

			event.preventDefault();
			event.stopImmediatePropagation();
			return false;
		}
	});

	for (var i = 0; i < data.cells.length; i++) {
		for (var j = 0; j < data.cells[i].length; j++) {
			var current = data.cells[i][j];
			var currentModel = new cellModel(current);

			self.cells.set(
				self.getKey(currentModel.userName(), currentModel.externalRoomId()),
				currentModel
			);
		}
	}
}

function editableHeader(title) {
	var self = this;

	self.title = ko.observable(title);

	self.isEdit = ko.observable(false);
}

/**
 * Model pro jednotlivou buňku.
 */
function cellModel(data) {
	var self = this;

	self.userIsDeleted = ko.observable(data.UserIsDeleted);

	self.userName = ko.observable(data.UserName);

	self.externalRoomId = ko.observable(data.ExternalRoomId);

	self.isFavorite = ko.observable(data.IsFavorite);

	self.canEdit = ko.observable(data.CanEdit);

	self.canView = ko.observable(data.CanView);

	self.toggle = function () {
		var canView = self.canView();
		var canEdit = self.canEdit();

		if (!canView && !canEdit) {
			self.canView(true);
			self.canEdit(true);
		}
		else if (canView && canEdit) {
			self.canEdit(false);
		}
		else {
			self.canView(false);
			self.canEdit(false);
		}
	}
}