using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_MeetingDetail : System.Web.UI.Page
{
	Competence_DB db = new Competence_DB();
	Meeting_DB mdb = new Meeting_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		bool c_status = true;
		// 管理者
		db._c_type = "06";
		DataTable dt = db.GetCompetenceList_Common();
		if (dt.Rows.Count > 0)
		{
			for(int i=0;i< dt.Rows.Count; i++)
			{
				if (LogInfo.empNo == dt.Rows[i]["value"].ToString().Trim())
					c_status = false;
			}
		}

		// 申請者
		string id = (string.IsNullOrEmpty(Request.QueryString["v"])) ? "" : Request.QueryString["v"].ToString().Trim();
		id = Common.Decrypt(id);

		mdb._m_id = id;
		DataTable mdt = mdb.GetMeetingDetail();
		if (mdt.Rows.Count > 0)
		{
			if (LogInfo.empNo == mdt.Rows[0]["m_createid"].ToString().Trim())
				c_status = false;
		}


		if (c_status)
		{
			Response.Write("<script type='text/javascript'>alert('您沒有權限進入此頁面!'); location.href=history.go(-1);</script>");
		}
	}
}