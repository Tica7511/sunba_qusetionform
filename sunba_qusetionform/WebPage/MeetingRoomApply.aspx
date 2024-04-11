<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MeetingRoomApply.aspx.cs" Inherits="WebPage_MeetingRoomApply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MeetingRoomApply.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<input id="sTime" type="hidden" value="<%=sTime %>" />
	<input id="eTime" type="hidden" value="<%=eTime %>" />
	<div class="container-xxl mb-3">
		<form id="dataForm" name="dataForm">
			<div class="border border-dark p-2 mt-1">
				<div class="ochiform TitleLength08">
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">使用場所</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input id="tmpRoomid" type="hidden" value="<%= MeetingRoom %>" />
									<select id="m_room" name="m_room" class="form-select newstr" aria-label="Default select example"></select>
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">申請類型</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<div class="selectSetTypeA">
										<select id="ApplyCategory" name="ApplyCategory" class="form-select" aria-label="Default select example">
											<option value="01">一般會議</option>
											<option value="02">週期性會議</option>
										</select>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="ItemForA">
						<div class="row gy-2 mt-1 align-items-center OchiRow">
							<div class="col-md-6">
								<div class="row flex-md-nowrap align-items-center">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label" for="single_date">使用日期</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input class="form-control newstr" id="m_date" name="m_date" type="date" value="<%= MeetingDate %>">
									</div>
								</div>
							</div>
							<div class="col-md-6">
								<div class="row flex-md-nowrap align-items-center">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">使用時段</label></div>
									<div class="col-md-auto flex-grow-1">
										<div class="d-flex align-items-center flex-nowrap">
											<div class="flex-grow-1 me-1">
												<select id="m_starthour" name="m_starthour" class="form-select width45 d-inline timeHour"></select>
												<select id="m_startmins" name="m_startmins" class="form-select width45 d-inline timeMin"></select>
											</div>
											<div class="text-center">~</div>
											<div class="flex-grow-1 ms-1">
												<select id="m_endhour" name="m_endhour" class="form-select width45 d-inline timeHour"></select>
												<select id="m_endmins" name="m_endmins" class="form-select width45 d-inline timeMin"></select>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="ItemForB" style="display: none;">
						<div class="row gy-2 mt-1 align-items-center OchiRow">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">週期設定</div>
								<div class="col-md-auto flex-grow-1">
									<div class="d-flex align-items-center flex-nowrap">
										<div class="flex-grow-1 me-1">
											<input class="form-control newstr" id="cycle_sdate" name="cycle_sdate" type="date">
										</div>
										<div class="text-center">~</div>
										<div class="flex-grow-1 ms-1">
											<input class="form-control newstr" id="cycle_edate" name="cycle_edate" type="date">
										</div>
									</div>
								</div>
							</div>

							<div class="col-md-6">
								<div class="row flex-md-nowrap align-items-center">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">星期</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<select id="week" name="week" class="form-select newstr" aria-label="Default select example">
											<option value="">請選擇</option>
											<option value="1">一</option>
											<option value="2">二</option>
											<option value="3">三</option>
											<option value="4">四</option>
											<option value="5">五</option>
											<option value="6">六</option>
											<option value="7">日</option>
										</select>
									</div>
								</div>
							</div>

							<div class="col-md-6">
								<div class="row flex-md-nowrap align-items-center">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">使用時段</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<div class="d-flex align-items-center flex-nowrap">
											<div class="flex-grow-1 me-1">
												<select id="cycle_shour" name="cycle_shour" class="form-select width45 d-inline timeHour"></select>
												<select id="cycle_smins" name="cycle_smins" class="form-select width45 d-inline timeMin"></select>
											</div>
											<div class="text-center">~</div>
											<div class="flex-grow-1 ms-1">
												<select id="cycle_ehour" name="cycle_ehour" class="form-select width45 d-inline timeHour"></select>
												<select id="cycle_emins" name="cycle_emins" class="form-select width45 d-inline timeMin"></select>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="m_desc">申請用途</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" id="m_desc" name="m_desc" type="text">
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="m_ps">備註</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control newstr" id="m_ps" name="m_ps" type="text">
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">參與人員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="ddlPerson" name="ddlPerson" class="multiple-select  width100" multiple="multiple" placeholder="請選擇參與人員"></select>
							</div>
						</div>
					</div>
				</div>

				<div class="text-end mt-2">
					<a href="javascript:void(0);" id="savebtn" class="btn btn-primary text-nowrap btn-sm">送出</a>
				</div>
			</div>
		</form>
		<div id="errMsg" style="color: red;"></div>
	</div>
</asp:Content>

