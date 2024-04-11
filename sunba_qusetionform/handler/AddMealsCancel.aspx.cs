using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_AddMealsCancel : System.Web.UI.Page
{
	MealsCancel_DB db = new MealsCancel_DB();
	Personnel_DB p_db = new Personnel_DB();
	MealsRegister_DB mr_db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 用餐取消
		///說    明:
		/// * Request["cReason"]: 事由
		/// * Request["cItem"]: 用餐項目
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			#region Check Session Timeout
			if (!LogInfo.isLogin)
			{
				throw new Exception("登入帳號已失效，請重新登入");
			}
			#endregion

			string cReason = (string.IsNullOrEmpty(Request["cReason"])) ? "" : Common.FilterCheckMarxString(Request["cReason"].ToString().Trim());
			string cItem = (string.IsNullOrEmpty(Request["cItem"])) ? "" : Common.FilterCheckMarxString(Request["cItem"].ToString().Trim());

			string tmpGuid = Guid.NewGuid().ToString("N");
			db._mc_guid = tmpGuid;
			db._mc_item = cItem;
			db._mc_reason = cReason;
			db._mc_createid = LogInfo.empNo;
			db._mc_createname = LogInfo.empName;
			db._mc_modid = LogInfo.empNo;
			db._mc_modname = LogInfo.empName;
			db.AddMealsCancel();

			string returnVal = Server.UrlEncode(Common.Encrypt(tmpGuid));
			
			#region 主任、課長級以上不用簽核
			if (CheckIdentity(LogInfo.empNo) > 0)
			{
				returnVal = "";
				mr_db._mr_modid = LogInfo.empNo;
				mr_db._mr_modname = LogInfo.empName;
				mr_db.ManagerMealsCancel(LogInfo.empNo, cItem);
			}
			#endregion

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><DataGuid>" + returnVal + "</DataGuid></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	private int CheckIdentity(string empno)
	{
		DataTable dt = p_db.GetEmployeeLevel(empno);
		if (dt.Rows.Count > 0)
		{
			return int.Parse(dt.Rows[0]["JobLv"].ToString());
		}
		else
			return 0;
	}
}