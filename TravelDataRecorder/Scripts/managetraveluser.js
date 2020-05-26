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
                       { data: "Summary_ProjectName" },
                       { data: "Summary_ContractNumber" },
                       { data: "SubmittedDate" },
                       { data: "Detail_TravelSitePOC" },
                       { data: "Cost_TotalExpense" },
                       { data: "Detail_AirportDepartingFrom" },
                       { data: "DepartingDate" },
                       { data: "ReturningDate" },
                       { data: "Summary_TravelerName" },
                       { data: "Id" },
                       { data: "LastStatus" }
        ];

        var url = "/travel/GetTravelUsersByAjax";
        if (window.location.href.indexOf("localhost") > -1) {
            url = "/TravelDataRecorder/travel/GetTravelUsersByAjax";
        }


        var oTable = $('#jqueryTravelDataTable').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "orderMulti": true,
            "pageLength": 10,
            "sPaginationType": "full_numbers",
            "bInfo": false, // used to hide Showing 1 to n enteries
            "destroy": true,
            "aaSorting": [[10, 'desc']],
            "ajax": {
                "url": url,
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
                    if (localStorage.getItem("dashboardChart") != null) {
                        d.getFilteredPieChart = localStorage.getItem("dashboardChart");
                    }
                    if (localStorage.getItem("dashboardChartDateFrom") != null) {
                        d.fromDate = localStorage.getItem("dashboardChartDateFrom");
                    }
                    if (localStorage.getItem("dashboardChartDateTo") != null) {
                        d.toDate = localStorage.getItem("dashboardChartDateTo");
                    }
                    //if (localStorage.getItem('dashboardChartCurrentRoleId') != null) {
                    //    d.
                    //}
                }
            },
            "aoColumns": tableColumns,
            "columnDefs": [
            {
                "targets": 9,
                "render": function (data, type, full, meta) {
                    var showprogressUrl = "";
                    var viewtUrl = "";
                    var RequestUrl = "";
                    var RoleName = $("#RoleName").val();
                    var status = full.TravelStatusMaster.Name;
                    var Requesturluser = '<i class="mdi mdi-window-close link-color" title="Reject"></i><a title="Resubmitted"  href=\"travel/viewdata/?travelId=' + data + '\"><i class="mdi mdi-table-edit"></i></a>';
                    viewtUrl = '<a title="View Details" href=\"travel/viewdata/?travelId=' + data + '&viewonly=viewonly\"><i class="mdi mdi-eye"></i></a>';
                    RequestUrl = '<a title="Request" data-toggle="modal" class="link-color" data-target="#mdlDecision" onclick="UpdateHiddenField(' + data + ',' + full.LastStatus + ')">';
                    showprogressUrl = '<a title="Show Progress" href=\"travel/viewdata/?travelId=' + data + '&viewonly=showprogress\"><i class="mdi mdi-backup-restore"></i></a>';
                    if (RoleName == 'User') {
                        if (status == "Submitted By User") {
                            RequestUrl = '<i class="mdi mdi-check link-color" title="Submitted"></i>';
                        }
                        else if (status == "Approved By PM") {
                            RequestUrl = '<i class="mdi mdi-comment-processing-outline link-color" title="Approved By Project Manager"></i>';
                        }
                        else if (status == "Approved By Project Officer") {
                            RequestUrl = '<i class="mdi mdi-comment-processing-outline link-color" title="Approved By Client1"></i>';
                        }
                        else if (status == "Approved By Chief End User") {
                            RequestUrl = '<i class="mdi mdi-comment-processing-outline link-color" title="Approved By Client2"></i>';
                        }
                        else if (status == "Rejected By PM") {
                            RequestUrl = Requesturluser;
                        }
                        else if (status == "Rejected By Project Officer") {
                            RequestUrl = Requesturluser;
                        }
                        else if (status == "Rejected By Chief End User") {
                            RequestUrl = Requesturluser;
                        }
                    }
                    else if (RoleName == 'ProjectManager') {
                        if (status == "Submitted By User" || status == "Resubmitted By User") {
                            RequestUrl += '<i class="mdi mdi-table-edit"></i></a>';
                        }
                        else if (status == "Approved By PM") {
                            RequestUrl = '<i class="mdi mdi-comment-processing-outline link-color" title="In Progress"></i>';
                        }
                        else if (status == "Rejected By PM") {
                            RequestUrl = '<i class="mdi mdi-window-close link-color" title="Reject"></i>';
                        }
                    }
                    else if (RoleName == 'Client 1') {
                        if (status == "Approved By PM") {
                            RequestUrl += '<i class="mdi mdi-table-edit link-color" title="Approved By ProjectManager"></i></a>';
                        }
                        else if (status == "Approved By Project Officer") {
                            RequestUrl = '<i class="mdi mdi-comment-processing-outline link-color" title="Approved By Client1"></i>';
                        }
                        else if (status == "Rejected By Project Officer") {
                            RequestUrl = '<i class="mdi mdi-window-close link-color" title="Reject"></i>';
                        }
                    }
                    else if (RoleName == 'Client 2') {
                        if (status == "Approved By PM") {
                            RequestUrl += '<i class="mdi mdi-table-edit link-color" title="In Progress"></i></a>';
                        }
                        else if (status == "Approved By Project Officer") {
                            RequestUrl += '<i class="mdi mdi-table-edit link-color" title="In Progress"></i></a>';
                        }
                        else if (status == "Rejected By Project Officer") {
                            RequestUrl = '<i class="mdi mdi-window-close link-color" title="Reject"></i></a>';
                        }
                        else if (status == "Rejected By PM") {
                            RequestUrl = '<i class="mdi mdi-window-close link-color" title="Reject"></i></a>';
                        }
                        else if (status == "Approved By Chief End User") {
                            RequestUrl = '<i class="mdi mdi-check link-color" title="Approve"></i></a>';
                        }
                        else if (status == "Rejected By Chief End User") {
                            RequestUrl = '<i class="mdi mdi-window-close link-color" title="Reject"></i></a>';
                        }
                    }
                    return viewtUrl + RequestUrl + showprogressUrl;
                },
            },
           
              {
                  "targets": [9],
                  "bSortable": false,
                  "searching": false,
              },
              //{
              //    "targets": [6],
              //    "render": function (data) {
              //        return formatJSONDate(data);
              //    },
              //},
              {
                  "targets": 10,
                  "render": function (data, type, full, meta) {
                      return "";
                  }
              }
            ],
            "initComplete": function (settings, json) {
                $("#jqueryTravelDataTable_filter").hide(); // Hide default search button of jquery datatable
                $("#jqueryTravelDataTable_length").addClass("m-t-20");
                $("#jqueryTravelDataTable_length").find("select").addClass("form-control form-control-sm");
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
        $('#jqueryTravelDataTable').dataTable().fnFilter(this.value);
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

