using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MeetingFeedBack_DB 的摘要描述
/// </summary>
public class MeetingFeedBack_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mfb_id = string.Empty;
	string mfb_guid = string.Empty;
	string mfb_room = string.Empty;
	string mfb_date = string.Empty;
	string mfb_content = string.Empty;
	string mfb_createid = string.Empty;
	string mfb_createname = string.Empty;
	DateTime mfb_createdate;
	string mfb_modid = string.Empty;
	string mfb_modname = string.Empty;
	DateTime mfb_moddate;
	string mfb_status = string.Empty;
	#endregion
	#region Public
	public string _mfb_id { set { mfb_id = value; } }
	public string _mfb_guid { set { mfb_guid = value; } }
	public string _mfb_room { set { mfb_room = value; } }
	public string _mfb_date { set { mfb_date = value; } }
	public string _mfb_content { set { mfb_content = value; } }
	public string _mfb_createid { set { mfb_createid = value; } }
	public string _mfb_createname { set { mfb_createname = value; } }
	public DateTime _mfb_createdate { set { mfb_createdate = value; } }
	public string _mfb_modid { set { mfb_modid = value; } }
	public string _mfb_modname { set { mfb_modname = value; } }
	public DateTime _mfb_moddate { set { mfb_moddate = value; } }
	public string _mfb_status { set { mfb_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mfb.*,mr_name from MeetingFeedBack as mfb
left join MeetingRoom as mr on mr_guid=mfb_room
where mfb_status='A'
order by convert(datetime, mfb_date) desc ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}


	public void addMeetingFeedBack()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MeetingFeedBack (
mfb_guid,
mfb_room,
mfb_date,
mfb_content,
mfb_createid,
mfb_createname,
mfb_modid,
mfb_modname,
mfb_status
) values (
@mfb_guid,
@mfb_room,
@mfb_date,
@mfb_content,
@mfb_createid,
@mfb_createname,
@mfb_modid,
@mfb_modname,
@mfb_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mfb_guid", mfb_guid);
		oCmd.Parameters.AddWithValue("@mfb_room", mfb_room);
		oCmd.Parameters.AddWithValue("@mfb_date", mfb_date);
		oCmd.Parameters.AddWithValue("@mfb_content", mfb_content);
		oCmd.Parameters.AddWithValue("@mfb_createid", mfb_createid);
		oCmd.Parameters.AddWithValue("@mfb_createname", mfb_createname);
		oCmd.Parameters.AddWithValue("@mfb_modid", mfb_modid);
		oCmd.Parameters.AddWithValue("@mfb_modname", mfb_modname);
		oCmd.Parameters.AddWithValue("@mfb_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataSet GetReviewList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mfb.*,mr.mr_name into #tmp from MeetingFeedBack as mfb
left join MeetingRoom as mr on mfb_room=mr_guid
where mfb_status='A' ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by convert(datetime,mfb_date) desc,mfb_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteMeetingFeedBack()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update MeetingFeedBack set
mfb_status=@mfb_status,
mfb_modid=@mfb_modid,
mfb_modname=@mfb_modname,
mfb_moddate=@mfb_moddate
where mfb_id=@mfb_id
";
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@mfb_id", mfb_id);
		oCmd.Parameters.AddWithValue("@mfb_modid", mfb_modid);
		oCmd.Parameters.AddWithValue("@mfb_modname", mfb_modname);
		oCmd.Parameters.AddWithValue("@mfb_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@mfb_status", "D");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}