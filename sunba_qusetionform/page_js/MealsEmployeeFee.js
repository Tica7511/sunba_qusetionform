$(document).ready(function () {
    getFeeList();

    // Detail
    $(document).on("click", "a[name='viewbtn']", function () {
        $("#tmpYear").val($(this).attr("aYear"));
        $("#tmpMonth").val($(this).attr("aMonth"));
        getData(0);
        $('#DetailModal').modal('show');
    });
}); // end js

function getFeeList() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsEmployeeFeeList.aspx",
        data: {
            year: $("#ddlYear").val()
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
                // ddl
                if ($("#ddlYear option").length == 0) {
                    $("#ddlYear").empty();
                    var selectstr = '';
                    if ($(data).find("year_item").length > 0) {
                        $(data).find("year_item").each(function (i) {
                            selectstr += '<option value="' + $(this).children("selYear").text().trim() + '">' + $(this).children("selYear").text().trim() + '</option>';
                        });
                    }
                    else
                        selectstr = '<option value="">查無用餐登記資料</option>';
                    $("#ddlYear").append(selectstr);
                }

                // Table
                $("#tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center">' + FormatMonth($(this).children("strMonth").text().trim()) + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("numSum").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("priceSum").text().trim() + '</td>';
                        tabstr += '<td class="text-center">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" aYear="' + $(this).children("strYear").text().trim() + '" aMonth="' + $(this).children("strMonth").text().trim() + '">明細</a>';
                        tabstr += '</td></tr>';
                    });
                }
                $("#tablist tbody").append(tabstr);
            }
        }
    });
}


function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsEmployeeFeeDetail.aspx",
        data: {
            PageNo: p,
            PageSize: Page.Option.PageSize,
            year: $("#tmpYear").val(),
            month: $("#tmpMonth").val()
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
                $("#detaillist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_date").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_lunch").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_lunch_num").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_lunch_location").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_dinner").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_dinner_num").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mr_dinner_location").text().trim() + '</td>';
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
                $("#detaillist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

function FormatMonth(strMonth) {
    switch (strMonth) {
        case "01":
            return "一月";
            break;
        case "02":
            return "二月";
            break;
        case "03":
            return "三月";
            break;
        case "04":
            return "四月";
            break;
        case "05":
            return "五月";
            break;
        case "06":
            return "六月";
            break;
        case "07":
            return "七月";
            break;
        case "08":
            return "八月";
            break;
        case "09":
            return "九月";
            break;
        case "10":
            return "十月";
            break;
        case "11":
            return "十一月";
            break;
        case "12":
            return "十二月";
            break;
    }
}