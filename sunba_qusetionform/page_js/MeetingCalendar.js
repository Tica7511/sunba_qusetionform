$(document).ready(function () {
    GetMeetingRoomList();
    GetFeedBackList();
    GetMeetingRoomDDL();

    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialDate: $.datepicker.formatDate('yy-mm-dd', new Date()),
        initialView: 'timeGridWeek',
        //contentHeight: 600,
        slotMinTime: "07:00:00",
        slotMaxTime: "21:00:00",
        allDaySlot: false,//取消全天顯示
        editable: false,
        selectable: true,//可拖拉時間操作dialog
        dayMaxEvents: true, // allow "more" link when too many events
        businessHours: {
            // days of week. an array of zero-based day of week integers (0=Sunday)
            daysOfWeek: [0, 1, 2, 3, 4, 5, 6],
            startTime: '7:00',
            endTime: '21:00',
        },
        events: function (info, successCallback, failureCallback) {
            var EventData = [];
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetMeetingCalendar.aspx",
                data: {
                    id: $.getQueryString("v"),
                    roomid: $("#tmpRoomid").val()
                },
                error: function (xhr) {
                    alert("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                complete: function () {
                    successCallback(EventData);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                EventData.push({
                                    title: $(this).children("m_desc").text().trim(),
                                    start: $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("m_date").text().trim())) + ' ' + $(this).children("m_starttime").text().trim(),
                                    end: $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("m_date").text().trim())) + ' ' + $(this).children("m_endtime").text().trim(),
                                    url: 'MeetingDetail.aspx?v=' + $(this).children("eid").text().trim(),
                                    extendedProps: {
                                        description: '<div class="text-start">申請人：' + $(this).children("m_createname").text().trim() + '</div>'
                                    }
                                });
                                $("#RoomName").html($(this).children("mr_name").text().trim());
                            });
                        }
                    }
                }
            });
        },
        //tooltip程式
        eventDidMount: function (info) {
            $(info.el).tooltip({
                title: info.event.extendedProps.description,
                placement: "top",
                trigger: "hover",
                container: "body",
                html: true,
            });
        },
        //拖拉選取時段
        select: function (info) {
            var datestart = info.start.toLocaleDateString('zh-TW') + '\xa0' + info.start.toLocaleTimeString('zh-TW', { hour12: false, hour: '2-digit', minute: '2-digit' });
            var datesend = info.end.toLocaleDateString('zh-TW') + '\xa0' + info.end.toLocaleTimeString('zh-TW', { hour12: false, hour: '2-digit', minute: '2-digit' });
            $('#meetingstart').html(datestart);
            $('#meetingend').html(datesend);
            $("#MeetingRoom").val($("#tmpRoomid").val());
            $("#MeetingDate").val($.datepicker.formatDate('yy-mm-dd', new Date(info.start)));
            $("#sTime").val($.FormatTime(info.start));
            $("#eTime").val($.FormatTime(info.end));
            $('#roomsetting').modal('show');
        }
    });

    calendar.render();


    $(document).on("click", "#subbtn", function () {
        // Get form
        var form = document.getElementById('reServeForm');
        form.submit();
    });

    $(document).on("click", "a[name='mRoombtn']", function () {
        $("#tmpRoomid").val(this.id);
        $("a[name='mRoombtn']").removeClass("active");
        $(this).addClass("active");
        calendar.refetchEvents();
    });

    // open 狀況回覆 modal
    $(document).on("click", "#openfeedbackbtn", function () {
        $(".newstr").val("");
        $("#feedbackModal").modal("show");
    });

    // 新增回饋
    $(document).on("click", "#feedbackBtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMeetingFeedBack.aspx",
            data: {
                mfb_room: $("#ddlMeetingRoom").val(),
                mfb_date: $("#feedback_Date").val(),
                mfb_content: $("#feedback").val()
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
                    GetFeedBackList();
                    $("#feedbackModal").modal("hide");
                }
            }
        });
    });
}); // end js

// 會議室列表
function GetMeetingRoomList() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCalendarRoomList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        complete: function () {
            successCallback(EventData);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#mRoomList").empty();
                var divstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        if (i == 0) {
                            divstr += '<a href="javascript:void(0);" name="mRoombtn" class="list-group-item list-group-item-action active" aria-current="true" id="' + $(this).children("mr_guid").text().trim() + '">' + $(this).children("mr_name").text().trim() + '</a>';
                            $("#tmpRoomid").val($(this).children("mr_guid").text().trim());
                        }
                        else
                            divstr += '<a href="javascript:void(0);" name="mRoombtn" class="list-group-item list-group-item-action" aria-current="true" id="' + $(this).children("mr_guid").text().trim() + '">' + $(this).children("mr_name").text().trim() + '</a>';
                    });
                }
                $("#mRoomList").append(divstr);
            }
        }
    });
}

// 會議室使用狀況列表
function GetFeedBackList() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMeetingFeedBackList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#feedbackList").empty();
                var str = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        str += '<li class="list-group-item"><div class="small">' + $(this).children("mfb_date").text().trim() + '</div>' + $(this).children("mr_name").text().trim();
                        str += '<div class="small fw-bold">' + $(this).children("mfb_content").text().trim() + '</div>';
                        str += '</li>';
                    });
                }
                $("#feedbackList").append(str);
            }
        }
    });
}

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
                $("#ddlMeetingRoom").empty();
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
                $("#ddlMeetingRoom").append(selectstr);
            }
        }
    });
}