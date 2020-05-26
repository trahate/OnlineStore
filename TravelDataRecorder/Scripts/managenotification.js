$(document).ready(function () {
    if ($('#search').val().length > 0) {
        $('#fafatimes').css('display', 'block');
        $("#faSearch").css('display', 'none');
    }

    $('#search').keyup(function () {
        if ($('#search').val().length == 0) {
            $('#fafatimes').css('display', 'none');
            $("#faSearch").css('display', 'block');
        }
        else {
            $('#fafatimes').css('display', 'block');
            $("#faSearch").css('display', 'none');
        }
    });




    $('#fafatimes').on('click', function () {
        $('#search').val('');
        $('#fafatimes').css('display', 'none');
        $("#faSearch").css('display', 'block');
        BindJqueryDatatable();
    });



    $('#userslist').on('change', function () {
        BindJqueryDatatable();
    });


    /* Load Jquery datatable*/
    function BindJqueryDatatable() {
        var tableColumns = [
                       { data: "Name" },
                       { data: "Message" },
                       { data: "NotificationDate" },
                       { data: "TravelId" }
        ];
        var oTable = $('#jqueryNotificationTable').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "orderMulti": true,
            "pageLength": 10,
            "sPaginationType": "full_numbers",
            "bInfo": false, // used to hide Showing 1 to n enteries
            "destroy": true,
            "aaSorting": [[2, 'desc']],
            "ajax": {
                "url": "user/getnotificationbyajax",
                "type": "POST",
                "datatype": "json",
                "data": function (d) {
                   // d.userType = $("#userslist").val();
                    d.status = sessionStorage.status;
                    d.isactive = sessionStorage.isactive;
                    if ($("#criteria").val().length != 0) {
                        d.criteria = $("#criteria").val();
                    }
                    else {
                        d.criteria = sessionStorage.Criteria;
                    }
                    d.customSearch = $('#search').val();
                }
            },
            "aoColumns": tableColumns,
            "columnDefs": [
            {
                "targets": 3,
                "render": function (data, type, full, meta) {
                    
                    var viewtUrl = "";
                    viewtUrl = '<a title="View Details" class="mdi mdi-eye action" href=\"travel/viewdata/?travelId=' + data + '&viewonly=viewonly&view=view\"></a>';
                  //  viewtUrl = '<a title="View Details" class="mdi mdi-eye action" onclick="RedirectToTravelForm(' + data + ')"></a>';
                    
                    return viewtUrl;
                },
            },
              {
                  "targets": [3],
                  "bSortable": false,
                  "searching": false,
              },
              {
                  "targets": [2],
                  "render": function (data) {
                      return formatJSONDate(data);
                  },
              },
            ],
            "initComplete": function (settings, json) {
                $("#jqueryNotificationTable_filter").hide(); // Hide default search button of jquery datatable
                $("#jqueryNotificationTable_length").addClass("m-t-20");
                $("#jqueryNotificationTable_length").find("select").addClass("form-control form-control-sm");
            },
            "oLanguage": {
                "sLengthMenu": "Page Size: _MENU_ ",
                "sProcessing": "<div id='dvloader_processing'></div>"
            },
            "dom": '<"top"i>rt<"bottom"flp><"clear">' // Change page size position to bottom of table
        });
    }

    BindJqueryDatatable();

    $("#search").on('keyup', function () {
        $('#jqueryNotificationTable').dataTable().fnFilter(this.value);
    });

   

});

function formatJSONDate(jsonDate) {
    var dateString = jsonDate.substr(6); //"\/Date(1334514600000)\/".substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    if (month < 10) {
        month = '0' + month;
    }
    if (day < 10) {
        day = '0' + day
    };
    //var date = year + "-" + month + "-" + day;
    var date = month + "/" + day + "/" + year;
    return date;
}

//function RedirectToTravelForm(id) {
//    localStorage.setItem("ViewTravelForm", id);
//    window.location.replace('travel/showtraveldata');
//}

