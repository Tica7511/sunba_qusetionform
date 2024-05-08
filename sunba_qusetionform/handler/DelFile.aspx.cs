using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_DelFile : System.Web.UI.Page
{
    FileTable_DB db = new FileTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 檔案
		///說    明:
		/// * Request["ftype"]: 檔案類型
		/// * Request["guid:"]: guid
		/// * Request["fsn:"]: 排序
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

            string ftype = (string.IsNullOrEmpty(Request["ftype"])) ? "" : Request["ftype"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string fsn = (string.IsNullOrEmpty(Request["fsn"])) ? "" : Request["fsn"].ToString().Trim();

            string xmlstr = string.Empty;
            db._檔案類型 = ftype;
            db._guid = guid;
            db._排序 = fsn;
            db.DeleteData();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除成功</Response></root>";
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