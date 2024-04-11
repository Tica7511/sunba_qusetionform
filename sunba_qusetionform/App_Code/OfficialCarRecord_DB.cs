using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OfficialCarRecord_DB 的摘要描述
/// </summary>
public class OfficialCarRecord_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string ocr_id = string.Empty;
	string ocr_guid = string.Empty;
	string ocr_parentid = string.Empty;
	string ocr_car = string.Empty;
	DateTime ocr_outtime;
	DateTime ocr_backtime;
	string ocr_createid = string.Empty;
	string ocr_createname = string.Empty;
	DateTime ocr_createdate;
	string ocr_modid = string.Empty;
	string ocr_modname = string.Empty;
	DateTime ocr_moddate;
	string ocr_status = string.Empty;
	#endregion
	#region Public
	public string _ocr_id { set { ocr_id = value; } }
	public string _ocr_guid { set { ocr_guid = value; } }
	public string _ocr_parentid { set { ocr_parentid = value; } }
	public string _ocr_car { set { ocr_car = value; } }
	public DateTime _ocr_outtime { set { ocr_outtime = value; } }
	public DateTime _ocr_backtime { set { ocr_backtime = value; } }
	public string _ocr_createid { set { ocr_createid = value; } }
	public string _ocr_createname { set { ocr_createname = value; } }
	public DateTime _ocr_createdate { set { ocr_createdate = value; } }
	public string _ocr_modid { set { ocr_modid = value; } }
	public string _ocr_modname { set { ocr_modname = value; } }
	public DateTime _ocr_moddate { set { ocr_moddate = value; } }
	public string _ocr_status { set { ocr_status = value; } }
	#endregion

	public void addOfficialCar()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
declare @rowCount int;
select @rowCount=count(*) from OfficialCarRecord where ocr_parentid=@ocr_parentid

if @rowCount>0
	begin
		update OfficialCarRecord set
		ocr_outtime=@ocr_outtime,
		ocr_backtime=@ocr_backtime,
		ocr_moddate=@ocr_moddate
		where ocr_parentid=@ocr_parentid
	end
else
	begin
		insert into OfficialCarRecord (
		ocr_guid,
		ocr_parentid,
		ocr_car,
		ocr_outtime,
		ocr_backtime,
		ocr_createid,
		ocr_createname,
		ocr_modid,
		ocr_modname,
		ocr_status
		) values (
		@ocr_guid,
		@ocr_parentid,
		@ocr_car,
		@ocr_outtime,
		@ocr_backtime,
		@ocr_createid,
		@ocr_createname,
		@ocr_modid,
		@ocr_modname,
		@ocr_status
		)	
	end ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@ocr_guid", ocr_guid);
		oCmd.Parameters.AddWithValue("@ocr_parentid", ocr_parentid);
		oCmd.Parameters.AddWithValue("@ocr_car", ocr_car);
		if (ocr_outtime.ToString("yyyy/MM/dd") == "0001/01/01")
			oCmd.Parameters.AddWithValue("@ocr_outtime", DBNull.Value);
		else
			oCmd.Parameters.AddWithValue("@ocr_outtime", ocr_outtime);
		if (ocr_backtime.ToString("yyyy/MM/dd") == "0001/01/01")
			oCmd.Parameters.AddWithValue("@ocr_backtime", DBNull.Value);
		else
			oCmd.Parameters.AddWithValue("@ocr_backtime", ocr_backtime);
		oCmd.Parameters.AddWithValue("@ocr_createid", ocr_createid);
		oCmd.Parameters.AddWithValue("@ocr_createname", ocr_createname);
		oCmd.Parameters.AddWithValue("@ocr_moddate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@ocr_modid", ocr_modid);
		oCmd.Parameters.AddWithValue("@ocr_modname", ocr_modname);
		oCmd.Parameters.AddWithValue("@ocr_status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}