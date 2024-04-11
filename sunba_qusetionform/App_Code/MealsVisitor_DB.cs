using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsVisitor_DB 的摘要描述
/// </summary>
public class MealsVisitor_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mv_id = string.Empty;
	string mv_guid = string.Empty;
	string mv_name = string.Empty;
	string mv_reason = string.Empty;
	string mv_date = string.Empty;
	int mv_lunch_meat;
	int mv_lunch_vegetarian;
	int mv_lunch_vegan;
	int mv_dinner_meat;
	int mv_dinner_vegetarian;
	int mv_dinner_vegan;
	string mv_createid = string.Empty;
	string mv_createname = string.Empty;
	DateTime mv_createdate;
	string mv_modid = string.Empty;
	string mv_modname = string.Empty;
	DateTime mv_moddate;
	string mv_status = string.Empty;
	#endregion
	#region Public
	public string _mv_id { set { mv_id = value; } }
	public string _mv_guid { set { mv_guid = value; } }
	public string _mv_name { set { mv_name = value; } }
	public string _mv_reason { set { mv_reason = value; } }
	public string _mv_date { set { mv_date = value; } }
	public int _mv_lunch_meat { set { mv_lunch_meat = value; } }
	public int _mv_lunch_vegetarian { set { mv_lunch_vegetarian = value; } }
	public int _mv_lunch_vegan { set { mv_lunch_vegan = value; } }
	public int _mv_dinner_meat { set { mv_dinner_meat = value; } }
	public int _mv_dinner_vegetarian { set { mv_dinner_vegetarian = value; } }
	public int _mv_dinner_vegan { set { mv_dinner_vegan = value; } }
	public string _mv_createid { set { mv_createid = value; } }
	public string _mv_createname { set { mv_createname = value; } }
	public DateTime _mv_createdate { set { mv_createdate = value; } }
	public string _mv_modid { set { mv_modid = value; } }
	public string _mv_modname { set { mv_modname = value; } }
	public DateTime _mv_moddate { set { mv_moddate = value; } }
	public string _mv_status { set { mv_status = value; } }
	#endregion

	public DataSet GetManageList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select mv.*,ISNULL(fm.fm_result,'') as fm_result into #tmp from MealsVisitor as mv
