<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryCancelRegister.aspx.cs" Inherits="WebPage_DormitoryCancelRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryCancelRegister.js"></script>
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
								<input class="form-control" type="text" disabled="disabled" value="<%= empName %>" />
							</div>
						</div>
					</div>
				</div>
				<div class="row gy-2 mt-1 align-items-center OchiRow">
					<div class="col-md-6">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">退宿日期</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="date" id="dc_canceldate" />
							</div>
						</div>
					</div>
				</div>
				<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
					<div class="col-md-auto TitleSetWidth text-md-end">
						<label class="form-label" for="reson">申請事由</label>
					</div>
					<div class="col-md-auto flex-grow-1">
						<input class="form-control" id="dc_reason" type="text" />
					</div>
				</div>
			</div>
			<div class="text-end mt-2">
				<a href="javascript:void(0);" id="savebtn" class="btn btn-primary text-nowrap btn-sm">送出</a>
			</div>
		</div>
	</div>
</asp:Content>

