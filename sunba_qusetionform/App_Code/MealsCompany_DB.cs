using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCompany_DB 的摘要描述
/// </summary>
public class MealsCompany_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mc_id = string.Empty;
	string mc_guid = string.Empty;
	string mc_category = string.Empty;
	string mc_name = string.Empty;
	string mc_createid = string.Empty;
	string mc_createname = string.Empty;
	DateTime mc_createdate;
	string mc_modid = string.Empty;
	string mc_modname = string.Empty;
	DateTime mc_moddate;
	string mc_status = string.Empty;
	#endregion
	#region Public
	public string _mc_id { set { mc_id = value; } }
	public string _mc_guid { set { mc_guid = value; } }
	public string _mc_category { set { mc_category = value; } }
	public string _mc_name { set { mc_name = value; } }
	public string _mc_createid { set { mc_createid = value; } }
	public string _mc_createname { set { mc_createname = value; } }
	public DateTime _mc_createdate { set { mc_createdate = value; } }
	public string _mc_modid { set { mc_modid = value; } }
	public string _mc_modname { set { mc_modname = value; } }
	public DateTime _mc_moddate { set { mc_moddate = value; } }
	public string _mc_status { set { mc_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mc.*,CodeTable.C_Item_cn as TypeCn from MealsCompany as mc
left join CodeTable on C_Item=mc_category and C_Group='06'
where mc_status='A' ");

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
		sb.Append(@"insert into MealsCompany (
mc_guid,
mc_category,
mc_name,
mc_createid,
mc_createname,
mc_modid,
mc_modname,
mc_status
) values (
@mc_guid,
@mc_category,
@mc_name,
@mc_createid,
@mc_createname,
@mc_modid,
@mc_modname,
@mc_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);
		oCmd.Parameters.AddWithValue("@mc_category", mc_category);
		oCmd.Parameters.AddWithValue("@mc_name", mc_name);
		oCmd.Parameters.AddWithValue("@mc_createid", mc_createid);
		oCmd.Parameters.AddWithValue("@mc_createname", mc_createname);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateCompany()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCompany set
mc_category=@mc_category,
mc_name=@mc_name,
mc_modid=@mc_modid,
mc_modname=@mc_modname,
mc_moddate=@mc_moddate
where mc_id=@mc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_id", mc_id);
		oCmd.Parameters.AddWithValue("@mc_category", mc_category);
		oCmd.Parameters.AddWithValue("@mc_name", mc_name);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		
		sb.Append(@"select * from MealsCompany where mc_id=@mc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mc_id", mc_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetCompanyName()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mc_name from MealsCompany where mc_guid=@mc_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);

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
		sb.Append(@"update MealsCompany set
mc_status='D',
mc_modid=@mc_modid,
mc_modname=@mc_modname,
mc_moddate=@mc_moddate
where mc_id=@mc_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_id", mc_id);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}