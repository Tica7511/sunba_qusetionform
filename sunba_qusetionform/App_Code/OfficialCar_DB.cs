using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OfficialCar_DB 的摘要描述
/// </summary>
public class OfficialCar_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string oc_id = string.Empty;
	string oc_guid = string.Empty;
	string oc_number = string.Empty;
	string oc_ps = string.Empty;
	string oc_createid = string.Empty;
	string oc_createname = string.Empty;
	DateTime oc_createdate;
	string oc_modid = string.Empty;
	string oc_modname = string.Empty;
	DateTime oc_moddate;
	string oc_status = string.Empty;
	#endregion
	#region Public
	public string _oc_id { set { oc_id = value; } }
	public string _oc_guid { set { oc_guid = value; } }
	public string _oc_number { set { oc_number = value; } }
	public string _oc_ps { set { oc_ps = value; } }
	public string _oc_createid { set { oc_createid = value; } }
	public string _oc_createname { set { oc_createname = value; } }
	public DateTime _oc_createdate { set { oc_createdate = value; } }
	public string _oc_modid { set { oc_modid = value; } }
	public string _oc_modname { set { oc_modname = value; } }
	public DateTime _oc_moddate { set { oc_moddate = value; } }
	public string _oc_status { set { oc_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from OfficialCar where oc_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addOfficialCar()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into OfficialCar (
oc_guid,
oc_number,
oc_ps,
oc_createid,
oc_createname,
oc_modid,
oc_modname,
oc_status
) values (
@oc_guid,
@oc_number,
@oc_ps,
@oc_createid,
@oc_createname,
@oc_modid,
@oc_modname,
@oc_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@oc_guid", oc_guid);
		oCmd.Parameters.AddWithValue("@oc_number", oc_number);
		oCmd.Parameters.AddWithValue("@oc_ps", oc_ps);
		oCmd.Parameters.AddWithValue("@oc_createid", oc_createid);
		oCmd.Parameters.AddWithValue("@oc_createname", oc_createname);
		oCmd.Parameters.AddWithValue("@oc_modid", oc_modid);
		oCmd.Parameters.AddWithValue("@oc_modname", oc_modname);
		oCmd.Parameters.AddWithValue("@oc_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateOfficialCar()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update OfficialCar set
oc_number=@oc_number,
oc_ps=@oc_ps,
oc_modid=@oc_modid,
oc_modname=@oc_modname,
oc_moddate=@oc_moddate
where oc_id=@oc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@oc_id", oc_id);
		oCmd.Parameters.AddWithValue("@oc_number", oc_number);
		oCmd.Parameters.AddWithValue("@oc_ps", oc_ps);
		oCmd.Parameters.AddWithValue("@oc_modid", oc_modid);
		oCmd.Parameters.AddWithValue("@oc_modname", oc_modname);
		oCmd.Parameters.AddWithValue("@oc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from OfficialCar where oc_id=@oc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@oc_id", oc_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteOfficialCar()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update OfficialCar set
oc_status='D',
oc_modid=@oc_modid,
oc_modname=@oc_modname,
oc_moddate=@oc_moddate
where oc_id=@oc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@oc_id", oc_id);
		oCmd.Parameters.AddWithValue("@oc_modid", oc_modid);
		oCmd.Parameters.AddWithValue("@oc_modname", oc_modname);
		oCmd.Parameters.AddWithValue("@oc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}