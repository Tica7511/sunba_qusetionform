<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="WebPage_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../tinymce/tinymce.min.js"></script>
    <script type="text/javascript" src="../page_js/index.js"></script>
	<style>
		.modal-dialog{
		    overflow-y: initial !important
		}
		.modal-body{
		    height: 600px;
		    overflow-y: auto;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" id="Mitem" />
    <input type="hidden" id="Mnum" />
    <input type="hidden" id="MquestionType" />
    <input type="hidden" id="Mempid" />
    <input type="hidden" id="Mfillformname" />
    <input type="hidden" id="Mcompanylist" />
    <input type="hidden" id="Morgnization" />
    <input type="hidden" id="Mstartday" />
    <input type="hidden" id="Mendday" />
    <input type="hidden" id="Mstate" />
    <input type="hidden" id="Mtype" />
    <input type="hidden" id="Mcontent" />
    <input type="hidden" id="Mreplycontent" />
    <input type="hidden" id="Fguid" />
    <input type="hidden" id="ffGuid" />
    <input type="hidden" id="Fsn" />
    <input type="hidden" id="Qguid" />
    <input type="hidden" id="PQguid" />
    <input type="hidden" id="OrderbyInfo" />
    <input type="hidden" id="OrderbyType" />
    <div class="container-xxl mb-3">
        <div class="border border-dark p-2 mt-1">
            <div class="ochiform TitleLength08">
                <div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">項次</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control newstr" id="txt_item" type="text">
							</div>
						</div>
					</div>
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">編號</label>
							</div>
							<div class="col-md-auto flex-grow-1">
                                <input class="form-control newstr" id="txt_num" type="text">
							</div>
						</div>
					</div>
				</div>
                <div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">問題類別</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<div class="selectSetTypeA">
									<select id="sel_questionType" class="form-select" aria-label="Default select example">
                                        <option value=""> -- 請選擇 -- </option>
									</select>
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">部門</label>
							</div>
							<div class="col-md-auto flex-grow-1">
                                <div class="selectSetTypeA">
									<select id="sel_orgnization" class="form-select" aria-label="Default select example">
                                        <option value=""> -- 請選擇 -- </option>
									</select>
								</div>
							</div>
						</div>
					</div>
				</div>
                <div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">員工編號</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<div class="selectSetTypeA">
									<select id="sel_empid" class="form-select" aria-label="Default select example">
                                        <option value=""> -- 請選擇 -- </option>
									</select>
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">提出日期</label>
							</div>
							<div class="col-md-auto flex-grow-1">
                                <div class="d-flex align-items-center flex-nowrap">
									<div class="flex-grow-1 me-1">
										<input class="form-control newstr width100" id="txt_startday" type="date">
									</div>
									<div class="text-center">~</div>
									<div class="flex-grow-1 ms-1">
										<input class="form-control newstr width100" id="txt_endday" type="date">
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
                <div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">急迫性</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<div id="div_type">
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">內容</label>
							</div>
							<div class="col-md-auto flex-grow-1">
                                <div class="selectSetTypeA">
									<input class="form-control newstr width100" placeholder="可輸入關鍵字" id="txt_content" type="text">
								</div>
							</div>
						</div>
					</div>
				</div>
            </div>
            <div class="text-end mt-2">
                總資料筆數: <span id="sp_count"></span>&nbsp;&nbsp;
                <a href="javascript:void(0);" id="clearbtn" class="btn btn-primary text-nowrap btn-sm">清除</a>
                <a href="javascript:void(0);" id="querybtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
            </div>
        </div>

        <div class="row align-items-center justify-content-between mt-3">
            <div class="col-md-auto order-1 order-md-0">
                <h3 class="fw-bold">查詢結果</h3>
            </div>
            <div class="col-md-auto order-0 order-md-1">
                <a href="javascript:void(0);" id="newbtn" class="btn btn-primary text-nowrap btn-sm docShow">新增</a>
            </div>
        </div>

        <div class="mt-1 table-responsive">
            <input id="tmporderby" type="hidden" value="" />
            <input id="tmpsortby" type="hidden" value="" />
            <table id="tablist" class="table table-bordered table-striped font-size2">
                <thead>
                    <tr class="table-secondary">
                        <th class="text-center" nowrap="">項次</th>
                        <th class="text-center" nowrap="">編號 <a href="javascript:void(0);" id="d_numAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_numDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
						<th class="text-center" width="15%" nowrap="">內容</th>
                        <th class="text-center" nowrap="">回覆內容</th>
                        <th class="text-center" nowrap="">問題類別 <a href="javascript:void(0);" id="d_questionTypeAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_questionTypeDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">員工編號 <a href="javascript:void(0);" id="d_empidAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_empidDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">填表人 <a href="javascript:void(0);" id="d_empNameAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_empNameDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">部門 <a href="javascript:void(0);" id="d_orgnizationAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_orgnizationDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">提出日期 <a href="javascript:void(0);" id="d_purposeDateAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_purposeDateDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
                        <th class="text-center" nowrap="">急迫性 <a href="javascript:void(0);" id="d_urgentAsc" class="text-dark" data-bs-toggle="tooltip" data-bs-placement="top" title="排序AtoZ"><i class="fa-solid fa-arrow-down-short-wide"></i></a><a href="javascript:void(0);" id="d_urgentDesc" data-bs-toggle="tooltip" data-bs-placement="top" title="排序ZtoA"><i class="fa-solid fa-arrow-down-wide-short"></i></a></th>
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
    <div class="modal fade" id="CommentModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">提問單內容</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <form id="commentData" name="commentData">
                    <div class="modal-body">
						<div class="ochiform TitleLength07">
						    <div class="row gy-2 mt-1 align-items-center OchiRow">
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">編號</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						    				<input class="form-control newstr" id="ntxt_num" type="text">
						    			</div>
						    		</div>
						    	</div>
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">問題類型</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						                    <div class="selectSetTypeA">
								        	    <select id="nsel_questionType" class="form-select newstr" aria-label="Default select example">
						                            <option value=""> -- 請選擇 -- </option>
								        	    </select>
								            </div>
						    			</div>
						    		</div>
						    	</div>
						    </div>

						    <div class="row gy-2 mt-1 align-items-center OchiRow">
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">部門</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						    				<input class="form-control" id="nsel_orgnization" type="text" disabled>
						    			</div>
						    		</div>
						    	</div>
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">填表人</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						                    <input class="form-control" id="ntxt_fillformname" type="text" disabled>
						    			</div>
						    		</div>
						    	</div>
						    </div>

						    <div class="row gy-2 mt-1 align-items-center OchiRow">
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">提出日期</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						    				<input class="form-control newstr" id="ntxt_day" type="date">
						    			</div>
						    		</div>
						    	</div>
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">急迫性</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						                    <div class="selectSetTypeA">
								        	    <select id="nsel_type" class="form-select newstr" aria-label="Default select example">
						                            <option value=""> -- 請選擇 -- </option>
								        	    </select>
								            </div>
						    			</div>
						    		</div>
						    	</div>
						    </div>

						    <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						        <div class="col-md-auto TitleSetWidth text-md-end">
						            <label class="form-label">檔案上傳</label>
						        </div>
								<div class="col-md-auto flex-grow-1">
						            <input type="file" class="form-control newstr" id="ntxt_file" name="ntxt_file" multiple="multiple">
						            <div class="mt-1" id="FileList"></div>
									<table id="tablistfile" class="table table-bordered table-striped">
										<thead>
											<tr class="table-secondary">
												<th class="text-center">文件名稱</th>
												<th width="25%" class="text-center">上傳日期</th>
												<th id="th_file" width="10%" class="text-center">功能</th>
											</tr>
										</thead>
										<tbody></tbody>
									</table>
						        </div>
						    </div>
						</div><br />
						<div id="div_commenttinymce" style="margin:auto;">
						    <textarea id="n_suggestion" rows="20" cols="5"></textarea>
						</div>
						<div id="div_commentdiv" class="container-xxl mb-3">
							<div id="div_suggestion" class="border border-dark p-2 mt-1">

							</div>
						</div>
                    </div>
                </form>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" id="n_subbtn">送出</button>
                </div>
            </div>
        </div>
    </div>

	<div class="modal fade" id="ReplyModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">回覆內容</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <form id="ReplyData" name="commentData">
                    <div class="modal-body">
						<div class="ochiform TitleLength07">
						    <div class="row gy-2 mt-1 align-items-center OchiRow">
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">回覆日期</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						    				<input class="form-control replystr" id="ntxt_returnday" type="date" disabled>
						    			</div>
						    		</div>
						    	</div>
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">預計完成日</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						    				<input class="form-control replystr" id="ntxt_finishday" type="date">
						    			</div>
						    		</div>
						    	</div>
						    </div>

						    <div class="row gy-2 mt-1 align-items-center OchiRow">
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">目前狀態</label>
						    			</div>
						    			<div class="col-md-auto flex-grow-1">
						                    <div class="selectSetTypeA">
								        	    <select id="nsel_state" class="form-select replystr" aria-label="Default select example">
						                            <option value=""> -- 請選擇 -- </option>
								        	    </select>
								            </div>
						    			</div>
						    		</div>
						    	</div>
						    	<div class="col-md-6">
						    		<div class="row flex-md-nowrap align-items-center">
						    			<div class="col-md-auto TitleSetWidth text-md-end">
						    				<label class="form-label">需求是否在第一期合約中</label>
						    			</div>
						    			<div id="div_contract" class="col-md-auto flex-grow-1">
											<input id="ck_contract" value="Y" type="checkbox" name="ckcontract" /> 是
						    			</div>
						    		</div>
						    	</div>
						    </div>

						    <div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						        <div class="col-md-auto TitleSetWidth text-md-end">
						            <label class="form-label">檔案上傳</label>
						        </div>
						        <div class="col-md-auto flex-grow-1">
						            <input type="file" class="form-control newstr" id="ntxt_file2" name="ntxt_file2" multiple="multiple">
						            <div class="mt-1" id="FileList2"></div>
									<table id="tablistfile2" class="table table-bordered table-striped">
										<thead>
											<tr class="table-secondary">
												<th class="text-center">文件名稱</th>
												<th width="25%" class="text-center">上傳日期</th>
												<th id="th_file2" width="10%" class="text-center">功能</th>
											</tr>
										</thead>
										<tbody></tbody>
									</table>
						        </div>
						    </div>
						</div><br />
						<div id="div_replytinymce" style="margin:auto;">
						    <textarea id="n_replies" rows="20" cols="5"></textarea>
						</div>
						<div id="div_replydiv" class="container-xxl mb-3">
							<div id="div_reply" class="border border-dark p-2 mt-1">

							</div>
						</div>
                    </div>
                </form>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
                    <button type="button" class="btn btn-primary text-nowrap btn-sm" id="n_subbtn2">送出</button>
                </div>
            </div>
        </div>
    </div>

	<%--<div class="modal fade" id="FileModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-lg modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">附件</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="row align-items-center justify-content-between mt-3">
						    <div class="col-md-auto order-1 order-md-0">
						        <h5 class="fw-bold">填表附件</h5>
						    </div>
						</div>
						<table id="tablistfile" class="table table-bordered table-striped">
							<thead>
								<tr class="table-secondary">
									<th class="text-center">文件名稱</th>
									<th class="text-center">上傳日期</th>
									<th class="text-center">功能</th>
								</tr>
							</thead>
							<tbody></tbody>
						</table>
						<div id="pageblockCommentFile"></div>
						<div class="row align-items-center justify-content-between mt-3">
						    <div class="col-md-auto order-1 order-md-0">
						        <h5 class="fw-bold">回覆附件</h5>
						    </div>
						</div>
						<table id="tablistfile2" class="table table-bordered table-striped">
							<thead>
								<tr class="table-secondary">
									<th class="text-center">文件名稱</th>
									<th class="text-center">上傳日期</th>
									<th class="text-center">功能</th>
								</tr>
							</thead>
							<tbody></tbody>
						</table>
						<div id="pageblockReplyFile"></div>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>--%>
</asp:Content>



