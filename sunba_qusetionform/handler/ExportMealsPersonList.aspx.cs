using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using NPOI.XSSF.UserModel;//-- XSSF 用來產生Excel 2007檔案（.xlsx）
using NPOI.SS.UserModel;//-- v.1.2.4起 新增的。
using NPOI.SS.Util;

public partial class handler_ExportMealsPersonList : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 當日用餐名單匯出
		///-----------------------------------------------------
		string today = DateTime.Now.ToString("yyyy-MM-dd");
		DataSet ds = db.GetExportMealsPersonList(today);

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		MemoryStream MS = new MemoryStream();
		DataTable dt = new DataTable();

		#region 員工
		XSSFSheet eSheet = (XSSFSheet)workbook.CreateSheet("員工");
		// 表頭
		IRow e_row = eSheet.CreateRow(0);
		e_row.CreateCell(0).SetCellValue("日期");
		e_row.CreateCell(1).SetCellValue("登記者工號");
		e_row.CreateCell(2).SetCellValue("登記者姓名");
		e_row.CreateCell(3).SetCellValue("午餐份數");
		e_row.CreateCell(4).SetCellValue("午餐用餐地點");
		e_row.CreateCell(5).SetCellValue("晚餐份數");
		e_row.CreateCell(6).SetCellValue("晚餐用餐地點");

		//設置欄位寬度
		eSheet.SetColumnWidth(0, 20 * 256);
		eSheet.SetColumnWidth(1, 20 * 256);
		eSheet.SetColumnWidth(2, 20 * 256);
		eSheet.SetColumnWidth(3, 20 * 256);
		eSheet.SetColumnWidth(4, 20 * 256);
		eSheet.SetColumnWidth(5, 20 * 256);
		eSheet.SetColumnWidth(6, 20 * 256);

		dt = ds.Tables[0];
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			e_row = eSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			e_row.CreateCell(0).SetCellValue(dt.Rows[i]["mr_date"].ToString().Trim());
			e_row.CreateCell(1).SetCellValue(dt.Rows[i]["mr_createid"].ToString().Trim());
			e_row.CreateCell(2).SetCellValue(dt.Rows[i]["mr_createname"].ToString().Trim());
			string LNum = (dt.Rows[i]["mr_lunch"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_lunch_num"].ToString().Trim() : "0";
			e_row.CreateCell(3).SetCellValue(LNum);
			string LunchLocation = (dt.Rows[i]["mr_lunch"].ToString().Trim() == "Y") ? dt.Rows[i]["LunchLocation"].ToString().Trim() : "";
			e_row.CreateCell(4).SetCellValue(LunchLocation);
			string DNum = (dt.Rows[i]["mr_dinner"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_dinner_num"].ToString().Trim() : "0";
			e_row.CreateCell(5).SetCellValue(DNum);
			string DinnerLocation = (dt.Rows[i]["mr_dinner"].ToString().Trim() == "Y") ? dt.Rows[i]["DinnerLocation"].ToString().Trim() : "";
			e_row.CreateCell(6).SetCellValue(DinnerLocation);
		}
		#endregion

		#region 廠商
		XSSFSheet cSheet = (XSSFSheet)workbook.CreateSheet("廠商");
		// 表頭
		IRow c_row = cSheet.CreateRow(0);
		c_row.CreateCell(0).SetCellValue("日期");
		c_row.CreateCell(1).SetCellValue("登記者工號");
		c_row.CreateCell(2).SetCellValue("登記者姓名");
		c_row.CreateCell(3).SetCellValue("廠商名稱");
		c_row.CreateCell(4).SetCellValue("午餐份數");
		c_row.CreateCell(5).SetCellValue("午餐用餐地點");
		c_row.CreateCell(6).SetCellValue("晚餐份數");
		c_row.CreateCell(7).SetCellValue("晚餐用餐地點");

		//設置欄位寬度
		cSheet.SetColumnWidth(0, 20 * 256);
		cSheet.SetColumnWidth(1, 20 * 256);
		cSheet.SetColumnWidth(2, 20 * 256);
		cSheet.SetColumnWidth(3, 20 * 256);
		cSheet.SetColumnWidth(4, 20 * 256);
		cSheet.SetColumnWidth(5, 20 * 256);
		cSheet.SetColumnWidth(6, 20 * 256);
		cSheet.SetColumnWidth(7, 20 * 256);

		dt = ds.Tables[1];
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			c_row = cSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			c_row.CreateCell(0).SetCellValue(dt.Rows[i]["mr_date"].ToString().Trim());
			c_row.CreateCell(1).SetCellValue(dt.Rows[i]["mr_createid"].ToString().Trim());
			c_row.CreateCell(2).SetCellValue(dt.Rows[i]["mr_createname"].ToString().Trim());
			c_row.CreateCell(3).SetCellValue(dt.Rows[i]["CompanyName"].ToString().Trim());
			string LNum = (dt.Rows[i]["mr_lunch"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_lunch_num"].ToString().Trim() : "0";
			c_row.CreateCell(4).SetCellValue(LNum);
			string LunchLocation = (dt.Rows[i]["mr_lunch"].ToString().Trim() == "Y") ? dt.Rows[i]["LunchLocation"].ToString().Trim() : "";
			c_row.CreateCell(5).SetCellValue(LunchLocation);
			string DNum = (dt.Rows[i]["mr_dinner"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_dinner_num"].ToString().Trim() : "0";
			c_row.CreateCell(6).SetCellValue(DNum);
			string DinnerLocation = (dt.Rows[i]["mr_dinner"].ToString().Trim() == "Y") ? dt.Rows[i]["DinnerLocation"].ToString().Trim() : "";
			c_row.CreateCell(7).SetCellValue(DinnerLocation);
		}
		#endregion

		#region 愛心便當
		XSSFSheet lSheet = (XSSFSheet)workbook.CreateSheet("愛心便當");
		// 表頭
		IRow l_row = lSheet.CreateRow(0);
		l_row.CreateCell(0).SetCellValue("日期");
		l_row.CreateCell(1).SetCellValue("登記者工號");
		l_row.CreateCell(2).SetCellValue("登記者姓名");
		l_row.CreateCell(3).SetCellValue("廠商名稱");
		l_row.CreateCell(4).SetCellValue("午餐份數");
		l_row.CreateCell(5).SetCellValue("晚餐份數");

		//設置欄位寬度
		lSheet.SetColumnWidth(0, 20 * 256);
		lSheet.SetColumnWidth(1, 20 * 256);
		lSheet.SetColumnWidth(2, 20 * 256);
		lSheet.SetColumnWidth(3, 20 * 256);
		lSheet.SetColumnWidth(4, 20 * 256);
		lSheet.SetColumnWidth(5, 20 * 256);

		dt = ds.Tables[2];
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			l_row = lSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			l_row.CreateCell(0).SetCellValue(dt.Rows[i]["mr_date"].ToString().Trim());
			l_row.CreateCell(1).SetCellValue(dt.Rows[i]["mr_createid"].ToString().Trim());
			l_row.CreateCell(2).SetCellValue(dt.Rows[i]["mr_createname"].ToString().Trim());
			l_row.CreateCell(3).SetCellValue(dt.Rows[i]["CompanyName"].ToString().Trim());
			string LNum = (dt.Rows[i]["mr_lunch"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_lunch_num"].ToString().Trim() : "0";
			l_row.CreateCell(4).SetCellValue(LNum);
			string DNum = (dt.Rows[i]["mr_dinner"].ToString().Trim() == "Y") ? dt.Rows[i]["mr_dinner_num"].ToString().Trim() : "0";
			l_row.CreateCell(5).SetCellValue(DNum);
		}
		#endregion

		#region 訪客
		XSSFSheet vSheet = (XSSFSheet)workbook.CreateSheet("訪客");
		// 表頭
		IRow v_row = vSheet.CreateRow(0);
		v_row.CreateCell(0).SetCellValue("日期");
		v_row.CreateCell(1).SetCellValue("登記者工號");
		v_row.CreateCell(2).SetCellValue("登記者姓名");
		v_row.CreateCell(3).SetCellValue("廠商名稱");
		v_row.CreateCell(4).SetCellValue("午餐份數");
		v_row.CreateCell(5).SetCellValue("晚餐份數");

		//設置欄位寬度
		vSheet.SetColumnWidth(0, 20 * 256);
		vSheet.SetColumnWidth(1, 20 * 256);
		vSheet.SetColumnWidth(2, 20 * 256);
		vSheet.SetColumnWidth(3, 20 * 256);
		vSheet.SetColumnWidth(4, 30 * 256);
		vSheet.SetColumnWidth(5, 30 * 256);

		dt = ds.Tables[3];
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			v_row = vSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			v_row.CreateCell(0).SetCellValue(dt.Rows[i]["mv_date"].ToString().Trim());
			v_row.CreateCell(1).SetCellValue(dt.Rows[i]["mv_createid"].ToString().Trim());
			v_row.CreateCell(2).SetCellValue(dt.Rows[i]["mv_createname"].ToString().Trim());
			v_row.CreateCell(3).SetCellValue(dt.Rows[i]["mv_name"].ToString().Trim());
			v_row.CreateCell(4).SetCellValue(dt.Rows[i]["lunch"].ToString().Trim());
			v_row.CreateCell(5).SetCellValue(dt.Rows[i]["dinner"].ToString().Trim());
		}
		#endregion

		// 資料中有 \n 換行時,匯出至 Excel 時也會換行
		//ICellStyle WrapStyle = workbook.CreateCellStyle();
		//WrapStyle.WrapText = true;

		workbook.Write(MS);
		string FileName = today + "_當日用餐名單.xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}