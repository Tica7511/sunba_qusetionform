using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsCostItemList : System.Web.UI.Page
{
	MealsCostItem_DB db = new MealsCostItem_DB();
	MealsCostFood_DB mcf_db = new MealsCostFood_DB();
	MealsCostCompany_DB mcc_db = new MealsCostCompany_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 成本項目清單
		/// * Request["parentid"]: 來源Guid
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string parentid = (string.IsNullOrEmpty(Request["parentid"])) ? "" : Common.FilterCheckMarxString(Request["parentid"].ToString().Trim());

			db._mci_parentid = parentid;
			DataTable dt = db.GetList();
			DataTable dt2 = mcf_db.GetSelectList();
			DataTable dt3 = mcc_db.GetSelectList();
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			string xmlstr3 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "mcfList", "mcf_item");
			xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "mccList", "mcc_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + xmlstr3 + "</root>";
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