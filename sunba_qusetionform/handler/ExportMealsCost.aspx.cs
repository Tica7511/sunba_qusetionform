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

public partial class handler_ExportMealsCost : System.Web.UI.Page
{
	MealsCost_DB db = new MealsCost_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 成本管理清單匯出
		///說明:
		/// * Request["category"]: 類別
		///-----------------------------------------------------
		string startdate = (string.IsNullOrEmpty(Request["startdate"])) ? "" : Common.FilterCheckMarxString(Request["startdate"].ToString().Trim());
		string enddate = (string.IsNullOrEmpty(Request["enddate"])) ? "" : Common.FilterCheckMarxString(Request["enddate"].ToString().Trim());
		string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());

		db._mc_category = category;
		DataTable dt = db.GetExportList(startdate, enddate);

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		XSSFSheet WorkSheet = (XSSFSheet)workbook.CreateSheet("工作表1");
		MemoryStream MS = new MemoryStream();

		//********************第一列表頭 ********************
		IRow dt_row = WorkSheet.CreateRow(0);
		dt_row.CreateCell(0).SetCellValue("日期");
		dt_row.CreateCell(1).SetCellValue("金額");
		dt_row.CreateCell(2).SetCellValue("備註");

		//設置欄位寬度
		WorkSheet.SetColumnWidth(0, 15 * 256);
		WorkSheet.SetColumnWidth(1, 15 * 256);
		WorkSheet.SetColumnWidth(2, 15 * 256);

		// 資料中有 \n 換行時,匯出至 Excel 時也會換行
		//ICellStyle WrapStyle = workbook.CreateCellStyle();
		//WrapStyle.WrapText = true;

		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			dt_row = WorkSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["mc_date"].ToString().Trim());
			dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["mc_price"].ToString().Trim());
			dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["mc_ps"].ToString().Trim());
		}
		workbook.Write(MS);
		string TypeName = (category == "income") ? "收入" : "成本";
		string FileName = TypeName + "項目統計.xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}