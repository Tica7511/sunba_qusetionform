$(document).ready(function () {
    getData();

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9]/g;
        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });

    // 程序書分類名稱英文轉大寫
    $(document).on("keyup", "input[name='dcName'],#dc_name", function () {
        var toUpStr = $(this).val().toUpperCase();
        $(this).val(toUpStr);
    });

    // 新增
    $(document).on("click", "a[name='addbtn']", function () {
        var name = $(this).closest("tr").find("input[name='dcName']").val();
        var sort = $(this).closest("tr").find("input[name='dcSort_new']").val();
        if (name == "")
            alert("以下欄位必填：\n程序書分類名稱");
        else {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/AddDocCategory.aspx",
                data: {
                    dc_name: name,
                    dc_sort: sort
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
                        if ($(data).find("Repeated").length > 0) {
                            alert($(data).find("Repeated").text().trim());
                        }
                        getData();
                    }
                }
            });
        }
    });

    // Detail
    $(document).on("click", "a[name='editbtn']", function () {
        $("#tmpid").val($(this).attr("aid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetDocCategoryDetail.aspx",
            data: {
                id: $("#tmpid").val()
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
                            $("#dc_name").val($(this).children("dc_name").text().trim());
                        });
                    }
                    $('#DocCategoryModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        if ($("#dc_name").val() == "")
            alert("以下欄位必填：\n程序書分類名稱");
        else {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/AddDocCategory.aspx",
                data: {
                    id: $("#tmpid").val(),
                    dc_name: $("#dc_name").val()
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
                        if ($(data).find("Repeated").length > 0) {
                            alert($(data).find("Repeated").text().trim());
                        }
                        else {
                            $('#DocCategoryModal').modal('hide');
                            getData();
                        }
                    }
                }
            });
        }
    });

    // 排序值按鈕修改
    $(document).on("click", "#savebtn_sort", function () {
        var tmpguidsort = "";
        var temp = new Array();
        var alertMsg = "提示：";
        for (var index in tmpguid) {
            tmpguidsort += tmpguid[index].guid + ":" + $("#" + tmpguid[index].guid).val() + ",";
            temp[index] = $("#" + tmpguid[index].guid).val();
        }
        alertMsg += new Set(temp).size !== temp.length ? "\n排序值重複" : "";
        alertMsg += new Set(temp).has("") ? "\n請輸入所有排序值" : "";
        if (alertMsg != "提示：") {
            alert(alertMsg);
        }
        else {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/AddDocCategory.aspx",
                data: {
                    updateSort: tmpguidsort,
                    id: "sort",
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
                        getData();
                    }
                }
            });
        }
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteDocCategory.aspx",
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
                        getData();
                    }
                }
            });
        }
    });

}); // end js

var tmpguid = new Array(); // 記錄 guid 給儲存排序使用
function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDocCategoryList.aspx",
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
                        tmpguid[i] = {
                            guid: $(this).children("dc_guid").text().trim(),
                            sort: $(this).children("dc_sort").text().trim()
                        }; // 記錄 guid 給儲存排序使用
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dc_name").text().trim() + '</td>';
                        //tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dc_sort").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<input type="text" id="'
                            + $(this).children("dc_guid").text().trim()
                            + '" name="dcSort" class="inputex width100 num" style="text-align:center" aid="'
                            + $(this).children("dc_id").text().trim()
                            + '" value="' + $(this).children("dc_sort").text().trim() + '" />';
                        tabstr += '</td > ';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("dc_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("dc_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                tabstr += AddNewRow();
                $("#tablist tbody").append(tabstr);
            }
        }
    });
}

function AddNewRow() {
    var str = '<tr>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<input type="text" name="dcName" class="inputex width100" />';
    str += '</td>';
    str += '<td class="text-center">';
    str += '<input type="text" name="dcSort_new" class="inputex width100 num" style="text-align:center" />';
    str += '</td>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<a name="addbtn" href="javascript:void(0);">新增</a>';
    str += '</td></tr>';
    return str;
}