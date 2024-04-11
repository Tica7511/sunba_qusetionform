using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_AddDormitoryCancel : System.Web.UI.Page
{
	DormitoryCancel_DB db = new DormitoryCancel_DB();
	Competence_DB c_db = new Competence_DB();
	Personnel_DB p_db = new Personnel_DB();
	MailUtil email = new MailUtil();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 退宿申請
		///說    明:
		/// * Request["category"]: 類型
		/// * Request["reason"]: 事由
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

			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
			string canceldate = (string.IsNullOrEmpty(Request["canceldate"])) ? "" : Common.FilterCheckMarxString(Request["canceldate"].ToString().Trim());
			string reason = (string.IsNullOrEmpty(Request["reason"])) ? "" : Common.FilterCheckMarxString(Request["reason"].ToString().Trim());

			db._dc_guid = Guid.NewGuid().ToString("N");
			db._dc_category = category;
			db._dc_canceldate = canceldate;
			db._dc_reason = reason;
			db._dc_createid = LogInfo.empNo;
			db._dc_createname = LogInfo.empName;
			db._dc_modid = LogInfo.empNo;
			db._dc_modname = LogInfo.empName;

			db.addDormitoryCancel();

			#region 發信通知
			string MailAddr = string.Empty;
			string Content = string.Empty;

			c_db._c_type = "04";
			DataTable dt = c_db.GetListOfType_ForMail();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataTable edt = p_db.GetEmployeeEmil(dt.Rows[0]["value"].ToString());
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
			Content += "同仁 " + LogInfo.empName + " 申請長期住宿退宿，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行檢視並調整宿舍入住管理，感謝您。";

			// 發信
			if (MailAddr != "")
				email.MailTo(MailAddr, "長期住宿退宿申請通知", Content);
			#endregion

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>申請已送出</Response></root>";
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