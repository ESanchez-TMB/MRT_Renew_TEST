using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class addresses : System.Web.UI.Page
    {        
        //
        RClassLibrary.renewal r;
        string licenseType;
        protected void Page_Load(object sender, EventArgs e)
        {
            msgLBL.Text = "";
            msgLBL.ForeColor = System.Drawing.Color.Black;
            msgLBL.Visible = false;
            // should they be here
            r = (RClassLibrary.renewal)Session["renewal"];                    
            //     
            if (r == null) Server.Transfer("login.aspx", false);
            if (r.status == "Success") //they have already paid
            {
                Session["errMessage"] = "No changes are allowed after payment is made.";
                Server.Transfer("error.aspx", false);
            }
            //
            if (!Page.IsPostBack)
            {
                if (r != null)
                {
                    populateControls();
                }
                else
                {
                    msgLBL.Text = "Please Login again.";
                    msgLBL.ForeColor = System.Drawing.Color.Red;
                    msgLBL.Visible = true;
                }
            }

        }
        private void populateControls()
        {
            initState(State1);
            initState(State2);
            //
            cntryDDL.Items.Add(new ListItem("", "")); // put a blank there
            initCountries(cntryDDL); //watch out USA not in alpha order
            pcntryDDL.Items.Add(new ListItem("", "")); // put a blank there
            initCountries(pcntryDDL); //watch out USA not in alpha order
            if (r.MailingAddressCountry == "US")
                r.MailingAddressCountry = "USA";
            if (r.PracticeAddressCountry == "US")
                r.PracticeAddressCountry = "USA";
            try
            {
                cntryDDL.SelectedValue = r.MailingAddressCountry;
            }
            catch
            {
                cntryDDL.SelectedValue = "";
            }
            try
            {
                pcntryDDL.SelectedValue = r.PracticeAddressCountry;
            }
            catch
            {
                pcntryDDL.SelectedValue = "";
            }
            //
            madd1TB.Text = r.MailingAddressLine1;
            madd2TB.Text = r.MailingAddressLine2;
            mcityTB.Text = r.MailingAddressCity;
            try
            {
                State1.SelectedValue = r.MailingAddressState;
            }
            catch
            { //donothing 
            }
            mZipTB.Text = App_Code.Utilities.formatZIP(r.MailingAddressZIP);
            phoneLBL.Text = App_Code.Utilities.phoneDisplay(r.PhoneNum);
            emailLBL.Text = r.EMailAddr;
            //faxTB.Text = App_Code.Utilities.phoneDisplay(r.FaxNum);
            


            // init practice           

            pAdd1.Text = r.PracticeAddressLine1;
            pAdd2.Text = r.PracticeAddressLine2;
            pCity.Text = r.PracticeAddressCity;
            try
            {
                State2.SelectedValue = r.PracticeAddressState;
            }
            catch
            { //do nothing
            }
            pZip.Text = App_Code.Utilities.formatZIP(r.PracticeAddressZIP);
            //pPhoneTB.Text = App_Code.Utilities.phoneDisplay(r.PPhoneNum);
            //pFaxTB.Text = App_Code.Utilities.phoneDisplay(r.PFaxNum);

            //pEmailTB.Text = r.PEMailAddr;

        }
        protected void initState(DropDownList ddl)
        {
            if (ddl.Items.Count < 2)
            {
                Dictionary<string, string> states = (Dictionary<string, string>)Application["states"];
                ddl.Items.Add(new ListItem("", ""));
                foreach (KeyValuePair<string, string> kvp in states)
                {
                    ddl.Items.Add(new ListItem(kvp.Key, kvp.Value));
                }
                ddl.Items.Add(new ListItem("Other", "XX"));
            }
        }
        protected void initCountries(DropDownList ddl)
        {
            if (ddl.Items.Count < 2) // don't build twice, 2 is arbitrary more thn 2 countries
            {
                Dictionary<string, string> countries = App_Code.DataAccess.getCountries();
                ddl.Items.Add(new ListItem("UNITED STATES", "USA")); // ddl.Items.Insert(0, (new ListItem(kvp.Key, kvp.Value)));
                foreach (KeyValuePair<string, string> kvp in countries)
                {
                    ddl.Items.Add(new ListItem(kvp.Key, kvp.Value));
                }
            }
        }
        protected void continueBTN_Click(object sender, EventArgs e)
        {
            string error = edit();
            if (error == "")
            {
                updates();
                //App_Code.Utilities.putRenewXML(r);
                App_Code.Utilities.putWIPrenewal(r);
                Session["renewal"] = r;    //meaningless but doesn't hurt
                Server.Transfer("questions.aspx", false);
            }
            else
            {
                msgLBL.Text = error;
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
            }

        }
        protected string edit()
        {
            string error = "";
            if (madd1TB.Text.Trim() == "") error = error + "Please enter a valid mailing address line 1. <br/>";
            if (mcityTB.Text.Trim() == "") error = error + "Please enter a valid mailing city. <br/>";
            if (cntryDDL.SelectedValue == "") error = error + "Please enter a valid mail province/country.  <br/>";
            if (cntryDDL.SelectedValue == "USA")
            {
                if (State1.SelectedValue == "") error = error + "Please select a valid mailing state. <br/>"; //2_21
                if (!RClassLibrary.utilities.IsValidUSZip2(mZipTB.Text))
                    error = error + "Please enter a valid mailing zip code.<br/>";
            }
            //10_17_19
            //if (phoneTB.Text.Trim() != "")
            //{
            //    string phone = App_Code.Utilities.onlyNumbers(phoneTB.Text);
            //    if (!App_Code.Utilities.IsValidUSphone(phone))
            //        error = error + "Please correct the phone number. <br/>";
            //}
            //10_17_19

            //if (faxTB.Text.Trim() != "")
            //{
            //    string fax = App_Code.Utilities.onlyNumbers(faxTB.Text);
            //    if (!App_Code.Utilities.IsValidUSphone(fax))
            //        error = error + "Please correct the mailing fax number. <br/>";
            //}
            //if (emailTB.Text.Trim() != "")
            //{
            //    if (!App_Code.Utilities.IsValidEmail(emailTB.Text.Trim()))
            //        error = error + "Please correct the mailing email address. <br/>";
            //}

            //if (pAdd1.Text.Trim() == "") error = error + "Please enter a valid primary practice address 1. <br/>";
            if (pAdd1.Text.Trim() != "")
            {
                if (pCity.Text.Trim() == "") error = error + "Please enter a valid primary practice city <br/>";
                if (pcntryDDL.SelectedValue == "") error = error + "Please enter a valid primary practice province/country.  <br/>";
                if (pcntryDDL.SelectedValue == "USA")
                {
                    if (State2.SelectedValue == "") error = error + "Please enter a valid primary practice state. <br/>";
                    //if ((!App_Code.Utilities.IsValidUSZip(pZip.Text)) || (pZip.Text.Length != 5))
                    //    error = error + "Please enter a valid primary practice zip code. <br/>";
                    if (!RClassLibrary.utilities.IsValidUSZip2(pZip.Text))
                        error = error + "Please enter a valid primary practice zip code. <br/>";
                }
            }
            //if (pPhoneTB.Text.Trim() != "")
            //{
            //    string phone = App_Code.Utilities.onlyNumbers(pPhoneTB.Text);
            //    if (!App_Code.Utilities.IsValidUSphone(phone))
            //        error = error + "Please correct the practice phone number. <br/>";
            //}
            //if (pFaxTB.Text.Trim() != "")
            //{
            //    string fax = App_Code.Utilities.onlyNumbers(pFaxTB.Text);
            //    if (!App_Code.Utilities.IsValidUSphone(fax))
            //        error = error + "Please correct the practice fax number. <br/>";
            //}
            //if (pEmailTB.Text.Trim() != "")
            //{
            //    if (!App_Code.Utilities.IsValidEmail(pEmailTB.Text.Trim()))
            //        error = error + "Please correct the practice email address. <br/>";
            //}
            return error;
        }

        protected void updates()
        {
            r.MailingAddressLine1 = madd1TB.Text.ToUpper().Trim();
            r.MailingAddressLine2 = madd2TB.Text.ToUpper().Trim();
            r.MailingAddressCity = mcityTB.Text.ToUpper().Trim();
            r.MailingAddressState = State1.SelectedValue;
            r.MailingAddressZIP = App_Code.Utilities.onlyNumbers(mZipTB.Text);

            r.MailingAddressCountry = cntryDDL.SelectedValue;
            //10_17_19
            //r.PhoneNum = App_Code.Utilities.onlyNumbers(phoneTB.Text.Trim());
            //r.EMailAddr = emailTB.Text.Trim();
            //10_17_19

            //r.FaxNum = App_Code.Utilities.onlyNumbers(faxTB.Text.Trim());
           
            //
            r.PracticeAddressLine1 = pAdd1.Text.ToUpper().Trim();
            r.PracticeAddressLine2 = pAdd2.Text.ToUpper().Trim();
            r.PracticeAddressCity = pCity.Text.ToUpper().Trim();
            r.PracticeAddressState = State2.SelectedValue;
            r.PracticeAddressZIP = App_Code.Utilities.onlyNumbers(pZip.Text);

            r.PracticeAddressCountry = pcntryDDL.SelectedValue;
            //r.PPhoneNum = App_Code.Utilities.onlyNumbers(pPhoneTB.Text.Trim());
            //r.PFaxNum = App_Code.Utilities.onlyNumbers(pFaxTB.Text.Trim());
            //r.PEMailAddr = pEmailTB.Text.Trim();
        }

        //
    }
}