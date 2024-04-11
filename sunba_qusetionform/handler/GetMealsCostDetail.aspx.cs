 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsCostDetail : System.Web.UI.Page
{
	MealsCost_DB main_db = new MealsCost_DB();
	MealsCostItem_DB item_db = new MealsCostItem_DB();
	File_DB fdb = new File_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 成本項目資訊
		///說    明:
		/// * Request["gid"]: GUID
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Common.FilterCheckMarxString(Request["gid"].ToString().Trim());
			gid = Common.Decrypt(Server.UrlDecode(gid));

			main_db._mc_guid = gid;
			DataTable dt = main_db.GetDetail();

			// 附件
			DataTable file_dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				fdb._File_Parentid = dt.Rows[0]["mc_guid"].ToString();
				file_dt = DataEncode(fdb.GetFileList());
			}
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(file_dt, "fileList", "file_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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
				dt.Rows[i]["EncodeID"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["File_ID"].ToString()));
			}
		}
		return dt;
	}
}