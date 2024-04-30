using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Data;
using System.IO;
using System.Linq;

namespace ED.HR.DOWNLOAD.WebForm
{
    public partial class DownloadImage : System.Web.UI.Page
    {
        string OrgName = string.Empty;
        string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
        FileTable_DB db = new FileTable_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Common.FilterCheckMarxString(Request.QueryString["v"])))
                {
                    if (!string.IsNullOrEmpty(Common.FilterCheckMarxString(Request.QueryString["type"])))
                        db._檔案類型 = Request.QueryString["type"].ToString();
                    if (!string.IsNullOrEmpty(Common.FilterCheckMarxString(Request.QueryString["fsn"])))
                        db._排序 = Request.QueryString["fsn"].ToString();
                    db._guid = Common.Decrypt(Common.FilterCheckMarxString(Request.QueryString["v"]));
                    DataTable dt = db.GetList();
                    if (dt.Rows.Count > 0)
                    {
                        //附件資料夾檔名
                        switch (Common.FilterCheckMarxString(dt.Rows[0]["檔案類型"].ToString()))
                        {
                            case "01":
                                UpLoadPath = UpLoadPath + "question\\";
                                break;
                            case "02":
                                UpLoadPath = UpLoadPath + "reply\\";
                                break;
                        }

                        //原檔名
                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());

                        // 附件目錄
                        DirectoryInfo dir = new DirectoryInfo(UpLoadPath);

                        //列舉全部檔案再比對檔名
                        string FileName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                        FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);

                        // 判斷檔案是否存在
                        if (file != null && file.Exists)
                            Download(file);
                        else
                            throw new Exception("檔案不存在");
                    }
                    else
                    {
                        throw new Exception("檔案不存在");
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error Message: " + ex.Message);
                //Response.End();
            }
        }


        private void Download(FileInfo DownloadFile)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = getMineType(DownloadFile.Extension);
            string DownloadName = (OrgName == "") ? DownloadFile.Name : OrgName;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(DownloadName, System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            Response.HeaderEncoding = System.Text.Encoding.GetEncoding("Big5");
            Response.WriteFile(DownloadFile.FullName);
            Response.Flush();
            Response.End();
        }

        #region 傳回 ContentType
        /// <summary>
        /// 傳回 ContentType
        /// </summary>
        public static string getMineType(string FileExtension)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(FileExtension);
            if (rk != null && rk.GetValue("Content Type") != null)
                return rk.GetValue("Content Type").ToString();
            else
                return "application/octet-stream";
        }
        #endregion
    }
}