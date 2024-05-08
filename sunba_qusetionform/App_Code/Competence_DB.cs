using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Competence_DB 的摘要描述
/// </summary>
public class Competence_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	string id = string.Empty;
	string guid = string.Empty;
	string 類別 = string.Empty;
	string 類別名稱 = string.Empty;
    string 員工編號 = string.Empty;
	string 建立者 = string.Empty;
	string 建立者id = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	string 修改者id = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region Public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _類別 { set { 類別 = value; } }
	public string _類別名稱 { set { 類別名稱 = value; } }
	public string _員工編號 { set { 員工編號 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public string _建立者id { set { 建立者id = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public string _修改者id { set { 修改者id = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 權限列表 where 資料狀態='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

//	public void addCompetence()
//	{
//		SqlCommand oCmd = new SqlCommand();
//		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
//		StringBuilder sb = new StringBuilder();
//		sb.Append(@"
//declare @dataCount int;

//select @dataCount = count(*) from Competence where c_status='A' and c_type=@c_type

//if @dataCount > 0 
//	begin
//		update Competence set 
//		c_empno=@c_empno,
//		c_modid=@c_modid,
//		c_modname=@c_modname,
//		c_moddate=@c_moddate
//		where c_type=@c_type
//	end
//else
//	begin
//		insert into Competence (
//		c_guid,
//		c_type,
//		c_typename,
//		c_empno,
//		c_createid,
//		c_createname,
//		c_modid,
//		c_modname,
//		c_status
//		) values (
//		@c_guid,
//		@c_type,
//		@c_typename,
//		@c_empno,
//		@c_createid,
//		@c_createname,
//		@c_modid,
//		@c_modname,
//		@c_status
//		)
//	end ");

//		oCmd.CommandText = sb.ToString();
//		oCmd.CommandType = CommandType.Text;
//		oCmd.Parameters.AddWithValue("@c_guid", c_guid);
//		oCmd.Parameters.AddWithValue("@c_type", c_type);
//		oCmd.Parameters.AddWithValue("@c_typename", c_typename);
//		oCmd.Parameters.AddWithValue("@c_empno", c_empno);
//		oCmd.Parameters.AddWithValue("@c_createid", c_createid);
//		oCmd.Parameters.AddWithValue("@c_createname", c_createname);
//		oCmd.Parameters.AddWithValue("@c_modid", c_modid);
//		oCmd.Parameters.AddWithValue("@c_modname", c_modname);
//		oCmd.Parameters.AddWithValue("@c_moddate", DateTime.Now);
//		oCmd.Parameters.AddWithValue("@c_status", "A");

//		oCmd.Connection.Open();
//		oCmd.ExecuteNonQuery();
//		oCmd.Connection.Close();
//	}

	public DataTable GetListOfType()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 權限列表 where 資料狀態='A' and 類別=@類別 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@類別", 類別);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable CheckIsSystemAdmin()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 員工編號 from 權限列表 where 資料狀態='A' and 類別='sa' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@員工編號", 員工編號);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

//	public DataTable GetReviewDormitoryCompetenceList()
//	{
//		SqlCommand oCmd = new SqlCommand();
//		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
//		StringBuilder sb = new StringBuilder();

//		sb.Append(@"
//select value from Competence 
//CROSS APPLY STRING_SPLIT(c_empno,',')
//where c_type='04' or c_type='05' or c_type='sa'
//union
//select fss_signperson from FormSiteSet
//where (fss_main_code='DL' or  fss_main_code='DS') and isnull(fss_signperson,'')<>'' ");

//		oCmd.CommandText = sb.ToString();
//		oCmd.CommandType = CommandType.Text;

//		oCmd.Parameters.AddWithValue("@c_empno", c_empno);

//		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
//		DataTable ds = new DataTable();
//		oda.Fill(ds);
//		return ds;
//	}

	public DataTable GetCompetenceList_Common()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select value from 權限列表 
CROSS APPLY STRING_SPLIT(員工編號,',')
where 類別='sa' ");

		if (類別 != "")
			sb.Append(@"or 類別=@類別 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@類別", 類別);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetCompetenceList_ForDormitory()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select value from 權限列表 
CROSS APPLY STRING_SPLIT(員工編號,',')
where 類別='sa' or 類別='04' or 類別='05' ");
		

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		//oCmd.Parameters.AddWithValue("@c_type", c_type);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetListOfType_ForMail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select value from 權限列表 
CROSS APPLY STRING_SPLIT(員工編號,',')
where 類別=@類別 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@類別", 類別);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}