$(document).ready(function () {
    getData(0);

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        if (confirm("確認送出")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMeetingFeedBack.aspx",
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
                        GetUnreviwedNotify();
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
        url: "../handler/GetReviewMeetingFeedBack.aspx",
        data: {
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
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("mr_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("mfb_date").text().trim())) + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">' + $(this).children("mfb_createname").text().trim() + '</td>';
                        tabstr += '<td class="align-middle" nowrap="nowrap">' + $(this).children("mfb_content").text().trim() + '</td>';
                        tabstr += '<td class="text-center align-middle" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("mfb_id").text().trim() + '">已處理完成</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}