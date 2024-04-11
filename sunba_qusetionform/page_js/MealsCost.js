// ※成本管理※
$(document).ready(function () {
    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9]/g;
        if (this.name == "mcc_tel" || this.name == "mccTel")
            reg = /[^0-9-]/g;

        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });

    GetStatisticsTable();
    getIncomeData(0);
    getCostData(0);

    // Open Modal
    $(document).on("click", "#newIncomeBtn,#newCostBtn", function () {
        $(".newstr").val("");
        if (this.id == "newIncomeBtn")
            $("#incomeModal").modal("show");
        else {
            $("#costModal").modal("show");
            getCostItemData("");
        }
    });

    // 刪除
    $(document).on("click", "a[name='delbtn']", function () {
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            var category = $(this).closest("table").attr("id");
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMealsCost.aspx",
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
                        if (category == "incomelist")
                            getIncomeData(0);
                        else
                            getCostData(0);
                    }
                }
            });
        }
    });

    // 儲存收入項目明細
    $(document).on("click", "#IncomeSaveBtn", function () {
        // Get form
        var form = $('#incomeData')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("category", "income");

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCost.aspx",
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
                    $("#incomeModal").modal("hide");
                    getIncomeData(0);
                    GetStatisticsTable();
                }
            }
        });
    });

    // 儲存成本項目明細
    $(document).on("click", "#CostSaveBtn", function () {
        // Get form
        var form = $('#costData')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("category", "cost");
        data.append("gid", $("#cost_tmpgid").val());
        data.append("file_delete_id", $("#cost_tmpfile_id").val());

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCost.aspx",
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
                    $("#costModal").modal("hide");
                    getCostData(0);
                    GetStatisticsTable();
                }
            }
        });
    });

    // 成本項目 Detail
    $(document).on("click", "a[name='viewbtn']", function () {
        $(".newstr").val("");
        $("#cost_tmpgid").val($(this).attr("agid"));
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetMealsCostDetail.aspx",
            data: {
                gid: $("#cost_tmpgid").val()
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
                            $("#cost_date").val($(this).children("mc_date").text().trim());
                            $("#cost_ps").val($(this).children("mc_ps").text().trim());

                            getCostItemData($(this).children("mc_guid").text().trim());
                        });
                    }

                    // Attach
                    var filestr = '';
                    $("#CostFileList").empty();
                    if ($(data).find("file_item").length > 0) {
                        $(data).find("file_item").each(function (i) {
                            filestr += '<div class="mt-1">';
                            filestr += '<a href="../DOWNLOAD.aspx?v=' + $(this).children("File_ID").text().trim() + '"  target="_blank">' + $(this).children("File_Orgname").text().trim() + $(this).children("File_Exten").text().trim() + '</a>&nbsp;';
                            filestr += '<a href="javascript:void(0);" aid="' + $(this).children("File_ID").text().trim() + '" name="file_delbtn" class="btn btn-outline-danger btn-sm">刪除</a>';
                            filestr += '</div>';
                        });
                    }
                    $("#CostFileList").append(filestr);

                    $('#costModal').modal('show');
                }
            }
        });
    });

    // 成本檢視檔案刪除
    $(document).on("click", "a[name='file_delbtn']", function () {
        var tmpstr = $("#cost_tmpfile_id").val();
        if (tmpstr != "") tmpstr += ",";
        tmpstr += $(this).attr("aid");
        $("#cost_tmpfile_id").val(tmpstr);
        $(this).parent().remove();
    });

    // 成本品項新增
    $(document).on("click", "a[name='costitem_addbtn']", function () {
        var thieRow = $(this).closest('tr');
        $("#costitemlist tbody").append(thieRow[0].outerHTML);

        thieRow.find('select[name="ddlCostitem"]').attr("name", "mci_item");
        thieRow.find('input[name="mciNum"]').attr("name", "mci_num");
        thieRow.find('input[name="mciUnitPrice"]').attr("name", "mci_unitprice");
        thieRow.find('input[name="mciPrice"]').attr("name", "mci_price");
        thieRow.find('select[name="ddlCostfirm"]').attr("name", "mci_company");
        thieRow.find('a').attr("aid", "");
        thieRow.find('a').attr("name", "mci_delbtn");
        thieRow.find('a').html("刪除");
    });

    // 成本品項刪除
    $(document).on("click", "a[name='mci_delbtn']", function () {
        $(this).parent().parent().remove();
    });

    // 成本品項單位 change
    $(document).on("change", "select[name='ddlCostitem'],select[name='mci_item']", function () {
        var unit = $(this).find('option:selected').attr("ounit");
        $(this).closest("tr").find("td:eq(2)").html(unit);
    });

    // 計算成本品項金額
    $(document).on("change", ".calculate", function () {
        var thisRow = $(this).closest("tr");
        var inputNum = thisRow.find("td:eq(1) input").val();
        var inputUnitPrice = thisRow.find("td:eq(3) input").val();
        if (inputNum != "" && inputUnitPrice != "") {
            var total = parseInt(inputNum) * parseInt(inputUnitPrice);
            thisRow.find("td:eq(4) input").val(total);
        }
    });

    // 匯出 Open Modal
    $(document).on("click", "#IncomeExportBtn,#CostExportBtn", function () {
        if (this.id == "IncomeExportBtn") 
            $("#ExportMode").val("income");
        else
            $("#ExportMode").val("cost");

        $("#ExportModal").modal("show");
    });

    // 匯出
    $(document).on("click", "#exportbtn", function () {
        window.open("../handler/ExportMealsCost.aspx?category=" + $("#ExportMode").val() + "&startdate=" + $("#Export_StartDate").val() + "&enddate=" + $("#Export_EndDate").val());
    });
}); // end js

