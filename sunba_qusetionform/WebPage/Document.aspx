<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Document.aspx.cs" Inherits="WebPage_Document" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../page_js/Document.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-xxl mb-3">
        <div class="border border-dark p-2 mt-1">
            <div class="ochiform TitleLength08">
                <div class="row gy-2 mt-1 align-items-center OchiRow">
                    <div class="col-md-6">
                        <div class="row flex-md-nowrap align-items-center">
                            <div class="col-md-auto TitleSetWidth text-md-end">
                                <label class="form-label">文件標題關鍵字</label>
                            </div>
                            <div class="col-md-auto flex-grow-1">
                                <input class="form-control" id="SearchStr" type="text">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row flex-md-nowrap align-items-center">
                            <div class="col-md-auto TitleSetWidth text-md-end">
                                <label class="form-label">文件分類/階層</label>
                            </div>
                            <div class="col-md-auto flex-grow-1">
                                <select class="form-select" id="DocCategory"></select>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="text-end mt-2">
                <a href="javascript:void(0);" id="SearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
            </div>
        </div>

        <div class="row align-items-center justify-content-between mt-3">
            <div class="col-md-auto order-1 order-md-0">
                <h3 class="fw-bold">查詢結果</h3>
            </div>
            <div class="col-md-auto order-0 order-md-1">
                <a href="javascript:void(0);" id="addbtn" class="btn btn-primary text-nowrap btn-sm docShow" style="display:none;">新增</a>
            </div>
        </div>

        <div class="mt-1 table-responsive">
            <input id="tmporderby" type="hidden" value="" />
            <input id="tmpsortby" type="hidden" value="" />
            <table id="tablist" class="table table-bordered table-striped font-size2">
                <thead>
                    <tr class="table-secondary">
                        <th class="text-center" nowrap="">發行日期</th>
                        <th class="text-center" nowrap="">文件分類/階層 <a href="javascript:void(0);" id="d_categoryAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_categoryDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">文件/表單編號 <a href="javascript:void(0);" id="d_noAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_noDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center">文件/表單名稱</th>
                        <th class="text-center" nowrap="">版本</th>
                        <th class="text-center" nowrap="">附件</th>
                        <th class="text-center docShow" nowrap="" style="display:none;">功能</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
            <div id="pageblock"></div>
        </div>
    </div>

    <!-- Modal -->
    <input id="tmpgid" type="hidden" value="" class="newstr" />
    <input id="tmpfile_id" type="hidden" value="" class="newstr" />
    <div class="modal fade" id="DocumentModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">文件管理</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <form id="documentData" name="documentData">
                    <div class="modal-body">
                        <div class="ochiform TitleLength07">
                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <span class="text-danger">*</span>
                                    <label class="form-label">發行日期</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control newstr" type="date" id="d_pubdate" value="">
                                </div>
                            </div>

                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <label class="form-label">文件分類/階層</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <select class="form-select newstr" id="d_category"></select>
                                </div>
                            </div>

                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <span class="text-danger">*</span>
                                    <label class="form-label">文件/表單編號</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control newstr" type="text" id="d_no" value="">
                                </div>
                            </div>

                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <span class="text-danger">*</span>
                                    <label class="form-label">文件/表單名稱</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control newstr" type="text" id="d_name" value="">
                                </div>
                            </div>

                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <label class="form-label">檔案上傳</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input type="file" class="form-control newstr" id="d_file" name="d_file" multiple="multiple">
                                    <div class="mt-1" id="FileList"></div>
                                </div>
                            </div>

                            <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                                <div class="col-md-auto TitleSetWidth text-md-end">
                                    <label class="form-label">版本</label>
                                </div>
                                <div class="col-md-auto flex-grow-1">
                                    <input class="form-control newstr" type="text" id="d_version" value="">
                                </div>
                            </div>

                            <%--<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                            <div class="col-md-auto TitleSetWidth text-md-end">
                                <label class="form-label">權責部門</label>
                            </div>
                            <div class="col-md-auto flex-grow-1">
                                <select class="form-control newstr" id="d_dept">
                                    <option>請選擇</option>
                                </select>
                            </div>
                        </div>

                        <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
                            <div class="col-md-auto TitleSetWidth text-md-end">
                                <label class="form-label">核定權責主管</label>
                            </div>
                            <div class="col-md-auto flex-grow-1">
                                <input class="form-control newstr" type="text" id="d_manager" value="">
                            </div>
                        </div>--%>
                        </div>
                    </div>
                </form>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">送出</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

