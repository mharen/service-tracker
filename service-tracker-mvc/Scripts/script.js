function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function formatMoney(n) {
    return (isNumber(n) ? n : 0).toFixed(2);
}

$(function () {
    if (!Modernizr.inputtypes.date) {
        $("input[type=date]").datepicker();
    }

    $("input[type='submit'].confirm-no-undo").click(function () {
        return confirm("Are you sure? There is no undo!");
    });

    $('table.ui-widget tr').hover(
        function () { $(this).children('td').addClass('ui-state-hover'); },
        function () { $(this).children('td').removeClass('ui-state-hover'); }
    )
    .click(function (e) {
        if ($(e.target).is('input,textarea,select')) {
            return;
        }
        $(this).children('td').toggleClass('ui-state-highlight');
    });

    $('#menu li').hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); });

    $('input:submit, button').button();

    // note: if there is no message, this will silently just not do anything :)
    $('#message').delay(50).show('slide', { direction: 'up' }, 'fast').delay(2000).hide('slide', { direction: 'up' }, 'fast');
});