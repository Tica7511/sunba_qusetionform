using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetReviewMeetingFeedBack : System.Web.UI.Page
{
	MeetingFeedBack_DB db = new MeetingFeedBack_DB();
	Competence_DB cdb = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 待審核清單-會議室狀況回覆列表
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

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			string xmlstr = "<?xml version='1.0' encoding='utf-8'?><root></root>";
			if (GetCompetence())
			{
				db._KeyWord = SearchStr;
				DataSet ds = db.GetReviewList(pageStart.ToString(), pageEnd.ToString());
				string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
				xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
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

	private bool GetCompetence()
	{
		bool status = false;
		cdb._c_type = "06";
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