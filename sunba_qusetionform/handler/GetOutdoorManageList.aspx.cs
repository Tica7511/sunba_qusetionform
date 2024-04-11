using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetOutdoorManageList : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 出廠證明單申請清單
		///說明:
		/// * Request["SearchStr"]:關鍵字
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (Request["SearchStr"] != null) ? Request["SearchStr"].ToString().Trim() : "";
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;
			string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Common.FilterCheckMarxString(Request["SortName"].ToString().Trim());
			string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "-" : Common.FilterCheckMarxString(Request["SortMethod"].ToString().Trim());
			SortMethod = (SortMethod == "+") ? "asc" : "desc";

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			db._KeyWord = SearchStr;
			DataSet ds = db.GetManageList(pageStart.ToString(), pageEnd.ToString(), SortName, SortMethod);
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
		dt.Columns.Add("Del_id", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["eid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_id"].ToString()));
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_guid"].ToString()));
				dt.Rows[i]["Del_id"] = Server.UrlEncode(Common.ToBase64String(dt.Rows[i]["o_id"].ToString()));
			}
		}
		return dt;
	}
}