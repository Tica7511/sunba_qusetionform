using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MealsCostItem_DB 的摘要描述
/// </summary>
public class MealsCostItem_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string mci_id = string.Empty;
	string mci_guid = string.Empty;
	string mci_parentid = string.Empty;
	string mci_item = string.Empty;
	int mci_num;
	int mci_unitprice;
	int mci_price;
	string mci_company = string.Empty;
	string mci_createid = string.Empty;
	string mci_createname = string.Empty;
	DateTime mci_createdate;
	string mci_modid = string.Empty;
	string mci_modname = string.Empty;
	DateTime mci_moddate;
	string mci_status = string.Empty;
	#endregion
	#region Public
	public string _mci_id { set { mci_id = value; } }
	public string _mci_guid { set { mci_guid = value; } }
	public string _mci_parentid { set { mci_parentid = value; } }
	public string _mci_item { set { mci_item = value; } }
	public int _mci_num { set { mci_num = value; } }
	public int _mci_unitprice { set { mci_unitprice = value; } }
	public int _mci_price { set { mci_price = value; } }
	public string _mci_company { set { mci_company = value; } }
	public string _mci_createid { set { mci_createid = value; } }
	public string _mci_createname { set { mci_createname = value; } }
	public DateTime _mci_createdate { set { mci_createdate = value; } }
	public string _mci_modid { set { mci_modid = value; } }
	public string _mci_modname { set { mci_modname = value; } }
	public DateTime _mci_moddate { set { mci_moddate = value; } }
	public string _mci_status { set { mci_status = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from MealsCostItem where mci_status='A' and mci_parentid=@mci_parentid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;

		oCmd.Parameters.AddWithValue("@mci_parentid", mci_parentid);

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		oda.Fill(ds);
		return ds;
	}

	public void addMealsCostItem(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into MealsCostItem (
mci_guid,
mci_parentid,
mci_item,
mci_num,
mci_unitprice,
mci_price,
mci_company,
mci_createid,
mci_createname,
mci_modid,
mci_modname,
mci_status
) values (
@mci_guid,
@mci_parentid,
@mci_item,
@mci_num,
@mci_unitprice,
@mci_price,
@mci_company,
@mci_createid,
@mci_createname,
@mci_modid,
@mci_modname,
@mci_status
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		
		oCmd.Parameters.AddWithValue("@mci_guid", mci_guid);
		oCmd.Parameters.AddWithValue("@mci_parentid", mci_parentid);
		oCmd.Parameters.AddWithValue("@mci_item", mci_item);
		oCmd.Parameters.AddWithValue("@mci_num", mci_num);
		oCmd.Parameters.AddWithValue("@mci_unitprice", mci_unitprice);
		oCmd.Parameters.AddWithValue("@mci_price", mci_price);
		oCmd.Parameters.AddWithValue("@mci_company", mci_company);
		oCmd.Parameters.AddWithValue("@mci_createid", mci_createid);
		oCmd.Parameters.AddWithValue("@mci_createname", mci_createname);
		oCmd.Parameters.AddWithValue("@mci_modid", mci_modid);
		oCmd.Parameters.AddWithValue("@mci_modname", mci_modname);
		oCmd.Parameters.AddWithValue("@mci_status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void DeleteItemList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update MealsCostItem set
mci_status='D',
mci_modid=@mci_modid,
mci_modname=@mci_modname,
mci_moddate=@mci_moddate
where mci_parentid=@mci_parentid and mci_status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		oCmd.Parameters.AddWithValue("@mci_parentid", mci_parentid);
		oCmd.Parameters.AddWithValue("@mci_modid", mci_modid);
		oCmd.Parameters.AddWithValue("@mci_modname", mci_modname);
		oCmd.Parameters.AddWithValue("@mci_moddate", DateTime.Now);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}