$(document).ready(function () {
    var lo = location.pathname.split('/');
    var page = lo[lo.length - 1];

    switch (page) {
        case "Default.aspx":
            $("#ToBeSignTab").tab("show");
            break;
        case "ReviewMeals.aspx":
            $("#MealsTab").tab("show");
            break;
        case "ReviewOutdoor.aspx":
            $("#OutdoorTab").tab("show");
            break;
        case "ReviewDormitory.aspx":
            $("#DormitoryTab").tab("show");
            break;
        case "ReviewMeetingSituation.aspx":
            $("#MeetingTab").tab("show");
            break;
    }
    
    GetUnreviwedNotify();

    $(document).on("click", ".meunbtn", function () {
        location.href = this.name + ".aspx";
    });
});

function GetUnreviwedNotify() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetUnreviewedCount.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                // 公文與簽辦
                if ($(data).find("uof_item").length == 0)
                    $("#ToBeSignNotify").parent().hide();
                else
                    $("#ToBeSignNotify").html($(data).find("uof_item").length);
                // 其他
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        // 用餐登記
                        if (parseInt($(this).children("m_Unreview").text().trim()) == 0)
                            $("#MealsNotify").parent().hide();
                        else
                            $("#MealsNotify").html($(this).children("m_Unreview").text().trim());
                        // 出廠證明單
                        if (parseInt($(this).children("of_Unreview").text().trim()) == 0)
                            $("#OutdoorNotify").parent().hide();
                        else
                            $("#OutdoorNotify").html($(this).children("of_Unreview").text().trim());
                        // 宿舍申請
                        if (parseInt($(this).children("d_Unreview").text().trim()) == 0)
                            $("#DormitoryNotify").parent().hide();
                        else
                            $("#DormitoryNotify").html($(this).children("d_Unreview").text().trim());
                        // 會議室使用狀況
                        if (parseInt($(this).children("mfb_Unreview").text().trim()) == 0)
                            $("#MeetingNotify").parent().hide();
                        else
                            $("#MeetingNotify").html($(this).children("mfb_Unreview").text().trim());
                    });
                }
            }
        }
    });
}