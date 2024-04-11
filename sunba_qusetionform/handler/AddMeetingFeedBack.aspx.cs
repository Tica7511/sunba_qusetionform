using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_AddMeetingFeedBack : System.Web.UI.Page
{
	MeetingFeedBack_DB db = new MeetingFeedBack_DB();
	Competence_DB c_db = new Competence_DB();
	Personnel_DB p_db = new Personnel_DB();
	MailUtil email = new MailUtil();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 新增會議室使用後狀況回覆
		///說    明:
		/// * Request["mfb_room"]: 會議室 Guid
		/// * Request["mfb_date"]: 日期
		/// * Request["mfb_content"]: 狀況回覆內容
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

			string mfb_room = (string.IsNullOrEmpty(Request["mfb_room"])) ? "" : Common.FilterCheckMarxString(Request["mfb_room"].ToString().Trim());
			string mfb_date = (string.IsNullOrEmpty(Request["mfb_date"])) ? "" : Common.FilterCheckMarxString(Request["mfb_date"].ToString().Trim());
			string mfb_content = (string.IsNullOrEmpty(Request["mfb_content"])) ? "" : Common.FilterCheckMarxString(Request["mfb_content"].ToString().Trim());

			db._mfb_room = mfb_room;
			db._mfb_date = mfb_date;
			db._mfb_content = mfb_content;
			db._mfb_createid = LogInfo.empNo;
			db._mfb_createname = LogInfo.empName;
			db._mfb_modid = LogInfo.empNo;
			db._mfb_modname = LogInfo.empName;

			db._mfb_guid = Guid.NewGuid().ToString("N");
			db.addMeetingFeedBack();

			#region 發信通知
			string MailAddr = string.Empty;
			string Content = string.Empty;

			c_db._c_type = "06";
			DataTable dt = c_db.GetListOfType_ForMail();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataTable edt = p_db.GetEmployeeEmil(dt.Rows[i]["value"].ToString());
					if (edt.Rows.Count > 0)
					{
						// 收件者
						if (MailAddr != "") MailAddr += ",";
						MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
					}
				}
			}
			
			// 信件內容
			Content = "系統通知:<br><br>";
			Content += "同仁 " + LogInfo.empName + " 回報會議室使用後狀況，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行檢視與處理，感謝您。";

			// 發信
			if (MailAddr != "")
				email.MailTo(MailAddr, "會議室使用後狀況回報通知", Content);
			#endregion

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