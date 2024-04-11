<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OfficialCarManage.aspx.cs" Inherits="WebPage_OfficialCarManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/OfficialCarManage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="mt-1">
			<table id="tablist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">車號</th>
						<th class="text-center">備註</th>
						<th class="text-center">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>

		<!-- Modal -->
		<input id="tmpid" type="hidden" value="" />
		<div class="modal fade" id="OfficialCarModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" role="dialog" data-bs-backdrop="static">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="exampleModalLabel">公務車維護</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="ochiform TitleLength05">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="formA1">車號</label></div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" id="oc_number" type="text"></div>
							</div>
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="formA2">備註</label></div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" id="oc_ps" type="text" maxlength="200" /></div>
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
	</div>
</asp:Content>

