using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DeleteMealsVisitor : System.Web.UI.Page
{
	MealsVisitor_DB db = new MealsVisitor_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 刪除訪客用餐登記
		///說    明:
		/// * Request["gid"]: guid
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

			string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Request["gid"].ToString().Trim();
			gid = Common.Decrypt(Server.UrlDecode(gid));

			db._mv_guid = gid;
			db._mv_modid = LogInfo.empNo;
			db._mv_modname = LogInfo.empName;
			db.DeleteMealsVisitor();

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