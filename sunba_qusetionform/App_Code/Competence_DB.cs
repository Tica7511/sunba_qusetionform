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
	string c_id = string.Empty;
	string c_guid = string.Empty;
	string c_type = string.Empty;
	string c_typename = string.Empty;
	string c_empno = string.Empty;
	string c_createid = string.Empty;
	string c_createname = string.Empty;
	DateTime c_createdate;
	string c_modid = string.Empty;
	string c_modname = string.Empty;
	DateTime c_moddate;
	string c_status = string.Empty;
	#endregion
	#region Public
	public string _c_id { set { c_id = value; } }
	public string _c_guid { set { c_guid = value; } }
	public string _c_type { set { c_type = value; } }
	public string _c_typename { set { c_typename = value; } }
	public string _c_empno { set { c_empno = value; } }
	public string _c_createid { set { c_createid = value; } }
	public string _c_createname { set { c_createname = value; } }
	public DateTime _c_createdate { set { c_createdate = value; } }
	public string _c_modid { set { c_modid = value; } }
	public string _c_modname { set { c_modname = value; } }
	public DateTime _c_moddate { set { c_moddate = value; } }
	public string _c_status { set { c_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from Competence where c_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addCompetence()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
declare @dataCount int;

select @dataCount = count(*) from Competence where c_status='A' and c_type=@c_type

if @dataCount > 0 
	begin
		update Competence set 
		c_empno=@c_empno,
		c_modid=@c_modid,
		c_modname=@c_modname,
		c_moddate=@c_moddate
		where c_type=@c_type
	end
else
	begin
		insert into Competence (
		c_guid,
		c_type,
		c_typename,
		c_empno,
		c_createid,
		c_createname,
		c_modid,
		c_modname,
		c_status
		) values (
		@c_guid,
		@c_type,
		@c_typename,
		@c_empno,
		@c_createid,
		@c_createname,
		@c_modid,
		@c_modname,
		@c_status
		)
	end ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@c_guid", c_guid);
		oCmd.Parameters.AddWithValue("@c_type", c_type);
		oCmd.Parameters.AddWithValue("@c_typename", c_typename);
		oCmd.Parameters.AddWithValue("@c_empno", c_empno);
		oCmd.Parameters.AddWithValue("@c_createid", c_createid);
		oCmd.Parameters.AddWithValue("@c_createname", c_createname);
		oCmd.Parameters.AddWithValue("@c_modid", c_modid);
		oCmd.Parameters.AddWithValue("@c_modname", c_modname);
		oCmd.Parameters.AddWithValue("@c_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@c_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetListOfType()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from Competence where c_status='A' and c_type=@c_type ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@c_type", c_type);

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

		sb.Append(@"select c_empno from Competence where c_status='A' and c_type='sa' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@c_empno", c_empno);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetReviewDormitoryCompetenceList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select value from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='04' or c_type='05' or c_type='sa'
union
select fss_signperson from FormSiteSet
where (fss_main_code='DL' or  fss_main_code='DS') and isnull(fss_signperson,'')<>'' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@c_empno", c_empno);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetCompetenceList_Common()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select value from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='sa' ");

		if (c_type != "")
			sb.Append(@"or c_type=@c_type ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@c_type", c_type);

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
select value from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='sa' or c_type='04' or c_type='05' ");
		

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
select value from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type=@c_type ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@c_type", c_type);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}