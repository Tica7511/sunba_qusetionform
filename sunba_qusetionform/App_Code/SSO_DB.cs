using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// SSO_DB 的摘要描述
/// </summary>
public class SSO_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string 編號 = string.Empty;
    string 姓名 = string.Empty;
    string GROUP_ID = string.Empty;
    string GROUP_NAME = string.Empty;
    string 帳號 = string.Empty;
    #endregion
    #region 公用
    public string _id
    {
        set { id = value; }
    }
    public string _編號
    {
        set { 編號 = value; }
    }
    public string _姓名
    {
        set { 姓名 = value; }
    }
    public string _GROUP_ID
    {
        set { GROUP_ID = value; }
    }
    public string _GROUP_NAME
    {
        set { GROUP_NAME = value; }
    }
    public string _帳號
    {
        set { 帳號 = value; }
    }
    #endregion

    public DataTable GetOrgnizationList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["SSOConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@" select GROUP_ID, GROUP_NAME from V_人員資料表2 
  where (GROUP_ID is not null or GROUP_ID<>'') and GROUP_NAME <> '工研院'
  group by GROUP_ID, GROUP_NAME
  order by GROUP_NAME ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        //oCmd.Parameters.AddWithValue("@extend1", extend1);
        //oCmd.Parameters.AddWithValue("@department", department);
        //oCmd.Parameters.AddWithValue("@cname", cname);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetEmpNameList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["SSOConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@" select distinct a.帳號, a.姓名 from V_人員資料表2 a
left join UOF_Training_SUNBA..TB_EB_USER b on a.帳號 COLLATE Chinese_Taiwan_Stroke_CI_AS =b.ACCOUNT ");

        if (!string.IsNullOrEmpty(GROUP_ID))
            sb.Append(@" where GROUP_ID=@GROUP_ID and (b.EXPIRE_DATE >= SYSDATETIMEOFFSET() and b.IS_SUSPENDED<>1) ");
        else
            sb.Append(@" where (b.EXPIRE_DATE >= SYSDATETIMEOFFSET() and b.IS_SUSPENDED<>1) ");

        sb.Append(@" order by 姓名, 帳號 asc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@GROUP_ID", GROUP_ID);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetListByEmpid()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["SSOConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@" select distinct EMAIL, 帳號, 姓名 from V_人員資料表2 ");

        if (!string.IsNullOrEmpty(GROUP_ID))
            sb.Append(@" where 帳號=@帳號 ");

        sb.Append(@" order by 姓名, 帳號, EMAIL asc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@帳號", 帳號);

        oda.Fill(ds);
        return ds;
    }
}