using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsRegister_DB 的摘要描述
/// </summary>
public class MealsRegister_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mr_id = string.Empty;
	string mr_guid = string.Empty;
	string mr_person_id = string.Empty;
	string mr_type = string.Empty;
	string mr_date = string.Empty;
	string mr_lunch = string.Empty;
	int mr_lunch_num;
	string mr_lunch_location = string.Empty;
	string mr_dinner = string.Empty;
	int mr_dinner_num;
	string mr_dinner_location = string.Empty;
	string mr_cancel_reason = string.Empty;
	string mr_cancel_item = string.Empty;
	string mr_createid = string.Empty;
	string mr_createname = string.Empty;
	DateTime mr_createdate;
	string mr_modid = string.Empty;
	string mr_modname = string.Empty;
	DateTime mr_moddate;
	string mr_status = string.Empty;
	#endregion
	#region Public
	public string _mr_id { set { mr_id = value; } }
	public string _mr_guid { set { mr_guid = value; } }
	public string _mr_person_id { set { mr_person_id = value; } }
	public string _mr_type { set { mr_type = value; } }
	public string _mr_date { set { mr_date = value; } }
	public string _mr_lunch { set { mr_lunch = value; } }
	public int _mr_lunch_num { set { mr_lunch_num = value; } }
	public string _mr_lunch_location { set { mr_lunch_location = value; } }
	public string _mr_dinner { set { mr_dinner = value; } }
	public int _mr_dinner_num { set { mr_dinner_num = value; } }
	public string _mr_dinner_location { set { mr_dinner_location = value; } }
	public string _mr_cancel_reason { set { mr_cancel_reason = value; } }
	public string _mr_cancel_item { set { mr_cancel_item = value; } }
	public string _mr_createid { set { mr_createid = value; } }
	public string _mr_createname { set { mr_createname = value; } }
	public DateTime _mr_createdate { set { mr_createdate = value; } }
	public string _mr_modid { set { mr_modid = value; } }
	public string _mr_modname { set { mr_modname = value; } }
	public DateTime _mr_moddate { set { mr_moddate = value; } }
	public string _mr_status { set { mr_status = value; } }
	#endregion

	public DataTable GetMealsList(string sDate, string eDate)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
-- 簽核資料
select 
fm_id,
CONVERT(nvarchar(10), fm_createdate,121) as fm_createdate,
fm_createid,
isnull(fm_result,'') as fm_result,
mc_item
into #CancelTmp
from FormMain 
left join MealsCancel on mc_guid=fm_data_guid
where fm_category='MC'

-- 用餐登記
select 
mr_date,
mr_lunch,
mr_lunch_num,
mr_lunch_location,
mr_dinner,
mr_dinner_num,
mr_dinner_location,
(select top 1 case when fm_result='' then 'Unreview' else fm_result end from #CancelTmp where fm_createid=@mr_person_id and CONVERT(nvarchar(10), fm_createdate,121)=mr_date and mc_item='午餐' order by fm_id desc) as CancelLunch,
(select top 1 case when fm_result='' then 'Unreview' else fm_result end from #CancelTmp where fm_createid=@mr_person_id and CONVERT(nvarchar(10), fm_createdate,121)=mr_date and mc_item='晚餐' order by fm_id desc) as CancelDinner
from MealsRegister 
where mr_status='A' and mr_type=@mr_type and mr_person_id=@mr_person_id
 and CONVERT(datetime, mr_date) between CONVERT(datetime, @sDate) and CONVERT(datetime,  @eDate) 

drop table #CancelTmp ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@sDate", sDate);
		oCmd.Parameters.AddWithValue("@eDate", eDate);
		oCmd.Parameters.AddWithValue("@mr_type", mr_type);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable CheckThisRange()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsRegister where mr_status='A' and mr_type=@mr_type and mr_date=@mr_date and mr_person_id=@mr_person_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mr_type", mr_type);
		oCmd.Parameters.AddWithValue("@mr_date", mr_date);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsRegister(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
declare @dataCount int;

select @dataCount = count(*) from MealsRegister where mr_status='A' and mr_date=@mr_date and mr_type=@mr_type and mr_person_id=@mr_person_id

if @dataCount > 0 
	begin
		update MealsRegister set 
		mr_lunch=@mr_lunch,
		mr_lunch_num=@mr_lunch_num,
		mr_lunch_location=@mr_lunch_location,
		mr_dinner=@mr_dinner,
		mr_dinner_num=@mr_dinner_num,
		mr_dinner_location=@mr_dinner_location,
		mr_modid=@mr_modid,
		mr_modname=@mr_modname,
		mr_moddate=@mr_moddate
		where mr_date=@mr_date and mr_person_id=@mr_person_id
	end
else
	begin
		insert into MealsRegister (
		mr_guid,
		mr_person_id,
		mr_type,
		mr_date,
		mr_lunch,
		mr_lunch_num,
		mr_lunch_location,
		mr_dinner,
		mr_dinner_num,
		mr_dinner_location,
		mr_createid,
		mr_createname,
		mr_modid,
		mr_modname,
		mr_status
		) values (
		@mr_guid,
		@mr_person_id,
		@mr_type,
		@mr_date,
		@mr_lunch,
		@mr_lunch_num,
		@mr_lunch_location,
		@mr_dinner,
		@mr_dinner_num,
		@mr_dinner_location,
		@mr_createid,
		@mr_createname,
		@mr_modid,
		@mr_modname,
		@mr_status
		)
	end ");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mr_guid", mr_guid);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);
		oCmd.Parameters.AddWithValue("@mr_type", mr_type);
		oCmd.Parameters.AddWithValue("@mr_date", mr_date);
		oCmd.Parameters.AddWithValue("@mr_lunch", mr_lunch);
		oCmd.Parameters.AddWithValue("@mr_lunch_num", mr_lunch_num);
		oCmd.Parameters.AddWithValue("@mr_lunch_location", mr_lunch_location);
		oCmd.Parameters.AddWithValue("@mr_dinner", mr_dinner);
		oCmd.Parameters.AddWithValue("@mr_dinner_num", mr_dinner_num);
		oCmd.Parameters.AddWithValue("@mr_dinner_location", mr_dinner_location);
		oCmd.Parameters.AddWithValue("@mr_createid", mr_createid);
		oCmd.Parameters.AddWithValue("@mr_createname", mr_createname);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@mr_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public DataSet GetEmployeeFeeList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--查詢條件年分
select SUBSTRING(mr_date,1,4) as selYear 
from MealsRegister 
where mr_status='A'
group by SUBSTRING(mr_date,1,4)
order by SUBSTRING(mr_date,1,4) desc

--統計資料 傳入參數 @selYear、@selEmpno
declare @selYear nvarchar(4)=@mr_date
declare @selEmpno nvarchar(20)=@mr_person_id
create table #tmpMonth(
	strMonth nvarchar(2)
)
declare @intLoop int =1;
declare @strLoopMonth nvarchar(2)=''

