using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetReplyForm : System.Web.UI.Page
{
    Reply_DB db = new Reply_DB();
    QuestionForm_DB qdb = new QuestionForm_DB();
    Competence_DB cdb = new Competence_DB();
    FileTable_DB fdb = new FileTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢回覆表單
        ///說    明:
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dtnew = new DataTable();
        try
        {
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();

            if (type == "list")
            {

            }
            else if (type == "data")
            {
                db._guid = guid;

                dt = db.GetData();

                DataTable file_dt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    fdb._guid = dt.Rows[0]["guid"].ToString();
                    file_dt = DataEncode(fdb.GetList());
                }

                qdb._guid = guid;

                dt2 = qdb.GetData();

                string xmlstr = string.Empty;
                string xmlstr2 = string.Empty;
                string xmlstr3 = string.Empty;
                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(file_dt, "fileList", "file_item");
                xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + xmlstr3 + "<datacount>" + dt.Rows.Count.ToString() + "</datacount></root>";
                xDoc.LoadXml(xmlstr);
            }
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
                dt.Rows[i]["EncodeGuid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["guid"].ToString()));
            }
        }
        return dt;
    }
}