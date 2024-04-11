<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsFee.aspx.cs" Inherits="WebPage_MealsFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsFee.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">同仁</th>
						<th class="text-center">廠商</th>
						<th class="text-center">訪客</th>
						<th class="text-center">愛心</th>
						<th class="text-center">生效日</th>
						<th class="text-center">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>

		<!-- Modal -->
		<input id="tmpid" type="hidden" value="" />
		<div class="modal fade" id="UpdateModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="exampleModalLabel">餐費管理</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="ochiform TitleLength05">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="mf_employee">同仁</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" id="mf_employee" type="text" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="mf_firm">廠商</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" id="mf_firm" type="text" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="mf_visitor">訪客</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" id="mf_visitor" type="text" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="mf_love">愛心</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" id="mf_love" type="text" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="mf_effectivedate">日期</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" id="mf_effectivedate" type="date" />
								</div>
							</div>
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

