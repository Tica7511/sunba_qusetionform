$(document).ready(function () {
    getApplyData(0);
    getCancelData(0);

    // 檢視
    $(document).on("click", "a[name='viewbtn']", function () {
        location.href = "DormitoryDetail.aspx?v=" + $(this).attr("aid");
    });

    // 同意
    $(document).on("click", "a[name='agreebtn']", function () {
        Flow.SignNext($(this).attr("agid"));
        getApplyData(0);
    });

    // 不同意
    $(document).on("click", "a[name='disagreebtn']", function () {
        Flow.Disagree($(this).attr("agid"));
        getApplyData(0);
    });

    // 確定退宿
    $(document).on("click", "a[name='cancelbtn']", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/DeleteReviewDormitoryCancel.aspx",
            data: {
                id: $(this).attr("aid"),
                empno: $(this).attr("empno"),
                roomno: $(this).attr("room")
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
                    getCancelData(0);
                }
            }
        });
    });
}); // end js

// 宿舍申請
function getApplyData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetReviewDormitoryList.aspx",
        data: {
            Category: "Apply",
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
                var tabstr = '';
                $("#applylist tbody").empty();
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("fm_createdate").text().trim())) + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("fm_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("d_department").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        var result_1 = ResultStr($(this).children("Sign_1").text().trim(), $(this).children("fm_result").text().trim());
                        var result_2 = ResultStr($(this).children("Sign_2").text().trim(), $(this).children("fm_result").text().trim());
                        var result_3 = ($(this).children("fm_category").text().trim() == "DL") ? ResultStr($(this).children("Sign_3").text().trim(), $(this).children("fm_result").text().trim()) : "";
                        var result_4 = ($(this).children("fm_category").text().trim() == "DL") ? ResultStr($(this).children("Sign_4").text().trim(), $(this).children("fm_result").text().trim()) : "";
                        // 宿舍承辦
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_1 + '</td>';
                        // 宿舍承辦主管
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_2 + '</td>';
                        // 行管部總務主管(主任)
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_3 + '</td>';
                        // 行管部主管(經理)
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_4 + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("edataid").text().trim() + '">檢視</a>&nbsp;';
                        var eventBtn = '<a href="javascript:void(0);" name="disagreebtn" agid="' + $(this).children("egid").text().trim() + '">否決</a>&nbsp;';
                        eventBtn += '<a href="javascript:void(0);" name="agreebtn" agid="' + $(this).children("egid").text().trim() + '">同意</a>';
                        if ($(this).children("fm_result").text().trim() == "") {
                            if ($("sa", data).text().trim() == "True")
                                tabstr += eventBtn;
                            else {
                                switch ($(this).children("fms_site").text().trim()) {
                                    case "1":
                                    case "2":
                                        var aryEmpno = $(this).children("SysSigner").text().trim().split(',');
                                        if ($.inArray($("LoginEmpno", data).text(), aryEmpno) > -1) {
                                            tabstr += eventBtn;
                                        }
                                        break;
                                    case "3":
                                    case "4":
                                        if ($("LoginEmpno", data).text() == $(this).children("fms_signperson").text().trim()) {
                                            tabstr += eventBtn;
                                        }
                                        break;
                                }
                            }
                        }
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="9">查詢無資料</td></tr>';

                $("#applylist tbody").append(tabstr);
                Page.Option.FunctionName = "getApplyData";
                Page.Option.Selector = "#apply_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// 退宿申請
function getCancelData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetReviewDormitoryList.aspx",
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
                var tabstr = '';
                $("#cancellist tbody").empty();
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dc_canceldate").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dc_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dr_no").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="cancelbtn" aid="' + $(this).children("dc_id").text().trim() + '" empno="' + $(this).children("dc_createid").text().trim() + '" room="' + $(this).children("dr_no").text().trim() + '">確定退宿</a>';
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


function ResultStr(str, result) {
    var ReturnStr = '';
    switch (str) {
        case "Y":
            ReturnStr = '<span class="text-success">同意</span>';
            break;
        case "N":
            ReturnStr = '<span class="text-danger">否決</span>';
            break;
        default:
            if (result == "N")
                ReturnStr = '';
            else
                ReturnStr = '待審核';
            break;
    }
    return ReturnStr;
}
