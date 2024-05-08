$(document).ready(RWDlayoutIni);
$(window).on('resize', RWDlayoutIni);

//RWD layout function
function RWDlayoutIni() {
    /* sidebar menu */
    $("#metismenu").metisMenu({
        toggle: false,//可展開所有項目
    });
    var docwidth = $(window).width();
    var docheight = $(window).height();
    var docheightpx = docheight + "px";
    var sidebarwrapper = $("#sidebar-wrapper");
    var pagecontentwrapper = $("#page-content-wrapper");
    var pagecontentheader = $("#page-content-header");
    sidebarwrapper.css("height", docheightpx);//修正側欄選單高度超過視窗時無法顯示捲軸
    var containerochi = $(".container-xxl");//客製化內容寬度，取代container-xxl
    //修改選單寬度要同步調整style.css檔
    var sidebarwidth = "-250px";
    var sidebarwidthR = "250px";
    var contentheaderwidth = docwidth - 250 + "px";

    sidebarwrapper.css({
        marginLeft: sidebarwidth,
    });
    pagecontentheader.css({
        marginLeft: 0,
        width: "100%",
    });
    pagecontentwrapper.css({
        marginLeft: 0,
    })
    //container-ochi寬度設定
    if (docwidth > 1650) {
        containerochi.css({
            maxWidth: "100%",
        });
    } else if (docwidth > 768 && docwidth < 1649) {
        containerochi.css({
            maxWidth: contentheaderwidth,
        });
    } else {
        containerochi.css({
            maxWidth: "100%"
        });
    }



}
//切換選單動作
$("#sidebarToggle").on("click", function () {
    var docwidth = $(window).width();
    var sidebarwrapper = $("#sidebar-wrapper");
    var pagecontentwrapper = $("#page-content-wrapper");
    var pagecontentheader = $("#page-content-header");
    //修改選單寬度要同步調整style.css檔
    var sidebarwidth = "-250px";
    var sidebarwidthR = "250px";
    var contentheaderwidth = docwidth - 250 + "px";
    if (sidebarwrapper.css('marginLeft') == sidebarwidth) {
        pagecontentwrapper.animate({
            marginLeft: sidebarwidthR,
        }, 300);
        pagecontentheader.css({
            width: contentheaderwidth,
        });
        sidebarwrapper.animate({
            marginLeft: 0,
        }, 100);
    } else {
        pagecontentwrapper.animate({
            marginLeft: 0,
        }, 300);
        pagecontentheader.css({
            width: "100%",
        });
        sidebarwrapper.animate({
            marginLeft: sidebarwidth,
        }, 100);
    }
});




