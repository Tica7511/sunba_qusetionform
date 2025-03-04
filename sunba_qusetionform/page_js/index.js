$(document).ready(function () {
    var ckclosedvalue = '';
    var ckincontract = '';
    getDDL('001', '', 'sel_questionType');
    getCompanyList("orgnization", "");
    getCompanyList("empname", "");
    getDDL2('005', '', 'div_type');
    getDDL('004', '', 'sel_state');
    $("input[name='ck_isclosed'][value='Y']").prop("checked", true);
    $("input[name='ck_isclosed'][value='']").prop("checked", true);
    $('input[name="ck_isclosed"]:checked').each(function () {
        if (ckclosedvalue == "") {
            ckclosedvalue += this.value;
        }
        else {
            ckclosedvalue += "," + this.value;
        }
    });
    $("#Misclosed").val(ckclosedvalue);
    $("input[name='ck_isincontract'][value='Y']").prop("checked", true);
    $("input[name='ck_isincontract'][value='']").prop("checked", true);
    $('input[name="ck_isincontract"]:checked').each(function () {
        if (ckincontract == "") {
            ckincontract += this.value;
        }
        else {
            ckincontract += "," + this.value;
        }
    });
    $("#Misincontract").val(ckincontract);
    getData(0);

    $("#exportbtn").attr("href", "../EXPORTEXCEL.aspx?item=" + $("#Mitem").val() + "&num=" + $("#Mnum").val() + "&questionType=" + $("#MquestionType").val() +
        "&empid=" + $("#Mempid").val() + "&fillformname=" + $("#Mfillformname").val() + "&orgnization=" + $("#Morgnization").val() + "&startday=" + $("#Mstartday").val() +
        "&endday=" + $("#Mendday").val() + "&state=" + $("#Mstate").val() + "&content=" + $("#Mcontent").val() + "&replycontent=" + $("#Mreplycontent").val() +
        "&urgency=" + $("#Mtype").val() + "&isclosed=" + $("#Misclosed").val() + "&isincontract=" + $("#Misincontract").val());

    //tinymce
    tinymce.init({
        selector: '#n_suggestion',
        height: 300,
        language: "zh_TW",
        plugins: 'advlist autolink lists link image table charmap print preview hr anchor pagebreak code paste',
        paste_data_images: true,
        toolbar: "undo redo | fontselect fontsizeselect | styleselect | forecolor backcolor | bold italic underline | alignleft aligncenter alignright alignjustify | link image",
        font_formats: "新細明體=新細明體;標楷體=標楷體;微軟正黑體=微軟正黑體;Andale Mono=andale mono,times; Arial=arial,helvetica,sans-serif; Arial Black=arial black,avant garde; Book Antiqua=book antiqua,palatino; Comic Sans MS=comic sans ms,sans-serif; Courier New=courier new,courier; Georgia=georgia,palatino; Helvetica=helvetica; Impact=impact,chicago; Symbol=symbol; Tahoma=tahoma,arial,helvetica,sans-serif; Terminal=terminal,monaco; Times New Roman=times new roman,times; Trebuchet MS=trebuchet ms,geneva; Verdana=verdana,geneva; Webdings=webdings; Wingdings=wingdings,zapf dingbats;",
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt 42pt 60pt 72pt",
        image_uploadtab: true, // 將此選項設置為 true，僅顯示上傳圖片的選項
        images_upload_handler: function (blobInfo, success, failure) {
            var formData = new FormData();
            formData.append('file', blobInfo.blob());
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../tinymce/myUpload/imgUpload.aspx",
                data: formData,
                processData: false,
                contentType: false,
                cache: false,
                error: function (xhr) {
                    failure("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        var img = new Image();
                        img.src = $("Response", data).text();
                        // set image width & height
                        img.onload = function () {
                            success($("Response", data).text());
                        }
                    }
                }
            });
        },
        setup: function (editor) {
            editor.on('paste', function (e) {
                var clipboardData = e.clipboardData || window.clipboardData;
                var items = clipboardData.items;

                for (var i = 0; i < items.length; i++) {
                    if (items[i].type.indexOf("image") !== -1) {
                        var blob = items[i].getAsFile();
                        var reader = new FileReader();

                        reader.onload = function (event) {
                            var base64 = event.target.result;
                            var filename = blob.name; // 取得原始檔案名稱
                            var extension = filename.split('.').pop(); // 取得副檔名

                            // 將 base64 資料轉換為 Blob 物件
                            var blob = base64ToBlob(base64, 'image/' + extension);
                        };

                        reader.readAsDataURL(blob);
                    }
                }
            });
        }
    });

    //tinymce
    tinymce.init({
        selector: '#n_replies',
        height: 300,
        language: "zh_TW",
        plugins: 'advlist autolink lists link image table charmap print preview hr anchor pagebreak code paste',
        paste_data_images: true,
        toolbar: "undo redo | fontselect fontsizeselect | styleselect | forecolor backcolor | bold italic underline | alignleft aligncenter alignright alignjustify | link image",
        font_formats: "新細明體=新細明體;標楷體=標楷體;微軟正黑體=微軟正黑體;Andale Mono=andale mono,times; Arial=arial,helvetica,sans-serif; Arial Black=arial black,avant garde; Book Antiqua=book antiqua,palatino; Comic Sans MS=comic sans ms,sans-serif; Courier New=courier new,courier; Georgia=georgia,palatino; Helvetica=helvetica; Impact=impact,chicago; Symbol=symbol; Tahoma=tahoma,arial,helvetica,sans-serif; Terminal=terminal,monaco; Times New Roman=times new roman,times; Trebuchet MS=trebuchet ms,geneva; Verdana=verdana,geneva; Webdings=webdings; Wingdings=wingdings,zapf dingbats;",
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt 42pt 60pt 72pt",
        image_uploadtab: true, // 將此選項設置為 true，僅顯示上傳圖片的選項
        images_upload_handler: function (blobInfo, success, failure) {
            var formData = new FormData();
            formData.append('file', blobInfo.blob());
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../tinymce/myUpload/imgUpload.aspx",
                data: formData,
                processData: false,
                contentType: false,
                cache: false,
                error: function (xhr) {
                    failure("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        var img = new Image();
                        img.src = $("Response", data).text();
                        // set image width & height
                        img.onload = function () {
                            success($("Response", data).text());
                        }
                    }
                }
            });
        },
        setup: function (editor) {
            editor.on('paste', function (e) {
                var clipboardData = e.clipboardData || window.clipboardData;
                var items = clipboardData.items;

                for (var i = 0; i < items.length; i++) {
                    if (items[i].type.indexOf("image") !== -1) {
                        var blob = items[i].getAsFile();
                        var reader = new FileReader();

                        reader.onload = function (event) {
                            var base64 = event.target.result;
                            var filename = blob.name; // 取得原始檔案名稱
                            var extension = filename.split('.').pop(); // 取得副檔名

                            // 將 base64 資料轉換為 Blob 物件
                            var blob = base64ToBlob(base64, 'image/' + extension);
                        };

                        reader.readAsDataURL(blob);
                    }
                }
            });
        }
    });

    //查詢按鈕
    $(document).on("click", "#querybtn", function () {
        var cktypevalue = '';
        var ckclosedvalue = '';
        var ckincontract = '';

        $("#Mitem").val($("#txt_item").val());
        $("#Mnum").val($("#txt_num").val());
        $("#MquestionType").val($("#sel_questionType option:selected").val());
        $("#Mempid").val($("#sel_empid option:selected").val());
        $("#Mfillformname").val($("#sel_fillformname option:selected").val());
        $("#Mcompanylist").val($("#sel_companylist").val());
        $("#Morgnization").val($("#sel_orgnization").val());
        $("#Mstartday").val(insertsqlDate($("#txt_startday").val()));
        $("#Mendday").val(insertsqlDate($("#txt_endday").val()));
        $("#Mstate").val($("#sel_state").val());
        $('input[name="cktype"]:checked').each(function () {
            if (cktypevalue == "") {
                cktypevalue += this.value;
            }
            else {
                cktypevalue += "," + this.value;
            }

        });
        $("#Mtype").val(cktypevalue);
        $('input[name="ck_isclosed"]:checked').each(function () {
            if (ckclosedvalue == "") {
                ckclosedvalue += this.value;
            }
            else {
                ckclosedvalue += "," + this.value;
            }
        });
        $("#Misclosed").val(ckclosedvalue);
        $('input[name="ck_isincontract"]:checked').each(function () {
            if (ckincontract == "") {
                ckincontract += this.value;
            }
            else {
                ckincontract += "," + this.value;
            }
        });
        $("#Misincontract").val(ckincontract);
        $("#Mcontent").val($("#txt_content").val());
        $("#Mreplycontent").val($("#txt_replycontent").val());

        getData(0);

        $("#exportbtn").attr("href", "../EXPORTEXCEL.aspx?item=" + $("#Mitem").val() + "&num=" + $("#Mnum").val() + "&questionType=" + $("#MquestionType").val() +
            "&empid=" + $("#Mempid").val() + "&fillformname=" + $("#Mfillformname").val() + "&orgnization=" + $("#Morgnization").val() + "&startday=" + $("#Mstartday").val() +
            "&endday=" + $("#Mendday").val() + "&state=" + $("#Mstate").val() + "&content=" + $("#Mcontent").val() + "&replycontent=" + $("#Mreplycontent").val() +
            "&urgency=" + $("#Mtype").val() + "&isclosed=" + $("#Misclosed").val() + "&isincontract=" + $("#Misincontract").val());
    });

    //清除按鈕
    $(document).on("click", "#clearbtn", function () {
        $("#txt_item").val('');
        $("#txt_num").val('');
        $("#sel_questionType").val('');
        $("#sel_orgnization").val('');
        getCompanyList('empname', '')
        $("#sel_empid").val('');
        getDDL2('005', '', 'div_type');
        $("input[name='ck_isclosed'][value='Y']").prop("checked", true);
        $("input[name='ck_isclosed'][value='']").prop("checked", true);
        $("input[name='ck_isincontract'][value='Y']").prop("checked", true);
        $("input[name='ck_isincontract'][value='']").prop("checked", true);
        $("#sel_state").val('');
        $("#txt_startday").val('');
        $("#txt_endday").val('');
        $("#txt_content").val('');
        $("#txt_replycontent").val('');
    });

    //變更部門
    $(document).on("change", "#sel_orgnization", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/GetCompanyList.aspx",
            data: {
                type: "empname",
                orgnization: $("#sel_orgnization option:selected").val()
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
                    var ddlstr = '<option value=""> -- 請選擇 -- </option>';
                    if ($(data).find("data_item").length > 0) {
                        $(data).find("data_item").each(function (i) {
                            ddlstr += '<option value="' + $(this).children("帳號").text().trim() + '">' + $(this).children("姓名").text().trim() + '</option>';
                        });
                    }

                    $("#sel_empid").empty();
                    $("#sel_empid").append(ddlstr);
                }
            }
        });
    });

    // 新增按鈕
    $(document).on("click", "#newbtn", function () {
        getDDL('001', '', 'nsel_questionType');
        getDDL('005', '', 'nsel_type');
        getDDL('004', '', 'nsel_state');
        var editor = tinymce.get('n_suggestion');
        editor.setContent('');

        $("#ntxt_fillformname").val($("#SSOempid").val() + " " + $("#SSOname").val());
        $("#nsel_orgnization").val($("#SSOgroupname").val());
        $("#Qguid").val('');
        $("#ntxt_file").val('');
        $("#ffGuid").val('');
        $("input[name='ckisclosed']").prop("checked", false);
        getDataFile('01', 'tablistfile');

        $("#ntxt_num").attr("disabled", true);
        $("#nsel_questionType").attr("disabled", false);
        $("#ntxt_day").attr("disabled", true);
        $("#ntxt_file").show();
        $("#n_subbtn").show();
        $("#div_isclosed").hide();

        $(".newstr").val("");
        $("#FileList").empty();
        $("#ntxt_num").val(getSn());
        $("#ntxt_day").val(getTaiwanDate());
        $("#nsel_type").attr("disabled", false);
        $("#div_commenttinymce").show();
        $("#div_commentdiv").hide();
        $("#n_subbtn").show();
        $("#CommentModal").modal("show");
    });

    //填表人編輯按鈕
    $(document).on("click", "a[name='commenteditbtn']", function () {
        getDDL('001', '', 'nsel_questionType');
        getDDL('005', '', 'nsel_type');
        getDDL('004', '', 'nsel_state');
        var editor = tinymce.get('n_suggestion');
        editor.setContent('');
        $("#Qguid").val($(this).attr("aid"));
        $("#ntxt_file").val('');
        $("#ntxt_num").attr("disabled", true);
        $("#nsel_questionType").attr("disabled", true);
        $("#ntxt_day").attr("disabled", true);
        $("#ffGuid").val($(this).attr("aid"));
        getDataFile('01', 'tablistfile');
        getData2();

        if ($(this).attr("atype") == 'edit') {
            $("#nsel_type").attr("disabled", false);
            $("#ntxt_file").show();
            $("#div_commenttinymce").show();
            $("#div_commentdiv").hide();
            $("#n_subbtn").show();
            $("#div_isclosed").show();
            $("input[name='ckisclosed']").attr("disabled", false);
        }
        else {
            $("#nsel_type").attr("disabled", true);
            $("#ntxt_file").hide();
            $("#div_commenttinymce").hide();
            $("#div_commentdiv").show();
            $("#n_subbtn").hide();
            $("#div_isclosed").show();
            $("input[name='ckisclosed']").attr("disabled", true);
        }

        $("#CommentModal").modal("show");
    });

    //回覆人編輯按鈕
    $(document).on("click", "a[name='replyeditbtn']", function () {
        getDDL('004', '', 'nsel_state');
        $("#Qguid").val($(this).attr("aid"));
        var editor = tinymce.get('n_replies');
        editor.setContent('');        
        $("#ntxt_file2").val('');
        $("#ntxt_returnday").val(getTaiwanDate());
        $("#ntxt_finishday").val('');
        $("#nsel_state").val('');
        //$("#ck_contract").prop("checked", false);
        $("input[name='ckcontract'][value='']").prop("checked", true);
        $("#div_reply").empty();
        $("#div_reply").append('');
        $("#ffGuid").val($(this).attr("aid"));
        getDataFile('02', 'tablistfile2');
        getData3();

        if ($(this).attr("atype") == 'edit') {
            $("#ntxt_finishday").attr("disabled", false);
            $("#nsel_state").attr("disabled", false);
            //$("#ck_contract").attr("disabled", false);
            $("input[name='ckcontract']").attr("disabled", false);
            $("#ntxt_file2").show();
            $("#div_replytinymce").show();
            $("#div_replydiv").hide();
            $("#n_subbtn2").show();
        }
        else {
            $("#ntxt_finishday").attr("disabled", true);
            $("#nsel_state").attr("disabled", true);
            //$("#ck_contract").attr("disabled", true);
            $("input[name='ckcontract']").attr("disabled", true);
            $("#ntxt_file2").hide();
            $("#div_replytinymce").hide();
            $("#div_replydiv").show();
            $("#n_subbtn2").hide();
        }

        $("#ReplyModal").modal("show");
    });

    //附件彈窗按鈕
    $(document).on("click", "a[name='filesbtn']", function () {
        $("#ffGuid").val($(this).attr("aid"));
        getDataFile('01', 'tablistfile');
        getDataFile('02', 'tablistfile2');
        $("#FileModal").modal("show");
    });

    //填表人儲存按鈕
    $(document).on("click", "#n_subbtn", function () {
        var msg = '';

        if ($("#nsel_questionType").val() == "")
            msg += "請選擇【問題類型】\n";
        if ($("#nsel_type").val() == "")
            msg += "請選擇【急迫性】\n";
        if (msg != "") {
            alert(msg);
            return false;
        }

        // '<' & '>' 做全形處理
        tinymce.activeEditor.dom.addClass(tinymce.activeEditor.dom.select('img'), 'img-responsive');
        var content_tmp = tinymce.get("n_suggestion").getContent().trim().replace(/&lt;/g, "＜").replace(/&gt;/g, "＞");

        // Get form
        var form = $('#form1')[0];

        // Create an FormData object 
        var data = new FormData(form);

        var mode = ($("#Qguid").val() == "") ? "new" : "edit";

        // If you want to add an extra field for the FormData
        data.append("guid", $("#Qguid").val());
        data.append("num", encodeURIComponent($("#ntxt_num").val()));
        data.append("mode", encodeURIComponent(mode));
        data.append("questionType", encodeURIComponent($("#nsel_questionType option:selected").val()));
        data.append("day", encodeURIComponent(insertsqlDate($("#ntxt_day").val())));
        data.append("rtype", encodeURIComponent($("#nsel_type").val()));
        data.append("nContent", encodeURIComponent(content_tmp));
        var ckisclosedvalue = ''
        $('input[name="ckisclosed"]:checked').each(function () {
            ckisclosedvalue = $(this).val();
        });
        data.append("ckisclosed", encodeURIComponent(ckisclosedvalue));
        $.each($("#ntxt_file")[0].files, function (i, file) {
            data.append('file', file);
        });

        $.ajax({
            type: "POST",
            async: true, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddQuestionForm.aspx",
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
                    alert($("Response", data).text());
                    getData(0);
                    $("#CommentModal").modal("hide");
                }
            }
        });
    });

    //回覆人儲存按鈕
    $(document).on("click", "#n_subbtn2", function () {
        var msg = '';

        if ($("#ntxt_finishday").val() != '')
            if (!ValidDate($("#ntxt_finishday").val()))
                msg += "【預計完成日】日期格式不正確\n";
        if ($("#nsel_state").val() == "")
            msg += "請填寫【目前狀態】\n";        
        if (msg != "") {
            alert(msg);
            return false;
        }

        // '<' & '>' 做全形處理
        tinymce.activeEditor.dom.addClass(tinymce.activeEditor.dom.select('img'), 'img-responsive');
        var content_tmp = tinymce.get("n_replies").getContent().trim().replace(/&lt;/g, "＜").replace(/&gt;/g, "＞");

        // Get form
        var form = $('#form1')[0];

        // Create an FormData object 
        var data = new FormData(form);

        var mode = ($("#Qguid").val() == "") ? "new" : "edit";

        // If you want to add an extra field for the FormData
        data.append("guid", $("#Qguid").val());
        data.append("returnday", encodeURIComponent(insertsqlDate($("#ntxt_returnday").val())));
        data.append("finishday", encodeURIComponent(insertsqlDate($("#ntxt_finishday").val())));
        data.append("state", encodeURIComponent($("#nsel_state option:selected").val()));
        data.append("mode", encodeURIComponent(mode));
        //var ckcontractvalue = ''
        //$('input[name="ckcontract"]:checked').each(function () {
        //    if (ckcontractvalue == "") {
        //        ckcontractvalue += this.value;
        //    }
        //    else {
        //        ckcontractvalue += "," + this.value;
        //    }
        //
        //});
        //data.append("ckcontract", encodeURIComponent(ckcontractvalue));
        data.append("ckcontract", encodeURIComponent($('input[name="ckcontract"]:checked').val()));
        data.append("nContent", encodeURIComponent(content_tmp));
        $.each($("#ntxt_file2")[0].files, function (i, file) {
            data.append('file2', file);
        });

        $.ajax({
            type: "POST",
            async: true, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/AddReplyForm.aspx",
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
                    alert($("Response", data).text());
                    getData(0);
                    $("#ReplyModal").modal("hide");
                }
            }
        });
    });

    //刪除檔案按鈕
    $(document).on("click", "a[name='delbtn2']", function () {
        var tabname = '';
        var ftype = $(this).attr("ftype");
        if ($(this).attr("ftype") == '01')
            tabname = 'tablistfile';
        else
            tabname = 'tablistfile2';

        if (confirm("確定刪除?")) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/DelFile.aspx",
                data: {
                    ftype: ftype,
                    fsn: $(this).attr("fsn"),
                    guid: $(this).attr("aid")
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
                        alert($("Response", data).text());
                        getDataFile(ftype, tabname);
                    }
                }
            });
        }
    });

    // 排序 編號 asc
    $(document).on("click", "#d_numAsc", function () {
        $("#tmporderby").val("編號");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 編號 desc
    $(document).on("click", "#d_numDesc", function () {
        $("#tmporderby").val("編號");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 是否結案 asc
    $(document).on("click", "#d_isclosedAsc", function () {
        $("#tmporderby").val("是否結案");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 是否結案 desc
    $(document).on("click", "#d_isclosedDesc", function () {
        $("#tmporderby").val("是否結案");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 問題類別 asc
    $(document).on("click", "#d_questionTypeAsc", function () {
        $("#tmporderby").val("問題類別");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 問題類別 desc
    $(document).on("click", "#d_questionTypeDesc", function () {
        $("#tmporderby").val("問題類別");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 員工編號 asc
    $(document).on("click", "#d_empidAsc", function () {
        $("#tmporderby").val("員工編號");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 員工編號 desc
    $(document).on("click", "#d_empidDesc", function () {
        $("#tmporderby").val("員工編號");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 填表人 asc
    $(document).on("click", "#d_empNameAsc", function () {
        $("#tmporderby").val("填表人");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 填表人 desc
    $(document).on("click", "#d_empNameDesc", function () {
        $("#tmporderby").val("填表人");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 部門 asc
    $(document).on("click", "#d_orgnizationAsc", function () {
        $("#tmporderby").val("部門");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 部門 desc
    $(document).on("click", "#d_orgnizationDesc", function () {
        $("#tmporderby").val("部門");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 提出日期 asc
    $(document).on("click", "#d_purposeDateAsc", function () {
        $("#tmporderby").val("提出日期");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 提出日期 desc
    $(document).on("click", "#d_purposeDateDesc", function () {
        $("#tmporderby").val("提出日期");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 預計完成日 asc
    $(document).on("click", "#d_finishdayAsc", function () {
        $("#tmporderby").val("預計完成日");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 預計完成日 desc
    $(document).on("click", "#d_finishdayDesc", function () {
        $("#tmporderby").val("預計完成日");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 急迫性 asc
    $(document).on("click", "#d_urgentAsc", function () {
        $("#tmporderby").val("程度");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 急迫性 desc
    $(document).on("click", "#d_urgentDesc", function () {
        $("#tmporderby").val("程度");
        $("#tmpsortby").val("desc");
        getData(0);
    });

    // 排序 狀態 asc
    $(document).on("click", "#d_stateAsc", function () {
        $("#tmporderby").val("狀態");
        $("#tmpsortby").val("asc");
        getData(0);
    });

    // 排序 狀態 desc
    $(document).on("click", "#d_stateDesc", function () {
        $("#tmporderby").val("狀態");
        $("#tmpsortby").val("desc");
        getData(0);
    });
});

function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetQestionForm.aspx",
        data: {
            type: "list",
            PageNo: p,
            PageSize: Page.Option.PageSize,
            item: $("#Mitem").val(),
            num: $("#Mnum").val(),
            questionType: $("#MquestionType").val(),
            empid: $("#Mempid").val(),
            fillformname: $("#Mfillformname").val(),
            orgnization: $("#Morgnization").val(),
            startday: $("#Mstartday").val(),
            endday: $("#Mendday").val(),
            state: $("#Mstate").val(),
            content: $("#Mcontent").val(),
            replycontent: $("#Mreplycontent").val(),
            urgency: $("#Mtype").val(),
            isclosed: $("#Misclosed").val(),
            isincontract: $("#Misincontract").val(),
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
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("項次").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">';
                        //管理者僅能瀏覽填表內容(lapua除外)，森霸人員可以編輯自己的填表內容不是自己的僅能編輯
                        if ($("IsManager", data).text() == "Y") {
                            if ($("#SSOempid").val() == 'laputa') {
                                tabstr += '<a href="javascript:void(0);" style="text-decoration: underline;" name="commenteditbtn" atype="edit" aid="' + $(this).children("guid").text().trim() + '">' + $(this).children("編號").text().trim() + '</a>';
                            }
                            else {
                                tabstr += '<a href="javascript:void(0);" style="text-decoration: underline;" name="commenteditbtn" atype="view" aid="' + $(this).children("guid").text().trim() + '">' + $(this).children("編號").text().trim() + '</a>';
                            }                            
                        }
                        else {
                            if ($("#SSOempid").val() == $(this).children("員工編號").text().trim())
                                tabstr += '<a href="javascript:void(0);" style="text-decoration: underline;" name="commenteditbtn" atype="edit" aid="' + $(this).children("guid").text().trim() + '">' + $(this).children("編號").text().trim() + '</a>';
                            else
                                tabstr += '<a href="javascript:void(0);" style="text-decoration: underline;" name="commenteditbtn" atype="view" aid="' + $(this).children("guid").text().trim() + '">' + $(this).children("編號").text().trim() + '</a>';
                        }
                        tabstr += '</td>';
                        if ($(this).children("是否結案").text().trim() == 'Y') {
                            tabstr += '<td class="text-center"><span style="color:red">已結案</span></td>';
                        }
                        else {
                            tabstr += '<td class="text-center">尚未結案</td>';
                        }
                        tabstr += '<td class="">' + $(this).children("內容").text().trim() + '</td>';
                        tabstr += '<td class="">' + $(this).children("回覆內容R").text().trim() + '</td>';
                        //管理者僅能編輯回覆內容(lapua除外)
                        if ($("IsManager", data).text() == "Y") {
                            if ($("#SSOempid").val() == 'laputa') {
                                tabstr += '<td class="text-center" nowrap="nowrap">';
                                tabstr += '<a href="javascript:void(0);" class="btn btn-outline-primary btn-sm text-nowrap" name="replyeditbtn" atype="edit" aid="' + $(this).children("guid").text().trim() + '">編輯</a>&nbsp;&nbsp;';
                                tabstr += '</td>';
                            }
                            else {
                                //if ($("#SSOempid").val() == $(this).children("員工編號").text().trim()) {
                                //tabstr += '<td class="text-center" nowrap="nowrap">';
                                //tabstr += '<a href="javascript:void(0);" class="btn btn-outline-primary btn-sm text-nowrap" name="replyeditbtn" atype="edit" aid="' + $(this).children("guid").text().trim() + '">編輯</a>&nbsp;&nbsp;';
                                //tabstr += '</td>';
                                //}
                                //else {
                                //tabstr += '<td class="text-center" nowrap="nowrap">';
                                //tabstr += '<a href="javascript:void(0);" class="btn btn-outline-primary btn-sm text-nowrap" name="replyeditbtn" atype="view" aid="' + $(this).children("guid").text().trim() + '">瀏覽</a>&nbsp;&nbsp;';
                                //tabstr += '</td>';
                                //}
                                tabstr += '<td class="text-center" nowrap="nowrap">';
                                tabstr += '<a href="javascript:void(0);" class="btn btn-outline-primary btn-sm text-nowrap" name="replyeditbtn" atype="edit" aid="' + $(this).children("guid").text().trim() + '">編輯</a>&nbsp;&nbsp;';
                                tabstr += '</td>';
                            }
                        }
                        else {
                            tabstr += '<td class="text-center" nowrap="nowrap">';
                            tabstr += '<a href="javascript:void(0);" class="btn btn-outline-primary btn-sm text-nowrap" name="replyeditbtn" aid="' + $(this).children("guid").text().trim() + '">瀏覽</a>&nbsp;&nbsp;';
                            tabstr += '</td>';
                        }
                        tabstr += '<td class="text-center">' + $(this).children("問題類別_V").text().trim() + '</td>';
                        //tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("員工編號").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("填表人").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("部門").text().trim() + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + getDate($(this).children("提出日期").text().trim()) + '</td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("程度_V").text().trim() + '</td>';
                        //tabstr += '<td class="text-center" ><a name="filesbtn" href="javascript:void(0);" aid="' + $(this).children("guid").text().trim() +
                        //    '" class="btn btn-outline-primary btn-sm text-nowrap">附件列表</a></td>';
                    });
                }
                else {
                    tabstr += '<tr><td colspan="13">查詢無資料</td></tr>';
                }
                $("#sp_count").html($("total", data).text());
                $("#tablist tbody").append(tabstr);

                Page.Option.Selector = "#pageblock";
                Page.CreatePage(p, $("total", data).text());

                //固定頁面上內容圖片的長寬
                $(".img-responsive").css({
                    'width': '250px',
                    'height': '200px'
                });
            }
        }
    });
}

function getData2() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetQestionForm.aspx",
        data: {
            type: "data",
            guid: $("#Qguid").val()
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
                        $("#ntxt_num").val($(this).children("編號").text().trim());
                        $("#nsel_questionType").val($(this).children("問題類別").text().trim());
                        $("#nsel_orgnization").val($(this).children("部門").text().trim());
                        $("#ntxt_fillformname").val($(this).children("填表人").text().trim());
                        $("#ntxt_day").val(getDate($(this).children("提出日期").text().trim()));
                        $("#nsel_type").val($(this).children("程度").text().trim());
                        if ($(this).children("是否結案").text().trim() == 'Y')
                            $("input[name='ckisclosed']").prop("checked", true);
                        else
                            $("input[name='ckisclosed']").prop("checked", false);
                        var editor = tinymce.get('n_suggestion');
                        editor.setContent($(this).children("內容").text().trim());
                        $("#n_suggestion").val($(this).children("內容").text().trim());
                        $("#div_suggestion").empty();
                        $("#div_suggestion").append($(this).children("內容").text().trim());
                    });
                }

                // Attach
                //var filestr = '';
                //$("#FileList").empty();
                //if ($(data).find("file_item").length > 0) {
                //    $(data).find("file_item").each(function (i) {
                //        filestr += '<div class="mt-1">';
                //        filestr += '<a href="../DOWNLOAD.aspx?type=01&fsn=' + $(this).children("排序").text().trim() + '&v=' + $(this).children("EncodeGuid").text().trim() + '">' + $(this).children("原檔名").text().trim() + $(this).children("附檔名").text().trim() + '</a>';
                //        filestr += '</div>';
                //    });
                //}
                //$("#FileList").append(filestr);
            }
        }
    });
}

function getData3() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetReplyForm.aspx",
        data: {
            type: "data",
            guid: $("#Qguid").val()
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
                        $("#ntxt_returnday").val(getDate($(this).children("回覆日期").text().trim()));
                        $("#ntxt_finishday").val(getDate($(this).children("預計完成日").text().trim()));
                        $("#nsel_state").val($(this).children("目前狀態").text().trim());
                        //if ($(this).children("需求是否在第一期合約中").text().trim() == 'Y')
                        //    $("#ck_contract").prop("checked", true);
                        //else
                        //    $("#ck_contract").prop("checked", false);
                        if ($(this).children("需求是否在第一期合約中").text().trim() == 'Y') {
                            $("input[name='ckcontract'][value='Y']").prop("checked", true);
                            $("input[name='ckcontract'][value='']").prop("checked", false);
                        }
                        else {
                            $("input[name='ckcontract'][value='Y']").prop("checked", false);
                            $("input[name='ckcontract'][value='']").prop("checked", true);
                        }
                        var editor = tinymce.get('n_replies');
                        editor.setContent($(this).children("回覆內容").text().trim());
                        $("#n_replies").val($(this).children("回覆內容").text().trim());
                        $("#div_reply").empty();
                        $("#div_reply").append($(this).children("回覆內容").text().trim());
                    });
                }

                // Attach
                //var filestr = '';
                //$("#FileList").empty();
                //if ($(data).find("file_item").length > 0) {
                //    $(data).find("file_item").each(function (i) {
                //        filestr += '<div class="mt-1">';
                //        filestr += '<a href="../DOWNLOAD.aspx?type=01&fsn=' + $(this).children("排序").text().trim() + '&v=' + $(this).children("EncodeGuid").text().trim() + '">' + $(this).children("原檔名").text().trim() + $(this).children("附檔名").text().trim() + '</a>';
                //        filestr += '</div>';
                //    });
                //}
                //$("#FileList").append(filestr);
            }
        }
    });
}

function getDDL(gNo, relation, idname) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            gNo: gNo,
            relation: relation,
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
                var ddlstr = '<option value=""> -- 請選擇 -- </option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        ddlstr += '<option value="' + $(this).children("項目代碼").text().trim() + '">' + $(this).children("項目名稱").text().trim() + '</option>';
                    });
                }

                $("#" + idname).empty();
                $("#" + idname).append(ddlstr);
            }
        }
    });
}

function getDDL2(gNo, relation, idname) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCodeTable.aspx",
        data: {
            gNo: gNo,
            relation: relation,
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
                var ddlstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        ddlstr += ' <input type="checkbox" name="cktype" value="' + $(this).children("項目代碼").text().trim() + '" checked> ' + $(this).children("項目名稱").text().trim();
                    });
                }
                $("#" + idname).empty();
                $("#" + idname).append(ddlstr);
            }
        }
    });
}

