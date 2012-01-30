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
});