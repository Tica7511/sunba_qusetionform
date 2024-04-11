using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddDocument : System.Web.UI.Page
{
    Document_DB db = new Document_DB();
    File_DB fdb = new File_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 文件總覽表管理
        ///說    明:
        /// * Request["gid"]: GUID
		/// * Request["file_delete_id"]: 需刪除的檔案 ID
        /// * Request["d_pubdate"]: 發行日期
        /// * Request["d_category"]: 文件分類/階層
        /// * Request["d_no"]: 文件/表單編號
        /// * Request["d_name"]: 文件/表單名稱
        /// * Request["d_version"]: 版本
        /// * Request["d_dept"]: 權責部門
        /// * Request["d_manager"]: 權費主管
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmmd = new SqlCommand();
        oCmmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmmd.Transaction = myTrans;
        try
        {
            #region Check Session Timeout
            if (!LogInfo.isLogin)
            {
                throw new Exception("登入帳號已失效，請重新登入");
            }
            #endregion

            string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Common.FilterCheckMarxString(Request["gid"].ToString().Trim());
            string file_delete_id = (string.IsNullOrEmpty(Request["file_delete_id"])) ? "" : Common.FilterCheckMarxString(Request["file_delete_id"].ToString().Trim());
            string d_pubdate = (string.IsNullOrEmpty(Request["d_pubdate"])) ? "" : Common.FilterCheckMarxString(Request["d_pubdate"].ToString().Trim());
            string d_category = (string.IsNullOrEmpty(Request["d_category"])) ? "" : Common.FilterCheckMarxString(Request["d_category"].ToString().Trim());
            string d_no = (string.IsNullOrEmpty(Request["d_no"])) ? "" : Common.FilterCheckMarxString(Request["d_no"].ToString().Trim());
            string d_name = (string.IsNullOrEmpty(Request["d_name"])) ? "" : Common.FilterCheckMarxString(Request["d_name"].ToString().Trim());
            string d_version = (string.IsNullOrEmpty(Request["d_version"])) ? "" : Common.FilterCheckMarxString(Request["d_version"].ToString().Trim());
            string d_dept = (string.IsNullOrEmpty(Request["d_dept"])) ? "" : Common.FilterCheckMarxString(Request["d_dept"].ToString().Trim());
            string d_manager = (string.IsNullOrEmpty(Request["d_manager"])) ? "" : Common.FilterCheckMarxString(Request["d_manager"].ToString().Trim());

            string tmpGuid = (gid == "") ? Guid.NewGuid().ToString("N") : Common.Decrypt(Server.UrlDecode(gid));

            db._d_guid = tmpGuid;
            db._d_pubdate = d_pubdate;
            db._d_category = d_category;
            db._d_no = d_no;
            db._d_name = d_name;
            db._d_version = d_version;
            db._d_dept = d_dept;
            db._d_manager = d_manager;
            db._d_createid = LogInfo.empNo;
            db._d_createname = LogInfo.empName;
            db._d_modid = LogInfo.empNo;
            db._d_modname = LogInfo.empName;

            if (gid == "")
                db.addDocument(oConn, myTrans);
            else
                db.UpdateDocument();

            #region 刪除檔案
            if (file_delete_id != "")
            {
                string[] fid = file_delete_id.Split(',');
                for (int i = 0; i < fid.Length; i++)
                {
                    fdb._File_ID = fid[i];
                    fdb._File_ModId = LogInfo.empNo;
                    fdb.DeleteFile();
                }
            }
            #endregion

            // 檔案上傳
            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Document\\";

                    //副檔名
                    string extension = Path.GetExtension(File.FileName);
                    //原檔名
                    string orgName = Path.GetFileName(File.FileName).Replace(extension, "");
                    //檔案大小
                    string file_size = File.ContentLength.ToString();
                    //取得TIME與GUID
                    string timeguid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N");
                    //儲存的名稱
                    string newName = timeguid + extension;
                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png"
                        || extension.ToLower() == ".pdf" || extension.ToLower() == ".ppt" || extension.ToLower() == ".pptx"
                        || extension.ToLower() == ".doc" || extension.ToLower() == ".docx"
                        || extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
                    {
                        File.SaveAs(UpLoadPath + newName);
                        // 進資料庫前, 儲存名稱要去除副檔名
                        newName = newName.Replace(extension, "");

                        fdb._File_Parentid = tmpGuid;
                        fdb._File_Type = "Document";
                        fdb._File_Orgname = orgName;
                        fdb._File_Encryname = newName;
                        fdb._File_Exten = extension;
                        fdb._File_Size = file_size;
                        fdb._File_CreateId = LogInfo.empNo;
                        fdb._File_ModId = LogInfo.empName;
                        fdb.AddFile_Trans(oConn, myTrans);
                    }
                    else
                    {
                        throw new Exception("檔案格式限制: .jpg .jpeg .png .pdf .doc .docx .xls .xlsx .ppt .ppt");
                    }
                }
            }

            myTrans.Commit();

            string xmlstr = string.Empty;
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>完成</Response></root>";
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