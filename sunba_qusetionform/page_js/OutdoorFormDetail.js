$(document).ready(function () {
    CreateHourAndMinutes();
    GetType();
    GetPersonDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        multiple: true,
        multipleWidth: 120,
        minimumCountSelected: 20
    });

    getData();

}); // end js

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetOutdoorFormDetail.aspx",
        data: {
            id: $.getQueryString("v")
        },
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        // info
                        $("#o_applydate").val($.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_createdate").text().trim())));
                        $("#o_type").val($(this).children("o_type").text().trim());
                        $("#o_startdate").val($.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_starttime").text().trim())));
                        var sHour = new Date($(this).children("o_starttime").text().trim()).getHours();
                        sHour = sHour < 10 ? "0" + sHour : sHour;
                        $("#o_starthour").val(sHour);
                        var sMinute = new Date($(this).children("o_starttime").text().trim()).getMinutes();
                        sMinute = sMinute < 10 ? "0" + sMinute : sMinute;
                        $("#o_startmins").val(sMinute);
                        $("#o_enddate").val($.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_endtime").text().trim())));
                        var eHour = new Date($(this).children("o_endtime").text().trim()).getHours();
                        eHour = eHour < 10 ? "0" + eHour : eHour;
                        $("#o_endhour").val(eHour);
                        var eMinute = new Date($(this).children("o_endtime").text().trim()).getMinutes();
                        eMinute = eMinute < 10 ? "0" + eMinute : eMinute;
                        $("#o_endmins").val(eMinute);
                        $("#CarNo").html($(this).children("CarNum").text().trim());
                        $("#o_place").val($(this).children("o_place").text().trim());
                        $("#o_reason").val($(this).children("o_reason").text().trim());
                        $("#o_ps").val($(this).children("o_ps").text().trim());

                        if ($(this).children("o_type").text().trim() == "02") {
                            $("#o_passenger_number").val($(this).children("o_passenger_number").text().trim());
                            // 共乘同仁
                            var person = $(this).children("o_passenger_empno").text().trim().split(',');
                            $("#ddlPerson").multipleSelect('setSelects', person);
                            $(".ItemForB").show();
                        }
                        else
                            $(".ItemForB").hide();
                    });
                }
            }
        }
    });
}


function GetType() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            group: "05"
        },
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#o_type").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#o_type").append(selectstr);

            }
        }
    });
}

function GetPersonDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetPersonSelectList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#ddlPerson").empty();
                var selectstr = '';
                if ($(data).find("dataList").length > 0) {
                    $(data).find("dataList").each(function (i) {
                        selectstr += '<optgroup label="' + $(this).attr("depName").trim() + '">';
                        if ($(this).find("data_item").length > 0) {
                            $(this).find("data_item").each(function (i) {
                                selectstr += '<option value="' + $(this).attr("empNo").trim() + '">' + $(this).attr("empName").trim() + '</option>';
                            });
                        }
                        selectstr += '</optgroup>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#ddlPerson").append(selectstr);
            }
        }
    });
}


function CreateHourAndMinutes() {
    var HourStr = '<option value="">--</option>';
    for (var h = 0; h < 24; h++) {
        var hour = (h < 10 ? '0' : '') + h;
        HourStr += '<option value="' + hour + '">' + hour + '</option>';
    }
    $(".timeHour").append(HourStr);

    var MinStr = '<option value="">--</option>';
    for (var m = 0; m < 60; m++) {
        var mins = (m < 10 ? '0' : '') + m;
        MinStr += '<option value="' + mins + '">' + mins + '</option>';
    }
    $(".timeMin").append(MinStr);
}