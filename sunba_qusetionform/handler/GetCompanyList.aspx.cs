using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetCompanyList : System.Web.UI.Page
{
    SSO_DB db = new SSO_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢部門列表
        ///說    明:
        /// * Request["type]: orgnization=部門列表 empname=員工列表
        /// * Request["orgnization]: 部門
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        DataTable dt = new DataTable();
        try
        {
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string orgnization = (string.IsNullOrEmpty(Request["orgnization"])) ? "" : Request["orgnization"].ToString().Trim();

            if (type == "orgnization")
            {
                dt = db.GetOrgnizationList();
            }
            else
            {
                db._GROUP_ID = orgnization;
                dt = db.GetEmpNameList();
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