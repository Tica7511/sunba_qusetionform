$(document).ready(function () {
    // dll
    GetType("#SearchArea", "04");
    GetType("#dr_area", "04");
    GetType("#dr_category", "02");
    GetType("#dr_roomtype", "03");
    getData(0);

    // 新增
    $(document).on("click", "#newbtn", function () {
        $(".newstr").val("");
        $("#dr_area option").eq(0).prop("selected", true);
        $("#dr_roomtype option").eq(0).prop("selected", true);
        $("#dr_category option").eq(0).prop("selected", true);
        $('#RoomSettingModal').modal('show');
    });

    // 編輯
    $(document).on("click", "a[name='editbtn']", function () {
        $("#tmpid").val($(this).attr("aid"));

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetDormitoryRoomDetail.aspx",
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
                            $("#dr_area").val($(this).children("dr_area").text().trim());
                            $("#dr_no").val($(this).children("dr_no").text().trim());
                            $("#dr_ext").val($(this).children("dr_ext").text().trim());
                            $("#dr_roomtype").val($(this).children("dr_roomtype").text().trim());
                            $("#dr_category").val($(this).children("dr_category").text().trim());
                            $("#dr_ps").val($(this).children("dr_ps").text().trim());
                        });
                    }
                }
            }
        });
        $('#RoomSettingModal').modal('show');
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("id", $("#tmpid").val());


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddDormitoryRoom.aspx",
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
                    alert($(data).find("Error").attr("Message"));
                }
                else {
                    getData(0);
                }
            }
        });
        $('#RoomSettingModal').modal('hide');
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        if (confirm('確定刪除?')) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteDormitoryRoom.aspx",
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
                        getData(0);
                    }
                }
            });
        }
    });

    // 廠區
    $(document).on("change", "#SearchArea", function () {
        getData(0);
    });
}); // end js

function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDormitoryRoomManageList.aspx",
        data: {
            SearchStr: $("#SearchStr").val(),
            PageNo: p,
            PageSize: Page.Option.PageSize,
            Area: $("#SearchArea").val()
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
                $("#tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dr_no").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dr_ext").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("RoomTypeCn").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("TypeCn").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dr_ps").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("dr_id").text().trim() + '">編輯</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("dr_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}


function GetType(Selector,Group) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            group: Group
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
                $(Selector).empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("C_Item").text().trim() + '">' + $(this).children("C_Item_cn").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $(Selector).append(selectstr);
            }
        }
    });
}