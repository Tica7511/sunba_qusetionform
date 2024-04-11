<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OutdoorFormDetail.aspx.cs" Inherits="WebPage_OutdoorFormDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript" src="../page_js/OutdoorFormDetail.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="container-xxl mb-3">
		<form id="dataForm" name="dataForm">
			<div class="border border-dark p-2 mt-1">
				<div class="ochiform TitleLength08">
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="lokidate">申請日期</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" id="o_applydate" type="date" disabled="disabled" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">申請類別</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<div class="selectSetTypeA">
										<select id="o_type" name="o_type" class="form-select" aria-label="Default select example" disabled="disabled"></select>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow ItemForB" style="display:none;">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="o_passenger_number">共乘人數</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control newstr" id="o_passenger_number" name="o_passenger_number" type="number" disabled="disabled" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">共乘同仁</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select id="ddlPerson" name="ddlPerson" class="multiple-select  width100" multiple placeholder="請選擇參與人員" disabled="disabled"></select>
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
									<input class="form-control newstr width57 d-inline" type="date" id="o_startdate" name="o_startdate" disabled="disabled" />
									<select id="o_starthour" name="o_starthour" class="form-select width20 d-inline timeHour" disabled="disabled"></select>
									<select id="o_startmins" name="o_startmins" class="form-select width20 d-inline timeMin" disabled="disabled"></select>
								</div>
								<div class="text-center">~</div>
								<div class="flex-grow-1 ms-1">
									<input class="form-control newstr width57 d-inline" type="date" id="o_enddate" name="o_enddate" disabled="disabled" />
									<select id="o_endhour" name="o_endhour" class="form-select width20 d-inline timeHour" disabled="disabled"></select>
									<select id="o_endmins" name="o_endmins" class="form-select width20 d-inline timeMin" disabled="disabled"></select>
								</div>
							</div>
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow ItemForB" style="display:none;">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">公務車車號</label>
						</div>
						<div class="col-md-auto flex-grow-1" id="CarNo"></div>
					</div>


					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_place">地點</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_place" name="o_place" disabled="disabled" />
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_reason">事由</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_reason" name="o_reason" disabled="disabled" />
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="o_ps">備註</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" type="text" id="o_ps" name="o_ps" disabled="disabled" />
						</div>
					</div>
				</div>
			</div>
		</form>
		<div id="errMsg" style="color: red;"></div>
	</div>
</asp:Content>