function getCompanyList(type, orgnization) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetCompanyList.aspx",
        data: {
            type: type,
            orgnization: orgnization
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
                var ddlstr = '<option value=""> -- 請選擇 -- </option>';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        if (type == 'orgnization') {
                            ddlstr += '<option value="' + $(this).children("GROUP_ID").text().trim() + '">' + $(this).children("GROUP_NAME").text().trim() + '</option>';
                        }
                        else {
                            ddlstr += '<option value="' + $(this).children("帳號").text().trim() + '">' + $(this).children("姓名").text().trim() + '</option>';
                        }
                    });
                }

                if (type == 'orgnization') {
                    $("#sel_orgnization").empty();
                    $("#sel_orgnization").append(ddlstr);
                }
                else {
                    $("#sel_empid").empty();
                    $("#sel_empid").append(ddlstr);
                }
            }
        }
    });
}

function getSn() {
    var sn = '';

    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetQestionForm.aspx",
        data: {
            type: "sn",
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
                sn = $("sn", data).text()
            }
        }
    });

    return sn;
}

function getDataFile(ftype, tablename) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetFileTable.aspx",
        data: {
            guid: $("#ffGuid").val(),
            ftype: ftype
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
                $("#" + tablename + " tbody").empty();
                var tabstr = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        tabstr += '<tr>'
                        tabstr += '<td class="text-center" nowrap="nowrap"><a href="../DOWNLOAD.aspx?type=' + ftype + '&fsn=' + $(this).children("排序").text().trim() + '&v=' + $(this).children("EncodeGuid").text().trim() + '">' + $(this).children("原檔名").text().trim() + $(this).children("附檔名").text().trim() + '</a></td>';
                        tabstr += '<td class="text-center" nowrap="nowrap">' + $(this).children("上傳日期").text().trim() + '</td>';
                        if ($("#SSOempid").val() == 'laputa') {
                            tabstr += '<td class="text-center" nowrap="nowrap" name="td_edit2"><a href="javascript:void(0);" name="delbtn2" ftype=' + ftype + ' fsn=' + $(this).children("排序").text().trim() + ' aid="' + $(this).children("guid").text().trim() + '">刪除</a></td>';
                        }
                        else {
                            if ($("#SSOempid").val() == $(this).children("建立者id").text().trim())
                                tabstr += '<td class="text-center" nowrap="nowrap" name="td_edit2"><a href="javascript:void(0);" name="delbtn2" ftype=' + ftype + ' fsn=' + $(this).children("排序").text().trim() + ' aid="' + $(this).children("guid").text().trim() + '">刪除</a></td>';
                            else
                                tabstr += '<td></td>';
                        }
                        tabstr += '</tr>';
                    });
                }
                else
                    tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                $("#" + tablename + " tbody").append(tabstr);

                //if ($("#SSOempid").val() == 'laputa') {
                //    $("#th_file").show();
                //    $("#th_file2").show();
                //}
                //else {
                //    if ($("#SSOempid").val() == $(this).children("建立者id").text().trim()) {
                //        if (ftype == '01') {
                //            $("#th_file").show();
                //        }
                //        else {
                //            $("#th_file2").show();
                //        }
                //    }
                //    else {
                //        if (ftype == '01') {
                //            $("#th_file").hide();
                //        }
                //        else {
                //            $("#th_file2").hide();
                //        }
                //    }
                //}
            }
        }
    });
}

