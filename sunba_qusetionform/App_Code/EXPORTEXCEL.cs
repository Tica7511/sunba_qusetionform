using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

/// <summary>
/// Gas_EXPORTEXCEL 的摘要描述
/// </summary>
namespace ED.HR.EXPORTEXCEL.WebForm
{
    public partial class Excel : System.Web.UI.Page
    {
        QuestionForm_DB db = new QuestionForm_DB();
        Reply_DB rdb = new Reply_DB();
        Competence_DB cdb = new Competence_DB();
        FileTable_DB fdb = new FileTable_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            string item = (string.IsNullOrEmpty(Request["item"])) ? "" : Request["item"].ToString().Trim();
            string num = (string.IsNullOrEmpty(Request["num"])) ? "" : Request["num"].ToString().Trim();
            string questionType = (string.IsNullOrEmpty(Request["questionType"])) ? "" : Request["questionType"].ToString().Trim();
            string empid = (string.IsNullOrEmpty(Request["empid"])) ? "" : Request["empid"].ToString().Trim();
            string orgnization = (string.IsNullOrEmpty(Request["orgnization"])) ? "" : Request["orgnization"].ToString().Trim();
            string startday = (string.IsNullOrEmpty(Request["startday"])) ? "" : Request["startday"].ToString().Trim();
            string endday = (string.IsNullOrEmpty(Request["endday"])) ? "" : Request["endday"].ToString().Trim();
            string state = (string.IsNullOrEmpty(Request["state"])) ? "" : Request["state"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string replycontent = (string.IsNullOrEmpty(Request["replycontent"])) ? "" : Request["replycontent"].ToString().Trim();
            string rtype = string.IsNullOrEmpty(Request["cktype"]) ? "" : Request["cktype"].ToString().Trim();
            string isclosed = string.IsNullOrEmpty(Request["isclosed"]) ? "" : Request["isclosed"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string urgency = (string.IsNullOrEmpty(Request["urgency"])) ? "" : Request["urgency"].ToString().Trim();
            string cpName = string.Empty;
            string fileName = string.Empty;

            string FilePath = Server.MapPath("~/Sample/ExportExcel.xls");
            HSSFWorkbook hssfworkbook;
            FileStream sampleFile;

            sampleFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            using (sampleFile)
            {
                //建立Excel
                hssfworkbook = new HSSFWorkbook(sampleFile);
            }

            DataTable dt = new DataTable();
            ISheet sheet = hssfworkbook.GetSheetAt(0);

            #region 管線基本資料

            db._項次 = item;
            db._編號 = num;
            db._問題類別 = questionType;
            db._員工編號 = empid;
            db._部門_id = orgnization;
            db._程度 = rtype;
            db._目前狀態 = state;
            db._內容 = content;
            db._回覆內容 = replycontent;
            db._程度 = urgency;
            db._是否結案 = isclosed;

            dt = db.GetExcelList(startday, endday);

            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("項次");
            sheet.GetRow(0).CreateCell(1).SetCellValue("編號");
            sheet.GetRow(0).CreateCell(2).SetCellValue("問題類別");
            sheet.GetRow(0).CreateCell(3).SetCellValue("是否結案");
            sheet.GetRow(0).CreateCell(4).SetCellValue("填表內容");
            sheet.GetRow(0).CreateCell(5).SetCellValue("回覆內容");
            sheet.GetRow(0).CreateCell(6).SetCellValue("填表人");
            sheet.GetRow(0).CreateCell(7).SetCellValue("部門");
            sheet.GetRow(0).CreateCell(8).SetCellValue("提出日期");
            sheet.GetRow(0).CreateCell(9).SetCellValue("急迫性");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sheet.CreateRow(i + 1);
                    sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["項次"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["編號"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["問題類別_V"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["是否結案_V"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["內容_V"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["回覆內容_V"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["填表人"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["部門"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["提出日期"].ToString().Trim());
                    sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["程度_V"].ToString().Trim());
                }
            }
            fileName = "提問單匯出總表.xls";

            #endregion

            Response.ContentType = "application / vnd.ms - excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.Clear();

            using (MemoryStream ms = new MemoryStream())
            {
                hssfworkbook.Write(ms);

                Response.BinaryWrite(ms.GetBuffer());
                Response.Flush();
                Response.End();
            }
        }
    }
}