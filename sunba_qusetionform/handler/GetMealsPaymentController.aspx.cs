using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsPaymentController : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費繳款管理 Controller
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			DataSet ds = db.GetPaymentYearAndMonthList();

			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "yearList", "year_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "monthList", "month_item");
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