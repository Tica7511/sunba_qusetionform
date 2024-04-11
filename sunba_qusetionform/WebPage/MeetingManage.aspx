<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MeetingManage.aspx.cs" Inherits="WebPage_MeetingManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MeetingManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap="">申請日期</th>
						<th class="text-center" nowrap="">申請人姓名</th>
						<th class="text-center" nowrap="">使用日期</th>
						<th class="text-center" nowrap="">使用時段</th>
						<th class="text-center" nowrap="">使用場所</th>
						<th class="text-center" nowrap="">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
</asp:Content>

