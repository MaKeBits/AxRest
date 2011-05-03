using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Chelab.AX2009.WebApp.Test
{
    public partial class _Default : System.Web.UI.Page
    {
        Labx.LabXSoapClient _Labx_WS = new Chelab.AX2009.WebApp.Test.Labx.LabXSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = _Labx_WS.GetTable();
            if (dt != null)
            {
                grv_01.DataSource = dt;
                grv_01.DataSourceID = "";
            }
            grv_01.DataBind();
        }

        protected void grv_01_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv_01.PageIndex = e.NewPageIndex;
            grv_01.DataBind();
        }
    }
}
