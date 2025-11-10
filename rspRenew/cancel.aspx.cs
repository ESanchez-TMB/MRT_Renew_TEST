using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class cancel : System.Web.UI.Page
    {

        RClassLibrary.renewal app;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                app = (RClassLibrary.renewal)Session["renewal"];
                if ((app != null) && (app.status.ToUpper().Trim() == "SUCCESS"))
                {
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
            }
            catch
            {
                Session["errMessage"] = "Cancel page, improper PRF application.";
                Server.Transfer("error.aspx", false);
            }
            //
            if ((!IsCallback) && (Page.PreviousPage == null) && (!IsPostBack))
            {
                string ORDERID = "";
                try
                {
                    //need to get the token
                    string token = Request["token"];
                    // the token is useless unless you access
                    // public static void updatePPwithResult(string token,string error_msg,string orderID)
                    ORDERID = (string)Session["orderID"];
                    int orderID = int.Parse(ORDERID);
                    //App_Code.DataAccess.audit(orderID, "NIC cancel", 4);
                    //App_Code.DataAccess.updateOrderStatus(orderID, 4);
                    RClassLibrary.DataAccess.audit(orderID, "NIC cancel", 4);
                    RClassLibrary.DataAccess.updateOrderStatus(orderID, 4, "mrtRnl");
                    if (app != null)
                    {
                        app.status = "cancel";
                        //App_Code.DataAccess.putApplDBwip(app);
                        //RClassLibrary.DataAccess.putApplDBwip(app);
                        App_Code.Utilities.putWIPrenewal(app);
                    }
                }
                catch
                {
                    App_Code.DataAccess.log("Cancel.aspx: could not chg status of order. orderID=" + ORDERID, "cancel.Page_Load");
                }
            }
            else
            {
                Session["errMessage"] = "Illegal navigation to cancel page.";
                Server.Transfer("error.aspx", false);
            }
            Server.Transfer("Cancel2.aspx");
        }
    }
}