// 收入項目統計清單
function getIncomeData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostList.aspx",
        data: {
            Category: "income",
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
                $("#incomelist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_date").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_price").trim() + '</td>';
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_ps").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).attr("mc_id").trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                $("#incomelist tbody").append(tabstr);
                Page.Option.FunctionName = "getIncomeData";
                Page.Option.Selector = "#income_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// 成本項目統計清單
function getCostData(p) {
    Page.Option.PageSize = 5;
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostList.aspx",
        data: {
            Category: "cost",
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
                $("#costlist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_date").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_price").trim() + '</td>';
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).attr("mc_ps").trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<a href="javascript:void(0);" name="viewbtn" agid="' + $(this).attr("mc_guid").trim() + '">檢視</a>&nbsp;&nbsp;';
                        tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).attr("mc_id").trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                $("#costlist tbody").append(tabstr);
                Page.Option.FunctionName = "getCostData";
                Page.Option.Selector = "#cost_pageblock";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// 成本品項清單
function getCostItemData(parent_id) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostItemList.aspx",
        data: {
            parentid: parent_id
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
                $("#costitemlist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        // 品名
                        var ItemUnit = "";
                        var costitem = $(this).children("mci_item").text().trim();
                        tabstr += '<td class="text-center"><select class="inputex" name="mci_item">';
                        if (costitem == "") tabstr += '<option value="">請選擇</option>';
                        if ($(data).find("mcf_item").length > 0) {
                            $(data).find("mcf_item").each(function (i) {
                                if ($(this).children("mcf_guid").text().trim() == costitem) {
                                    tabstr += '<option value="' + $(this).children("mcf_guid").text().trim() + '" ounit="' + $(this).children("mcf_unit").text().trim() + '" selected>' + $(this).children("mcf_name").text().trim() + '</option>';
                                    ItemUnit = $(this).children("mcf_unit").text().trim();
                                }
                                else
                                    tabstr += '<option value="' + $(this).children("mcf_guid").text().trim() + '">' + $(this).children("mcf_name").text().trim() + '</option>';
                            });
                        }
                        tabstr += '</select></td>';
                        tabstr += '<td class="text-center"><input type="number" class="inputex width100 num calculate" name="mci_num" value="' + $(this).children("mci_num").text().trim() + '" /></td>';
                        tabstr += '<td class="text-center">' + ItemUnit + '</td>';
                        tabstr += '<td class="text-center"><input type="number" class="inputex width100 num calculate" name="mci_unitprice" value="' + $(this).children("mci_unitprice").text().trim() + '" /></td>';
                        tabstr += '<td class="text-center"><input type="number" class="inputex width100 num" name="mci_price" value="' + $(this).children("mci_price").text().trim() + '" /></td>';
                        // 廠商
                        var costfirm = $(this).children("mci_company").text().trim();
                        tabstr += '<td class="text-center"><select class="inputex" name="mci_company">';
                        if ($(data).find("mcc_item").length > 0) {
                            $(data).find("mcc_item").each(function (i) {
                                if ($(this).children("mcc_guid").text().trim() == costfirm) {
                                    tabstr += '<option value="' + $(this).children("mcc_guid").text().trim() + '" selected>' + $(this).children("mcc_name").text().trim() + '</option>';
                                }
                                else
                                    tabstr += '<option value="' + $(this).children("mcc_guid").text().trim() + '">' + $(this).children("mcc_name").text().trim() + '</option>';
                            });
                        }
                        tabstr += '</select></td>';
                        tabstr += '<td class="text-center">';
                        tabstr += '<a href="javascript:void(0);" name="mci_delbtn" aid="' + $(this).children("mci_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }

                tabstr += AddCostItemNewRow();
                $("#costitemlist tbody").append(tabstr);
                GetCostFoodDDL();
                GetCostCompanyDDL();
            }
        }
    });
}

