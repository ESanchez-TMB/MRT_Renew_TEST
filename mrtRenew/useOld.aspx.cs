using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class useOld : System.Web.UI.Page
    {
        private AMS_LegacyOnlineInterface m_oAMS_LegacyOnlineInterface = null;
        RClassLibrary.renewal r;
        protected void Page_Load(object sender, EventArgs e)
        {
            r = (RClassLibrary.renewal)Session["renewal"];
            if (r == null) Server.Transfer("login.aspx", false);
            if (Page.IsPostBack)
                createXXX();
        }

        protected void usePrevious_Click(object sender, EventArgs e)
        {           
            RClassLibrary.renewal r2 = App_Code.Utilities.getWIPrenewal(r.LicNum);

            if (r2 != null)
            {
                if (r.status == "Success") //they have already paid
                {
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
                //
                Session["renewal"] = r2;
                Server.Transfer("addresses.aspx", false);
            }
        }

        protected void begin_Click(object sender, EventArgs e)
        {

            string license = r.LicNum;
            string ssn = r.SSN.Substring(5, 4);
            int idNum = 0;
            //todo: chuck's routine will go here
            if (m_oAMS_LegacyOnlineInterface == null)
                createXXX(); //second try 
            RClassLibrary.renewal r2 = null;
            try
            {
                r2 = m_oAMS_LegacyOnlineInterface.AMS_GetRenewalInfo(license, ssn);
            }
            catch (Exception exc)
            {
                App_Code.DataAccess.log("Error chuck: " + exc.Message, "lmpRenew.Login continue_Click()");
            }
            //
            idNum = r2.id_num;
            if (idNum == 0)
            {
                msgLBL.Text = "Your license number did not match our records. Please go to \"Login\" and try again.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            r2.LicType = "MR";          //17_10_17
            //
            Session["renewal"] = r2;
            Server.Transfer("addresses.aspx", false);

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
                App_Code.DataAccess.log("Error creating AMS_LegacyOnlineInterface: "+exc.Message.ToString()+"\n Source: "+exc.Source+"\n Method:"+exc.TargetSite, "mrtRenew.Login");
            }
        }

    }
}