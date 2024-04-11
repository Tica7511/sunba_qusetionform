$(document).ready(function () {
    getData();

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9]/g;
        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });


    // 新增
    $(document).on("click", "a[name='addbtn']", function () {
        var msg = '';
        if ($("input[name='mfEffectiveDate']").val() == "")
            msg += "請輸入【生效日】\n";

        if (msg != "") {
            alert("Message: \n" + msg);
            return false;
        }


        var employee = $(this).closest("tr").find("input[name='mfEmployee']").val();
        var firm = $(this).closest("tr").find("input[name='mfFirm']").val();
        var visitor = $(this).closest("tr").find("input[name='mfVisitor']").val();
        var love = $(this).closest("tr").find("input[name='mfLove']").val();
        var effdate = $(this).closest("tr").find("input[name='mfEffectiveDate']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsFee.aspx",
            data: {
                mf_employee: employee,
                mf_firm: firm,
                mf_visitor: visitor,
                mf_love: love,
                mf_effectivedate: effdate
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
            url: "../handler/GetMealsFeeDetail.aspx",
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
                            $("#mf_employee").val($(this).children("mf_employee").text().trim());
                            $("#mf_firm").val($(this).children("mf_firm").text().trim());
                            $("#mf_visitor").val($(this).children("mf_visitor").text().trim());
                            $("#mf_love").val($(this).children("mf_love").text().trim());
                            $("#mf_effectivedate").val($(this).children("mf_effectivedate").text().trim());
                        });
                    }
                    $('#UpdateModal').modal('show');
                }
            }
        });
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        var msg = '';
        if ($("#mf_effectivedate").val() == "")
            msg += "請輸入【生效日】\n";

        if (msg != "") {
            alert("Message: \n" + msg);
            return false;
        }

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsFee.aspx",
            data: {
                id: $("#tmpid").val(),
                mf_employee: $("#mf_employee").val(),
                mf_firm: $("#mf_firm").val(),
                mf_visitor: $("#mf_visitor").val(),
                mf_love: $("#mf_love").val(),
                mf_effectivedate: $("#mf_effectivedate").val()
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
                url: "../handler/DeleteMealsFee.aspx",
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
        url: "../handler/GetMealsFeeList.aspx",
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mf_employee").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mf_firm").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mf_visitor").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mf_love").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("mf_effectivedate").text().trim())) + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("mf_id").text().trim() + '">修改</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("mf_id").text().trim() + '">刪除</a>';
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
    str += '<td class="text-center"><input type="text" class="inputex num" name="mfEmployee" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex num" name="mfFirm" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex num" name="mfVisitor" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex num" name="mfLove" /></td>';
    str += '<td class="text-center"><input type="date" class="inputex" name="mfEffectiveDate" /></td>';
    str += '<td class="text-center"><a href="javascript:void(0);" name="addbtn">新增</a></td>';
    str += '</tr>';
    return str;
}