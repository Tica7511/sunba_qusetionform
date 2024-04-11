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
using NPOI.HSSF.UserModel;

public partial class handler_ExportOutdoorManagerList : System.Web.UI.Page
{
	OutdoorForm_DB db = new OutdoorForm_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 外出單查詢管理清單匯出
		///說明:
		/// * Request["SearchStr"]:關鍵字
		/// * Request["SearchType"]:申請類別
		/// * Request["SearchCarNo"]:車號
		/// * Request["SearchStartDate"]:預計離廠日
		/// * Request["SearchEndDate"]:預計返廠日
		/// * Request["SearchActualOut"]:實際離廠日
		/// * Request["SearchActualBack"]:實際返廠日
		///-----------------------------------------------------

		string SearchStr = (Request["SearchStr"] != null) ? Request["SearchStr"].ToString().Trim() : "";
		string SearchType = (Request["SearchType"] != null) ? Request["SearchType"].ToString().Trim() : "";
		string SearchCarNo = (Request["SearchCarNo"] != null) ? Request["SearchCarNo"].ToString().Trim() : "";
		string SearchStartDate = (Request["SearchStartDate"] != null) ? Request["SearchStartDate"].ToString().Trim() : "";
		string SearchEndDate = (Request["SearchEndDate"] != null) ? Request["SearchEndDate"].ToString().Trim() : "";
		string SearchActualOut = (Request["SearchActualOut"] != null) ? Request["SearchActualOut"].ToString().Trim() : "";
		string SearchActualBack = (Request["SearchActualBack"] != null) ? Request["SearchActualBack"].ToString().Trim() : "";

		db._KeyWord = SearchStr;
		db._o_type = SearchType;
		DataSet ds = db.GetManagerList(SearchCarNo, SearchStartDate, SearchEndDate, SearchActualOut, SearchActualBack, "1", "999999999");
		DataTable dt = ds.Tables[1];

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		XSSFSheet WorkSheet = (XSSFSheet)workbook.CreateSheet("工作表1");
		MemoryStream MS = new MemoryStream();

		// 表頭
		IRow dt_row = WorkSheet.CreateRow(0);
		dt_row.CreateCell(0).SetCellValue("申請類別");
		dt_row.CreateCell(1).SetCellValue("申請日期");
		dt_row.CreateCell(2).SetCellValue("申請人姓名");
		dt_row.CreateCell(3).SetCellValue("車號");
		dt_row.CreateCell(4).SetCellValue("地點");
		dt_row.CreateCell(5).SetCellValue("預計往返日期與時間");
		dt_row.CreateCell(6).SetCellValue("實際往返日期與時間");
		dt_row.CreateCell(7).SetCellValue("事由");


		// 設置欄位寬度
		WorkSheet.SetColumnWidth(0, 15 * 256);
		WorkSheet.SetColumnWidth(1, 15 * 256);
		WorkSheet.SetColumnWidth(2, 15 * 256);
		WorkSheet.SetColumnWidth(3, 15 * 256);
		WorkSheet.SetColumnWidth(4, 15 * 256);
		WorkSheet.SetColumnWidth(5, 25 * 256);
		WorkSheet.SetColumnWidth(6, 25 * 256);
		WorkSheet.SetColumnWidth(7, 15 * 256);

		// 換行
		ICellStyle WrapStyle = workbook.CreateCellStyle();
		WrapStyle.WrapText = true;

		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			dt_row = WorkSheet.CreateRow(x);
			dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["TypeCn"].ToString().Trim());
			dt_row.CreateCell(1).SetCellValue(DateTime.Parse(dt.Rows[i]["o_createdate"].ToString().Trim()).ToString("yyyy-MM-dd"));
			dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["o_createname"].ToString().Trim());
			dt_row.CreateCell(3).SetCellValue(dt.Rows[i]["CarNum"].ToString().Trim());
			dt_row.CreateCell(4).SetCellValue(dt.Rows[i]["o_place"].ToString().Trim());
			string ReserveStart = "出廠: " + DateTime.Parse(dt.Rows[i]["o_starttime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
			string ReserveEnd = "返廠: " + DateTime.Parse(dt.Rows[i]["o_endtime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
			dt_row.CreateCell(5).SetCellValue(ReserveStart + "\n" + ReserveEnd);
			dt_row.GetCell(5).CellStyle = WrapStyle;
			dt_row.CreateCell(6).SetCellValue("出廠: " + dt.Rows[i]["ActualOutTime"].ToString().Trim() + "\n" + "返廠: " + dt.Rows[i]["ActualBackTime"].ToString().Trim());
			dt_row.GetCell(6).CellStyle = WrapStyle;
			dt_row.CreateCell(7).SetCellValue(dt.Rows[i]["o_reason"].ToString().Trim());
			// 因為有換行,加高度
			dt_row.HeightInPoints = 2 * WorkSheet.DefaultRowHeight / 20;
		}

		workbook.Write(MS);
		string FileName = DateTime.Now.ToString("yyyyMMdd") + "_外出單查詢結果.xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}