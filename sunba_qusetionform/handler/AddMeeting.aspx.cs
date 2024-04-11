using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class handler_AddMeeting : System.Web.UI.Page
{
	Meeting_DB db = new Meeting_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 會議室申請
		///說    明:
		/// * Request["id"]: ID
		/// * Request["m_room"]: 使用場所
		/// * Request["ApplyCategory"]: 申請類型
		/// * Request["single_date"]: 一般會議日期
		/// * Request["single_stime"]: 一般會議 開始時間
		/// * Request["single_etime"]: 一般會議 結束時間
		/// * Request["cycle_sdate"]: 週期會議 開始日期
		/// * Request["cycle_edate"]: 週期會議 結束日期
		/// * Request["week"]: 星期
		/// * Request["cycle_stime"]: 週期會議 開始時間
		/// * Request["cycle_etime"]: 週期會議 結束時間
		/// * Request["m_desc"]: 申請用途
		/// * Request["ddlApplydept"]: 申請單位
		/// * Request["ddlPerson"]: 參與人員
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
			string m_room = (string.IsNullOrEmpty(Request["m_room"])) ? "" : Common.FilterCheckMarxString(Request["m_room"].ToString().Trim());
			string ApplyCategory = (string.IsNullOrEmpty(Request["ApplyCategory"])) ? "" : Common.FilterCheckMarxString(Request["ApplyCategory"].ToString().Trim());
			string m_date = (string.IsNullOrEmpty(Request["m_date"])) ? "" : Common.FilterCheckMarxString(Request["m_date"].ToString().Trim());
			string m_starthour = (string.IsNullOrEmpty(Request["m_starthour"])) ? "" : Common.FilterCheckMarxString(Request["m_starthour"].ToString().Trim());
			string m_endhour = (string.IsNullOrEmpty(Request["m_endhour"])) ? "" : Common.FilterCheckMarxString(Request["m_endhour"].ToString().Trim());
			string m_startmins = (string.IsNullOrEmpty(Request["m_startmins"])) ? "" : Common.FilterCheckMarxString(Request["m_startmins"].ToString().Trim());
			string m_endmins = (string.IsNullOrEmpty(Request["m_endmins"])) ? "" : Common.FilterCheckMarxString(Request["m_endmins"].ToString().Trim());
			string cycle_sdate = (string.IsNullOrEmpty(Request["cycle_sdate"])) ? "" : Common.FilterCheckMarxString(Request["cycle_sdate"].ToString().Trim());
			string cycle_edate = (string.IsNullOrEmpty(Request["cycle_edate"])) ? "" : Common.FilterCheckMarxString(Request["cycle_edate"].ToString().Trim());
			string week = (string.IsNullOrEmpty(Request["week"])) ? "" : Common.FilterCheckMarxString(Request["week"].ToString().Trim());
			string cycle_shour = (string.IsNullOrEmpty(Request["cycle_shour"])) ? "" : Common.FilterCheckMarxString(Request["cycle_shour"].ToString().Trim());
			string cycle_ehour = (string.IsNullOrEmpty(Request["cycle_ehour"])) ? "" : Common.FilterCheckMarxString(Request["cycle_ehour"].ToString().Trim());
			string cycle_smins = (string.IsNullOrEmpty(Request["cycle_smins"])) ? "" : Common.FilterCheckMarxString(Request["cycle_smins"].ToString().Trim());
			string cycle_emins = (string.IsNullOrEmpty(Request["cycle_emins"])) ? "" : Common.FilterCheckMarxString(Request["cycle_emins"].ToString().Trim());
			string m_desc = (string.IsNullOrEmpty(Request["m_desc"])) ? "" : Common.FilterCheckMarxString(Request["m_desc"].ToString().Trim());
			string m_ps = (string.IsNullOrEmpty(Request["m_ps"])) ? "" : Common.FilterCheckMarxString(Request["m_ps"].ToString().Trim());
			string ddlPerson = (string.IsNullOrEmpty(Request["ddlPerson"])) ? "" : Common.FilterCheckMarxString(Request["ddlPerson"].ToString().Trim());

			string startTime = m_starthour + ":" + m_startmins;
			string endTime = m_endhour + ":" + m_endmins;
			db._m_room = m_room;
			db._m_date = m_date;
			db._m_starttime = startTime;
			db._m_endtime = endTime;
			db._m_desc = m_desc;
			db._m_ps = m_ps;
			db._m_participant = ddlPerson;
			db._m_createid = LogInfo.empNo;
			db._m_createname = LogInfo.empName;
			db._m_modid = LogInfo.empNo;
			db._m_modname = LogInfo.empName;

			// 新增
			if (id == "")
			{
				// 一般會議
				if (ApplyCategory == "01")
				{
					db._m_guid = Guid.NewGuid().ToString("N");
					if (CheckMeeting("", m_room, m_date, startTime, endTime))
						db.addMeeting();
					else
						throw new Exception("該時段已有預約，請重新輸入日期或時段");
				}
				else // 週期性會議
				{
					DateTime StartDay = DateTime.Parse(cycle_sdate);
					DateTime EndDay = DateTime.Parse(cycle_edate);
					for (DateTime date = StartDay; date <= EndDay; date = date.AddDays(1))
					{
						if ((int)date.DayOfWeek == Int32.Parse(week))
						{
							string sTime = cycle_shour + ":" + cycle_smins;
							string eTime = cycle_ehour + ":" + cycle_emins;
							db._m_guid = Guid.NewGuid().ToString("N");
							db._m_date = date.ToString("yyyy-MM-dd");
							db._m_starttime = sTime;
							db._m_endtime = eTime;
							if (CheckMeeting("", m_room, date.ToString("yyyy-MM-dd"), sTime, eTime))
								db.addMeeting();
							else
								throw new Exception(date.ToString("yyyy-MM-dd") + " 該時段已有預約，請重新輸入日期或時段");
						}
					}
				}
			}
			else // 修改
			{
				id = Common.Decrypt(id);
				db._m_id = id;
				if (CheckMeeting(id, m_room, m_date, startTime, endTime))
					db.UpdateMeeting();
				else
					throw new Exception(" 該時段已有預約，請重新輸入日期或時段");
			}

			string xmlstr = string.Empty;
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>會議室申請成功</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	private bool CheckMeeting(string id, string mRoom, string date, string sTime, string eTime)
	{
		bool status = true;
		db._m_room = mRoom;
		db._m_date = date;
		db._m_starttime = sTime;
		db._m_endtime = eTime;
		DataTable dt = db.CheckMeeting();
		// 新增
		if (id == "")
		{
			if (dt.Rows.Count > 0)
				status = false;
		}
		else // 修改
		{
			if (dt.Rows.Count == 1)
			{
				if (dt.Rows[0]["m_id"].ToString() != id)
					status = false;
			}
			else if (dt.Rows.Count > 1)
				status = false;
		}
		return status;
	}
}