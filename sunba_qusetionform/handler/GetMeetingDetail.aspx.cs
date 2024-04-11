using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMeetingDetail : System.Web.UI.Page
{
	Meeting_DB db = new Meeting_DB();
	protected void Page_Load(object sender, EventArgs e)
	{

		///-----------------------------------------------------
		///功    能: 會議室申請詳細資料
		///說明:
		/// * Request["id"]: data id
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Request["id"].ToString().Trim();
			id = Common.Decrypt(id);
			
			db._m_id = id;
			DataTable dt = db.GetMeetingDetail();

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