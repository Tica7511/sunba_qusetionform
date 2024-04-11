using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Configuration;

public partial class handler_FormFlowController : System.Web.UI.Page
{
	Flow_DB db = new Flow_DB();
	Personnel_DB p_db = new Personnel_DB();
	Competence_DB c_db = new Competence_DB();
	OutdoorForm_DB of_db = new OutdoorForm_DB();
	MailUtil email = new MailUtil();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 簽核流程 Controller
		///說    明:
		/// * Request["method"]: method
		/// * Request["dataGuid"]: 資料Guid
		/// * Request["formType"]: 表單類別
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

			string method = (string.IsNullOrEmpty(Request["method"])) ? "" : Common.FilterCheckMarxString(Request["method"].ToString().Trim());
			string dataGuid = (string.IsNullOrEmpty(Request["dataGuid"])) ? "" : Common.FilterCheckMarxString(Request["dataGuid"].ToString().Trim());
			string formType = (string.IsNullOrEmpty(Request["formType"])) ? "" : Common.FilterCheckMarxString(Request["formType"].ToString().Trim());
			string Signer = (string.IsNullOrEmpty(Request["signer"])) ? "" : Common.FilterCheckMarxString(Request["signer"].ToString().Trim());
			dataGuid = Common.Decrypt(Server.UrlDecode(dataGuid));

			string NextSigner = string.Empty;
			switch (method)
			{
				// 開單
				case "SendForm":
					string tmpGuid = Guid.NewGuid().ToString("N");
					// 副課長級以上無需走簽核
					if (CheckIdentity(LogInfo.empNo) > 0)
					{
						SendFormForManage(tmpGuid, formType, dataGuid);
					}
					// 一般簽核
					else
					{
						// FormMain
						db._fm_guid = tmpGuid;
						db._fm_category = formType;
						db._fm_data_guid = dataGuid;
						db._fm_createid = LogInfo.empNo;
						db._fm_createname = LogInfo.empName;
						// FormMainSite
						db._fms_guid = Guid.NewGuid().ToString("N");
						db._fms_parentid = tmpGuid;
						db._fms_site = 1;
						if (formType == "OFN")
							db._fms_signperson = Signer;
						else
							db._fms_signperson = GetSignPerson(formType, 1);
						db._fms_signredesc = "";
						db.SendForm();

						// 發信通知
						CreateForm_Mail(formType, dataGuid, Signer);
					}
					break;
				// 簽核
				case "SignNext":
					db._fms_guid = dataGuid;
					db._fms_actual_signer = LogInfo.empNo;
					db.SignNext();

					// 發信通知
					SignNext_Mail(dataGuid);
					break;
				// 否決
				case "Disagree":
					db._fms_guid = dataGuid;
					db.Disagree();
					break;
				// 抽單
				case "TerminateTask":
					db._fm_data_guid = dataGuid;
					db.TerminateTask();
					break;
			}

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	private void SendFormForManage(string tmpGuid, string formType, string dataGuid)
	{
		db._fm_guid = tmpGuid;
		db._fm_category = formType;
		db._fm_data_guid = dataGuid;
		db._fm_createid = LogInfo.empNo;
		db._fm_createname = LogInfo.empName;
		db._fm_result = "Y";
		db.SendFormForManager();
	}

	/// <summary>
	/// 下一位簽核者
	/// </summary>
	private string GetSignPerson(string category, int site)
	{
		string SignPersonEmpno = string.Empty;
		db._fss_main_code = category;
		db._fss_site = site;
		DataTable dt = db.GetSigner();
		if (dt.Rows.Count > 0)
		{
			SignPersonEmpno = dt.Rows[0]["NextSigner"].ToString().Trim();
		}
		return SignPersonEmpno;
	}

	private int CheckIdentity(string empno)
	{
		DataTable dt = p_db.GetEmployeeLevel(empno);
		if (dt.Rows.Count > 0)
		{
			return int.Parse(dt.Rows[0]["JobLv"].ToString());
		}
		else
			return 0;
	}

	#region 開單通知信
	private void CreateForm_Mail(string category, string dataGuid, string Signer)
	{
		string MailAddr = string.Empty;
		string Title = string.Empty;
		string Content = string.Empty;
		DataTable dt = new DataTable();
		switch (category)
		{
			case "MV":
				Title += "訪客用餐申請通知";
				c_db._c_type = "01";
				dt = c_db.GetListOfType_ForMail();
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						DataTable edt = p_db.GetEmployeeEmil(dt.Rows[i]["value"].ToString());
						if (edt.Rows.Count > 0)
						{
							// 收件者
							if (MailAddr != "") MailAddr += ",";
							MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
						}
					}
				}

				string mv_ApplyDate = string.Empty;
				MealsVisitor_DB mv = new MealsVisitor_DB();
				mv._mv_guid = dataGuid;
				DataTable mvdt = mv.GetDetailByGuid();
				if (mvdt.Rows.Count > 0)
					mv_ApplyDate = mvdt.Rows[0]["mv_date"].ToString();

				// 信件內容
				Content = "系統通知:<br><br>";
				Content += "同仁 " + LogInfo.empName + " 申請(" + mv_ApplyDate + ")當日訪客用餐，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";

