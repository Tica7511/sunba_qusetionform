using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class handler_GetMealsRegister : System.Web.UI.Page
{
	MealsRegister_DB db = new MealsRegister_DB();
	MealsLocation_DB ml_db = new MealsLocation_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 用餐登記清單
		///說明:
		/// * Request["category"]:用餐登記類別  01: 員工 02: 廠商 03: 愛心便當
		/// * Request["RegisterId"]:登記者 id
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Common.FilterCheckMarxString(Request["SearchStr"].ToString().Trim());
			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
			string RegisterId = (string.IsNullOrEmpty(Request["RegisterId"])) ? "" : Common.FilterCheckMarxString(Request["RegisterId"].ToString().Trim());

			string PersonID = (category == "01") ? LogInfo.empNo : Common.Decrypt(RegisterId);

			#region 建立日期區間資料
			DateTime today = DateTime.Now;
			int thisYear = today.Year;
			int thisMonth = today.Month;
			int daysInMonth = DateTime.DaysInMonth(thisYear, thisMonth);
			DateTime Range1st = new DateTime(thisYear, thisMonth, 1);
			DateTime RangeStartDate = new DateTime(thisYear, thisMonth, 25);
			DateTime RangeEndDate = new DateTime(thisYear, thisMonth, daysInMonth);
			
			int dayNo = today.Day;
			if (dayNo >= 25)
			{
				Range1st = new DateTime(thisYear, thisMonth, 1).AddMonths(1);
				daysInMonth = DateTime.DaysInMonth(Range1st.Year, Range1st.Month);
				RangeEndDate = new DateTime(Range1st.Year, Range1st.Month, daysInMonth);
			}
			else
			{
				DateTime lastMonth = new DateTime(thisYear, thisMonth, 1).AddDays(-1);
				RangeStartDate = new DateTime(lastMonth.Year, lastMonth.Month, 25);
			}

			db._mr_type = category;
			db._mr_person_id = PersonID;
			DataTable dt = db.GetMealsList(RangeStartDate.ToString("yyyy-MM-dd"), RangeEndDate.ToString("yyyy-MM-dd"));
			DataView dv = dt.DefaultView;
			
			db._mr_date = RangeEndDate.ToString("yyyy-MM-dd");
			DataTable CheckDt = db.CheckThisRange();
			if (CheckDt.Rows.Count == 0)
			{
				for (DateTime idate = RangeStartDate; idate <= RangeEndDate; idate = idate.AddDays(1))
				{
					dv.RowFilter = "mr_date='" + idate.ToString("yyyy-MM-dd") + "' ";
					if (dv.Count == 0)
					{
						DataRow newRow = dt.NewRow();
						newRow["mr_date"] = idate.ToString("yyyy-MM-dd");
						newRow["mr_lunch_num"] = 0;
						newRow["mr_dinner_num"] = 0;
						dt.Rows.Add(newRow);
					}
				}
			}
			#endregion

			DataTable ml_dt = ml_db.GetSelectList();

			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			string dateStart = "<dateStart>" + RangeStartDate.ToString("yyyy-MM-dd") + "</dateStart>";
			string dateEnd = "<dateEnd>" + RangeEndDate.ToString("yyyy-MM-dd") + "</dateEnd>";
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(ml_dt, "placeList", "place_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + dateStart + dateEnd + xmlstr + xmlstr2 + "</root>";
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