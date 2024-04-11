<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GuardSignIn.aspx.cs" Inherits="WebPage_GuardSignIn" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="content-language" content="zh-TW" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。-->
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。-->
	<meta name="author" content="ochison" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... -->
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>森霸電力營運管理系統-警衛室</title>
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" /><!-- bootstrap 5.2 -->
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/mdb.free.css") %>" /><!-- bootstrap mdb5 -->
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/fontawesome-all.min.css") %>"><!-- css icon fontawesome 5 -->
	<link rel="stylesheet" href="<%= ResolveUrl("~/css/login.css") %>" /><!-- 本系統專用 -->
	
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-3.7.0.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.bundle.min.js") %>"></script><!-- bootstrap 5.2 -->
</head>
<body>
	<section class="intro">
		<div class="bg-image h-100">
			<div class="mask d-flex align-items-center h-100 bodygbg">
				<div class="container">
					<div class="row d-flex justify-content-center align-items-center">
						<div class="col-12 col-lg-9 col-xl-8">
							<div class="card" style="border-radius: 1rem;">
								<div class="row g-0">
									<div class="col-md-4 d-none d-md-block">
										<img src="<%= ResolveUrl("~/images/logoimg.jpg") %>" alt="login form" class="img-fluid" style="border-top-left-radius: 1rem; border-bottom-left-radius: 1rem;" />
									</div>
									<div class="col-md-8 d-flex align-items-center">
										<div class="card-body py-5 px-4 p-md-5">

											<form runat="server">
												<div class="mb-3 d-flex align-items-center">
													<img src="<%= ResolveUrl("~/images/logo.svg") %>" height="35">
													<span class="fs-3 fw-bold">森霸電力營運管理系統-警衛室</span>
												</div>

												<div class="form-outline mb-4">
													<asp:TextBox ID="tb_text" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
													<label class="form-label" for="loginpw">密碼</label>
													<asp:Label ID="errorMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
												</div>

												<div class="d-flex justify-content-end pt-1 mb-4">
													<asp:Button ID="submit" runat="server" text="登入" CssClass="btn btn-primary" OnClick="SignInBtn" />
												</div>
											</form>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</section>
	<script src="js/mdb.min.js"></script>
</body>
</html>