while @intLoop<=12--12個月
	begin
		--月份左邊補0
		if @intLoop < 10
			select @strLoopMonth = '0'+convert(nvarchar(2),@intLoop)
		else
			select @strLoopMonth = convert(nvarchar(2),@intLoop)
		insert into #tmpMonth values(@strLoopMonth)
		select @intLoop = @intLoop+1
	end

--午餐明細 數量及單價
select mr_person_id, mr_date,mr_lunch_num,(select top 1 mf_employee from MealsFee where mf_effectivedate <= mr_date and mf_status='A' order by CONVERT(datetime, mf_effectivedate) desc) as price
into #tmpLunch 
from MealsRegister
where mr_type='01' and mr_status='A' and mr_person_id=@selEmpno and SUBSTRING(mr_date,1,4)=@selYear and mr_lunch='Y'
--晚餐明細 數量及單價
select mr_person_id, mr_date,mr_dinner_num ,(select top 1 mf_employee from MealsFee where mf_effectivedate <= mr_date and mf_status='A' order by CONVERT(datetime, mf_effectivedate) desc) as price
into #tmpDinner from MealsRegister
where mr_type='01' and mr_status='A' and mr_person_id=@selEmpno and SUBSTRING(mr_date,1,4)=@selYear and mr_dinner='Y'

--最後的資料
select @selYear as strYear,strMonth,numSum
--負值表示再算餐費的時候有異常(抓不到餐費設定)，直接給空白
,case when priceSum<0 then '' else CONVERT(nvarchar(10),priceSum) end as priceSum
from (
	select strMonth
	,ISNULL(lunchSum,0)+ISNULL(dinnerSum,0) as numSum 
	,ISNULL(lunchSumPrice,0)+ISNULL(dinnerSumPrice,0) as priceSum 
	from (
		select strMonth
		,(select SUM(mr_lunch_num)
		from #tmpLunch
		where strMonth=SUBSTRING(mr_date,6,2)) as lunchSum
		,(select SUM(mr_lunch_num*isnull(price,-1000000))--如果抓不到設定的餐費 就給負值
		from #tmpLunch
		where strMonth=SUBSTRING(mr_date,6,2)) as lunchSumPrice
		,(select SUM(mr_dinner_num)
		from #tmpDinner
		where strMonth=SUBSTRING(mr_date,6,2)) as dinnerSum
		,(select SUM(mr_dinner_num*isnull(price,-1000000))--如果抓不到設定的餐費 就給負值
		from #tmpDinner
		where strMonth=SUBSTRING(mr_date,6,2)) as dinnerSumPrice
		from #tmpMonth
	)t
)t2

