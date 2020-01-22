

$('#today').on('click' ,(function () {
    $('#example').DataTable().clear().destroy();
    var table = $('#example').DataTable( {
        "processing": true,
        "serverSide": false,
        "ajax": {
            "url": "BookingOverview?handler=test",
            "type": "GET",
            "datatype": "application/json",
            "dataSrc": "bookings",

        },

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": "internalId",
                "defaultContent": '',
                "render": function (data) {
                    return '<i class="fas fa-caret-down" aria-hidden="true"></i> <input type="hidden" name="id" value="' + data + '">';
                },
                width:"15px"
            },
            {
                "data": "bookingTime" , render: function(data ) {
                    return moment(data).format(" DD MM") + '<input type="hidden" name="dateTo" value=" ' + data + ' "/>';
                },
                width:"70px"
            },
            { "data": "externalId" },

            { "data": "totalPallets" },
            { "data": "transporterName" },
            { "data": "email" },
            {"data" : "port"},
            {"data" : "interval.startTime" , render: function(data) {
                    return moment(data).format("HH:MM")
                }},
            {"data" : "actualArrival" , render: function(data){
                    return '<input class="form-control" name="actualArrival" value="' +moment(data).format("HH:mm")+'" type="time"/> '
                } },

            {"data" : "startLoading" , render: function(data){
                    return '<input class="form-control" name="startLoading" value="' + moment(data).format("HH:mm")+'"  type="time"/> '
                }},
            {"data" : "endLoading" , render: function(data){
                    return  ' <input  class="form-control" name="endLoading" value="'+  moment(data).format("HH:mm")   +'" type="time"/>  '
                }},
            {"data" : null , render: function(data){
                    return  '<Button  class=" SubmitButton btn btn-secondary"  type="submit" >Opdater</button> </form> '
                }},


        ]
    } );

    $('#SubmitButton').on('click', function() {
        var data = table.$('input, select').serialize();
        alert(
            "The following data would have been submitted to the server: \n\n"+
            data.substr( 0, 120 )+'...'
        );
        return false;
    } );

    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = table.row(tr);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            tdi.first().removeClass('fas fa-caret-up');
            tdi.first().addClass('fas fa-caret-down');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tdi.first().removeClass('fas fa-caret-down');
            tdi.first().addClass('fas fa-caret-up');
        }
    });

    table.on("user-select", function (e, dt, type, cell, originalEvent) {
        if ($(cell.node()).hasClass("details-control")) {
            e.preventDefault();
        }
    });

} )); 
    