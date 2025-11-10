using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class Confirm : System.Web.UI.Page
    {
        RClassLibrary.renewal r;
        string licenseType;
        protected void Page_Load(object sender, EventArgs e)
        {
            r = (RClassLibrary.renewal)Session["renewal"]; //Session["renewal"] = r; 
            if (r == null) Server.Transfer("login.aspx", false);
            if (r.status == "Success") //they have already paid
            {
                Session["errMessage"] = "No changes are allowed after payment is made.";
                Server.Transfer("error.aspx", false);
            }
            //
            feePanel.Visible = false; //DQ
            if (!Page.IsPostBack)
            {
                populateControls();
            }
        }
        //
        private void populateControls()
        {
            
            name1LBL.Text = r.PAname; // App_Code.Utilities.nameFormat1(r.PAName);
            name2LBL.Text = r.PAname; //App_Code .Utilities .nameFormat1 ( r.DocName);
            nameLBL3.Text = r.PAname; //App_Code .Utilities .nameFormat1 ( r.DocName);
            //feeLBL .Text = r.CURRFEE.ToString ();
            feeLBL.Text = String.Format("{0:c}", (r.CURRFEE));
            licenseLBL.Text = r.LicNum;
            //
            if ((r.lateFee > 0) || (r.backFee > 0))                      //DQ
            {                                                            //DQ
                feePanel.Visible = true;                                 //DQ
                lateFeeLBL.Text = String.Format("{0:c}", (r.lateFee));   //DQ chg 2_29_16
                backFeeLBL.Text = String.Format("{0:c}", (r.backFee));   //DQ chg 2_29_16
            }
        }

        protected void continue_Click(object sender, EventArgs e)
        {
            //RClassLibrary.renewal r2 = App_Code.Utilities.getRenewXML(r.LicNum + r.SSN);
            RClassLibrary.renewal r2 = App_Code.Utilities.getWIPrenewal(r.LicNum);

            if (r2 != null)
            {
                //10_18_11
                if (r2.status == "Success") //they have already paid
                {
                    r = null;
                    Session["renewal"] = null;
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
                //10_18_11
                Server.Transfer("useOld.aspx", false);
            }
            Server.Transfer("addresses.aspx", false);
        }

        protected void return_Click(object sender, EventArgs e)
        {
            Server.Transfer("Login.aspx", false);
        }

    }
}