using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsCostCompany : System.Web.UI.Page
{
	MealsCostCompany_DB db = new MealsCostCompany_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 進貨廠商管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["mode"]: add : 新增 / mod: 修改儲存
		/// * Request["mcc_name"]: 進貨商名稱
		/// * Request["mcc_tel"]: 電話
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

			string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Common.FilterCheckMarxString(Request["mode"].ToString().Trim());
			string id = (string.IsNullOrEmpty(Request["mcc_id"])) ? "" : Common.FilterCheckMarxString(Request["mcc_id"].ToString().Trim());
			string mcc_name = (string.IsNullOrEmpty(Request["mcc_name"])) ? "" : Common.FilterCheckMarxString(Request["mcc_name"].ToString().Trim());
			string mcc_tel = (string.IsNullOrEmpty(Request["mcc_tel"])) ? "" : Common.FilterCheckMarxString(Request["mcc_tel"].ToString().Trim());

			db._mcc_createid = LogInfo.empNo;
			db._mcc_createname = LogInfo.empName;
			db._mcc_modid = LogInfo.empNo;
			db._mcc_modname = LogInfo.empName;

			if (mode == "add")
			{
				db._mcc_name = mcc_name;
				db._mcc_tel = mcc_tel;
				db._mcc_guid = Guid.NewGuid().ToString("N");
				db.addCompany();
			}
			else
			{
				if (id != "")
				{
					string[] aryID = id.Split(',');
					string[] aryName = mcc_name.Split(',');
					string[] aryTel = mcc_tel.Split(',');
					if (aryID.Length > 0)
					{
						for (int i = 0; i < aryID.Length; i++)
						{
							db._mcc_id = aryID[i];
							db._mcc_name = aryName[i];
							db._mcc_tel = aryTel[i];
							db.UpdateCompany();
						}
					}
				}
			}

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