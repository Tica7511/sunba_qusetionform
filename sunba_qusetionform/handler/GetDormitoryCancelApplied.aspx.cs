using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetDormitoryCancelApplied : System.Web.UI.Page
{
	DormitoryCancel_DB db = new DormitoryCancel_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢代碼檔
		///說明:
		/// * Request["group"]: CodeTable Group 代碼
		/// * Request["item"]: CodeTable Item 代碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string xmlstr = string.Empty;

			db._dc_createid = LogInfo.empNo;
			DataTable dt = db.CheckIsApplied();
			if (dt.Rows.Count > 0)
				xmlstr = "Y";
			else
				xmlstr = "N";
			
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + xmlstr + "</Response></root>";
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