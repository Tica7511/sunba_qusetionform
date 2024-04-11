using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class handler_GetReviewToUOFThirdLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 第三方登入 - 直接轉進 UOF
        ///-----------------------------------------------------
        string url = string.Empty;
        try
        {
            string WebUrl = ConfigurationManager.AppSettings["WebURL"];
            string TargetUrl = (Request["TargetUrl"] != null) ? Request["TargetUrl"].ToString().Trim() : "";

            string appName = "UOFTeams";
            string key = "$$20200707##wllz)@aF";
            string iv = "tms6$77*kIsnlAqe";

            url = ThirdLoginHelper.Encryption.GetUrl(appName, WebUrl, TargetUrl, LogInfo.empNo, key, iv);
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        Response.Redirect(url);
    }
}