using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// 簽核流程
/// </summary>
public class Flow_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region Private
	// FormMain
	string fm_id = string.Empty;
	string fm_guid = string.Empty;
	string fm_category = string.Empty;
	string fm_data_guid = string.Empty;
	string fm_createid = string.Empty;
	string fm_createname = string.Empty;
	DateTime fm_createdate;
	string fm_result = string.Empty;
	string fm_status = string.Empty;

	// FormMainSite
	string fms_id = string.Empty;
	string fms_guid = string.Empty;
	string fms_parentid = string.Empty;
	int fms_site;
	string fms_islastsite = string.Empty;
	string fms_signperson = string.Empty;
	string fms_actual_signer = string.Empty;
	DateTime fms_signdate;
	string fms_signresult = string.Empty;
	string fms_signredesc = string.Empty;
	string fms_status = string.Empty;

	// FormSet
	string fs_id = string.Empty;
	string fs_guid = string.Empty;
	string fs_name_cn = string.Empty;
	string fs_name_en = string.Empty;
	string fs_code = string.Empty;

	// FormSiteSet
	string fss_id = string.Empty;
	string fss_guid = string.Empty;
	string fss_main_guid = string.Empty;
	string fss_main_code = string.Empty;
	int fss_site;
	string fss_sitename = string.Empty;
	string fss_signperson = string.Empty;
	#endregion
	#region Public
	// FormMain
	public string _fm_id { set { fm_id = value; } }
	public string _fm_guid { set { fm_guid = value; } }
	public string _fm_category { set { fm_category = value; } }
	public string _fm_data_guid { set { fm_data_guid = value; } }
	public string _fm_createid { set { fm_createid = value; } }
	public string _fm_createname { set { fm_createname = value; } }
	public DateTime _fm_createdate { set { fm_createdate = value; } }
	public string _fm_result { set { fm_result = value; } }
	public string _fm_status { set { fm_status = value; } }

	// FormMainSite
	public string _fms_id { set { fms_id = value; } }
	public string _fms_guid { set { fms_guid = value; } }
	public string _fms_parentid { set { fms_parentid = value; } }
	public int _fms_site { set { fms_site = value; } }
	public string _fms_islastsite { set { fms_islastsite = value; } }
	public string _fms_signperson { set { fms_signperson = value; } }
	public string _fms_actual_signer { set { fms_actual_signer = value; } }
	public DateTime _fms_signdate { set { fms_signdate = value; } }
	public string _fms_signresult { set { fms_signresult = value; } }
	public string _fms_signredesc { set { fms_signredesc = value; } }
	public string _fms_status { set { fms_status = value; } }

	// FormSet
	public string _fs_id { set { fs_id = value; } }
	public string _fs_guid { set { fs_guid = value; } }
	public string _fs_name_cn { set { fs_name_cn = value; } }
	public string _fs_name_en { set { fs_name_en = value; } }
	public string _fs_code { set { fs_code = value; } }

	// FormSiteSet
	public string _fss_id { set { fss_id = value; } }
	public string _fss_guid { set { fss_guid = value; } }
	public string _fss_main_guid { set { fss_main_guid = value; } }
	public string _fss_main_code { set { fss_main_code = value; } }
	public int _fss_site { set { fss_site = value; } }
	public string _fss_sitename { set { fss_sitename = value; } }
	public string _fss_signperson { set { fss_signperson = value; } }
	#endregion

	/// <summary>
	/// 開單 / 送單
	/// </summary>
	public void SendForm()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
		insert into FormMain (
		fm_guid,
		fm_category,
		fm_data_guid,
		fm_createid,
		fm_createname,
		fm_status
		) values (
		@fm_guid,
		@fm_category,
		@fm_data_guid,
		@fm_createid,
		@fm_createname,
		@fm_status
		) 

		insert into FormMainSite (
		fms_guid,
		fms_parentid,
		fms_site,
		fms_signperson,
		fms_status
		) values (
		@fms_guid,
		@fms_parentid,
		@fms_site,
		@fms_signperson,
		@fms_status
		) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fm_guid", fm_guid);
		oCmd.Parameters.AddWithValue("@fm_category", fm_category);
		oCmd.Parameters.AddWithValue("@fm_data_guid", fm_data_guid);
		oCmd.Parameters.AddWithValue("@fm_createid", fm_createid);
		oCmd.Parameters.AddWithValue("@fm_createname", fm_createname);
		oCmd.Parameters.AddWithValue("@fm_status", "A");

		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);
		oCmd.Parameters.AddWithValue("@fms_parentid", fm_guid);
		oCmd.Parameters.AddWithValue("@fms_site", fms_site);
		oCmd.Parameters.AddWithValue("@fms_signperson", fms_signperson);
		oCmd.Parameters.AddWithValue("@fms_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	/// <summary>
	/// 抽單
	/// </summary>
	public void TerminateTask()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
		declare @tmpguid nvarchar(50);
		select @tmpguid=fm_guid from FormMain
		where fm_data_guid=@fm_data_guid

		update FormMain set
		fm_status='D'
		where fm_data_guid=@fm_data_guid

		update FormMainSite set
		fms_status='D' 
		where fms_parentid=@tmpguid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fm_guid", fm_guid);
		oCmd.Parameters.AddWithValue("@fm_category", fm_category);
		oCmd.Parameters.AddWithValue("@fm_data_guid", fm_data_guid);
		oCmd.Parameters.AddWithValue("@fm_createid", fm_createid);
		oCmd.Parameters.AddWithValue("@fm_createname", fm_createname);
		oCmd.Parameters.AddWithValue("@fm_status", "A");

		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);
		oCmd.Parameters.AddWithValue("@fms_parentid", fm_guid);
		oCmd.Parameters.AddWithValue("@fms_site", fms_site);
		oCmd.Parameters.AddWithValue("@fms_signperson", fms_signperson);
		oCmd.Parameters.AddWithValue("@fms_signdate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@fms_signredesc", fms_signredesc);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	/// <summary>
	/// 同意至下一關
	/// </summary>
	public void SignNext()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
