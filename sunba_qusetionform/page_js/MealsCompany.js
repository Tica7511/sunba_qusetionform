$(document).ready(function () {
    GetType();
    getData();

    // 新增
    $(document).on("click", "#newbtn", function () {
        $(".newstr").val("");
        $("#ddltype").val("02");
        $("#EditModal").modal("show");
    });

    // Detail
    $(document).on("click", "a[name='editbtn']", function () {
        $("#tmpid").val($(this).attr("aid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetMealsCompanyDetail.aspx",
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
                            $("#mc_name").val($(this).children("mc_name").text().trim());
                            $("#mc_category").val($(this).children("ddltype").text().trim());
                        });
                    }
                    $('#EditModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCompany.aspx",
            data: {
                id: $("#tmpid").val(),
                mc_category: $("#ddltype").val(),
                mc_name: $("#mc_name").val()
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
                    $('#EditModal').modal('hide');
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
                url: "../handler/DeleteMealsCompany.aspx",
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

    // 用餐登記
    $(document).on("click", "a[name='viewbtn']", function () {
        location.href = "MealsCompanyRegister.aspx?v=" + $(this).attr("aid") + "&tp=" + $(this).attr("atype");
    });
}); // end js

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCompanyList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mc_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" aid="' + $(this).children("mc_guid").text().trim() + '" atype="' + $(this).children("mc_category").text().trim() + '">用餐登記</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("mc_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("mc_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
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
            group: "06"
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
                $("#ddltype").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        if ($(this).children("C_Item").text().trim() != "01")
                            selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("#ddltype").append(selectstr);

            }
        }
    });
}