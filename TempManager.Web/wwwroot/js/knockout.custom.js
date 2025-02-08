/*
 * Drobné utilitky, které se mohou hodit pro práci s KO.
 * 
 * BINDINGS:
 * fade						- jako visible, ale dělá fade-in/fade-out
 * slideVisible				- jako visible, jen dělá slide (up/down) efekt
 * ko.numericObservable		- proměnná pro číselné typy s desetinnou čárkou - umožňuje nastavit formát v inputu a bindovat hodnotu na js číslo
 */

ko.bindingHandlers.fade = {
	init: function (element, valueAccessor) {
		var value = valueAccessor();
		$(element).toggle(ko.utils.unwrapObservable(value));
	},
	update: function (element, valueAccessor) {
		var value = valueAccessor();
		ko.utils.unwrapObservable(value) ? $(element).fadeIn(200) : $(element).fadeOut(200);
	}
};

ko.bindingHandlers.slideVisible = {
	init: function (element, valueAccessor) {
		var value = valueAccessor();
		$(element).toggle(ko.utils.unwrapObservable(value));
	},
	update: function (element, valueAccessor) {
		var value = valueAccessor();
		ko.utils.unwrapObservable(value) ? $(element).slideDown(200) : $(element).slideUp(200);
	}
};

ko.numericObservable = function (initValue, format) {
	format = format || "0.00";

	var currentValue = ko.observable(initValue);
	var getCurrentFormatted = function () {
		if (currentValue() == null)
			return "";

		return numeral(currentValue()).format(format);
	}

	var lastInput = ko.observable(getCurrentFormatted());
	var result = ko.computed({
		read: function () {
			var read = getCurrentFormatted();
			return read;
		},
		write: function (value) {
			lastInput(value);

			var num = numeral(value).value();
			if (!isNaN(num)) {
				currentValue(num);
			}
		}
	});

	result.subscribe(function (newValue) {
		if (typeof newValue === "undefined")
			return;

		result(newValue);
	});

	result.number = function (newValue) {
		if (arguments.length === 1) {
			currentValue(newValue);
		}

		return currentValue();
	}

	result.isInvalid = ko.computed(function () {
		var val1 = lastInput();
		var val2 = result();

		return val1 !== val2;
	});

	return result;
}
