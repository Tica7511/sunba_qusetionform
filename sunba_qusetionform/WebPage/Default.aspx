<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WebPage_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../page_js/ReviewToBeSign.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-xxl mb-3">
        <!--#include virtual="~/Webpage/DefaultMenu.html"-->
        <br />
        <!--<button type="button" id="btn_signForm" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">前往簽核系統</button>-->
        <div class="py-2">
            <table id="tobesignlist" class="table table-bordered table-striped font-size2">
                <thead>
                    <tr class="table-secondary">
                        <th class="text-center" nowrap="">待簽表單名稱</th>
                        <th class="text-center" nowrap="">申請日期</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
            <%--<div id="pageblock"></div>--%>
        </div>
    </div>
</asp:Content>

