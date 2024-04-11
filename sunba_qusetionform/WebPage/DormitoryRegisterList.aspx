<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryRegisterList.aspx.cs" Inherits="WebPage_DormitoryRegisterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryRegisterList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped font-size2">
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
						<th class="text-center" nowrap="">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>
</asp:Content>

