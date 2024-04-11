$(document).ready(function () {
    // dll
    GetType("#mr_place");
    getData();

    // 新增
    $(document).on("click", "a[name='addbtn']", function () {
        var name = $(this).closest("tr").find("input[name='rName']").val();
        var place = $(this).closest("tr").find("select[name='newPlace']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMeetingRoom.aspx",
            data: {
                mr_name: name,
                mr_place: place
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
            url: "../handler/GetMeetingRoomDetail.aspx",
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
                            $("#mr_name").val($(this).children("mr_name").text().trim());
                            $("#mr_place").val($(this).children("mr_place").text().trim());
                        });
                    }
                    $('#MeetingRoomModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMeetingRoom.aspx",
            data: {
                id: $("#tmpid").val(),
                mr_name: $("#mr_name").val(),
                mr_place: $("#mr_place").val()
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
                    $('#MeetingRoomModal').modal('hide');
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
                url: "../handler/DeleteMeetingRoom.aspx",
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
        url: "../handler/GetMeetingRoomList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mr_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("Area").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("mr_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("mr_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }

                tabstr += AddNewRow();
                $("#tablist tbody").append(tabstr);
                GetType("select[name='newPlace']");
            }
        }
    });
}

function AddNewRow() {
    var str = '<tr>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<input type="text" name="rName" class="inputex width100" />';
    str += '</td>';
    str += '<td class="text-center">';
    str += '<select name="newPlace" class="form-select" aria-label="Default select example"></select>';
    str += '</td>';
    str += '<td class="text-center" style="vertical-align: middle;">';
    str += '<a name="addbtn" href="javascript:void(0);">新增</a>';
    str += '</td></tr>';
    return str;
}

function GetType(Selector) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            group: "04"
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