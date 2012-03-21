function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function formatMoney(n) {
    return (isNumber(n) ? n : 0).toFixed(2);
}

$(function () {

    // patch the validate "date" method to accomodate iOS dates
    var originalDateValidator = $.validator.methods.date;
    $.validator.methods.date = function (value, element) {
        var originalDateResult = originalDateValidator.apply(this, arguments);
        var dateParts = value.split(/[-]/);
        var isValidDate = originalDateResult || !/Invalid|NaN/.test(new Date(dateParts[0], dateParts[1] - 1, dateParts[2]));

        return isValidDate;
    };

    if (!Modernizr.inputtypes.date) {
        $("input[type=date]").datepicker().attr("novalidate", "novalidate");
    }

    $("input[type='submit'].confirm-no-undo").click(function () {
        return confirm("Are you sure? There is no undo!");
    });

    $('table.ui-widget tr').hover(
        function () { $(this).children('td').addClass('ui-state-hover'); },
        function () { $(this).children('td').removeClass('ui-state-hover'); }
    )
    .click(function (e) {
        if ($(e.target).is('tr,td,th')) {
            $(this).children('td').toggleClass('ui-state-highlight');
        }
    });

    $('#menu li').hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); });

    $('input:submit, button').button();

    // note: if there is no message, this will silently just not do anything :)
    $('#message').delay(50).show('slide', { direction: 'up' }, 'fast').delay(2000).hide('slide', { direction: 'up' }, 'fast');
});