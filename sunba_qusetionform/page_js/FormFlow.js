var Flow = {
    // 開單
    SendForm: function (v, category, signer) {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/FormFlowController.aspx",
            data: {
                method: "SendForm",
                dataGuid: v,
                formType: category,
                signer: signer
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
    },
    // 抽單
    TerminateTask: function (v) {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/FormFlowController.aspx",
            data: {
                method: "TerminateTask",
                dataGuid: v
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
    },
    // 同意
    SignNext: function (v) {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/FormFlowController.aspx",
            data: {
                method: "SignNext",
                dataGuid: v
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
                }
            }
        });
    },
    // 否決
    Disagree: function (v) {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/FormFlowController.aspx",
            data: {
                method: "Disagree",
                dataGuid: v
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
                }
            }
        });
    }
} // end js
