using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;


/// <summary>
/// Meeting_DB 的摘要描述
/// </summary>
public class Meeting_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	string m_id = string.Empty;
	string m_guid = string.Empty;
	string m_room = string.Empty;
	string m_date = string.Empty;
	string m_starttime = string.Empty;
	string m_endtime = string.Empty;
	string m_desc = string.Empty;
	string m_ps = string.Empty;
	string m_participant = string.Empty;
	string m_createid = string.Empty;
	string m_createname = string.Empty;
	DateTime m_createdate;
	string m_modid = string.Empty;
	string m_modname = string.Empty;
	DateTime m_moddate;
	string m_status = string.Empty;
	#endregion
	#region Public
	public string _m_id { set { m_id = value; } }
	public string _m_guid { set { m_guid = value; } }
	public string _m_room { set { m_room = value; } }
	public string _m_date { set { m_date = value; } }
	public string _m_starttime { set { m_starttime = value; } }
	public string _m_endtime { set { m_endtime = value; } }
	public string _m_desc { set { m_desc = value; } }
	public string _m_ps { set { m_ps = value; } }
	public string _m_participant { set { m_participant = value; } }
	public string _m_createid { set { m_createid = value; } }
	public string _m_createname { set { m_createname = value; } }
	public DateTime _m_createdate { set { m_createdate = value; } }
	public string _m_modid { set { m_modid = value; } }
	public string _m_modname { set { m_modname = value; } }
	public DateTime _m_moddate { set { m_moddate = value; } }
	public string _m_status { set { m_status = value; } }
	#endregion

	public DataSet GetManageList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select Meeting.*,mr.mr_name as RoomName into #tmp from Meeting 
left join MeetingRoom as mr on m_room=mr_guid
where m_status='A' and m_createid=@m_createid ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by m_date desc,convert(datetime,m_starttime)) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@m_createid", LogInfo.empNo);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}


	public void addMeeting()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into Meeting (
m_guid,
m_room,
m_date,
m_starttime,
m_endtime,
m_desc,
m_ps,
m_participant,
m_createid,
m_createname,
m_modid,
m_modname,
m_status
) values (
@m_guid,
@m_room,
@m_date,
@m_starttime,
@m_endtime,
@m_desc,
@m_ps,
@m_participant,
@m_createid,
@m_createname,
@m_modid,
@m_modname,
@m_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@m_guid", m_guid);
		oCmd.Parameters.AddWithValue("@m_room", m_room);
		oCmd.Parameters.AddWithValue("@m_date", m_date);
		oCmd.Parameters.AddWithValue("@m_starttime", m_starttime);
		oCmd.Parameters.AddWithValue("@m_endtime", m_endtime);
		oCmd.Parameters.AddWithValue("@m_desc", m_desc);
		oCmd.Parameters.AddWithValue("@m_ps", m_ps);
		oCmd.Parameters.AddWithValue("@m_participant", m_participant);
		oCmd.Parameters.AddWithValue("@m_createid", m_createid);
		oCmd.Parameters.AddWithValue("@m_createname", m_createname);
		oCmd.Parameters.AddWithValue("@m_modid", m_modid);
		oCmd.Parameters.AddWithValue("@m_modname", m_modname);
		oCmd.Parameters.AddWithValue("@m_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateMeeting()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update Meeting set
m_room=@m_room,
m_date=@m_date,
m_starttime=@m_starttime,
m_endtime=@m_endtime,
m_desc=@m_desc,
m_ps=@m_ps,
m_participant=@m_participant,
m_modid=@m_modid,
m_modname=@m_modname,
m_moddate=@m_moddate
where m_id=@m_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@m_id", m_id);
		oCmd.Parameters.AddWithValue("@m_room", m_room);
		oCmd.Parameters.AddWithValue("@m_date", m_date);
		oCmd.Parameters.AddWithValue("@m_starttime", m_starttime);
		oCmd.Parameters.AddWithValue("@m_endtime", m_endtime);
		oCmd.Parameters.AddWithValue("@m_desc", m_desc);
		oCmd.Parameters.AddWithValue("@m_ps", m_ps);
		oCmd.Parameters.AddWithValue("@m_participant", m_participant);
		oCmd.Parameters.AddWithValue("@m_modid", m_modid);
		oCmd.Parameters.AddWithValue("@m_modname", m_modname);
		oCmd.Parameters.AddWithValue("@m_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable CheckMeeting()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
  declare @sTime time = @m_starttime;
  declare @eTime time = @m_endtime;

  select * from Meeting
  where m_date=@m_date and m_room=@m_room and m_status='A' 
  and (
  (CONVERT (time,m_starttime) > @sTime and CONVERT (time,m_starttime) < @eTime)
  or
  (CONVERT (time,m_starttime) < @sTime and CONVERT (time,m_endtime) > @eTime)
  or
  (CONVERT (time,m_endtime) > @sTime and CONVERT (time,m_endtime) < @eTime)
  or
  (CONVERT (time,m_starttime) = @sTime and CONVERT (time,m_endtime) = @eTime)
  or
  (CONVERT (time,m_starttime) = @sTime and CONVERT (time,m_endtime) > @eTime)
  or
  (CONVERT (time,m_starttime) < @sTime and CONVERT (time,m_endtime) = @eTime)
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@m_room", m_room);
		oCmd.Parameters.AddWithValue("@m_date", m_date);
		oCmd.Parameters.AddWithValue("@m_starttime", m_starttime);
		oCmd.Parameters.AddWithValue("@m_endtime", m_endtime);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteMeeting()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update Meeting set
m_status=@m_status,
m_modid=@m_modid,
m_modname=@m_modname,
m_moddate=@m_moddate
where m_id=@m_id
";
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@m_id", m_id);
		oCmd.Parameters.AddWithValue("@m_modid", m_modid);
		oCmd.Parameters.AddWithValue("@m_modname", m_modname);
		oCmd.Parameters.AddWithValue("@m_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@m_status", "D");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetMeetingDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from Meeting where m_id=@m_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@m_id", m_id);
		oda.Fill(ds);
		return ds;
	}


	public DataTable GetCalendarList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from Meeting 
left join MeetingRoom on mr_guid=m_room
where m_status='A' ");

		if(m_room!="")
			sb.Append(@"and m_room=@m_room ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@m_room", m_room);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}