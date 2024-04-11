<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReviewDormitory.aspx.cs" Inherits="WebPage_ReviewDormitory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/ReviewDormitory.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<!--#include virtual="~/Webpage/DefaultMenu.html"-->
		<div class="py-2">
			<div class="row align-items-center justify-content-between mt-3">
				<div class="col-md-auto order-1 order-md-0">
					<h4 class="text-primary">宿舍申請</h4>
				</div>
				<div class="col-md-auto order-0 order-md-1">
				</div>
			</div>
			<table id="applylist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap="">申請日期</th>
						<th class="text-center" nowrap="">申請人姓名</th>
						<th class="text-center" nowrap="">申請者單位</th>
						<th class="text-center" nowrap="">申請類別</th>
						<th class="text-center" nowrap="">宿舍承辦</th>
						<th class="text-center" nowrap="">宿舍承辦主管</th>
						<th class="text-center" nowrap="">行管部總務主管</th>
						<th class="text-center" nowrap="">行管部主管</th>
						<th class="text-center" nowrap="" width="150">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="apply_pageblock"></div>


			<div class="row align-items-center justify-content-between mt-3">
				<div class="col-md-auto order-1 order-md-0">
					<h4 class="text-primary">退宿申請</h4>
				</div>
				<div class="col-md-auto order-0 order-md-1">
				</div>
			</div>
			<table id="cancellist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap="">退宿日期</th>
						<th class="text-center" nowrap="">申請人姓名</th>
						<th class="text-center" nowrap="">使用宿舍</th>
						<th class="text-center" nowrap="">申請類別</th>
						<th class="text-center" nowrap="" width="150">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="cancel_pageblock"></div>
		</div>
	</div>
</asp:Content>

