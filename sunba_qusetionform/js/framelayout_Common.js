// JavaScript Document
$(document).ready(MainFunction);
$( window ).resize(ResizeFunction);
/* 主程式 */
function MainFunction(){
    clonemenu();//RWD選單
    //superfishmenufix();////引用選單項目平均寬度

}
//RWD選單處理:
function clonemenu(){
    //複製選單到sidebar
    var clonemenu = $(".superfishmenu > ul").clone(false);
    //移除superfish的id與class並給予新id
    clonemenu.appendTo("#sidebarmenu").attr("id", "sidemenu");
    //啟動下拉選單superfish
    //$(".superfishmenu > ul").superfish({delay:0}).supposition();
    //若下拉選單產生空間不足以至無法完整呈現時，引用<script src="js/supposition.js"></script>並使用上述方法
    $(".superfishmenu > ul").superfish({delay:0});
    //metisMenu menu初始html設定
    $('#sidemenu li a').each(function(){
        if($(this).next().is('ul')){
            $(this).addClass("has-arrow");
        }
    });
    //套用metisMenu menu 官網https://mm.onokumus.com/
    $('#sidemenu').metisMenu();

    //側選單顯示按釷
    $("#switchsidebarmenu").on("click",function(){
        //$(".ochisidebar").toggle();
        $(".ochisidebar").animate({
            height: "toggle"
        },300);
    })
}

function ResizeFunction(){
    //superfishmenufix();////引用選單項目平均寬度
    //當mobile menu開啟後，若恢復成桌機尺寸，則隱藏mobile menu
    var Winwdth=$(window).width();
    if(Winwdth > 993){
        $(".ochisidebar").css("display","none");
    }
}
//選單項目平均寬度
function superfishmenufix(){
    var menuitem = $(".superfishmenu > ul > li").length;
    var menutotalwidth = $(".ochibasecontainerF").width();
    var menuitemwidth = menutotalwidth / menuitem;
    $(".superfishmenu > ul > li > a ").css({"width":menuitemwidth + "px","text-align" : "center"});

}

