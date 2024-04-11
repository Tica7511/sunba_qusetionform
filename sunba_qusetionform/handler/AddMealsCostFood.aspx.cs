using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddMealsCostFood : System.Web.UI.Page
{
	MealsCostFood_DB db = new MealsCostFood_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 品名管理
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
			string id = (string.IsNullOrEmpty(Request["mcf_id"])) ? "" : Common.FilterCheckMarxString(Request["mcf_id"].ToString().Trim());
			string mcf_name = (string.IsNullOrEmpty(Request["mcf_name"])) ? "" : Common.FilterCheckMarxString(Request["mcf_name"].ToString().Trim());
			string mcf_unit = (string.IsNullOrEmpty(Request["mcf_unit"])) ? "" : Common.FilterCheckMarxString(Request["mcf_unit"].ToString().Trim());

			db._mcf_createid = LogInfo.empNo;
			db._mcf_createname = LogInfo.empName;
			db._mcf_modid = LogInfo.empNo;
			db._mcf_modname = LogInfo.empName;

			if (mode == "add")
			{
				db._mcf_name = mcf_name;
				db._mcf_unit = mcf_unit;
				db._mcf_guid = Guid.NewGuid().ToString("N");
				db.addCompany();
			}
			else
			{
				if (id != "")
				{
					string[] aryID = id.Split(',');
					string[] aryName = mcf_name.Split(',');
					string[] aryUnit = mcf_unit.Split(',');
					if (aryID.Length > 0)
					{
						for (int i = 0; i < aryID.Length; i++)
						{
							db._mcf_id = aryID[i];
							db._mcf_name = aryName[i];
							db._mcf_unit = aryUnit[i];
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