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

public partial class handler_ExportMealsPayment : System.Web.UI.Page
{
	MealsRegister_DB mr_db = new MealsRegister_DB();
	MealsVisitor_DB mv_db = new MealsVisitor_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 餐費繳款清單匯出
		///說明:
		/// * Request["category"]:類別
		/// * Request["SearchStr"]:查詢條件
		///-----------------------------------------------------
		string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
		string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Common.FilterCheckMarxString(Request["year"].ToString().Trim());
		string month = (string.IsNullOrEmpty(Request["month"])) ? "" : Common.FilterCheckMarxString(Request["month"].ToString().Trim());
		string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		XSSFSheet WorkSheet = (XSSFSheet)workbook.CreateSheet("工作表1");
		MemoryStream MS = new MemoryStream();

		string FileName = string.Empty;
		IRow dt_row = WorkSheet.CreateRow(0);
		DataTable dt = new DataTable();
		switch (category)
		{
			case "Employee":
				FileName = "同仁餐費統計表";
				mr_db._mr_person_id = SearchStr;
				dt = mr_db.GetPaymentExportData(year, month, category);
				// 表頭
				dt_row.CreateCell(0).SetCellValue("工號");
				dt_row.CreateCell(1).SetCellValue("單位");
				dt_row.CreateCell(2).SetCellValue("姓名");
				dt_row.CreateCell(3).SetCellValue("餐費");
				// 資料
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int x = i + 1;
					dt_row = WorkSheet.CreateRow(x);
					dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["mr_person_id"].ToString().Trim());
					dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["EmpDept"].ToString().Trim());
					dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["EmpName"].ToString().Trim());
					dt_row.CreateCell(3).SetCellValue(dt.Rows[i]["SumPrice"].ToString().Trim());
				}
				break;
			case "Company":
				FileName = "廠商餐費統計表";
				mr_db._KeyWord = SearchStr;
				dt = mr_db.GetPaymentExportData(year, month, category);
				// 表頭
				dt_row.CreateCell(0).SetCellValue("廠商名稱");
				dt_row.CreateCell(1).SetCellValue("類別");
				dt_row.CreateCell(2).SetCellValue("餐費");
				// 資料
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int x = i + 1;
					dt_row = WorkSheet.CreateRow(x);
					dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["mc_name"].ToString().Trim());
					dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["mc_category"].ToString().Trim());
					dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["SumPrice"].ToString().Trim());
				}
				break;
			case "Visitor":
				FileName = "訪客餐費統計表";
				mv_db._KeyWord = SearchStr;
				dt = mv_db.GetPaymentExportData(year, month);
				// 表頭
				dt_row.CreateCell(0).SetCellValue("日期");
				dt_row.CreateCell(1).SetCellValue("申請人");
				dt_row.CreateCell(2).SetCellValue("午餐份數");
				dt_row.CreateCell(3).SetCellValue("晚餐份數");
				dt_row.CreateCell(4).SetCellValue("餐費");
				// 資料
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int x = i + 1;
					dt_row = WorkSheet.CreateRow(x);
					dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["mv_date"].ToString().Trim());
					dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["mv_createname"].ToString().Trim());
					dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["lunchNum"].ToString().Trim());
					dt_row.CreateCell(3).SetCellValue(dt.Rows[i]["dinnerNum"].ToString().Trim());
					dt_row.CreateCell(4).SetCellValue(dt.Rows[i]["SumPrice"].ToString().Trim());
				}
				break;
		}

		//設置欄位寬度
		WorkSheet.SetColumnWidth(0, 15 * 256);
		WorkSheet.SetColumnWidth(1, 15 * 256);
		WorkSheet.SetColumnWidth(2, 25 * 256);
		WorkSheet.SetColumnWidth(3, 25 * 256);
		WorkSheet.SetColumnWidth(4, 15 * 256);
		

		// 資料中有 \n 換行時,匯出至 Excel 時也會換行
		//ICellStyle WrapStyle = workbook.CreateCellStyle();
		//WrapStyle.WrapText = true;
		workbook.Write(MS);

		FileName = year + "年" + month + "月_" + FileName + ".xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}