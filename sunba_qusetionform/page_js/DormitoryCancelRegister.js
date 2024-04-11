$(document).ready(function () {
    CheckIsApplied();

    // 送出
    $(document).on("click", "#savebtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddDormitoryCancel.aspx",
            data: {
                category: "01",
                canceldate: $("#dc_canceldate").val(),
                reason: $("#dc_reason").val()
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
                    alert($("Response", data).text());
                    $("#dc_reason").val("");
                    CheckIsApplied();
                }
            }
        });
    });
}); // end js

function CheckIsApplied() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDormitoryCancelApplied.aspx",
        data: {
            category: "01",
            reason: $("#dc_reason").val()
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
                if ($("Response", data).text() == "Y") {
                    $("#savebtn").hide();
                }
            }
        }
    });
}