declare @mainGuid nvarchar(50)
declare @formCode nvarchar(10)
declare @flowSite int

select @mainGuid=fms_parentid,@formCode=fm_category,@flowSite=fms_site from FormMainSite
left join FormMain on fms_parentid=fm_guid
where fms_guid=@fms_guid

set @flowSite=@flowSite+1

declare @nextSigner nvarchar(50)
select  @nextSigner=isnull(fss_signperson,'') from FormSiteSet
where fss_site=@flowSite and fss_main_code=@formCode

declare @nextCount int
select @nextCount=count(*) from FormSiteSet
where fss_site=@flowSite and fss_main_code=@formCode

if @nextCount>0
	begin
		declare @newGuid nvarchar(50) = (select lower(replace( newid(),'-','')));
		insert into FormMainSite (
		fms_guid,
		fms_parentid,
		fms_site,
		fms_signperson,
		fms_status)
		values (
		@newGuid,
		@mainGuid,
		@flowSite,
		@nextSigner,
		'A')
	end
else
	begin
		update FormMain set
		fm_result='Y'
		where fm_guid=@mainGuid
	end

update FormMainSite set
fms_signdate=getdate(),
fms_actual_signer=@fms_actual_signer,
fms_signresult='Y'
where fms_guid=@fms_guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fms_actual_signer", fms_actual_signer);
		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	/// <summary>
	/// 否決
	/// </summary>
	public void Disagree()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
update FormMain set
fm_result='N'
where fm_guid=(select fms_parentid from FormMainSite where fms_guid=@fms_guid)

