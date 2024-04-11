using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetDormitoryDetail : System.Web.UI.Page
{
	Dormitory_DB db = new Dormitory_DB();
	File_DB fdb = new File_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 宿舍申請詳細資料
		///說明:
		/// * Request["id"]: data id
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
			id = Common.Decrypt(id);

			// 宿舍
			db._d_id = id;
			DataTable dt = db.GetDormitoryDetail();

			// 附件
			DataTable fdt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				fdb._File_Parentid = dt.Rows[0]["d_guid"].ToString();
				fdt = fdb.GetFileList();
				fdt = DataEncode(fdt);
			}

			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(fdt, "fileList", "file_item");
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
		dt.Columns.Add("EncodeGuid", typeof(string));
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i]["EncodeGuid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["File_ID"].ToString()));
			}
		}
		return dt;
	}
}