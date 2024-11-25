/*
 * Drobné utilitky, které se mohou hodit pro práci s KO.
 * 
 * BINDINGS:
 * fade - jako visible, ale dělá fade-in/fade-out
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