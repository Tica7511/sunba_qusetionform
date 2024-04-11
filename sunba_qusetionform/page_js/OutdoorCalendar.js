$(document).ready(function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
        initialView: 'resourceTimeGridDay',
        allDaySlot: false,
        eventTimeFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        },
        resources: function (info, successCallback, failureCallback) {
            var ResourcesData = [];
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetOutdoorCalendar.aspx",
                data: {
                    mode: "car"
                },
                error: function (xhr) {
                    alert("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                complete: function () {
                    successCallback(ResourcesData);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ResourcesData.push({
                                    id: $(this).children("oc_id").text().trim(),
                                    title: $(this).children("oc_number").text().trim()
                                });
                            });
                        }
                    }
                }
            });
        },
        events: function (info, successCallback, failureCallback) {
            var EventData = [];
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetOutdoorCalendar.aspx",
                data: {
                    mode: "Calendar"
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
                                var desc = '<div class="text-start">申請人：' + $(this).children("o_createname").text().trim();
                                desc += '<br>出廠：' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_starttime").text().trim())) + ' ' + $.FormatTime($(this).children("o_starttime").text().trim());
                                desc += '<br>返廠：' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_endtime").text().trim())) + ' ' + $.FormatTime($(this).children("o_endtime").text().trim());
                                desc += '<br>事由：' + $(this).children("o_reason").text().trim();
                                desc += '</div>';
                                EventData.push({
                                    resourceId: $(this).children("oc_id").text().trim(),
                                    title: $(this).children("o_createname").text().trim(),
                                    start: $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_starttime").text().trim())) + ' ' + $.FormatTime($(this).children("o_starttime").text().trim()),
                                    end: $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_endtime").text().trim())) + ' ' + $.FormatTime($(this).children("o_endtime").text().trim()),
                                    extendedProps: {
                                        description: desc
                                    }
                                });
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
        }
    });

    calendar.render();
});
