<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsStatistics.aspx.cs" Inherits="WebPage_MealsStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsStatistics.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="row align-items-center justify-content-between mt-3">
			<div class="col-md-auto order-1 order-md-0">
				<select class="inputex" id="ddlYear"></select>
				<select class="inputex" id="ddlMonth"></select>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<a href="<%= ResolveUrl("~/handler/ExportMealsStatistics.aspx") %>" target="_blank" class="btn btn-primary text-nowrap btn-sm">匯出本週用餐統計</a>
				<a href="<%= ResolveUrl("~/handler/ExportMealsPersonList.aspx") %>" target="_blank" class="btn btn-primary text-nowrap btn-sm">匯出當日用餐名單</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" rowspan="2">日期</th>
						<th class="text-center" rowspan="2">地點</th>
						<th class="text-center" colspan="4">午餐份數</th>
						<th class="text-center" rowspan="2">合計</th>
						<th class="text-center" colspan="4">晚餐份數</th>
						<th class="text-center" rowspan="2">合計</th>
					</tr>
					<tr class="table-secondary">
						<th class="text-center">同仁</th>
						<th class="text-center">廠商</th>
						<th class="text-center">愛心</th>
						<th class="text-center">訪客(葷/奶蛋素/全素)</th>
						<th class="text-center">同仁</th>
						<th class="text-center">廠商</th>
						<th class="text-center">愛心</th>
						<th class="text-center">訪客(葷/奶蛋素/全素)</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
</asp:Content>

