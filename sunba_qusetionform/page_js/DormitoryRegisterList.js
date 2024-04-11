$(document).ready(function () {
    getData(0);

    // 檢視
    $(document).on("click", "a[name='viewbtn']", function () {
        location.href = "DormitoryDetail.aspx?v=" + $(this).attr("aid");
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        var gid = $(this).attr("agid");
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteDormitoryRegister.aspx",
                data: {
                    gid: gid
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
});

function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDormitoryRegisterList.aspx",
        data: {
            SearchStr: $("#SearchStr").val(),
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
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("d_createdate").text().trim())) + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("d_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("d_department").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        var result_1 = ResultStr($(this).children("Sign_1").text().trim(), $(this).children("SignDate_1").text().trim());
                        var result_2 = ResultStr($(this).children("Sign_2").text().trim(), $(this).children("SignDate_1").text().trim());
                        var result_3 = ResultStr($(this).children("Sign_3").text().trim(), $(this).children("SignDate_1").text().trim());
                        var result_4 = ResultStr($(this).children("Sign_4").text().trim(), $(this).children("SignDate_1").text().trim());
                        result_3 = ($(this).children("d_type").text().trim() == "01") ? result_3 : "";
                        result_4 = ($(this).children("d_type").text().trim() == "01") ? result_4 : "";
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_1 + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_2 + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_3 + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + result_4 + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("eid").text().trim() + '">檢視</a>&nbsp;&nbsp;';
                        if ($(this).children("fm_result").text().trim() == "")
                            tabstr += '<a href="javascript:void(0);" name="delbtn" agid="' + $(this).children("egid").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="9">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}


function ResultStr(str, signdate) {
    var ReturnStr = '';
    switch (str) {
        case "Y":
            ReturnStr = '<span class="text-success">同意</span>(' + $.datepicker.formatDate('yy-mm-dd', new Date(signdate)) + ')';
            break;
        case "N":
            ReturnStr = '<span class="text-danger">否決</span>(' + $.datepicker.formatDate('yy-mm-dd', new Date(signdate)) + ')';
            break;
        default:
            ReturnStr = '待審核';
            break;
    }
    return ReturnStr;
}