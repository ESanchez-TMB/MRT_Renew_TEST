using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class duplicate : System.Web.UI.Page
    {


        RClassLibrary.renewal app;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                app = (RClassLibrary.renewal)Session["application"];
                if ((app != null) && (app.status.ToUpper().Trim() == "SUCCESS"))
                {
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
            }
            catch
            {
                Session["errMessage"] = "Duplicate page, improper LMP application.";
                Server.Transfer("error.aspx", false);
            }
            //
            if ((app != null) && (app.status.ToUpper().Trim() == "SUCCESS"))
            {
                Session["errMessage"] = "No changes are allowed after payment is made.";
                Server.Transfer("error.aspx", false);
            }

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
                    RClassLibrary.DataAccess.audit(orderID, "NIC duplicate", 6);
                    RClassLibrary.DataAccess.updateOrderStatus(orderID, 6, "mrtRnl");
                    if (app != null)
                    {
                        app.status = "duplicate";
                        string answer = App_Code.Utilities.putWIPrenewal(app); //maybe this shoul have been dataAccess
                    }
                }
                catch
                {
                    App_Code.DataAccess.log("duplicate.aspx: could not chg status of order. orderID=" + ORDERID, "duplicate.Page_Load");
                    Session["errMessage"] = "Transaction is canceled but status of duplicate as not updated.";
                    Server.Transfer("error.aspx", false);
                }
            }
            else
            {
                Session["errMessage"] = "Illegal navigation to duplicate page.";
                Server.Transfer("error.aspx", false);
            }
            Server.Transfer("duplicate2.aspx");

        }

    }
}