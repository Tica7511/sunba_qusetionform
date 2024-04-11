$(document).ready(function () {
    getVisitorData(0);
    getCancelData(0);

    // 同意
    $(document).on("click", "a[name='agreebtn']", function () {
        Flow.SignNext($(this).attr("agid"));
        if (this.id == "v_agree") {
            getVisitorData(0);
        }
        else {
            CancelMeals($(this).attr("agid"));
            getCancelData(0);
        }
    });

    // 不同意
    $(document).on("click", "a[name='disagreebtn']", function () {
        Flow.Disagree($(this).attr("agid"));
        if (this.id == "v_disagree")
            getVisitorData(0);
        else
            getCancelData(0);
    });
}); // end js

function getVisitorData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetReviewMealsList.aspx",
        data: {
            Category: "Visitor",
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
                $("#visitorlist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("fm_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mv_name").text().trim() + '</td>';
                        tabstr += '<td>' + $(this).children("mv_reason").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mv_date").text().trim() + '</td>';
                        tabstr += '<td class="text-center">葷(' + $(this).children("mv_lunch_meat").text().trim() + ')、奶蛋素(' + $(this).children("mv_lunch_vegetarian").text().trim() + ')、全素(' + $(this).children("mv_lunch_vegan").text().trim() + ')</td>';
                        tabstr += '<td class="text-center">葷(' + $(this).children("mv_dinner_meat").text().trim() + ')、奶蛋素(' + $(this).children("mv_dinner_vegetarian").text().trim() + ')、全素(' + $(this).children("mv_dinner_vegan").text().trim() + ')</td>';
                        var result = ResultStr($(this).children("fm_result").text().trim());
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        if ($(this).children("fm_result").text().trim() == "") {
                            tabstr += '<a href="javascript:void(0);" id="v_disagree" name="disagreebtn" agid="' + $(this).children("egid").text().trim() + '">否決</a>&nbsp;';
                            tabstr += '<a href="javascript:void(0);" id="v_agree" name="agreebtn" agid="' + $(this).children("egid").text().trim() + '">同意</a>';
                        }
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                $("#visitorlist tbody").append(tabstr);
                Page.Option.FunctionName = "getVisitorData";
                Page.Option.Selector = "#visitor_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

function getCancelData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetReviewMealsList.aspx",
        data: {
            Category: "Cancel",
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
                $("#cancellist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("fm_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_reason").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_item").text().trim() + '</td>';
                        var result = ResultStr($(this).children("fm_result").text().trim());
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        if ($(this).children("fm_result").text().trim() == "") {
                            tabstr += '<a href="javascript:void(0);" id="c_disagree" name="disagreebtn" agid="' + $(this).children("egid").text().trim() + '">否決</a>&nbsp;';
                            tabstr += '<a href="javascript:void(0);" id="c_agree" name="agreebtn" agid="' + $(this).children("egid").text().trim() + '">同意</a>';
                        }
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                $("#cancellist tbody").append(tabstr);
                Page.Option.FunctionName = "getCancelData";
                Page.Option.Selector = "#cancel_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

function CancelMeals(v) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/CancelMeals.aspx",
        data: {
            gid: v
        },
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
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
