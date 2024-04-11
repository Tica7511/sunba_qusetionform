using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetOutdoorCalendar : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	OfficialCar_DB oc_db = new OfficialCar_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 公務車使用狀況 Calendar
		///說明:
		/// * Request["mode"]: 資料類別
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();

			DataTable dt = new DataTable();
			if (mode == "car")
			{
				dt = oc_db.GetList();
			}
			else
			{
				dt = db.GetOutdoorCalendarList();
			}

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