using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_GuardRoom : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["AffairsGuard"] != null)
		{
			if(string.IsNullOrEmpty(Session["AffairsGuard"].ToString()))
			{
				Response.Redirect("~/WebPage/GuardSignIn.aspx");
			}
		}
		else
		{
			Response.Redirect("~/WebPage/GuardSignIn.aspx");
		}
	}
}