//<!-- ============================================================== -->
//<!-- AddTravelForm.js -->
//<!-- ============================================================== -->


//call loader function
SetLoading();
//call loader function

function GetFromCity() {
    var FromstateId = $('#traveldetail_Detail_TravelingFromState').val();
    if (FromstateId != "") {
        $.ajax({
            url: "travel/GetCity",
            type: "POST",
            dataType: "JSON",
            data: { stateId: FromstateId },
            success: function (cities) {
                $("#traveldetail_Detail_TravelingFromCity").html("");
                $("#traveldetail_Detail_TravelingFromCity").append(
                        $('<option></option>').val("0").html("Select City"));
                $.each(cities, function (i, city) {

                    $("#traveldetail_Detail_TravelingFromCity").append(
                        $('<option></option>').val(city.Id).html(city.Name));
                });
            }
        });
    }
    else {
        $("#traveldetail_Detail_TravelingFromCity").html("");
        $("#traveldetail_Detail_TravelingFromCity").append(
                $('<option></option>').val("0").html("Select City"));
    }

}
function GetToCity() {
    var TostateId = $('#traveldetail_Detail_TravelingToState').val();
    if (TostateId != "") {
        $.ajax({
            url: "travel/GetCity",
            type: "POST",
            dataType: "JSON",
            data: { stateId: TostateId },
            success: function (Tocities) {
                $("#traveldetail_Detail_TravelingToCity").html("");
                $("#traveldetail_Detail_TravelingToCity").append(
                       $('<option></option>').val("0").html("Select City"));
                $.each(Tocities, function (i, city) {
                    $("#traveldetail_Detail_TravelingToCity").append(
                        $('<option></option>').val(city.Id).html(city.Name));
                });
            }
        });
    }
    else {
        $("#traveldetail_Detail_TravelingToCity").html("");
        $("#traveldetail_Detail_TravelingToCity").append(
               $('<option></option>').val("0").html("Select City"));
    }
}
function GetReturnToCity() {
    var TostateId = $('#traveldetail_Detail_ReturningToState').val();
    if (TostateId != "")
        {
    $.ajax({
        url: "travel/GetCity",
        type: "POST",
        dataType: "JSON",
        data: { stateId: TostateId },
        success: function (ReturnTocities) {
            $("#traveldetail_Detail_ReturningToCity").html("");
            $("#traveldetail_Detail_ReturningToCity").append(
                   $('<option></option>').val("0").html("Select City"));
            $.each(ReturnTocities, function (i, city) {
                $("#traveldetail_Detail_ReturningToCity").append(
                    $('<option></option>').val(city.Id).html(city.Name));
            });
        }
    });
    }
    else {
        $("#traveldetail_Detail_ReturningToCity").html("");
        $("#traveldetail_Detail_ReturningToCity").append(
               $('<option></option>').val("0").html("Select City"));
    }
}


