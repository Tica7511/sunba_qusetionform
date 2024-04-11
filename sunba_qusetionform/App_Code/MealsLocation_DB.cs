using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsLocation_DB 的摘要描述
/// </summary>
public class MealsLocation_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string ml_id = string.Empty;
	string ml_guid = string.Empty;
	string ml_name = string.Empty;
	string ml_createid = string.Empty;
	string ml_createname = string.Empty;
	DateTime ml_createdate;
	string ml_modid = string.Empty;
	string ml_modname = string.Empty;
	DateTime ml_moddate;
	string ml_status = string.Empty;
	#endregion
	#region Public
	public string _ml_id { set { ml_id = value; } }
	public string _ml_guid { set { ml_guid = value; } }
	public string _ml_name { set { ml_name = value; } }
	public string _ml_createid { set { ml_createid = value; } }
	public string _ml_createname { set { ml_createname = value; } }
	public DateTime _ml_createdate { set { ml_createdate = value; } }
	public string _ml_modid { set { ml_modid = value; } }
	public string _ml_modname { set { ml_modname = value; } }
	public DateTime _ml_moddate { set { ml_moddate = value; } }
	public string _ml_status { set { ml_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsLocation where ml_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsLocation where ml_id=@ml_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@ml_id", ml_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsLocation()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsLocation (
ml_guid,
ml_name,
ml_createid,
ml_createname,
ml_modid,
ml_modname,
ml_status
) values (
@ml_guid,
@ml_name,
@ml_createid,
@ml_createname,
@ml_modid,
@ml_modname,
@ml_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@ml_guid", ml_guid);
		oCmd.Parameters.AddWithValue("@ml_name", ml_name);
		oCmd.Parameters.AddWithValue("@ml_createid", ml_createid);
		oCmd.Parameters.AddWithValue("@ml_createname", ml_createname);
		oCmd.Parameters.AddWithValue("@ml_modid", ml_modid);
		oCmd.Parameters.AddWithValue("@ml_modname", ml_modname);
		oCmd.Parameters.AddWithValue("@ml_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateMealsLocation()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsLocation set
ml_name=@ml_name,
ml_modid=@ml_modid,
ml_modname=@ml_modname,
ml_moddate=@ml_moddate
where ml_id=@ml_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@ml_id", ml_id);
		oCmd.Parameters.AddWithValue("@ml_name", ml_name);
		oCmd.Parameters.AddWithValue("@ml_modid", ml_modid);
		oCmd.Parameters.AddWithValue("@ml_modname", ml_modname);
		oCmd.Parameters.AddWithValue("@ml_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteMealsLocation()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsLocation set
ml_status='D',
ml_modid=@ml_modid,
ml_modname=@ml_modname,
ml_moddate=@ml_moddate
where ml_id=@ml_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@ml_id", ml_id);
		oCmd.Parameters.AddWithValue("@ml_modid", ml_modid);
		oCmd.Parameters.AddWithValue("@ml_modname", ml_modname);
		oCmd.Parameters.AddWithValue("@ml_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsLocation where ml_status='A' and ml_guid<>'kitchen' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}