function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function formatMoney(n) {
    return (isNumber(n) ? n : 0).toFixed(2);
}

$(function () {
    $("input[type=date]").datepicker();

    $("input[type='submit'].confirm-no-undo").click(function () {
        return confirm("Are you sure? There is no undo!");
    });

    $('table.ui-widget tr').hover(
        function () { $(this).children('td').addClass('ui-state-hover'); },
        function () { $(this).children('td').removeClass('ui-state-hover'); }
    )
    .click(function () { $(this).children('td').toggleClass('ui-state-highlight'); });

    $('#menu li').hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); });

    $('input:submit, button').button();
});