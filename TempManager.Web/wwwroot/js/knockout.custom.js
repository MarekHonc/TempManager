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

ko.bindingHandlers.gauge = {
	init: function (element, valueAccessor) {
		var radius = 20;
		var circumference = 2 * Math.PI * radius;
		var gaugeCircle = element.querySelector('.progress-circle');
		gaugeCircle.style.strokeDasharray = circumference;
		return { controlsDescendantBindings: true };
	},
	update: function (element, valueAccessor) {
		// Očekáváme objekt s { current: ..., min: ..., max: ..., staticColor: (volitelně), enableGlow: (volitelně) }
		var data = ko.unwrap(valueAccessor());
		var current = ko.unwrap(data.current.number());

		var min = data.min;
		var max = data.max;

		if (current > max) {
			current = max;
		}
		if (current < min) {
			current = min;
		}

		// Přepočet aktuální hodnoty na procenta
		var percentage = ((current - min) / (max - min)) * 100;

		var gaugeCircle = element.querySelector('.progress-circle');
		var gaugeText = element.querySelector('.gauge-text');
		var gaugeSvg = element.querySelector('.gauge-svg');

		var radius = 20;
		var circumference = 2 * Math.PI * radius;
		var offset = circumference * (1 - percentage / 100);
		gaugeCircle.style.strokeDashoffset = offset;

		// Volba barvy: pokud je definována statická barva, použije se; jinak se interpoluje od zelené k červené
		if (data.staticColor) {
			gaugeCircle.style.stroke = data.staticColor;
		} else {
			var r = Math.round(255 * (percentage / 100));
			var g = Math.round(255 * (1 - percentage / 100));
			gaugeCircle.style.stroke = 'rgb(' + r + ',' + g + ',0)';
		}

		// Aktualizace textu – zobrazuje aktuální hodnotu
		gaugeText.textContent = ko.unwrap(data.current);

		// Glow efekt: pokud je povolen (enableGlow) a aktuální hodnota dosáhla či překročila max, přidá se glow
		if (data.enableGlow && current >= max) {
			gaugeSvg.classList.add('glow');
		} else {
			gaugeSvg.classList.remove('glow');
		}
	}
};