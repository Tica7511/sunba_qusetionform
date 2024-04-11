using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class handler_AddDocCategory : System.Web.UI.Page
{
    DocCategory_DB db = new DocCategory_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 文件分類/階層管理
        ///說    明:
        /// * Request["id"]: ID
        /// * Request["dc_name"]: 程序書分類名稱
        /// * Request["dc_sort"]: 新增排序值
        /// * Request["updateSort"]: 修改排序值
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            #region Check Session Timeout
            if (!LogInfo.isLogin)
            {
                throw new Exception("登入帳號已失效，請重新登入");
            }
            #endregion

            DataTable dt = db.GetSortValue();
            string defaultSortValue = (db.GetSortValue().Select().Length == 0) ? "1" : (int.Parse(dt.Rows[0]["dc_sort"].ToString().Trim()) + 1).ToString().Trim(); // 當前顯示排序最大值 +1

            string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Common.FilterCheckMarxString(Request["id"].ToString().Trim());
            string dc_name = (string.IsNullOrEmpty(Request["dc_name"])) ? "" : Common.FilterCheckMarxString(Request["dc_name"].ToString().Trim());
            string dc_sort = (string.IsNullOrEmpty(Request["dc_sort"])) ? defaultSortValue : Common.FilterCheckMarxString(Request["dc_sort"].ToString().Trim());

            string updateSort = (string.IsNullOrEmpty(Request["updateSort"])) ? "" : Common.FilterCheckMarxString(Request["updateSort"].ToString().Trim());
            string[] guid_sort = updateSort.Split(',');
            guid_sort = guid_sort.Take(guid_sort.Count() - 1).ToArray();

            db._dc_name = dc_name;
            db._dc_sort = dc_sort;
            db._dc_createid = LogInfo.empNo;
            db._dc_createname = LogInfo.empName;
            db._dc_modid = LogInfo.empNo;
            db._dc_modname = LogInfo.empName;

            string xmlstr = string.Empty;

            if (dt.Select("dc_sort='" + dc_sort + "'").Length > 0 || db.GetList().Select("convert(dc_id,'System.String')<>'" + id + "' and dc_name='" + dc_name + "'").Length > 0)
            {
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Repeated>已存在相同內容</Repeated></root>";
            }
            else
            {
                if (id == "")
                {
                    db._dc_guid = Guid.NewGuid().ToString("N");
                    db.addDocCategory();
                }
                else if (id == "sort")
                {
                    string[] temp = new string[2];
                    foreach (var item in guid_sort)
                    {
                        temp = item.Split(':');
                        db._dc_guid = temp[0];
                        db._dc_sort = temp[1];
                        db.UpdateSortValue();
                    }
                }
                else
                {
                    db._dc_id = id;
                    db.UpdateDocCategory();
                }
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>完成</Response></root>";
            }

            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}