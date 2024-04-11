using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DeleteReviewDormitoryCancel : System.Web.UI.Page
{
	DormitoryCancel_DB db = new DormitoryCancel_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核 - 確定退宿
		///說    明:
		/// * Request["id"]: id
		/// * Request["empno"]: 工號
		/// * Request["roomno"]: 房號
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
			string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Common.FilterCheckMarxString(Request["empno"].ToString().Trim());
			string roomno = (string.IsNullOrEmpty(Request["roomno"])) ? "" : Common.FilterCheckMarxString(Request["roomno"].ToString().Trim());

			db._dc_id = id;
			db._dc_modid = LogInfo.empNo;
			db._dc_modname = LogInfo.empName;
			db.DeleteDormitoryCancel(empno, roomno);

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