left join FormMain as fm on fm_data_guid=mv_guid
where mv_status='A' and mv_createid=@mv_createid ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by convert(datetime,mv_date) desc,mv_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mv_createid", LogInfo.empNo);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsVisitor()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsVisitor (
mv_guid,
mv_name,
mv_reason,
mv_date,
mv_lunch_meat,
mv_lunch_vegetarian,
mv_lunch_vegan,
mv_dinner_meat,
mv_dinner_vegetarian,
mv_dinner_vegan,
mv_createid,
mv_createname,
mv_modid,
mv_modname,
mv_status
) values (
@mv_guid,
@mv_name,
@mv_reason,
@mv_date,
@mv_lunch_meat,
@mv_lunch_vegetarian,
@mv_lunch_vegan,
@mv_dinner_meat,
@mv_dinner_vegetarian,
@mv_dinner_vegan,
@mv_createid,
@mv_createname,
@mv_modid,
@mv_modname,
@mv_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mv_guid", mv_guid);
		oCmd.Parameters.AddWithValue("@mv_name", mv_name);
		oCmd.Parameters.AddWithValue("@mv_reason", mv_reason);
		oCmd.Parameters.AddWithValue("@mv_date", mv_date);
		oCmd.Parameters.AddWithValue("@mv_lunch_meat", mv_lunch_meat);
		oCmd.Parameters.AddWithValue("@mv_lunch_vegetarian", mv_lunch_vegetarian);
		oCmd.Parameters.AddWithValue("@mv_lunch_vegan", mv_lunch_vegan);
		oCmd.Parameters.AddWithValue("@mv_dinner_meat", mv_dinner_meat);
		oCmd.Parameters.AddWithValue("@mv_dinner_vegetarian", mv_dinner_vegetarian);
		oCmd.Parameters.AddWithValue("@mv_dinner_vegan", mv_dinner_vegan);
		oCmd.Parameters.AddWithValue("@mv_createid", mv_createid);
		oCmd.Parameters.AddWithValue("@mv_createname", mv_createname);
		oCmd.Parameters.AddWithValue("@mv_modid", mv_modid);
		oCmd.Parameters.AddWithValue("@mv_modname", mv_modname);
		oCmd.Parameters.AddWithValue("@mv_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateMealsVisitor()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsVisitor set
mv_name=@mv_name,
mv_reason=@mv_reason,
mv_date=@mv_date,
mv_lunch_meat=@mv_lunch_meat,
mv_lunch_vegetarian=@mv_lunch_vegetarian,
mv_lunch_vegan=@mv_lunch_vegan,
mv_dinner_meat=@mv_dinner_meat,
mv_dinner_vegetarian=@mv_dinner_vegetarian,
mv_dinner_vegan=@mv_dinner_vegan,
mv_modid=@mv_modid,
mv_modname=@mv_modname,
mv_moddate=@mv_moddate
where mv_id=@mv_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mv_id", mv_id);
		oCmd.Parameters.AddWithValue("@mv_name", mv_name);
		oCmd.Parameters.AddWithValue("@mv_reason", mv_reason);
		oCmd.Parameters.AddWithValue("@mv_date", mv_date);
		oCmd.Parameters.AddWithValue("@mv_lunch_meat", mv_lunch_meat);
		oCmd.Parameters.AddWithValue("@mv_lunch_vegetarian", mv_lunch_vegetarian);
		oCmd.Parameters.AddWithValue("@mv_lunch_vegan", mv_lunch_vegan);
		oCmd.Parameters.AddWithValue("@mv_dinner_meat", mv_dinner_meat);
		oCmd.Parameters.AddWithValue("@mv_dinner_vegetarian", mv_dinner_vegetarian);
		oCmd.Parameters.AddWithValue("@mv_dinner_vegan", mv_dinner_vegan);
		oCmd.Parameters.AddWithValue("@mv_modid", mv_modid);
		oCmd.Parameters.AddWithValue("@mv_modname", mv_modname);
		oCmd.Parameters.AddWithValue("@mv_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteMealsVisitor()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsVisitor set
mv_status='D',
mv_modid=@mv_modid,
mv_modname=@mv_modname,
mv_moddate=@mv_moddate
where mv_guid=@mv_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mv_guid", mv_guid);
		oCmd.Parameters.AddWithValue("@mv_modid", mv_modid);
		oCmd.Parameters.AddWithValue("@mv_modname", mv_modname);
		oCmd.Parameters.AddWithValue("@mv_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsVisitor where mv_id=@mv_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mv_id", mv_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetPaymentVisitorList(string strYear, string strMonth, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select *,CONVERT(int,mv_lunch_meat)+ CONVERT(int,mv_lunch_vegetarian)+ CONVERT(int,mv_lunch_vegan)+ CONVERT(int,mv_dinner_meat)+ CONVERT(int,mv_dinner_vegetarian)+ CONVERT(int,mv_dinner_vegan) as SumNum
,(select top 1 mf_visitor from MealsFee where mf_effectivedate <= mv_date order by mf_effectivedate desc) as priceV
into #tmpVisitor
from MealsVisitor 
left join FormMain on fm_category='MV' and mv_guid=fm_data_guid
where year(convert(datetime,mv_date))=@mrYear and month(convert(datetime,mv_date))=@mrMonth and fm_result = 'Y'

--訪客餐費統計表
select mv_date,mv_createname
,'葷:'+ CONVERT(nvarchar(10),mv_lunch_meat)+' 蛋奶素:'+CONVERT(nvarchar(10),mv_lunch_vegetarian)+' 全素:'+CONVERT(nvarchar(10),mv_lunch_vegan) as lunchNum
,'葷:'+ CONVERT(nvarchar(10),mv_dinner_meat)+' 蛋奶素:'+CONVERT(nvarchar(10),mv_dinner_vegetarian)+' 全素:'+CONVERT(nvarchar(10),mv_dinner_vegan) as dinnerNum
,SumNum*CONVERT(int,ISNULL(priceV,0)) as SumPrice
into #tmp
from #tmpVisitor
where mv_status='A' ");

		if (KeyWord != "")
			sb.Append(@"and (lower(mv_createid)+lower(mv_createname)) like '%' + lower(@KeyWord) + '%' ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by convert(datetime,mv_date)) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetPaymentExportData(string strYear, string strMonth)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select *,CONVERT(int,mv_lunch_meat)+ CONVERT(int,mv_lunch_vegetarian)+ CONVERT(int,mv_lunch_vegan)+ CONVERT(int,mv_dinner_meat)+ CONVERT(int,mv_dinner_vegetarian)+ CONVERT(int,mv_dinner_vegan) as SumNum
,(select top 1 mf_visitor from MealsFee where mf_effectivedate <= mv_date order by mf_effectivedate desc) as priceV
into #tmpVisitor
from MealsVisitor where year(convert(datetime,mv_date))=@mrYear and month(convert(datetime,mv_date))=@mrMonth

--訪客餐費統計表
select mv_date,mv_createname
,'葷:'+ CONVERT(nvarchar(10),mv_lunch_meat)+' 蛋奶素:'+CONVERT(nvarchar(10),mv_lunch_vegetarian)+' 全素:'+CONVERT(nvarchar(10),mv_lunch_vegan) as lunchNum
,'葷:'+ CONVERT(nvarchar(10),mv_dinner_meat)+' 蛋奶素:'+CONVERT(nvarchar(10),mv_dinner_vegetarian)+' 全素:'+CONVERT(nvarchar(10),mv_dinner_vegan) as dinnerNum
,SumNum*CONVERT(int,ISNULL(priceV,0)) as SumPrice
from #tmpVisitor
where mv_status='A' ");

		if (KeyWord != "")
			sb.Append(@"and (lower(mv_createid)+lower(mv_createname)) like '%' + lower(@KeyWord) + '%' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetDetailByGuid()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsVisitor where mv_guid=@mv_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mv_guid", mv_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}