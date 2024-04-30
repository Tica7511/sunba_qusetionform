using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

public partial class tinymce_ImageUpload_filedownload : System.Web.UI.Page
{
	string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "tinymce\\";
	protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string FileName = (Request["v"] != null) ? Request["v"].ToString().Trim() : "";

            #region 檢查 SQL Injection
            if (!CheckSQLInjection(FileName))
            {
                throw new Exception("請勿輸入非法字元");
            }
            #endregion

			// 附件目錄
			DirectoryInfo dir = new DirectoryInfo(UpLoadPath);

			//列舉全部檔案再比對檔名
			FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);

            if (file.Exists)
			{
				Response.ContentType = getContentType(file.Extension); ;
                string strDownloadName = string.Empty;

                if (Request.Browser.Browser == "InternetExplorer")
                {
                    Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    strDownloadName = Request["v"];
                }
                else
                {
                    strDownloadName = HttpUtility.UrlEncode(Request["v"]);
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + strDownloadName);
				Response.AppendHeader("Content-Length", file.Length.ToString());
				Response.WriteFile(file.FullName);
                Response.Flush();
                Response.End();
            }
        }
        catch(Exception ex)
        {
            //Response.Write(ex.Message);
            //Response.End();
        }
    }

	#region 傳回 ContentType
	/// <summary>
	/// 傳回 ContentType
	/// </summary>
	public static string getContentType(string FileExtension)
	{
		Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(FileExtension);
		if (rk != null && rk.GetValue("Content Type") != null)
			return rk.GetValue("Content Type").ToString();
		else
			return "application/octet-stream";
	}
	#endregion

	public static bool CheckSQLInjection(string checkValue)
    {
        //「%27」:「'」(單引號)
        //「%2B」:「+」(加號)
        //「%3D」:「=」(等號)
        //「%7C」:「|」(｜)
        //「ALERT(」
        //「--」
        //「%2F*」:「/*」
        //「*%2F」:「*/」
        //「%26」:「&」
        //「%40」:「@」
        //「%25」:「%」
        //「%3B」:「;」
        //「%24」:「$」
        //「%26」:「*」
        //「%22」:「"」
        //「%2C」:「,」
        //「%2f」:「/」
        //「%5c」:「\」
        //「%22」:「"」
        //「%3C」:「<」
        //「%3E」:「>」

        if (checkValue.Length > 0 && (checkValue.ToUpper().IndexOf("%27") >= 0 || checkValue.ToUpper().IndexOf("%2B") >= 0
          || checkValue.ToUpper().IndexOf("'") >= 0) || checkValue.ToUpper().IndexOf("ALERT(") >= 0
          || checkValue.ToUpper().IndexOf("%3C") >= 0 || checkValue.ToUpper().IndexOf("%3E") >= 0
          || checkValue.ToUpper().IndexOf("%3D") >= 0 || checkValue.ToUpper().IndexOf("=") >= 0
          || checkValue.ToUpper().IndexOf("--") >= 0 || checkValue.ToUpper().IndexOf("%7C") >= 0
          || checkValue.ToUpper().IndexOf("%2F*") >= 0 || checkValue.ToUpper().IndexOf("*%2F") >= 0
          || checkValue.ToUpper().IndexOf("%26") >= 0
          || checkValue.ToUpper().IndexOf("%25") >= 0 || checkValue.ToUpper().IndexOf("%3B") >= 0
          || checkValue.ToUpper().IndexOf("%24") >= 0 || checkValue.ToUpper().IndexOf("*") >= 0
          || checkValue.ToUpper().IndexOf("%22") >= 0 || checkValue.ToUpper().IndexOf("%2C") >= 0
          || checkValue.ToUpper().IndexOf("%2F") >= 0 || checkValue.ToUpper().IndexOf("%5C") >= 0
          || checkValue.ToUpper().IndexOf("%40") >= 0
          || checkValue.ToUpper().IndexOf("../") >= 0 || checkValue.ToUpper().IndexOf("%") >= 0 || checkValue.ToUpper().IndexOf("@") >= 0
          || checkValue.ToUpper().IndexOf("&") >= 0 || checkValue.ToUpper().IndexOf("..\\") >= 0 || checkValue.ToUpper().IndexOf("$") >= 0
          || checkValue.ToUpper().IndexOf("?") >= 0
          )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}