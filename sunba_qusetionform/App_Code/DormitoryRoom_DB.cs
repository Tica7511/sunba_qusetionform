using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// DormitoryRoom_DB 的摘要描述
/// </summary>
public class DormitoryRoom_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string dr_id = string.Empty;
	string dr_guid = string.Empty;
	string dr_no = string.Empty;
	string dr_area = string.Empty;
	string dr_ext = string.Empty;
	string dr_roomtype = string.Empty;
	string dr_category = string.Empty;
	string dr_ps = string.Empty;
	string dr_createid = string.Empty;
	string dr_createname = string.Empty;
	DateTime dr_createdate;
	string dr_modid = string.Empty;
	string dr_modname = string.Empty;
	DateTime dr_moddate;
	string dr_status = string.Empty;

	#endregion
	#region Public
	public string _dr_id { set { dr_id = value; } }
	public string _dr_guid { set { dr_guid = value; } }
	public string _dr_no { set { dr_no = value; } }
	public string _dr_area { set { dr_area = value; } }
	public string _dr_ext { set { dr_ext = value; } }
	public string _dr_roomtype { set { dr_roomtype = value; } }
	public string _dr_category { set { dr_category = value; } }
	public string _dr_ps { set { dr_ps = value; } }
	public string _dr_createid { set { dr_createid = value; } }
	public string _dr_createname { set { dr_createname = value; } }
	public DateTime _dr_createdate { set { dr_createdate = value; } }
	public string _dr_modid { set { dr_modid = value; } }
	public string _dr_modname { set { dr_modname = value; } }
	public DateTime _dr_moddate { set { dr_moddate = value; } }
	public string _dr_status { set { dr_status = value; } }
	#endregion

	public DataSet GetRoomManageList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * into #tmp from DormitoryRoom where dr_status='A' and dr_area=@dr_area ");
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
		oCmd.Parameters.AddWithValue("@dr_area", dr_area);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();
		oda.Fill(ds);
		return ds;
	}

	public void addDormitoryRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into DormitoryRoom (
dr_guid,
dr_no,
dr_area,
dr_ext,
dr_roomtype,
dr_category,
dr_ps,
dr_createid,
dr_createname,
dr_modid,
dr_modname,
dr_status
) values (
@dr_guid,
@dr_no,
@dr_area,
@dr_ext,
@dr_roomtype,
@dr_category,
@dr_ps,
@dr_createid,
@dr_createname,
@dr_modid,
@dr_modname,
@dr_status
) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dr_guid", dr_guid);
		oCmd.Parameters.AddWithValue("@dr_no", dr_no);
		oCmd.Parameters.AddWithValue("@dr_area", dr_area);
		oCmd.Parameters.AddWithValue("@dr_ext", dr_ext);
		oCmd.Parameters.AddWithValue("@dr_roomtype", dr_roomtype);
		oCmd.Parameters.AddWithValue("@dr_category", dr_category);
		oCmd.Parameters.AddWithValue("@dr_ps", dr_ps);
		oCmd.Parameters.AddWithValue("@dr_createid", dr_createid);
		oCmd.Parameters.AddWithValue("@dr_createname", dr_createname);
		oCmd.Parameters.AddWithValue("@dr_modid", dr_modid);
		oCmd.Parameters.AddWithValue("@dr_modname", dr_modname);
		oCmd.Parameters.AddWithValue("@dr_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void UpdateDormitoryRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update DormitoryRoom set
dr_no=@dr_no,
dr_area=@dr_area,
dr_ext=@dr_ext,
dr_roomtype=@dr_roomtype,
dr_category=@dr_category,
dr_ps=@dr_ps,
dr_modid=@dr_modid,
dr_modname=@dr_modname,
dr_moddate=@dr_moddate
where dr_id=@dr_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dr_id", dr_id);
		oCmd.Parameters.AddWithValue("@dr_no", dr_no);
		oCmd.Parameters.AddWithValue("@dr_area", dr_area);
		oCmd.Parameters.AddWithValue("@dr_ext", dr_ext);
		oCmd.Parameters.AddWithValue("@dr_roomtype", dr_roomtype);
		oCmd.Parameters.AddWithValue("@dr_category", dr_category);
		oCmd.Parameters.AddWithValue("@dr_ps", dr_ps);
		oCmd.Parameters.AddWithValue("@dr_modid", dr_modid);
		oCmd.Parameters.AddWithValue("@dr_modname", dr_modname);
		oCmd.Parameters.AddWithValue("@dr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteRoom()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update DormitoryRoom set
dr_modid=@dr_modid,
dr_modname=@dr_modname,
dr_moddate=@dr_moddate,
dr_status='D'
where dr_id=@dr_id ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@dr_id", dr_id);
		oCmd.Parameters.AddWithValue("@dr_modid", dr_modid);
		oCmd.Parameters.AddWithValue("@dr_modname", dr_modname);
		oCmd.Parameters.AddWithValue("@dr_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetRoomDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from DormitoryRoom where dr_id=@dr_id ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@dr_id", dr_id);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}
}