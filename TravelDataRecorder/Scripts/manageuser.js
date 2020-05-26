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
                       { data: "FirstName" },
                       { data: "LastName" },
                       { data: "Email" },
            { data: "ContactNumber" },
            { data: "Address" },

            { data: "RoleName" },
                       { data: "ID" }
        ];
        var oTable = $('#jqueryDataTable').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "orderMulti": true,
            "pageLength": 10,
            "sPaginationType": "full_numbers",
            "bInfo": false, // used to hide Showing 1 to n enteries
            "destroy": true,
            "aaSorting": [[0, 'asc']],
            "ajax": {
                "url": "admin/GetUsersByAjax",
                "type": "POST",
                "datatype": "json",
                "data": function (d) {
                    d.userType = $("#userslist").val();
                    if ($("#criteria").val().length != 0) {
                        d.criteria = $("#criteria").val();
                    }
                    else {
                        d.criteria = sessionStorage.Criteria;
                    }
                    d.customSearch = $('#search').val();
                    if (localStorage.getItem("dashboardChartDateFrom") != null) {
                        d.fromDate = localStorage.getItem("dashboardChartDateFrom");
                    }
                    if (localStorage.getItem("dashboardChartDateTo") != null) {
                        d.toDate = localStorage.getItem("dashboardChartDateTo");
                    }
                }
            },
            "aoColumns": tableColumns,
            "columnDefs": [
            {
                "targets": 6,
                "render": function (data, type, full, meta) {
                    var editUrl = "";
                    var deleteUrl = "";
                    editUrl = '<a title="Edit" class="mdi mdi-lead-pencil action" href=\"user/viewprofile/?userid=' + data + '&edituser=edituser\"></a>';
                    deleteUrl = '<a title="Delete" style="cursor: pointer;" class="mdi mdi-window-close link-color" onclick="DeleteConferm(' + data + ')"></a>';

                    return editUrl + deleteUrl;
                },
            },
              {
                  "targets": [6],
                  "bSortable": false,
                  "searching": false,
              },
              //{
              //    "targets": [6],
              //    "render": function (data) {
              //        return formatJSONDate(data);
              //    },
              //},
            ],
            "initComplete": function (settings, json) {
                $("#jqueryDataTable_filter").hide(); // Hide default search button of jquery datatable
                $("#jqueryDataTable_length").addClass("m-t-20");
                $("#jqueryDataTable_length").find("select").addClass("form-control form-control-sm");
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
        $('#jqueryDataTable').dataTable().fnFilter(this.value);
    });

});

function formatJSONDate(jsonDate) {
    //var dateString = jsonDate.substr(6); //"\/Date(1334514600000)\/".substr(6);
    //var currentTime = new Date(parseInt(dateString));
    //var month = currentTime.getMonth() + 1;
    //var day = currentTime.getDate();
    //var year = currentTime.getFullYear();
    //if (month < 10) {
    //    month = '0' + month;
    //}
    //if (day < 10) {
    //    day = '0' + day
    //};
    ////var date = year + "-" + month + "-" + day;
    //var date = month + "/" + day + "/" + year;
    //return date;
}


