using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddSysSetting : System.Web.UI.Page
{
	Competence_DB db = new Competence_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 系統管理
		///說    明:
		/// * Request["MealsVisitor"]: 訪客用餐承辦人
		/// * Request["MealsCancel"]: 當日取消用餐承辦人
		/// * Request["OutdoorForm"]: 出廠證明單承辦人
		/// * Request["Dormitory"]: 宿舍承辦人
		/// * Request["DormitoryManager"]: 宿舍承辦主管
		/// * Request["Meeting"]: 會議室管理員
		/// * Request["Doc"]: 文件管理員
		/// * Request["SystemAdmin"]: 系統管理員
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

			string MealsVisitor = (string.IsNullOrEmpty(Request["MealsVisitor"])) ? "" : Common.FilterCheckMarxString(Request["MealsVisitor"].ToString().Trim());
			string MealsCancel = (string.IsNullOrEmpty(Request["MealsCancel"])) ? "" : Common.FilterCheckMarxString(Request["MealsCancel"].ToString().Trim());
			string OutdoorForm = (string.IsNullOrEmpty(Request["OutdoorForm"])) ? "" : Common.FilterCheckMarxString(Request["OutdoorForm"].ToString().Trim());
			string Dormitory = (string.IsNullOrEmpty(Request["Dormitory"])) ? "" : Common.FilterCheckMarxString(Request["Dormitory"].ToString().Trim());
			string DormitoryManager = (string.IsNullOrEmpty(Request["DormitoryManager"])) ? "" : Common.FilterCheckMarxString(Request["DormitoryManager"].ToString().Trim());
			string Meeting = (string.IsNullOrEmpty(Request["Meeting"])) ? "" : Common.FilterCheckMarxString(Request["Meeting"].ToString().Trim());
			string Doc = (string.IsNullOrEmpty(Request["Doc"])) ? "" : Common.FilterCheckMarxString(Request["Doc"].ToString().Trim());
			string SystemAdmin = (string.IsNullOrEmpty(Request["SystemAdmin"])) ? "" : Common.FilterCheckMarxString(Request["SystemAdmin"].ToString().Trim());

			DataToSQL(MealsVisitor, "MealsVisitor");
			DataToSQL(MealsCancel, "MealsCancel");
			DataToSQL(OutdoorForm, "OutdoorForm");
			DataToSQL(Dormitory, "Dormitory");
			DataToSQL(DormitoryManager, "DormitoryManager");
			DataToSQL(Meeting, "Meeting");
			DataToSQL(Doc, "Doc");
			DataToSQL(SystemAdmin, "SystemAdmin");

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	private void DataToSQL(string ReuqestStr, string Category)
	{

		switch (Category)
		{
			case "MealsVisitor":
				db._c_type = "01";
				db._c_typename = "訪客用餐承辦人";
				break;
			case "MealsCancel":
				db._c_type = "02";
				db._c_typename = "當日取消用餐承辦人";
				break;
			case "OutdoorForm":
				db._c_type = "03";
				db._c_typename = "外出單承辦人";
				break;
			case "Dormitory":
				db._c_type = "04";
				db._c_typename = "宿舍承辦人";
				break;
			case "DormitoryManager":
				db._c_type = "05";
				db._c_typename = "宿舍承辦主管";
				break;
			case "Meeting":
				db._c_type = "06";
				db._c_typename = "會議室管理員";
				break;
			case "Doc":
				db._c_type = "07";
				db._c_typename = "文件管理員";
				break;
			case "SystemAdmin":
				db._c_type = "sa";
				db._c_typename = "系統管理員";
				break;
		}

		db._c_guid = Guid.NewGuid().ToString("N");
		db._c_createid = LogInfo.empNo;
		db._c_createname = LogInfo.empName;
		db._c_modid = LogInfo.empNo;
		db._c_modname = LogInfo.empName;

		string empno = string.Empty;
		if (ReuqestStr != "")
		{
			string[] DataArray = ReuqestStr.Split(',');
		    DataArray = DataArray.Distinct().ToArray();
			for (int i = 0; i < DataArray.Length; i++)
			{
				if (empno != "") empno += ",";
				empno += DataArray[i];
			}
		}
		db._c_empno = empno;

		db.addCompetence();
	}
}