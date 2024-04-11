using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetGuardRoomList : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 警衛室清單
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;
			string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Common.FilterCheckMarxString(Request["SortName"].ToString().Trim());
			string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "-" : Common.FilterCheckMarxString(Request["SortMethod"].ToString().Trim());
			SortMethod = (SortMethod == "+") ? "asc" : "desc";

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			string today = DateTime.Now.ToString("yyyy-MM-dd");
			// 公務車出入時間管理
			DataTable dt = db.GetListForGuard(today);
			// 外出單列表
			DataSet ds = db.GetAllListForGuard(today, pageStart.ToString(), pageEnd.ToString(), SortName, SortMethod);

			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "allList", "all_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + xmlstr2 + "</root>";
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