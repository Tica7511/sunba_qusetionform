$(document).ready(function () {
    GetYearAndMonthDDL();
    getData(0);

    // 年&月 change
    $(document).on("change", "#ddlYear,#ddlMonth", function () {
        getData(0);
    });
}); // end js

function getData(p) {
    Page.Option.PageSize = 7;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsStatisticsList.aspx",
        data: {
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val(),
            PageNo: p,
            PageSize: Page.Option.PageSize
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
                    var locationTotal = parseInt($("locationTotal", data).text());
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        if (i % locationTotal == 0)
                            tabstr += '<td class="text-center align-middle" rowspan="' + locationTotal + '">' + $(this).children("mr_date").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("ml_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("lunchE").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("lunchF").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("lunchL").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("lunchV").text().trim() + '</td>';
                        if (i % locationTotal == 0)
                            tabstr += '<td class="text-center align-middle" rowspan="' + locationTotal + '">' + $(this).children("totalLunchPrice").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dinnerE").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dinnerF").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dinnerL").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dinnerV").text().trim() + '</td>';
                        if (i % locationTotal == 0)
                            tabstr += '<td class="text-center align-middle" rowspan="' + locationTotal + '">' + $(this).children("totalDinnerPrice").text().trim() + '</td>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="12">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// ddl 年&月
function GetYearAndMonthDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsPaymentController.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var thisYear = new Date().getFullYear();
                var thisMonth = new Date().getMonth() + 1;
                $("#ddlYear").empty();
                var selectstr = '';
                if ($(data).find("year_item").length > 0) {
                    $(data).find("year_item").each(function (i) {
                        if ($(this).children("strYear").text().trim() == thisYear)
                            selectstr += '<option value="' + $(this).children("strYear").text().trim() + '" selected>' + $(this).children("strYear").text().trim() + '</option>';
                        else
                            selectstr += '<option value="' + $(this).children("strYear").text().trim() + '">' + $(this).children("strYear").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#ddlYear").append(selectstr);

                $("#ddlMonth").empty();
                var selectstr = '';
                if ($(data).find("month_item").length > 0) {
                    $(data).find("month_item").each(function (i) {
                        if ($(this).children("monthNum").text().trim() == thisMonth)
                            selectstr += '<option value="' + $(this).children("monthNum").text().trim() + '" selected>' + $(this).children("strMonth").text().trim() + '</option>';
                        else
                            selectstr += '<option value="' + $(this).children("monthNum").text().trim() + '">' + $(this).children("strMonth").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#ddlMonth").append(selectstr);
            }
        }
    });
}
