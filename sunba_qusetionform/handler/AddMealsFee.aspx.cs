using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsFee : System.Web.UI.Page
{
	MealsFee_DB db = new MealsFee_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 房間管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["mr_name"]: 會議室名稱
		/// * Request["mr_place"]: 地點
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
			int mf_employee = (string.IsNullOrEmpty(Request["mf_employee"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mf_employee"].ToString().Trim()));
			int mf_firm = (string.IsNullOrEmpty(Request["mf_firm"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mf_firm"].ToString().Trim()));
			int mf_visitor = (string.IsNullOrEmpty(Request["mf_visitor"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mf_visitor"].ToString().Trim()));
			int mf_love = (string.IsNullOrEmpty(Request["mf_love"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mf_love"].ToString().Trim()));
			string mf_effectivedate = (string.IsNullOrEmpty(Request["mf_effectivedate"])) ? "" : Common.FilterCheckMarxString(Request["mf_effectivedate"].ToString().Trim());

			db._mf_employee = mf_employee;
			db._mf_firm = mf_firm;
			db._mf_visitor = mf_visitor;
			db._mf_love = mf_love;
			db._mf_effectivedate = mf_effectivedate;
			db._mf_createid = LogInfo.empNo;
			db._mf_createname = LogInfo.empName;
			db._mf_modid = LogInfo.empNo;
			db._mf_modname = LogInfo.empName;

			if (id == "")
			{
				db.addMealsFee();
			}
			else
			{
				db._mf_id = id;
				db.UpdateMealsFee();
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