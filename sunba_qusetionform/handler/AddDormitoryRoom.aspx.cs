using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddDormitoryRoom : System.Web.UI.Page
{
	DormitoryRoom_DB db = new DormitoryRoom_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 房間管理
		///說    明:
		/// * Request["id"]: ID
		/// * Request["dr_area"]: 廠區
		/// * Request["dr_no"]: 房號
		/// * Request["dr_ext"]: 分機
		/// * Request["dr_roomtype"]: 房型
		/// * Request["dr_category"]: 類型
		/// * Request["dr_ps"]: 備註
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

			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
			string dr_area = (string.IsNullOrEmpty(Request["dr_area"])) ? "" : Common.FilterCheckMarxString(Request["dr_area"].ToString().Trim());
			string dr_no = (string.IsNullOrEmpty(Request["dr_no"])) ? "" : Common.FilterCheckMarxString(Request["dr_no"].ToString().Trim());
			string dr_ext = (string.IsNullOrEmpty(Request["dr_ext"])) ? "" : Common.FilterCheckMarxString(Request["dr_ext"].ToString().Trim());
			string dr_roomtype = (string.IsNullOrEmpty(Request["dr_roomtype"])) ? "" : Common.FilterCheckMarxString(Request["dr_roomtype"].ToString().Trim());
			string dr_category = (string.IsNullOrEmpty(Request["dr_category"])) ? "" : Common.FilterCheckMarxString(Request["dr_category"].ToString().Trim());
			string dr_ps = (string.IsNullOrEmpty(Request["dr_ps"])) ? "" : Common.FilterCheckMarxString(Request["dr_ps"].ToString().Trim());

			db._dr_area = dr_area;
			db._dr_no = dr_no;
			db._dr_ext = dr_ext;
			db._dr_roomtype = dr_roomtype;
			db._dr_category = dr_category;
			db._dr_ps = dr_ps;
			db._dr_createid = LogInfo.empNo;
			db._dr_createname = LogInfo.empName;
			db._dr_modid = LogInfo.empNo;
			db._dr_modname = LogInfo.empName;

			if (id == "")
			{
				db._dr_guid = Guid.NewGuid().ToString("N");
				db.addDormitoryRoom();
			}
			else
			{
				db._dr_id = id;
				db.UpdateDormitoryRoom();
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