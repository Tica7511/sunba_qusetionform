using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Dormitory_DB 的摘要描述
/// </summary>
public class Dormitory_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string d_id = string.Empty;
	string d_guid = string.Empty;
	string d_type = string.Empty;
	string d_name = string.Empty;
	string d_empno = string.Empty;
	string d_department = string.Empty;
	string d_startday = string.Empty;
	string d_endday = string.Empty;
	string d_reason = string.Empty;
	string d_tel = string.Empty;
	string d_bloodtype = string.Empty;
	string d_emergency_contact = string.Empty;
	string d_emergency_tel = string.Empty;
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
	public string _d_type { set { d_type = value; } }
	public string _d_name { set { d_name = value; } }
	public string _d_empno { set { d_empno = value; } }
	public string _d_department { set { d_department = value; } }
	public string _d_startday { set { d_startday = value; } }
	public string _d_endday { set { d_endday = value; } }
	public string _d_reason { set { d_reason = value; } }
	public string _d_tel { set { d_tel = value; } }
	public string _d_bloodtype { set { d_bloodtype = value; } }
	public string _d_emergency_contact { set { d_emergency_contact = value; } }
	public string _d_emergency_tel { set { d_emergency_tel = value; } }
	public string _d_createid { set { d_createid = value; } }
	public string _d_createname { set { d_createname = value; } }
	public DateTime _d_createdate { set { d_createdate = value; } }
	public string _d_modid { set { d_modid = value; } }
	public string _d_modname { set { d_modname = value; } }
	public DateTime _d_moddate { set { d_moddate = value; } }
	public string _d_status { set { d_status = value; } }
	#endregion

	public DataSet GetRegisterList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select Dormitory.*,C_Item_Cn as TypeCn,fm.fm_result as fm_result 
,(select fms_signresult from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=1  ) as Sign_1
,(select fms_signresult from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=2  ) as Sign_2
,(select fms_signresult from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=3  ) as Sign_3
,(select fms_signresult from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=4  ) as Sign_4
,(select fms_signdate from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=1  ) as SignDate_1
,(select fms_signdate from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=2  ) as SignDate_2
,(select fms_signdate from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=3  ) as SignDate_3
,(select fms_signdate from FormMainSite left join FormMain on fms_parentid=fm_guid where fm_data_guid=d_guid and fms_site=4  ) as SignDate_4
into #tmp 
from Dormitory
left join CodeTable on C_Group='02' and d_type=C_Item
left join FormMain as fm on fm_data_guid=d_guid
where d_status='A' and d_empno=@empno ");

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by d_createdate desc,d_id desc) itemNo,* from #tmp
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

	public void addDormitory(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into Dormitory (
d_guid,
d_type,
d_name,
d_empno,
d_department,
d_startday,
d_endday,
d_reason,
d_tel,
d_bloodtype,
d_emergency_contact,
d_emergency_tel,
d_createid,
d_createname,
d_modid,
d_modname,
d_status
) values (
@d_guid,
@d_type,
@d_name,
@d_empno,
@d_department,
@d_startday,
@d_endday,
@d_reason,
@d_tel,
@d_bloodtype,
@d_emergency_contact,
@d_emergency_tel,
@d_createid,
@d_createname,
@d_modid,
@d_modname,
@d_status
) 

if @d_type='02'
	begin
		declare @newGuid nvarchar(50) = (select lower(replace(newid(),'-','')));
		insert into DormitoryCancel (
		dc_guid,
		dc_category,
		dc_canceldate,
		dc_reason,
		dc_createid,
		dc_createname,
		dc_modid,
		dc_modname,
		dc_status
		) values (
		@newGuid,
		@d_type,
		@d_endday,
		@d_reason,
		@d_createid,
		@d_createname,
		@d_modid,
		@d_modname,
		@d_status
		)
	end
");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@d_guid", d_guid);
		oCmd.Parameters.AddWithValue("@d_type", d_type);
		oCmd.Parameters.AddWithValue("@d_name", d_name);
		oCmd.Parameters.AddWithValue("@d_empno", d_empno);
		oCmd.Parameters.AddWithValue("@d_department", d_department);
		oCmd.Parameters.AddWithValue("@d_startday", d_startday);
		oCmd.Parameters.AddWithValue("@d_endday", d_endday);
		oCmd.Parameters.AddWithValue("@d_reason", d_reason);
		oCmd.Parameters.AddWithValue("@d_tel", d_tel);
		oCmd.Parameters.AddWithValue("@d_bloodtype", d_bloodtype);
		oCmd.Parameters.AddWithValue("@d_emergency_contact", d_emergency_contact);
		oCmd.Parameters.AddWithValue("@d_emergency_tel", d_emergency_tel);
		oCmd.Parameters.AddWithValue("@d_createid", d_createid);
		oCmd.Parameters.AddWithValue("@d_createname", d_createname);
		oCmd.Parameters.AddWithValue("@d_modid", d_modid);
		oCmd.Parameters.AddWithValue("@d_modname", d_modname);
		oCmd.Parameters.AddWithValue("@d_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void DeleteDormitoryRegister()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update Dormitory set
d_status=@d_status,
d_modid=@d_modid,
d_modname=@d_modname,
d_moddate=@d_moddate
where d_guid=@d_guid
";
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@d_guid", d_guid);
		oCmd.Parameters.AddWithValue("@d_modid", d_modid);
		oCmd.Parameters.AddWithValue("@d_modname", d_modname);
		oCmd.Parameters.AddWithValue("@d_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@d_status", "D");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetDormitoryDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from Dormitory where d_id=@d_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@d_id", d_id);
		oda.Fill(ds);
		return ds;
	}
}