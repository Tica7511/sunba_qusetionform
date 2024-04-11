<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryRoomManage.aspx.cs" Inherits="WebPage_DormitoryRoomManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryRoomManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="row align-items-center justify-content-between my-2">
			<div class="col-md-auto order-1 order-md-0">
				<select class="inputex" id="SearchArea"></select>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<a id="newbtn" href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm">新增</a>
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
						<th class="text-center" nowrap="">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>

	<input type="hidden" class="newstr" id="tmpid" value="" />
	<div class="modal fade" id="RoomSettingModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true" role="dialog" data-bs-backdrop="static">
		<div class="modal-dialog modal-dialog-scrollable">
			<form id="dataForm">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">房間設定</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="ochiform TitleLength04">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">廠區</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select class="form-select" aria-label="Default select example" id="dr_area" name="dr_area"></select>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">房號</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control newstr" type="text" id="dr_no" name="dr_no" />
								</div>
							</div>
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">分機</label></div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control newstr" type="text" id="dr_ext" name="dr_ext" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">房型</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select class="form-select" aria-label="Default select example" id="dr_roomtype" name="dr_roomtype"></select>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">類型</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select class="form-select" aria-label="Default select example" id="dr_category" name="dr_category"></select>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">備註</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control newstr" type="text" id="dr_ps" name="dr_ps" />
								</div>
							</div>
						</div>
					</div>

					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">送出</button>
					</div>
				</div>
			</form>
		</div>
	</div>
</asp:Content>

