<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GuardRoom.aspx.cs" Inherits="WebPage_GuardRoom" %>

<!DOCTYPE html>

<html>
<head runat="server">
	<title>森霸庶務系統</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="content-language" content="zh-TW" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" />
	<meta name="generator" content="Notepad" />
	<meta name="author" content="ochison" />
	<meta name="copyright" content="本網頁著作權所有" />
	<meta name="revisit-after" content="3 days" />

	<link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap-icons.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/fontawesome.min.css") %>">
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/metisMenu.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/multiple-select.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/OClayout.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/style.css") %>" />
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/NickStyle.css") %>" />
	<!-- 跑馬燈 -->
	<!-- IE 瀏覽器處理 -->
	<!--[if lte IE 11]>
	<link rel="stylesheet" href="css/cssie.css" />
	<![endif]-->
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-3.7.0.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-ui.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.bundle.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/metisMenu.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/multiple-select.min.js") %>"></script>

	<!--MyJS-->
	<script type="text/javascript" src="<%= ResolveUrl("~/page_js/GuardRoom.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/NickCommon.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/PageList.js") %>"></script>
</head>
<body class="bodybgcolor">
	<div class="d-flex">
		<div class="border-end bg-white shadowR" id="sidebar-wrapper">
			<div class="sidebar-heading"></div>
			<div class="metismenubox">
				<ul id="metismenu">
					<li><a target="_self" href="<%= ResolveUrl("~/WebPage/GuardSignOut.aspx") %>">登出</a></li>
				</ul>
			</div>
		</div>

		<div id="page-content-wrapper" class="d-flex vh-100 flex-column flex-content-between">
			<div class="flex-grow-1">
				<nav id="page-content-header" class="navbar navbar-expand-lg navbar-light bg-light border-bottom shadow-0 headerbgcolor">
					<div class="container-fluid">
						<div class="d-flex">
							<button class="btn btn-warning btn-sm" id="sidebarToggle"><i class="fas fa-bars "></i></button>
							<div class="d-inline-block ps-lg-2 fs-4 fw-bold text-white">森霸電力營運管理系統</div>
						</div>
					</div>
				</nav>

				<div id="page-content-body">
					<div class="container-xxl">
						<div class="d-flex align-items-center justify-content-between mt-4">
							<div>
								外出單 / 外出單(警衛室)
								<div class="fs-3 fw-bold">外出單</div>
							</div>
						</div>
						<div class="container-xxl mb-3">
							<div class="row align-items-center justify-content-between mt-1">
								<div class="col-md-auto order-1 order-md-0">
									<h4 class="text-primary">公務車出入時間管理</h4>
								</div>
								<div class="col-md-auto order-0 order-md-1 text-danger">請協助填寫公務車實際出入廠時間</div>
							</div>
							<div id="datalist" class="row row-cols-1 row-cols-sm-2 row-cols-md-2 row-cols-lg-3 mt-2"></div>

							<div class="row align-items-center justify-content-between mt-3">
								<div class="col-md-auto order-1 order-md-0">
									<h4 class="text-primary">外出單列表</h4>
								</div>
								<div class="col-md-auto order-0 order-md-1 text-danger"></div>
							</div>

							<div class="mt-1">
								<table id="tablist" class="table table-bordered table-striped font-size2">
									<thead>
										<tr>
											<th class="text-center" nowrap=""><a href="javascript:void(0);" name="sortbtn" sortname="o_type">申請類別</a></th>
											<th class="text-center" nowrap="">申請日期</th>
											<th class="text-center" nowrap="">申請人姓名</th>
											<th class="text-center" nowrap="">公務車(車號)</th>
											<th class="text-center" nowrap="">地點</th>
											<th class="text-center" nowrap=""><a href="javascript:void(0);" name="sortbtn" sortname="outTime">預計往返日期與時間</a></th>
											<th class="text-center" nowrap="">事由</th>
										</tr>
									</thead>
									<tbody></tbody>
								</table>
								<div id="pageblock"></div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div id="page-content-footer" class="border-top w-100 text-center p-2 small footerbgcolor">
				版權所有© 2023 ITRI. 工業技術研究院著作
			</div>
		</div>
	</div>
	<script src="<%= ResolveUrl("~/js/unitcommon.js") %>"></script>
</body>
</html>
