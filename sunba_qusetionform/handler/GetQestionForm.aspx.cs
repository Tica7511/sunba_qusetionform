using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetQestionForm : System.Web.UI.Page
{
    QuestionForm_DB db = new QuestionForm_DB();
    Reply_DB rdb = new Reply_DB();
    Competence_DB cdb = new Competence_DB();
    FileTable_DB fdb = new FileTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢提問表單
        ///說    明:
        /// * Request["num"]: 編號
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
        /// * Request["questionType"]: 問題類別
        /// * Request["empid]: 員工編號
        /// * Request["orgnization]: 部門
        /// * Request["startday]: 提出日期 起
        /// * Request["endday]: 提出日期 迄
        /// * Request["state]: 目前狀態
        /// * Request["content]: 內容
        /// * Request["OrderBy"]:排序欄位
		/// * Request["SortBy"]:排序升降
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtnew = new DataTable();
        try
        {
            string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Common.FilterCheckMarxString(Request["PageNo"].ToString().Trim());
            int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 10 : int.Parse(Common.FilterCheckMarxString(Request["PageSize"].ToString().Trim()));
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
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
            string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Common.FilterCheckMarxString(Request["SortName"].ToString().Trim());
            string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "" : Common.FilterCheckMarxString(Request["SortMethod"].ToString().Trim());

            if (type == "list")
            {
                //計算起始與結束
                int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
                int pageStart = pageEnd - PageSize + 1;

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
                db._排序名稱 = SortName;
                db._排序狀態 = SortMethod;

                ds = db.GetList(pageStart.ToString(), pageEnd.ToString(), startday, endday);
                DataTable qdt = ds.Tables[1];

                if (qdt.Rows.Count > 0)
                {
                    qdt.Columns.Add("回覆內容R", typeof(string));
                    for (int i = 0; i < qdt.Rows.Count; i++)
                    {
                        rdb._guid = qdt.Rows[i]["guid"].ToString().Trim();
                        DataTable rdt = rdb.GetData();

                        if (rdt.Rows.Count > 0)
                        {
                            qdt.Rows[i]["回覆內容R"] = rdt.Rows[0]["回覆內容"].ToString().Trim();
                        }
                    }
                }

                // 管理者權限
                string Manager = string.Empty;
                if (GetCompetence("sa"))
                    Manager = "<IsManager>Y</IsManager>";
                else
                    Manager = "<IsManager>N</IsManager>";

                string xmlstr = string.Empty;
                string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + Manager + totalxml + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else if (type == "data")
            {
                db._guid = guid;

                dt = db.GetData();

                DataTable file_dt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    fdb._guid = dt.Rows[0]["guid"].ToString();
                    file_dt = DataEncode(fdb.GetList());
                }

                string xmlstr = string.Empty;
                string xmlstr2 = string.Empty;
                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(file_dt, "fileList", "file_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "<datacount>" + dt.Rows.Count.ToString() + "</datacount></root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                string nMonth = string.Empty;
                string sn = string.Empty;

                if (DateTime.Now.Month.ToString().Length == 1)
                {
                    nMonth = "0" + DateTime.Now.Month.ToString();
                }
                else
                {
                    nMonth = DateTime.Now.Month.ToString();
                }

                db._年度 = DateTime.Now.Year.ToString();
                db._月份 = nMonth;

                DataTable sndb = db.GetSn();

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

                string xmlstr = string.Empty;
                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><sn>" + DateTime.Now.Year.ToString() + "-" + nMonth + "-" + sn + "</sn></root>";
                xDoc.LoadXml(xmlstr);
            }            
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }

    private bool GetCompetence(string type)
    {
        bool status = false;
        cdb._類別 = type;
        DataTable dt = cdb.GetCompetenceList_Common();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (LogInfo.empNo == dt.Rows[i]["value"].ToString().Trim())
                    status = true;
            }
        }
        return status;
    }

    private DataTable DataEncode(DataTable dt)
    {
        dt.Columns.Add("EncodeGuid", typeof(string));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["EncodeGuid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["guid"].ToString()));
            }
        }
        return dt;
    }
}