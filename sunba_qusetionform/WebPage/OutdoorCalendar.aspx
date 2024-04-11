<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OutdoorCalendar.aspx.cs" Inherits="WebPage_OutdoorCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../fullcalendar/index.global.min.js"></script>
	<script type="text/javascript" src='../fullcalendar/popper.min.js'></script>
	<script type="text/javascript" src="../page_js/OutdoorCalendar.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<div class="col-lg-12">
			<div id="calendar"></div>
		</div>
	</div>
</asp:Content>

