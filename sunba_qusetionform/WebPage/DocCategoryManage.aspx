<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DocCategoryManage.aspx.cs" Inherits="WebPage_DocCategoryManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../page_js/DocCategoryManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-xxl mb-3">
        <div class="mt-1">
            <table id="tablist" class="table table-bordered table-striped">
                <thead>
                    <tr class="table-secondary">
                        <th class="text-center">程序書分類名稱</th>
                        <th class="text-center" width="10%">排序</th>
                        <th class="text-center">功能</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
            <div class="text-end">
                <a href="javascript:void(0);" id="savebtn_sort" class="btn btn-primary text-nowrap btn-sm">儲存排序</a>
            </div>
            <%--<div id="saveMsg" class="col-md-auto order-0 order-md-1" style="color: red; display: none">儲存成功</div>--%>
        </div>

        <!-- Modal -->
        <input id="tmpid" type="hidden" value="" />
        <div class="modal fade" id="DocCategoryModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" role="dialog" data-bs-backdrop="static">
            <div class="modal-dialog modal-dialog-scrollable">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">程序書分類名稱維護</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="ochiform TitleLength05">
                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <label class="form-label" for="formA1">名稱</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control" id="dc_name" type="text">
                                </div>
                            </div>
                            <%--<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <label class="form-label" for="formA1">排序</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control num" id="dc_sort" type="text">
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">儲存</button>
                        <button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

