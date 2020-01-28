$(function() {
    $("input.date-and-time").daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        timePicker: true,
        timePicker24Hour : true,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(32, 'hour'),
        showDropdowns: true,
        minYear: 1901,
        maxYear: new Date().getFullYear()+1,

    });

    $("input.date-and-time").on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY:HH:mm'));
    });

});




$(function() {
    $("input.date").daterangepicker({
        singleDatePicker: true,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(32, 'hour'),
        showDropdowns: true,
        minYear: 1901,
        maxYear: new Date().getFullYear()+1
    });
    $("input.date").on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY'));
    });
});


$(function() {
    $("input.time").daterangepicker({
        timePicker : true,
        singleDatePicker:true,
        timePicker24Hour : true,
        timePickerIncrement : 1,
        timePickerSeconds : true,
        locale : {
            format : 'HH:mm:ss'
        }
    }).on('show.daterangepicker', function(ev, picker) {
        picker.container.find(".calendar-table").hide();
    });

    $("input.time").on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('HH:mm'));
    });
});


$(function() {
    $("input.date-range").daterangepicker({
        opens: 'left'
    });

    $("input.date-range").on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('dd/MM/yyyy - dd/MM/yyyy'));
    });
    
    
});