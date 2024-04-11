<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsVisitorRegister.aspx.cs" Inherits="WebPage_MealsVisitorRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsVisitorRegister.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-3">
			<div class="text-end my-2">
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="newbtn">新增訪客用餐</a>
			</div>
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">申請人</th>
						<th class="text-center">廠商/訪客名稱</th>
						<th class="text-center">事由</th>
						<th class="text-center">用餐時間</th>
						<th class="text-center">午餐份數</th>
						<th class="text-center">晚餐份數</th>
						<th class="text-center">審核</th>
						<th class="text-center">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="pageblock"></div>
		</div>
	</div>

	<input id="tmpid" type="hidden" class="newstr" value="" />
	<div class="modal fade" id="EditModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">訪客用餐明細</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<form id="dataForm" name="dataForm">
						<div class="ochiform TitleLength08">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">廠商/訪客名稱</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input id="mv_name" name="mv_name" class="form-control newstr" type="text" maxlength="50" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">事由</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input id="mv_reason" name="mv_reason" class="form-control newstr" type="text" maxlength="200" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">用餐時間</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input id="mv_date" name="mv_date" class="form-control newstr" type="date" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">午餐份數</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<div>
										葷
									<input id="mv_lunch_meat" name="mv_lunch_meat" type="text" class="inputex newstr num" size="2" />、奶蛋素
									<input id="mv_lunch_vegetarian" name="mv_lunch_vegetarian" type="text" class="inputex newstr num" size="2" />、全素
									<input id="mv_lunch_vegan" name="mv_lunch_vegan" type="text" class="inputex newstr num" size="2" />
									</div>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">晚餐份數</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<div>
										葷
									<input id="mv_dinner_meat" name="mv_dinner_meat" type="text" class="inputex newstr num" size="2" />、奶蛋素
									<input id="mv_dinner_vegetarian" name="mv_dinner_vegetarian" type="text" class="inputex newstr num" size="2" />、全素
									<input id="mv_dinner_vegan" name="mv_dinner_vegan" type="text" class="inputex newstr num" size="2" />
									</div>
								</div>
							</div>
						</div>
					</form>
					<div id="errMsg" style="color:red;"></div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					<button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">送出</button>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

