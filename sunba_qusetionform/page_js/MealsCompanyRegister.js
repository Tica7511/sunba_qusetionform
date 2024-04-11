$(document).ready(function () {
    if ($.getQueryString("v") == "" || $.getQueryString("tp") == "")
        location.href = "MealsCompany.aspx";

    GetComapnyName();
    getData();
    GetLocationDDL();

    //限制只能輸入數字
    $(document).on("keyup", ".num", function () {
        var reg = /[^0-9]/g;
        if (reg.test(this.value)) {
            this.value = this.value.replace(reg, '');
        }
    });

    // 批次設定
    $(document).on("click", "#setbtn", function () {
        // 午餐
        //var eatLunch = $("input[name='setL']:checked").val();
        var LunchNum = $("#setL_num").val();
        var LunchPlace = $("#setL_place").val();
        var LunchExclude = $("#setL_exclude").multipleSelect('getSelects');

        var LunchStartDate = new Date($("#setL_startdate").val());
        var LunchEndDate = new Date($("#setL_enddate").val());
        for (var idate = LunchStartDate; idate <= LunchEndDate; idate = new Date(LunchStartDate.setDate(LunchStartDate.getDate() + 1))) {
            var rowid = $.datepicker.formatDate('yymmdd', new Date(idate));
            if ($.inArray(rowid, LunchExclude) == -1) {
                var tbRow = $("#tablist tbody tr[id='" + rowid + "']");
                tbRow.find("input[name='rbLunch_" + rowid + "'][value='Y']").prop('checked', true);
                tbRow.find("input[name='NumL']").val(LunchNum);
                tbRow.find("select[name='PlaceL']").val(LunchPlace);
            }
            else {
                var tbRow = $("#tablist tbody tr[id='" + rowid + "']");
                tbRow.find("input[name='rbLunch_" + rowid + "'][value='N']").prop('checked', true);
            }
        }

        // 晚餐
        //var eatDinner = $("input[name='setD']:checked").val();
        var DinnerNum = $("#setD_num").val();
        var DinnerPlace = $("#setD_place").val();
        var DinnerExclude = $("#setD_exclude").multipleSelect('getSelects');

        var DinnerStartDate = new Date($("#setD_startdate").val());
        var DinnerEndDate = new Date($("#setD_enddate").val());
        for (var idate = DinnerStartDate; idate <= DinnerEndDate; idate = new Date(DinnerStartDate.setDate(DinnerStartDate.getDate() + 1))) {
            var rowid = $.datepicker.formatDate('yymmdd', new Date(idate));
            if ($.inArray(rowid, DinnerExclude) == -1) {
                var tbRow = $("#tablist tbody tr[id='" + rowid + "']");
                tbRow.find("input[name='rbDinner_" + rowid + "'][value='Y']").prop('checked', true);
                tbRow.find("input[name='NumD']").val(DinnerNum);
                tbRow.find("select[name='PlaceD']").val(DinnerPlace);
            }
            else {
                var tbRow = $("#tablist tbody tr[id='" + rowid + "']");
                tbRow.find("input[name='rbDinner_" + rowid + "'][value='N']").prop('checked', true);
            }
        }

        $("#SettingModal").modal("hide");
    });

    $(document).on("click", "#savebtn", function () {
        var emptyStatus = false;
        $("input[name='NumL']").each(function () {
            if (this.value == "") emptyStatus = true;
        });
        $("input[name='NumD']").each(function () {
            if (this.value == "") emptyStatus = true;
        });

        if (emptyStatus) {
            alert("份數不得為空");
            return false;
        }

        // disabled Request 抓不到值
        $("input:radio").prop("disabled", false);
        $("input[name='NumL']").prop("disabled", false);
        $("select[name='PlaceL']").prop("disabled", false);
        $("input[name='NumD']").prop("disabled", false);
        $("select[name='PlaceD']").prop("disabled", false);

        // Get form
        var form = $('#dataForm')[0];

        // Create an FormData object 
        var data = new FormData(form);

        // If you want to add an extra field for the FormData
        data.append("category", $.getQueryString("tp"));
        data.append("RegisterId", $.getQueryString("v"));


        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddMealsRegister.aspx",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            error: function (xhr) {
                alert("Error: " + xhr.status);
                console.log(xhr.responseText);
            },
            complete: function () {
                getData();
            },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    alert($(data).find("Error").attr("Message"));
                }
                else {
                    alert($("Response", data).text());
                }
            }
        });
    });
}); // end js

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsRegister.aspx",
        data: {
            category: $.getQueryString("tp"),
            RegisterId: $.getQueryString("v")
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
                        var weekday = ["日", "一", "二", "三", "四", "五", "六"];
                        var today = $.datepicker.formatDate('yy-mm-dd', new Date());
                        var rowid = $.datepicker.formatDate('yymmdd', new Date($(this).children("mr_date").text().trim()));
                        var passStatus = (parseInt(rowid) <= parseInt($.datepicker.formatDate('yymmdd', new Date()))) ? "disabled" : "";

                        tabstr += '<tr id="' + rowid + '">';
                        // 日期
                        var RegDate = new Date($(this).children("mr_date").text().trim());
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("mr_date").text().trim())) + ' (' + weekday[RegDate.getDay()] + ')';
                        tabstr += '<input type="hidden" name="dtRow" value="' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("mr_date").text().trim())) + '" /></td>';
                        // 是否用午餐
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<input class="form-check-input" type="radio" value="Y" name="rbLunch_' + rowid + '" ';
                        if ($(this).children("mr_lunch").text().trim() == "Y") tabstr += 'checked ';
                        tabstr += passStatus + ' />';
                        tabstr += '<label class="form-check-label">&nbsp;是&nbsp;</label>';
                        tabstr += '<input class="form-check-input" type="radio" value="N" name="rbLunch_' + rowid + '" ';
                        if ($(this).children("mr_lunch").text().trim() == "N") tabstr += 'checked ';
                        tabstr += passStatus + ' />';
                        tabstr += '<label class="form-check-label">&nbsp;否</label>';
                        tabstr += '</td>';
                        // 午餐份數
                        var LunchNum = ($(this).children("mr_lunch_num").text().trim() == "0") ? "1" : $(this).children("mr_lunch_num").text().trim();
                        tabstr += '<td class="text-center" nowrap="nowrap"><input type="text" class="inputex num" size="3" name="NumL" value="' + LunchNum + '" ' + passStatus + ' /></td>';
                        // 午餐地點
                        var placeL = $(this).children("mr_lunch_location").text().trim();
                        tabstr += '<td class="text-center" nowrap="nowrap"><select class="inputex width100" name="PlaceL" ' + passStatus + '>';
                        if ($(data).find("place_item").length > 0) {
                            $(data).find("place_item").each(function (i) {
                                if ($(this).children("ml_guid").text().trim() == placeL)
                                    tabstr += '<option value="' + $(this).children("ml_guid").text().trim() + '" selected>' + $(this).children("ml_name").text().trim() + '</option>';
                                else
                                    tabstr += '<option value="' + $(this).children("ml_guid").text().trim() + '">' + $(this).children("ml_name").text().trim() + '</option>';
                            });
                        }
                        tabstr += '</select></td>';
                        // 是否用晚餐
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        tabstr += '<input class="form-check-input" type="radio" value="Y" name="rbDinner_' + rowid + '" ';
                        if ($(this).children("mr_dinner").text().trim() == "Y") tabstr += 'checked ';
                        tabstr += passStatus + ' />';
                        tabstr += '<label class="form-check-label">&nbsp;是&nbsp;</label>';
                        tabstr += '<input class="form-check-input" type="radio" value="N" name="rbDinner_' + rowid + '" ';
                        if ($(this).children("mr_dinner").text().trim() == "N") tabstr += 'checked ';
                        tabstr += passStatus + ' />';
                        tabstr += '<label class="form-check-label">&nbsp;否</label>';
                        tabstr += '</td>';
                        // 晚餐份數
                        var DinnerNum = ($(this).children("mr_dinner_num").text().trim() == "0") ? "1" : $(this).children("mr_dinner_num").text().trim();
                        tabstr += '<td class="text-center" nowrap="nowrap"><input type="text" class="inputex num" size="3" name="NumD" value="' + DinnerNum + '" ' + passStatus + ' /></td>';
                        // 晚餐地點
                        var placeD = $(this).children("mr_dinner_location").text().trim();
                        tabstr += '<td class="text-center" nowrap="nowrap"><select class="inputex width100" name="PlaceD" ' + passStatus + '>';
                        if ($(data).find("place_item").length > 0) {
                            $(data).find("place_item").each(function (i) {
                                if ($(this).children("ml_guid").text().trim() == placeD)
                                    tabstr += '<option value="' + $(this).children("ml_guid").text().trim() + '" selected>' + $(this).children("ml_name").text().trim() + '</option>';
                                else
                                    tabstr += '<option value="' + $(this).children("ml_guid").text().trim() + '">' + $(this).children("ml_name").text().trim() + '</option>';
                            });
                        }
                        tabstr += '</select></td>';
                        tabstr += '</tr>';

                    });
                }
                else
                    tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
                $("#tablist tbody").append(tabstr);

                // 批次設定 datepick range 範圍
                var setStart = new Date().setDate(new Date().getDate() + 1);
                setStart = $.datepicker.formatDate('yy-mm-dd', new Date(setStart));
                $(".dateRange").attr("min", setStart);
                $(".dateRange").attr("max", $("dateEnd", data).text());

                // ddl 排除日期
                var startDate = new Date($("dateStart", data).text());
                var endDate = new Date($("dateEnd", data).text());
                var daysOfWeek = ["日", "一", "二", "三", "四", "五", "六"];
                var ddlstr = '';
                for (var d = startDate; d <= endDate; d = new Date(startDate.setDate(startDate.getDate() + 1))) {
                    ddlstr += '<option value="' + $.datepicker.formatDate('yymmdd', new Date(d)) + '">';
                    ddlstr += $.datepicker.formatDate('mm/dd', new Date(d)) + "(" + daysOfWeek[d.getDay()] + ")";
                    ddlstr += '</option>';
                }
                $(".ddlexclude").append(ddlstr);

                $('.multiple-select').multipleSelect({
                    selectAll: false,
                    multiple: true,
                    multipleWidth: 120,
                    minimumCountSelected: 20
                });
            }
        }
    });
}

function GetComapnyName() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsCompanyDetail.aspx",
        data: {
            mode: "PageTitle",
            gid: $.getQueryString("v")
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
                        $("#CompanyName").html($(this).children("mc_name").text().trim());
                    });
                }
            }
        }
    });
}

function GetLocationDDL() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetMealsLocationSelectList.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("select[name='ddlplace']").empty();
                var selectstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        selectstr += '<option value="' + $(this).children("ml_guid").text().trim() + '">' + $(this).children("ml_name").text().trim() + '</option>';
                    });
                }
                else
                    selectstr += '<option value="">查詢無資料</option>';
                $("select[name='ddlplace']").append(selectstr);
            }
        }
    });
}

