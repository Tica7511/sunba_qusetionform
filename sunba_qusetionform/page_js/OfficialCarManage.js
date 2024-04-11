$(document).ready(function () {
    getData();

    $(document).on("keyup", "input[name='ocNum'],#oc_number", function () {
        var toUpStr = $(this).val().toUpperCase();
        $(this).val(toUpStr);
    });

    // 新增
    $(document).on("click", "a[name='addbtn']", function () {
        var number = $(this).closest("tr").find("input[name='ocNum']").val();
        var ps = $(this).closest("tr").find("input[name='ocPs']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddOfficialCar.aspx",
            data: {
                oc_number: number,
                oc_ps: ps
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
            url: "../handler/GetOfficialCarDetail.aspx",
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
                            $("#oc_number").val($(this).children("oc_number").text().trim());
                            $("#oc_ps").val($(this).children("oc_ps").text().trim());
                        });
                    }
                    $('#OfficialCarModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddOfficialCar.aspx",
            data: {
                id: $("#tmpid").val(),
                oc_number: $("#oc_number").val(),
                oc_ps: $("#oc_ps").val()
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
                    $('#OfficialCarModal').modal('hide');
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
                url: "../handler/DeleteOfficialCar.aspx",
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
        url: "../handler/GetOfficialCarList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("oc_number").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("oc_ps").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("oc_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("oc_id").text().trim() + '">刪除</a>';
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
    str += '<input type="text" name="ocNum" class="inputex width100" />';
    str += '</td>';
    str += '<td class="text-center">';
    str += '<input type="text" name="ocPs" class="inputex width100" />';
    str += '</td>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<a name="addbtn" href="javascript:void(0);">新增</a>';
    str += '</td></tr>';
    return str;
}