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
        string OrgName = "";
        string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
        File_DB fdb = new File_DB();
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(Common.FilterCheckMarxString(Request.QueryString["v"])))
				{
					fdb._File_ID = Common.Decrypt(Common.FilterCheckMarxString(Request.QueryString["v"]));
					DataTable dt = fdb.GetFileDetail();
					if (dt.Rows.Count > 0)
					{
						//附件資料夾檔名 > File_Type=資料夾名稱
						UpLoadPath = UpLoadPath + dt.Rows[0]["File_Type"].ToString() + "\\";

						//原檔名
						OrgName = Common.FilterCheckMarxString(dt.Rows[0]["File_Orgname"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["File_Exten"].ToString());

						// 附件目錄
						DirectoryInfo dir = new DirectoryInfo(UpLoadPath);

						//列舉全部檔案再比對檔名
						string FileName = Common.FilterCheckMarxString(dt.Rows[0]["File_Encryname"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["File_Exten"].ToString());
						FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);

						// 判斷檔案是否存在
						if (file != null && file.Exists)
							Download(file);
						else
							throw new Exception("File not exist");
					}
					else
					{
						throw new Exception("File not exist");
					}
				}
			}
			catch (Exception ex)
			{
				Response.Write("Error Message: " + ex.Message);
				Response.End();
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
			//忽視之後透過Response.Write輸出的內容
			Response.SuppressContent = true;
			//忽略之後ASP.NET Pipeline的處理步驟，直接跳關到EndRequest
			HttpContext.Current.ApplicationInstance.CompleteRequest();
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