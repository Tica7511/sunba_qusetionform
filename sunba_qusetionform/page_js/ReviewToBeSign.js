$(document).ready(function () {
    getFormList();

    // 第三方登入
    $('#btn_signForm').click(function () {
        ThirdLogin();
    });
});

// 取得待審核表單列表
function getFormList() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../Handler/GetReviewToBeSignList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#tobesignlist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        // 待簽表單名稱
                        tabstr += '<td class="text-center text-nowrap">' + $(this).children("FORM_NAME").text().trim() + '</td>';
                        // 申請日期
                        tabstr += '<td class="text-center text-nowrap">' + $(this).children("START_TIME").text().trim().substring(0, 10).replaceAll("-", "/") + " " + $(this).children("START_TIME").text().trim().substring(11, 19) + '</td>';
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="2">查詢無資料</td></tr>';
                $("#tobesignlist tbody").append(tabstr);
            }
        }
    });
}