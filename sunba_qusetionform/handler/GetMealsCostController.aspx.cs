using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsCostController : System.Web.UI.Page
{
	MealsCost_DB mc_db = new MealsCost_DB();
	MealsCostFood_DB mcf_db = new MealsCostFood_DB();
	MealsCostCompany_DB mcc_db = new MealsCostCompany_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 成本管理 Controller
		/// * Request["mode"]: 事件控制項目
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Common.FilterCheckMarxString(Request["mode"].ToString().Trim());

			DataTable dt = new DataTable();

			switch (mode)
			{
				case "CostFoodSelectList":
					dt = mcf_db.GetSelectList();
					break;
				case "CostCompanySelectList":
					dt = mcc_db.GetSelectList();
					break;
				case "StatisticsTable":
					dt = mc_db.GetStatisticsTable();
					break;
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