using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsLocation : System.Web.UI.Page
{
	MealsLocation_DB db = new MealsLocation_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 用餐地點管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["ml_name"]: 地點名稱
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
			string ml_name = (string.IsNullOrEmpty(Request["ml_name"])) ? "" : Common.FilterCheckMarxString(Request["ml_name"].ToString().Trim());

			db._ml_name = ml_name;
			db._ml_createid = LogInfo.empNo;
			db._ml_createname = LogInfo.empName;
			db._ml_modid = LogInfo.empNo;
			db._ml_modname = LogInfo.empName;

			if (id == "")
			{
				db._ml_guid = Guid.NewGuid().ToString("N");
				db.addMealsLocation();
			}
			else
			{
				db._ml_id = id;
				db.UpdateMealsLocation();
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