function AddCostItemNewRow() {
    var str = '<tr>';
    str += '<td class="text-center"><select class="inputex" name="ddlCostitem"></select></td>';
    str += '<td class="text-center"><input type="text" class="inputex width100 num calculate" name="mciNum" value="" /></td>';
    str += '<td class="text-center"></td>';
    str += '<td class="text-center"><input type="text" class="inputex width100 num calculate" name="mciUnitPrice" value="" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex width100 num" name="mciPrice" value="" /></td>';
    str += '<td class="text-center"><select class="inputex" name="ddlCostfirm"></select></td>';
    str += '<td class="text-center"><a href="javascript:void(0);" name="costitem_addbtn">新增</a></td>';
    str += '</tr>';
    return str;
}

function GetCostFoodDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostController.aspx",
        data: {
            mode: "CostFoodSelectList"
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
                $("select[name='ddlCostitem']").empty();
                var selectstr = '<option value="">請選擇</option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("mcf_guid").text().trim() + '" ounit="' + $(this).children("mcf_unit").text().trim() + '">' + $(this).children("mcf_name").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("select[name='ddlCostitem']").append(selectstr);
            }
        }
    });
}

function GetCostCompanyDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostController.aspx",
        data: {
            mode: "CostCompanySelectList"
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
                $("select[name='ddlCostfirm']").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("mcc_guid").text().trim() + '">' + $(this).children("mcc_name").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("select[name='ddlCostfirm']").append(selectstr);
            }
        }
    });
}

// 收支損益表
function GetStatisticsTable() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostController.aspx",
        data: {
            mode: "StatisticsTable"
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
                $("#statisticslist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center">' + $.FormatThousandGroup($(this).children("TotalCost").text().trim()) + '</td>';
                        tabstr += '<td class="text-center">' + $.FormatThousandGroup($(this).children("TotalIncome").text().trim()) + '</td>';
                        tabstr += '</tr>';
                        tabstr += '<tr><td colspan="2" class="text-center"><span class="fw-bold text-primary">結餘</span>&nbsp;';
                        var TotalPrice = parseInt($(this).children("TotalIncome").text().trim()) - parseInt($(this).children("TotalCost").text().trim());
                        tabstr += '<span class="fw-bold text-primary">' + $.FormatThousandGroup(TotalPrice) + '</span>';
                        tabstr += '</td></tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="2">查詢無資料</td></tr>';
                $("#statisticslist tbody").append(tabstr);
            }
        }
    });
}


// ※品名管理※
$(document).ready(function () {
    // Open Modal
    $(document).on("click", "#newFoodBtn", function () {
        getCostFoodData();
        $("#CostFoodModal").modal("show");
    });

    // 新增
    $(document).on("click", "a[name='food_addbtn']", function () {
        var name = $(this).closest("tr").find("input[name='mcfName']").val();
        var unit = $(this).closest("tr").find("input[name='mcfUnit']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCostFood.aspx",
            data: {
                mode: "add",
                mcf_name: name,
                mcf_unit: unit
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
                    getCostFoodData();
                }
            }
        });
    });

    // 儲存列表
    $(document).on("click", "#CostFoodSaveBtn", function () {
        var msg = '';
        $("input[name='mcf_name']").each(function () {
            if (this.value == "")
                msg = "請輸入【品名】";
        });


        if (msg != "") {
            alert("Message: \n" + msg);
            return false;
        }

        // Get form
        var form = $('#mcfForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("mode", "mod");


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCostFood.aspx",
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
                    $("#CostFoodModal").modal("hide");
                }
            }
        });
    });

    // 刪除
    $(document).on("click", "a[name='mcf_delbtn']", function () {
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMealsCostFood.aspx",
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
                        getCostFoodData();
                    }
                }
            });
        }
    });
}); // end js

