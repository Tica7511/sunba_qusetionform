using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class WebPage_GuardSignIn : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["AffairsGuard"] != null)
		{
			if (!string.IsNullOrEmpty(Session["AffairsGuard"].ToString()))
			{
				Response.Redirect("~/WebPage/GuardRoom.aspx");
			}
		}
	}

	protected void SignInBtn(object sender, EventArgs e)
	{
		if (tb_text.Text == ConfigurationManager.AppSettings["GuardPW"])
		{
			Session["AffairsGuard"] = "GuardSession";
			Response.Redirect("~/WebPage/GuardRoom.aspx");
		}
		else
		{
			errorMsg.Visible = true;
			errorMsg.Text = "密碼錯誤";
		}
	}
}