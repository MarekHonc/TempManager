/*
 * Drobné utilitky, které se mohou hodit pro práci s KO.
 * 
 * BINDINGS:
 * fade			- jako visible, ale dělá fade-in/fade-out
 * slideVisible	- jako visible, jen dělá slide (up/down) efekt
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