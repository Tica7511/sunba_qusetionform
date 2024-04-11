using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// DormitoryTenant_DB 的摘要描述
/// </summary>
public class DormitoryTenant_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string dt_id = string.Empty;
	string dt_guid = string.Empty;
	string dt_roomid = string.Empty;
	string dt_empno = string.Empty;
	string dt_name = string.Empty;
	string dt_createid = string.Empty;
	string dt_createname = string.Empty;
	DateTime dt_createdate;
	string dt_modid = string.Empty;
	string dt_modname = string.Empty;
	DateTime dt_moddate;
	string dt_status = string.Empty;
	#endregion
	#region Public
	public string _dt_id { set { dt_id = value; } }
	public string _dt_guid { set { dt_guid = value; } }
	public string _dt_roomid { set { dt_roomid = value; } }
	public string _dt_empno { set { dt_empno = value; } }
	public string _dt_name { set { dt_name = value; } }
	public string _dt_createid { set { dt_createid = value; } }
	public string _dt_createname { set { dt_createname = value; } }
	public DateTime _dt_createdate { set { dt_createdate = value; } }
	public string _dt_modid { set { dt_modid = value; } }
	public string _dt_modname { set { dt_modname = value; } }
	public DateTime _dt_moddate { set { dt_moddate = value; } }
	public string _dt_status { set { dt_status = value; } }
	#endregion

	public DataSet GetManageList(string pStart, string pEnd, string area)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select * into #tmp
from (
    select *
    ,REVERSE(STUFF(REVERSE((
    SELECT dt_name + '、' FROM DormitoryTenant h1
    WHERE dr_guid=h1.dt_roomid and h1.dt_status='A'
    FOR XML PATH('')
    )),1,1,'')) as dt_name
    ,(
    SELECT dt_empno + '' FROM DormitoryTenant h1
    WHERE dr_guid=h1.dt_roomid and h1.dt_status='A'
    FOR XML PATH('')
    ) as dt_empno
    from DormitoryRoom 
    where dr_status='A' and dr_area=@area
)t
where 1=1 ");

		if (KeyWord != "")
		{
			sb.Append(@"and lower(
                        isnull(dt_empno,'')+isnull(dt_name,'')
                        ) like '%' + lower(@KeyWord) + '%' ");
		}

		sb.Append(@"
--總筆數
select count(*) as total from #tmp
--分頁資料
select * from (
select ROW_NUMBER() over (order by dr_createdate desc,dr_id desc) itemNo,
#tmp.*,
a.C_Item_cn as TypeCn,
b.C_Item_cn as RoomTypeCn
from #tmp
left join CodeTable a on a.C_Group='02' and dr_category=a.C_Item
left join CodeTable b on b.C_Group='03' and dr_roomtype=b.C_Item
)#t where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);
		oCmd.Parameters.AddWithValue("@area", area);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void addDormitoryTenant(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into DormitoryTenant (
dt_guid,
dt_roomid,
dt_empno,
dt_name,
dt_createid,
dt_createname,
dt_modid,
dt_modname,
dt_status
) values (
@dt_guid,
@dt_roomid,
@dt_empno,
@dt_name,
@dt_createid,
@dt_createname,
@dt_modid,
@dt_modname,
@dt_status
) ");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dt_guid", dt_guid);
		oCmd.Parameters.AddWithValue("@dt_roomid", dt_roomid);
		oCmd.Parameters.AddWithValue("@dt_empno", dt_empno);
		oCmd.Parameters.AddWithValue("@dt_name", dt_name);
		oCmd.Parameters.AddWithValue("@dt_createid", dt_createid);
		oCmd.Parameters.AddWithValue("@dt_createname", dt_createname);
		oCmd.Parameters.AddWithValue("@dt_modid", dt_modid);
		oCmd.Parameters.AddWithValue("@dt_modname", dt_modname);
		oCmd.Parameters.AddWithValue("@dt_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@dt_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void DeleteByRoomID(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update DormitoryTenant set 
dt_status='D',
dt_modid=@dt_modid,
dt_modname=@dt_modname,
dt_moddate=@dt_moddate
where dt_roomid=@dt_roomid ");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dt_roomid", dt_roomid);
		oCmd.Parameters.AddWithValue("@dt_modid", dt_modid);
		oCmd.Parameters.AddWithValue("@dt_modname", dt_modname);
		oCmd.Parameters.AddWithValue("@dt_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@dt_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public DataSet GetTenantDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select DormitoryRoom.*,
a.C_Item_cn as TypeCn,
b.C_Item_cn as RoomTypeCn 
from DormitoryRoom 
left join CodeTable a on a.C_Group='02' and dr_category=a.C_Item
left join CodeTable b on b.C_Group='03' and dr_roomtype=b.C_Item
where dr_guid=@dt_roomid ");

		sb.Append(@"select * from DormitoryTenant where dt_roomid=@dt_roomid and dt_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dt_roomid", dt_roomid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}
}