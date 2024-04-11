using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DeleteMeetingFeedBack : System.Web.UI.Page
{
	MeetingFeedBack_DB db = new MeetingFeedBack_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 會議室使用狀況已處理完成
		///說    明:
		/// * Request["id"]: id
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

			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Request["id"].ToString().Trim();

			db._mfb_id = id;
			db._mfb_modid = LogInfo.empNo;
			db._mfb_modname = LogInfo.empName;
			db.DeleteMeetingFeedBack();

			string xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
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