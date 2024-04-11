using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCancel_DB 的摘要描述
/// </summary>
public class MealsCancel_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mc_id = string.Empty;
	string mc_guid = string.Empty;
	string mc_reason = string.Empty;
	string mc_item = string.Empty;
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
	public string _mc_reason { set { mc_reason = value; } }
	public string _mc_item { set { mc_item = value; } }
	public string _mc_createid { set { mc_createid = value; } }
	public string _mc_createname { set { mc_createname = value; } }
	public DateTime _mc_createdate { set { mc_createdate = value; } }
	public string _mc_modid { set { mc_modid = value; } }
	public string _mc_modname { set { mc_modname = value; } }
	public DateTime _mc_moddate { set { mc_moddate = value; } }
	public string _mc_status { set { mc_status = value; } }
	#endregion

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"  select * from MealsCancel where mc_guid=@mc_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}


	public void AddMealsCancel()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
		insert into MealsCancel (
		mc_guid,
		mc_reason,
		mc_item,
		mc_createid,
		mc_createname,
		mc_modid,
		mc_modname,
		mc_status
		) values (
		@mc_guid,
		@mc_reason,
		@mc_item,
		@mc_createid,
		@mc_createname,
		@mc_modid,
		@mc_modname,
		@mc_status
		) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);
		oCmd.Parameters.AddWithValue("@mc_reason", mc_reason);
		oCmd.Parameters.AddWithValue("@mc_item", mc_item);
		oCmd.Parameters.AddWithValue("@mc_createid", mc_createid);
		oCmd.Parameters.AddWithValue("@mc_createname", mc_createname);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}