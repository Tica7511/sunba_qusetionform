using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Type_DB 的摘要描述
/// </summary>
public class CodeTable_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region 私用
	string C_ID = string.Empty;
	string C_Group_cn = string.Empty;
	string C_Group = string.Empty;
	string C_Item_cn = string.Empty;
	string C_Item = string.Empty;
	string C_Sort = string.Empty;
	#endregion
	#region 公用
	public string _C_ID { set { C_ID = value; } }
	public string _C_Group_cn { set { C_Group_cn = value; } }
	public string _C_Group { set { C_Group = value; } }
	public string _C_Item_cn { set { C_Item_cn = value; } }
	public string _C_Item { set { C_Item = value; } }
	public string _C_Sort { set { C_Sort = value; } }
	#endregion

	public DataTable GetCode()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select C_Item_cn,C_Item from CodeTable where C_Group=@C_Group ");

		if (C_Item != "")
			sb.Append(@"and C_Item=@C_Item ");

		sb.Append(@"order by C_Sort ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@C_Group", C_Group);
		oCmd.Parameters.AddWithValue("@C_Item", C_Item);

		oda.Fill(ds);
		return ds;
	}
}