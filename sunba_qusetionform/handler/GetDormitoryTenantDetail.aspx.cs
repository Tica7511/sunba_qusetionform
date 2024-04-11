using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetDormitoryTenantDetail : System.Web.UI.Page
{
	DormitoryTenant_DB db = new DormitoryTenant_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 入住詳細資料
		///說明:
		/// * Request["roomid"]: 房間 Guid
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string roomid = (string.IsNullOrEmpty(Request["roomid"])) ? "" : Common.FilterCheckMarxString(Request["roomid"].ToString().Trim());

			db._dt_roomid = roomid;
			DataSet ds = db.GetTenantDetail();
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "roomList", "room_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "tenantList", "tenant_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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