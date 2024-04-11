using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// DormitoryCancel_DB 的摘要描述
/// </summary>
public class DormitoryCancel_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string dc_id = string.Empty;
	string dc_guid = string.Empty;
	string dc_category = string.Empty;
	string dc_canceldate = string.Empty;
	string dc_reason = string.Empty;
	string dc_createid = string.Empty;
	string dc_createname = string.Empty;
	DateTime dc_createdate;
	string dc_modid = string.Empty;
	string dc_modname = string.Empty;
	DateTime dc_moddate;
	string dc_status = string.Empty;
	#endregion
	#region Public
	public string _dc_id { set { dc_id = value; } }
	public string _dc_guid { set { dc_guid = value; } }
	public string _dc_category { set { dc_category = value; } }
	public string _dc_canceldate { set { dc_canceldate = value; } }
	public string _dc_reason { set { dc_reason = value; } }
	public string _dc_createid { set { dc_createid = value; } }
	public string _dc_createname { set { dc_createname = value; } }
	public DateTime _dc_createdate { set { dc_createdate = value; } }
	public string _dc_modid { set { dc_modid = value; } }
	public string _dc_modname { set { dc_modname = value; } }
	public DateTime _dc_moddate { set { dc_moddate = value; } }
	public string _dc_status { set { dc_status = value; } }
	#endregion

	public void addDormitoryCancel()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into DormitoryCancel (
dc_guid,
dc_category,
dc_canceldate,
dc_reason,
dc_createid,
dc_createname,
dc_modid,
dc_modname,
dc_status
) values (
@dc_guid,
@dc_category,
@dc_canceldate,
@dc_reason,
@dc_createid,
@dc_createname,
@dc_modid,
@dc_modname,
@dc_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dc_guid", dc_guid);
		oCmd.Parameters.AddWithValue("@dc_category", dc_category);
		oCmd.Parameters.AddWithValue("@dc_canceldate", dc_canceldate);
		oCmd.Parameters.AddWithValue("@dc_reason", dc_reason);
		oCmd.Parameters.AddWithValue("@dc_createid", dc_createid);
		oCmd.Parameters.AddWithValue("@dc_createname", dc_createname);
		oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
		oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
		oCmd.Parameters.AddWithValue("@dc_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable CheckIsApplied()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from DormitoryCancel where dc_createid=@dc_createid and dc_status='A' and dc_category='01' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@dc_createid", dc_createid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}


	public void DeleteDormitoryCancel(string empNo,string roomNo)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update DormitoryCancel set 
dc_status='D',
dc_modid=@dc_modid,
dc_modname=@dc_modname,
dc_moddate=@dc_moddate
where dc_id=@dc_id 

declare @tmpID bigint
select @tmpID=dt_id from DormitoryRoom
left join DormitoryTenant on dt_roomid=dr_guid and dt_status='A' and dt_empno=@empNo
where dr_status='A' and dr_no=@roomNo

update DormitoryTenant set 
dt_status='D',
dt_modid=@dc_modid,
dt_modname=@dc_modname,
dt_moddate=@dc_moddate
where dt_id=@tmpID ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dc_id", dc_id);
		oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
		oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
		oCmd.Parameters.AddWithValue("@dc_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@empNo", empNo);
		oCmd.Parameters.AddWithValue("@roomNo", roomNo);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}