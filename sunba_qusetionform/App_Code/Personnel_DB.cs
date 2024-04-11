using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Personnel_DB 的摘要描述
/// </summary>
public class Personnel_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	public DataTable GetEmployeeSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select EMPLOYEE_ID, 
  EMPLOYEE_NO as empno, 
  EMPLOYEE_CNAME as empname, 
  DEPARTMENT_CODE as deptid,
  DEPARTMENT_CNAME as dept
  from vwZZ_EMPLOYEE
  where EMPLOYEE_WORK_STATUS=1
  order by  DEPARTMENT_ID ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetDeptSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
DEPARTMENT_CODE as deptid,
DEPARTMENT_CNAME as dept
from vwZZ_DEPARTMENT
where DEPARTMENT_STATUS=0
order by DEPARTMENT_ID ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		//oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}


	public DataTable GetSysSettingSelectList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select EMPLOYEE_ID, 
  EMPLOYEE_NO as empno, 
  EMPLOYEE_CNAME as empname, 
  DEPARTMENT_CODE as deptid,
  DEPARTMENT_CNAME as dept
  from vwZZ_EMPLOYEE
  where EMPLOYEE_WORK_STATUS=1 and DEPARTMENT_CNAME like '%行政管理部%'
  order by DEPARTMENT_ID ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetOutdoorNormalSigners()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
create table #tmpflow(
--flowsite int identity(1,1),
empno nvarchar(50),
empname nvarchar(50),
jobtitle nvarchar(50)
)

-- 登入者工號
declare @CreateEmpno nvarchar(50)=@empno

-- 運轉課值班主管
SELECT 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
into #DutyManager
FROM vwZZ_DEPARTMENT as dep
left join vwZZ_EMPLOYEE as emp on DEPARTMENT_LEADER_ID=EMPLOYEE_ID
where dep.DEPARTMENT_CODE in ('AA012-05','AA012-06','AA012-07','AA012-08','AA012-09','AA012-10')

-- 電廠所有副廠長 (104 資料結構設定問題)
select 
EMPLOYEE_NO as empno,
EMPLOYEE_CNAME as empname,
JOB_CNAME as jobtitle
into #DeputyDirector
from vwZZ_EMPLOYEE
where EMPLOYEE_WORK_STATUS=1 and JOB_CNAME like '%副廠長%'

-- 排除的部門
declare @WithoutDept nvarchar(50)='AA001,AA003,AA004'
-- 逐層向上直屬主管
declare @dep1 nvarchar(50)

select @dep1=DEPARTMENT_ID
from vwZZ_EMPLOYEE
where EMPLOYEE_NO=@CreateEmpno

-- 1
insert into #tmpflow
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
from vwZZ_DEPARTMENT as a
left join vwZZ_EMPLOYEE on DEPARTMENT_LEADER_ID=EMPLOYEE_ID
where a.DEPARTMENT_ID=@dep1 and EMPLOYEE_NO<>@CreateEmpno and EMPLOYEE_WORK_STATUS=1

declare @dep2 nvarchar(50)

select @dep2=PART_DEPARTMENT_ID
from vwZZ_DEPARTMENT
where DEPARTMENT_ID=@dep1

-- 2
insert into #tmpflow
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
from vwZZ_DEPARTMENT as a
left join vwZZ_EMPLOYEE on DEPARTMENT_LEADER_ID=EMPLOYEE_ID
where a.DEPARTMENT_ID=@dep2  and EMPLOYEE_WORK_STATUS=1
and charindex(a.DEPARTMENT_CODE,@WithoutDept)=0  -- charindex = where not in 用法

declare @dep3 nvarchar(50)

select 
@dep3=PART_DEPARTMENT_ID
from vwZZ_DEPARTMENT
where DEPARTMENT_ID=@dep2

-- 3
insert into #tmpflow
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
from vwZZ_DEPARTMENT as a
left join vwZZ_EMPLOYEE on DEPARTMENT_LEADER_ID=EMPLOYEE_ID and EMPLOYEE_WORK_STATUS=1
where a.DEPARTMENT_ID=@dep3 and EMPLOYEE_WORK_STATUS=1
and charindex(a.DEPARTMENT_CODE,@WithoutDept)=0

declare @dep4 nvarchar(50)

select 
@dep4=PART_DEPARTMENT_ID
from vwZZ_DEPARTMENT
where DEPARTMENT_ID=@dep3

