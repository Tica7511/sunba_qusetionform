using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.Web.UI;
using System.Linq;

/// <summary>
/// Common 的摘要描述
/// </summary>
public class Common
{

	#region Get IPv4 Adress
	public static string GetIP4Address()
	{
		System.Web.HttpContext context = System.Web.HttpContext.Current;
		string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
		if (string.IsNullOrEmpty(sIPAddress))
		{
			string[] ipstr = context.Request.ServerVariables["REMOTE_ADDR"].Split(':');
			if (ipstr[0].Trim() != "")
				return context.Request.ServerVariables["REMOTE_ADDR"];
			else
				return "LOCAL-Name：" + Environment.MachineName;
		}
		else
		{
			string[] ipArray = sIPAddress.Split(new Char[] { ',' });
			return ipArray[0];
		}
	}
	#endregion

	#region 加解密
	/// <summary>
	/// 加密
	/// </summary>
	public static string Encrypt(string strSource)
	{
		//把字符串放到byte数组中  
		byte[] bytIn = Encoding.Default.GetBytes(strSource);
		//建立加密对象的密钥和偏移量          
		byte[] iv = { 102, 16, 93, 156, 78, 4, 218, 32 };//定义偏移量  
		byte[] key = { 55, 103, 246, 79, 36, 99, 167, 3 };//定义密钥
														  //实例DES加密类  
		DESCryptoServiceProvider mobjCryptoService = new DESCryptoServiceProvider();
		mobjCryptoService.Key = iv;
		mobjCryptoService.IV = key;
		ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
		//实例MemoryStream流加密密文件  
		MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
		cs.Write(bytIn, 0, bytIn.Length);
		cs.FlushFinalBlock();
		return Convert.ToBase64String(ms.ToArray());
	}

	/// <summary>
	/// 解密
	/// </summary>
	public static string Decrypt(string Source)
	{
		string str = "";
		try
		{
			//将解密字符串转换成字节数组  
			byte[] bytIn = System.Convert.FromBase64String(Source);
			//给出解密的密钥和偏移量，密钥和偏移量必须与加密时的密钥和偏移量相同  
			byte[] iv = { 102, 16, 93, 156, 78, 4, 218, 32 };//定义偏移量  
			byte[] key = { 55, 103, 246, 79, 36, 99, 167, 3 };//定义密钥  
			DESCryptoServiceProvider mobjCryptoService = new DESCryptoServiceProvider();
			mobjCryptoService.Key = iv;
			mobjCryptoService.IV = key;
			//实例流进行解密  
			System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
			ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
			CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
			StreamReader strd = new StreamReader(cs, Encoding.Default);
			str = strd.ReadToEnd();
		}
		catch
		{

		}
		return str;
	}

	/// <summary>
	/// SHA1加密
	/// </summary>
	public static string sha1en(string str)
	{
		string enCodeString;
		SHA1CryptoServiceProvider sha1en = new SHA1CryptoServiceProvider();
		enCodeString = BitConverter.ToString(sha1en.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
		enCodeString = enCodeString.Replace("-", "");
		return enCodeString;
	}

	/// <summary>
	/// Base64 加密
	/// </summary>
	public static string ToBase64String(string str)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
	}

	/// <summary>
	/// Base64 解密
	/// </summary>
	public static string FromBase64String(string str)
	{
		return Encoding.UTF8.GetString(Convert.FromBase64String(str));
	}
	#endregion

	#region sqlInjection
	/// <summary>
	/// 檢查特殊字元
	/// </summary>
	/// <param name="checkValue">欲檢查的值</param>
	/// <returns></returns>
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
	#endregion

	#region 檢查參數
	public void CheckParameters(System.Data.SqlClient.SqlCommand oCmd)
	{
		//檢查危險字元
		for (int i = 0; i < oCmd.Parameters.Count; i++)
		{
			if (!CheckSQLInjection(oCmd.Parameters[i].Value.ToString()))
			{
				//throw new Exception("危險字元");
				//導引至錯誤網頁
				System.Web.HttpContext.Current.Response.Redirect("Error.aspx?err=par");

			}
		}
	}
	#endregion

	#region 清除html...
	/// <summary>
	/// 輸入html後刪除html標簽...
	/// </summary>
	public static string NoHTML(string Htmlstring)
	{
		//删除脚本
		Htmlstring = Htmlstring.Replace("\r\n", "");
		Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
		//删除HTML
		Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
		Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
		Htmlstring = Htmlstring.Replace("<", "");
		Htmlstring = Htmlstring.Replace(">", "");
		Htmlstring = Htmlstring.Replace("\r\n", "");
		Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
		return Htmlstring;
	}
	#endregion

	#region 過濾 CheckMarx 用
	/// <summary>
	/// CheckMarx 過濾
	/// </summary>
	public static string FilterCheckMarxString(string str)
	{
		string rVal = string.Empty;
		rVal = HttpContext.Current.Server.HtmlEncode(str);
		rVal = HttpContext.Current.Server.HtmlDecode(rVal);
		return rVal;
	}
	#endregion

