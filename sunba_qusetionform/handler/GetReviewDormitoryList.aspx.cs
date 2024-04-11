using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetReviewDormitoryList : System.Web.UI.Page
{
	Flow_DB db = new Flow_DB();
	Competence_DB cdb = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核清單 - 宿舍申請列表
		///說明:
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

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			DataSet ds = new DataSet();
			DataTable dt = new DataTable();
			string xmlstr = "<?xml version='1.0' encoding='utf-8'?><root></root>";
			if (GetCompetence())
			{
				if (Category == "Apply")
				{
					ds = db.GetReviewDormitoryApplyList(pageStart.ToString(), pageEnd.ToString());
					dt = DataEncode(ds.Tables[1]);
				}
				else
				{
					ds = db.GetReviewDormitoryCancelList(pageStart.ToString(), pageEnd.ToString());
					dt = ds.Tables[1];
				}

				string nowLogin = "<LoginEmpno>" + LogInfo.empNo + "</LoginEmpno>";
				string saxml = "<sa>" + Common.CheckIsSA() + "</sa>";
				string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + nowLogin + saxml + totalxml + xmlstr + "</root>";
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
		dt.Columns.Add("egid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["edataid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["d_id"].ToString()));
				dt.Rows[i]["egid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["fms_guid"].ToString()));
			}
		}
		return dt;
	}

	private bool GetCompetence()
	{
		bool status = false;
		DataTable dt = cdb.GetReviewDormitoryCompetenceList();
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