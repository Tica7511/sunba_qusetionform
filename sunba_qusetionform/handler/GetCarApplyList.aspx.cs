using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetCarApplyList : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 公務車使用狀況列表
		///說明:
		/// * Request["stime"]: 起始日期與時間
		/// * Request["etime"]: 結束日期與時間
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string stime = (string.IsNullOrEmpty(Request["stime"])) ? "" : Request["stime"].ToString().Trim();
			string etime = (string.IsNullOrEmpty(Request["etime"])) ? "" : Request["etime"].ToString().Trim();
			
			db._o_starttime = DateTime.Parse(stime);
			db._o_endtime = DateTime.Parse(etime);
			DataTable dt = db.GetCarList();
			#region 刪除空的 Element
			XmlDocument tmpX = new XmlDocument();
			tmpX.LoadXml(dt.Rows[0]["XmlCol"].ToString());
			XmlNodeList tmpNodeList = tmpX.SelectNodes("//data_item");
			foreach (XmlNode tmpNode in tmpNodeList)
			{
				if (tmpNode.Attributes.Count == 0)
				{
					tmpNode.ParentNode.RemoveChild(tmpNode);
				}
			}
			#endregion

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?>"+ tmpX.OuterXml;
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