using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_DormitoryCancelRegister : System.Web.UI.Page
{
	public string empName = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		empName = LogInfo.empName;
	}
}