using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// QuestionForm_DB 的摘要描述
/// </summary>
public class QuestionForm_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string guid = string.Empty;
    string 項次 = string.Empty;
    string 編號 = string.Empty;
    string 問題類別 = string.Empty;
    string 年度 = string.Empty;
    string 月份 = string.Empty;
    string 序號 = string.Empty;
    string 員工編號 = string.Empty;
    string 填表人 = string.Empty;
    string 部門_id = string.Empty;
    string 部門 = string.Empty;
    string 提出日期 = string.Empty;
    string 程度 = string.Empty;
    string 目前狀態 = string.Empty;
    string 內容 = string.Empty;
    string 回覆日期 = string.Empty;
    string 預計完成日 = string.Empty;
    string 回覆內容 = string.Empty;
    string 資料狀態 = string.Empty;
    string 排序名稱 = string.Empty;
    string 排序狀態 = string.Empty;
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
    public string _項次 { set { 項次 = value; } }
    public string _編號 { set { 編號 = value; } }
    public string _問題類別 { set { 問題類別 = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _月份 { set { 月份 = value; } }
    public string _序號 { set { 序號 = value; } }
    public string _員工編號 { set { 員工編號 = value; } }
    public string _填表人 { set { 填表人 = value; } }
    public string _部門_id { set { 部門_id = value; } }
    public string _部門 { set { 部門 = value; } }
    public string _提出日期 { set { 提出日期 = value; } }
    public string _程度 { set { 程度 = value; } }
    public string _目前狀態 { set { 目前狀態 = value; } }
    public string _內容 { set { 內容 = value; } }
    public string _回覆日期 { set { 回覆日期 = value; } }
    public string _預計完成日 { set { 預計完成日 = value; } }
    public string _回覆內容 { set { 回覆內容 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    public string _排序名稱 { set { 排序名稱 = value; } }
    public string _排序狀態 { set { 排序狀態 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public string _建立者id { set { 建立者id = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 建立者 = value; } }
    public string _修改者id { set { 修改者id = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    #endregion

    public DataTable GetList(string startday, string endday)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *, 
項次,年度,月份,
問題類別_V=(select 項目名稱 from 代碼檔 where 群組代碼='001' and 代碼檔.項目代碼=提問表單.問題類別),
程度_V=(select 項目名稱 from 代碼檔 where 群組代碼='005' and 代碼檔.項目代碼=提問表單.程度),
目前狀態_V=(select 項目名稱 from 代碼檔 where 群組代碼='004' and 代碼檔.項目代碼=提問表單.目前狀態),
( select dbo.clearTag( 內容 ) )as 內容_V,
( select dbo.clearTag( 回覆內容 ) )as 回覆內容_V 
from 提問表單 where 資料狀態='A' 
 and (@項次='' or 項次 like '%' + @項次 + '%') and (@編號='' or 編號 like '%' + @編號 + '%') and (@目前狀態='' or 目前狀態=@目前狀態) 
and (@內容='' or 內容 like '%' + @內容 + '%') and (@回覆內容='' or 回覆內容 like '%' + @回覆內容 + '%') 
and (@問題類別='' or 問題類別=@問題類別) and (@員工編號='' or 員工編號=@員工編號) and (@部門_id='' or 部門_id=@部門_id) 
");
        if (!string.IsNullOrEmpty(startday) && !string.IsNullOrEmpty(endday))
            sb.Append(@" and 提出日期 between convert(int, @查詢起日) and convert(int, @查詢迄日)");

        if (!string.IsNullOrEmpty(程度))
        {
            string[] strtemp = 程度.Split(',');
            switch(strtemp.Length)
            {
                case 1:
                    sb.Append(@" and (程度='" + strtemp[0] + "')");
                    break;
                case 2:
                    sb.Append(@" and (程度='" + strtemp[0]+ "' or 程度='" + strtemp[1] + "')");
                    break;
                case 3:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "'or 程度='" + strtemp[2] + "')");
                    break;
            }
            //if (strtemp.Length == 1)
            //{
            //    sb.Append(@" and (程度=" + strtemp[0]);
            //}
            //else
            //{
            //    for (int i = 0; i < strtemp.Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            sb.Append(@" and (程度=" + strtemp[0]);
            //        }
            //        else
            //        {
            //            sb.Append(@" or 程度=" + strtemp[i]);
            //        }
            //    }
            //}

            //sb.Append(@") ");
        }

        if (!string.IsNullOrEmpty(排序名稱))
            if (排序狀態 == "DESC")
            {
                switch (排序名稱)
                {
                    case "編號":
                        sb.Append(@"order by 編號 DESC");
                        break;
                    case "問題類別":
                        sb.Append(@"order by 問題類別 DESC");
                        break;
                    case "員工編號":
                        sb.Append(@"order by 員工編號 DESC");
                        break;
                    case "填表人":
                        sb.Append(@"order by 填表人 DESC");
                        break;
                    case "部門":
                        sb.Append(@"order by 部門 DESC");
                        break;
                    case "提出日期":
                        sb.Append(@"order by 提出日期 DESC");
                        break;
                    case "預計完成日":
                        sb.Append(@"order by 預計完成日 DESC");
                        break;
                    case "程度":
                        sb.Append(@"order by 程度 DESC");
                        break;
                    case "目前狀態":
                        sb.Append(@"order by 目前狀態 DESC");
                        break;
                }
            }
            else
            {
                switch (排序名稱)
                {
                    case "編號":
                        sb.Append(@"order by 編號 ASC");
                        break;
                    case "問題類別":
                        sb.Append(@"order by 問題類別 ASC");
                        break;
                    case "員工編號":
                        sb.Append(@"order by 員工編號 ASC");
                        break;
                    case "填表人":
                        sb.Append(@"order by 填表人 ASC");
                        break;
                    case "部門":
                        sb.Append(@"order by 部門 ASC");
                        break;
                    case "提出日期":
                        sb.Append(@"order by 提出日期 ASC");
                        break;
                    case "預計完成日":
                        sb.Append(@"order by 預計完成日 ASC");
                        break;
                    case "程度":
                        sb.Append(@"order by 程度 ASC");
                        break;
                    case "目前狀態":
                        sb.Append(@"order by 目前狀態 ASC");
                        break;
                }
            }
        else
            sb.Append(@" order by convert(int, 項次) desc");

                oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@項次", 項次);
        oCmd.Parameters.AddWithValue("@編號", 編號);
        oCmd.Parameters.AddWithValue("@問題類別", 問題類別);
        oCmd.Parameters.AddWithValue("@員工編號", 員工編號);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@部門_id", 部門_id);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@查詢起日", startday);
        oCmd.Parameters.AddWithValue("@查詢迄日", endday);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);
        oCmd.Parameters.AddWithValue("@排序名稱", 排序名稱);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetList(string pStart, string pEnd, string startday, string endday)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *,
問題類別_V=(select 項目名稱 from 代碼檔 where 群組代碼='001' and 代碼檔.項目代碼=提問表單.問題類別),
程度_V=(select 項目名稱 from 代碼檔 where 群組代碼='005' and 代碼檔.項目代碼=提問表單.程度),
目前狀態_V=(select 項目名稱 from 代碼檔 where 群組代碼='004' and 代碼檔.項目代碼=提問表單.目前狀態),
( select dbo.clearTag( 內容 ) )as 內容_V,
( select dbo.clearTag( 回覆內容 ) )as 回覆內容_V 
into #tmp 
from 提問表單 where 資料狀態='A' 
 and (@項次='' or 項次 like '%' + @項次 + '%') and (@編號='' or 編號 like '%' + @編號 + '%') and (@目前狀態='' or 目前狀態=@目前狀態) 
and (@內容='' or 內容 like '%' + @內容 + '%') and (@回覆內容='' or 回覆內容 like '%' + @回覆內容 + '%') 
and (@問題類別='' or 問題類別=@問題類別) and (@員工編號='' or 員工編號=@員工編號) and (@部門_id='' or 部門_id=@部門_id) 
");
        if (!string.IsNullOrEmpty(startday) && !string.IsNullOrEmpty(endday))
            sb.Append(@" and 提出日期 between convert(int, @查詢起日) and convert(int, @查詢迄日)");

        if (!string.IsNullOrEmpty(程度))
        {
            string[] strtemp = 程度.Split(',');
            switch (strtemp.Length)
            {
                case 1:
                    sb.Append(@" and (程度='" + strtemp[0] + "')");
                    break;
                case 2:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "')");
                    break;
                case 3:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "'or 程度='" + strtemp[2] + "')");
                    break;
            }
        }

        sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (");

        if (!string.IsNullOrEmpty(排序名稱))
            if (排序狀態 == "desc")
            {
                switch (排序名稱)
                {
                    case "編號":
                        sb.Append(@"order by 編號 DESC");
                        break;
                    case "問題類別":
                        sb.Append(@"order by 問題類別 DESC");
                        break;
                    case "員工編號":
                        sb.Append(@"order by 員工編號 DESC");
                        break;
                    case "填表人":
                        sb.Append(@"order by 填表人 DESC");
                        break;
                    case "部門":
                        sb.Append(@"order by 部門 DESC");
                        break;
                    case "提出日期":
                        sb.Append(@"order by 提出日期 DESC");
                        break;
                    case "預計完成日":
                        sb.Append(@"order by 預計完成日 DESC");
                        break;
                    case "程度":
                        sb.Append(@"order by 程度 DESC");
                        break;
                    case "目前狀態":
                        sb.Append(@"order by 目前狀態 DESC");
                        break;
                }
            }
            else
            {
                switch (排序名稱)
                {
                    case "編號":
                        sb.Append(@"order by 編號 ASC");
                        break;
                    case "問題類別":
                        sb.Append(@"order by 問題類別 ASC");
                        break;
                    case "員工編號":
                        sb.Append(@"order by 員工編號 ASC");
                        break;
                    case "填表人":
                        sb.Append(@"order by 填表人 ASC");
                        break;
                    case "部門":
                        sb.Append(@"order by 部門 ASC");
                        break;
                    case "提出日期":
                        sb.Append(@"order by 提出日期 ASC");
                        break;
                    case "預計完成日":
                        sb.Append(@"order by 預計完成日 ASC");
                        break;
                    case "程度":
                        sb.Append(@"order by 程度 ASC");
                        break;
                    case "目前狀態":
                        sb.Append(@"order by 目前狀態 ASC");
                        break;
                }
            }
        else
            sb.Append(@" order by convert(int, 項次) desc");

        sb.Append(@") itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@項次", 項次);
        oCmd.Parameters.AddWithValue("@編號", 編號);
        oCmd.Parameters.AddWithValue("@問題類別", 問題類別);
        oCmd.Parameters.AddWithValue("@員工編號", 員工編號);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@部門_id", 部門_id);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@查詢起日", startday);
        oCmd.Parameters.AddWithValue("@查詢迄日", endday);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);
        oCmd.Parameters.AddWithValue("@排序名稱", 排序名稱);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetExcelList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *, 
項次,舊編號,年度,月份,
公司別_V=(select 項目名稱 from 代碼檔 where 群組代碼='001' and 代碼檔.項目代碼=提問表單.公司別),
case 公司別 when '01' then (select 項目名稱 from 代碼檔 where 群組代碼='002' and 代碼檔.項目代碼=提問表單.單位)
when '02' then (select 項目名稱 from 代碼檔 where 群組代碼='003' and 代碼檔.項目代碼=提問表單.單位) end '單位_V',
程度_V=(select 項目名稱 from 代碼檔 where 群組代碼='005' and 代碼檔.項目代碼=提問表單.程度),
目前狀態_V=(select 項目名稱 from 代碼檔 where 群組代碼='004' and 代碼檔.項目代碼=提問表單.目前狀態),
( select dbo.clearTag( 內容 ) )as 內容_V,
( select dbo.clearTag( 回覆內容 ) )as 回覆內容_V 
from 提問表單 where 資料狀態='A' 
 and (@項次='' or 項次 like '%' + @項次 + '%') and (@編號='' or 編號 like '%' + @編號 + '%') and (@填表人='' or 填表人=@填表人) and (@公司別='' or 公司別=@公司別) 
and (@單位='' or 單位=@單位) and (@目前狀態='' or 目前狀態=@目前狀態) and (@內容='' or 內容 like '%' + @內容 + '%' or 舊編號 like '%' + @內容 + '%') 
and (@回覆內容='' or 回覆內容 like '%' + @回覆內容 + '%' or 舊編號 like '%' + @回覆內容 + '%')  
and convert(int, SUBSTRING(編號,1,4) + SUBSTRING(編號,6,2) + SUBSTRING(編號,9,2))<=20230299
");

        if (!string.IsNullOrEmpty(程度))
        {
            string[] strtemp = 程度.Split(',');
            switch (strtemp.Length)
            {
                case 1:
                    sb.Append(@" and (程度='" + strtemp[0] + "')");
                    break;
                case 2:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "')");
                    break;
                case 3:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "'or 程度='" + strtemp[2] + "')");
                    break;
            }
        }

        sb.Append(@" order by convert(int, 項次) DESC");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@項次", 項次);
        oCmd.Parameters.AddWithValue("@編號", 編號);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetExcelList2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *, 
