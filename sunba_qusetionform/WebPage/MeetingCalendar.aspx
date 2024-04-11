<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MeetingCalendar.aspx.cs" Inherits="WebPage_MeetingCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../fullcalendar/index.global.min.js"></script>
	<script type="text/javascript" src='../fullcalendar/popper.min.js'></script>
	<script type="text/javascript" src="../page_js/MeetingCalendar.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<input type="hidden" id="eventdata" />
	<div class="container-xxl mb-3">
		<div class="row">
			<div class="col-lg-2">
				<input id="tmpRoomid" type="hidden" />
				<div id="mRoomList" class="list-group"></div>
				<div class="card mt-2">
					<div class="card-header">
						會議室使用狀況
					</div>
					<ul id="feedbackList" class="list-group list-group-flush"></ul>
					<div class="d-grid gap-2">
						<button class="btn btn-primary text-nowrap btn-sm" type="button" id="openfeedbackbtn">回報使用後狀況</button>
					</div>
				</div>
			</div>
			<div class="col-lg-10">
				<div id="calendar"></div>
			</div>
		</div>
	</div>


	<div class="modal fade" id="roomsetting">
		<div class="modal-dialog modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="cancelmealLabel">會議室使用明細</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="ochiform TitleLength06">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">會議室名稱</label></div>
							<div class="col-md-auto flex-grow-1">
								<span id="RoomName"></span>
							</div>
						</div>
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">日期時間</label></div>
							<div class="col-md-auto flex-grow-1">
								<span id="meetingstart"></span> ~ <span id="meetingend"></span>
								<form id="reServeForm" action="<%= ResolveUrl("~/Webpage/MeetingRoomApply.aspx") %>" method="post">
										<input id="MeetingRoom" name="MeetingRoom" type="hidden" />
										<input id="MeetingDate" name="MeetingDate" type="hidden" />
										<input id="sTime" name="sTime" type="hidden" />
										<input id="eTime" name="eTime" type="hidden" />
								</form>
							</div>
						</div>
					</div>
				</div>

				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">關閉</button>
					<a href="javascript:void(0);" id="subbtn" class="btn btn-primary text-nowrap btn-sm">前往預約</a>
				</div>
			</div>
		</div>
	</div>

	
	<div class="modal fade" id="feedbackModal">
		<div class="modal-dialog modal-dialog-scrollable">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="">會議室使用後狀況回覆</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="ochiform TitleLength06">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">會議室名稱</label></div>
							<div class="col-md-auto flex-grow-1">
								<select class="form-select newstr" id="ddlMeetingRoom"></select>
							</div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">日期</label></div>
							<div class="col-md-auto flex-grow-1">
								<input type="date" class="form-control newstr" id="feedback_Date">
							</div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label">狀況回覆</label></div>
							<div class="col-md-auto flex-grow-1">
								<input type="text" class="form-control newstr" id="feedback">
							</div>
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-primary text-nowrap btn-sm" data-bs-dismiss="modal">取消</button>
					<a href="javascript:void(0);" id="feedbackBtn" class="btn btn-primary text-nowrap btn-sm">送出</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

