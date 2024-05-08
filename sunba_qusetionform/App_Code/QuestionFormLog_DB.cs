using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// QuestionFormLog_DB 的摘要描述
/// </summary>
public class QuestionFormLog_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string 類別 = string.Empty;
    string 儲存類別 = string.Empty;
    string 填表人 = string.Empty;
    string 儲存內容 = string.Empty;
    DateTime 建立日期;
    #endregion
    #region 公用
    public string _id { set { id = value; } }
    public string _類別 { set { 類別 = value; } }
    public string _儲存類別 { set { 儲存類別 = value; } }
    public string _填表人 { set { 填表人 = value; } }
    public string _儲存內容 { set { 儲存內容 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    #endregion

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 提問表單儲存log (
類別,
儲存類別,
填表人,
儲存內容
) values (
@類別,
@儲存類別,
@填表人,
@儲存內容)  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@類別", 類別);
        oCmd.Parameters.AddWithValue("@儲存類別", 儲存類別);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@儲存內容", 儲存內容);

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}