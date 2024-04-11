using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetUnreviewedCount : System.Web.UI.Page
{
	Flow_DB db = new Flow_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核清單 - 未審核通知數
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			// 公文與簽辦 UOF
			DataSet ds = db.GetReviewToBeSignList(LogInfo.empNo);
			// 其他
			DataTable dt = db.GetUnreviewed();
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "UOFList", "uof_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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