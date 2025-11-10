using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class payment : System.Web.UI.Page
    {
        RClassLibrary.renewal r;
        string licenseType;
        protected void Page_Load(object sender, EventArgs e)
        {
            r = (RClassLibrary.renewal)Session["renewal"];
            //
            if (r == null) Server.Transfer("login.aspx", false);
            if (r.status == "Success") //they have already paid
            {
                Session["errMessage"] = "No changes are allowed after payment is made.";
                Server.Transfer("error.aspx", false);
            }
            //
        }

        protected void paymentRB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bla bla
            if (paymentRB.SelectedValue == "K")
            {
                r.methodOfPayment = "ACH";
            } //then check
            else
            {
                r.methodOfPayment = "CC";
            } //credit card 
            Session["renewal"] = r;  //un-necessary
            App_Code.Utilities.putWIPrenewal(r);
            //Server.Transfer("disclaimer.aspx", false);
            //Server.Transfer("disclaimer2.aspx", false);
            Server.Transfer("disclaimer4.aspx", false);
        }
    }
}