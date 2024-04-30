$(document).ready(function () {
    SetPageTitle();

    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../handler/GetSysSetting.aspx",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var MealsManager, OutdoorManager, MeetingManager, Dormitory, DormitoryManager, DocManager, SystemAdmin;
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        var empno = $(this).children("員工編號").text().trim().split(',');
                        switch ($(this).children("類別").text().trim()) {
                            case "sa":
                                SystemAdmin = empno;
                                break;
                        }
                    });
                }

                //if ($.inArray($("LoginEmpno", data).text(), MealsManager) != -1 || $.inArray($("LoginEmpno", data).text(), SystemAdmin) != -1)
                //    $(".meals").show();

                if ($.inArray($("LoginEmpno", data).text(), SystemAdmin) != -1)
                    $(".sa").show();
            }
        }
    });

    $(document).on("click", "#signoutbtn", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "../handler/Signout.aspx",
            error: function (xhr) {
                alert("Error: " + xhr.status);
                console.log(xhr.responseText);
            },
            success: function (data) {
                var lo = location.pathname.split('/');
                var page = lo[lo.length - 1];
                location.href = page;
            }
        });
    });

    // 第三方登入
    $('#ToUOF').click(function () {
        ThirdLogin();
    });
}); // end js


function SetPageTitle() {
    var lo = location.pathname.split('/');
    var pageName = lo[lo.length - 1];

    $.ajax({
        type: "GET",
        url: "../PageTitle/pgTitle.xml",
        dataType: "xml",
        error: function (xhr) {
            alert("Error: " + xhr.status);
            console.log(xhr.responseText);
        },
        success: function (data) {
            var mt = $(data).find("page[name='" + pageName + "']").attr("menuTitle");
            var pt = $(data).find("page[name='" + pageName + "']").attr("pageTitle");
            $(".MenuTitle").html(mt);
            $(".PageTitle").html(pt);
        }
    });
}

// 第三方登入
function ThirdLogin() {
    var form = document.getElementById("form_signForm");
    $("#signForm_targetUrl").val("/WKF/FormUse/PersonalBox/MyFormList.aspx?item=SignSelf");
    form.action = "../Handler/GetReviewToUOFThirdLogin.aspx";
    form.submit();
}