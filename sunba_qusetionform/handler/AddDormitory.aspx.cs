using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

public partial class handler_AddDormitory : System.Web.UI.Page
{
	Dormitory_DB db = new Dormitory_DB();
	File_DB fdb = new File_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 宿舍申請
		///說    明:
		/// * Request["id"]: ID
		/// * Request["gid"]: Guid
		/// * Request["d_type"]: 類別
		/// * Request["d_startday"]: 短期入住開始日期
		/// * Request["d_endday"]: 短期入住結束日期
		/// * Request["d_reason"]: 申請事由
		/// * Request["d_tel"]: 聯絡電話(手機)
		/// * Request["d_bloodtype"]: 血型
		/// * Request["d_emergency_contact"]: 緊急聯絡人
		/// * Request["d_emergency_tel"]: 緊急聯絡人電話
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

			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
			string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Common.FilterCheckMarxString(Request["gid"].ToString().Trim());
			string d_type = (string.IsNullOrEmpty(Request["d_type"])) ? "" : Common.FilterCheckMarxString(Request["d_type"].ToString().Trim());
			string d_startday = (string.IsNullOrEmpty(Request["d_startday"])) ? "" : Common.FilterCheckMarxString(Request["d_startday"].ToString().Trim());
			string d_endday = (string.IsNullOrEmpty(Request["d_endday"])) ? "" : Common.FilterCheckMarxString(Request["d_endday"].ToString().Trim());
			string d_reason = (string.IsNullOrEmpty(Request["d_reason"])) ? "" : Common.FilterCheckMarxString(Request["d_reason"].ToString().Trim());
			string d_tel = (string.IsNullOrEmpty(Request["d_tel"])) ? "" : Common.FilterCheckMarxString(Request["d_tel"].ToString().Trim());
			string d_bloodtype = (string.IsNullOrEmpty(Request["d_bloodtype"])) ? "" : Common.FilterCheckMarxString(Request["d_bloodtype"].ToString().Trim());
			string d_emergency_contact = (string.IsNullOrEmpty(Request["d_emergency_contact"])) ? "" : Common.FilterCheckMarxString(Request["d_emergency_contact"].ToString().Trim());
			string d_emergency_tel = (string.IsNullOrEmpty(Request["d_emergency_tel"])) ? "" : Common.FilterCheckMarxString(Request["d_emergency_tel"].ToString().Trim());

			string tmpGuid = Guid.NewGuid().ToString("N");

			db._d_guid = tmpGuid;
			db._d_type = d_type;
			db._d_name = LogInfo.empName;
			db._d_empno = LogInfo.empNo;
			db._d_department = LogInfo.deptName;
			db._d_startday = d_startday;
			db._d_endday = d_endday;
			db._d_reason = d_reason;
			db._d_tel = d_tel;
			db._d_bloodtype = d_bloodtype.ToUpper();
			db._d_emergency_contact = d_emergency_contact;
			db._d_emergency_tel = d_emergency_tel;
			db._d_createid = LogInfo.empNo;
			db._d_createname = LogInfo.empName;
			db._d_modid = LogInfo.empNo;
			db._d_modname = LogInfo.empName;

			db.addDormitory(oConn, myTrans);

			// 檔案上傳
			HttpFileCollection uploadFiles = Request.Files;
			HttpPostedFile File = uploadFiles[0];
			if (File.FileName.Trim() != "")
			{
				string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Dormitory\\";

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

				if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".pdf")
				{
					File.SaveAs(UpLoadPath + newName);
					// 進資料庫前, 儲存名稱要去除副檔名
					newName = newName.Replace(extension, "");

					fdb._File_Type = "Dormitory";
					fdb._File_Orgname = orgName;
					fdb._File_Encryname = newName;
					fdb._File_Exten = extension;
					fdb._File_Size = file_size;
					fdb._File_CreateId = LogInfo.empNo;
					fdb._File_ModId = LogInfo.empName;

					fdb._File_Parentid = tmpGuid;
					fdb.AddFile_Trans(oConn, myTrans);
				}
				else
				{
					throw new Exception("檔案格式限制: .jpg .jpeg .png .pdf");
				}
			}

			myTrans.Commit();

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><DataGuid>" + Server.UrlEncode(Common.Encrypt(tmpGuid)) + "</DataGuid></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			myTrans.Rollback();
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		finally
		{
			oCmmd.Connection.Close();
			oConn.Close();
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}