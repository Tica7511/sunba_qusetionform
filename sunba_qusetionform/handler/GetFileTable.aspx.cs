using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetFileTable : System.Web.UI.Page
{
    FileTable_DB db = new FileTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢 檔案列表
		///說    明:
        /// * Request["guid"]: guid
        /// * Request["ftype"]: 檔案類型
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        DataTable dt = new DataTable();
        try
        {
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string ftype = (string.IsNullOrEmpty(Request["ftype"])) ? "" : Request["ftype"].ToString().Trim();

            db._guid = guid;
            db._檔案類型 = ftype;
            dt = db.GetList();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("EncodeGuid", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["EncodeGuid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["guid"].ToString()));
                }
            }

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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