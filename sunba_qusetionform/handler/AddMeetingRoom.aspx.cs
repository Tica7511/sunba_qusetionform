using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMeetingRoom : System.Web.UI.Page
{
	MeetingRoom_DB db = new MeetingRoom_DB();
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
			string mr_name = (string.IsNullOrEmpty(Request["mr_name"])) ? "" : Common.FilterCheckMarxString(Request["mr_name"].ToString().Trim());
			string mr_place = (string.IsNullOrEmpty(Request["mr_place"])) ? "" : Common.FilterCheckMarxString(Request["mr_place"].ToString().Trim());

			db._mr_name = mr_name;
			db._mr_place = mr_place;
			db._mr_createid = LogInfo.empNo;
			db._mr_createname = LogInfo.empName;
			db._mr_modid = LogInfo.empNo;
			db._mr_modname = LogInfo.empName;

			if (id == "")
			{
				db._mr_guid = Guid.NewGuid().ToString("N");
				db.addMeetingRoom();
			}
			else
			{
				db._mr_id = id;
				db.UpdateMeetingRoom();
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