using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class MasterPage : System.Web.UI.MasterPage
{
	public string LoginName, LoginEmpno, LoginDeptCode, LoginDeptName;
	Competence_DB cdb = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!LogInfo.isLogin)
		{
			if(ConfigurationManager.AppSettings["IsTesting"] == "open")
            {
				Session["登入工號"] = "laputa";
				Session["登入姓名"] = "賴斐瓔";
				Session["dept_code"] = "Company";
				Session["dept_name"] = "森霸";
				LoginEmpno = Session["登入工號"].ToString();
				LoginName = Session["登入姓名"].ToString();
				LoginDeptCode = Session["dept_code"].ToString();
				LoginDeptName = Session["dept_name"].ToString();
			}
            else
            {
				Response.Redirect("https://powersunba.com.tw/SSO/");
			}
		}
		else
		{
			LoginEmpno = Session["登入工號"].ToString();
			LoginName = Session["登入姓名"].ToString();
			LoginDeptCode = Session["dept_code"].ToString();
			LoginDeptName = Session["dept_name"].ToString();
			//Account.ExecSignIn(LoginEmpno);
			CheckCompetence();
		}
	}

	private void CheckCompetence()
	{
		string[] PageUrl = Request.AppRelativeCurrentExecutionFilePath.Split('/');
		string nowPage = PageUrl[PageUrl.Length - 1];
		bool cStatus = false;
		switch (nowPage)
		{
			case "index.aspx":
				break;
			case "SystemSetting.aspx":
				cStatus = CheckDtList("");
				break;
		}

		if (cStatus)
			Response.Write("<script type='text/javascript'>alert('您沒有權限進入此頁面!'); location.href='index.aspx'</script>");
	}

	private bool CheckDtList(string cType)
	{
		bool status = true;
		DataTable dt = new DataTable();
		dt = cdb.GetCompetenceList_Common();

		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (dt.Rows[i]["value"].ToString() == LogInfo.empNo)
					status = false;
			}
		}
		return status;
	}
}
