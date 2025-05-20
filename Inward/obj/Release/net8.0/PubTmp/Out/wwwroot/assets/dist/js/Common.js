const ResponseMsgType = {
    success: 1,
    error: 2,
    warning: 3,
    info: 4
}

//function ShowDynamicSwalAlert(title, msg) {
//    const myArray = msg.split("||");
//    msg = myArray[0];
//    var type = myArray[1];
//    if (msg != null && msg != '') {
//        if (type.toString() == ResponseMsgType.success.toString()) {
//            debugger;
//            swal({
//                title: title,
//                text: msg,
//                type: "success",
//                showCancelButtonClass: "btn-primary",
//                confirmButtonText: 'OK'
//            }).then(function (result) {
//                if (result.value) {
//                    // User clicked "OK," so redirect to another page
//                    window.location = '/EmployeeTravelReqDetails/AddEmployeeTravelReqDetail';
//                }
//            });

//        }
//        else if (type.toString() == ResponseMsgType.error.toString()) {
//            swal({
//                title: title,
//                text: msg,
//                type: "error",
//                showCancelButtonClass: "btn-primary",
//                confirmButtonText: 'OK'
//            });
//        }
//        else if (type.toString() == ResponseMsgType.warning.toString()) {
//            swal({
//                title: title,
//                text: msg,
//                type: "warning",
//                showCancelButtonClass: "btn-primary",
//                confirmButtonText: 'OK'
//            });
//        }
//        else if (type.toString() == ResponseMsgType.info.toString()) {
//            swal({
//                title: title,
//                text: msg,
//                type: "info",
//                showCancelButtonClass: "btn-primary",
//                confirmButtonText: 'OK'
//            });
//        }
//    }
//}
//$(function () {
//    $('.datepicker').datepicker({
//        changeMonth: true,
//        changeYear: true,
//        format: "dd/mm/yyyy",
//        language: "local",
//        todayHighlight: 'TRUE',
//        autoclose: true
//    });
//});
//$(function () {
//    $(".pastdisdatepicker").datepicker({
//        changeMonth: true,
//        changeYear: true,
//        format: "dd/mm/yyyy",
//        language: "local",
//        todayHighlight: 'TRUE',
//        autoclose: true,
//        startDate: new Date()
//    }).datepicker();
//});

function AllowNumeric(e) {


    var keyCode = e.which ? e.which : e.keyCode

    if (!((keyCode >= 48 && keyCode <= 57) || keyCode == 46)) {

        return false;
    } else {

    }

}

function AllowAlphaNumeric(e) {


    var k = e.keyCode || e.which;
    var ok = k >= 65 && k <= 90 || // A-Z
        k >= 96 && k <= 105 || // a-z
        k >= 37 && k <= 40 || // arrows
        k == 9 || //tab
        k == 46 || //del
        k == 8 || // backspaces
        (!e.shiftKey && k >= 48 && k <= 57); // only 0-9 (ignore SHIFT options)

    if (!ok || (e.ctrlKey && e.altKey)) {
        e.preventDefault();
    }

}
function funCancelForm(controllerName, actionName) {
    debugger;
    window.location.href = "/" + controllerName + "/" + actionName;
}

function GetTalukaByDistrictId(districtId) {
    debugger;
    $.ajax({
        type: "get",
        url: "/Common/GetTalukaByDistrictId",
        data: { districtId: districtId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var TalukaList = "";
            console.log(data.data.result.length);
            TalukaList = TalukaList + '<option value="">- Please Select Taluka -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                TalukaList = TalukaList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listTaluka').html(TalukaList);
        }
    });
}
function GetBlockByTalukaId(talukaId) {
    debugger;
    $.ajax({
        type: "get",
        url: "/Common/GetBlockByTalukaId",
        data: { talukaId: talukaId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var BlockList = "";
            console.log(data.data.result.length);
            BlockList = BlockList + '<option value="">- Please Select Block -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                BlockList = BlockList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listBlock').html(BlockList);
        }
    });
}
function GetSejaByBlockId(blockId) {
    debugger;
    $.ajax({
        type: "get",
        url: "/Common/GetSejaByBlockId",
        data: { blockId: blockId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var SejaList = "";
            console.log(data.data.result.length);
            SejaList = SejaList + '<option value="">- Please Select Seja -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                SejaList = SejaList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listSeja').html(SejaList);
        }
    });
}
function GetVillageBySejaId(sejaId) {
    debugger;
    $.ajax({
        type: "get",
        url: "/Common/GetVillageBySejaId",
        data: { sejaId: sejaId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var VillageList = "";
            console.log(data.data.result.length);
            VillageList = VillageList + '<option value="">- Please Select Village -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                VillageList = VillageList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listVillage').html(VillageList);
        }
    });
}
function GetAanganwadiByVillageId(villageId) {
    debugger;
    $.ajax({
        type: "get",
        url: "/Common/GetAanganwadiByVillageId",
        data: { villageId: villageId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var AanganwadiList = "";
            console.log(data.data.result.length);
            AanganwadiList = AanganwadiList + '<option value="">- Please Select Aanganwadi -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                AanganwadiList = AanganwadiList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listAanganwadi').html(AanganwadiList);
        }
    });
}