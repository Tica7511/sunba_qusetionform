using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_GetReviewToBeSignList : System.Web.UI.Page
{
    Flow_DB db = new Flow_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 待審核清單 - 公文與簽辦
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            DataSet ds = db.GetReviewToBeSignList(LogInfo.empNo);
            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "dataList", "data_item");
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