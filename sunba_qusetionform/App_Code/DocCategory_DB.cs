using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// DocCategory_DB 的摘要描述
/// </summary>
public class DocCategory_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region Private
    string dc_id = string.Empty;
    string dc_guid = string.Empty;
    string dc_name = string.Empty;
    string dc_sort = string.Empty;
    string dc_createid = string.Empty;
    string dc_createname = string.Empty;
    DateTime dc_createdate;
    string dc_modid = string.Empty;
    string dc_modname = string.Empty;
    DateTime dc_moddate;
    string dc_status = string.Empty;
    #endregion
    #region Public
    public string _dc_id { set { dc_id = value; } }
    public string _dc_guid { set { dc_guid = value; } }
    public string _dc_name { set { dc_name = value; } }
    public string _dc_sort { set { dc_sort = value; } }
    public string _dc_createid { set { dc_createid = value; } }
    public string _dc_createname { set { dc_createname = value; } }
    public DateTime _dc_createdate { set { dc_createdate = value; } }
    public string _dc_modid { set { dc_modid = value; } }
    public string _dc_modname { set { dc_modname = value; } }
    public DateTime _dc_moddate { set { dc_moddate = value; } }
    public string _dc_status { set { dc_status = value; } }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from DocCategory where dc_status='A' order by dc_sort asc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;

        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oda.Fill(ds);
        return ds;
    }

    public DataTable GetSortValue()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select dc_sort from DocCategory where dc_status='A' order by dc_sort desc "); // 當前顯示排序值 (status='A')

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;

        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oda.Fill(ds);
        return ds;
    }

    public void addDocCategory()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into DocCategory (
dc_guid,
dc_name,
dc_sort,
dc_createid,
dc_createname,
dc_modid,
dc_modname,
dc_status
) values (
@dc_guid,
@dc_name,
@dc_sort,
@dc_createid,
@dc_createname,
@dc_modid,
@dc_modname,
@dc_status
) "
        );

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@dc_guid", dc_guid);
        oCmd.Parameters.AddWithValue("@dc_name", dc_name);
        oCmd.Parameters.AddWithValue("@dc_sort", dc_sort);
        oCmd.Parameters.AddWithValue("@dc_createid", dc_createid);
        oCmd.Parameters.AddWithValue("@dc_createname", dc_createname);
        oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
        oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
        oCmd.Parameters.AddWithValue("@dc_status", "A");

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public void UpdateDocCategory()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update DocCategory set
dc_name=@dc_name,
--dc_sort=@dc_sort,
dc_modid=@dc_modid,
dc_modname=@dc_modname,
dc_moddate=@dc_moddate
where dc_id=@dc_id "
        );

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@dc_id", dc_id);
        oCmd.Parameters.AddWithValue("@dc_name", dc_name);
        //oCmd.Parameters.AddWithValue("@dc_sort", dc_sort);
        oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
        oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
        oCmd.Parameters.AddWithValue("@dc_moddate", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
    public void UpdateSortValue()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update DocCategory set
--dc_name=@dc_name,
dc_sort=@dc_sort,
dc_modid=@dc_modid,
dc_modname=@dc_modname,
dc_moddate=@dc_moddate
where dc_guid=@dc_guid "
        );

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@dc_guid", dc_guid);
        //oCmd.Parameters.AddWithValue("@dc_name", dc_name);
        oCmd.Parameters.AddWithValue("@dc_sort", dc_sort);
        oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
        oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
        oCmd.Parameters.AddWithValue("@dc_moddate", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable GetDetail()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from DocCategory where dc_id=@dc_id ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;

        oCmd.Parameters.AddWithValue("@dc_id", dc_id);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oda.Fill(ds);
        return ds;
    }

    public void DeleteDocCategory()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update DocCategory set
dc_status='D',
dc_modid=@dc_modid,
dc_modname=@dc_modname,
dc_moddate=@dc_moddate
where dc_id=@dc_id ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@dc_id", dc_id);
        oCmd.Parameters.AddWithValue("@dc_modid", dc_modid);
        oCmd.Parameters.AddWithValue("@dc_modname", dc_modname);
        oCmd.Parameters.AddWithValue("@dc_moddate", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}