<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsCompany.aspx.cs" Inherits="WebPage_MealsCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsCompany.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="text-end my-2">
			<a href="javascript:void(0);" id="newbtn" class="btn btn-primary text-nowrap btn-sm">新增廠商/愛心便當</a>
		</div>

		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">外部廠商名稱</th>
						<th class="text-center">類別</th>
						<th class="text-center" width="200">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>
	</div>

	<!-- Modal -->
	<input id="tmpid" type="hidden" value="" class="newstr" />
	<div class="modal fade" id="EditModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">廠商/愛心便當維護</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="ochiform TitleLength07">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label" for="mc_name">名稱</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control newstr" id="mc_name" type="text" />
							</div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">類別</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<select class="form-select" id="ddltype"></select>
							</div>
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" id="savebtn">儲存</button>
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

