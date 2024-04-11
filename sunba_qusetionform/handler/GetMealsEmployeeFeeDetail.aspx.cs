using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsEmployeeFeeDetail : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費統計明細
		///說明:
		/// * Request["year"]: 年
		/// * Request["month"]: 月
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
			string month = (string.IsNullOrEmpty(Request["month"])) ? "" : Common.FilterCheckMarxString(Request["month"].ToString().Trim());
			string person_id = (string.IsNullOrEmpty(Request["person_id"])) ? "" : Common.FilterCheckMarxString(Request["person_id"].ToString().Trim());

			string id = (person_id == "") ? LogInfo.empNo : Common.Decrypt(Server.UrlDecode(person_id));

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;
			
			db._mr_person_id = id;
			DataSet ds = db.GetEmployeeFeeDetail(year, month,pageStart.ToString(), pageEnd.ToString());

			string xmlstr = string.Empty;
			string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
			xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
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