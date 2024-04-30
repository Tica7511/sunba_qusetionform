using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Reply_DB 的摘要描述
/// </summary>
public class Reply_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string guid = string.Empty;
    string 父層guid = string.Empty;
    string 回覆日期 = string.Empty;
    string 預計完成日 = string.Empty;
    string 目前狀態 = string.Empty;
    string 需求是否在第一期合約中 = string.Empty;
    string 回覆內容 = string.Empty;
    string 資料狀態 = string.Empty;
    string 建立者 = string.Empty;
    string 建立者id = string.Empty;
    DateTime 修改日期;
    string 修改者 = string.Empty;
    string 修改者id = string.Empty;
    DateTime 建立日期;
    #endregion
    #region 公用
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _父層guid { set { 父層guid = value; } }
    public string _回覆日期 { set { 回覆日期 = value; } }
    public string _預計完成日 { set { 預計完成日 = value; } }
    public string _目前狀態 { set { 目前狀態 = value; } }
    public string _需求是否在第一期合約中 { set { 需求是否在第一期合約中 = value; } }
    public string _回覆內容 { set { 回覆內容 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public string _建立者id { set { 建立者id = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public string _修改者id { set { 修改者id = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    #endregion

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select *  
from 回覆表單 
where guid=@guid and 資料狀態='A' 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oda.Fill(ds);
        return ds;
    }

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 回覆表單 (
guid,
回覆日期,
預計完成日,
目前狀態,
需求是否在第一期合約中,
回覆內容,
建立者,
建立者id,
修改者,
修改者id,
資料狀態 
) values (
@guid,
@回覆日期,
@預計完成日,
@目前狀態,
@需求是否在第一期合約中,
@回覆內容,
@建立者,
@建立者id,
@修改者,
@修改者id,
@資料狀態)  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@回覆日期", 回覆日期);
        oCmd.Parameters.AddWithValue("@預計完成日", 預計完成日);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@需求是否在第一期合約中", 需求是否在第一期合約中);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立者id", 建立者id);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改者id", 修改者id);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
update 回覆表單 set 
目前狀態=@目前狀態,
預計完成日=@預計完成日,
需求是否在第一期合約中=@需求是否在第一期合約中,
回覆內容=@回覆內容,
修改者=@修改者,
修改者id=@修改者id,
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@預計完成日", 預計完成日);
        oCmd.Parameters.AddWithValue("@需求是否在第一期合約中", 需求是否在第一期合約中);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改者id", 修改者id);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 回覆表單 set 
修改者=@修改者, 
修改者id=@修改者id, 
修改日期=@修改日期, 
資料狀態='D' 
where guid=@guid ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改者id", 修改者id);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}