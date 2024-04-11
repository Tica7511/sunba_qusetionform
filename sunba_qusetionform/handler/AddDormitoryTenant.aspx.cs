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

public partial class handler_AddDormitoryTenant : System.Web.UI.Page
{
	DormitoryTenant_DB db = new DormitoryTenant_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 入住管理
		///說    明:
		/// * Request["gid"]: ID
		/// * Request["person"]: 人員
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
			string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Common.FilterCheckMarxString(Request["empno"].ToString().Trim());
			string person = (string.IsNullOrEmpty(Request["person"])) ? "" : Common.FilterCheckMarxString(Request["person"].ToString().Trim());
			string[] empnoAry = empno.Split(',');
			string[] personAry = person.Split(',');
			if (personAry.Length > 0)
			{
				db._dt_roomid = gid;
				db._dt_modid = LogInfo.empNo;
				db._dt_modname = LogInfo.empName;
				db.DeleteByRoomID(oConn, myTrans);

				for (int i = 0; i < personAry.Length; i++)
				{
					db._dt_guid = Guid.NewGuid().ToString("N");
					db._dt_empno = empnoAry[i];
					db._dt_name = personAry[i];
					db._dt_createid = LogInfo.empNo;
					db._dt_createname = LogInfo.empName;
					db.addDormitoryTenant(oConn, myTrans);
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