$(document).ready(function () {
    // dll
    GetType();
    getData();
});

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDormitoryDetail.aspx",
        data: {
            id: $.getQueryString("v")
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
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        // info
                        $("#d_name").val($(this).children("d_name").text().trim());
                        $("#d_type").val($(this).children("d_type").text().trim());
                        $("#d_startday").val($(this).children("d_startday").text().trim());
                        $("#d_endday").val($(this).children("d_endday").text().trim());
                        $("#d_reason").val($(this).children("d_reason").text().trim());
                        $("#d_tel").val($(this).children("d_tel").text().trim());
                        $("#d_bloodtype").val($(this).children("d_bloodtype").text().trim());
                        $("#d_emergency_contact").val($(this).children("d_emergency_contact").text().trim());
                        $("#d_emergency_tel").val($(this).children("d_emergency_tel").text().trim());

                        // page control
                        if ($(this).children("d_type").text().trim() == "01") {
                            $(".ItemForA").show();
                            $(".ItemForB").hide();
                        }
                        else {
                            $(".ItemForB").show();
                            $(".ItemForA").hide();
                        }
                    });

                    // File
                    if ($(data).find("file_item").length > 0) {
                        $(data).find("file_item").each(function (i) {
                            var filestr = '<a href="../DOWNLOAD.aspx?v=' + $(this).children("EncodeGuid").text().trim() + '" target="_blank">';
                            filestr += $(this).children("File_Orgname").text().trim() + $(this).children("File_Exten").text().trim();
                            filestr += '</a>';
                            $("#d_attach").append(filestr);
                        });
                    }
                }
            }
        }
    });
}


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