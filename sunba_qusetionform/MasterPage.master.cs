using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
	public string LoginName, LoginEmpno;
	Competence_DB cdb = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!LogInfo.isLogin)
		{
			Response.Redirect("https://powersunba.com.tw/SSO/");
			//Session["登入工號"] = "531777";
			//Session["登入姓名"] = "Nick";
		}
		else
		{
			LoginEmpno = Session["登入工號"].ToString();
			LoginName = Session["登入姓名"].ToString();
			Account.ExecSignIn(LoginEmpno);
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
			case "MealsStatistics.aspx":
			case "MealsCompany.aspx":
			case "MealsCompanyRegister.aspx":
			case "MealsPaymentManage.aspx":
			case "MealsFee.aspx":
			case "MealsCost.aspx":
			case "MealsLocation.aspx":
				cStatus = CheckDtList("01");
				break;
			case "OfficialCarManage.aspx":
			case "OutdoorManagerList.aspx":
				cStatus = CheckDtList("03");
				break;
			case "MeetingRoomManage.aspx":
				cStatus = CheckDtList("06");
				break;
			case "DormitoryManage.aspx":
			case "DormitoryRoomManage.aspx":
				cStatus = CheckDtList("dormitory");
				break;
			case "SystemSetting.aspx":
				cStatus = CheckDtList("");
				break;
		}

		if (cStatus)
			Response.Write("<script type='text/javascript'>alert('您沒有權限進入此頁面!'); location.href='Default.aspx'</script>");
	}

	private bool CheckDtList(string cType)
	{
		bool status = true;
		DataTable dt = new DataTable();
		if (cType != "dormitory")
		{
			cdb._c_type = cType;
			dt = cdb.GetCompetenceList_Common();
		}
		else
			dt = cdb.GetCompetenceList_ForDormitory();


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
