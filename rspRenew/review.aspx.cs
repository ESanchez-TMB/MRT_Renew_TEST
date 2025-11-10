using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class review : System.Web.UI.Page
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
        protected void populateControls()
        {
            Dictionary<string, string> countys = (Dictionary<string, string>)Application["texasCounties"];
            Dictionary<string, string> reverse_countys = new Dictionary<string, string>();
            Dictionary<string, string> ethnicity = (Dictionary<string, string>)Application["ethnicity"];
            foreach (KeyValuePair<string, string> kvp in countys)
            {
                reverse_countys.Add(kvp.Value, kvp.Key);
            }
            nameLBL.Text = r.PAname;
            licNumLBL.Text = r.LicNum;
            if (r.LicenseExpirationDate == DateTime.MinValue) expirDateLBL.Text = "";
            else expirDateLBL.Text = r.LicenseExpirationDate.ToShortDateString();
            // do addresses
            //initState(State1);
            //initState(State2);
            madd1LBL.Text = r.MailingAddressLine1;
            madd2LBL.Text = r.MailingAddressLine2;
            mcityLBL.Text = r.MailingAddressCity;
            try
            {
                State1LBL.Text = r.MailingAddressState;
            }
            catch
            { //donothing 
            }
            mZipLBL.Text = r.MailingAddressZIP;
            try
            {
                cntryLBL.Text = r.MailingAddressCountry;
            }
            catch { }
            phoneLBL.Text = App_Code.Utilities.phoneDisplay(r.PhoneNum);
            //faxLBL.Text = App_Code.Utilities.phoneDisplay(r.FaxNum);
            emailLBL.Text = r.EMailAddr;


            // init practice           

            pAdd1.Text = r.PracticeAddressLine1;
            pAdd2.Text = r.PracticeAddressLine2;
            pCity.Text = r.PracticeAddressCity;
            try
            {
                State2LBL.Text = r.PracticeAddressState;
            }
            catch
            { //do nothing
            }
            pZip.Text = r.PracticeAddressZIP;
            try
            {
                pcntryLBL.Text = r.PracticeAddressCountry;
            }
            catch { }
            //pPhoneLBL.Text = App_Code.Utilities.phoneDisplay(r.PPhoneNum);
            //pFaxLBL.Text = App_Code.Utilities.phoneDisplay(r.PFaxNum);
            //pEmailLBL.Text = r.PEMailAddr;

            // do questions
            arrestLBL.Text = App_Code.Utilities.yesNo(r.arrested);
            cited_chargedLBL.Text = App_Code.Utilities.yesNo(r.cited_charged); // = cited_chargedRBL2.SelectedValue;
            crim_investigationsLBL.Text = App_Code.Utilities.yesNo(r.crim_investigation); // = crim_investigationRBL3.SelectedValue;
            convictedLBL.Text = App_Code.Utilities.yesNo(r.convicted); // = convictedRBL4.SelectedValue;
            investigationLBL.Text = App_Code.Utilities.yesNo(r.investigation); // = investigationRBL5.SelectedValue;
            //2020_06_01 question swap
            //depressedLBL.Text = App_Code.Utilities.yesNo(r.depressed); //  = depressedRBL6.SelectedValue;
            //alcoholLBL.Text = App_Code.Utilities.yesNo(r.alcohol); // = alcoholRBL7.SelectedValue;
            impairmentLBL.Text = App_Code.Utilities.yesNo(r.impairment); // = impairmentRBL8.SelectedValue;
            //sexualLBL.Text = App_Code.Utilities.yesNo(r.sexual); // = sexualRBL9.SelectedValue;
            //2020_06_01 question swap
            malpracticeLBL.Text = App_Code.Utilities.yesNo(r.malpractice); // = malpracticeRBL10.SelectedValue;
            cmeLBL.Text = App_Code.Utilities.yesNo(r.CME); // = cmeRBL11.SelectedValue;
            //
            Dictionary<string, string> practiceHours = (Dictionary<string, string>)Application["practiceHours"];
            try
            {
                practice_hoursLBL.Text = practiceHours[r.total_hrs];
            }
            catch
            {
                practice_hoursLBL.Text = "";
            }
            try
            {
                primary_prac_hoursLBL.Text = practiceHours[r.primary_hrs];
            }
            catch
            {
                primary_prac_hoursLBL.Text = "";
            }
            //practice_hoursLBL.Text = r.total_hrs; // = PRACTICE_HOURS.SelectedValue;
            //primary_prac_hoursLBL.Text = r.primary_hrs; // = PRIMARY_PRAC_HOURS.SelectedValue;
            //ppCountyLBL.Text = r.primCounty; // = ppCOUNTY.SelectedValue;
            try
            {
                ppCountyLBL.Text = reverse_countys[r.primCounty];
            }
            catch
            {
                ppCountyLBL.Text = "";
            }
            //
            Dictionary<string, string> practiceSettingDic = (Dictionary<string, string>)Application["practiceSetting"];
            try
            {
                ppPracticeInfoLBL.Text = practiceSettingDic[r.primSetting];
            }
            catch
            {
                ppPracticeInfoLBL.Text = "";
            }
            //ppPracticeInfoLBL.Text = r.primSetting; // = ppPracticeInfo.SelectedValue; //required
            ppNumber_in_groupLBL.Text = r.primGrpNbr; // = ppNumber_in_group.Text;
            //
            spZipLBL.Text = r.SecPracZip; // = spZip.Text;
            //spCountyLBL.Text = r.SecPracCounty; // = spCOUNTY.SelectedValue;
            try
            {
                spCountyLBL.Text = reverse_countys[r.SecPracCounty];
            }
            catch
            {
                spCountyLBL.Text = "";
            }
            try
            {
                spPracticeInfoLBL.Text = practiceSettingDic[r.SecPracSetting];
            }
            catch
            {
                spPracticeInfoLBL.Text = "";
            }
            //spPracticeInfoLBL.Text = r.SecPracSetting; // = spPracticeInfo.SelectedValue; //required
            spNumber_in_groupLBL.Text = r.SecPracGrpNbr; // = spNumber_in_group.Text;
            //ethnicityLBL.Text = r.Race; // = ethnicityDDL.SelectedValue;
            try
            {
                ethnicityLBL.Text = ethnicity[r.Race];
            }
            catch
            {
                ethnicityLBL.Text = "";
            }
            hispanic_originLBL.Text = App_Code.Utilities.yesNo(r.Hispanic_flg); // = hispanic_originRBL.SelectedValue;
            //highSchoolCountyLBL.Text  = r.TxHScounty; // = highSchoolCountyDDL.SelectedValue;
            try
            {
                highSchoolCountyLBL.Text = reverse_countys[r.TxHScounty];
            }
            catch
            {
                highSchoolCountyLBL.Text = "";
            }

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
            }
        }

        protected void goPayBTN_Click(object sender, EventArgs e)
        {
            //new code
            string error = App_Code.Utilities.edit(r);
            //if (error == "") Server.Transfer("disclaimer.aspx", false);
            if (error == "") Server.Transfer("payment.aspx", false);
            else
            {
                msgLBL.Text = error;
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
            }
        }
    }
}