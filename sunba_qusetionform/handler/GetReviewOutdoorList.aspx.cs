using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetReviewOutdoorList : System.Web.UI.Page
{
	Flow_DB db = new Flow_DB();
	Competence_DB cdb = new Competence_DB();
	Personnel_DB p_db = new Personnel_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核清單 - 出廠證明單列表
		///說    明:
		/// * Request["SearchStr"]:關鍵字
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
			int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));
			string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Common.FilterCheckMarxString(Request["SortName"].ToString().Trim());
			string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "-" : Common.FilterCheckMarxString(Request["SortMethod"].ToString().Trim());
			SortMethod = (SortMethod == "+") ? "asc" : "desc";

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			DataSet ds = new DataSet();
			if (GetCompetence("03"))
				db._fms_signperson = string.Empty;
			else
				db._fms_signperson = LogInfo.empNo;

			ds = db.GetReviewOutdoorList(pageStart.ToString(), pageEnd.ToString(), SortName, SortMethod);

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
		dt.Columns.Add("edataid", typeof(string));
		dt.Columns.Add("edatagid", typeof(string));
		dt.Columns.Add("Del_id", typeof(string));
		dt.Columns.Add("egid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["edataid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_id"].ToString()));
				dt.Rows[i]["edatagid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["o_guid"].ToString()));
				dt.Rows[i]["Del_id"] = Server.UrlEncode(Common.ToBase64String(dt.Rows[i]["o_id"].ToString()));
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["fms_guid"].ToString()));
			}
		}
		dt.Columns.Remove("fms_guid");
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