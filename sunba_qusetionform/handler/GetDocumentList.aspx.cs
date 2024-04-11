using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_GetDocumentList : System.Web.UI.Page
{
	Document_DB db = new Document_DB();
	Competence_DB cdb = new Competence_DB();
	File_DB fdb = new File_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 文件總覽表列表
		///說明:
		/// * Request["SearchStr"]:文件標題關鍵字
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		/// * Request["DocCategory"]:程序書分類
		/// * Request["OrderBy"]:排序欄位
		/// * Request["SortBy"]:排序升降
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
			int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));
			string DocCategory = (string.IsNullOrEmpty(Request["DocCategory"])) ? "" : Common.FilterCheckMarxString(Request["DocCategory"].ToString().Trim());
			string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Common.FilterCheckMarxString(Request["SortName"].ToString().Trim());
			string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "" : Common.FilterCheckMarxString(Request["SortMethod"].ToString().Trim());

			//計算起始與結束
			int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
			int pageStart = pageEnd - PageSize + 1;

			db._KeyWord = SearchStr;
			db._d_category = DocCategory;
			DataSet ds = db.GetList(pageStart.ToString(), pageEnd.ToString(), SortName, SortMethod);

			// 管理者權限
			string Manager = string.Empty;
			if (GetCompetence("07"))
				Manager = "<IsManager>Y</IsManager>";
			else
				Manager = "<IsManager>N</IsManager>";

			string xmlstr = string.Empty;
			string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
			string tmpXml = DataTableToXml.ConvertDatatableToXmlByAttribute(ds.Tables[1], "dataList", "data_item");
			xmlstr = DataToXml(tmpXml);
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + Manager + totalxml + xmlstr + "</root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	private string DataToXml(string xmlstr)
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(xmlstr);

		XmlNodeList xNList = doc.SelectNodes("dataList/data_item");
		if (xNList.Count > 0)
		{
			for (int i = 0; i < xNList.Count; i++)
			{
				fdb._File_Parentid = xNList[i].Attributes["d_guid"].Value;
				DataTable fdt = fdb.GetFileList();
				if (fdt.Rows.Count > 0)
				{
					for (int a = 0; a < fdt.Rows.Count; a++)
					{
						XmlElement xNode = doc.CreateElement("file");
						xNode.SetAttribute("id", Server.UrlEncode(Common.Encrypt(fdt.Rows[a]["File_ID"].ToString().Trim())));
						xNode.SetAttribute("orgname", fdt.Rows[a]["File_Orgname"].ToString().Trim());
						xNode.SetAttribute("newname", fdt.Rows[a]["File_Encryname"].ToString().Trim());
						xNode.SetAttribute("ext", fdt.Rows[a]["File_Exten"].ToString().Trim());
						xNList[i].AppendChild(xNode);
					}
				}
				xNList[i].Attributes["d_guid"].Value = Server.UrlEncode(Common.Encrypt(xNList[i].Attributes["d_guid"].Value));
			}
		}

		return doc.OuterXml;
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