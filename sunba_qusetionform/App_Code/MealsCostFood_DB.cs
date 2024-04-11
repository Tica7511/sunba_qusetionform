using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCostFood_DB 的摘要描述
/// </summary>
public class MealsCostFood_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	string mcf_id = string.Empty;
	string mcf_guid = string.Empty;
	string mcf_name = string.Empty;
	string mcf_unit = string.Empty;
	string mcf_createid = string.Empty;
	string mcf_createname = string.Empty;
	DateTime mcf_createdate;
	string mcf_modid = string.Empty;
	string mcf_modname = string.Empty;
	DateTime mcf_moddate;
	string mcf_status = string.Empty;
	#endregion
	#region Public
	public string _mcf_id { set { mcf_id = value; } }
	public string _mcf_guid { set { mcf_guid = value; } }
	public string _mcf_name { set { mcf_name = value; } }
	public string _mcf_unit { set { mcf_unit = value; } }
	public string _mcf_createid { set { mcf_createid = value; } }
	public string _mcf_createname { set { mcf_createname = value; } }
	public DateTime _mcf_createdate { set { mcf_createdate = value; } }
	public string _mcf_modid { set { mcf_modid = value; } }
	public string _mcf_modname { set { mcf_modname = value; } }
	public DateTime _mcf_moddate { set { mcf_moddate = value; } }
	public string _mcf_status { set { mcf_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostFood where mcf_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addCompany()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsCostFood (
mcf_guid,
mcf_name,
mcf_unit,
mcf_createid,
mcf_createname,
mcf_modid,
mcf_modname,
mcf_status
) values (
@mcf_guid,
@mcf_name,
@mcf_unit,
@mcf_createid,
@mcf_createname,
@mcf_modid,
@mcf_modname,
@mcf_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcf_guid", mcf_guid);
		oCmd.Parameters.AddWithValue("@mcf_name", mcf_name);
		oCmd.Parameters.AddWithValue("@mcf_unit", mcf_unit);
		oCmd.Parameters.AddWithValue("@mcf_createid", mcf_createid);
		oCmd.Parameters.AddWithValue("@mcf_createname", mcf_createname);
		oCmd.Parameters.AddWithValue("@mcf_modid", mcf_modid);
		oCmd.Parameters.AddWithValue("@mcf_modname", mcf_modname);
		oCmd.Parameters.AddWithValue("@mcf_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateCompany()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCostFood set
mcf_name=@mcf_name,
mcf_unit=@mcf_unit,
mcf_modid=@mcf_modid,
mcf_modname=@mcf_modname,
mcf_moddate=@mcf_moddate
where mcf_id=@mcf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcf_id", mcf_id);
		oCmd.Parameters.AddWithValue("@mcf_name", mcf_name);
		oCmd.Parameters.AddWithValue("@mcf_unit", mcf_unit);
		oCmd.Parameters.AddWithValue("@mcf_modid", mcf_modid);
		oCmd.Parameters.AddWithValue("@mcf_modname", mcf_modname);
		oCmd.Parameters.AddWithValue("@mcf_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostFood where mcf_id=@mcf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mcf_id", mcf_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteCompany()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCostFood set
mcf_status='D',
mcf_modid=@mcf_modid,
mcf_modname=@mcf_modname,
mcf_moddate=@mcf_moddate
where mcf_id=@mcf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcf_id", mcf_id);
		oCmd.Parameters.AddWithValue("@mcf_modid", mcf_modid);
		oCmd.Parameters.AddWithValue("@mcf_modname", mcf_modname);
		oCmd.Parameters.AddWithValue("@mcf_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostFood where mcf_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}