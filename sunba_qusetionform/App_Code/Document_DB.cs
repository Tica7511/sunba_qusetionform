using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;

/// <summary>
/// Document_DB 的摘要描述
/// </summary>
public class Document_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region Private
    string d_id = string.Empty;
    string d_guid = string.Empty;
    string d_pubdate = string.Empty;
    string d_category = string.Empty;
    string d_no = string.Empty;
    string d_name = string.Empty;
    string d_version = string.Empty;
    string d_dept = string.Empty;
    string d_manager = string.Empty;
    string d_createid = string.Empty;
    string d_createname = string.Empty;
    DateTime d_createdate;
    string d_modid = string.Empty;
    string d_modname = string.Empty;
    DateTime d_moddate;
    string d_status = string.Empty;
    #endregion
    #region Public
    public string _d_id { set { d_id = value; } }
    public string _d_guid { set { d_guid = value; } }
    public string _d_pubdate { set { d_pubdate = value; } }
    public string _d_category { set { d_category = value; } }
    public string _d_no { set { d_no = value; } }
    public string _d_name { set { d_name = value; } }
    public string _d_version { set { d_version = value; } }
    public string _d_dept { set { d_dept = value; } }
    public string _d_manager { set { d_manager = value; } }
    public string _d_createid { set { d_createid = value; } }
    public string _d_createname { set { d_createname = value; } }
    public DateTime _d_createdate { set { d_createdate = value; } }
    public string _d_modid { set { d_modid = value; } }
    public string _d_modname { set { d_modname = value; } }
    public DateTime _d_moddate { set { d_moddate = value; } }
    public string _d_status { set { d_status = value; } }
    #endregion

    public DataSet GetList(string pStart, string pEnd, string sortName, string sortMethod)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
		
        sb.Append(@"select 
d_id,
d_guid,
d_category,
d_pubdate,
d_no,
d_name,
d_version,
dc_name,
dc_sort
into #tmp 
from Document 
left join DocCategory on dc_guid=d_category
where d_status='A' ");

		if (d_category != "")
		{
			sb.Append(@"and d_category=@d_category ");
		}
		if (KeyWord != "")
		{
			sb.Append(@"and lower(d_name+d_no) like '%' + lower(@KeyWord) + '%' ");
		}

        sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by ");

		switch (sortName)
		{
			case "d_no":
				if (sortMethod == "asc")
					sb.Append(@"d_no,");
				else
					sb.Append(@"d_no desc,");
				break;
			case "dc_sort":
				if (sortMethod == "asc")
					sb.Append(@"dc_sort,");
				else
					sb.Append(@"dc_sort desc,");
				break;
		}

		sb.Append(@"d_pubdate,d_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;

        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
        oCmd.Parameters.AddWithValue("@d_category", d_category);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();
        oda.Fill(ds);
        return ds;
    }

    public void addDocument(SqlConnection oConn, SqlTransaction oTran)
    {
        //SqlCommand oCmd = new SqlCommand();
        //oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into Document (
d_guid,
d_pubdate,
d_category,
d_no,
d_name,
d_version,
d_dept,
d_manager,
d_createid,
d_createname,
d_modid,
d_modname,
d_status
) values (
@d_guid,
@d_pubdate,
@d_category,
@d_no,
@d_name,
@d_version,
@d_dept,
@d_manager,
@d_createid,
@d_createname,
@d_modid,
@d_modname,
@d_status
) "
        );

        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@d_guid", d_guid);
        oCmd.Parameters.AddWithValue("@d_pubdate", d_pubdate);
        oCmd.Parameters.AddWithValue("@d_category", d_category);
        oCmd.Parameters.AddWithValue("@d_no", d_no);
        oCmd.Parameters.AddWithValue("@d_name", d_name);
        oCmd.Parameters.AddWithValue("@d_version", d_version);
        oCmd.Parameters.AddWithValue("@d_dept", d_dept);
        oCmd.Parameters.AddWithValue("@d_manager", d_manager);
        oCmd.Parameters.AddWithValue("@d_createid", d_createid);
        oCmd.Parameters.AddWithValue("@d_createname", d_createname);
        oCmd.Parameters.AddWithValue("@d_modid", d_modid);
        oCmd.Parameters.AddWithValue("@d_modname", d_modname);
        oCmd.Parameters.AddWithValue("@d_status", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();

        //oCmd.Connection.Open();
        //oCmd.ExecuteNonQuery();
        //oCmd.Connection.Close();
    }

    public void UpdateDocument()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update Document set
d_pubdate=@d_pubdate,
d_category=@d_category,
d_no=@d_no,
d_name=@d_name,
d_version=@d_version,
d_dept=@d_dept,
d_manager=@d_manager,
d_modid=@d_modid,
d_modname=@d_modname,
d_moddate=@d_moddate
where d_guid=@d_guid "
        );

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@d_guid", d_guid);
        oCmd.Parameters.AddWithValue("@d_pubdate", d_pubdate);
        oCmd.Parameters.AddWithValue("@d_category", d_category);
        oCmd.Parameters.AddWithValue("@d_no", d_no);
        oCmd.Parameters.AddWithValue("@d_name", d_name);
        oCmd.Parameters.AddWithValue("@d_version", d_version);
        oCmd.Parameters.AddWithValue("@d_dept", d_dept);
        oCmd.Parameters.AddWithValue("@d_manager", d_manager);
        oCmd.Parameters.AddWithValue("@d_modid", d_modid);
        oCmd.Parameters.AddWithValue("@d_modname", d_modname);
        oCmd.Parameters.AddWithValue("@d_moddate", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable GetDetail()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from Document where d_guid=@d_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;

        oCmd.Parameters.AddWithValue("@d_guid", d_guid);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oda.Fill(ds);
        return ds;
    }

    public void DeleteDocument()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update Document set
d_status='D',
d_modid=@d_modid,
d_modname=@d_modname,
d_moddate=@d_moddate
where d_id=@d_id ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@d_id", d_id);
        oCmd.Parameters.AddWithValue("@d_modid", d_modid);
        oCmd.Parameters.AddWithValue("@d_modname", d_modname);
        oCmd.Parameters.AddWithValue("@d_moddate", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}