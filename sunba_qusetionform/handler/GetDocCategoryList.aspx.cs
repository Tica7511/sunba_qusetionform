using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_GetDocCategoryList : System.Web.UI.Page
{
    DocCategory_DB db = new DocCategory_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 文件分類/階層管理列表
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            DataTable ds = db.GetList();
            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(ds, "dataList", "data_item");
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