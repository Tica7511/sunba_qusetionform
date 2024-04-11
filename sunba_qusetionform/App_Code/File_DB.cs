using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// File_DB 的摘要描述
/// </summary>
public class File_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region Private
	string File_ID = string.Empty;
	string File_Parentid = string.Empty;
	string File_Type = string.Empty;
	string File_Orgname = string.Empty;
	string File_Encryname = string.Empty;
	string File_Exten = string.Empty;
	string File_Size = string.Empty;
	DateTime File_CreateDate;
	string File_CreateId = string.Empty;
	DateTime File_ModDate;
	string File_ModId = string.Empty;
	string File_Status = string.Empty;
	#endregion
	#region Public
	public string _File_ID { set { File_ID = value; } }
	public string _File_Parentid { set { File_Parentid = value; } }
	public string _File_Type { set { File_Type = value; } }
	public string _File_Orgname { set { File_Orgname = value; } }
	public string _File_Encryname { set { File_Encryname = value; } }
	public string _File_Exten { set { File_Exten = value; } }
	public string _File_Size { set { File_Size = value; } }
	public DateTime _File_CreateDate { set { File_CreateDate = value; } }
	public string _File_CreateId { set { File_CreateId = value; } }
	public DateTime _File_ModDate { set { File_ModDate = value; } }
	public string _File_ModId { set { File_ModId = value; } }
	public string _File_Status { set { File_Status = value; } }
	#endregion

	public void InsertFile()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"insert into FileTable (
File_Parentid,
File_Type,
File_Orgname,
File_Encryname, 
File_Exten, 
File_Size,
File_CreateId,
File_ModId,
File_Status
) values (
@File_Parentid,
@File_Type,
@File_Orgname,
@File_Encryname, 
@File_Exten, 
@File_Size,
@File_CreateId,
@File_ModId,
@File_Status
) ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oCmd.Parameters.AddWithValue("@File_Type", File_Type);
		oCmd.Parameters.AddWithValue("@File_Orgname", File_Orgname);
		oCmd.Parameters.AddWithValue("@File_Encryname", File_Encryname);
		oCmd.Parameters.AddWithValue("@File_Exten", File_Exten);
		oCmd.Parameters.AddWithValue("@File_Size", File_Size);
		oCmd.Parameters.AddWithValue("@File_CreateId", File_CreateId);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);
		oCmd.Parameters.AddWithValue("@File_Status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public DataTable GetFileDetail()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"SELECT * from FileTable where File_ID=@File_ID and File_Status='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();
		
		oCmd.Parameters.AddWithValue("@File_ID", File_ID);
		oda.Fill(ds);
		return ds;
	}

	public DataTable GetFileList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"SELECT * from FileTable where File_Parentid=@File_Parentid and File_Status='A' ");

		if (File_Type != "")
			sb.Append(@"and File_Type=@File_Type ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@File_Type", File_Type);
		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oda.Fill(ds);
		return ds;
	}

	public void DeleteFile()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update FileTable set
File_ModDate=@File_ModDate,
File_ModId=@File_ModId,
File_Status='D'
where File_ID=@File_ID ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@File_ID", File_ID);
		oCmd.Parameters.AddWithValue("@File_ModDate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteFileList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update FileTable set
File_ModDate=@File_ModDate,
File_ModId=@File_ModId,
File_Status='D'
where File_Parentid=@File_Parentid ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oCmd.Parameters.AddWithValue("@File_ModDate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}


	public void DeleteFile_Trans(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update FileTable set
File_ModDate=@File_ModDate,
File_ModId=@File_ModId,
File_Status='D'
where File_Parentid=@File_Parentid ");

		if (File_Type != "")
			sb.Append(@"and File_Type=@File_Type");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@File_Type", File_Type);
		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oCmd.Parameters.AddWithValue("@File_ModDate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void UpdateFile()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update FileTable set 
File_Type=@File_Type,
File_Orgname=@File_Orgname,
File_Encryname=@File_Encryname, 
File_Exten=	@File_Exten, 
File_Size=@File_Size,
File_ModDate=@File_ModDate,
File_ModId=@File_ModId,
File_Status=@File_Status
where File_Parentid=@File_Parentid ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oCmd.Parameters.AddWithValue("@File_Type", File_Type);
		oCmd.Parameters.AddWithValue("@File_Orgname", File_Orgname);
		oCmd.Parameters.AddWithValue("@File_Encryname", File_Encryname);
		oCmd.Parameters.AddWithValue("@File_Exten", File_Exten);
		oCmd.Parameters.AddWithValue("@File_Size", File_Size);
		oCmd.Parameters.AddWithValue("@File_ModDate", DateTime.Now);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);
		oCmd.Parameters.AddWithValue("@File_Status", "A");

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void AddFile_Trans(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"
		insert into FileTable (
		File_Parentid,
		File_Type,
		File_Orgname,
		File_Encryname, 
		File_Exten, 
		File_Size,
		File_CreateId,
		File_ModId,
		File_Status
		) values (
		@File_Parentid,
		@File_Type,
		@File_Orgname,
		@File_Encryname, 
		@File_Exten, 
		@File_Size,
		@File_CreateId,
		@File_ModId,
		@File_Status
		) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@File_Parentid", File_Parentid);
		oCmd.Parameters.AddWithValue("@File_Type", File_Type);
		oCmd.Parameters.AddWithValue("@File_Orgname", File_Orgname);
		oCmd.Parameters.AddWithValue("@File_Encryname", File_Encryname);
		oCmd.Parameters.AddWithValue("@File_Exten", File_Exten);
		oCmd.Parameters.AddWithValue("@File_Size", File_Size);
		oCmd.Parameters.AddWithValue("@File_CreateId", File_CreateId);
		oCmd.Parameters.AddWithValue("@File_ModId", File_ModId);
		oCmd.Parameters.AddWithValue("@File_Status", "A");

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}
}