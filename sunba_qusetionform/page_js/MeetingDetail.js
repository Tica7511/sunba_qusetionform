$(document).ready(function () {
    CreateHourAndMinutes();
    GetMeetingRoomDDL();
    GetPersonDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        multiple: true,
        multipleWidth: 120,
        minimumCountSelected: 20
    });

    getData();


    // 取消預約
    $(document).on("click", "#cancelbtn", function () {
        if (confirm("確定取消?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMeeting.aspx",
                data: {
                    id: $.getQueryString('v')
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
                        location.href = "MeetingManage.aspx";
                    }
                }
            });
        }
    });

    // 儲存
    $(document).on("click", "#savebtn", function () {
        $("#errMsg").empty();
        var msg = '';
        if ($("#m_room").val() == "")
            msg += "請選擇【使用場所】<br>";
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

        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("id", $.getQueryString('v'));


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
                    //location.href = "MeetingManage.aspx";
                    alert("儲存成功");
                }
            }
        });
    });

}); // end js

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMeetingDetail.aspx",
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
                        $("#m_room").val($(this).children("m_room").text().trim());
                        $("#m_date").val($(this).children("m_date").text().trim());
                        var sTime = $(this).children("m_starttime").text().trim().split(":");
                        var eTime = $(this).children("m_endtime").text().trim().split(":");
                        $("#m_starthour").val(sTime[0]);
                        $("#m_startmins").val(sTime[1]);
                        $("#m_endhour").val(eTime[0]);
                        $("#m_endmins").val(eTime[1]);
                        $("#m_desc").val($(this).children("m_desc").text().trim());
                        $("#m_ps").val($(this).children("m_ps").text().trim());

                        // 人員
                        var person = $(this).children("m_participant").text().trim().split(',');
                        $("#ddlPerson").multipleSelect('setSelects', person);
                    });
                }
            }
        }
    });
}

function GetMeetingRoomDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMeetingRoomSelectList.aspx",
        data: {
            category: "meeting"
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