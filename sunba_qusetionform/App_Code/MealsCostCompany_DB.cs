using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCostCompany_DB 的摘要描述
/// </summary>
public class MealsCostCompany_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	string mcc_id = string.Empty;
	string mcc_guid = string.Empty;
	string mcc_name = string.Empty;
	string mcc_tel = string.Empty;
	string mcc_createid = string.Empty;
	string mcc_createname = string.Empty;
	DateTime mcc_createdate;
	string mcc_modid = string.Empty;
	string mcc_modname = string.Empty;
	DateTime mcc_moddate;
	string mcc_status = string.Empty;
	#endregion
	#region Public
	public string _mcc_id { set { mcc_id = value; } }
	public string _mcc_guid { set { mcc_guid = value; } }
	public string _mcc_name { set { mcc_name = value; } }
	public string _mcc_tel { set { mcc_tel = value; } }
	public string _mcc_createid { set { mcc_createid = value; } }
	public string _mcc_createname { set { mcc_createname = value; } }
	public DateTime _mcc_createdate { set { mcc_createdate = value; } }
	public string _mcc_modid { set { mcc_modid = value; } }
	public string _mcc_modname { set { mcc_modname = value; } }
	public DateTime _mcc_moddate { set { mcc_moddate = value; } }
	public string _mcc_status { set { mcc_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostCompany where mcc_status='A' ");

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
		sb.Append(@"insert into MealsCostCompany (
mcc_guid,
mcc_name,
mcc_tel,
mcc_createid,
mcc_createname,
mcc_modid,
mcc_modname,
mcc_status
) values (
@mcc_guid,
@mcc_name,
@mcc_tel,
@mcc_createid,
@mcc_createname,
@mcc_modid,
@mcc_modname,
@mcc_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcc_guid", mcc_guid);
		oCmd.Parameters.AddWithValue("@mcc_name", mcc_name);
		oCmd.Parameters.AddWithValue("@mcc_tel", mcc_tel);
		oCmd.Parameters.AddWithValue("@mcc_createid", mcc_createid);
		oCmd.Parameters.AddWithValue("@mcc_createname", mcc_createname);
		oCmd.Parameters.AddWithValue("@mcc_modid", mcc_modid);
		oCmd.Parameters.AddWithValue("@mcc_modname", mcc_modname);
		oCmd.Parameters.AddWithValue("@mcc_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateCompany()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCostCompany set
mcc_name=@mcc_name,
mcc_tel=@mcc_tel,
mcc_modid=@mcc_modid,
mcc_modname=@mcc_modname,
mcc_moddate=@mcc_moddate
where mcc_id=@mcc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcc_id", mcc_id);
		oCmd.Parameters.AddWithValue("@mcc_name", mcc_name);
		oCmd.Parameters.AddWithValue("@mcc_tel", mcc_tel);
		oCmd.Parameters.AddWithValue("@mcc_modid", mcc_modid);
		oCmd.Parameters.AddWithValue("@mcc_modname", mcc_modname);
		oCmd.Parameters.AddWithValue("@mcc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostCompany where mcc_id=@mcc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mcc_id", mcc_id);

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
		sb.Append(@"update MealsCostCompany set
mcc_status='D',
mcc_modid=@mcc_modid,
mcc_modname=@mcc_modname,
mcc_moddate=@mcc_moddate
where mcc_id=@mcc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mcc_id", mcc_id);
		oCmd.Parameters.AddWithValue("@mcc_modid", mcc_modid);
		oCmd.Parameters.AddWithValue("@mcc_modname", mcc_modname);
		oCmd.Parameters.AddWithValue("@mcc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostCompany where mcc_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}