-- 4
insert into #tmpflow
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
from vwZZ_DEPARTMENT as a
left join vwZZ_EMPLOYEE on DEPARTMENT_LEADER_ID=EMPLOYEE_ID and EMPLOYEE_WORK_STATUS=1
where a.DEPARTMENT_ID=@dep4 and EMPLOYEE_WORK_STATUS=1
and charindex(a.DEPARTMENT_CODE,@WithoutDept)=0

declare @dep5 nvarchar(50)

select 
@dep5=PART_DEPARTMENT_ID
from vwZZ_DEPARTMENT
where DEPARTMENT_ID=@dep4

-- 5
insert into #tmpflow
select 
EMPLOYEE_NO,
EMPLOYEE_CNAME,
JOB_CNAME
from vwZZ_DEPARTMENT as a
left join vwZZ_EMPLOYEE on DEPARTMENT_LEADER_ID=EMPLOYEE_ID and EMPLOYEE_WORK_STATUS=1
where a.DEPARTMENT_ID=@dep5 and EMPLOYEE_WORK_STATUS=1
and charindex(a.DEPARTMENT_CODE,@WithoutDept)=0

-- 確認登入者是否為運轉課值班主管
declare @dutycount int
select @dutycount=count(*) from #DutyManager
where EMPLOYEE_NO=@CreateEmpno

-- 確認登入者是否為電廠人員
declare @deputycount int
select @deputycount=count(*) from vwZZ_EMPLOYEE 
where EMPLOYEE_WORK_STATUS=1 and EMPLOYEE_NO=@CreateEmpno
and DEPARTMENT_CODE in ('AA012','AA013','AA015','AA012-01','AA012-02','AA012-03','AA012-04','AA012-05','AA012-06','AA012-07','AA012-08','AA012-09',
'AA013-01','AA013-02','AA013-03','AA013-04','AA015-01','AA015-02','AA013-05','AA012-10','AA012-13','AA012-012','AA012-02-1','A0013-011','A0013-06','AA013-06')

if @dutycount>0
	begin
		select * from #tmpflow
		union
		select * from #DutyManager
		union
		select * from #DeputyDirector
	end
else
	begin
		if @deputycount>0
			begin
				select empno,empname,jobtitle from #tmpflow
				union
				select * from #DeputyDirector
			end
		else
			begin
				select empno,empname,jobtitle from #tmpflow
				group by empno,empname,jobtitle
			end
	end

drop table #DeputyDirector
drop table #DutyManager
drop table #tmpflow ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@empno", LogInfo.empNo);
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable dt = new DataTable();
		oda.Fill(dt);
		return dt;
	}


	public DataTable GetEmployeeLevel(string empno)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select
EMPLOYEE_NO as empno,
EMPLOYEE_CNAME as empname,
dep.DEPARTMENT_CNAME as dept,
dep.DEPARTMENT_CODE as deptid,
case 
when dep.DEPARTMENT_CODE in ('AA001','AA003','AA004','AA005','AA006','AA007','AA008','AA007-01','AA007-02') then 2 -- 經理級以上
when  dep.DEPARTMENT_CODE in ('AA005-05','AA005-02','AA005-03','AA006-01','AA006-02','AA008-01','AA012','AA012-01','AA012-012','AA012-13','AA013','AA013-01','A0013-011','AA015','AA015-01') then 1 -- 副課長、課長、主任
else 0 end as JobLv
into #tmp
from vwZZ_DEPARTMENT as dep
left join vwZZ_EMPLOYEE as emp on dep.DEPARTMENT_LEADER_ID= emp.EMPLOYEE_ID
where dep.DEPARTMENT_STATUS=0 and emp.EMPLOYEE_WORK_STATUS=1

select top 1 * from #tmp
where empno=@empno
order by convert(int,JobLv) desc
drop table #tmp ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@empno", empno);
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable dt = new DataTable();
		oda.Fill(dt);
		return dt;
	}

	public DataTable GetEmployeeEmil(string empno)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["Common.104ContStr"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 
EMPLOYEE_CNAME,
EMPLOYEE_EMAIL_1 
from vwZZ_EMPLOYEE
where EMPLOYEE_NO=@empno ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@empno", empno);
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable dt = new DataTable();
		oda.Fill(dt);
		return dt;
	}
}