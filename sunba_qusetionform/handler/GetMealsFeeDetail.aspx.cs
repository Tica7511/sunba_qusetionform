using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsFeeDetail : System.Web.UI.Page
{
	MealsFee_DB db = new MealsFee_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費資訊
		///說    明:
		/// * Request["id"]: ID
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());

			db._mf_id = id;
			DataTable dt = db.GetDetail();
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