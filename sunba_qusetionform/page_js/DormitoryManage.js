$(document).ready(function () {
    GetPersonDDL();

    $('.multiple-select').multipleSelect({
        selectAll: false,
        position: 'top',
        maxHeight: 220,
    });

    // dll
    GetType("#SearchArea", "04");

    getData(0);

    // 查詢
    $(document).on("click", "#SearchBtn", function () {
        getData(0);
    });

    // 匯出
    $(document).on("click", "#ExportBtn", function () {
        window.open("../handler/ExportDormitoryTenant.aspx?SearchStr=" + $("#SearchStr").val() + "&Area=" + $("#SearchArea").val());
    });

    // 編輯
    $(document).on("click", "a[name='editbtn']", function () {
        $("#tmpgid").val($(this).attr("agid"));

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetDormitoryTenantDetail.aspx",
            data: {
                roomid: $("#tmpgid").val()
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
                    // 房間
                    if ($(data).find("room_item").length > 0) {
                        $(data).find("room_item").each(function (i) {
                            $("#Area").html($(this).children("dr_area").text().trim());
                            $("#RoomNo").html($(this).children("dr_no").text().trim());
                            $("#Ext").html($(this).children("dr_ext").text().trim());
                            $("#RoomType").html($(this).children("RoomTypeCn").text().trim());
                            $("#Category").html($(this).children("TypeCn").text().trim());
                            $("#Ps").html($(this).children("dr_ps").text().trim());
                        });
                    }

                    // 入住人員
                    var person = [];
                    if ($(data).find("tenant_item").length > 0) {
                        $(data).find("tenant_item").each(function (i) {
                            person.push($(this).children("dt_empno").text().trim());
                        });
                    }
                    $("#ddlPerson").multipleSelect('setSelects', person);
                }
            }
        });
        $('#CheckInSettingModal').modal('show');
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        var Empno = "";
        var EmpName = "";
        var selectArray = $("#ddlPerson option:selected");
        selectArray.each(function () {
            if (Empno != "") Empno += ",";
            Empno += this.value;

            if (EmpName != "") EmpName += ",";
            EmpName += this.text;
        });

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddDormitoryTenant.aspx",
            data: {
                gid: $("#tmpgid").val(),
                empno: Empno,
                person: EmpName
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
                    $('#CheckInSettingModal').modal('hide');
                }
            }
        });
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
        url: "../handler/GetDormitoryManageList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("dt_name").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("dr_id").text().trim() + '" agid="' + $(this).children("dr_guid").text().trim() + '">編輯</a>';
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


function GetType(Selector, Group) {
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