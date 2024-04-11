using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsFee_DB 的摘要描述
/// </summary>
public class MealsFee_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mf_id = string.Empty;
	int mf_employee;
	int mf_firm;
	int mf_visitor;
	int mf_love;
	string mf_effectivedate = string.Empty;
	string mf_createid = string.Empty;
	string mf_createname = string.Empty;
	DateTime mf_createdate;
	string mf_modid = string.Empty;
	string mf_modname = string.Empty;
	DateTime mf_moddate;
	string mf_status = string.Empty;
	#endregion
	#region Public
	public string _mf_id { set { mf_id = value; } }
	public int _mf_employee { set { mf_employee = value; } }
	public int _mf_firm { set { mf_firm = value; } }
	public int _mf_visitor { set { mf_visitor = value; } }
	public int _mf_love { set { mf_love = value; } }
	public string _mf_effectivedate { set { mf_effectivedate = value; } }
	public string _mf_createid { set { mf_createid = value; } }
	public string _mf_createname { set { mf_createname = value; } }
	public DateTime _mf_createdate { set { mf_createdate = value; } }
	public string _mf_modid { set { mf_modid = value; } }
	public string _mf_modname { set { mf_modname = value; } }
	public DateTime _mf_moddate { set { mf_moddate = value; } }
	public string _mf_status { set { mf_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsFee where mf_status='A' 
order by convert(datetime, mf_effectivedate) desc ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsFee()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsFee (
mf_employee,
mf_firm,
mf_visitor,
mf_love,
mf_effectivedate,
mf_createid,
mf_createname,
mf_modid,
mf_modname,
mf_status
) values (
@mf_employee,
@mf_firm,
@mf_visitor,
@mf_love,
@mf_effectivedate,
@mf_createid,
@mf_createname,
@mf_modid,
@mf_modname,
@mf_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mf_employee", mf_employee);
		oCmd.Parameters.AddWithValue("@mf_firm", mf_firm);
		oCmd.Parameters.AddWithValue("@mf_visitor", mf_visitor);
		oCmd.Parameters.AddWithValue("@mf_love", mf_love);
		oCmd.Parameters.AddWithValue("@mf_effectivedate", mf_effectivedate);
		oCmd.Parameters.AddWithValue("@mf_createid", mf_createid);
		oCmd.Parameters.AddWithValue("@mf_createname", mf_createname);
		oCmd.Parameters.AddWithValue("@mf_modid", mf_modid);
		oCmd.Parameters.AddWithValue("@mf_modname", mf_modname);
		oCmd.Parameters.AddWithValue("@mf_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateMealsFee()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsFee set
mf_employee=@mf_employee,
mf_firm=@mf_firm,
mf_visitor=@mf_visitor,
mf_love=@mf_love,
mf_effectivedate=@mf_effectivedate,
mf_modid=@mf_modid,
mf_modname=@mf_modname,
mf_moddate=@mf_moddate
where mf_id=@mf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mf_id", mf_id);
		oCmd.Parameters.AddWithValue("@mf_employee", mf_employee);
		oCmd.Parameters.AddWithValue("@mf_firm", mf_firm);
		oCmd.Parameters.AddWithValue("@mf_visitor", mf_visitor);
		oCmd.Parameters.AddWithValue("@mf_love", mf_love);
		oCmd.Parameters.AddWithValue("@mf_effectivedate", mf_effectivedate);
		oCmd.Parameters.AddWithValue("@mf_modid", mf_modid);
		oCmd.Parameters.AddWithValue("@mf_modname", mf_modname);
		oCmd.Parameters.AddWithValue("@mf_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteMealsFee()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsFee set
mf_status='D',
mf_modid=@mf_modid,
mf_modname=@mf_modname,
mf_moddate=@mf_moddate
where mf_id=@mf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mf_id", mf_id);
		oCmd.Parameters.AddWithValue("@mf_modid", mf_modid);
		oCmd.Parameters.AddWithValue("@mf_modname", mf_modname);
		oCmd.Parameters.AddWithValue("@mf_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsFee where mf_id=@mf_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mf_id", mf_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}