項次,舊編號,年度,月份,
公司別_V=(select 項目名稱 from 代碼檔 where 群組代碼='001' and 代碼檔.項目代碼=提問表單.公司別),
case 公司別 when '01' then (select 項目名稱 from 代碼檔 where 群組代碼='002' and 代碼檔.項目代碼=提問表單.單位)
when '02' then (select 項目名稱 from 代碼檔 where 群組代碼='003' and 代碼檔.項目代碼=提問表單.單位) end '單位_V',
程度_V=(select 項目名稱 from 代碼檔 where 群組代碼='005' and 代碼檔.項目代碼=提問表單.程度),
目前狀態_V=(select 項目名稱 from 代碼檔 where 群組代碼='004' and 代碼檔.項目代碼=提問表單.目前狀態),
( select dbo.clearTag( 內容 ) )as 內容_V,
( select dbo.clearTag( 回覆內容 ) )as 回覆內容_V 
from 提問表單 where 資料狀態='A' 
 and (@項次='' or 項次 like '%' + @項次 + '%') and (@編號='' or 編號 like '%' + @編號 + '%') and (@填表人='' or 填表人=@填表人) and (@公司別='' or 公司別=@公司別) 
and (@單位='' or 單位=@單位) and (@目前狀態='' or 目前狀態=@目前狀態) and (@內容='' or 內容 like '%' + @內容 + '%' or 舊編號 like '%' + @內容 + '%') 
and (@回覆內容='' or 回覆內容 like '%' + @回覆內容 + '%' or 舊編號 like '%' + @回覆內容 + '%') 
and convert(int, SUBSTRING(編號,1,4) + SUBSTRING(編號,6,2) + SUBSTRING(編號,9,2))>20230299
");

        if (!string.IsNullOrEmpty(程度))
        {
            string[] strtemp = 程度.Split(',');
            switch (strtemp.Length)
            {
                case 1:
                    sb.Append(@" and (程度='" + strtemp[0] + "')");
                    break;
                case 2:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "')");
                    break;
                case 3:
                    sb.Append(@" and (程度='" + strtemp[0] + "' or 程度='" + strtemp[1] + "'or 程度='" + strtemp[2] + "')");
                    break;
            }
        }

        sb.Append(@" order by convert(int, 項次) DESC");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@項次", 項次);
        oCmd.Parameters.AddWithValue("@編號", 編號);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@目前狀態", 目前狀態);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@回覆內容", 回覆內容);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select *  
