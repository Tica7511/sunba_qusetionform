$(document).ready(function () {
    // dll
    GetType();

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9-]/g;
        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });

    // 申請類別 change
    $(document).on("change", "#d_type", function () {
        var tmp = $(this).val();
        if (tmp == "01") {
            $(".ItemForA").show();
            $(".ItemForB").hide();
        }
        else {
            $(".ItemForB").show();
            $(".ItemForA").hide();
        }
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        $("#errMsg").empty();
        var msg = '';
        if ($("#d_type").val() == "01") {
            if (!$("#cb_check").is(":checked"))
                msg += "請閱讀並同意宿舍管理要點<br>";
            if ($("#d_file").val() == "")
                msg += "請上傳戶口名簿印本<br>";
        }
        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        // disabled Request 抓不到值
        $("#d_name").prop("disabled", false);  

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        //data.append("mode", mode);


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddDormitory.aspx",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            error: function (xhr) {
                $("#errMsg").html("Error: " + xhr.status);
                console.log(xhr.responseText);
            },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    $("#errMsg").html($(data).find("Error").attr("Message"));
                }
                else {
                    var dType = "";
                    if ($("#d_type").val() == "01") 
                        dType = "DL";
                    else
                        dType = "DS";

                    Flow.SendForm($("DataGuid", data).text(), dType);
                    alert("宿舍申請成功");
                    location.href = "DormitoryRegister.aspx";
                }
            }
        });
    });
}); // end js

function GetType() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            group: "02"
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
                $("#d_type").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#d_type").append(selectstr);
                
            }
        }
    });
}