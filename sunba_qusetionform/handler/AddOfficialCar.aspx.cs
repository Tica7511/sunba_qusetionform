using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddOfficialCar : System.Web.UI.Page
{
	OfficialCar_DB db = new OfficialCar_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 公務車管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["oc_number"]: 車牌號碼
		/// * Request["oc_ps"]: 備註
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
			string oc_number = (string.IsNullOrEmpty(Request["oc_number"])) ? "" : Common.FilterCheckMarxString(Request["oc_number"].ToString().Trim());
			string oc_ps = (string.IsNullOrEmpty(Request["oc_ps"])) ? "" : Common.FilterCheckMarxString(Request["oc_ps"].ToString().Trim());

			db._oc_number = oc_number;
			db._oc_ps = oc_ps;
			db._oc_createid = LogInfo.empNo;
			db._oc_createname = LogInfo.empName;
			db._oc_modid = LogInfo.empNo;
			db._oc_modname = LogInfo.empName;

			if (id == "")
			{
				db._oc_guid = Guid.NewGuid().ToString("N");
				db.addOfficialCar();
			}
			else
			{
				db._oc_id = id;
				db.UpdateOfficialCar();
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