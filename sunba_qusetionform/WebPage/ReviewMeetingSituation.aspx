<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReviewMeetingSituation.aspx.cs" Inherits="WebPage_ReviewMeetingSituation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/ReviewMeetingSituation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<!--#include virtual="~/Webpage/DefaultMenu.html"-->

		<div class="py-2">
			<table id="tablist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap="">會議室名稱</th>
						<th class="text-center" nowrap="">使用日期</th>
						<th class="text-center" nowrap="">申請人姓名</th>
						<th class="text-center" nowrap="" width="50%">使用後狀況</th>
						<th class="text-center" nowrap="" width="100">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
</asp:Content>

