<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsEmployeeFee.aspx.cs" Inherits="WebPage_MealsEmployeeFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsEmployeeFee.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-3">
			年份:<select class="inputex" id="ddlYear"></select>
			<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" onclick="getFeeList()">查詢</a>
		</div>

		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">月份</th>
						<th class="text-center">份數</th>
						<th class="text-center">餐費</th>
						<th class="text-center">明細</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>

		<!-- Modal -->
		<input id="tmpYear" type="hidden" />
		<input id="tmpMonth" type="hidden" />
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
							<tbody>
								<tr>
									<td class="text-center">2023/06/19</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/20</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/21</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/22</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/23</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/24</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
								<tr>
									<td class="text-center">2023/06/25</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
									<td class="text-center">是
									</td>
									<td class="text-center">1</td>
									<td class="text-center">員工餐廳</td>
								</tr>
							</tbody>
						</table>
						<div id="pageblock"></div>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

