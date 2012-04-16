/**** Utilities ****/
window.log = function () {
    log.history = log.history || [];
    log.history.push(arguments);
    arguments.callee = arguments.callee.caller;
    if (this.console) console.log(Array.prototype.slice.call(arguments));
};
(function (b) { function c() { } for (var d = "assert,count,debug,dir,dirxml,error,exception,group,groupCollapsed,groupEnd,info,log,markTimeline,profile,profileEnd,time,timeEnd,trace,warn".split(","), a; a = d.pop(); ) b[a] = b[a] || c })(window.console = window.console || {});

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function formatMoney(n) {
    return (isNumber(n) ? n : 0).toFixed(2);
}

/**** Every page load ****/
$(function () {

    if ($.validator) {
        // patch the validate "date" method to accomodate iOS-style ISO dates
        // because somehow iOS supports HTML5 date inputs, but its Date() implementation
        // doesn't parse them... WUT?!
        var originalDateValidator1 = $.validator.methods.date;
        var originalDateValidator2 = $.validator.methods.dateISO;
        $.validator.methods.date = function (value, element) {
            var isValidDate =
            originalDateValidator1.apply(this, arguments) ||
            originalDateValidator2.apply(this, arguments);
            return isValidDate;
        };
    }

    // attach the jquery datepicker unless the current browser has one
    if (!Modernizr.inputtypes.date) {
        $("input[type=date]").datepicker();
    }

    // prompt for confirmation on some buttons
    $("input[type='submit'].confirm-no-undo").click(function () {
        return confirm("Are you sure? There is no undo!");
    });

    // add hover effects to tables
    $('table.ui-widget tr').hover(
        function () { $(this).children('td').addClass('ui-state-hover'); },
        function () { $(this).children('td').removeClass('ui-state-hover'); }
    )
    // and click effects
    .click(function (e) {
        if ($(e.target).is('tr,td,th')) {
            $(this).children('td').toggleClass('ui-state-highlight');
        }
    });

    // make the tabs look nice when hovered
    $('#menu li').hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); });

    // buttonize inputs and buttons to make them look all fancy
    $('input:submit, button, a.button').button();

    // note: if there is no message, this will silently just not do anything :)
    $('#message').delay(50).show('slide', { direction: 'up' }, 'fast').delay(2000).hide('slide', { direction: 'up' }, 'fast');

    if ($.timeago) {
        $('time.timeago').timeago();
    }

    $('.expand-next-element')
        .next().hide().end()
        .on('click', function (e) { $(this).next().slideToggle(); e.preventDefault(); });
});