update FormMainSite set
fms_signdate=getdate(),
fms_signperson='',
fms_signresult='N'
where fms_guid=@fms_guid  ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	/// <summary>
	/// 副課長級以上(無需走簽核)
	/// 開單 / 送單
	/// </summary>
	public void SendFormForManager()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
		insert into FormMain (
		fm_guid,
		fm_category,
		fm_data_guid,
		fm_createid,
		fm_createname,
		fm_result,
		fm_status
		) values (
		@fm_guid,
		@fm_category,
		@fm_data_guid,
		@fm_createid,
		@fm_createname,
		@fm_result,
		@fm_status
		) 
		
		insert into FormMainSite
		select (select lower(replace( newid(),'-',''))),@fm_guid,fss_site,@fm_createid,@fm_createid,getdate(),'Y',null,'A'
		from FormSiteSet
		where fss_main_code=@fm_category ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@fm_guid", fm_guid);
		oCmd.Parameters.AddWithValue("@fm_category", fm_category);
		oCmd.Parameters.AddWithValue("@fm_data_guid", fm_data_guid);
		oCmd.Parameters.AddWithValue("@fm_createid", fm_createid);
		oCmd.Parameters.AddWithValue("@fm_createname", fm_createname);
		oCmd.Parameters.AddWithValue("@fm_result", fm_result);
		oCmd.Parameters.AddWithValue("@fm_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetSigner()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select fss_signperson as NextSigner from FormSiteSet
where fss_main_code=@fss_main_code and fss_site=@fss_site ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@fss_main_code", fss_main_code);
		oCmd.Parameters.AddWithValue("@fss_site", fss_site);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}


	public DataTable GetUnreviewed()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
declare @empno nvarchar(50)  = @loginEmpno
create table #tmp (
	m_Unreview int,
	of_Unreview int,
	d_Unreview int,
	mfb_Unreview int
)
-- 撈出所有系統設定權限名單(不適用宿舍申請與公務車出廠證明單)
select c_type,value 
into #tmpCompList
from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')

declare @cmpCount int

-- <訪客用餐>
-- 確認系統權限
select @cmpCount=count(*) from #tmpCompList
where [value]=@empno and (c_type='01' or  c_type='sa')

declare @mvCount int =0
if @cmpCount>0
	begin
		select @mvCount=count(*) from FormMain 
		where fm_status='A' and fm_category='MV' and isnull(fm_result,'')='' 
	end

-- <用餐取消>
-- 確認系統權限
select @cmpCount=count(*) from #tmpCompList
where [value]=@empno and (c_type='02' or  c_type='sa')

declare @CancelCount int =0
if @cmpCount>0
	begin
		select @CancelCount=count(*) from FormMain
		where fm_status='A' and fm_category='MC' and isnull(fm_result,'')='' 
	end
-- 用餐登記待簽核數
declare @MealsCount int 
set @MealsCount = @mvCount + @CancelCount

-- <外出單>
-- 確認系統權限
select @cmpCount=count(*) from #tmpCompList
where [value]=@empno and (c_type='03' or  c_type='sa')

declare @ofCount int =0
if @cmpCount>0
	begin
		select @ofCount=count(*) from FormMain 
		left join FormMainSite on fms_parentid=fm_guid and isnull(fms_signresult,'')='' and fm_status='A'
		where fm_status='A' and isnull(fm_result,'')='' and (fm_category='OFC' or fm_category='OFN') and fms_status='A'
	end
else
	begin
		select @ofCount=count(*) from FormMainSite
		left join FormMain on fm_guid=fms_parentid and isnull(fms_signresult,'')='' and fm_status='A'
		where fm_category='OFN' and fms_signperson=@empno  and fms_status='A'
	end

-- <宿舍申請>
declare @sa int
select @sa=count(value) from Competence
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='sa' and [value]=@empno

select 
c_type,
value as c_empno,
case c_type
when '04' then '1'
when '05' then '2'
end as FlowSite
into #tmp_SysSigner
from Competence
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='04' or c_type='05'

declare @d_ApplyCount int=0
if @sa>0
	begin
		select @d_ApplyCount=count(*) from FormMainSite
		left join FormMain on  fms_parentid=fm_guid and fm_status='A'
		where (fm_category='DL' or fm_category='DS') and isnull(fms_signresult,'')='' and fms_status='A'
	end
