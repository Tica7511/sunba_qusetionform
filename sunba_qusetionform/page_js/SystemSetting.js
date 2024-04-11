$(document).ready(function () {
    GetPersonDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        multiple: true,
        multipleWidth: 120,
        minimumCountSelected: 20
        // ----  處理人事資料人員重複問題 Start ----
        //onClick: function (view) {
        //    var selector = $("#" + this.name);
        //    var ary = selector.multipleSelect('getSelects');
        //    if (!view.selected) {
        //        var setVal = $.grep(ary, function (value) {
        //            return value != view.value;
        //        });

        //        selector.multipleSelect('setSelects', setVal);
        //    }
        //},
        //onOptgroupClick: function (view) {
        //    var selector = $("#" + this.name);
        //    var ary = selector.multipleSelect('getSelects');
        //    $.each(view.children, function () {
        //        if (!this.selected) {
        //            var thisAry = this;
        //            var setVal = $.grep(ary, function (value) {
        //                return value != thisAry.value;
        //            });

        //            ary = setVal;
        //            selector.multipleSelect('setSelects', setVal);
        //        }
        //    });
        //}
    });

    // Set multiple-select Name
    //$('.multiple-select').each(function () {
    //    var nameStr = this.name;
    //    $("#" + nameStr).multipleSelect('refreshOptions', {
    //        name: nameStr
    //    });
    //});

    // ----  處理人事資料人員重複問題 End ----

    getData();

    // 儲存
    $(document).on("click", "#savebtn", function () {
        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        //data.append("mode", mode);

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddSysSetting.aspx",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
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
                    getData();
                }
            }
        });
    });
}); // end js


function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetSysSetting.aspx",
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
                        var empno = $(this).children("c_empno").text().trim().split(',');
                        switch ($(this).children("c_type").text().trim()) {
                            case "01":
                                $("#MealsVisitor").multipleSelect('setSelects', empno);
                                break;
                            case "02":
                                $("#MealsCancel").multipleSelect('setSelects', empno);
                                break;
                            case "03":
                                $("#OutdoorForm").multipleSelect('setSelects', empno);
                                break;
                            case "04":
                                $("#Dormitory").multipleSelect('setSelects', empno);
                                break;
                            case "05":
                                $("#DormitoryManager").multipleSelect('setSelects', empno);
                                break;
                            case "06":
                                $("#Meeting").multipleSelect('setSelects', empno);
                                break;
                            case "07":
                                $("#Doc").multipleSelect('setSelects', empno);
                                break;
                            case "sa":
                                $("#SystemAdmin").multipleSelect('setSelects', empno);
                                break;
                        }
                    });
                }
            }
        }
    });
}



function GetPersonDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetPersonSelectList.aspx",
        data: {
            mode: "setting"
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
                $(".multiple-select").empty();
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
                $(".multiple-select").append(selectstr);
            }
        }
    });
}