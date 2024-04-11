using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsVisitor : System.Web.UI.Page
{
	MealsVisitor_DB db = new MealsVisitor_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 訪客用餐登記
		///說    明:
		/// * Request["id"]: ID
		/// * Request["mv_name"]: 廠商/訪客名稱
		/// * Request["mv_reason"]: 事由
		/// * Request["mv_date"]: 用餐時間
		/// * Request["mv_lunch_meat"]: 午餐葷
		/// * Request["mv_lunch_vegetarian"]: 午餐蛋奶素
		/// * Request["mv_lunch_vegan"]: 午餐全素
		/// * Request["mv_dinner_meat"]: 晚餐葷
		/// * Request["mv_dinner_vegetarian"]: 晚餐蛋奶素
		/// * Request["mv_dinner_vegan"]: 晚餐全素
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
			string mv_name = (string.IsNullOrEmpty(Request["mv_name"])) ? "" : Common.FilterCheckMarxString(Request["mv_name"].ToString().Trim());
			string mv_reason = (string.IsNullOrEmpty(Request["mv_reason"])) ? "" : Common.FilterCheckMarxString(Request["mv_reason"].ToString().Trim());
			string mv_date = (string.IsNullOrEmpty(Request["mv_date"])) ? "" : Common.FilterCheckMarxString(Request["mv_date"].ToString().Trim());
			int mv_lunch_meat = (string.IsNullOrEmpty(Request["mv_lunch_meat"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_lunch_meat"].ToString().Trim()));
			int mv_lunch_vegetarian = (string.IsNullOrEmpty(Request["mv_lunch_vegetarian"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_lunch_vegetarian"].ToString().Trim()));
			int mv_lunch_vegan = (string.IsNullOrEmpty(Request["mv_lunch_vegan"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_lunch_vegan"].ToString().Trim()));
			int mv_dinner_meat = (string.IsNullOrEmpty(Request["mv_dinner_meat"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_dinner_meat"].ToString().Trim()));
			int mv_dinner_vegetarian = (string.IsNullOrEmpty(Request["mv_dinner_vegetarian"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_dinner_vegetarian"].ToString().Trim()));
			int mv_dinner_vegan = (string.IsNullOrEmpty(Request["mv_dinner_vegan"])) ? 0 : Int32.Parse(Common.FilterCheckMarxString(Request["mv_dinner_vegan"].ToString().Trim()));

			db._mv_name = mv_name;
			db._mv_reason = mv_reason;
			db._mv_date = mv_date;
			db._mv_lunch_meat = mv_lunch_meat;
			db._mv_lunch_vegetarian = mv_lunch_vegetarian;
			db._mv_lunch_vegan = mv_lunch_vegan;
			db._mv_dinner_meat = mv_dinner_meat;
			db._mv_dinner_vegetarian = mv_dinner_vegetarian;
			db._mv_dinner_vegan = mv_dinner_vegan;
			db._mv_createid = LogInfo.empNo;
			db._mv_createname = LogInfo.empName;
			db._mv_modid = LogInfo.empNo;
			db._mv_modname = LogInfo.empName;

			string tmpGuid= Guid.NewGuid().ToString("N");
			if (id == "")
			{
				db._mv_guid = tmpGuid;
				db.addMealsVisitor();
			}
			else
			{
				db._mv_id = id;
				db.UpdateMealsVisitor();
			}

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><DataGuid>" + Server.UrlEncode(Common.Encrypt(tmpGuid)) + "</DataGuid></root>";
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