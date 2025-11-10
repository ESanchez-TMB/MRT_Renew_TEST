using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class Login : System.Web.UI.Page
    {
        //
        private AMS_LegacyOnlineInterface m_oAMS_LegacyOnlineInterface = null;
        private string loginFailure;
        //private RClassLibrary.renewalPA r;
        private RClassLibrary.renewal r;
        protected void Page_Load(object sender, EventArgs e)
        {
            errMsg.Visible = false;
            errMsg.Text = "";
            errMsg.ForeColor = System.Drawing.Color.Black;

            loginFailure = "<p style=\"color:Red;\"> Our records indicate that you are not eligible to register through the online system at this time.  Reasons someone may not be able to register include but are not limited to:</p>" +
            "<ul style=\"color:Red;\"> <li>you are trying to register too early, </li>" +
            "<li>you are past your expiration date for registration,</li>" +
            "<li>your previous registration was incomplete, or</li>" +
            "<li>your fingerprint background check results have not been received at the TMB.</li>" +
            "</ul>" +
            "<p style=\"color:Red;\">If you believe this is in error or to update your information with the Board, please contact Consumer and Application Resources at 512-305-7030 or email us at Registrations@tmb.state.tx.us.</p>";

            //if (!Request.IsSecureConnection && !Page.IsPostBack)  //send user to SSL 
            //{
            //    string serverName = HttpUtility.UrlEncode(Request.ServerVariables["SERVER_NAME"]);
            //    string filePath = Request.FilePath;
            //    Response.Redirect("https://" + serverName + filePath);
            //}

            devPanel.Visible = false;
            //
            string mode = App_Code.Rconfiguration.NICMode;
            if (mode == "DEV" || mode == "TEST")
            {
                devPanel.Visible = true;
            }
            //
            createXXX();
        }

        protected void continue_Click(object sender, EventArgs e)
        {
            
            string ssn = SOCIAL_SECURITY_NUMBER.Text.Trim();
            string license = LICENSE_NUMBER.Text.ToUpper().Trim();
            //
            if ((ssn.Trim() == "") || (license.Trim() == ""))
            {
                errMsg.Text = "Please enter both a license number and the last four digits of your SSN.";
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Visible = true;
                return;
            }
            
            //                     
            if ((ssn.Length > 4) || (ssn.Length < 4))
            {
                errMsg.Text = "Please enter exactly four numbers for the last four digits of your SSN.";
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Visible = true;
                return;
            }
            if (!App_Code.Utilities.isNumeric(ssn))
            {
                errMsg.Text = "The last four digits of your SSN must be numeric.";
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Visible = true;
                return;
            }
            //todo: chuck's routine will go here
            // r = App_Code .Utilities .createNewMRTrenew();
             //
             //r = App_Code.DataAccess.getRrecord(license, ssn);
            if (m_oAMS_LegacyOnlineInterface == null)
                createXXX(); //second try 
            RClassLibrary.renewal r = null;
            try
            {
                r = m_oAMS_LegacyOnlineInterface.AMS_GetRenewalInfo(license, ssn);
            }
            catch (Exception exc)
            {
                string mode = System.Configuration.ConfigurationManager.AppSettings["NICMode"];
                App_Code.DataAccess.log(string.Format("Error chuck-m_oAMS_LegacyOnlineInterface.AMS_GetRenewalInfo({0}, {1}): mode {2} ", license, ssn, mode) + " " + exc.Message, "mrtregRenew.Login continue_Click()");
                //App_Code.DataAccess.log("Error chuck: " + exc.Message, "mrtRenew.Login continue_Click()");
            }
            //
             if (r == null)
             {
                 App_Code.DataAccess.log(license + ": renewal record is null", "login.aspx,continue_Click");
                 errMsg.Text = loginFailure;
                 errMsg.ForeColor = System.Drawing.Color.Red;
                 errMsg.Visible = true;
                 return;
             }
             if ( (r.LicNum.Substring(0, 2) != "GM") && (r.LicNum.Substring(0, 2) != "LM") )
             {
                 errMsg.Text = "This website is for Medical Radiologic Technologist renewal, not Non-Certified Rad Tech - Registry renewal.";
                 errMsg.ForeColor = System.Drawing.Color.Red;
                 errMsg.Visible = true;
                 return;
             }
            //
               
                RClassLibrary.renewal r2 = App_Code.Utilities.getWIPrenewal(r.LicNum);
                if ((r2 != null))
                {
                    if (r2.status == "Success") //they have already paid
                    {
                        r = null;
                        Session["renewal"] = null;
                        Session["errMessage"] = "No changes are allowed after payment is made.";
                        Server.Transfer("error.aspx", false);
                    }
                }
                //continue
                r.LicType = "MR";


                Session["renewal"] = r;
                string error = "";
                // there is no purpose to this code - chuck makes this detemination
                //error = App_Code.Utilities.isEligible2renew (r);  //auto pass for test
                //              
                if (error != "")
                {
                    App_Code.DataAccess.log(license + ": " + error, "login.aspx,continue_Click");
                    //errMsg.Text = "We have encountered the following problem. " + error;
                    errMsg.Text = loginFailure;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Visible = true;
                    Session["renewal"] = null;
                    return; //un-necessary
                }
                else
                {                    
                    App_Code.DataAccess.log(license + ": " + "Sign-on", "login.aspx,continue_Click");
                    Server.Transfer("Confirm.aspx", false);
                }             
        }

        protected void reset_Click(object sender, EventArgs e)
        {
            SOCIAL_SECURITY_NUMBER.Text = "";
            LICENSE_NUMBER.Text = "";
        }
        //
        protected void createXXX()
        {
            try
            {
                string mode = System.Configuration.ConfigurationManager.AppSettings["NICMode"];
                if (mode == "DEV") mode = "TEST";     
                
                m_oAMS_LegacyOnlineInterface = new AMS_LegacyOnlineInterface(mode);

            }
            catch (Exception exc)
            {
                App_Code.DataAccess.log("Error Login: " + exc.Message.ToString()+"\n Source: "+exc.Source+"\n Method:"+exc.TargetSite, "mrtRenew.Login");
                 
      
	        }

         }
      

    }
}