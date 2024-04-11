using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DeleteMeeting : System.Web.UI.Page
{
	Meeting_DB db = new Meeting_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 刪除會議申請
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
			string[] FromPage = Request.UrlReferrer.AbsolutePath.Split('/');
			if (FromPage[FromPage.Length - 1] == "MeetingDetail.aspx")
				id = Common.Decrypt(id);
			else
				id = Common.FromBase64String(Server.UrlDecode(id));

			db._m_id = id;
			db._m_modid = LogInfo.empNo;
			db._m_modname = LogInfo.empName;
			db.DeleteMeeting();

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