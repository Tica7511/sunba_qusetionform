using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCost_DB 的摘要描述
/// </summary>
public class MealsCost_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mc_id = string.Empty;
	string mc_guid = string.Empty;
	string mc_category = string.Empty;
	string mc_date = string.Empty;
	int mc_price;
	string mc_ps = string.Empty;
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
	public string _mc_date { set { mc_date = value; } }
	public int _mc_price { set { mc_price = value; } }
	public string _mc_ps { set { mc_ps = value; } }
	public string _mc_createid { set { mc_createid = value; } }
	public string _mc_createname { set { mc_createname = value; } }
	public DateTime _mc_createdate { set { mc_createdate = value; } }
	public string _mc_modid { set { mc_modid = value; } }
	public string _mc_modname { set { mc_modname = value; } }
	public DateTime _mc_moddate { set { mc_moddate = value; } }
	public string _mc_status { set { mc_status = value; } }
	#endregion

	public DataSet GetManageList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select * into #tmp from MealsCost where mc_status='A' and mc_category=@mc_category ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by convert(datetime,mc_date) desc,mc_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@mc_category", mc_category);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsCost(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsCost (
mc_guid,
mc_category,
mc_date,
mc_price,
mc_ps,
mc_createid,
mc_createname,
mc_modid,
mc_modname,
mc_status
) values (
@mc_guid,
@mc_category,
@mc_date,
@mc_price,
@mc_ps,
@mc_createid,
@mc_createname,
@mc_modid,
@mc_modname,
@mc_status
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);
		oCmd.Parameters.AddWithValue("@mc_category", mc_category);
		oCmd.Parameters.AddWithValue("@mc_date", mc_date);
		oCmd.Parameters.AddWithValue("@mc_price", mc_price);
		oCmd.Parameters.AddWithValue("@mc_ps", mc_ps);
		oCmd.Parameters.AddWithValue("@mc_createid", mc_createid);
		oCmd.Parameters.AddWithValue("@mc_createname", mc_createname);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void DeleteMealsCost()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCost set
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

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCost where mc_guid=@mc_guid ");
		
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void UpdateMealsCost()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCost set
mc_date=@mc_date,
mc_price=@mc_price,
mc_ps=@mc_ps,
mc_modid=@mc_modid,
mc_modname=@mc_modname,
mc_moddate=@mc_moddate
where mc_guid=@mc_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);
		oCmd.Parameters.AddWithValue("@mc_date", mc_date);
		oCmd.Parameters.AddWithValue("@mc_price", mc_price);
		oCmd.Parameters.AddWithValue("@mc_ps", mc_ps);
		oCmd.Parameters.AddWithValue("@mc_modid", mc_modid);
		oCmd.Parameters.AddWithValue("@mc_modname", mc_modname);
		oCmd.Parameters.AddWithValue("@mc_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetStatisticsTable()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"SELECT
    SUM(CASE WHEN mc_category = 'cost' THEN mc_price ELSE 0 END) AS TotalCost,
    SUM(CASE WHEN mc_category = 'income' THEN mc_price ELSE 0 END) AS TotalIncome
FROM MealsCost
where mc_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		//oCmd.Parameters.AddWithValue("@mc_guid", mc_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetExportList(string startdate,string enddate)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCost where CONVERT(datetime, mc_date) between CONVERT(datetime, @startdate) and CONVERT(datetime, @enddate) 
and  mc_category=@mc_category and mc_status='A'
order by CONVERT(datetime, mc_date) desc ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mc_category", mc_category);
		oCmd.Parameters.AddWithValue("@startdate", startdate);
		oCmd.Parameters.AddWithValue("@enddate", enddate);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}