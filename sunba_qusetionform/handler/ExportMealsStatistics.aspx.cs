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

public partial class handler_ExportMealsStatistics : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 同仁用餐統計匯出
		///-----------------------------------------------------
		DateTime today = DateTime.Now;
		// 今日星期，系統預設星期天為0
		int ToayOfWeek = ((int)today.DayOfWeek == 0) ? 7 : (int)today.DayOfWeek;
		// 本週的星期一
		int tmpM = 1 - ToayOfWeek;
		DateTime Mondate = today.AddDays(tmpM);
		// 本週的星期日
		int tmpS = 7 - ToayOfWeek;
		DateTime Sundate = today.AddDays(tmpS);

		string ThisMonday = Mondate.ToString("yyyy-MM-dd");
		string ThisSunday = Sundate.ToString("yyyy-MM-dd");

		DataSet ds = db.GetExportMealsStatistics(ThisMonday, ThisSunday);
		DataTable dt = ds.Tables[2];

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		XSSFSheet WorkSheet = (XSSFSheet)workbook.CreateSheet("工作表1");
		MemoryStream MS = new MemoryStream();

		// 表頭
		IRow dt_row = WorkSheet.CreateRow(0);
		dt_row.CreateCell(0).SetCellValue("日期");
		dt_row.CreateCell(1).SetCellValue("地點");
		dt_row.CreateCell(2).SetCellValue("午餐份數");
		dt_row.CreateCell(6).SetCellValue("合計");
		dt_row.CreateCell(7).SetCellValue("晚餐份數");
		dt_row.CreateCell(11).SetCellValue("合計");

		IRow dt_row2 = WorkSheet.CreateRow(1);
		dt_row2.CreateCell(2).SetCellValue("同仁");
		dt_row2.CreateCell(3).SetCellValue("廠商");
		dt_row2.CreateCell(4).SetCellValue("愛心");
		dt_row2.CreateCell(5).SetCellValue("訪客(葷/奶蛋素/全素)");
		dt_row2.CreateCell(7).SetCellValue("同仁");
		dt_row2.CreateCell(8).SetCellValue("廠商");
		dt_row2.CreateCell(9).SetCellValue("愛心");
		dt_row2.CreateCell(10).SetCellValue("訪客(葷/奶蛋素/全素)");


		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 1, 1, 1));
		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 5));
		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 1, 6, 6));
		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 0, 7, 10));
		WorkSheet.AddMergedRegion(new CellRangeAddress(0, 1, 11, 11));

		//設置欄位寬度
		WorkSheet.SetColumnWidth(0, 25 * 256);
		WorkSheet.SetColumnWidth(1, 25 * 256);
		WorkSheet.SetColumnWidth(2, 10 * 256);
		WorkSheet.SetColumnWidth(3, 10 * 256);
		WorkSheet.SetColumnWidth(4, 10 * 256);
		WorkSheet.SetColumnWidth(5, 25 * 256);
		WorkSheet.SetColumnWidth(6, 25 * 256);
		WorkSheet.SetColumnWidth(7, 10 * 256);
		WorkSheet.SetColumnWidth(8, 10 * 256);
		WorkSheet.SetColumnWidth(9, 10 * 256);
		WorkSheet.SetColumnWidth(10, 25 * 256);
		WorkSheet.SetColumnWidth(11, 25 * 256);

		// 資料中有 \n 換行時,匯出至 Excel 時也會換行
		//ICellStyle WrapStyle = workbook.CreateCellStyle();
		//WrapStyle.WrapText = true;

		int rSpanCount = int.Parse(ds.Tables[0].Rows[0]["LocationTotal"].ToString());
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 2;
			dt_row = WorkSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["mr_date"].ToString().Trim());
			dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["ml_name"].ToString().Trim());
			dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["lunchE"].ToString().Trim());
			dt_row.CreateCell(3).SetCellValue(dt.Rows[i]["lunchF"].ToString().Trim());
			dt_row.CreateCell(4).SetCellValue(dt.Rows[i]["lunchL"].ToString().Trim());
			dt_row.CreateCell(5).SetCellValue(dt.Rows[i]["lunchV"].ToString().Trim());
			dt_row.CreateCell(6).SetCellValue(dt.Rows[i]["totalLunchPrice"].ToString().Trim());
			dt_row.CreateCell(7).SetCellValue(dt.Rows[i]["dinnerE"].ToString().Trim());
			dt_row.CreateCell(8).SetCellValue(dt.Rows[i]["dinnerF"].ToString().Trim());
			dt_row.CreateCell(9).SetCellValue(dt.Rows[i]["dinnerL"].ToString().Trim());
			dt_row.CreateCell(10).SetCellValue(dt.Rows[i]["dinnerV"].ToString().Trim());
			dt_row.CreateCell(11).SetCellValue(dt.Rows[i]["totalDinnerPrice"].ToString().Trim());

			if (i % rSpanCount == 0)
			{
				WorkSheet.AddMergedRegion(new CellRangeAddress(x, (x + rSpanCount - 1), 0, 0));
				WorkSheet.AddMergedRegion(new CellRangeAddress(x, (x + rSpanCount - 1), 6, 6));
				WorkSheet.AddMergedRegion(new CellRangeAddress(x, (x + rSpanCount - 1), 11, 11));
			}
		}
		workbook.Write(MS);

		string FileName = "本週同仁用餐統計(" + ThisMonday + "~" + ThisSunday + ").xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}