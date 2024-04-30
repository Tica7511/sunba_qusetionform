using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetCodeTable : System.Web.UI.Page
{
	CodeTable_DB db = new CodeTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢代碼檔列表
		///說    明:
        /// * Request["gNo"]: 群組代碼
        /// * Request["relation"]: 項目代碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        DataTable dt = new DataTable();
        try
        {
            string gNo = (string.IsNullOrEmpty(Request["gNo"])) ? "" : Request["gNo"].ToString().Trim();
            string relation = (string.IsNullOrEmpty(Request["relation"])) ? "" : Request["relation"].ToString().Trim();
            string department = (string.IsNullOrEmpty(Request["department"])) ? "" : Request["department"].ToString().Trim();

            db._群組代碼 = gNo;
            db._項目代碼 = relation;
            dt = db.GetList();

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