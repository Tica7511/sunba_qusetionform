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

public partial class handler_AddMealsCost : System.Web.UI.Page
{
	MealsCost_DB mc_db = new MealsCost_DB();
	MealsCostItem_DB mci_db = new MealsCostItem_DB();
	File_DB fdb = new File_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 成本管理
		///說    明:
		/// Main
		/// * Request["gid"]: mc_guid
		/// * Request["file_delete_id"]: 需刪除的檔案 ID
		/// * Request["category"]: 類別
		/// * Request["mc_date"]: 日期
		/// * Request["mc_price"]: 金額
		/// * Request["mc_ps"]: 備註
		/// Detail
		/// * Request["mci_item"]: 品名
		/// * Request["mci_num"]: 數量
		/// * Request["mci_unitprice"]: 單價
		/// * Request["mci_price"]: 金額
		/// * Request["mci_company"]: 廠商
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
			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
			// Main
			string mc_date = (string.IsNullOrEmpty(Request["mc_date"])) ? "" : Common.FilterCheckMarxString(Request["mc_date"].ToString().Trim());
			string mc_price = (string.IsNullOrEmpty(Request["mc_price"])) ? "0" : Common.FilterCheckMarxString(Request["mc_price"].ToString().Trim());
			string mc_ps = (string.IsNullOrEmpty(Request["mc_ps"])) ? "" : Common.FilterCheckMarxString(Request["mc_ps"].ToString().Trim());
			// Detail
			string mci_item = (string.IsNullOrEmpty(Request["mci_item"])) ? "" : Common.FilterCheckMarxString(Request["mci_item"].ToString().Trim());
			string mci_num = (string.IsNullOrEmpty(Request["mci_num"])) ? "" : Common.FilterCheckMarxString(Request["mci_num"].ToString().Trim());
			string mci_unitprice = (string.IsNullOrEmpty(Request["mci_unitprice"])) ? "" : Common.FilterCheckMarxString(Request["mci_unitprice"].ToString().Trim());
			string mci_price = (string.IsNullOrEmpty(Request["mci_price"])) ? "" : Common.FilterCheckMarxString(Request["mci_price"].ToString().Trim());
			string mci_company = (string.IsNullOrEmpty(Request["mci_company"])) ? "" : Common.FilterCheckMarxString(Request["mci_company"].ToString().Trim());

			string tmpGuid = (gid == "") ? Guid.NewGuid().ToString("N") : Common.Decrypt(Server.UrlDecode(gid));

			int ItemTotalPrice = 0;
			// 成本項目
			if (category == "cost")
			{
				if (mci_item != "")
				{
					#region 刪除原資料重新 Insert
					mci_db._mci_parentid = tmpGuid;
					mci_db._mci_modid = LogInfo.empNo;
					mci_db._mci_modname = LogInfo.empName;
					mci_db.DeleteItemList();
					#endregion

					string[] mciItem = mci_item.Split(',');
					string[] mciNum = mci_num.Split(',');
					string[] mciUnitPrice = mci_unitprice.Split(',');
					string[] mciPrice = mci_price.Split(',');
					string[] mciCompany = mci_company.Split(',');

					for (int i = 0; i < mciItem.Length; i++)
					{
						// Main 加總金額
						ItemTotalPrice = ItemTotalPrice + int.Parse(mciPrice[i]);

						mci_db._mci_guid = Guid.NewGuid().ToString("N");
						mci_db._mci_parentid = tmpGuid;
						mci_db._mci_item = mciItem[i];
						mci_db._mci_num = int.Parse(mciNum[i]);
						mci_db._mci_unitprice = int.Parse(mciUnitPrice[i]);
						mci_db._mci_price = int.Parse(mciPrice[i]);
						mci_db._mci_company = mciCompany[i];
						mci_db._mci_createid = LogInfo.empNo;
						mci_db._mci_createname = LogInfo.empName;
						mci_db._mci_modid = LogInfo.empNo;
						mci_db._mci_modname = LogInfo.empName;
						mci_db.addMealsCostItem(oConn, myTrans);
					}
				}
			}

			// Main
			mc_db._mc_guid = tmpGuid;
			mc_db._mc_category = category;
			mc_db._mc_date = mc_date;
			mc_db._mc_price = (category == "cost") ? ItemTotalPrice : int.Parse(mc_price);
			mc_db._mc_ps = mc_ps;
			mc_db._mc_createid = LogInfo.empNo;
			mc_db._mc_createname = LogInfo.empName;
			mc_db._mc_modid = LogInfo.empNo;
			mc_db._mc_modname = LogInfo.empName;
			if (gid == "")
				mc_db.addMealsCost(oConn, myTrans);
			else
				mc_db.UpdateMealsCost();


			#region 刪除檔案
			if (file_delete_id != "")
			{
				string[] fid = file_delete_id.Split(',');
				for(int i=0;i< fid.Length; i++)
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
					string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "MealsCost\\";

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

					if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".pdf" || extension.ToLower() == ".ppt" || extension.ToLower() == ".pptx"
					|| extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
					{
						File.SaveAs(UpLoadPath + newName);
						// 進資料庫前, 儲存名稱要去除副檔名
						newName = newName.Replace(extension, "");

						fdb._File_Parentid = tmpGuid;
						fdb._File_Type = "MealsCost";
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