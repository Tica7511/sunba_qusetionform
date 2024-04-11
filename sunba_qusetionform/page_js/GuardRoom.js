$(document).ready(function () {
    getData(0);

    /// 表頭排序
    $(document).on("click", "a[name='sortbtn']", function () {
        $("a[name='sortbtn']").removeClass("asc desc")
        if (Page.Option.SortName != $(this).attr("sortname")) {
            Page.Option.SortMethod = "-";
        }
        Page.Option.SortName = $(this).attr("sortname");
        if (Page.Option.SortMethod == "-") {
            Page.Option.SortMethod = "+";
            $(this).addClass('asc');
        }
        else {
            Page.Option.SortMethod = "-";
            $(this).addClass('desc');
        }

        getData(0);
    });

    setInterval(function () {
        getData(0);
    }, 300000); // 1000 = 1秒

    // 儲存
    $(document).on("click", "button[name='savebtn']", function () {
        var pid = $(this).attr("apid");
        var carno = $(this).attr("carno");
        var thisRow = $(this).closest(".card-body");
        var starttime = thisRow.find("input[name='OutTime']").val();
        var endtime = thisRow.find("input[name='BackTime']").val();

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddOfficialCarRecord.aspx",
            data: {
                pid: pid,
                ocr_car: carno,
                ocr_outtime: starttime,
                ocr_backtime: endtime
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
                    alert($("Response", data).text());
                    getCarList();
                }
            }
        });
    });
}); // end js

function getData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetGuardRoomList.aspx",
        data: {
            PageNo: p,
            PageSize: Page.Option.PageSize,
            SortName: Page.Option.SortName,
            SortMethod: Page.Option.SortMethod
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
                $("#datalist").empty();
                var datastr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        datastr += '<div class="col"><div class="card">';
                        datastr += '<div class="card-header text-center">';
                        datastr += '<div class="fs-3">' + $(this).children("CarNum").text().trim() + '</div>';
                        datastr += '<div>申請人：' + $(this).children("o_createname").text().trim() + '</div>';
                        datastr += '</div>';
                        datastr += '<div class="card-body">';
                        datastr += '<div class="ochiform TitleLength06">';
                        datastr += '<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">';
                        datastr += '<div class="col-md-auto TitleSetWidth text-md-end"><label class="form-label">預計出廠時間</label></div>';
                        var oTime = new Date($(this).children("StartAll").text().trim());
                        var OutStr = $(this).children("startdate").text().trim() + ' ' + FormatAmPm(oTime.getHours()) + ' ' + $(this).children("starttime").text().trim();
                        datastr += '<div class="col-md-auto flex-grow-1">' + OutStr + '</div>';
                        datastr += '</div>';
                        datastr += '<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">';
                        datastr += '<div class="col-md-auto TitleSetWidth text-md-end"><label class="form-label">實際出廠時間</label></div>';
                        datastr += '<div class="col-md-auto flex-grow-1">';
                        datastr += '<input class="form-control" type="time" name="OutTime" value="' + $(this).children("ActualOutTime").text().trim() + '" />';
                        datastr += '</div></div>';
                        datastr += '<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">';
                        datastr += '<div class="col-md-auto TitleSetWidth text-md-end"><label class="form-label">預計回廠時間</label></div>';
                        var bTime = new Date($(this).children("EndAll").text().trim());
                        var BackStr = $(this).children("enddate").text().trim() + ' ' + FormatAmPm(bTime.getHours()) + ' ' + $(this).children("endtime").text().trim();
                        datastr += '<div class="col-md-auto flex-grow-1">' + BackStr + '</div>';
                        datastr += '</div>';
                        datastr += '<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">';
                        datastr += '<div class="col-md-auto TitleSetWidth text-md-end"><label class="form-label">實際回廠時間</label></div>';
                        datastr += '<div class="col-md-auto flex-grow-1">';
                        datastr += '<input class="form-control" type="time" name="BackTime" value="' + $(this).children("ActualBackTime").text().trim() + '" />';
                        datastr += '</div></div></div>';
                        datastr += '<div class="d-grid gap-2 mt-2">';
                        datastr += '<button class="btn btn-primary" type="button" name="savebtn" apid="' + $(this).children("fm_data_guid").text().trim() + '" carno="' + $(this).children("oc_guid").text().trim() + '">儲存</button>';
                        datastr += '</div></div></div></div>';
                    });
                }
                else
                    datastr = '<span style="margin-left:5px;">查無外出單(公務車)申請</span>';
                $("#datalist").append(datastr);

                $("#tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("all_item").length > 0) {
                    $(data).find("all_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("C_Item_cn").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("startdate").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("o_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("CarNum").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("o_place").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">出廠:' + $(this).children("startdate").text().trim() + ' ' + $(this).children("starttime").text().trim() + '<br>';
                        tabstr += '返廠:' + $(this).children("enddate").text().trim() + ' ' + $(this).children("endtime").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("o_reason").text().trim() + '</td>';
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

function FormatAmPm(t) {
    var str = (t >= 12) ? "下午" : "上午";
    return str;
}