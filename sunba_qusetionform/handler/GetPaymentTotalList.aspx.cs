using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetPaymentTotalList : System.Web.UI.Page
{
	MealsRegister_DB mr_db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費統計總表
		///說明:
		/// * Request["year"]:年
		/// * Request["month"]:月
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
			string month = (string.IsNullOrEmpty(Request["month"])) ? "" : Common.FilterCheckMarxString(Request["month"].ToString().Trim());
			
			DataTable dt = mr_db.GetPaymentTotalList(year, month);

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