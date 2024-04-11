using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_AddOutdoorForm : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 出廠證明單申請
		///說    明:
		/// * Request["id"]: ID
		/// * Request["o_type"]: 申請類型
		/// * Request["o_passenger_number"]: 共乘人數
		/// * Request["ddlPerson"]: 共乘同仁
		/// * Request["o_starttime"]: 進場時間
		/// * Request["o_endtime"]: 出廠時間
		/// * Request["o_place"]: 地點
		/// * Request["o_reason"]: 事由
		/// * Request["o_ps"]: 備註
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
			string o_type = (string.IsNullOrEmpty(Request["o_type"])) ? "" : Common.FilterCheckMarxString(Request["o_type"].ToString().Trim());
			string o_passenger_number = (string.IsNullOrEmpty(Request["o_passenger_number"])) ? "" : Common.FilterCheckMarxString(Request["o_passenger_number"].ToString().Trim());
			string ddlPerson = (string.IsNullOrEmpty(Request["ddlPerson"])) ? "" : Common.FilterCheckMarxString(Request["ddlPerson"].ToString().Trim());
			string o_startdate = (string.IsNullOrEmpty(Request["o_startdate"])) ? "" : Common.FilterCheckMarxString(Request["o_startdate"].ToString().Trim());
			string o_enddate = (string.IsNullOrEmpty(Request["o_enddate"])) ? "" : Common.FilterCheckMarxString(Request["o_enddate"].ToString().Trim());
			string o_starthour = (string.IsNullOrEmpty(Request["o_starthour"])) ? "" : Common.FilterCheckMarxString(Request["o_starthour"].ToString().Trim());
			string o_endhour = (string.IsNullOrEmpty(Request["o_endhour"])) ? "" : Common.FilterCheckMarxString(Request["o_endhour"].ToString().Trim());
			string o_startmins = (string.IsNullOrEmpty(Request["o_startmins"])) ? "" : Common.FilterCheckMarxString(Request["o_startmins"].ToString().Trim());
			string o_endmins = (string.IsNullOrEmpty(Request["o_endmins"])) ? "" : Common.FilterCheckMarxString(Request["o_endmins"].ToString().Trim());
			string o_car = (string.IsNullOrEmpty(Request["o_car"])) ? "" : Common.FilterCheckMarxString(Request["o_car"].ToString().Trim());
			string o_place = (string.IsNullOrEmpty(Request["o_place"])) ? "" : Common.FilterCheckMarxString(Request["o_place"].ToString().Trim());
			string o_reason = (string.IsNullOrEmpty(Request["o_reason"])) ? "" : Common.FilterCheckMarxString(Request["o_reason"].ToString().Trim());
			string o_ps = (string.IsNullOrEmpty(Request["o_ps"])) ? "" : Common.FilterCheckMarxString(Request["o_ps"].ToString().Trim());

			string tmpGuid = Guid.NewGuid().ToString("N");

			DateTime sTime = DateTime.Parse(o_startdate + " " + o_starthour + ":" + o_startmins);
			DateTime eTime = DateTime.Parse(o_enddate + " " + o_endhour + ":" + o_endmins);

			db._o_guid = tmpGuid;
			db._o_type = o_type;
			db._o_starttime = sTime;
			db._o_endtime = eTime;
			if (o_type == "02")
			{
				db._o_passenger_number = o_passenger_number;
				db._o_passenger_empno = ddlPerson;
				db._o_car = o_car;
			}
			db._o_place = o_place;
			db._o_reason = o_reason;
			db._o_ps = o_ps;
			db._o_createid = LogInfo.empNo;
			db._o_createname = LogInfo.empName;
			db._o_modid = LogInfo.empNo;
			db._o_modname = LogInfo.empName;

			db.addOutdoor();

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