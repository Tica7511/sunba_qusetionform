$(document).ready(function () {
    CreateHourAndMinutes();
    GetType();
    GetPersonDDL();
    GetSignersDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        multiple: true,
        multipleWidth: 120,
        minimumCountSelected: 20
    });

    // 申請類別 change
    $(document).on("change", "#o_type", function () {
        var tmp = $(this).val();
        if (tmp == "01") {
            $(".ItemForA").show();
            $(".ItemForB").hide();
        }
        else {
            $(".ItemForA").hide();
            $(".ItemForB").show();
            $("#tablist").hide();
            $("#instructions").show();
        }
    });

    // 時間 change
    $(document).on("change", "#o_startdate,#o_starthour,#o_startmins,#o_enddate,#o_endhour,#o_endmins", function () {
        $("#tablist tbody").empty();
        $("#tablist").hide();
        $("#instructions").show();
    });

    $(document).on("click", "#CarSearchBtn", function () {
        $("#errMsg").empty();

        var sTime = $("#o_startdate").val() + " " + $("#o_starthour").val() + ":" + $("#o_startmins").val();
        var eTime = $("#o_enddate").val() + " " + $("#o_endhour").val() + ":" + $("#o_endmins").val();

        var msg = '';
        if ($("#o_starthour").val() == "" || $("#o_startmins").val() == "" || $("#o_endhour").val() == "" || $("#o_endmins").val() == "")
            msg += "請選擇【日期與時間區間】<br>";
        else {
            if (new Date(sTime) > new Date(eTime))
                msg += "【日期與時間區間】選擇有誤，請重新確認<br>";
        }

        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetCarApplyList.aspx",
            data: {
                stime: sTime,
                etime: eTime
            },
            error: function (xhr) {
                $("#errMsg").html("Error: " + xhr.status);
                console.log(xhr.responseText);
            },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    $("#errMsg").html($(data).find("Error").attr("Message"));
                }
                else {
                    $("#tablist tbody").empty();
                    var tabstr = '';
                    if ($(data).find("OfficialCar ").length > 0) {
                        $(data).find("OfficialCar ").each(function (i) {
                            tabstr += '<tr>';
                            // 勾選
                            if ($(this).find("data_item").length > 0)
                                tabstr += '<td class="text-center" nowrap="nowrap"></td>';
                            else
                                tabstr += '<td class="text-center" nowrap="nowrap"><input class="form-check-input" type="radio" name="o_car" value=' + $(this).attr("oc_guid").trim() + ' /></td>';
                            // 車號
                            tabstr += '<td class="text-center" nowrap="nowrap" style="vertical-align:middle;">' + $(this).attr("oc_number").trim() + '</td>';
                            // 使用狀況
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            var itemStr = '';
                            if ($(this).find("data_item").length > 0) {
                                $(this).find("data_item").each(function (i) {
                                    if (itemStr != "") itemStr += "<br>";
                                    itemStr += '已預約。使用者:' + $(this).attr("o_createname").trim() + ' 預約時間:';
                                    itemStr += '(' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).attr("o_starttime").trim())) + ' ' + $.FormatTime($(this).attr("o_starttime").trim()) + ' ~ ';
                                    itemStr += $.datepicker.formatDate('yy-mm-dd', new Date($(this).attr("o_endtime").trim())) + ' ' + $.FormatTime($(this).attr("o_endtime").trim()) + ')';
                                });
                            }
                            else
                                itemStr += '可使用';
                            tabstr += itemStr;
                            tabstr += '</td></tr>';
                        });
                    }
                    else
                        tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                    $("#tablist tbody").append(tabstr);
                    $("#errMsg").html("");
                }
            }
        });

        $("#tablist").show();
        $("#instructions").hide();
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        $("#errMsg").empty();
        var msg = '';
        if ($("#o_type").val() == "01" && $("#SubSigner").val() == "")
            msg += "請選擇【送簽主管】<br>";
        if ($("#o_starthour").val() == "" || $("#o_startmins").val() == "" || $("#o_endhour").val() == "" || $("#o_endmins").val() == "")
            msg += "請選擇【日期與時間區間】<br>";
        else {
            var sTime = $("#o_startdate").val() + " " + $("#o_starthour").val() + ":" + $("#o_startmins").val();
            var eTime = $("#o_enddate").val() + " " + $("#o_endhour").val() + ":" + $("#o_endmins").val();
            if (new Date(sTime) > new Date(eTime))
                msg += "【日期與時間區間】選擇有誤，請重新確認<br>";
        }
        if ($("#o_type").val() == "02") {
            if ($("input[name='o_car']:checked").length == 0)
                msg += "請選擇【車輛】<br>";
            if ($("#o_place").val() == "")
                msg += "請輸入【地點】<br>";
        }
        if ($("#o_reason").val() == "")
            msg += "請輸入【事由】<br>";


        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        //data.append("mode", mode);


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddOutdoorForm.aspx",
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
                    var oType = "";
                    if ($("#o_type").val() == "01")
                        oType = "OFN";
                    else
                        oType = "OFC";
                    Flow.SendForm($("DataGuid", data).text(), oType, $("#SubSigner").val());

                    $(".newstr").val("");
                    $("#ddlPerson").multipleSelect('setSelects', []);
                    $("#tablist").hide();
                    $("#instructions").show();
                    $("#errMsg").html("");
                    alert("申請成功");

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
            group: "05"
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
                $("#o_type").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#o_type").append(selectstr);

            }
        }
    });
}

function GetPersonDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetPersonSelectList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#ddlPerson").empty();
                var selectstr = '';
                if ($(data).find("dataList").length > 0) {
                    $(data).find("dataList").each(function (i) {
                        selectstr += '<optgroup label="' + $(this).attr("depName").trim() + '">';
                        if ($(this).find("data_item").length > 0) {
                            $(this).find("data_item").each(function (i) {
                                selectstr += '<option value="' + $(this).attr("empNo").trim() + '">' + $(this).attr("empName").trim() + '</option>';
                            });
                        }
                        selectstr += '</optgroup>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#ddlPerson").append(selectstr);
            }
        }
    });
}


function GetSignersDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetOutdoorNormalSigners.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#SubSigner").empty();
                var selectstr = '<option value="">請選擇</option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("empno").text().trim() + '">' + $(this).children("empName").text().trim() + ' (' + $(this).children("empno").text().trim() + ')</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#SubSigner").append(selectstr);
            }
        }
    });
}

function CreateHourAndMinutes() {
    var HourStr = '<option value="">--</option>';
    for (var h = 0; h < 24; h++) {
        var hour = (h < 10 ? '0' : '') + h;
        HourStr += '<option value="' + hour + '">' + hour + '</option>';
    }
    $(".timeHour").append(HourStr);

    var MinStr = '<option value="">--</option>';
    for (var m = 0; m < 60; m++) {
        var mins = (m < 10 ? '0' : '') + m;
        MinStr += '<option value="' + mins + '">' + mins + '</option>';
    }
    $(".timeMin").append(MinStr);
}