else
	begin
		select 
		case when fms_signperson='' then c_empno else fms_signperson end as Signer
		into #DormitoryList
		from FormMainSite
		left join FormMain on  fms_parentid=fm_guid and fm_status='A'
		left join #tmp_SysSigner on fms_site=FlowSite
		where (fm_category='DL' or fm_category='DS') and isnull(fms_signresult,'')='' and fms_status='A'

		select @d_ApplyCount=count(*) from #DormitoryList
		where Signer=@empno

		drop table #DormitoryList
	end

-- <退宿申請>
-- 確認系統權限
select * into #tmpCancelComp from (
select value as Signer from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='04' or c_type='05' or c_type='sa'
union
select fss_signperson from FormSiteSet
where (fss_main_code='DL' or  fss_main_code='DS') and isnull(fss_signperson,'')<>''
)t

select @cmpCount=count(*) from #tmpCancelComp
where Signer=@empno

declare @d_CancelCount int
if @cmpCount>0
	begin
		select @d_CancelCount=count(*) from DormitoryCancel where dc_status='A' and convert(datetime,dc_canceldate)<=getdate()
	end
	
-- 宿舍申請待簽核數
declare @dormitoryCount int
set @dormitoryCount = @d_ApplyCount + @d_CancelCount

-- <會議室使用狀況>
-- 確認系統權限
select @cmpCount=count(*) from #tmpCompList
where [value]=@empno and (c_type='06' or  c_type='sa')

declare @mfbCount int =0
if @cmpCount>0
	begin
		select @mfbCount=Count(*) from MeetingFeedBack where mfb_status='A'
	end

insert into #tmp (m_Unreview,of_Unreview,d_Unreview,mfb_Unreview) values (@MealsCount,@ofCount,@dormitoryCount,@mfbCount)

select * from #tmp

drop table #tmp
drop table #tmpCompList
drop table #tmpCancelComp
drop table #tmp_SysSigner ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@loginEmpno", LogInfo.empNo);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	// 待審核表單 - 公文與簽辦
	public DataSet GetReviewToBeSignList(string account)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["UOF_ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"SELECT Task.DOC_NBR,TaskNode.START_TIME,TaskNode.TASK_ID,TaskNode.SITE_ID,Form.FORM_ID,Form.FORM_NAME,Task.FORM_VERSION_ID FROM TB_WKF_TASK_NODE AS TaskNode
                    LEFT JOIN TB_WKF_TASK AS Task ON Task.TASK_ID=TaskNode.TASK_ID
                    LEFT JOIN TB_WKF_FORM_VERSION AS FormVersion ON FormVersion.FORM_VERSION_ID=Task.FORM_VERSION_ID
                    LEFT JOIN TB_WKF_FORM AS Form ON Form.FORM_ID=FormVersion.FORM_ID
                    LEFT JOIN TB_EB_USER AS U ON U.ACCOUNT=@ACCOUNT
                    WHERE TaskNode.ORIGINAL_SIGNER=U.USER_GUID AND (FINISH_TIME IS NULL OR FINISH_TIME='')
                    ORDER BY START_TIME DESC");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@ACCOUNT", account);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetReviewMealsVisitorList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * into #tmp from FormMain
left join MealsVisitor on mv_guid=fm_data_guid
left join FormMainSite on isnull(fms_signresult,'')='' and fms_parentid=fm_guid
where fm_category='MV' and fm_status='A' ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by fm_result,fm_createdate desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetReviewMealsCancelList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * into #tmp from FormMain
left join MealsCancel on mc_guid=fm_data_guid
left join FormMainSite on isnull(fms_signresult,'')='' and fms_parentid=fm_guid
where fm_category='MC' and fm_status='A' ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by fm_result,fm_createdate desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetReviewDormitoryApplyList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select 
c_type,
c_empno,
case c_type
when '04' then '1'
when '05' then '2'
end as FlowSite
into #tmp_SysSigner
from Competence
where c_type='04' or c_type='05'

