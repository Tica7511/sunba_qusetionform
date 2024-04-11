using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OutdoorForm_DB 的摘要描述
/// </summary>
public class OutdoorForm_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	string o_id = string.Empty;
	string o_guid = string.Empty;
	string o_type = string.Empty;
	string o_passenger_number = string.Empty;
	string o_passenger_empno = string.Empty;
	DateTime o_starttime;
	DateTime o_endtime;
	string o_car = string.Empty;
	string o_place = string.Empty;
	string o_reason = string.Empty;
	string o_ps = string.Empty;
	string o_createid = string.Empty;
	string o_createname = string.Empty;
	DateTime o_createdate;
	string o_modid = string.Empty;
	string o_modname = string.Empty;
	DateTime o_moddate;
	string o_status = string.Empty;
	#endregion
	#region Public
	public string _o_id { set { o_id = value; } }
	public string _o_guid { set { o_guid = value; } }
	public string _o_type { set { o_type = value; } }
	public string _o_passenger_number { set { o_passenger_number = value; } }
	public string _o_passenger_empno { set { o_passenger_empno = value; } }
	public DateTime _o_starttime { set { o_starttime = value; } }
	public DateTime _o_endtime { set { o_endtime = value; } }
	public string _o_car { set { o_car = value; } }
	public string _o_place { set { o_place = value; } }
	public string _o_reason { set { o_reason = value; } }
	public string _o_ps { set { o_ps = value; } }
	public string _o_createid { set { o_createid = value; } }
	public string _o_createname { set { o_createname = value; } }
	public DateTime _o_createdate { set { o_createdate = value; } }
	public string _o_modid { set { o_modid = value; } }
	public string _o_modname { set { o_modname = value; } }
	public DateTime _o_moddate { set { o_moddate = value; } }
	public string _o_status { set { o_status = value; } }
	#endregion

	public DataSet GetManageList(string pStart, string pEnd, string sortName, string sortMethod)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