function getCostFoodData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostFoodList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#mcf_tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center"><input type="text" class="inputex" name="mcf_name" value="' + $(this).children("mcf_name").text().trim() + '" /></td>';
                        tabstr += '<td class="text-center"><input type="text" class="inputex" name="mcf_unit" value="' + $(this).children("mcf_unit").text().trim() + '" size="4" /></td>';
                        tabstr += '<td class="text-center">';
                        tabstr += '<input type="hidden" class="inputex" name="mcf_id" value="' + $(this).children("mcf_id").text().trim() + '" />';
                        tabstr += '<a href="javascript:void(0);" name="mcf_delbtn" aid="' + $(this).children("mcf_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }

                tabstr += AddCostFoodNewRow();
                $("#mcf_tablist tbody").append(tabstr);
            }
        }
    });
}

function AddCostFoodNewRow() {
    var str = '<tr>';
    str += '<td class="text-center"><input type="text" class="inputex" name="mcfName" value="" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex" name="mcfUnit" value="" size="4" /></td>';
    str += '<td class="text-center"><a name="food_addbtn" href="javascript:void(0);">新增</a></td>';
    str += '</tr>';
    return str;
}


// ※進貨廠商管理※
$(document).ready(function () {
    // Open Modal
    $(document).on("click", "#newCompanyBtn", function () {
        getCompanyData();
        $("#CostCompanyModal").modal("show");
    });

    // 新增
    $(document).on("click", "a[name='company_addbtn']", function () {
        var name = $(this).closest("tr").find("input[name='mccName']").val();
        var tel = $(this).closest("tr").find("input[name='mccTel']").val();
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCostCompany.aspx",
            data: {
                mode: "add",
                mcc_name: name,
                mcc_tel: tel
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
                    getCompanyData();
                }
            }
        });
    });

    // 儲存列表
    $(document).on("click", "#CompanySaveBtn", function () {
        var msg = '';
        $("input[name='mcc_name']").each(function () {
            if (this.value == "")
                msg = "請輸入【進貨商名稱】";
        });


        if (msg != "") {
            alert("Message: \n" + msg);
            return false;
        }

        // Get form
        var form = $('#mccForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("mode", "mod");


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsCostCompany.aspx",
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
                    $("#CostCompanyModal").modal("hide");
                }
            }
        });
    });

    // 刪除
    $(document).on("click", "a[name='mcc_delbtn']", function () {
        if (confirm("刪除後資料無法復原，確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DeleteMealsCostCompany.aspx",
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
                        getCompanyData();
                    }
                }
            });
        }
    });
}); // end js

function getCompanyData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCostCompanyList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#mcc_tablist tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>';
                        tabstr += '<td class="text-center"><input type="text" class="inputex" name="mcc_name" value="' + $(this).children("mcc_name").text().trim() + '" /></td>';
                        tabstr += '<td class="text-center"><input type="text" class="inputex num" name="mcc_tel" value="' + $(this).children("mcc_tel").text().trim() + '" size="10" maxlength="20" /></td>';
                        tabstr += '<td class="text-center">';
                        tabstr += '<input type="hidden" class="inputex" name="mcc_id" value="' + $(this).children("mcc_id").text().trim() + '" />';
                        tabstr += '<a href="javascript:void(0);" name="mcc_delbtn" aid="' + $(this).children("mcc_id").text().trim() + '">刪除</a>';
                        tabstr += '</td></tr>';
                    });
                }

                tabstr += AddCompanyNewRow();
                $("#mcc_tablist tbody").append(tabstr);
            }
        }
    });
}

function AddCompanyNewRow() {
    var str = '<tr>';
    str += '<td class="text-center"><input type="text" class="inputex" name="mccName" value="" /></td>';
    str += '<td class="text-center"><input type="text" class="inputex num" name="mccTel" value="" size="10" maxlength="20" /></td>';
    str += '<td class="text-center"><a name="company_addbtn" href="javascript:void(0);">新增</a></td>';
    str += '</tr>';
    return str;
}
