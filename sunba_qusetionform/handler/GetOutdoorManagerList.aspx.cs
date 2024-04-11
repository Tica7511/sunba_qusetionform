using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetOutdoorManagerList : System.Web.UI.Page
{

	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 外出單查詢管理清單
		///說明:
		/// * Request["SearchStr"]:關鍵字
		/// * Request["SearchType"]:申請類別
		/// * Request["SearchCarNo"]:車號
		/// * Request["SearchStartDate"]:預計離廠日
		/// * Request["SearchEndDate"]:預計返廠日
		/// * Request["SearchActualOut"]:實際離廠日
		/// * Request["SearchActualBack"]:實際返廠日
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (Request["SearchStr"] != null) ? Request["SearchStr"].ToString().Trim() : "";
			string SearchType = (Request["SearchType"] != null) ? Request["SearchType"].ToString().Trim() : "";
			string SearchCarNo = (Request["SearchCarNo"] != null) ? Request["SearchCarNo"].ToString().Trim() : "";
			string SearchStartDate = (Request["SearchStartDate"] != null) ? Request["SearchStartDate"].ToString().Trim() : "";
			string SearchEndDate = (Request["SearchEndDate"] != null) ? Request["SearchEndDate"].ToString().Trim() : "";
			string SearchActualOut = (Request["SearchActualOut"] != null) ? Request["SearchActualOut"].ToString().Trim() : "";
			string SearchActualBack = (Request["SearchActualBack"] != null) ? Request["SearchActualBack"].ToString().Trim() : "";
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			db._KeyWord = SearchStr;
			db._o_type = SearchType;
			DataSet ds = db.GetManagerList(SearchCarNo, SearchStartDate, SearchEndDate, SearchActualOut, SearchActualBack, pageStart.ToString(), pageEnd.ToString());
			DataTable dt = DataEncode(ds.Tables[1]);
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
		dt.Columns.Add("eid", typeof(string));
		dt.Columns.Add("egid", typeof(string));
		dt.Columns.Add("deid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["eid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_id"].ToString()));
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_guid"].ToString()));
				dt.Rows[i]["deid"] = Server.UrlEncode(Common.ToBase64String(dt.Rows[i]["o_id"].ToString()));
			}
		}
		return dt;
	}
}