select 
fm.fm_category,
fm.fm_createdate,
fm.fm_createname,
fm.fm_result,
d.d_id,
d.d_department,
ct.C_Item_cn as TypeCn,
fms.fms_guid,
fms.fms_site,
fms.fms_signperson,
c_empno as SysSigner,
(select fms_signresult from FormMainSite where fms_parentid=fm.fm_guid and fms_site=1  ) as Sign_1,
(select fms_signresult from FormMainSite where fms_parentid=fm.fm_guid and fms_site=2  ) as Sign_2,
(select fms_signresult from FormMainSite where fms_parentid=fm.fm_guid and fms_site=3  ) as Sign_3,
(select fms_signresult from FormMainSite where fms_parentid=fm.fm_guid and fms_site=4  ) as Sign_4
into #tmp 
from FormMain as fm
left join FormMainSite as fms on isnull(fms.fms_signresult,'')='' and fms.fms_parentid=fm.fm_guid
left join Dormitory as d on d.d_guid=fm.fm_data_guid
left join CodeTable as ct on ct.C_Group='02' and ct.C_Item=d.d_type
left join #tmp_SysSigner on fms_site=FlowSite
where fm.fm_status='A' and  (fm.fm_category='DL' or  fm.fm_category='DS') ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by fm_result,fm_createdate desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetReviewDormitoryCancelList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select dt_name,dt_empno,dr_no,dr_ext,dr_category into #tmpRoom from DormitoryTenant
left join DormitoryRoom on dr_guid=dt_roomid
where dt_status='A'

select dc.*,ct.C_Item_cn as TypeCn,#tmpRoom.*
into #tmp from DormitoryCancel as dc
left join CodeTable as ct on ct.C_Group='02' and ct.C_Item=dc.dc_category
left join #tmpRoom on dt_empno=dc.dc_createid
where dc.dc_status='A' and convert(datetime,dc.dc_canceldate)<=getdate() ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by dc_canceldate,dc_id desc) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		sb.Append(@"
-- 撈出系統權限
select value from Competence 
CROSS APPLY STRING_SPLIT(c_empno,',')
where c_type='04' or c_type='05' or c_type='sa'
union
select fss_signperson from FormSiteSet
where (fss_main_code='DL' or  fss_main_code='DS') and isnull(fss_signperson,'')<>'' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataSet GetReviewOutdoorList(string pStart, string pEnd,string sortName, string sortMethod)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		
		sb.Append(@"
select 
o_id,
o_type,
o_guid,
C_Item_cn as TypeCn,
fm_guid,
fm_category,
fm_createdate,
fm_createid,
fm_createname,
fm_result,
oc_number,
o_place,
o_starttime,
o_endtime,
o_reason,
fms_guid,
fms_signperson
into #tmp 
from FormMain 
left join FormMainSite on  fms_parentid=fm_guid
left join OutdoorForm on o_guid=fm_data_guid
left join CodeTable  on C_Group='05' and C_Item=o_type
left join OfficialCar on oc_guid=o_car
where fm_status='A' and (fm_category='OFN' or  fm_category='OFC') ");

		if (fms_signperson != "")
			sb.Append(@"and fms_signperson=@fms_signperson");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by ");

		// order by 不允許使用參數(Parameters)
		switch (sortName)
		{
			case "o_type":
				if (sortMethod == "asc")
					sb.Append(@"o_type,");
				else
					sb.Append(@"o_type desc,");
				break;
			case "outTime":
				if (sortMethod == "asc")
					sb.Append(@"o_starttime,o_endtime,");
				else
					sb.Append(@"o_starttime desc,o_endtime desc,");
				break;
		}

		sb.Append(@"fm_result) itemNo,* from #tmp
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@fms_signperson", fms_signperson);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetFlowResult()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select fms_signresult from FormMainSite where fms_parentid=@fms_parentid and fms_site=@fms_site and fms_signperson=@fms_signperson ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@fms_site", fms_site);
		oCmd.Parameters.AddWithValue("@fms_parentid", fms_parentid);
		oCmd.Parameters.AddWithValue("@fms_signperson", fms_signperson);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetNextSigner_ForMail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"declare @parentid nvarchar(50)
select @parentid=fms_parentid from FormMainSite
where fms_guid=@fms_guid

select * from FormMainSite
left join FormMain on fm_guid=fms_parentid
where fms_parentid=@parentid and isnull(fms_signresult,'')='' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@fms_guid", fms_guid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

}