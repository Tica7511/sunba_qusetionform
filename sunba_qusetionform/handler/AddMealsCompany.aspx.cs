using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsCompany : System.Web.UI.Page
{
	MealsCompany_DB db = new MealsCompany_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 廠商/愛心便當管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["mc_category"]: 類別  02: 廠商  03: 愛心便當
		/// * Request["mc_name"]: 名稱
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

			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
			string mc_category = (string.IsNullOrEmpty(Request["mc_category"])) ? "" : Common.FilterCheckMarxString(Request["mc_category"].ToString().Trim());
			string mc_name = (string.IsNullOrEmpty(Request["mc_name"])) ? "" : Common.FilterCheckMarxString(Request["mc_name"].ToString().Trim());

			db._mc_category = mc_category;
			db._mc_name = mc_name;
			db._mc_createid = LogInfo.empNo;
			db._mc_createname = LogInfo.empName;
			db._mc_modid = LogInfo.empNo;
			db._mc_modname = LogInfo.empName;

			if (id == "")
			{
				db._mc_guid = Guid.NewGuid().ToString("N");
				db.addCompany();
			}
			else
			{
				db._mc_id = id;
				db.UpdateCompany();
			}

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>完成</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}