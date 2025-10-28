(function () {
    if (!window.jQuery || !jQuery.validator) return;

    var number = jQuery.validator.methods.number;
    jQuery.validator.methods.number = function (value, element) {
        if (typeof value === "string") value = value.replace(',', '.');
        return number.call(this, value, element);
    };

    var range = jQuery.validator.methods.range;
    jQuery.validator.methods.range = function (value, element, param) {
        if (typeof value === "string") value = value.replace(',', '.');
        return range.call(this, value, element, param);
    };
})();
