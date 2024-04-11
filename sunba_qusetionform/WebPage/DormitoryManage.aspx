<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryManage.aspx.cs" Inherits="WebPage_DormitoryManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="row align-items-center justify-content-between my-2">
			<div class="col-md-auto order-1 order-md-0">
				<select id="SearchArea" class="inputex"></select>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				工號:<input type="text" class="inputex" size="10" id="SearchStr">
				<a href="javascript:void(0);" id="SearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
				<a href="javascript:void(0);" id="ExportBtn" class="btn btn-primary text-nowrap btn-sm">匯出</a>
			</div>
		</div>

		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap="">宿舍房號</th>
						<th class="text-center" nowrap="">宿舍分機</th>
						<th class="text-center" nowrap="">房型</th>
						<th class="text-center" nowrap="">類型</th>
						<th class="text-center" nowrap="">備註</th>
						<th class="text-center" nowrap="">住宿人員</th>
						<th class="text-center" nowrap="">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
	
	<input type="hidden" id="tmpgid" value="" />
	<div class="modal fade" id="CheckInSettingModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true" role="dialog" data-bs-backdrop="static">
		<div class="modal-dialog modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="cancelmealLabel">房間設定</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="ochiform TitleLength04">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">廠區</label>
							</div>
							<div id="Area" class="col-md-auto flex-grow-1"></div>
						</div>
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">房號</label>
							</div>
							<div id="RoomNo" class="col-md-auto flex-grow-1"></div>
						</div>
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">分機</label>
							</div>
							<div id="Ext" class="col-md-auto flex-grow-1"></div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">房型</label>
							</div>
							<div id="RoomType" class="col-md-auto flex-grow-1"></div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">類型</label>
							</div>
							<div id="Category" class="col-md-auto flex-grow-1"></div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">備註</label>
							</div>
							<div id="Ps" class="col-md-auto flex-grow-1"></div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">住宿人員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="ddlPerson" class="multiple-select  width100" multiple="multiple" placeholder="請選擇住宿人員"></select>
							</div>
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					<button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">送出</button>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

