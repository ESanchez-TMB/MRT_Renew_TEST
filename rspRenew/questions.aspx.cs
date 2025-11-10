using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class questions : System.Web.UI.Page
    {
        //        
        RClassLibrary.renewal r;
        string licenseType;
        protected void Page_Load(object sender, EventArgs e)
        {
            msgLBL.Text = "";
            msgLBL.ForeColor = System.Drawing.Color.Black;
            msgLBL.Visible = false;           
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
                populateControls();
            }

        }
        private void populateControls()
        {
            
            initCounty(ppCOUNTY);
            initCounty(spCOUNTY);
            initCounty(highSchoolCountyDDL);
            initEthnicity();
            //
            if ((r.arrested == "Y") || (r.arrested == "N")) arrestRBL1.SelectedValue = r.arrested;
            if ((r.cited_charged == "Y") || (r.cited_charged == "N")) cited_chargedRBL2.SelectedValue = r.cited_charged;
            if ((r.crim_investigation == "Y") || (r.crim_investigation == "N")) crim_investigationRBL3.SelectedValue = r.crim_investigation;
            if ((r.convicted == "Y") || (r.convicted == "N")) convictedRBL4.SelectedValue = r.convicted;
            if ((r.investigation == "Y") || (r.investigation == "N")) investigationRBL5.SelectedValue = r.investigation;
            //2020_06_01 question swap
            //if ((r.depressed == "Y") || (r.depressed == "N")) depressedRBL6.SelectedValue = r.depressed;
            //if ((r.alcohol == "Y") || (r.alcohol == "N")) alcoholRBL7.SelectedValue = r.alcohol;
            if ((r.impairment == "Y") || (r.impairment == "N")) impairmentRBL6.SelectedValue = r.impairment;
            //if ((r.sexual == "Y") || (r.sexual == "N")) sexualRBL9.SelectedValue = r.sexual;
            //2020_06_01 question swap
            if ((r.malpractice == "Y") || (r.malpractice == "N")) malpracticeRBL10.SelectedValue = r.malpractice;
            if (r.CME == "Y") cmeRBL11.SelectedValue = r.CME;
            //
            if (r.total_hrs != "") PRACTICE_HOURS.SelectedValue = r.total_hrs;
            if (r.primary_hrs != "") PRIMARY_PRAC_HOURS.SelectedValue = r.primary_hrs;
            if (r.primCounty != "") ppCOUNTY.SelectedValue = r.primCounty;
            if (r.primSetting != "") ppPracticeInfo.SelectedValue = r.primSetting;
            if (r.primGrpNbr != "") ppNumber_in_group.Text = r.primGrpNbr;
            //
            if (r.SecPracZip != "") spZip.Text = r.SecPracZip;
            if (r.SecPracCounty != "") spCOUNTY.SelectedValue = r.SecPracCounty;
            if (r.SecPracSetting != "") spPracticeInfo.SelectedValue = r.SecPracSetting;
            if (r.SecPracGrpNbr != "") spNumber_in_group.Text = r.SecPracGrpNbr;
            //
            try
            {
                if (r.Race != "") ethnicityDDL.SelectedValue = r.Race;
            }
            catch { }
            try
            {
                if (r.TxHScounty != "") highSchoolCountyDDL.SelectedValue = r.TxHScounty; // no guarantee we have data or that is valid
            }
            catch { }
            try
            {
                if (r.Hispanic_flg != "") hispanic_originRBL.SelectedValue = r.Hispanic_flg;
            }
            catch { }


        }
        protected void initCounty(DropDownList ddl)
        {
            if (ddl.Items.Count < 2)
            {
                Dictionary<string, string> states = (Dictionary<string, string>)Application["texasCounties"];
                ddl.Items.Add(new ListItem("", ""));
                foreach (KeyValuePair<string, string> kvp in states)
                {
                    ddl.Items.Add(new ListItem(kvp.Key, kvp.Value));
                }
            }
        }
        private void initEthnicity()
        {
            Dictionary<string, string> ethnicity = (Dictionary<string, string>)Application["ethnicity"];
            ethnicityDDL.Items.Add(new ListItem("Please select your race.", ""));
            foreach (KeyValuePair<string, string> kvp in ethnicity)
            {
                ethnicityDDL.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }

        protected void continue_Click(object sender, EventArgs e)
        {
            string error = edit();
            if (error == "")
            {
                update();
                //App_Code.Utilities.putRenewXML(r);
                App_Code.Utilities.putWIPrenewal(r);
                Session["renewal"] = r;
                //
                Server.Transfer("review.aspx", false);
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
            if (arrestRBL1.SelectedIndex == -1)
                error = error + "You must answer Question 1. <br/>";
            if (cited_chargedRBL2.SelectedIndex == -1)
                error = error + "You must answer Question 2. <br/>";
            if (crim_investigationRBL3.SelectedIndex == -1)
                error = error + "You must answer Question 3. <br/>";
            if (convictedRBL4.SelectedIndex == -1)
                error = error + "You must answer Question 4. <br/>";
            if (investigationRBL5.SelectedIndex == -1)
                error = error + "You must answer Question 5. <br/>";
            //2020_06_01 question swap
            //if (depressedRBL6.SelectedIndex == -1)
            //    error = error + "You must answer Question 6. <br/>";
            //if (alcoholRBL7.SelectedIndex == -1)
            //    error = error + "You must answer Question 7. <br/>";
            if (impairmentRBL6.SelectedIndex == -1)
                error = error + "You must answer Question 6. <br/>";
            //if (sexualRBL9.SelectedIndex == -1)
            //    error = error + "You must answer Question 9. <br/>";
            //2020_06_01 question swap
            if (malpracticeRBL10.SelectedIndex == -1)
                error = error + "You must answer Question 10. <br/>";
            //if (ppPracticeInfo .SelectedValue = "0") "You must specify a
            if (ppPracticeInfo.SelectedValue == "7")
            {
                if ((ppNumber_in_group.Text == "0") || (ppNumber_in_group.Text == "1") || (ppNumber_in_group.Text == "") || (!App_Code.Utilities.IsDigits(ppNumber_in_group.Text)))
                {
                    error = error + "If you are in a primary practice partnership/group practice, you must specify a number in your group.<br/>";
                }
            }
            else
            {
                if (ppNumber_in_group.Text.Trim() != "") error = error + "If you are not in a primary practice partnership/group, you must leave the number-in-group field blank.<br/>";
            }

            if (spPracticeInfo.SelectedValue == "7")
            {
                if ((spNumber_in_group.Text == "0") || (spNumber_in_group.Text == "1") || (spNumber_in_group.Text == "") || (!App_Code.Utilities.IsDigits(spNumber_in_group.Text)))
                {
                    error = error + "If you are in a secondary practice partnership/group practice, you must specify a number in your group.<br/>";
                }
            }
            else
            {
                if (spNumber_in_group.Text.Trim() != "") error = error + "If you are not in a secondary practice partnership/group, you must leave the number-in-group field blank.<br/>";
            }

            if (cmeRBL11.SelectedValue != "Y")
                error = error + "You must have completed your CE requirements (question 11) to be able to renew your license. <br/>";

            if ((spZip.Text.Trim() != "") && (!App_Code.Utilities.IsValidUSZip2(spZip.Text)))
                error = error + "You must enter a valid zip for secondary practice zip, question 17. <br/>";
            if ((hispanic_originRBL.SelectedValue != "Y") && (hispanic_originRBL.SelectedValue != "N"))
                error = error + "You must have indicate if you are of hispanic origin, question 22. <br/>";
            if ((ethnicityDDL.SelectedIndex == -1) || (ethnicityDDL.SelectedIndex == 0))
                error = error + "You must enter a race for question 21. <br/>";
            return error;
        }
        protected void update()
        {
            r.arrested = arrestRBL1.SelectedValue;
            r.cited_charged = cited_chargedRBL2.SelectedValue;
            r.crim_investigation = crim_investigationRBL3.SelectedValue;
            r.convicted = convictedRBL4.SelectedValue;
            r.investigation = investigationRBL5.SelectedValue;
            //2020_06_01 question swap
            //r.depressed = depressedRBL6.SelectedValue;
            //r.alcohol = alcoholRBL7.SelectedValue;
            r.impairment = impairmentRBL6.SelectedValue;
            //r.sexual = sexualRBL9.SelectedValue;
            //2020_06_01 question swap
            r.malpractice = malpracticeRBL10.SelectedValue;
            r.CME = cmeRBL11.SelectedValue;
            //
            r.total_hrs = PRACTICE_HOURS.SelectedValue;
            r.primary_hrs = PRIMARY_PRAC_HOURS.SelectedValue;
            r.primCounty = ppCOUNTY.SelectedValue;
            r.primSetting = ppPracticeInfo.SelectedValue; //required
            r.primGrpNbr = ppNumber_in_group.Text;
            //
            r.SecPracZip = spZip.Text;
            r.SecPracCounty = spCOUNTY.SelectedValue;
            r.SecPracSetting = spPracticeInfo.SelectedValue; //required
            r.SecPracGrpNbr = spNumber_in_group.Text;
            r.Race = ethnicityDDL.SelectedValue;
            r.Hispanic_flg = hispanic_originRBL.SelectedValue;
            r.TxHScounty = highSchoolCountyDDL.SelectedValue;
        }
        //
    }
}