drop table #tmpMonth
drop table #tmpLunch
drop table #tmpDinner ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mr_date", mr_date);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetEmployeeFeeDetail(string strYear, string strMonth, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
mr_date,
case when mr_lunch='Y' then '是' else '否' end as mr_lunch,
mr_lunch_num,
Lml.ml_name as mr_lunch_location,
case when mr_dinner='Y' then '是' else '否' end as mr_dinner,
mr_dinner_num,
Dml.ml_name as mr_dinner_location
into #tmp
from MealsRegister
left join MealsLocation as Lml on mr_lunch_location=Lml.ml_guid
left join MealsLocation as Dml on mr_dinner_location=Dml.ml_guid
where mr_status='A' and mr_person_id=@mr_person_id
and Year(convert(datetime,mr_date))=@strYear
and MONTH(convert(datetime,mr_date))=@strMonth 

--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by convert(datetime,mr_date)) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetPaymentYearAndMonthList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select Year(convert(datetime,mr_date)) as strYear from MealsRegister where mr_status='A'
union
select Year(convert(datetime,mv_date)) as strYear from MealsVisitor where mv_status='A'

select Month(convert(datetime,mr_date)) as monthNum,
CASE
        WHEN Month(convert(datetime,mr_date)) = '1' THEN '一月'
        WHEN Month(convert(datetime,mr_date)) = '2' THEN '二月'
        WHEN Month(convert(datetime,mr_date)) = '3' THEN '三月'
        WHEN Month(convert(datetime,mr_date)) = '4' THEN '四月'
        WHEN Month(convert(datetime,mr_date)) = '5' THEN '五月'
        WHEN Month(convert(datetime,mr_date)) = '6' THEN '六月'
        WHEN Month(convert(datetime,mr_date)) = '7' THEN '七月'
        WHEN Month(convert(datetime,mr_date)) = '8' THEN '八月'
        WHEN Month(convert(datetime,mr_date)) = '9' THEN '九月'
        WHEN Month(convert(datetime,mr_date)) = '10' THEN '十月'
        WHEN Month(convert(datetime,mr_date)) = '11' THEN '十一月'
        WHEN Month(convert(datetime,mr_date)) = '12' THEN '十二月'
    END AS strMonth
from MealsRegister where mr_status='A'
union
select Month(convert(datetime,mv_date)) as monthNum,
CASE
        WHEN Month(convert(datetime,mv_date)) = '1' THEN '一月'
        WHEN Month(convert(datetime,mv_date)) = '2' THEN '二月'
        WHEN Month(convert(datetime,mv_date)) = '3' THEN '三月'
        WHEN Month(convert(datetime,mv_date)) = '4' THEN '四月'
        WHEN Month(convert(datetime,mv_date)) = '5' THEN '五月'
        WHEN Month(convert(datetime,mv_date)) = '6' THEN '六月'
        WHEN Month(convert(datetime,mv_date)) = '7' THEN '七月'
        WHEN Month(convert(datetime,mv_date)) = '8' THEN '八月'
        WHEN Month(convert(datetime,mv_date)) = '9' THEN '九月'
        WHEN Month(convert(datetime,mv_date)) = '10' THEN '十月'
        WHEN Month(convert(datetime,mv_date)) = '11' THEN '十一月'
        WHEN Month(convert(datetime,mv_date)) = '12' THEN '十二月'
    END AS strMonth
from MealsVisitor where mv_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		//oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetPaymentEmployeeList(string strYear, string strMonth, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select a.mr_id,a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and year(convert(datetime,a.mr_date))=@mrYear and month(convert(datetime,a.mr_date))=@mrMonth

