<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryDetail.aspx.cs" Inherits="WebPage_DormitoryDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryDetail.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="border border-dark p-2 mt-1">
			<div class="ochiform TitleLength08">
				<div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">申請人姓名</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" id="d_name" type="text" disabled="disabled" />
							</div>
						</div>
					</div>

					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label" for="Name">申請類別</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select class="form-select" aria-label="Default select example" disabled="disabled" id="d_type"></select>
							</div>
						</div>
					</div>
				</div>

				<div class="ItemForA">
					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">戶口名簿印本</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<div class="mt-1">
								<div class="mt-1" id="d_attach"></div>
							</div>
						</div>
					</div>
				</div>

				<div class="ItemForB">
					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">住宿起訖</div>
						<div class="col-md-auto flex-grow-1">
							<div class="d-flex align-items-center flex-nowrap">
								<div class="flex-grow-1 me-1"><input class="form-control" type="date" disabled="disabled" id="d_startday"></div>
								<div class="text-center">~</div>
								<div class="flex-grow-1 ms-1"><input class="form-control" type="date" disabled="disabled" id="d_endday"></div>
							</div>
						</div>
					</div>

					<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label" for="reson">申請事由</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control" id="d_reason" type="text" disabled="disabled" />
						</div>
					</div>
				</div>

				<div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">聯絡電話(手機)</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="text" disabled="disabled"  id="d_tel" />
							</div>
						</div>
					</div>
					<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">血型</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" type="text" disabled="disabled" id="d_bloodtype">
								</div>
							</div>
						</div>
				</div>

				<div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">緊急聯絡人</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="text" disabled  id="d_emergency_contact" />
							</div>
						</div>
					</div>
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">緊急聯絡人電話</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="text" disabled  id="d_emergency_tel" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

