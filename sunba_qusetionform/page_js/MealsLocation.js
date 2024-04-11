$(document).ready(function () {
    getData();

    // 新增
    $(document).on("click", "a[name='addbtn']", function () {
        var mlName = $(this).closest("tr").find("input[name='mlName']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsLocation.aspx",
            data: {
                ml_name: mlName
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
    });

    // Detail
    $(document).on("click", "a[name='editbtn']", function () {
        $("#tmpid").val($(this).attr("aid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetMealsLocationDetail.aspx",
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
                            $("#ml_name").val($(this).children("ml_name").text().trim());
                        });
                    }
                    $('#UpdateModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsLocation.aspx",
            data: {
                id: $("#tmpid").val(),
                ml_name: $("#ml_name").val()
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
                    $('#UpdateModal').modal('hide');
                    getData();
                }
            }
        });
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMealsLocation.aspx",
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

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsLocationList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("ml_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        if ($(this).children("ml_guid").text().trim() != "kitchen") {
                            tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("ml_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                            tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("ml_id").text().trim() + '">刪除</a>';
                        }
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
    str += '<input type="text" name="mlName" class="inputex width100" />';
    str += '</td>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<a name="addbtn" href="javascript:void(0);">新增</a>';
    str += '</td></tr>';
    return str;
}