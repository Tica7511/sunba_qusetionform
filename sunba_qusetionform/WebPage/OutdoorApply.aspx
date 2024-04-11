<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OutdoorApply.aspx.cs" Inherits="WebPage_OutdoorApply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/OutdoorApply.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<form id="dataForm" name="dataForm">
			<div class="border border-dark p-2 mt-1">
				<div class="ochiform TitleLength08">
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">申請類別</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select id="o_type" name="o_type" class="form-select" aria-label="Default select example"></select>
								</div>
							</div>
						</div>
						<div class="col-md-6 ItemForA">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">送簽主管</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select id="SubSigner" name="SubSigner" class="form-select newstr" aria-label="Default select example"></select>
								</div>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow ItemForB" style="display: none;">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="o_passenger_number">共乘人數</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control newstr" id="o_passenger_number" name="o_passenger_number" type="number" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">共乘同仁</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select id="ddlPerson" name="ddlPerson" class="multiple-select  width100" multiple placeholder="請選擇參與人員"></select>
								</div>
							</div>
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="">使用日期與時間</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<div class="d-flex align-items-center flex-nowrap">
								<div class="flex-grow-1 me-1">
									<input class="form-control newstr width57 d-inline" type="date" id="o_startdate" name="o_startdate" />
									<select id="o_starthour" name="o_starthour" class="form-select width20 d-inline timeHour"></select>
									<select id="o_startmins" name="o_startmins" class="form-select width20 d-inline timeMin"></select>
								</div>
								<div class="text-center">~</div>
								<div class="flex-grow-1 ms-1">
									<input class="form-control newstr width57 d-inline" type="date" id="o_enddate" name="o_enddate" />
									<select id="o_endhour" name="o_endhour" class="form-select width20 d-inline timeHour"></select>
									<select id="o_endmins" name="o_endmins" class="form-select width20 d-inline timeMin"></select>
								</div>
								<div class="text-center px-1 ItemForB" style="display: none;">
									<button type="button" class="btn btn-primary text-nowrap btn-sm" id="CarSearchBtn">查詢</button>
								</div>
							</div>
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow ItemForB" style="display: none;">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">公務車使用狀況</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<div id="instructions" class="text-danger">輸入欲使用公務車的日期與時間，按下查詢即可取得可借用車輛資訊</div>
							<table class="table table-bordered table-striped" id="tablist" style="display: none;">
								<thead>
									<tr class="table-secondary">
										<th class="text-center" width="60">勾選</th>
										<th class="text-center" width="200">車號</th>
										<th class="text-center">使用狀況</th>
									</tr>
								</thead>
								<tbody></tbody>
							</table>
						</div>
					</div>


					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_place">地點</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_place" name="o_place" />
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_reason">事由</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_reason" name="o_reason" />
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_ps">備註</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_ps" name="o_ps" />
						</div>
					</div>
				</div>

				<div class="text-end mt-2">
					<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="savebtn">送出</a>
				</div>
			</div>
		</form>
		<div id="errMsg" style="color: red;"></div>
	</div>
</asp:Content>

