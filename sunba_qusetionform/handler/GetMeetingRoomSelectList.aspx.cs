using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetMeetingRoomSelectList : System.Web.UI.Page
{
	MeetingRoom_DB mrdb = new MeetingRoom_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 會議室下拉選單
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string xmlstr = string.Empty;
			DataTable dt = mrdb.GetList();
			xmlstr = GetMeetingRoom(dt);
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

	private string GetMeetingRoom(DataTable dt)
	{
		string tmpStr = string.Empty;
		XmlDocument doc = new XmlDocument();
		XmlElement XmlRoot = doc.CreateElement("dataRoot");
		doc.AppendChild(XmlRoot);
		XmlElement xNode_1 = doc.DocumentElement;
		XmlElement xNode_2 = doc.DocumentElement;
		if (dt.Rows.Count > 0)
		{
			string tmpCode = string.Empty;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (i != 0 && tmpCode != dt.Rows[i]["mr_place"].ToString())
					XmlRoot.AppendChild(xNode_1);

				// 廠區
				if (tmpCode != dt.Rows[i]["mr_place"].ToString())
				{
					xNode_1 = doc.CreateElement("dataList");
					xNode_1.SetAttribute("place", dt.Rows[i]["mr_place"].ToString());
					xNode_1.SetAttribute("area", dt.Rows[i]["Area"].ToString());
				}
				// 會議室
				xNode_2 = doc.CreateElement("data_item");
				xNode_2.SetAttribute("roomid", dt.Rows[i]["mr_guid"].ToString());
				xNode_2.SetAttribute("room", dt.Rows[i]["mr_name"].ToString());
				xNode_1.AppendChild(xNode_2);

				// Append Last Row
				if (i == dt.Rows.Count - 1)
					XmlRoot.AppendChild(xNode_1);

				tmpCode = dt.Rows[i]["mr_place"].ToString();
			}
		}
		tmpStr = doc.OuterXml;
		return tmpStr;
	}
}