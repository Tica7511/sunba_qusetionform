<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReviewMeals.aspx.cs" Inherits="WebPage_ReviewMeals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/ReviewMeals.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<!--#include virtual="~/Webpage/DefaultMenu.html"-->

		<div class="py-2">
			<div class="row align-items-center justify-content-between mt-3">
				<div class="col-md-auto order-1 order-md-0">
					<h4 class="text-primary">訪客用餐申請</h4>
				</div>
				<div class="col-md-auto order-0 order-md-1">
				</div>
			</div>

			<table id="visitorlist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">申請人</th>
						<th class="text-center">廠商/訪客名稱</th>
						<th class="text-center">事由</th>
						<th class="text-center">用餐時間</th>
						<th class="text-center">午餐份數</th>
						<th class="text-center">晚餐份數</th>
						<th class="text-center">行管部</th>
						<th class="text-center" width="100">審核</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="visitor_pageblock"></div>


			<div class="row align-items-center justify-content-between mt-3">
				<div class="col-md-auto order-1 order-md-0">
					<h4 class="text-primary">當日取消用餐</h4>
				</div>
				<div class="col-md-auto order-0 order-md-1">
				</div>
			</div>

			<table id="cancellist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">申請人</th>
						<th class="text-center">事由</th>
						<th class="text-center">用餐項目</th>
						<th class="text-center" style="width:150px;">行管部</th>
						<th class="text-center" style="width:100px;">審核</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="cancel_pageblock"></div>
		</div>
	</div>
</asp:Content>