--同仁餐費統計表
select mr_person_id,EMPLOYEE_CNAME as EmpName,DEPARTMENT_CNAME as EmpDept
,(SUM(isnull(lunchNum,0)*CONVERT(int,ISNULL(priceE,0))+isnull(dinnerNum,0)*CONVERT(int,ISNULL(priceE,0)))) as SumPrice 
into #tmp
from #tmpRegister 
left join [70783499]..vwZZ_EMPLOYEE on mr_person_id=EMPLOYEE_NO
where mr_type='01' ");

		if (mr_person_id != "")
			sb.Append(@"and lower(EMPLOYEE_NO) like '%' + lower(@mr_person_id) + '%' ");

		sb.Append(@"group by mr_person_id,EMPLOYEE_CNAME,DEPARTMENT_CNAME order by mr_person_id ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by mr_person_id) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetPaymentCompanyList(string strYear, string strMonth, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select a.mr_id,a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and year(convert(datetime,a.mr_date))=@mrYear and month(convert(datetime,a.mr_date))=@mrMonth

--廠商餐費統計表
select mr_person_id,mc_name,ct.C_Item_cn as mc_category,mr_type
,(
SUM(
  (isnull(lunchNum,0)+isnull(dinnerNum,0))*　
  case mr_type when '02' then CONVERT(int,ISNULL(priceF,0)) when '03' then CONVERT(int,ISNULL(priceL,0)) else 0 end
 )
) as SumPrice
into #tmp
from #tmpRegister 
left join MealsCompany on mr_person_id=mc_guid and mr_type=mc_category
left join CodeTable as ct on ct.C_Item=mc_category and ct.C_Group='06'
where (mr_type='02' or  mr_type='03') ");

		if (KeyWord != "")
			sb.Append(@"and lower(mc_name) like '%' + lower(@KeyWord) + '%' ");

		sb.Append(@"group by mr_person_id,mc_name,ct.C_Item_cn,mr_type order by mr_person_id ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by mc_name) itemNo,* from #tmp
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

	public DataTable GetPaymentTotalList(string strYear, string strMonth)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select a.mr_id,a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and year(convert(datetime,a.mr_date))=@mrYear and month(convert(datetime,a.mr_date))=@mrMonth

select *,CONVERT(int,mv_lunch_meat)+ CONVERT(int,mv_lunch_vegetarian)+ CONVERT(int,mv_lunch_vegan)+ CONVERT(int,mv_dinner_meat)+ CONVERT(int,mv_dinner_vegetarian)+ CONVERT(int,mv_dinner_vegan) as SumNum
,(select top 1 mf_visitor from MealsFee where mf_effectivedate <= mv_date order by mf_effectivedate desc) as priceV
into #tmpVisitor
from MealsVisitor where year(convert(datetime,mv_date))=@mrYear and month(convert(datetime,mv_date))=@mrMonth and mv_status='A' 

select 
(select SUM(isnull(lunchNum,0)*CONVERT(int,ISNULL(priceE,0))+isnull(dinnerNum,0)*CONVERT(int,ISNULL(priceE,0))) from #tmpRegister where mr_type='01') as Employee
,(select SUM(isnull(lunchNum,0)*CONVERT(int,ISNULL(priceF,0))+isnull(dinnerNum,0)*CONVERT(int,ISNULL(priceF,0))) from #tmpRegister where mr_type='02') as Firm
,(select SUM(isnull(lunchNum,0)*CONVERT(int,ISNULL(priceL,0))+isnull(dinnerNum,0)*CONVERT(int,ISNULL(priceL,0))) from #tmpRegister where mr_type='03')	as Love
,(select SUM(isnull(SumNum,0)*CONVERT(int,ISNULL(priceV,0))) from #tmpVisitor)	as Visitor ");


		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetPaymentExportData(string strYear, string strMonth,string type)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--將當月所有用餐資料撈出來
select a.mr_id,a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by convert(datetime,mf_effectivedate) desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and year(convert(datetime,a.mr_date))=@mrYear and month(convert(datetime,a.mr_date))=@mrMonth ");

		if (type == "Employee")
		{
			sb.Append(@"
--同仁餐費統計表
select mr_person_id,EMPLOYEE_CNAME as EmpName,DEPARTMENT_CNAME as EmpDept
,(SUM(isnull(lunchNum,0)*CONVERT(int,ISNULL(priceE,0))+isnull(dinnerNum,0)*CONVERT(int,ISNULL(priceE,0)))) as SumPrice 
from #tmpRegister 
left join [70783499]..vwZZ_EMPLOYEE on mr_person_id=EMPLOYEE_NO
where mr_type='01' ");

			if (mr_person_id != "")
				sb.Append(@"and lower(EMPLOYEE_NO) like '%' + lower(@mr_person_id) + '%' ");

			sb.Append(@"group by mr_person_id,EMPLOYEE_CNAME,DEPARTMENT_CNAME order by mr_person_id ");
		}
		else
		{
			sb.Append(@"
--廠商餐費統計表
select mr_person_id,mc_name,ct.C_Item_cn as mc_category,mr_type
,(
SUM(
  (isnull(lunchNum,0)+isnull(dinnerNum,0))*　
  case mr_type when '02' then CONVERT(int,ISNULL(priceF,0)) when '03' then CONVERT(int,ISNULL(priceL,0)) else 0 end
 )
) as SumPrice
from #tmpRegister 
left join MealsCompany on mr_person_id=mc_guid and mr_type=mc_category
left join CodeTable as ct on ct.C_Item=mc_category and ct.C_Group='06'
where (mr_type='02' or  mr_type='03') ");

			if (KeyWord != "")
				sb.Append(@"and lower(mc_name) like '%' + lower(@KeyWord) + '%' ");

			sb.Append(@"group by mr_person_id,mc_name,ct.C_Item_cn,mr_type order by mr_person_id ");
		}

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@strYear", strYear);
		oCmd.Parameters.AddWithValue("@strMonth", strMonth);
		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@mr_person_id", mr_person_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetMealsStatistics(string strYear, string strMonth, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
--傳入參數
declare @mrYear nvarchar(4)=@strYear--年
declare @mrMonth nvarchar(2)=@strMonth--月

--用餐地點，把廚房放在最後
select * into #tmpLocation from(
select ml_guid,ml_name from MealsLocation where ml_status='A' and ml_guid<>'kitchen'
union
select ml_guid,ml_name from MealsLocation where ml_status='A' and ml_guid='kitchen'
)t

--分頁，MealsRegister+MealsVisitor的日期
select mr_date into #tmpPageDate  from (
	select mr_date from MealsRegister where mr_status='A' and year(convert(datetime,mr_date))=@mrYear and month(convert(datetime,mr_date))=@mrMonth
	union
	select mv_date as mr_date from MealsVisitor where year(convert(datetime,mv_date))=@mrYear and month(convert(datetime,mv_date))=@mrMonth and mv_status='A'
)t

select mr_date into #tmpDate  from (
    select ROW_NUMBER() over (order by mr_date asc) itemNo,mr_date from #tmpPageDate
)#tmp where itemNo between @pStart and @pEnd

--將當月所有用餐資料撈出來
select a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_lunch_location from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchLocation
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select mr_dinner_location from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerLocation
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and a.mr_date in (select mr_date from #tmpDate)

select *,CONVERT(int,mv_lunch_meat)+ CONVERT(int,mv_lunch_vegetarian)+ CONVERT(int,mv_lunch_vegan)+ CONVERT(int,mv_dinner_meat)+ CONVERT(int,mv_dinner_vegetarian)+ CONVERT(int,mv_dinner_vegan) as SumNum
,(select top 1 mf_visitor from MealsFee where mf_effectivedate <= mv_date order by mf_effectivedate desc) as priceV
,CONVERT(int,mv_lunch_meat) as lunch_meat,CONVERT(int,mv_lunch_vegetarian) as lunch_vegetarian,CONVERT(int,mv_lunch_vegan) as lunch_vegan
,CONVERT(int,mv_dinner_meat) as dinner_meat,CONVERT(int,mv_dinner_vegetarian) as dinner_vegetarian,CONVERT(int,mv_dinner_vegan) as dinner_vegan
into #tmpVisitor
from MealsVisitor 
left join FormMain on fm_data_guid=mv_guid 
where  mv_status='A' and mv_date in (select mr_date from #tmpDate) and fm_result='Y'

--把MealsRegister跟MealsVisitor弄一起
select * into #tmpMerge from (
	select mr_date,mr_type
	,lunchNum,lunchLocation
	,dinnerNum,dinnerLocation
	,priceE,priceF,priceL,0 as priceV 
	,0 as lunch_meat,0 as lunch_vegetarian,0 as lunch_vegan
	,0 as dinner_meat,0 as dinner_vegetarian,0 as dinner_vegan
	from #tmpRegister
	union all
	select mv_date as mr_date,'04' as mr_type
	,(lunch_meat+lunch_vegetarian+lunch_vegan) as lunchNum,'kitchen' as lunchLocation
	,(dinner_meat+dinner_vegetarian+dinner_vegan) as dinnerNum,'kitchen' as dinnerLocation
	,0 as priceE,0 as priceF,0 as priceL,priceV
	,lunch_meat,lunch_vegetarian,lunch_vegan
	,dinner_meat,dinner_vegetarian,dinner_vegan
	from #tmpVisitor
)t

--把用餐地點跟日期整個弄成一張表
select ml_guid,ml_name,mr_date into #tmpLocationAndDate
from #tmpLocation
CROSS  join (
	select mr_date from #tmpMerge group by mr_date
)t

select
mr_date,ml_guid,ml_name
,isnull(lunchE,0) as lunchE,isnull(lunchF,0) as lunchF,isnull(lunchL,0) as lunchL,isnull(lunchV,0) as lunchV
,isnull(dinnerE,0) as dinnerE,isnull(dinnerF,0) as dinnerF,isnull(dinnerL,0) as dinnerL,isnull(dinnerV,0) as dinnerV
,isnull(sumLunchV,0) as sumLunchV,isnull(sumDinnerV,0) as sumDinnerV
,isnull(priceE,0) as priceE,isnull(priceF,0) as priceF,isnull(priceL,0) as priceL,isnull(priceV,0) as priceV
into #tmpDetail
from (
	select a.mr_date,a.ml_guid,a.ml_name
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='01') as lunchE
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='02') as lunchF
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='03') as lunchL
	,(select convert(nvarchar(10),SUM(b.lunch_meat))+'/'+convert(nvarchar(10),SUM(b.lunch_vegetarian))+'/'+convert(nvarchar(10),SUM(b.lunch_vegan)) from #tmpMerge b where a.ml_guid=b.lunchLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as lunchV
	,(select SUM(b.lunch_meat)+SUM(b.lunch_vegetarian)+SUM(b.lunch_vegan) from #tmpMerge b where a.ml_guid=b.lunchLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as sumLunchV
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='01') as dinnerE
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='02') as dinnerF
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='03') as dinnerL
	,(select convert(nvarchar(10),SUM(b.dinner_meat))+'/'+convert(nvarchar(10),SUM(b.dinner_vegetarian))+'/'+convert(nvarchar(10),SUM(b.dinner_vegan)) from #tmpMerge b where a.ml_guid=b.dinnerLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as dinnerV
	,(select SUM(b.dinner_meat)+SUM(b.dinner_vegetarian)+SUM(b.dinner_vegan) from #tmpMerge b where a.ml_guid=b.dinnerLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as sumDinnerV
	,(select top 1 priceE from #tmpMerge b where a.mr_date=b.mr_date ) as priceE
	,(select top 1 priceF from #tmpMerge b where a.mr_date=b.mr_date ) as priceF
	,(select top 1 priceL from #tmpMerge b where a.mr_date=b.mr_date ) as priceL
	,(select top 1 priceV from #tmpMerge b where a.mr_date=b.mr_date and priceV<>0 ) as priceV
	from #tmpLocationAndDate a
)t
order by mr_date,ml_guid

--合計
select mr_date 
,SUM(lunchE)+SUM(lunchF)+SUM(lunchL)+SUM(sumLunchV) as totalLunchPrice
,SUM(dinnerE)+SUM(dinnerF)+SUM(dinnerL)+SUM(sumDinnerV) as totalDinnerPrice
into #tmpTotal
from #tmpDetail
group by mr_date,priceE,priceF,priceL,priceV

--用餐地點數量
select count(*) as LocationTotal from MealsLocation where ml_status='A'

--總筆數分頁用
select count(*) as total from #tmpPageDate

--最後資料輸出
select a.mr_date,a.ml_name,a.lunchE,a.lunchF,a.lunchL,a.lunchV,b.totalLunchPrice
,a.dinnerE,a.dinnerF,a.dinnerL,a.dinnerV,b.totalDinnerPrice
from #tmpDetail a left join #tmpTotal b on a.mr_date=b.mr_date
order by a.mr_date,a.ml_guid


drop table #tmpLocation
drop table #tmpPageDate
drop table #tmpDate
drop table #tmpLocationAndDate
drop table #tmpRegister
drop table #tmpMerge
drop table #tmpVisitor
drop table #tmpDetail
drop table #tmpTotal ");

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

	public void UpdateMealsCancel(string fms_guid)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
declare @empno nvarchar(50) -- 工號
declare @mrDate nvarchar(50) -- 取消日期
declare @c_item nvarchar(50) -- 午 / 晚餐

select 
@empno=fm_createid,
@mrDate=convert(nvarchar(10),fm_createdate,121),
@c_item=mc_item
from FormMainSite
left join FormMain on fms_parentid=fm_guid
left join MealsCancel on mc_guid=fm_data_guid
where fms_guid=@fms_guid

if @c_item='午餐'
	begin
		update MealsRegister set 
		mr_lunch='N',
		mr_modid=@mr_modid,
		mr_modname=@mr_modname,
		mr_moddate=@mr_moddate
		where mr_date=@mrDate and mr_person_id=@empno
	end
else
	begin
		update MealsRegister set 
		mr_dinner='N',
		mr_modid=@mr_modid,
		mr_modname=@mr_modname,
		mr_moddate=@mr_moddate
		where mr_date=@mrDate and mr_person_id=@empno
	end ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void ManagerMealsCancel(string empno,string MealsItem)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsRegister set ");

		if (MealsItem == "午餐")
			sb.Append(@"mr_lunch='N', ");
		else
			sb.Append(@"mr_dinner='N', ");

		sb.Append(@"
		mr_modid=@mr_modid,
		mr_modname=@mr_modname,
		mr_moddate=@mr_moddate
		where mr_date=convert(nvarchar(10), getdate(),121) and mr_person_id=@empno ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@empno", empno);
		oCmd.Parameters.AddWithValue("@mr_modid", mr_modid);
		oCmd.Parameters.AddWithValue("@mr_modname", mr_modname);
		oCmd.Parameters.AddWithValue("@mr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

    //匯出當周資料 語法跟GetMealsStatistics一樣 只是直接帶當周日期
    public DataSet GetExportMealsStatistics(string startdate, string enddate)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
--用餐地點，把廚房放在最後
select * into #tmpLocation from(
select ml_guid,ml_name from MealsLocation where ml_status='A' and ml_guid<>'kitchen'
union
select ml_guid,ml_name from MealsLocation where ml_status='A' and ml_guid='kitchen'
)t

--分頁，MealsRegister+MealsVisitor的日期
select mr_date into #tmpPageDate  from (
	select mr_date from MealsRegister where mr_status='A' and convert(datetime,mr_date) between @startdate and @enddate
	union
	select mv_date as mr_date from MealsVisitor where convert(datetime,mv_date) between @startdate and @enddate and mv_status='A'
)t

select mr_date into #tmpDate  from (
    select ROW_NUMBER() over (order by mr_date asc) itemNo,mr_date from #tmpPageDate
)#tmp where itemNo between 1 and 7

--將當月所有用餐資料撈出來
select a.mr_date,a.mr_type,a.mr_person_id,a.mr_createname
,(select mr_lunch_num from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchNum
,(select mr_lunch_location from MealsRegister b where a.mr_status='A' and a.mr_lunch='Y' and a.mr_id=b.mr_id ) as lunchLocation
,(select mr_dinner_num from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerNum
,(select mr_dinner_location from MealsRegister b where a.mr_status='A' and a.mr_dinner='Y' and a.mr_id=b.mr_id ) as dinnerLocation
,(select top 1 mf_employee from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceE
,(select top 1 mf_firm from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceF
,(select top 1 mf_love from MealsFee where mf_effectivedate <= a.mr_date and mf_status='A' order by mf_effectivedate desc) as priceL
into #tmpRegister
from MealsRegister a where a.mr_status='A' and a.mr_date in (select mr_date from #tmpDate)

select *,CONVERT(int,mv_lunch_meat)+ CONVERT(int,mv_lunch_vegetarian)+ CONVERT(int,mv_lunch_vegan)+ CONVERT(int,mv_dinner_meat)+ CONVERT(int,mv_dinner_vegetarian)+ CONVERT(int,mv_dinner_vegan) as SumNum
,(select top 1 mf_visitor from MealsFee where mf_effectivedate <= mv_date order by mf_effectivedate desc) as priceV
,CONVERT(int,mv_lunch_meat) as lunch_meat,CONVERT(int,mv_lunch_vegetarian) as lunch_vegetarian,CONVERT(int,mv_lunch_vegan) as lunch_vegan
,CONVERT(int,mv_dinner_meat) as dinner_meat,CONVERT(int,mv_dinner_vegetarian) as dinner_vegetarian,CONVERT(int,mv_dinner_vegan) as dinner_vegan
into #tmpVisitor
from MealsVisitor 
left join FormMain on fm_data_guid=mv_guid 
where  mv_status='A' and mv_date in (select mr_date from #tmpDate) and fm_result='Y'

--把MealsRegister跟MealsVisitor弄一起
select * into #tmpMerge from (
	select mr_date,mr_type
	,lunchNum,lunchLocation
	,dinnerNum,dinnerLocation
	,priceE,priceF,priceL,0 as priceV 
	,0 as lunch_meat,0 as lunch_vegetarian,0 as lunch_vegan
	,0 as dinner_meat,0 as dinner_vegetarian,0 as dinner_vegan
	from #tmpRegister
	union all
	select mv_date as mr_date,'04' as mr_type
	,(lunch_meat+lunch_vegetarian+lunch_vegan) as lunchNum,'kitchen' as lunchLocation
	,(dinner_meat+dinner_vegetarian+dinner_vegan) as dinnerNum,'kitchen' as dinnerLocation
	,0 as priceE,0 as priceF,0 as priceL,priceV
	,lunch_meat,lunch_vegetarian,lunch_vegan
	,dinner_meat,dinner_vegetarian,dinner_vegan
	from #tmpVisitor
)t

--把用餐地點跟日期整個弄成一張表
select ml_guid,ml_name,mr_date into #tmpLocationAndDate
from #tmpLocation
CROSS  join (
	select mr_date from #tmpMerge group by mr_date
)t

select
mr_date,ml_guid,ml_name
,isnull(lunchE,0) as lunchE,isnull(lunchF,0) as lunchF,isnull(lunchL,0) as lunchL,isnull(lunchV,0) as lunchV
,isnull(dinnerE,0) as dinnerE,isnull(dinnerF,0) as dinnerF,isnull(dinnerL,0) as dinnerL,isnull(dinnerV,0) as dinnerV
,isnull(sumLunchV,0) as sumLunchV,isnull(sumDinnerV,0) as sumDinnerV
,isnull(priceE,0) as priceE,isnull(priceF,0) as priceF,isnull(priceL,0) as priceL,isnull(priceV,0) as priceV
into #tmpDetail
from (
	select a.mr_date,a.ml_guid,a.ml_name
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='01') as lunchE
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='02') as lunchF
	,(select SUM(isnull(b.lunchNum,0)) from #tmpMerge b where a.ml_guid=b.lunchLocation and a.mr_date = b.mr_date and b.mr_type='03') as lunchL
	,(select convert(nvarchar(10),SUM(b.lunch_meat))+'/'+convert(nvarchar(10),SUM(b.lunch_vegetarian))+'/'+convert(nvarchar(10),SUM(b.lunch_vegan)) from #tmpMerge b where a.ml_guid=b.lunchLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as lunchV
	,(select SUM(b.lunch_meat)+SUM(b.lunch_vegetarian)+SUM(b.lunch_vegan) from #tmpMerge b where a.ml_guid=b.lunchLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as sumLunchV
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='01') as dinnerE
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='02') as dinnerF
	,(select SUM(isnull(b.dinnerNum,0)) from #tmpMerge b where a.ml_guid=b.dinnerLocation and a.mr_date = b.mr_date and b.mr_type='03') as dinnerL
	,(select convert(nvarchar(10),SUM(b.dinner_meat))+'/'+convert(nvarchar(10),SUM(b.dinner_vegetarian))+'/'+convert(nvarchar(10),SUM(b.dinner_vegan)) from #tmpMerge b where a.ml_guid=b.dinnerLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as dinnerV
	,(select SUM(b.dinner_meat)+SUM(b.dinner_vegetarian)+SUM(b.dinner_vegan) from #tmpMerge b where a.ml_guid=b.dinnerLocation  and a.mr_date = b.mr_date  and b.mr_type='04') as sumDinnerV
	,(select top 1 priceE from #tmpMerge b where a.mr_date=b.mr_date ) as priceE
	,(select top 1 priceF from #tmpMerge b where a.mr_date=b.mr_date ) as priceF
	,(select top 1 priceL from #tmpMerge b where a.mr_date=b.mr_date ) as priceL
	,(select top 1 priceV from #tmpMerge b where a.mr_date=b.mr_date and priceV<>0 ) as priceV
	from #tmpLocationAndDate a
)t
order by mr_date,ml_guid

--合計
select mr_date 
,SUM(lunchE)+SUM(lunchF)+SUM(lunchL)+SUM(sumLunchV) as totalLunchPrice
,SUM(dinnerE)+SUM(dinnerF)+SUM(dinnerL)+SUM(sumDinnerV) as totalDinnerPrice
into #tmpTotal
from #tmpDetail
group by mr_date,priceE,priceF,priceL,priceV

--用餐地點數量
select count(*) as LocationTotal from MealsLocation where ml_status='A'

--總筆數分頁用
select count(*) as total from #tmpPageDate

--最後資料輸出
select a.mr_date,a.ml_name,a.lunchE,a.lunchF,a.lunchL,a.lunchV,b.totalLunchPrice
,a.dinnerE,a.dinnerF,a.dinnerL,a.dinnerV,b.totalDinnerPrice
from #tmpDetail a left join #tmpTotal b on a.mr_date=b.mr_date
order by a.mr_date,a.ml_guid


drop table #tmpLocation
drop table #tmpPageDate
drop table #tmpDate
drop table #tmpLocationAndDate
drop table #tmpRegister
drop table #tmpMerge
drop table #tmpVisitor
drop table #tmpDetail
drop table #tmpTotal ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        oCmd.Parameters.AddWithValue("@startdate", startdate);
        oCmd.Parameters.AddWithValue("@enddate", enddate);

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();
        oda.Fill(ds);
        return ds;
    }

	//匯出當周用餐人員名單
	public DataSet GetExportMealsPersonList(string today)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
declare @exportDate datetime = convert(datetime, @today)

-- 同仁/廠商/愛心便當
select
mr_type,
mr_person_id,
mr_date,
mr_lunch,
mr_lunch_num,
ll.ml_name as LunchLocation,
mr_dinner,
mr_dinner_num,
dl.ml_name as DinnerLocation,
mr_createid,
mr_createname,
mc_name as CompanyName
into #tmpReg
from MealsRegister
left join MealsLocation as ll on ll.ml_guid=mr_lunch_location
left join MealsLocation as dl on dl.ml_guid=mr_dinner_location
left join MealsCompany on mc_guid=mr_person_id
where convert(datetime, mr_date)=@exportDate
order by ll.ml_id

select * from #tmpReg where mr_type='01'
select * from #tmpReg where mr_type='02'
select * from #tmpReg where mr_type='03'

-- 訪客
select mv_date,
'葷: '+ convert(nvarchar(10),mv_lunch_meat)+' 奶蛋素: '+ convert(nvarchar(10),mv_lunch_vegetarian)+' 全素: '+ convert(nvarchar(10),mv_lunch_vegan) as lunch,
'葷: '+ convert(nvarchar(10),mv_dinner_meat)+' 奶蛋素: '+ convert(nvarchar(10),mv_dinner_vegetarian)+' 全素: '+ convert(nvarchar(10),mv_dinner_vegan) as dinner,
mv_createid,
mv_createname,
mv_name
from MealsVisitor
left join FormMain on fm_data_guid=mv_guid 
where convert(datetime, mv_date)=@exportDate and fm_result='Y'

drop table #tmpReg ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@today", today);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}
}