//驗證 執行日期 日期格式是否有誤
function ValidDate(str) {
    var status = true;
    var ValidAry = str.split('-');
    if (ValidAry.length < 2)
        status = false;
    else {
        // 年
        if (parseInt(ValidAry[0]) < 1 || parseInt(ValidAry[0]) > 9999)
            status = false;
        // 月
        if (parseInt(ValidAry[1]) < 1 || parseInt(ValidAry[1]) > 12)
            status = false;
        else {
            if (ValidAry[1].length != 2)
                status = false;
        }

        if (ValidAry[2] != null) {
            // 日
            if (parseInt(ValidAry[2]) < 1 || parseInt(ValidAry[2]) > 31)
                status = false;
            else {
                if (ValidAry[2].length != 2)
                    status = false;
            }
        }
    }
    return status;
}

function base64ToBlob(base64Data, contentType) {
    contentType = contentType || '';
    var sliceSize = 10240;
    var byteCharacters = atob(base64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);

        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}

function getDate(fulldate) {
    if (fulldate != '') {
        var year = fulldate.substring("0", "4");
        var month = fulldate.substring("4", "6");
        var date = fulldate.substring("6", "8");

        return year + "-" + month + "-" + date;
    }
    else {
        return fulldate;
    }
}

function insertsqlDate(fulldate) {
    if (fulldate != '') {
        var farray = new Array();
        farray = fulldate.split("-");
        var yyyy = farray[0];
        var mm = farray[1];
        var dd = farray[2];

        return yyyy + mm + dd;
    }
    else {
        return fulldate;
    }
}

function getTaiwanDate() {
    var nowDate = new Date();

    var nowYear = nowDate.getFullYear();
    var nowTwYear = nowYear;
    var nowMonth = (nowDate.getMonth() + 1);
    if (nowMonth < 10)
        nowMonth = '0' + nowMonth;
    var nowDay = nowDate.getDate();
    if (nowDay < 10)
        nowDay = '0' + nowDay;

    return nowTwYear + '-' + nowMonth + '-' + nowDay;
}