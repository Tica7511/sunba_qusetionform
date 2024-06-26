﻿/*-------------------------------------
Title：分頁
Author：Nick Lai
Create Date：2018/03/16
Modify Date：2018/11/08
Version：1.1
-------------------------------------*/

var Page = {

    // 參數設定
    Option: {
        Selector: '', // Selector
        FunctionName: 'getData',
        PageSize: 10, // 每頁顯示資料筆數,預設10
        PageNum: 10, // 分頁頁籤顯示數,預設10
        PageNow: 0, // 當前頁數
        HomeBtn: false, // 顯示首頁按鈕
        LastBtn: false, // 顯示最末頁按鈕
        PrevBtn: true, // 顯示上一頁按鈕
        NextBtn: true, // 顯示下一頁按鈕
        PrevStep: false, // 顯示上一階按鈕
        NextStep: false, // 顯示下一階按鈕
        JumpVisible: false, //顯示跳頁功能
        TotalDataVisible: false, // 顯示總筆數
        TotalPageVisible: false, // 顯示總頁數
        TotalPages: 0 // 總頁數
    },

    //Math.ceil -> 無條件進位
    //javascript:scroll(0,0) 回到頂端
    CreatePage: function (nowPage, totalData) {
        Page.Option.PageNow = nowPage;
        var PageListStr = '';
        var PageNum = Page.Option.PageNum;
        var ItemNum = (Page.Option.PageSize == 0) ? totalData : Page.Option.PageSize;
        var PagesLen = Math.ceil(totalData / ItemNum);
        Page.Option.TotalPages = PagesLen;
        if (PagesLen <= 1)
            $(Page.Option.Selector).hide();
        else
            $(Page.Option.Selector).show();
        
        //總筆數
        if (Page.Option.TotalDataVisible == true) {
            PageListStr += totalData + " results";
        }

        //總頁數 
        if (Page.Option.TotalPageVisible == true) {
            PageListStr += " / " + PagesLen + " pages&nbsp;";
        }

        //首頁
        if (Page.Option.HomeBtn == true) {
            if (nowPage == 0) PageListStr += '<span>首頁</span>&nbsp;&nbsp;';
            else PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName + '(0)">首頁</a>&nbsp;&nbsp;';
        }

        //上一階
        if (Page.Option.PrevStep == true) {
            if (nowPage == 0) { PageListStr += '<span>上' + PageNum + '階</span>&nbsp;&nbsp;' }
            else if (nowPage >= 10) { PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName + '(' + (nowPage - PageNum) + ')">上' + PageNum + '階</a>' }
            else { PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName + '(0)">上' + PageNum + '階</a>&nbsp;&nbsp;' }
        }

        PageListStr += '<nav><ul class="pagination justify-content-center">';

        //上一頁
        if (Page.Option.PrevBtn == true) {
            if (nowPage != 0) { PageListStr += '<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="' + Page.Option.FunctionName + '(' + (nowPage - 1) + ')">上一頁</a></li>' }
            else { PageListStr += '<li class="page-item"><a class="page-link" href="javascript:void(0);">上一頁</a></li>' }
        }

        //頁碼
        var startPage, endPage;
        var pn = (PageNum - 1);
        var tmp = nowPage % PageNum;

        endPage = (nowPage - tmp) + pn;
        startPage = endPage - pn;

        //最後一頁不大於總頁數
        if (endPage > (PagesLen - 1)) endPage = (PagesLen - 1);

        for (var i = startPage; i <= endPage; i++) {
            if (i == nowPage) PageListStr += '<li class="page-item"><a class="page-link active" href="javascript:void(0);">' + (i + 1) + '</a></li>';
            else PageListStr += '<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="' + Page.Option.FunctionName + '(' + i + ')">' + (i + 1) + '</a></li>';
        }

        //下一頁
        if (Page.Option.NextBtn == true) {
            if (nowPage != (PagesLen - 1)) { PageListStr += '<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="' + Page.Option.FunctionName + '(' + (nowPage + 1) + ')">下一頁</a></li>' }
            else { PageListStr += '<li class="page-item"><a class="page-link" href="javascript:void(0);">下一頁</a></li>' }
        }

        PageListStr += '</ul></nav>';

        //下一階
        if (Page.Option.NextStep == true) {
            if (nowPage == (PagesLen - 1)) { PageListStr += '<span>下' + PageNum + '階</span>&nbsp;&nbsp;' }
            else if ((nowPage + 10) >= (PagesLen - 1)) { PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName + '(' + (PagesLen - 1) + ')">下' + PageNum + '階</a>&nbsp;&nbsp;' }
            else { PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName + '(' + (nowPage + PageNum) + ')">下' + PageNum + '階</a>&nbsp;&nbsp;' }
        }

        //最末頁
        if (Page.Option.LastBtn == true) {
            if (nowPage != (PagesLen - 1)) { PageListStr += '<a href="javascript:scroll(0,0)" onclick="' + Page.Option.FunctionName+'(' + (PagesLen - 1) + ')">最末頁</a>' }
            else { PageListStr += '<span class="disabled">最末頁</span>' }
        }

        //跳頁
        if (Page.Option.JumpVisible == true) {
            PageListStr += '，移至 <input type="text" name="PageJump" onkeypress="JumpFun(event)" style="width:25px; height:20px;" /> 頁'
        }
        
        $(Page.Option.Selector).html(PageListStr);
    }
}

//跳頁Function
function JumpFun(e) {
    if (e.keyCode == 13) {
        var p = parseInt(e.target.value) - 1;

        if (!isNaN(p))
            Page.Option.FunctionName(p);
        else
            e.target.value = "";
    }
}