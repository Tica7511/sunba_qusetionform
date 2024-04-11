using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DeleteDocument : System.Web.UI.Page
{
    Document_DB db = new Document_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除文件文件總覽表
        ///說    明:
        /// * Request["id"]: id
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            #region Check Session Timeout
            if (!LogInfo.isLogin)
            {
                throw new Exception("登入帳號已失效，請重新登入");
            }
            #endregion

            string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Request["id"].ToString().Trim();

            db._d_id = id;
            db._d_modid = LogInfo.empNo;
            db._d_modname = LogInfo.empName;
            db.DeleteDocument();

            string xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
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