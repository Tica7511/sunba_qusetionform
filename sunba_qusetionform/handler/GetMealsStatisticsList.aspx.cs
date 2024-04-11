using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsStatisticsList : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 同仁用餐統計
		/// * Request["year"]:年
		/// * Request["month"]:月
		/// * Request["SearchStr"]:關鍵字
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
			string month = (string.IsNullOrEmpty(Request["month"])) ? "" : Common.FilterCheckMarxString(Request["month"].ToString().Trim());
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
			int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			DataSet ds = db.GetMealsStatistics(year, month, pageStart.ToString(), pageEnd.ToString());
			string xmlstr = string.Empty;
			string LocationTotalxml = "<locationTotal>" + ds.Tables[0].Rows[0]["LocationTotal"].ToString() + "</locationTotal>";
			string totalxml = "<total>" + ds.Tables[1].Rows[0]["total"].ToString() + "</total>";
			xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[2], "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + LocationTotalxml + totalxml + xmlstr + "</root>";
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