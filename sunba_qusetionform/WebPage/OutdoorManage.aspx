<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OutdoorManage.aspx.cs" Inherits="WebPage_OutdoorManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/OutdoorManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped font-size2">
				<thead>
					<tr class="table-secondary">
						<th class="text-center" nowrap=""><a href="javascript:void(0);" name="sortbtn" sortname="o_type">申請類別</a></th>
						<th class="text-center" nowrap="">申請日期</th>
						<th class="text-center" nowrap="">申請人姓名</th>
						<th class="text-center" nowrap="">公務車(車號)</th>
						<th class="text-center" nowrap="">地點</th>
						<th class="text-center" nowrap="">預計往返日期與時間</th>
						<th class="text-center" nowrap="">實際往返日期與時間</th>
						<th class="text-center" nowrap="">事由</th>
						<th class="text-center" nowrap="">審核</th>
						<th class="text-center" nowrap="">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
</asp:Content>

