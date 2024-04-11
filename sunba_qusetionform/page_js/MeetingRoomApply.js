$(document).ready(function () {
    CreateHourAndMinutes();
    $("#m_starttime").val($("#sTime").val());
    $("#m_endtime").val($("#eTime").val());
    GetMeetingRoomDDL();
    GetPersonDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        multiple: true,
        multipleWidth: 120,
        minimumCountSelected: 20
    });

    // 申請類別 change
    $(document).on("change", "#ApplyCategory", function () {
        var tmp = $(this).val();
        if (tmp == "01") {
            $(".ItemForA").show();
            $(".ItemForB").hide();
        }
        else {
            $(".ItemForB").show();
            $(".ItemForA").hide();
        }
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        $("#errMsg").empty();
        var msg = '';
        if ($("#m_room").val() == "")
            msg += "請選擇【使用場所】<br>";
        if ($("#ApplyCategory").val() == "01") {
            if ($("#m_date").val() == "")
                msg += "請輸入【使用日期】<br>";
            if ($("#m_starthour").val() == "" || $("#m_startmins").val() == "" || $("#m_endhour").val() == "" || $("#m_endmins").val() == "")
                msg += "請選擇【使用時段】<br>";
            else {
                var sTime = $("#m_starthour").val().toString() + $("#m_startmins").val().toString();
                var eTime = $("#m_endhour").val().toString() + $("#m_endmins").val().toString();
                if (parseInt(sTime) >= parseInt(eTime))
                    msg += "【使用時段】選擇有誤，請重新確認<br>";
            }
        }
        else {
            if ($("#cycle_sdate").val() == "" || $("#cycle_edate").val() == "")
                msg += "請輸入【日期區間】<br>";
            else {
                if (new Date($("#cycle_sdate").val()) > new Date($("#cycle_edate").val()))
                    msg += "【日期區間】選擇有誤，請重新確認<br>";
            }
            if ($("#week").val() == "")
                msg += "請選擇【星期】<br>";
            if ($("#cycle_shour").val() == "" || $("#cycle_smins").val() == "" || $("#cycle_ehour").val() == "" || $("#cycle_emins").val() == "")
                msg += "請選擇【使用時段】<br>";
            else {
                var sTime = $("#cycle_shour").val().toString() + $("#cycle_smins").val().toString();
                var eTime = $("#cycle_ehour").val().toString() + $("#cycle_emins").val().toString();
                if (parseInt(sTime) > parseInt(eTime))
                    msg += "【使用時段】選擇有誤，請重新確認<br>";
            }
        }

        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        //data.append("mode", mode);


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMeeting.aspx",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            error: function (xhr) {
                $("#errMsg").html("Error: " + xhr.status);
                console.log(xhr.responseText);
            },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    $("#errMsg").html($(data).find("Error").attr("Message"));
                }
                else {
                    alert($("Response", data).text());
                    $(".newstr").val("");
                    $("#ddlPerson").multipleSelect('setSelects', []);
                    $("#errMsg").html("");
                }
            }
        });
    });

}); // end js

function GetMeetingRoomDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMeetingRoomSelectList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#m_room").empty();
                var selectstr = '';
                if ($(data).find("dataList").length > 0) {
                    selectstr += '<optgroup label="">';
                    selectstr += '<option value="">-- 請選擇 --</option>';
                    selectstr += '</optgroup>';
                    $(data).find("dataList").each(function (i) {
                        selectstr += '<optgroup label="' + $(this).attr("area").trim() + '">';
                        if ($(this).find("data_item").length > 0) {
                            $(this).find("data_item").each(function (i) {
                                selectstr += '<option value="' + $(this).attr("roomid").trim() + '">' + $(this).attr("room").trim() + '</option>';
                            });
                        }
                        selectstr += '</optgroup>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#m_room").append(selectstr);
                if ($("#tmpRoomid").val() != "")
                    $("#m_room").val($("#tmpRoomid").val());
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