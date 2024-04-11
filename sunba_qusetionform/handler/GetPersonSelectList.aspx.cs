using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_GetPersonSelectList : System.Web.UI.Page
{
	Personnel_DB db = new Personnel_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 人事資料下拉選單
		/// * Request["mode"]: 功能分類
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Common.FilterCheckMarxString(Request["mode"].ToString().Trim());

			DataTable dt = new DataTable();
			if (mode == "setting")
				dt = db.GetSysSettingSelectList();
			else
				dt = db.GetEmployeeSelectList();
			string xmlstr = string.Empty;
			xmlstr = DataProcessing(dt);
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

	private string DataProcessing(DataTable dt)
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
				if (i != 0 && tmpCode != dt.Rows[i]["deptid"].ToString())
					XmlRoot.AppendChild(xNode_1);

				// 分組資料
				if (tmpCode != dt.Rows[i]["deptid"].ToString())
				{
					xNode_1 = doc.CreateElement("dataList");
					xNode_1.SetAttribute("depCode", dt.Rows[i]["deptid"].ToString());
					xNode_1.SetAttribute("depName", dt.Rows[i]["dept"].ToString());
				}
				// 人員資料
				xNode_2 = doc.CreateElement("data_item");
				xNode_2.SetAttribute("empNo", dt.Rows[i]["empno"].ToString());
				xNode_2.SetAttribute("empName", dt.Rows[i]["empname"].ToString());
				xNode_1.AppendChild(xNode_2);

				// Append Last Row
				if (i == dt.Rows.Count - 1)
					XmlRoot.AppendChild(xNode_1);

				tmpCode = dt.Rows[i]["deptid"].ToString();
			}
		}
		tmpStr = doc.OuterXml;
		return tmpStr;
	}
}