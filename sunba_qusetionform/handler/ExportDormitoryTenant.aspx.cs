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

public partial class handler_ExportDormitoryTenant : System.Web.UI.Page
{
	DormitoryTenant_DB db = new DormitoryTenant_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 宿舍入住清單匯出
		///說明:
		/// * Request["SearchStr"]:關鍵字
		/// * Request["Area"]:廠區
		///-----------------------------------------------------
		string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
		string Area = (string.IsNullOrEmpty(Request["Area"])) ? "" : Common.FilterCheckMarxString(Request["Area"].ToString().Trim());

		db._KeyWord = SearchStr;
		DataSet ds = db.GetManageList("1", "9999999", Area);
		DataTable dt = ds.Tables[1];

		IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
		XSSFSheet WorkSheet = (XSSFSheet)workbook.CreateSheet("工作表1");
		MemoryStream MS = new MemoryStream();

		//********************第一列表頭 ********************
		IRow dt_row = WorkSheet.CreateRow(0);
		dt_row.CreateCell(0).SetCellValue("項次");
		dt_row.CreateCell(1).SetCellValue("宿舍房號");
		dt_row.CreateCell(2).SetCellValue("宿舍分機");
		dt_row.CreateCell(3).SetCellValue("房型");
		dt_row.CreateCell(4).SetCellValue("類型");
		dt_row.CreateCell(5).SetCellValue("備註");
		dt_row.CreateCell(6).SetCellValue("住宿人員");

		//設置欄位寬度
		WorkSheet.SetColumnWidth(0, 6 * 256);
		WorkSheet.SetColumnWidth(1, 15 * 256);
		WorkSheet.SetColumnWidth(2, 15 * 256);
		WorkSheet.SetColumnWidth(3, 15 * 256);
		WorkSheet.SetColumnWidth(4, 15 * 256);
		WorkSheet.SetColumnWidth(5, 30 * 256);
		WorkSheet.SetColumnWidth(6, 25 * 256);

		// 資料中有 \n 換行時,匯出至 Excel 時也會換行
		//ICellStyle WrapStyle = workbook.CreateCellStyle();
		//WrapStyle.WrapText = true;

		for (int i = 0; i < dt.Rows.Count; i++)
		{
			int x = i + 1;
			dt_row = WorkSheet.CreateRow(x);    // 在工作表裡面，產生一列。
			dt_row.CreateCell(0).SetCellValue(dt.Rows[i]["itemNo"].ToString().Trim());
			dt_row.CreateCell(1).SetCellValue(dt.Rows[i]["dr_no"].ToString().Trim());
			dt_row.CreateCell(2).SetCellValue(dt.Rows[i]["dr_ext"].ToString().Trim());
			dt_row.CreateCell(3).SetCellValue(dt.Rows[i]["RoomTypeCn"].ToString().Trim());
			dt_row.CreateCell(4).SetCellValue(dt.Rows[i]["TypeCn"].ToString().Trim());
			dt_row.CreateCell(5).SetCellValue(dt.Rows[i]["dr_ps"].ToString().Trim());
			dt_row.CreateCell(6).SetCellValue(dt.Rows[i]["dt_name"].ToString().Trim());
		}
		workbook.Write(MS);
		string FileName = "入住管理清冊(" + Area + "期).xlsx";
		Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
		Response.BinaryWrite(MS.ToArray());
		workbook = null;
		MS.Close();
		MS.Dispose();
	}
}