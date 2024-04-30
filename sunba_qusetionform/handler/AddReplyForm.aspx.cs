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

public partial class handler_AddReplyForm : System.Web.UI.Page
{
    FileTable_DB fdb = new FileTable_DB();
    Reply_DB rdb = new Reply_DB();
    QuestionForm_DB qdb = new QuestionForm_DB();
    //Admin_DB adb = new Admin_DB();
    CodeTable_DB cdb = new CodeTable_DB();
    SendMail send_mail = new SendMail();
    SSO_DB sdb = new SSO_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/編輯 回覆表 
        ///說    明:
        /// * Request["guid"]: guid 
        /// * Request["returnday"]: 回覆日期 
        /// * Request["finishday"]: 預計完成日 
        /// * Request["state"]: 目前狀態
        /// * Request["nContent"]: 回覆內容
        /// * Request["mode"]: new=新增 edit=編輯  
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
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string returnday = (string.IsNullOrEmpty(Request["returnday"])) ? "" : Request["returnday"].ToString().Trim();
            string finishday = (string.IsNullOrEmpty(Request["finishday"])) ? "" : Request["finishday"].ToString().Trim();
            string state = (string.IsNullOrEmpty(Request["state"])) ? "" : Request["state"].ToString().Trim();
            string contract = string.IsNullOrEmpty(Request["ckcontract"]) ? "" : Request["ckcontract"].ToString().Trim();
            string nContent = (string.IsNullOrEmpty(Request["nContent"])) ? "" : Request["nContent"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string tmpGuid = (Server.UrlDecode(mode) == "new") ? Guid.NewGuid().ToString("N") : guid;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();
            DataTable qdt = new DataTable();
            DataTable adt = new DataTable();
            DataTable rdt = new DataTable();
            DataTable sdt = new DataTable();
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
            string ccMail = string.Empty;
            string qContent = string.Empty;
            string day = string.Empty;
            string rtype = string.Empty;
            string rtype_v = string.Empty;
            string questionType = string.Empty;
            string questionTypeName = string.Empty;

            rdb._guid = guid;
            rdb._預計完成日 = Server.UrlDecode(finishday);
            rdb._目前狀態 = Server.UrlDecode(state);
            rdb._需求是否在第一期合約中 = Server.UrlDecode(contract);
            rdb._回覆內容 = Server.UrlDecode(nContent);

            rdt = rdb.GetData();
            if (rdt.Rows.Count > 0)
            {
                mode = "edit";
            }
            else
            {
                mode = "new";
            }

            if (mode == "new")
            {
                rdb._回覆日期 = Server.UrlDecode(returnday);
                rdb._建立者 = LogInfo.empName;
                rdb._建立者id = LogInfo.empNo;
                rdb._修改者 = LogInfo.empName;
                rdb._修改者id = LogInfo.empNo;

                rdb.InsertData(oConn, myTrans);

                //qldb._類別 = "新增";
                //qldb._儲存類別 = "填表人";
                //qldb._填表人 = Server.UrlDecode(fillformname);
                //qldb._儲存內容 = Server.UrlDecode(orgnization) + Server.UrlDecode(day)
                //    + Server.UrlDecode(rtype) + Server.UrlDecode(nContent) + item + sn + DateTime.Now.Year.ToString() + "-" + nMonth + "-" + sn;
                //
                //qldb.InsertData(oConn, myTrans);

                qdb._guid = guid;
                qdt = qdb.GetData();

                if (qdt.Rows.Count > 0)
                {
                    day = qdt.Rows[0]["提出日期"].ToString().Trim();

                    cdb._群組代碼 = "001";
                    cdb._項目代碼 = qdt.Rows[0]["問題類別"].ToString().Trim();
                    DataTable qcdt = cdb.GetList();

                    if (qcdt.Rows.Count > 0)
                    {
                        questionTypeName = qcdt.Rows[0]["項目名稱"].ToString().Trim();
                    }

                    cdb._群組代碼 = "002";
                    cdb._項目代碼 = qdt.Rows[0]["問題類別"].ToString().Trim();
                    adt = cdb.GetList();

                    if (adt.Rows.Count > 0)
                    {
                        ccMail = adt.Rows[0]["項目名稱"].ToString().Trim();
                    }

                    cdb._群組代碼 = "005";
                    cdb._項目代碼 = qdt.Rows[0]["程度"].ToString().Trim();
                    dt = cdb.GetList();

                    if (dt.Rows.Count > 0)
                    {
                        rtype_v = dt.Rows[0]["項目名稱"].ToString().Trim();
                    }

                    string day_v = string.IsNullOrEmpty(day) ? "" : day.Substring(0, 4) + "/" + day.Substring(4, 2) + "/" + day.Substring(6, 2);

                    string Subject = "提問單回覆人員問題回覆通知";

                    mailContent = "系統通知:<br><br>線上提問單系統之問題已有回覆，請上 <a href = 'https://powersunba.com.tw/SunBa_Question/index.aspx'>線上提問單系統</a>";
                    mailContent += " 進行檢視並確認問題回覆之內容，以下為問題回覆之詳細資料，感謝您<br>";
                    mailContent += "編號: " + qdt.Rows[0]["編號"].ToString().Trim() + "<br/>問題類別:" + questionTypeName + "<br/>填表人: " + qdt.Rows[0]["填表人"].ToString().Trim() +
                        "<br/>提出日期: " + day_v + "<br/>急迫性: " + rtype_v + "<br/>問題描述: " + qdt.Rows[0]["內容"].ToString().Trim() + "<br/>回覆人" + fillformname + "<br/>回覆內容: " + Server.UrlDecode(nContent);

                    sdb._帳號 = dt.Rows[0]["員工編號"].ToString().Trim();
                    sdt = sdb.GetListByEmpid();

                    if (sdt.Rows.Count > 0)
                    {
                        mailTo = sdt.Rows[0]["EMAIL"].ToString().Trim();
                        send_mail.MailTo(mailTo, ccMail, Subject, mailContent);
                    }
                }
            }
            else
            {
                rdb._修改者 = LogInfo.empName;
                rdb._修改者id = LogInfo.empNo;
                rdb.UpdateData(oConn, myTrans);

                //qldb._類別 = "編輯";
                //qldb._儲存類別 = "填表人員";
                //qldb._填表人 = Server.UrlDecode(fillformname);
                //qldb._儲存內容 = Server.UrlDecode(orgnization) + Server.UrlDecode(day)
                //    + Server.UrlDecode(rtype) + Server.UrlDecode(nContent);
                //
                //qldb.InsertData(oConn, myTrans);
            }


            // 檔案上傳
            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "reply\\";
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
                    cdb._項目代碼 = "02";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        fdb._guid = tmpGuid;
                        fdb._檔案類型 = "02";

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

                    fdb._guid = guid;
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