odf.*,
ct.C_Item_cn as TypeCn,
oc.oc_number as CarNum,
fm.fm_result as SignResult,
convert(nvarchar(16),ocr.ocr_outtime,121) as ActualOutTime,
convert(nvarchar(16),ocr.ocr_backtime,121) as ActualBackTime
into #tmp from OutdoorForm as odf
left join CodeTable as ct on C_Group='05' and C_Item=o_type
left join OfficialCar as oc on oc_guid=o_car
left join FormMain as fm on fm_data_guid=o_guid
left join OfficialCarRecord as ocr on ocr_parentid=o_guid
where o_status='A' and o_createid=@empno ");


		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by ");

		switch (sortName)
		{
			case "o_type":
				if (sortMethod == "asc")
					sb.Append(@"o_type,");
				else
					sb.Append(@"o_type desc,");
				break;
		}

		sb.Append(@"o_createdate desc,o_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@empno", LogInfo.empNo);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void addOutdoor()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into OutdoorForm (
o_guid,
o_type,
o_passenger_number,
o_passenger_empno,
o_starttime,
o_endtime,
o_car,
o_place,
o_reason,
o_ps,
o_createid,
o_createname,
o_modid,
o_modname,
o_status
) values (
@o_guid,
@o_type,
@o_passenger_number,
@o_passenger_empno,
@o_starttime,
@o_endtime,
@o_car,
@o_place,
@o_reason,
@o_ps,
@o_createid,
@o_createname,
@o_modid,
@o_modname,
@o_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@o_guid", o_guid);
		oCmd.Parameters.AddWithValue("@o_type", o_type);
		oCmd.Parameters.AddWithValue("@o_passenger_number", o_passenger_number);
		oCmd.Parameters.AddWithValue("@o_passenger_empno", o_passenger_empno);
		oCmd.Parameters.AddWithValue("@o_starttime", o_starttime);
		oCmd.Parameters.AddWithValue("@o_endtime", o_endtime);
		oCmd.Parameters.AddWithValue("@o_car", o_car);
		oCmd.Parameters.AddWithValue("@o_place", o_place);
		oCmd.Parameters.AddWithValue("@o_reason", o_reason);
		oCmd.Parameters.AddWithValue("@o_ps", o_ps);
		oCmd.Parameters.AddWithValue("@o_createid", o_createid);
		oCmd.Parameters.AddWithValue("@o_createname", o_createname);
		oCmd.Parameters.AddWithValue("@o_modid", o_modid);
		oCmd.Parameters.AddWithValue("@o_modname", o_modname);
		oCmd.Parameters.AddWithValue("@o_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}


	public DataTable GetCarList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
declare @sTime datetime = @o_starttime;
declare @eTime datetime = @o_endtime;

select 
o_guid,
o_starttime,
case when (ocr_backtime is null or ocr_backtime='') then o_endtime else ocr_backtime end as o_endtime,
o_car,
o_createname,
o_status
into #tmplist from OutdoorForm
left join OfficialCarRecord on ocr_parentid=o_guid and ocr_status='A'
where  o_status='A' and o_type='02'

select * into #tmp from #tmplist
where
(
(CONVERT (datetime,o_starttime) > @sTime and CONVERT (datetime,o_starttime) < @eTime)
or
(CONVERT (datetime,o_starttime) < @sTime and CONVERT (datetime,o_endtime) > @eTime)
or
(CONVERT (datetime,o_endtime) > @sTime and CONVERT (datetime,o_endtime) < @eTime)
or
(CONVERT (datetime,o_starttime) = @sTime and CONVERT (datetime,o_endtime) = @eTime)
or
(CONVERT (datetime,o_starttime) = @sTime and CONVERT (datetime,o_endtime) > @eTime)
or
(CONVERT (datetime,o_starttime) < @sTime and CONVERT (datetime,o_endtime) = @eTime)
) 

select (
	select oc_guid,oc_number,oc_ps,data_item.* from OfficialCar
	left join #tmp as data_item on oc_guid=o_car
	where oc_status='A'
	order by o_starttime
	for xml auto,root('root') 
) as XmlCol

drop table #tmplist
drop table #tmp ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		
		oCmd.Parameters.AddWithValue("@o_starttime", o_starttime);
		oCmd.Parameters.AddWithValue("@o_endtime", o_endtime);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void DeleteOutdoorForm()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update OutdoorForm set
o_status=@o_status,
o_modid=@o_modid,
o_modname=@o_modname,
o_moddate=@o_moddate
where o_id=@o_id
";
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@o_id", o_id);
		oCmd.Parameters.AddWithValue("@o_modid", o_modid);
		oCmd.Parameters.AddWithValue("@o_modname", o_modname);
		oCmd.Parameters.AddWithValue("@o_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@o_status", "D");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetOutdoorFormDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select odf.*,oc_number as CarNum from OutdoorForm as odf
left join OfficialCar as oc on o_car=oc_guid
where o_id=@o_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@o_id", o_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetListForGuard(string today)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
fm_data_guid,
o_createname,
oc_number as CarNum,
oc_guid,
CONVERT(nvarchar(10),o_starttime,121) as startdate,
CONVERT(nvarchar(10),o_endtime,121) as enddate,
CONVERT(nvarchar(5),o_starttime,108) as starttime,
CONVERT(nvarchar(5),o_endtime,108) as endtime,
o_starttime as StartAll,
o_endtime as EndAll,
CONVERT(nvarchar(5),ocr_outtime,108) as ActualOutTime,
CONVERT(nvarchar(5),ocr_backtime,108) as ActualBackTime
from FormMain
left join OutdoorForm on o_guid=fm_data_guid
left join OfficialCar on o_car=oc_guid
left join OfficialCarRecord on ocr_parentid=fm_data_guid
where fm_status='A' and fm_result='Y' and fm_category='OFC'
and convert(datetime,@today) between convert(datetime,convert(nvarchar(10),o_starttime,121)) and convert(datetime,convert(nvarchar(10),o_endtime,121))  ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@today", today);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetAllListForGuard(string today,string pStart, string pEnd, string sortName, string sortMethod)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
o_type,
C_Item_cn,
o_createname,
oc_number as CarNum,
o_place,
o_reason,
o_starttime,
o_endtime,
CONVERT(nvarchar(10),o_starttime,121) as startdate,
CONVERT(nvarchar(10),o_endtime,121) as enddate,
CONVERT(nvarchar(5),o_starttime,108) as starttime,
CONVERT(nvarchar(5),o_endtime,108) as endtime
into #tmp
from FormMain
left join OutdoorForm on o_guid=fm_data_guid
left join OfficialCar on o_car=oc_guid
left join CodeTable on C_Group='05' and C_Item=o_type
where fm_status='A' and fm_result='Y' and (fm_category='OFC' or fm_category='OFN')
and (CONVERT(nvarchar(10),o_starttime,121)=@today or CONVERT(nvarchar(10),o_endtime,121)=@today) ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by ");

		switch (sortName)
		{
			case "o_type":
				if (sortMethod == "asc")
					sb.Append(@"o_type");
				else
					sb.Append(@"o_type desc");
				break;
			case "outTime":
				if (sortMethod == "asc")
					sb.Append(@"o_starttime,o_endtime");
				else
					sb.Append(@"o_starttime desc,o_endtime desc");
				break;
			default:
					sb.Append(@"convert(datetime,startdate),convert(datetime,starttime)");
				break;
		}

		sb.Append(@") itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@today", today);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from OutdoorForm where o_guid=@o_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@o_guid", o_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetManagerList(string carNo, string startDate, string endDate, string actualOut, string actualBack, string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
odf.*,
ct.C_Item_cn as TypeCn,
oc.oc_number as CarNum,
fm.fm_result as SignResult,
convert(nvarchar(16),ocr.ocr_outtime,121) as ActualOutTime,
convert(nvarchar(16),ocr.ocr_backtime,121) as ActualBackTime
into #tmp from OutdoorForm as odf
left join CodeTable as ct on C_Group='05' and C_Item=o_type
left join OfficialCar as oc on oc_guid=o_car
left join FormMain as fm on fm_data_guid=o_guid
left join OfficialCarRecord as ocr on ocr_parentid=o_guid
where o_status='A' ");

		if (KeyWord != "")
			sb.Append(@"and lower(o_createname) like '%' + lower(@KeyWord) + '%' ");
		if(o_type!="")
			sb.Append(@"and o_type=@o_type ");
		if(carNo != "")
			sb.Append(@"and oc_number=@oc_number ");
		if (startDate != "")
			sb.Append(@"and convert(datetime,convert(nvarchar(10),o_starttime,121)) >= convert(datetime,@o_starttime) ");
		if (endDate != "")
			sb.Append(@"and convert(datetime,convert(nvarchar(10),o_endtime,121)) <= convert(datetime,@o_endtime) ");
		if (actualOut != "")
			sb.Append(@"and convert(datetime,convert(nvarchar(10),ocr_outtime,121)) >= convert(datetime,@ocr_outtime) ");
		if (actualBack != "")
			sb.Append(@"and convert(datetime,convert(nvarchar(10),ocr_backtime,121)) <= convert(datetime,@ocr_backtime) ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by o_createdate desc,o_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@o_type", o_type);
		oCmd.Parameters.AddWithValue("@oc_number", carNo);
		oCmd.Parameters.AddWithValue("@o_starttime", startDate);
		oCmd.Parameters.AddWithValue("@o_endtime", endDate);
		oCmd.Parameters.AddWithValue("@ocr_outtime", actualOut);
		oCmd.Parameters.AddWithValue("@ocr_backtime", actualBack);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetOutdoorCalendarList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select
o_id,
o_guid,
o_reason,
o_createid,
o_createname,
oc_id,
oc_number,
o_starttime,
o_endtime
from OutdoorForm 
left join OfficialCar on oc_guid=o_car
where o_status='A' and o_type='02' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		//oCmd.Parameters.AddWithValue("@nowDate", nowDate);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}