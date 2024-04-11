$(document).ready(function () {
    GetYearAndMonthDDL();
    getTotalData();
    getEmployeeData(0);
    getCompanyData(0);
    getVisitorData(0);

    // 年&月 change
    $(document).on("change", "#ddlYear,#ddlMonth", function () {
        getTotalData();
        getEmployeeData(0);
        getCompanyData(0);
        getVisitorData(0);
    });

    // 查詢
    $(document).on("click", "#EmployeeSearchBtn,#CompanySearchBtn,#VisitorSearchBtn", function () {
        switch (this.id) {
            case "EmployeeSearchBtn":
                getEmployeeData(0);
                break;
            case "CompanySearchBtn":
                getCompanyData(0);
                break;
            case "VisitorSearchBtn":
                getVisitorData(0);
                break;
        }
    });

    // 明細
    $(document).on("click", "a[name='viewbtn']", function () {
        $("#tmpid").val($(this).attr("aid"));
        getData(0);
        $("#DetailModal").modal("show");
    });

    // 匯出
    $(document).on("click", "#EmployeeExportBtn,#CompanyExportBtn,#VisitorExportBtn", function () {
        var type = "";
        var str = "";
        switch (this.id) {
            case "EmployeeExportBtn":
                type = "Employee";
                str = encodeURIComponent($("#EmployeeSearchStr").val());
                break;
            case "CompanyExportBtn":
                type = "Company";
                str = encodeURIComponent($("#CompanySearchStr").val());
                break;
            case "VisitorExportBtn":
                type = "Visitor";
                str = encodeURIComponent($("#VisitorSearchStr").val());
                break;
        }
        window.open("../handler/ExportMealsPayment.aspx?category=" + type + "&year=" + $("#ddlYear").val() + "&month=" + $("#ddlMonth").val() + "&SearchStr=" + str);
    });
}); // end js

// 餐費統計總表
function getTotalData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetPaymentTotalList.aspx",
        data: {
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val()
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
                $("#totallist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("Employee").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("Firm").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("Love").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("Visitor").text().trim() + '</td>';
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                $("#totallist tbody").append(tabstr);
            }
        }
    });
}

// 同仁餐費統計表
function getEmployeeData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsPaymentList.aspx",
        data: {
            category: "Employee",
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val(),
            SearchStr: $("#EmployeeSearchStr").val(),
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
                $("#employeelist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        var tmpMod = i % 2;
                        if (tmpMod == 0) {
                            tabstr += '<tr>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mr_person_id").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("EmpDept").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("EmpName").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("SumPrice").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("EncodeID").text().trim() + '">明細</a>';
                            tabstr += '</td>';

                            // 最後一筆
                            if ($(data).find("data_item").length == (i + 1)) {
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td></tr>';
                            }
                        }
                        else {
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mr_person_id").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("EmpDept").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("EmpName").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("SumPrice").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("EncodeID").text().trim() + '">明細</a>';
                            tabstr += '</td></tr>';
                        }
                    });
                }
                else
                    tabstr += '<tr><td colspan="10">查詢無資料</td></tr>';
                $("#employeelist tbody").append(tabstr);
                Page.Option.FunctionName = "getEmployeeData";
                Page.Option.Selector = "#employee_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// 廠商餐費統計表
function getCompanyData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsPaymentList.aspx",
        data: {
            category: "Company",
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val(),
            SearchStr: $("#CompanySearchStr").val(),
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
                $("#companylist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        var tmpMod = i % 2;
                        if (tmpMod == 0) {
                            tabstr += '<tr>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_name").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_category").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("SumPrice").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("EncodeID").text().trim() + '">明細</a>';
                            tabstr += '</td>';

                            if ($(data).find("data_item").length == (i + 1)) {
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                                tabstr += '<td class="text-center" nowrap="nowrap"></td></tr>';
                            }
                        }
                        else {
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_name").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_category").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("SumPrice").text().trim() + '</td>';
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("EncodeID").text().trim() + '">明細</a>';
                            tabstr += '</td></tr>';
                        }
                    });
                }
                else
                    tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                $("#companylist tbody").append(tabstr);
                Page.Option.FunctionName = "getCompanyData";
                Page.Option.Selector = "#company_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// 訪客餐費統計表
function getVisitorData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsPaymentList.aspx",
        data: {
            category: "Visitor",
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val(),
            SearchStr: $("#VisitorSearchStr").val()
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
                $("#visitlist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mv_date").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mv_createname").text().trim() + '</td>';
                        tabstr += '<td nowrap="nowrap">' + $(this).children("lunchNum").text().trim() + '</td>';
                        tabstr += '<td nowrap="nowrap">' + $(this).children("dinnerNum").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("SumPrice").text().trim() + '</td>';
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                $("#visitlist tbody").append(tabstr);
                Page.Option.FunctionName = "getVisitorData";
                Page.Option.Selector = "#visit_pageblock";
                Page.CreatePage(p, $("total", data).text());
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
            year: $("#ddlYear").val(),
            month: $("#ddlMonth").val(),
            person_id: $("#tmpid").val()
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
                Page.Option.FunctionName = "getData";
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