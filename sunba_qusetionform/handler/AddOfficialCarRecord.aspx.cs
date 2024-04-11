using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddOfficialCarRecord : System.Web.UI.Page
{
	OfficialCarRecord_DB db = new OfficialCarRecord_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 公務車出入時間管理
		///說    明:
		/// * Request["pid"]: 出廠證明單Guid
		/// * Request["ocr_car"]: 公務車Guid
		/// * Request["ocr_outtime"]: 出廠時間
		/// * Request["ocr_backtime"]: 回廠時間
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			#region Check Session Timeout
			if (Session["AffairsGuard"] != null)
			{
				if (string.IsNullOrEmpty(Session["AffairsGuard"].ToString()))
				{
					throw new Exception("登入帳號已失效，請重新登入");
				}
			}
			else
			{
				throw new Exception("登入帳號已失效，請重新登入");
			}
			#endregion

			string pid = (string.IsNullOrEmpty(Request["pid"])) ? "" : Common.FilterCheckMarxString(Request["pid"].ToString().Trim());
			string ocr_car = (string.IsNullOrEmpty(Request["ocr_car"])) ? "" : Common.FilterCheckMarxString(Request["ocr_car"].ToString().Trim());
			string ocr_outtime = (string.IsNullOrEmpty(Request["ocr_outtime"])) ? "" : Common.FilterCheckMarxString(Request["ocr_outtime"].ToString().Trim());
			string ocr_backtime = (string.IsNullOrEmpty(Request["ocr_backtime"])) ? "" : Common.FilterCheckMarxString(Request["ocr_backtime"].ToString().Trim());

			db._ocr_guid = Guid.NewGuid().ToString("N");
			db._ocr_parentid = pid;
			db._ocr_car = ocr_car;
			if (ocr_outtime != "")
				db._ocr_outtime = DateTime.Parse(ocr_outtime);
			if (ocr_backtime != "")
				db._ocr_backtime = DateTime.Parse(ocr_backtime);
			db._ocr_createid = "Guard";
			db._ocr_createname = "警衛室";
			db._ocr_modid = "Guard";
			db._ocr_modname = "警衛室";
			db.addOfficialCar();

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存成功</Response></root>";
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