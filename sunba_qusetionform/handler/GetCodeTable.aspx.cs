using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetCodeTable : System.Web.UI.Page
{
	CodeTable_DB db = new CodeTable_DB();
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
			string group = (string.IsNullOrEmpty(Request["group"])) ? "" : Request["group"].ToString().Trim();
			string item = (string.IsNullOrEmpty(Request["item"])) ? "" : Request["item"].ToString().Trim();

			db._C_Group = group;
			db._C_Item = item;
			DataTable dt = db.GetCode();

			string xmlstr = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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