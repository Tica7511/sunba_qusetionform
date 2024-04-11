$(document).ready(function () {
    getData(0);

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        if (/[^0-9]/g.test(this.value)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });

    $(document).on("click", "#newbtn", function () {
        $("#savebtn").show();
        $(".newstr").prop("disabled", false);
        $(".newstr").val("");
        $("#EditModal").modal("show");
    });

    // 送出
    $(document).on("click", "#savebtn", function () {
        $("#errMsg").empty();
        var msg = '';
        if ($("#mv_name").val() == "")
            msg += "請輸入【廠商/訪客名稱】<br>";
        if ($("#mv_date").val() == "")
            msg += "請輸入【用餐時間】<br>";

        if (msg != "") {
            $("#errMsg").html("Message: <br>" + msg);
            return false;
        }

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        //data.append("id", $("#tmpid").val());

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsVisitor.aspx",
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
                    Flow.SendForm($("DataGuid", data).text(), "MV");
                    getData(0);
                    $("#EditModal").modal("hide");
                    $("#errMsg").html("");
                }
            }
        });
    });

    // Detail
    $(document).on("click", "a[name='editbtn']", function () {
        // 需簽核不可修改
        $("#savebtn").hide();
        $(".newstr").prop("disabled", true);

        $("#tmpid").val($(this).attr("aid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetMealsVisitorDetail.aspx",
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
                            $("#mv_name").val($(this).children("mv_name").text().trim());
                            $("#mv_reason").val($(this).children("mv_reason").text().trim());
                            $("#mv_date").val($(this).children("mv_date").text().trim());
                            $("#mv_lunch_meat").val($(this).children("mv_lunch_meat").text().trim());
                            $("#mv_lunch_vegetarian").val($(this).children("mv_lunch_vegetarian").text().trim());
                            $("#mv_lunch_vegan").val($(this).children("mv_lunch_vegan").text().trim());
                            $("#mv_dinner_meat").val($(this).children("mv_dinner_meat").text().trim());
                            $("#mv_dinner_vegetarian").val($(this).children("mv_dinner_vegetarian").text().trim());
                            $("#mv_dinner_vegan").val($(this).children("mv_dinner_vegan").text().trim());
                        });
                    }
                    $('#EditModal').modal('show');
                }
            }
        });
    });


    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        var gid = $(this).attr("agid");
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMealsVisitor.aspx",
                data: {
                    gid: gid
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
                        Flow.TerminateTask(gid);
                        getData();
                    }
                }
            });
        }
    });
}); // end js

function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsVisitorList.aspx",
        data: {
            SearchStr: $("#SearchStr").val(),
            PageNo: p,
            PageSize: Page.Option.PageSize
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("mv_createname").text().trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).children("mv_name").text().trim() + '</td>';
                        tabstr += '<td>' + $(this).children("mv_reason").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("mv_date").text().trim())) + '</td>';
                        tabstr += '<td class="text-center">葷(' + $(this).children("mv_lunch_meat").text().trim() + ')、奶蛋素(' + $(this).children("mv_lunch_vegetarian").text().trim() + ')、全素(' + $(this).children("mv_lunch_vegan").text().trim() + ')</td>';
                        tabstr += '<td class="text-center">葷(' + $(this).children("mv_dinner_meat").text().trim() + ')、奶蛋素(' + $(this).children("mv_dinner_vegetarian").text().trim() + ')、全素(' + $(this).children("mv_dinner_vegan").text().trim() + ')</td>';
                        var result = '';
                        switch ($(this).children("fm_result").text().trim()) {
                            case "Y":
                                result = '<span class="text-success">同意</span>';
                                break;
                            case "N":
                                result = '<span class="text-danger">否決</span>';
                                break;
                            default:
                                result = '待審核';
                                break;
                        }
                        tabstr += '<td class="text-center">' + result + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="editbtn" aid="' + $(this).children("mv_id").text().trim() + '">檢視</a>&nbsp;&nbsp;';
                        if ($(this).children("fm_result").text().trim() == "")
                            tabstr += '<a href="javascript:void(0);" name="delbtn" agid="' + $(this).children("egid").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);
                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}