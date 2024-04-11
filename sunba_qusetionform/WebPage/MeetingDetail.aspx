<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MeetingDetail.aspx.cs" Inherits="WebPage_MeetingDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MeetingDetail.js"></script>
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
									<label class="form-label">使用場所</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select id="m_room" name="m_room" class="form-select" aria-label="Default select example"></select>
								</div>
							</div>
						</div>
						<div class="col-md-6"></div>
					</div>


					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="carnum">使用日期</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" id="m_date" name="m_date" type="date" />
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

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="m_desc">申請用途</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control" id="m_desc" name="m_desc" type="text" />
						</div>
					</div>
					
					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="m_ps">備註</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control" id="m_ps" name="m_ps" type="text" />
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">參與人員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="ddlPerson" name="ddlPerson" class="multiple-select  width100" multiple placeholder="請選擇參與人員"></select>
							</div>
						</div>
					</div>
				</div>

				<div class="text-end mt-2">
					<a href="javascript:void(0);" class="btn btn-danger text-nowrap btn-sm" id="cancelbtn">取消預約</a>
					<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="savebtn">儲存</a>
				</div>
			</div>
		</form>
		<div id="errMsg" style="color: red;"></div>
	</div>
</asp:Content>

