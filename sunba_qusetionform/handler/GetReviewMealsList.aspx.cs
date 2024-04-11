using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetReviewMealsList : System.Web.UI.Page
{
	Flow_DB db = new Flow_DB();
	Competence_DB cdb = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核清單 - 用餐登記列表
		///說    明:
		/// * Request["Category"]:功能類別
		/// * Request["SearchStr"]:關鍵字
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string Category = (string.IsNullOrEmpty(Request["Category"])) ? "" : Common.FilterCheckMarxString(Request["Category"].ToString().Trim());
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
			int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));

			// 計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			DataSet ds = new DataSet();
			{
				if (Category == "Visitor")
				{
					if (GetCompetence("01"))
						ds = db.GetReviewMealsVisitorList(pageStart.ToString(), pageEnd.ToString());
				}
				else
				{
					if (GetCompetence("02"))
						ds = db.GetReviewMealsCancelList(pageStart.ToString(), pageEnd.ToString());
				}
			}

			DataTable dt = new DataTable();
			string xmlstr = "<?xml version='1.0' encoding='utf-8'?><root></root>";
			if (ds.Tables.Count > 0)
			{
				dt = DataEncode(ds.Tables[1]);
				string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
			}

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
		dt.Columns.Add("egid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["fms_guid"].ToString()));
			}
		}
		return dt;
	}

	private bool GetCompetence(string type)
	{
		bool status = false;
		cdb._c_type = type;
		DataTable dt = cdb.GetCompetenceList_Common();
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (LogInfo.empNo == dt.Rows[i]["value"].ToString().Trim())
					status = true;
			}
		}
		return status;
	}
}