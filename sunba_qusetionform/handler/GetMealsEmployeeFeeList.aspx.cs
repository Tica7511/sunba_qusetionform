using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsEmployeeFeeList : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費統計
		///說明:
		/// * Request["year"]: 年
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
			year = (year == "") ? DateTime.Now.ToString("yyyy") : year;

			db._mr_date = year;
			db._mr_person_id = LogInfo.empNo;
			DataSet ds = db.GetEmployeeFeeList();
			
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "yearList", "year_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
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