from 提問表單 
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

    public DataTable GetDataTran(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
select *  
from 提問表單 
where guid=@guid and 資料狀態='A' 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
        oda.Fill(ds);
        return ds;
    }

    public DataTable GetSn()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select Max(序號) as 序號 
from 提問表單 
where 年度=@年度 and 月份=@月份 and 資料狀態='A' 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@月份", 月份);
        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMaxitem()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select Max(convert(int,項次)) + 1 as 項次 
from 提問表單 
where 資料狀態='A' 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 提問表單 (
guid,
問題類別,
項次,
年度,
月份,
編號,
序號,
員工編號,
填表人,
部門_id,
部門,
提出日期,
程度,
內容,
建立者,
建立者id,
修改者,
修改者id,
資料狀態 
) values (
@guid,
@問題類別,
@項次,
@年度,
@月份,
@編號,
@序號,
@員工編號,
@填表人,
@部門_id,
@部門,
@提出日期,
@程度,
@內容,
@建立者,
@建立者id,
@修改者,
@修改者id,
@資料狀態)  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@問題類別", 問題類別);
        oCmd.Parameters.AddWithValue("@項次", 項次);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@月份", 月份);
        oCmd.Parameters.AddWithValue("@編號", 編號);
        oCmd.Parameters.AddWithValue("@序號", 序號);
        oCmd.Parameters.AddWithValue("@員工編號", 員工編號);
        oCmd.Parameters.AddWithValue("@填表人", 填表人);
        oCmd.Parameters.AddWithValue("@部門_id", 部門_id);
        oCmd.Parameters.AddWithValue("@部門", 部門);
        oCmd.Parameters.AddWithValue("@提出日期", 提出日期);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@內容", 內容);
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
update 提問表單 set 
程度=@程度,
內容=@內容,
修改者=@修改者, 
修改者id=@修改者id, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@程度", 程度);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改者id", 修改者id);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
update 提問表單 set 
目前狀態=@目前狀態,
回覆日期=@回覆日期,
預計完成日=@預計完成日,
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
        oCmd.Parameters.AddWithValue("@回覆日期", 回覆日期);
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
        oCmd.CommandText = @"update 提問表單 set 
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

    public DataTable GetStatisticState()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select 
項目名稱, 
總計=(select count(*) from 提問表單 where 目前狀態=代碼檔.項目代碼  and convert(int, 項次)>170 and 資料狀態='A')
from 代碼檔 where 群組代碼='004'
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetStatisticState2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select 
項目名稱, 
總計=(select count(*) from 提問表單 where 目前狀態=代碼檔.項目代碼  and convert(int, 項次)<=170 and 資料狀態='A')
from 代碼檔 where 群組代碼='004'
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetStatisticType()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select 
項目名稱, 
總計=(select count(*) from 提問表單 where 程度=代碼檔.項目代碼  and convert(int, 項次)>170 and 資料狀態='A')
from 代碼檔 where 群組代碼='005'
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetStatisticType2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select 
項目名稱, 
總計=(select count(*) from 提問表單 where 程度=代碼檔.項目代碼  and convert(int, 項次)<=170 and 資料狀態='A')
from 代碼檔 where 群組代碼='005'
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }
}