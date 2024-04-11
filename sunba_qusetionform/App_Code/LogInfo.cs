using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// LogInfo 的摘要描述
/// </summary>
public class LogInfo
{
	#region 會員系統
	/// <summary>
	/// 是否登入
	/// </summary>
	public static bool isLogin
	{
		get
		{
			return (HttpContext.Current.Session["登入工號"] != null) ?
				(!string.IsNullOrEmpty(HttpContext.Current.Session["登入工號"].ToString())) ? true : false : false;
		}
	}

	/// <summary>
	/// 登入者工號
	/// </summary>
	public static string empNo
	{
        get
        {
            return (HttpContext.Current.Session["登入工號"] != null) ?
                 (!string.IsNullOrEmpty(HttpContext.Current.Session["登入工號"].ToString())) ? HttpContext.Current.Session["登入工號"].ToString() : "" : "";
        }
        set
        {
            HttpContext.Current.Session["登入工號"] = value;
        }
    }

	/// <summary>
	/// 登入者姓名
	/// </summary>
	public static string empName
	{
        get
        {
            return (HttpContext.Current.Session["登入姓名"] != null) ?
                 (!string.IsNullOrEmpty(HttpContext.Current.Session["登入姓名"].ToString())) ? HttpContext.Current.Session["登入姓名"].ToString() : "" : "";
        }
        set
        {
            HttpContext.Current.Session["登入姓名"] = value;
        }
    }


	/// <summary>
	/// 登入者所屬單位
	/// </summary>
	public static string deptName
	{
		get
		{
			return (HttpContext.Current.Session["affairs_dept_name"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["affairs_dept_name"].ToString())) ? HttpContext.Current.Session["affairs_dept_name"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["affairs_dept_name"] = value;
		}
	}

	/// <summary>
	/// 登入者所屬單位代碼
	/// </summary>
	public static string deptCode
	{
        get
        {
            return (HttpContext.Current.Session["affairs_dept_code"] != null) ?
                 (!string.IsNullOrEmpty(HttpContext.Current.Session["affairs_dept_code"].ToString())) ? HttpContext.Current.Session["affairs_dept_code"].ToString() : "" : "";
        }
        set
        {
            HttpContext.Current.Session["affairs_dept_code"] = value;
        }
    }

    /// <summary>
    /// 身份/權限。
    /// </summary>
    public static string competence
    {
        get
        {
            return (HttpContext.Current.Session["affairs_competence"] != null) ?
                 (!string.IsNullOrEmpty(HttpContext.Current.Session["affairs_competence"].ToString())) ? HttpContext.Current.Session["affairs_competence"].ToString() : "" : "";
        }
        set
        {
            HttpContext.Current.Session["affairs_competence"] = value;
        }
    }
    #endregion
}