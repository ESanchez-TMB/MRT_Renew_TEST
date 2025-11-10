using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = (string)Session["errMessage"];
            if ((msg != null) && (msg.Length > 0))
            {

                msgLBL.Text = msg;
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
            }
        }
    }
}