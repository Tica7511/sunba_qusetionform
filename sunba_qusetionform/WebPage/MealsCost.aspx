<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MealsCost.aspx.cs" Inherits="WebPage_MealsCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/MealsCost.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="row align-items-center justify-content-between mt-3">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">收支損益表</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
			</div>
		</div>

		<table id="statisticslist" class="table table-bordered table-striped">
			<thead>
				<tr class="table-secondary">
					<th class="text-center">成本</th>
					<th class="text-center">收入</th>
				</tr>
			</thead>
			<tbody></tbody>
		</table>

		<div class="row align-items-center justify-content-between mt-4">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">收入項目統計</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="newIncomeBtn">新增收入項目</a>
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="IncomeExportBtn">匯出</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="incomelist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">日期</th>
						<th class="text-center">金額</th>
						<th class="text-center">附件</th>
						<th class="text-center">備註</th>
						<th class="text-center" width="120">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="income_pageblock"></div>
		</div>

		<div class="row align-items-center justify-content-between mt-4">
			<div class="col-md-auto order-1 order-md-0">
				<h4 class="text-primary">成本項目統計</h4>
			</div>
			<div class="col-md-auto order-0 order-md-1">
				<a href="javascript:void(0);" id="newFoodBtn" class="btn btn-primary text-nowrap btn-sm">品名管理</a>
				<a href="javascript:void(0);" id="newCompanyBtn" class="btn btn-primary text-nowrap btn-sm">進貨廠商管理</a>
				<a href="javascript:void(0);" id="newCostBtn" class="btn btn-primary text-nowrap btn-sm">新增成本項目</a>
				<a href="javascript:void(0);" class="btn btn-primary text-nowrap btn-sm" id="CostExportBtn">匯出</a>
			</div>
		</div>

		<div class="mt-2">
			<table id="costlist" class="table table-bordered table-striped">
				<thead>
					<tr class="table-secondary">
						<th class="text-center">日期</th>
						<th class="text-center">金額</th>
						<th class="text-center">附件</th>
						<th class="text-center">備註</th>
						<th class="text-center" width="120">功能</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div id="cost_pageblock"></div>
		</div>

		<!-- Modal 收入項目-->
		<div class="modal fade" id="incomeModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">收入項目明細</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<form id="incomeData" name="incomeData">
						<div class="modal-body">
							<div class="ochiform TitleLength03">
								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">日期</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input type="date" class="form-control newstr" id="income_date" name="mc_date" />
									</div>
								</div>

								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">金額</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input type="text" class="form-control newstr num" id="income_price" name="mc_price" />
									</div>
								</div>

								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">附件</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input class="form-control newstr" type="file" id="income_file" name="mc_file" />
									</div>
								</div>

								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label">備註</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input class="form-control newstr" type="text" id="income_ps" name="mc_ps" />
									</div>
								</div>
							</div>
						</div>
					</form>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="IncomeSaveBtn">儲存</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal 成本項目-->
		<input id="cost_tmpgid" type="hidden" value="" class="newstr" />
		<input id="cost_tmpfile_id" type="hidden" value="" class="newstr" />
		<div class="modal fade" id="costModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-xl modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="exampleModalLabel">成本項目明細</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<form id="costData" name="costData">
						<div class="modal-body">
							<div class="ochiform TitleLength05">
								<div class="row gy-2 mt-1 align-items-center OchiRow">
									<div class="col-md-6">
										<div class="row flex-md-nowrap align-items-center">
											<div class="col-md-auto TitleSetWidth text-md-end">
												<label class="form-label" for="cost_date">日期</label>
											</div>
											<div class="col-md-auto flex-grow-1">
												<input class="form-control newstr" type="date" id="cost_date" name="mc_date" />
											</div>
										</div>
									</div>
								</div>

								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label" for="formA1">附件</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input class="form-control newstr" id="cost_file" name="mc_file" type="file" multiple="multiple" />
										<div class="mt-1" id="CostFileList"></div>
									</div>
								</div>

								<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
									<div class="col-md-auto TitleSetWidth text-md-end">
										<label class="form-label" for="formA31">備註</label>
									</div>
									<div class="col-md-auto flex-grow-1">
										<input class="form-control newstr" id="cost_ps" name="mc_ps" type="text" />
									</div>
								</div>
							</div>

							<h4 class="mt-2">項目</h4>
							<div class="mt-1">
								<table id="costitemlist" class="table table-bordered table-striped">
									<thead>
										<tr class="table-secondary">
											<th class="text-center">品名</th>
											<th class="text-center" width="80">數量</th>
											<th class="text-center" width="80">單位</th>
											<th class="text-center">單價</th>
											<th class="text-center">金額</th>
											<th class="text-center">廠商</th>
											<th class="text-center" width="80">功能</th>
										</tr>
									</thead>
									<tbody></tbody>
								</table>
							</div>
						</div>
					</form>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="CostSaveBtn">儲存</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal 品名管理-->
		<div class="modal fade" id="CostFoodModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">品名管理</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<form id="mcfForm" name="mcfForm">
							<table id="mcf_tablist" class="table table-bordered table-striped">
								<thead>
									<tr class="table-secondary">
										<th class="text-center">品名</th>
										<th class="text-center">單位</th>
										<th class="text-center">功能</th>
									</tr>
								</thead>
								<tbody></tbody>
							</table>
						</form>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="CostFoodSaveBtn">儲存</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal 進貨商管理-->
		<div class="modal fade" id="CostCompanyModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">進貨商管理</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<form id="mccForm" name="mccForm">
							<table id="mcc_tablist" class="table table-bordered table-striped">
								<thead>
									<tr class="table-secondary">
										<th class="text-center">進貨商名稱</th>
										<th class="text-center">電話</th>
										<th class="text-center">功能</th>
									</tr>
								</thead>
								<tbody></tbody>
							</table>
						</form>
					</div>
					<div class="modal-footer">
						<button id="CompanySaveBtn" type="button" class="btn btn-primary text-nowrap btn-sm">儲存</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal 匯出設定-->
		<input id="ExportMode" type="hidden" value="" />
		<div class="modal fade" id="ExportModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">匯出設定</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<div class="ochiform TitleLength04">
							<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
								<div class="col-md-auto TitleSetWidth text-md-end">時間起訖</div>
								<div class="col-md-auto flex-grow-1">
									<div class="d-flex align-items-center flex-nowrap">
										<div class="flex-grow-1 me-1">
											<input class="form-control" type="date" id="Export_StartDate">
										</div>
										<div class="text-center">~</div>
										<div class="flex-grow-1 me-1">
											<input class="form-control" type="date" id="Export_EndDate">
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-primary text-nowrap btn-sm" id="exportbtn">匯出</button>
						<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
