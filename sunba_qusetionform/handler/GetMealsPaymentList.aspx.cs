using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsPaymentList : System.Web.UI.Page
{
	MealsRegister_DB mr_db = new MealsRegister_DB();
	MealsVisitor_DB mv_db = new MealsVisitor_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費繳款管理清單
		///說明:
		/// * Request["category"]:類別
		/// * Request["year"]:年
		/// * Request["month"]:月
		/// * Request["empno"]:工號
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
			string month = (string.IsNullOrEmpty(Request["month"])) ? "" : Common.FilterCheckMarxString(Request["month"].ToString().Trim());
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
			int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			DataSet ds = new DataSet();
			DataTable dt = new DataTable();
			switch (category)
			{
				case "Employee":
					mr_db._mr_person_id = SearchStr;
					ds = mr_db.GetPaymentEmployeeList(year, month, pageStart.ToString(), pageEnd.ToString());
					dt = DataEncode(ds.Tables[1]);
					break;
				case "Company":
					mr_db._KeyWord = SearchStr;
					ds = mr_db.GetPaymentCompanyList(year, month, pageStart.ToString(), pageEnd.ToString());
					dt = DataEncode(ds.Tables[1]);
					break;
				case "Visitor":
					mv_db._KeyWord = SearchStr;
					ds = mv_db.GetPaymentVisitorList(year, month, pageStart.ToString(), pageEnd.ToString());
					dt = ds.Tables[1];
					break;
			}
			string xmlstr = string.Empty;
			string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
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


	private DataTable DataEncode(DataTable dt)
	{
		dt.Columns.Add("EncodeID", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["EncodeID"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["mr_person_id"].ToString()));
			}
		}
		return dt;
	}
}