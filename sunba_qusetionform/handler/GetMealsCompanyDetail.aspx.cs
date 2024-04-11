using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMealsCompanyDetail : System.Web.UI.Page
{
	MealsCompany_DB db = new MealsCompany_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 廠商/愛心便當資訊
		///說    明:
		/// * Request["mode"]: mode
		/// * Request["id"]: ID
		/// * Request["gid"]: Guid
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Common.FilterCheckMarxString(Request["mode"].ToString().Trim());
			string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
			string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Common.FilterCheckMarxString(Request["gid"].ToString().Trim());
			gid = Common.Decrypt(gid);

			DataTable dt = new DataTable();
			db._mc_id = id;
			db._mc_guid = gid;
			if (mode == "PageTitle")
				dt = db.GetCompanyName();
			else
				dt = db.GetDetail();
			string xmlstr = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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