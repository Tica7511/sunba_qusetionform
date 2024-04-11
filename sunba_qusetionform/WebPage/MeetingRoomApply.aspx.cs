using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_MeetingRoomApply : System.Web.UI.Page
{
	public string MeetingRoom,MeetingDate, sTime, eTime;
	protected void Page_Load(object sender, EventArgs e)
	{
		MeetingRoom = (string.IsNullOrEmpty(Request["MeetingRoom"])) ? "" : Common.FilterCheckMarxString(Request["MeetingRoom"].ToString().Trim());
		MeetingDate = (string.IsNullOrEmpty(Request["MeetingDate"])) ? "" : Common.FilterCheckMarxString(Request["MeetingDate"].ToString().Trim());
		sTime = (string.IsNullOrEmpty(Request["sTime"])) ? "" : Common.FilterCheckMarxString(Request["sTime"].ToString().Trim());
		eTime = (string.IsNullOrEmpty(Request["eTime"])) ? "" : Common.FilterCheckMarxString(Request["eTime"].ToString().Trim());
	}
}