$(function () {
    //for seeting up datatable pagination
    //$(".table-responsive .bottom").insertAfter(".table-responsive");
    //for seeting up datatable pagination
    var currentURL=window.location.href;
    if (currentURL.indexOf('showprogress') > 0 || currentURL.indexOf('showtraveldata') > 0)
    {
        $('.nav .nav-item:nth-child(4)').addClass('active');
    }

    $(".dateDiff").datepicker({
        minDate: 0,
        dateFormat: 'M dd, yy'
    });

    $(".filterCal").datepicker({ dateFormat: 'M dd, yy' });
    $(".dashboardCal").datepicker({ dateFormat: 'M dd, yy' });

    if ($(".filterCal").val() == "01-01-0001" || $(".filterCal").val() == "Jan 1, 0001")
    {
        $(".filterCal").val("mm/dd/yyyy");
    }
    
    $(".filterCal").on("change", function () {
        var fromDate = $('#FromDate').val();
        var toDate = $('#ToDate').val();

        var from = new Date(fromDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        var to = new Date(toDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));

        if (from > to) {
            $('#ToDate').val("mm/dd/yyyy");
            showErrorMessage('"From Date" can not be greater than "To Date"', 4000, 'warningMessage');
        }

    });
    if ($(".dateDiff").val() == "01-01-0001" || $(".dateDiff").val() == "Jan 1, 0001") {
        $(".dateDiff").val("mm/dd/yyyy");
    }
    $(".dateDiff").on("change", function () {
        var fromDate = $('#start_date').val();
        var toDate = $('#end_date').val();
        var d = new Date();

        var month = d.getMonth() + 1;
        var day = d.getDate();

        var TodayDate = d.getFullYear() + '-' +
            (month < 10 ? '0' : '') + month + '-' +
            (day < 10 ? '0' : '') + day;

        var from = new Date(fromDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        var to = new Date(toDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        var today = new Date(TodayDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        today = today.setHours(0, 0, 0, 0);
        var diff = new Date(to - from);
        var days = (diff / 1000 / 60 / 60 / 24) + 1;

        if (days < 0) {
            $('#end_date').val("");
            $("#totaltraveldays").val("");
            showErrorMessage('Departing Date can not be greater than  Returning date', 4000, 'warningMessage');

        } else if (from < today) {
            $('#end_date').val("");
            $("#totaltraveldays").val("");
            showErrorMessage('Selected date must be greater than or equal to today date', 4000, 'warningMessage');
            $(this).val('');
        }
        else if (to < today) {
            $('#end_date').val("");
            $("#totaltraveldays").val("");
            showErrorMessage('Selected date must be greater than or equal to today date', 4000, 'warningMessage');
            $(this).val('');
        }
        else if (to < from) {
            $('#end_date').val("");
            $("#totaltraveldays").val("");
            showErrorMessage('Returning date must be greater than Departing Date', 4000, 'warningMessage');
            $(this).val('');
        }

        else if (days.toString() != 'NaN') {
            var totaldays = days.toString();
            GetCheckDate(totaldays);

        }
    });
    function GetCheckDate(days) {
        var startdate = $('#start_date').val();
        var enddate = $('#end_date').val();
        var result = "";
        $.ajax({
            url: "travel/GetCheckDate",
            type: "POST",
            dataType: "JSON",
            data: { startdate: startdate, enddate: enddate },
            success: function (response) {
                if (response == "false") {
                    result = response;
                    $("#totaltraveldays").val("");
                    $('#end_date').val("");
                    showErrorMessage('You have already submitted form between in these selected dates', 4000, 'warningMessage');
                }
                else {
                    $("#totaltraveldays").val(days);
                }

            }
        });
        return result;
    }





    //for showing only text
    $(".txt").keydown(function (e) {
        if (e.shiftKey || e.ctrlKey || e.altKey || e.keyCode == 9) {
          //  e.preventDefault();
        } else {
            var key = e.keyCode;
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
                e.preventDefault();
            }
        }
    });
    $(".js-allowOnlyTab").keydown(function (e) {
        if (e.which != 9) {
            return false;

        }
    });
  

    $('.amt').keyup(function () {
        var sum = 0;
        $('.amt').each(function () {
            sum += Number($(this).val()
           );
        });
        $('#totalexpenses').val(sum);
    });
});
$('#btnCancel1').click(function () {
    //location.reload(true);
    $('.js-clear').val('');
});

$("#submittravel").click(function (e) {

    if ($('#projectname').val().trim().length == 0) {

        showErrorMessage('Project Name is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#primecontractor').val().trim().length == 0) {
        showErrorMessage('Prime Contractor is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#contractnumber').val().trim().length == 0) {
        showErrorMessage('Contract  Number is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#taskorder').val().trim().length == 0) {
        showErrorMessage('Task Order is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#corname').val().trim().length == 0) {
        showErrorMessage('COR Name is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#travelername').val().trim().length == 0) {
        showErrorMessage('Traveller Name is required', 4000, 'errorMessage');
        e.preventDefault();
    }

    else if ($('#airportdepartingfrom').val().trim().length == 0) {
        showErrorMessage('Airport Departing From is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#travelpurpose').val().trim().length == 0) {
        showErrorMessage('Travel Purpose is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#sitepoc').val().trim().length == 0) {
        showErrorMessage('SITE POC is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_TravelingFromState').val() == 0) {
        showErrorMessage(' Travelling From State is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_TravelingFromCity').val() == 0) {
        showErrorMessage(' Travelling From City is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_TravelingToState').val() == 0) {
        showErrorMessage(' Travelling To State is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_TravelingToCity').val() == 0) {
        showErrorMessage(' Travelling To City is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_ReturningToState').val() == 0) {
        showErrorMessage(' Returning To State is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#traveldetail_Detail_ReturningToCity').val() == 0) {
        showErrorMessage(' Returning To City is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#start_date').val() == "mm/dd/yyyy") {
        showErrorMessage('Departing Date is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#end_date').val() == "mm/dd/yyyy") {
        showErrorMessage('Returning Date is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#totaltraveldays').val().trim().length == 0) {
        showErrorMessage('Total Travel Days is required', 4000, 'errorMessage');
        e.preventDefault();
    }

    else if ($('#costofairfare').val().trim().length == 0) {
        showErrorMessage('Cost Of Airfare is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#perdiemrate').val().trim().length == 0) {
        showErrorMessage('PER DIEM Rate is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#totalperdiem').val().trim().length == 0) {
        showErrorMessage('Total PER DIEM is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#registrationfee').val().trim().length == 0) {
        showErrorMessage('Registration Fee is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#rentalcar').val().trim().length == 0) {
        showErrorMessage('Rental Car is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#hotellodging').val().trim().length == 0) {
        showErrorMessage('HOTEL LODGING is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#miscellaneous').val().trim().length == 0) {
        showErrorMessage('Miscellaneous is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#totalexpenses').val().trim().length == 0) {
        showErrorMessage('Total Expense is required', 4000, 'errorMessage');
        e.preventDefault();
    }

    else if ($('#odcbudgetstatus').val().trim().length == 0) {
        showErrorMessage('ODC Budget Status is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else {
        $("#mainLoader").show();
    }

});
//<!-- ============================================================== -->
//<!-- Login.js -->
//<!-- ============================================================== -->
$('#to-login').click(function () {

    $("#recoverform").hide();
    $("#loginform").fadeIn();
});

$("#loginclick").click(function (e) {
    if ($('#useremail').val() != '') {

        var userinput = $.trim($('#useremail').val());
        var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i
        var userpwd = $.trim($('#userpwd').val());

        if (!pattern.test(userinput)) {
            showErrorMessage('Invalid Email', 4000, 'warningMessage');
            e.preventDefault();
        }
        else if (userpwd.length == 0) {
            showErrorMessage('Password is Required', 4000, 'errorMessage');
            e.preventDefault();
        }
        else {
            $("#mainLoader").show();
        }
    }

    else {
        showErrorMessage("Email address is required", 4000, 'errorMessage');
        e.preventDefault();
    }
});

//$(document).ready(function () {

//    if (!$('#InvalidUsernamePassword').val().trim().length == 0) {
//        showErrorMessage($('#InvalidUsernamePassword').val(), 4000);
//    }
//});


//<!-- ============================================================== -->
//<!-- Profile.js -->
//<!-- ============================================================== -->

$("#ProfileData").submit(function (e) { // note that it's better to use form Id to select form
    //alert('Save Successfully!')
    var emailReg = /^([\w-\.]+@@([\w-]+\.)+[\w-]{2,4})?$/;
    emailReg = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    if ($('#Firstname').val().trim().length == 0) {
        //Email is required
        showErrorMessage('First Name is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#Lastname').val().trim().length == 0) {
        showErrorMessage('Last Name is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#Email').val().trim().length == 0) {
        //Email is required
        showErrorMessage('Email is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if (!emailReg.test($('#Email').val())) {
        showErrorMessage('Invalid email', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#ContactNumber').val().trim().length == 0) {
        showErrorMessage('Contact Number is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else {
        $("#mainLoader").show();
    }
});


//<!-- ============================================================== -->
//<!-- AddUser.js -->
//<!-- ============================================================== -->

$('#adduser').on('click', function (e) {
    var emailReg = /^([\w-\.]+@@([\w-]+\.)+[\w-]{2,4})?$/;
    emailReg = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;

    //if ($('#txtFirstName').val().length == 0) {
    //    showErrorMessage('First Name is required', 4000, 'errorMessage');
    //    e.preventDefault();
    //}
    //if ($('#txtLastName').val().length == 0) {
    //    showErrorMessage('Last Name is required', 4000, 'errorMessage');
    //    e.preventDefault();
    //}
    if ($('#txtEmail').val().length == 0) {
        showErrorMessage('Email address is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if (!emailReg.test($('#txtEmail').val())) {
        showErrorMessage('Invalid email', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if ($('#RoleId').val().trim().length == 0) {
        showErrorMessage('Please Select Role', 4000, 'errorMessage');
        e.preventDefault();
    }
    else {
        $("#mainLoader").show();
    }
});

//<!-- ============================================================== -->
//<!-- GeneratePassword.js -->
//<!-- ============================================================== -->

$('#btnpassword').on('click', function (e) {
    //alert('Alert')
    var NewPass = $("#txtNewPassword").val();
    var ConfirmPass = $("#txtConfirmPassword").val();

    if (NewPass.length == 0) {
        //Email is required
        showErrorMessage(' New Password is required', 4000, 'errorMessage');
        e.preventDefault();
    }
    else if (NewPass.length < 6) {
        showErrorMessage('New Password length should be minimum of six characters', 4000, 'warningMessage');
        e.preventDefault();
    }
    else if (ConfirmPass.trim().length == 0) {
        showErrorMessage('Confirm Password is required', 4000, 'errorMessage');
        e.preventDefault();
    }
   
    else if (ConfirmPass.length < 6) {
       showErrorMessage('Confirm Password length should be minimum of six characters', 4000, 'warningMessage');
       e.preventDefault();
   }
   else if (NewPass != ConfirmPass) {
        //Email is required
       showErrorMessage('New Password and Confirm password does not match', 4000, 'warningMessage');
        e.preventDefault();
    }
});




//<!-- ============================================================== -->
//<!--common function -->
//<!-- ============================================================== -->

function SetLoading() {
    document.onreadystatechange = function () {
        var state = document.readyState;
        if (state == 'interactive') {
            $("#mainLoader").show();
            $("#mainLoader").removeAttr("display");
        } else if (state == 'complete') {
            $("#mainLoader").hide();
        }
    }
}

function showErrorMessage(message, hideAfterTime, messageClass) {
    $(".main-validation").removeClass('errorMessage');
    $(".main-validation").removeClass('successMessage');
    $(".main-validation").removeClass('warningMessage');
    $(".main-validation").addClass(messageClass);
    //close existing popup if any
    $('.main-validation').hide();
    //show the error message
    $('.main-validation').show();
    //display the message
    $('.main-validation .show-msg').text(message);
    //hide error message after specified time
    setTimeout(function () {
        $('.main-validation').fadeOut();
    }, hideAfterTime);
    return false;
}
$(".main-validation i").click(function () {
    $(".main-validation").hide();
});





$('.js-loader').on('click', function () {
    $("#mainLoader").show();
    $(document).keydown(function (event) {
        if (event.which == "17")
            $("#mainLoader").hide();
    }); 
});





