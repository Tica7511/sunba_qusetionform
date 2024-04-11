$(document).ready(function () {
    GetDocCategoryDDL();
    getData(0);

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9]/g;
        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });

    // 新增
    $(document).on("click", "#addbtn", function () {
        $(".newstr").val("");
        $("#FileList").empty();
        $("#DocumentModal").modal("show");
    });

    // Detail
    $(document).on("click", "a[name='editbtn']", function () {
        $(".newstr").val("");
        $("#tmpgid").val($(this).attr("agid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetDocumentDetail.aspx",
            data: {
                gid: $("#tmpgid").val()
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
                    // Main
                    if ($(data).find("data_item").length > 0) {
                        $(data).find("data_item").each(function (i) {
                            $("#d_pubdate").val($(this).children("d_pubdate").text().trim());
                            $("#d_category").val($(this).children("d_category").text().trim());
                            $("#d_no").val($(this).children("d_no").text().trim());
                            $("#d_name").val($(this).children("d_name").text().trim());
                            $("#d_version").val($(this).children("d_version").text().trim());
                        });
                    }

                    // Attach
                    var filestr = '';
                    $("#FileList").empty();
                    if ($(data).find("file_item").length > 0) {
                        $(data).find("file_item").each(function (i) {
                            filestr += '<div class="mt-1">';
                            filestr += '<a href="../DOWNLOAD.aspx?v=' + $(this).children("File_ID").text().trim() + '"  target="_blank">' + $(this).children("File_Orgname").text().trim() + $(this).children("File_Exten").text().trim() + '</a>&nbsp;';
                            filestr += '<a href="javascript:void(0);" aid="' + $(this).children("File_ID").text().trim() + '" name="file_delbtn" class="btn btn-outline-danger btn-sm">刪除</a>';
                            filestr += '</div>';
                        });
                    }
                    $("#FileList").append(filestr);

                    $('#DocumentModal').modal('show');
                }
            }
        });
    });

    // 檔案刪除
    $(document).on("click", "a[name='file_delbtn']", function () {
        var tmpstr = $("#tmpfile_id").val();
        if (tmpstr != "") tmpstr += ",";
        tmpstr += $(this).attr("aid");
        $("#tmpfile_id").val(tmpstr);
        $(this).parent().remove();
    });

    // 儲存修改
    $(document).on("click", "#savebtn", function () {
        var alertMsg = "以下欄位必填：";
        alertMsg += ($("#d_pubdate").val() == "") ? "\n發行日期" : "";
        alertMsg += ($("#d_no").val() == "") ? "\n文件/表單編號" : "";
        alertMsg += ($("#d_name").val() == "") ? "\n文件/表單名稱" : "";
        if (alertMsg != "以下欄位必填：") alert(alertMsg);
        else {
            // Get form
            var form = $('#documentData')[0];

            // Create an FormData object 
            var data = new FormData(form);

            // If you want to add an extra field for the FormData
            data.append("gid", $("#tmpgid").val());
            data.append("file_delete_id", $("#tmpfile_id").val());
            data.append("d_pubdate", $("#d_pubdate").val());
            data.append("d_category", $("#d_category").val());
            data.append("d_no", $("#d_no").val());
            data.append("d_name", $("#d_name").val());
            data.append("d_version", $("#d_version").val());

            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/AddDocument.aspx",
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
                        $('#DocumentModal').modal('hide');
                        getData(0);
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
                url: "../handler/DeleteDocument.aspx",
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

    // 查詢
    $(document).on("click", "#SearchBtn", function () {
        getData(0);
    });

    // 排序 文件分類/階層 asc
    $(document).on("click", "#d_categoryAsc", function () {
        $("#tmporderby").val("dc_sort");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 文件分類/階層 desc
    $(document).on("click", "#d_categoryDesc", function () {
        $("#tmporderby").val("dc_sort");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 文件/表單編號 asc
    $(document).on("click", "#d_noAsc", function () {
        $("#tmporderby").val("d_no");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 文件/表單編號 desc
    $(document).on("click", "#d_noDesc", function () {
        $("#tmporderby").val("d_no");
        $("#tmpsortby").val("desc");
        getData(0);
    });

}); // end js

function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDocumentList.aspx",
        data: {
            SearchStr: $("#SearchStr").val(),
            PageNo: p,
            PageSize: Page.Option.PageSize,
            DocCategory: $("#DocCategory").val(),
            SortName: $("#tmporderby").val(),
            SortMethod: $("#tmpsortby").val()
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
                $("#tmporderby").empty();
                $("#tmpsortby").empty();
                $("#tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("d_pubdate").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("dc_name").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("d_no").trim() + '</td>';
                        tabstr += '<td class="text-center">' + $(this).attr("d_name").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("d_version").trim() + '</td>';
                        tabstr += '<td><div class="mt-1">';
                        if ($(this).find("file").length > 0) {
                            var filestr = '';
                            $(this).find("file").each(function (i) {
                                if (filestr != "") filestr += "<br />";
                                filestr += '<a href="../DOWNLOAD.aspx?v=' + $(this).attr("id").trim() + '"  target="_blank">' + $(this).attr("orgname").trim() + $(this).attr("ext").trim() + '</a>';
                            });
                            tabstr += filestr;
                        }
                        tabstr += '</div></td>';
                        if ($("IsManager", data).text() == "Y") {
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" name="editbtn" agid="' + $(this).attr("d_guid").trim() + '">修改</a>&nbsp;&nbsp;';
                            tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).attr("d_id").trim() + '">刪除</a>';
                            tabstr += '</td>';
                        }
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';

                if ($("IsManager", data).text() == "Y")
                    $(".docShow").show();

                $("#tablist tbody").append(tabstr);

                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

function GetDocCategoryDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetDocCategorySelectList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("dc_guid").text().trim() + '">' + $(this).children("dc_name").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';

                $("#DocCategory").empty();
                $("#d_category").empty();
                var SearchStr = '<option value="">全部</option>' + selectstr;
                $("#DocCategory").append(SearchStr);
                selectstr = '<option value="">-- 請選擇 --</option>' + selectstr;
                $("#d_category").append(selectstr);
            }
        }
    });
}