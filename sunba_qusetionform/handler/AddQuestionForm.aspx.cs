using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class handler_AddQuestionForm : System.Web.UI.Page
{
    FileTable_DB fdb = new FileTable_DB();
    QuestionForm_DB qdb = new QuestionForm_DB();
    QuestionFormLog_DB qldb = new QuestionFormLog_DB();
    //Admin_DB adb = new Admin_DB();
    CodeTable_DB cdb = new CodeTable_DB();
    SendMail send_mail = new SendMail();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/編輯 提問表 
        ///說    明:
        /// * Request["guid"]: guid 
        /// * Request["cpid"]: 業者Guid 
        /// * Request["category"]: 網頁類別 gas/oil 
        /// * Request["type"]: 檔案類型  
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmmd = new SqlCommand();
        oCmmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmmd.Transaction = myTrans;

        try
        {
            #region Check Session Timeout
            if (!LogInfo.isLogin)
            {
                throw new Exception("登入帳號已失效，請重新登入");
            }
            #endregion

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string num = (string.IsNullOrEmpty(Request["num"])) ? "" : Request["num"].ToString().Trim();
            string questionType = (string.IsNullOrEmpty(Request["questionType"])) ? "" : Request["questionType"].ToString().Trim();
            string day = (string.IsNullOrEmpty(Request["day"])) ? "" : Request["day"].ToString().Trim();
            string rtype = (string.IsNullOrEmpty(Request["rtype"])) ? "" : Request["rtype"].ToString().Trim();
            string nContent = (string.IsNullOrEmpty(Request["nContent"])) ? "" : Request["nContent"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string tmpGuid = (Server.UrlDecode(mode) == "new") ? Guid.NewGuid().ToString("N") : guid;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();
            DataTable adt = new DataTable();
            //string fillformname = "test";
            //string empid = "123456";
            //string orgid = "test";
            //string orgnization = "test";
            string fillformname = LogInfo.empName;
            string empid = LogInfo.empNo;
            string orgid = LogInfo.deptCode;
            string orgnization = LogInfo.deptName;
            string sn = string.Empty;
            string item = string.Empty;
            string nMonth = string.Empty;
            string email = string.Empty;
            string mailContent = string.Empty;
            string mailTo = string.Empty;
            string rtype_v = string.Empty;
            string questionTypeName = string.Empty;

            qdb._guid = tmpGuid;
            qdb._程度 = Server.UrlDecode(rtype);
            qdb._內容 = Server.UrlDecode(nContent);

            if (Server.UrlDecode(mode) == "new")
            {
                qdb._問題類別 = Server.UrlDecode(questionType);
                qdb._提出日期 = Server.UrlDecode(day);
                qdb._員工編號 = Server.UrlDecode(empid);
                qdb._填表人 = Server.UrlDecode(fillformname);
                qdb._部門_id = Server.UrlDecode(orgid);
                qdb._部門 = Server.UrlDecode(orgnization);
                qdb._建立者 = LogInfo.empName;
                qdb._建立者id = LogInfo.empNo;
                qdb._修改者 = LogInfo.empName;
                qdb._修改者id = LogInfo.empNo;

                if (DateTime.Now.Month.ToString().Length == 1)
                {
                    nMonth = "0" + DateTime.Now.Month.ToString();
                }
                else
                {
                    nMonth = DateTime.Now.Month.ToString();
                }

                qdb._年度 = DateTime.Now.Year.ToString();
                qdb._月份 = nMonth;

                DataTable sndb = qdb.GetSn();

                if (sndb.Rows.Count > 0)
                {
                    sn = sndb.Rows[0]["序號"].ToString().Trim();
                    if (!string.IsNullOrEmpty(sn))
                    {
                        sn = (Convert.ToInt32(sn) + 1).ToString();
                        if (sn.Length < 2)
                            sn = "0" + sn;
                    }
                    else
                    {
                        sn = "01";
                    }

                }

                DataTable idt = qdb.GetMaxitem();

                if (idt.Rows.Count > 0)
                {
                    item = (string.IsNullOrEmpty(idt.Rows[0]["項次"].ToString().Trim())) ? "1" : idt.Rows[0]["項次"].ToString().Trim();
                }

                qdb._項次 = item;
                qdb._序號 = sn;
                qdb._編號 = DateTime.Now.Year.ToString() + "-" + nMonth + "-" + sn;

                qdb.InsertData(oConn, myTrans);

                qldb._類別 = "新增";
                qldb._儲存類別 = "填表人";
                qldb._填表人 = Server.UrlDecode(fillformname);
                qldb._儲存內容 = Server.UrlDecode(orgnization) + Server.UrlDecode(day)
                    + Server.UrlDecode(rtype) + Server.UrlDecode(nContent) + item + sn + DateTime.Now.Year.ToString() + "-" + nMonth + "-" + sn;
                
                qldb.InsertData(oConn, myTrans);

                cdb._群組代碼 = "001";
                cdb._項目代碼 = Server.UrlDecode(questionType);
                DataTable qdt = cdb.GetList();

                if (qdt.Rows.Count > 0)
                {
                    questionTypeName = qdt.Rows[0]["項目名稱"].ToString().Trim();
                }

                cdb._群組代碼 = "002";
                cdb._項目代碼 = Server.UrlDecode(questionType);
                adt = cdb.GetList();

                if (adt.Rows.Count > 0)
                {
                    mailTo = adt.Rows[0]["項目名稱"].ToString().Trim();
                }

                cdb._群組代碼 = "005";
                cdb._項目代碼 = Server.UrlDecode(rtype);
                dt = cdb.GetList();

                if (dt.Rows.Count > 0)
                {
                    rtype_v = dt.Rows[0]["項目名稱"].ToString().Trim();
                }

                string day_v = string.IsNullOrEmpty(day) ? "" : Server.UrlDecode(day).Substring(0, 4) + "/" + Server.UrlDecode(day).Substring(4, 2) + "/" + Server.UrlDecode(day).Substring(6, 2);

                string Subject = "提問單填表人員問題新增通知";

                mailContent = "系統通知:<br><br>線上提問單系統已有同仁新增問題，請上 <a href = 'https://powersunba.com.tw/SunBa_Question/WebPage/index.aspx'>線上提問單系統</a>";
                mailContent += " 進行檢視並確認問題內容，以下為問題詳細資料，感謝您<br>";
                mailContent += "編號: " + DateTime.Now.Year.ToString() + "-" + nMonth + "-" + sn + "<br/>問題類別:" + questionTypeName + "<br/>填表人: " + Server.UrlDecode(fillformname) +
                    "<br/>提出日期: " + day_v + "<br/>急迫性: " + rtype_v + "<br/>問題描述: " + Server.UrlDecode(nContent);
                send_mail.MailTo(mailTo, Subject, mailContent);
            }
            else
            {
                qdb._修改者 = LogInfo.empName;
                qdb._修改者id = LogInfo.empNo;
                qdb.UpdateData(oConn, myTrans);

                qldb._類別 = "編輯";
                qldb._儲存類別 = "填表人員";
                qldb._填表人 = Server.UrlDecode(fillformname);
                qldb._儲存內容 = Server.UrlDecode(orgnization) + Server.UrlDecode(day)
                    + Server.UrlDecode(rtype) + Server.UrlDecode(nContent);

                qldb.InsertData(oConn, myTrans);
            }


            // 檔案上傳
            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "question\\";
                    //原檔名
                    string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                    //副檔名
                    string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                    //取得TIME與GUID
                    string timeguid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N");
                    //儲存的名稱
                    string newName = timeguid.Replace("..", "").Replace("\\", "") + extension.Replace("..", "").Replace("\\", "");

                    string file_size = File.ContentLength.ToString();

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    cdb._群組代碼 = "006";
                    cdb._項目代碼 = "01";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        fdb._guid = tmpGuid;
                        fdb._檔案類型 = "01";

                        if (Server.UrlDecode(mode) == "new")
                        {
                            sn = "0" + (i + 1).ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sn))
                            {
                                if (0 < Convert.ToInt32(sn) && Convert.ToInt32(sn) < 9)
                                {
                                    sn = "0" + (Convert.ToInt32(sn) + 1).ToString();
                                }
                                else
                                {
                                    sn = (Convert.ToInt32(sn) + 1).ToString();
                                }
                            }
                            else
                            {
                                DataTable fdt = fdb.GetMaxSn();

                                if (fdt.Rows.Count > 0)
                                {
                                    int maxsn = Convert.ToInt32(fdt.Rows[0]["Sort"].ToString().Trim());
                                    if (maxsn > 9)
                                        sn = maxsn.ToString();
                                    else
                                        sn = "0" + maxsn.ToString();
                                }
                                else
                                {
                                    sn = "01";
                                }
                            }
                        }
                    }

                    File.SaveAs(UpLoadPath + newName);
                    //進資料庫前, 儲存名稱要去除副檔名
                    newName = newName.Replace(extension, "");

                    fdb._guid = tmpGuid;
                    fdb._原檔名 = orgName;
                    fdb._新檔名 = newName;
                    fdb._附檔名 = extension;
                    fdb._排序 = sn;
                    fdb._檔案大小 = file_size;
                    fdb._建立者 = LogInfo.empName;
                    fdb._建立者id = LogInfo.empNo;
                    fdb._建立日期 = DateTime.Now;
                    fdb._修改者 = LogInfo.empName;
                    fdb._修改者id = LogInfo.empNo;
                    fdb._修改日期 = DateTime.Now;

                    fdb.UpdateFile_Trans(oConn, myTrans);
                }
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            myTrans.Rollback();
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        finally
        {
            oCmmd.Connection.Close();
            oConn.Close();
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}