<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsCompanyRegister.aspx.cs" Inherits="WebPage_MealsCompanyRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsCompanyRegister.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="row align-items-center justify-content-between mt-3">
			<div class="col-md-auto order-1 order-md-0">
				<h4><span id="CompanyName"></span></h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" data-bs-toggle="modal" data-bs-target="#SettingModal">用餐批次設定</a>
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="savebtn">儲存用餐設定</a>
			</div>
		</div>

		<div class="mt-2">
			<form id="dataForm" name="dataForm">
				<table id="tablist" class="table table-bordered table-striped">
					<thead>
						<tr class="table-secondary">
							<th class="text-center">日期</th>
							<th class="text-center">是否用餐(午餐)</th>
							<th class="text-center">份數</th>
							<th class="text-center">地點</th>
							<th class="text-center">是否用餐(晚餐)</th>
							<th class="text-center">份數</th>
							<th class="text-center">地點</th>
						</tr>
					</thead>
					<tbody></tbody>
				</table>
			</form>
		</div>

		<!-- Modal -->
		<div class="modal fade" id="SettingModal" tabindex="-1" aria-labelledby="defaultsetting" aria-hidden="true">
			<div class="modal-dialog modal-lg modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="exampleModalLabel">用餐批次設定</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="ochiform TitleLength08">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">是否用餐(午餐)</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<%--<input class="form-check-input" type="radio" value="Y" name="setL" checked="checked" />
									<label class="form-check-label">是</label>
									<input class="form-check-input" type="radio" value="N" name="setL" />
									<label class="form-check-label">否</label>--%>
									預設份數:
									<div class="d-inline ms-1">
										<input type="text" class="inputex num" size="3" id="setL_num" value="1" />
									</div>
									<div class="d-inline ms-4">預設地點:</div>
									<div class="d-inline ms-1">
										<select class="inputex" id="setL_place" name="ddlplace"></select>
									</div>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end"></div>
								<div class="col-md-auto flex-grow-1">
									設定日期:
								<input type="date" class="inputex dateRange" id="setL_startdate" onkeydown="return false" />
									~
								<input type="date" class="inputex dateRange" id="setL_enddate" onkeydown="return false" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end"></div>
								<div class="col-md-auto flex-grow-1">
									排除日期:<select class="multiple-select  width100 ddlexclude" multiple="multiple" placeholder="請選擇時段" id="setL_exclude"></select>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">是否用餐(晚餐)</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<%--<input class="form-check-input" type="radio" value="Y" name="setD" checked="checked" />
									<label class="form-check-label">是</label>
									<input class="form-check-input" type="radio" value="N" name="setD" />
									<label class="form-check-label">否</label>--%>
									預設份數:
									<div class="d-inline ms-1">
										<input type="text" class="inputex num" size="3" id="setD_num" value="1" />
									</div>
									<div class="d-inline ms-4">預設地點:</div>
									<div class="d-inline ms-1">
										<select class="inputex" id="setD_place" name="ddlplace"></select>
									</div>
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end"></div>
								<div class="col-md-auto flex-grow-1">
									設定日期:
								<input type="date" class="inputex dateRange" id="setD_startdate" onkeydown="return false" />
									~
								<input type="date" class="inputex dateRange" id="setD_enddate" onkeydown="return false" />
								</div>
							</div>

							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end"></div>
								<div class="col-md-auto flex-grow-1">
									排除日期:<select class="multiple-select  width100 ddlexclude" multiple="multiple" placeholder="請選擇時段" id="setD_exclude"></select>
								</div>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="setbtn">套用批次用餐設定</button>
						<div>
							<span class="text-warning">按下"套用批次用餐設定"後會將未設定用餐的日期套用預設的設定，您可以再自行調整是否用餐。</span>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>

