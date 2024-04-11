using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

public partial class handler_AddMealsRegister : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 用餐登記
		///說    明:
		/// * Request["category"]: 用餐登記類別 01: 員工 02: 廠商 03: 愛心便當
		/// * Request["RegisterId"]: 登記者 id
		/// * Request["dtRow"]: list row date -> 日期 yyyy-MM-dd
		/// * Request["LunchNum"]: 午餐份數
		/// * Request["LunchPlace"]: 午餐地點
		/// * Request["DinnerNum"]: 晚餐份數
		/// * Request["DinnerPlace"]: 晚餐地點
		///-----------------------------------------------------
		///紀　錄：
		/// 2023-12-19 同仁用餐移除份數，固定為1份
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();

		/// Transaction
		SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
		oConn.Open();
		SqlCommand oCmmd = new SqlCommand();
		oCmmd.Connection = oConn;
		SqlTransaction myTrans = oConn.BeginTransaction();
		oCmmd.Transaction = myTrans;
		try
		{
			#region Check Session Timeout
			if (!LogInfo.isLogin)
			{
				throw new Exception("登入帳號已失效，請重新登入");
			}
			#endregion

			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
			string RegisterId = (string.IsNullOrEmpty(Request["RegisterId"])) ? "" : Common.FilterCheckMarxString(Request["RegisterId"].ToString().Trim());
			string[] dtRow = (string.IsNullOrEmpty(Request["dtRow"])) ? new string[] { } : Common.FilterCheckMarxString(Request["dtRow"].ToString().Trim()).Split(',');
			string[] LunchNum = (string.IsNullOrEmpty(Request["NumL"])) ? new string[] { } : Common.FilterCheckMarxString(Request["NumL"].ToString().Trim()).Split(',');
			string[] LunchPlace = (string.IsNullOrEmpty(Request["PlaceL"])) ? new string[] { } : Common.FilterCheckMarxString(Request["PlaceL"].ToString().Trim()).Split(',');
			string[] DinnerNum = (string.IsNullOrEmpty(Request["NumD"])) ? new string[] { } : Common.FilterCheckMarxString(Request["NumD"].ToString().Trim()).Split(',');
			string[] DinnerPlace = (string.IsNullOrEmpty(Request["PlaceD"])) ? new string[] { } : Common.FilterCheckMarxString(Request["PlaceD"].ToString().Trim()).Split(',');

			string PersonID = (category == "01") ? LogInfo.empNo : Common.Decrypt(RegisterId);

			if (dtRow.Length > 0)
			{
				for (int i = 0; i < dtRow.Length; i++)
				{
					string eatLunch = (string.IsNullOrEmpty(Request["rbLunch_" + DateTime.Parse(dtRow[i]).ToString("yyyyMMdd")])) ? "" : Common.FilterCheckMarxString(Request["rbLunch_" + DateTime.Parse(dtRow[i]).ToString("yyyyMMdd")].ToString().Trim());
					string eatDinner = (string.IsNullOrEmpty(Request["rbDinner_" + DateTime.Parse(dtRow[i]).ToString("yyyyMMdd")])) ? "" : Common.FilterCheckMarxString(Request["rbDinner_" + DateTime.Parse(dtRow[i]).ToString("yyyyMMdd")].ToString().Trim());

					db._mr_guid = Guid.NewGuid().ToString("N");
					db._mr_person_id = PersonID;
					db._mr_type = category;
					db._mr_date = dtRow[i];
					db._mr_lunch = eatLunch;
					db._mr_lunch_num = (category == "01") ? 1 : int.Parse(LunchNum[i]);
					db._mr_lunch_location = LunchPlace[i];
					db._mr_dinner = eatDinner;
					db._mr_dinner_num = (category == "01") ? 1 : int.Parse(DinnerNum[i]);
					db._mr_dinner_location = DinnerPlace[i];
					db._mr_createid = LogInfo.empNo;
					db._mr_createname = LogInfo.empName;
					db._mr_modid = LogInfo.empNo;
					db._mr_modname = LogInfo.empName;
					db.addMealsRegister(oConn, myTrans);
				}
			}

			myTrans.Commit();
			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>登記完成</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			myTrans.Rollback();
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		finally
		{
			oCmmd.Connection.Close();
			oConn.Close();
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}