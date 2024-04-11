$(document).ready(function () {
    GetType();
    GetCarSelect();
    getData(0);

    // 查詢
    $(document).on("click", "#SearchBtn", function () {
        getData(0);
    });

    // 檢視
    $(document).on("click", "a[name='viewbtn']", function () {
        location.href = "OutdoorFormDetail.aspx?v=" + $(this).attr("aid");
    });

    // 匯出
    $(document).on("click", "#exportbtn", function () {
        var pmt = 'SearchStr=' + $("#SearchStr").val();
        pmt += '&SearchType=' + $("#SearchType").val();
        pmt += '&SearchCarNo=' + $("#SearchCarNo").val();
        pmt += '&SearchStartDate=' + $("#SearchStartDate").val();
        pmt += '&SearchEndDate=' + $("#SearchEndDate").val();
        pmt += '&SearchActualOut=' + $("#SearchActualOut").val();
        pmt += '&SearchActualBack=' + $("#SearchActualBack").val();

        window.open("../handler/ExportOutdoorManagerList.aspx?" + pmt)
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        var gid = $(this).attr("agid");
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteOutdoorForm.aspx",
                data: {
                    id: $(this).attr("aid")
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
                        Flow.TerminateTask(gid);
                        getData(0);
                    }
                }
            });
        }
    });
}); // end js


function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetOutdoorManagerList.aspx",
        data: {
            PageNo: p,
            PageSize: Page.Option.PageSize,
            SearchStr: $("#SearchStr").val(),
            SearchType: $("#SearchType").val(),
            SearchCarNo: $("#SearchCarNo").val(),
            SearchStartDate: $("#SearchStartDate").val(),
            SearchEndDate: $("#SearchEndDate").val(),
            SearchActualOut: $("#SearchActualOut").val(),
            SearchActualBack: $("#SearchActualBack").val()
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
                $("#tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_createdate").text().trim())) + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("o_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("CarNum").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("o_place").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '出廠:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_starttime").text().trim())) + ' ' + $.FormatTime($(this).children("o_starttime").text().trim()) + "<br>";
                        tabstr += '返廠:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("o_endtime").text().trim())) + ' ' + $.FormatTime($(this).children("o_endtime").text().trim());
                        tabstr += '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        if ($(this).children("ActualOutTime").text().trim() != "")
                            tabstr += '出廠:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("ActualOutTime").text().trim())) + ' ' + $.FormatTime($(this).children("ActualOutTime").text().trim()) + "<br>";
                        if ($(this).children("ActualBackTime").text().trim() != "")
                            tabstr += '返廠:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("ActualBackTime").text().trim())) + ' ' + $.FormatTime($(this).children("ActualBackTime").text().trim());
                        tabstr += '</td>';
                        tabstr += '<td>' + $(this).children("o_reason").text().trim() + '</td>';
                        var result = ResultStr($(this).children("SignResult").text().trim());
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("eid").text().trim() + '">檢視</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("Del_id").text().trim() + '"  agid="' + $(this).children("egid").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="10">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
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
                $("#SearchType").empty();
                var selectstr = '<option value="">全部</option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#SearchType").append(selectstr);

            }
        }
    });
}

function GetCarSelect() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetOfficialCarList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#SearchCarNo").empty();
                var selectstr = '<option value="">全部</option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("oc_number").text().trim() + '">' + $(this).children("oc_number").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#SearchCarNo").append(selectstr);

            }
        }
    });
}

function ResultStr(str) {
    var ReturnStr = '';
    switch (str) {
        case "Y":
            ReturnStr = '<span class="text-success">同意</span>';
            break;
        case "N":
            ReturnStr = '<span class="text-danger">否決</span>';
            break;
        default:
            ReturnStr = '待審核';
            break;
    }
    return ReturnStr;
}