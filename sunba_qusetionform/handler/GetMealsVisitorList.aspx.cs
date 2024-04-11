using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetMealsVisitorList : System.Web.UI.Page
{
	MealsVisitor_DB db = new MealsVisitor_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 訪客用餐登記列表
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (Request["SearchStr"] != null) ? Request["SearchStr"].ToString().Trim() : "";
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			db._KeyWord = SearchStr;
			DataSet ds = db.GetManageList(pageStart.ToString(), pageEnd.ToString());
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
		dt.Columns.Add("egid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["mv_guid"].ToString()));
			}
		}
		return dt;
	}
}