<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OutdoorManagerList.aspx.cs" Inherits="WebPage_OutdoorManagerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/OutdoorManagerList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="border border-dark p-2 mt-1">
		<div class="ochiform TitleLength08">
			<div class="row gy-2 mt-1 align-items-center OchiRow">
				<div class="col-md-6">
					<div class="row flex-md-nowrap align-items-center">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">申請人姓名</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<input class="form-control" type="text" id="SearchStr" />
						</div>
					</div>
				</div>
			</div>
			<div class="row gy-2 mt-1 align-items-center OchiRow">
				<div class="col-md-6">
					<div class="row flex-md-nowrap align-items-center">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">申請類別</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<select class="form-select" id="SearchType"></select>
						</div>
					</div>
				</div>
				<div class="col-md-6">
					<div class="row flex-md-nowrap align-items-center">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">車號</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<select class="form-select" id="SearchCarNo"></select>
						</div>
					</div>
				</div>
			</div>

			<div class="row gy-2 mt-1 align-items-center OchiRow">
				<div class="col-md-6">
					<div class="row flex-md-nowrap align-items-center">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">預計往返日期</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<div class="d-flex align-items-center flex-nowrap">
								<div class="flex-grow-1 me-1"><input class="form-control" type="date" id="SearchStartDate" /></div>
								<div class="text-center">~</div>
								<div class="flex-grow-1 ms-1"><input class="form-control" type="date" id="SearchEndDate" /></div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-md-6">
					<div class="row flex-md-nowrap align-items-center">
						<div class="col-md-auto TitleSetWidth text-md-end">
							<label class="form-label">實際往返日期</label>
						</div>
						<div class="col-md-auto flex-grow-1">
							<div class="d-flex align-items-center flex-nowrap">
								<div class="flex-grow-1 me-1"><input class="form-control" type="date"  id="SearchActualOut" /></div>
								<div class="text-center">~</div>
								<div class="flex-grow-1 ms-1"><input class="form-control" type="date"  id="SearchActualBack" /></div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="text-end mt-2">
			<a href="javascript:void(0);" id="SearchBtn" class="btn btn-primary text-nowrap btn-sm">查詢</a>
		</div>
	</div>

	<div class="row align-items-center justify-content-between mt-3">
		<div class="col-md-auto order-1 order-md-0">
			<h4 class="fw-bold">查詢結果</h4>
		</div>
		<div class="col-md-auto order-0 order-md-1"><a href="javascript:void(0);" id="exportbtn" class="btn btn-primary text-nowrap btn-sm">匯出</a></div>
	</div>
	<div class="mt-1">
		<table id="tablist" class="table table-bordered table-striped font-size2">
			<thead>
				<tr class="table-secondary">
					<th class="text-center" nowrap="">申請類別</th>
					<th class="text-center" nowrap="">申請日期</th>
					<th class="text-center" nowrap="">申請人姓名</th>
					<th class="text-center" nowrap="">公務車(車號)</th>
					<th class="text-center" nowrap="">地點</th>
					<th class="text-center" nowrap="">預計往返日期與時間</th>
					<th class="text-center" nowrap="">實際往返日期與時間</th>
					<th class="text-center" nowrap="">事由</th>
					<th class="text-center" nowrap="">審核</th>
					<th class="text-center" nowrap="">功能</th>
				</tr>
			</thead>
			<tbody></tbody>
		</table>
		<div id="pageblock"></div>
	</div>
</asp:Content>