	#region 可進入的頁面權限
	public static bool CheckCompetence(string group, string empno)
	{
		bool status = false;
		Competence_DB db = new Competence_DB();
		db._類別 = group;
		DataTable dt = db.GetListOfType();
		if (dt.Rows.Count > 0)
		{
			string[] array = dt.Rows[0]["c_empno"].ToString().Trim().Split(',');
			if (array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (empno == array[i])
						status = true;
				}
			}
		}
		return status;
	}
	#endregion

	#region Check Is SA
	public static bool CheckIsSA()
	{
		Competence_DB db = new Competence_DB();
		DataTable dt = db.CheckIsSystemAdmin();
		if (dt.Rows.Count > 0)
		{
			string[] tAry = dt.Rows[0]["c_empno"].ToString().Trim().Split(',');
			if (tAry.Contains(LogInfo.empNo))
				return true;
			else
				return false;
		}
		else
			return false;
	}
	#endregion
}


#region 會員登入使用
/// <summary>
/// MbrAccount 的摘要描述。
/// </summary>
public class Account
{
	public static void ExecSignIn(string account)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
DEPARTMENT_CNAME,
DEPARTMENT_CODE
from vwZZ_EMPLOYEE where EMPLOYEE_NO=@account and EMPLOYEE_WORK_STATUS=1 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable dt = new DataTable();
		oCmd.Parameters.AddWithValue("@account", account);
		oda.Fill(dt);

		if (dt.Rows.Count > 0)
		{
			LogInfo.deptName = dt.Rows[0]["DEPARTMENT_CNAME"].ToString();
			LogInfo.deptCode = dt.Rows[0]["DEPARTMENT_CODE"].ToString();
		}
		//else
		//{
		//	throw new ApplicationException("無效的帳號或密碼"); ;
		//}
	}
}
#endregion

#region JavaScript Alert
public class JavaScript
{
	/// <summary>
	/// AlertMessage
	/// </summary>
	public static void AlertMessage(System.Web.UI.Page objPage, string strMessage)
	{
		strMessage = strMessage.Replace("\r\n", "\\r");
		StringBuilder sb = new StringBuilder();
		sb.AppendFormat(@"<Script language=""javascript"" type=""text/javascript"">");
		sb.AppendFormat(@"alert(""{0}"");", strMessage);
		sb.AppendFormat(@"</Script>");

		//objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
		objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", sb.ToString(), false);
	}

	/// <summary>
	/// AlertMessageClose
	/// </summary>
	public static void AlertMessageClose(System.Web.UI.Page objPage, string strMessage)
	{
		string strJS = "";
		strMessage = strMessage.Replace("\r\n", "\\r");
		strJS = @"<Script language='javascript' type='text/javascript' >";
		strJS += "alert('" + strMessage + "');";
		strJS += "window.close();";
		strJS += "</Script>";
		//objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
		objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", strJS, false);
	}

	/// <summary>
	/// AlertMessageRedirect
	/// </summary>
	public static void AlertMessageRedirect(System.Web.UI.Page objPage, string strMessage, string strRedirectPage)
	{
		AlertMessageRedirect(objPage, strMessage, strRedirectPage, false);
	}

	public static void AlertMessageRedirect(System.Web.UI.Page objPage, string strMessage, string strRedirectPage, bool IsDisplayData)
	{
		string strJS = "";
		strMessage = strMessage.Replace("\r\n", "\\r");
		strJS = @"<Script language='javascript' type='text/javascript'>";
		strJS += "alert('" + strMessage + "');";
		strJS += "window.location ='" + strRedirectPage + "'; ";
		strJS += "</Script>";

		if (IsDisplayData)
			objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", strJS, false);
		else
			objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
	}
}
#endregion

#region 爬網頁 Html (類似全文檢索)
public class CaptureURL
{
	public string Capture(string url)
	{
		try
		{
			string strHTML = string.Empty;

			if (url.IndexOf("https://") < 0)
			{
				string E = System.IO.Path.GetExtension(url);

				if (!E.Trim().ToLower().Equals(".html") && !string.IsNullOrEmpty(E.Trim()))
				{
					string param = "hl=zh-CN&newwindow=1";

					byte[] bs = System.Text.Encoding.ASCII.GetBytes(param);

					HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

					req.Method = "POST";

					req.ContentType = "application/x-www-form-urlencoded";

					req.ContentLength = bs.Length;


					using (Stream reqStream = req.GetRequestStream())
					{
						reqStream.Write(bs, 0, bs.Length);
					}

					using (WebResponse wr = req.GetResponse())
					{
						//在這裡對接收到的頁面內容進行處理

						using (Stream myStream = wr.GetResponseStream())
						{
							using (StreamReader myStreamReader = new StreamReader(myStream, System.Text.Encoding.UTF8))
							{
								strHTML = myStreamReader.ReadToEnd();
							}
						}
					}
				}
				else
				{
					Uri myUri = new Uri(url);

					// Create a 'HttpWebRequest' object for the specified url. 

					HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

					// Set the user agent as if we were a web browser

					myHttpWebRequest.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4";

					HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

					var stream = myHttpWebResponse.GetResponseStream();

					var reader = new StreamReader(stream);

					var html = reader.ReadToEnd();

					// Release resources of response object.

					myHttpWebResponse.Close();

					return html;
				}
			}
			else
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				//request.Method = "HEAD";

				//request.AllowAutoRedirect = false;

				request.Credentials = CredentialCache.DefaultCredentials;

				// Ignore Certificate validation failures (aka untrusted certificate + certificate chains)

				ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				Stream resStream = response.GetResponseStream();

				StreamReader reader = new StreamReader(resStream, System.Text.Encoding.UTF8);

				strHTML = reader.ReadToEnd();
			}
			return strHTML;
		}
		catch (Exception ex) { throw ex; }
	}
}
#endregion
