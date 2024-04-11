<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SystemSetting.aspx.cs" Inherits="WebPage_SystemSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/SystemSetting.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<form id="dataForm" name="dataForm">
			<div class="border border-dark p-2 mt-1">
				<div class="ochiform TitleLength09">
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">訪客用餐承辦人</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="MealsVisitor" name="MealsVisitor" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">當日取消用餐承辦人</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="MealsCancel" name="MealsCancel" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">外出單承辦主管</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="OutdoorForm" name="OutdoorForm" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">宿舍承辦人</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="Dormitory" name="Dormitory" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">宿舍承辦主管</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="DormitoryManager" name="DormitoryManager" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">會議室管理員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="Meeting" name="Meeting" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">文件管理員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="Doc" name="Doc" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="row flex-md-nowrap align-items-center">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">系統管理員</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select id="SystemAdmin" name="SystemAdmin" class="multiple-select  width100" multiple placeholder="請選擇人員"></select>
							</div>
						</div>
					</div>
				</div>

				<div class="text-end mt-2">
					<a href="javascript:void(0);" id="savebtn" class="btn btn-primary text-nowrap btn-sm">儲存</a>
				</div>
			</div>
		</form>
	</div>
</asp:Content>

