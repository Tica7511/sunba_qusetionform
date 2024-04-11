using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MeetingRoom_DB 的摘要描述
/// </summary>
public class MeetingRoom_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mr_id = string.Empty;
	string mr_guid = string.Empty;
	string mr_name = string.Empty;
	string mr_place = string.Empty;
	string mr_createid = string.Empty;
	string mr_createname = string.Empty;
	DateTime mr_createdate;
	string mr_modid = string.Empty;
	string mr_modname = string.Empty;
	DateTime mr_moddate;
	string mr_status = string.Empty;
	#endregion
	#region Public
	public string _mr_id { set { mr_id = value; } }
	public string _mr_guid { set { mr_guid = value; } }
	public string _mr_name { set { mr_name = value; } }
	public string _mr_place { set { mr_place = value; } }
	public string _mr_createid { set { mr_createid = value; } }
	public string _mr_createname { set { mr_createname = value; } }
	public DateTime _mr_createdate { set { mr_createdate = value; } }
	public string _mr_modid { set { mr_modid = value; } }
	public string _mr_modname { set { mr_modname = value; } }
	public DateTime _mr_moddate { set { mr_moddate = value; } }
	public string _mr_status { set { mr_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select MeetingRoom.*,C_Item_cn as Area from MeetingRoom 
  left join CodeTable on C_Group='04' and mr_place=C_Item
  where mr_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addMeetingRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MeetingRoom (
mr_guid,
mr_name,
mr_place,
mr_createid,
mr_createname,
mr_modid,
mr_modname,
mr_status
) values (
@mr_guid,
@mr_name,
@mr_place,
@mr_createid,
@mr_createname,
@mr_modid,
@mr_modname,
@mr_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mr_guid", mr_guid);
		oCmd.Parameters.AddWithValue("@mr_name", mr_name);
		oCmd.Parameters.AddWithValue("@mr_place", mr_place);
		oCmd.Parameters.AddWithValue("@mr_createid", mr_createid);
		oCmd.Parameters.AddWithValue("@mr_createname", mr_createname);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateMeetingRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MeetingRoom set
mr_name=@mr_name,
mr_place=@mr_place,
mr_modid=@mr_modid,
mr_modname=@mr_modname,
mr_moddate=@mr_moddate
where mr_id=@mr_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mr_id", mr_id);
		oCmd.Parameters.AddWithValue("@mr_name", mr_name);
		oCmd.Parameters.AddWithValue("@mr_place", mr_place);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MeetingRoom where mr_id=@mr_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mr_id", mr_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteMeetingRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MeetingRoom set
mr_status='D',
mr_modid=@mr_modid,
mr_modname=@mr_modname,
mr_moddate=@mr_moddate
where mr_id=@mr_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mr_id", mr_id);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetCalendarRoomList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mr.mr_guid,mr.mr_name,C_Item_cn as Area from MeetingRoom as mr
  left join CodeTable on C_Group='04' and mr_place=C_Item
  where mr_status='A'
  order by mr.mr_place ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}