				break;
			case "MC":
				Title += "當日用餐取消申請通知";
				c_db._c_type = "02";
				dt = c_db.GetListOfType_ForMail();
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						DataTable edt = p_db.GetEmployeeEmil(dt.Rows[i]["value"].ToString());
						if (edt.Rows.Count > 0)
						{
							// 收件者
							if (MailAddr != "") MailAddr += ",";
							MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
						}
					}
				}

				string mcItem = string.Empty;
				MealsCancel_DB mc = new MealsCancel_DB();
				mc._mc_guid = dataGuid;
				DataTable mcdt = mc.GetDetail();
				if (mcdt.Rows.Count > 0)
					mcItem = mcdt.Rows[0]["mc_item"].ToString();

				// 信件內容
				Content = "系統通知:<br><br>";
				Content += "同仁 " + LogInfo.empName + " 申請(" + DateTime.Now.ToString("yyyy-MM-dd") + ")當日(" + mcItem + ")取消，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";
				break;
			case "OFN":
				Title += "外出單申請通知";
				// 收件者
				DataTable empdt = p_db.GetEmployeeEmil(Signer);
				if (empdt.Rows.Count > 0)
					MailAddr = empdt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();

				string ofn_ApplyDate = string.Empty;
				of_db._o_guid = dataGuid;
				DataTable ofndt = of_db.GetDetail();
				if (ofndt.Rows.Count > 0)
					ofn_ApplyDate = DateTime.Parse(ofndt.Rows[0]["o_starttime"].ToString()).ToString("yyyy-MM-dd");

				// 信件內容
				Content = "系統通知:<br><br>";
				Content += "同仁 " + LogInfo.empName + " 申請(" + ofn_ApplyDate + ")外出單，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";
				break;
			case "OFC":
				Title += "外出單公務車申請通知";
				c_db._c_type = "03";
				dt = c_db.GetListOfType_ForMail();
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						DataTable edt = p_db.GetEmployeeEmil(dt.Rows[i]["value"].ToString());
						if (edt.Rows.Count > 0)
						{
							// 收件者
							if (MailAddr != "") MailAddr += ",";
							MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
						}
					}
				}

				string ofc_ApplyDate = string.Empty;
				of_db._o_guid = dataGuid;
				DataTable ofcdt = of_db.GetDetail();
				if (ofcdt.Rows.Count > 0)
					ofc_ApplyDate = DateTime.Parse(ofcdt.Rows[0]["o_starttime"].ToString()).ToString("yyyy-MM-dd");

				// 信件內容
				Content = "系統通知:<br><br>";
				Content += "同仁 " + LogInfo.empName + " 申請(" + ofc_ApplyDate + ")公務車外出單，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";
				break;
			case "DL":
			case "DS":
				string TypeName = (category == "DL") ? "長期住舍" : "短期住舍";
				Title += TypeName + "申請通知";
				c_db._c_type = "04";
				dt = c_db.GetListOfType_ForMail();
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						DataTable edt = p_db.GetEmployeeEmil(dt.Rows[0]["value"].ToString());
						if (edt.Rows.Count > 0)
						{
							// 收件者
							if (MailAddr != "") MailAddr += ",";
							MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
						}
					}
				}

				// 信件內容
				Content = "系統通知:<br><br>";
				Content += "同仁 " + LogInfo.empName + " 申請" + TypeName + "，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";
				break;
		}

		// 發信
		if (MailAddr != "")
			email.MailTo(MailAddr, Title, Content);
	}
	#endregion

	#region 簽核至下一關通知信
	private void SignNext_Mail(string dataGuid)
	{
		string MailAddr = string.Empty;
		string Title = ConfigurationManager.AppSettings["MailSender"];
		string Content = string.Empty;

		DataTable dt = new DataTable();

		db._fms_guid = dataGuid;
		DataTable nSignerDt = db.GetNextSigner_ForMail();
		if (nSignerDt.Rows.Count > 0)
		{
			switch (nSignerDt.Rows[0]["fm_category"].ToString())
			{
				case "DL":
				case "DS":
					string TypeName = (nSignerDt.Rows[0]["fm_category"].ToString() == "DL") ? "長期住舍" : "短期住舍";
					Title += TypeName + "申請通知";

					// 沒有預定簽核者即為第二關 - 宿舍承辦主管
					if (nSignerDt.Rows[0]["fms_signperson"].ToString() == "")
					{
						c_db._c_type = "05";
						dt = c_db.GetListOfType_ForMail();
						if (dt.Rows.Count > 0)
						{
							for (int i = 0; i < dt.Rows.Count; i++)
							{
								DataTable edt = p_db.GetEmployeeEmil(dt.Rows[0]["value"].ToString());
								if (edt.Rows.Count > 0)
								{
									// 收件者
									if (MailAddr != "") MailAddr += ",";
									MailAddr += edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
								}
							}
						}
					}
					else
					{
						DataTable edt = p_db.GetEmployeeEmil(nSignerDt.Rows[0]["fms_signperson"].ToString());
						if (edt.Rows.Count > 0)
							MailAddr = edt.Rows[0]["EMPLOYEE_EMAIL_1"].ToString();
					}

					// 信件內容
					Content = "系統通知:<br><br>";
					Content += "同仁 " + nSignerDt.Rows[0]["fm_createname"].ToString() + " 申請" + TypeName + "，請上<a href='https://powersunba.com.tw/SunBa_Affairs/WebPage/Default.aspx'>庶務系統</a>進行審核，感謝您。";
					break;
			}
		}

		// 發信
		if (MailAddr != "")
			email.MailTo(MailAddr, Title, Content);
	}
	#endregion
}