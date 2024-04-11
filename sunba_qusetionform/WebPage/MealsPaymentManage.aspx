<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsPaymentManage.aspx.cs" Inherits="WebPage_MealsPaymentManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsPaymentManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<!--餐費統計總表-->
		<div class="row align-items-center justify-content-between mt-3">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">餐費統計總表</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<select id="ddlYear" class="inputex"></select>
				<select id="ddlMonth" class="inputex"></select>
			</div>
		</div>

		<div class="mt-2">
			<table id="totallist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">同仁</th>
						<th class="text-center">外部廠商</th>
						<th class="text-center">愛心便當</th>
						<th class="text-center">訪客</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>

		<!--同仁餐費統計表-->
		<div class="row align-items-center justify-content-between mt-5">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">同仁餐費統計表</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				工號:<input type="text" id="EmployeeSearchStr" class="inputex" size="10">
				<a href="javascript:void(0);" id="EmployeeSearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
				<a href="javascript:void(0);" id="EmployeeExportBtn" class="btn btn-primary text-nowrap btn-sm">匯出</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="employeelist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">工號</th>
						<th class="text-center">單位</th>
						<th class="text-center">姓名</th>
						<th class="text-center">餐費</th>
						<th class="text-center">用餐明細</th>
						<th class="text-center">工號</th>
						<th class="text-center">單位</th>
						<th class="text-center">姓名</th>
						<th class="text-center">餐費</th>
						<th class="text-center">用餐明細</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="employee_pageblock"></div>
		</div>

		<!--廠商餐費統計表-->
		<div class="row align-items-center justify-content-between mt-5">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">廠商餐費統計表</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				廠商名稱:<input type="text" id="CompanySearchStr" class="inputex" size="10">
				<a href="javascript:void(0);" id="CompanySearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
				<a href="javascript:void(0);" id="CompanyExportBtn" class="btn btn-primary text-nowrap btn-sm">匯出</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="companylist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">廠商名稱</th>
						<th class="text-center">類別</th>
						<th class="text-center">餐費</th>
						<th class="text-center">用餐明細</th>
						<th class="text-center">廠商名稱</th>
						<th class="text-center">類別</th>
						<th class="text-center">餐費</th>
						<th class="text-center">用餐明細</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="company_pageblock"></div>
		</div>

		<!--訪客餐費統計表-->
		<div class="row align-items-center justify-content-between mt-5">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">訪客餐費統計表</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				申請人:<input type="text" id="VisitorSearchStr" class="inputex" size="10">
				<a href="javascript:void(0);" id="VisitorSearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
				<a href="javascript:void(0);" id="VisitorExportBtn" class="btn btn-primary text-nowrap btn-sm">匯出</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="visitlist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">日期</th>
						<th class="text-center">申請人</th>
						<th class="text-center">午餐份數</th>
						<th class="text-center">晚餐份數</th>
						<th class="text-center">餐費</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="visit_pageblock"></div>
		</div>
	</div>

	<!-- Modal -->
	<input type="hidden" id="tmpid" value="" />
	<div class="modal fade" id="DetailModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-lg modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">用餐明細</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<table id="detaillist" class="table table-bordered table-striped">
						<thead>
							<tr class="table-secondary">
								<th class="text-center">日期</th>
								<th class="text-center">午餐</th>
								<th class="text-center">份數</th>
								<th class="text-center">地點</th>
								<th class="text-center">晚餐</th>
								<th class="text-center">份數</th>
								<th class="text-center">地點</th>
							</tr>
						</thead>
						<tbody></tbody>
					</table>
			<div id="pageblock"></div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

