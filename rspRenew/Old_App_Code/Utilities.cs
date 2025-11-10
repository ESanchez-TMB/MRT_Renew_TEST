using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace rspRenew.App_Code
{
    public class Utilities
    {
        public static string calculate_RefID_mrtRnl(int OrderID)
        {
            string month = "";
            string year = "";
            int mm = DateTime.Now.Month;
            int yy = DateTime.Now.Year;
            if (mm < 10)
                month = "0" + mm.ToString();
            else
                month = mm.ToString();
            year = (yy.ToString()).Substring(3, 1); //shorten from two to one
            //return Rconfiguration.NIClocalrefid + "R" +
            return "503MRR" + year + month +
                (int.Parse("1000000") + OrderID).ToString().Substring(1, 6);
        }
        //
        public static decimal getCfee(decimal tot)
        {
            //in globals
            decimal percentage =  RClassLibrary.DataAccess.getPercentage();
            decimal addon = RClassLibrary.DataAccess.getAddon();
            return (tot + addon) * percentage + addon;
        }
        //
        public static string formatZIP(string input)
        {
            if (input.Length == 5) return input;
            if (input.Length == 9) return input.Substring(0, 5) + "-" + input.Substring(5, 4);
            return "";
        }
        public static bool licenseType(string LicNum, string type)
        {
            if (LicNum.Length > 1)
            {
                string firstTwo = LicNum.Substring(0, 2);
                if (firstTwo == type) return true;
                else return false;
            }
            return false;
        }
        //public static bool isPHY(string license)
        //{
        //    if (license.Length > 1)
        //    {
        //        string firstTwo = license.Substring(0, 2);
        //        if (firstTwo.ToUpper() == "PA") return true;
        //        else return false;
        //    }
        //    return false;
        //}
        public static bool isPA(string license)
        {
            if (license.Length > 1)
            {
                string firstTwo = license.Substring(0, 2);
                if (firstTwo.ToUpper() == "PA") return true;
                else return false;
            }
            return false;
        }
        public static bool isAC(string license)
        {
            if (license.Length > 1)
            {
                string firstTwo = license.Substring(0, 2);
                if (firstTwo.ToUpper() == "AC") return true;
                else return false;
            }
            return false;
        }
        public static bool isNCT(string license)
        {
            if (license.Length > 1)
            {
                string firstTwo = license.Substring(0, 2);
                if (firstTwo.ToUpper() == "NC") return true;
                else return false;
            }
            return false;
        }
        public static string nameFormat1(string name)
        {
            //Smith, Everett james
            string[] parts = name.Split(new char[] { ',' });
            return parts[1] + " " + parts[0];
        }
        public static bool causeNum_bad(string causeNum)
        {
            try
            {
                if (causeNum.Length < 5) return true;
                if (causeNum.Length > 20) return true;
                foreach (char c in causeNum)
                {
                    if (Char.IsLetterOrDigit(c)) continue;
                    if (c.Equals('-')) continue;
                    else return true;
                }
                // all zeros are not allowed
                try
                {
                    ulong u = ulong.Parse(causeNum);
                    if (u == 0) return true;
                }
                catch { }
                return false;
            }
            catch { return false; }
        }
       //
        public static bool putRenewXML(RClassLibrary.renewal r)
        {
            string path = ConfigurationManager.AppSettings["path"];
            try
            {
                //@"c:\inProgressPA\"
                System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(r.GetType());
                Stream stream = new FileStream(path + r.LicNum.Trim() + r.SSN.Trim(),
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, r);
                stream.Close();
                return true;

            }
            catch (Exception e)
            {
                App_Code.DataAccess.log("Error " + e.Message, "Utilities.putRenewXML");
                return false;
            }
        }
        
        //todo: this will be a stubb for testing until db is ready
        //chuck to furnish routinees
        public static RClassLibrary.renewal createNewMRTrenew()
        {
            RClassLibrary.renewal r = new RClassLibrary.renewal();
            r.Race = "WHT";
            r.Hispanic_flg =
            r.TxHScounty = "";
            r.LicenseExpirationDate = DateTime.Parse("1/31/2016");
            r.LicNum = "fantastic1";
            r.SSN = "123";
            r.PAname = "John" + " " + "Doe";
            r.CURRFEE = decimal.Parse("66.00");
            r.FeeCode = "";

            //addresses  - mail                  
            r.MailingAddressLine1 = "100 Main";
            r.MailingAddressLine2 = ""; ;
            r.MailingAddressCity = "Austin";
            r.MailingAddressState = "TX";
            r.MailingAddressZIP = "78745";
            r.MailingAddressCountry = "US";
            r.PhoneNum = "5124443333";
            r.EMailAddr = "John.Doe@gmail.com";
            r.FaxNum = "";
            //practice
            r.PracticeAddressLine1 = "200 Riverside Blvd";
            r.PracticeAddressLine2 = "";
            r.PracticeAddressCity = "Austin";
            r.PracticeAddressState = "TX";
            r.PracticeAddressZIP = "78701";
            r.PracticeAddressCountry = "USA";
            r.PPhoneNum = "5127763456";
            r.PEMailAddr = "";
            r.PFaxNum = "";
            return r;

        }
        //
        public static RClassLibrary .renewal  getWIPrenewal(string license)
        {   // will be 1 or 0 
            //as of 11_26 ln_fn_dob will actually be a email + dob
             renewDB.ARO_DEV01 db = new  renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
            try
            {
                //get the order

                license = license.ToUpper();
                 renewDB.Tb_work_inprogress wip = (from w in db.Tb_work_inprogress
                                                 where w.License_num == license 
                                                 select w).Single< renewDB.Tb_work_inprogress>();
                if (wip != null)
                {
                    var xs = new XmlSerializer(typeof(RClassLibrary .renewal));
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal r = (RClassLibrary.renewal)xs.Deserialize(sr);
                    return r;
                }
                else
                    return null;
            }
            
            catch (Exception e)
            {
                RClassLibrary .DataAccess .log ("Error: " + e.Message, string.Format("getWIPrenew({0})", license));
                return null;
            }
        }
       
        //
        public static string putWIPrenewal(RClassLibrary.renewal r)
        {
            string status = "";
            try
            {
                var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                
                //
                RClassLibrary.renewal app2 = getWIPrenewal(r.LicNum); //should only be 0 or 1
                //
                if (app2 != null)
                {
                    if (app2.status.ToUpper().Trim() == "SUCCESS")
                    {
                        return "failure: update to successful application not allowed.";
                    }
                    status = updateWIPrenewal(r); //needs to do a update instead a an insert - must chang all applications
                    if (status != "success")
                    {
                        RClassLibrary.DataAccess.log("Error: updateAppDBwip failed.", string.Format("DataAccess.putWIPrenewal({0})", r.LicNum ));
                        return "error";
                    }
                }
                else
                {// first wip
                    status = insertWIPrenewal(r);
                    if (status != "success")
                    {
                        RClassLibrary.DataAccess .log("Error: insertAppDBwip failed.", "DataAccess.putApplDBwip(phyAppCL.phyApp r)");
                        return "error";
                    }
                }

                return "success";
            }
            catch (Exception e)
            {
                RClassLibrary.DataAccess.log("Error: " + e.Message, "DataAccess.putApplDBwip(phyAppCL.phyApp r)");
                return e.Message;
            }
        }
        //
        public static string insertWIPrenewal(RClassLibrary.renewal r)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                //serialize
                var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                //
                
                //
               renewDB.Tb_work_inprogress wip = new renewDB.Tb_work_inprogress
                {
                    License_num = r.LicNum ,                    
                    Work_data = sw.ToString(),
                    Create_dt = DateTime.Now,
                    Maint_dt = DateTime.Now
                };
               wip.Create_user = "rtRnl";
               wip.Maint_user = "rtRnl";


                db.Tb_work_inprogress.InsertOnSubmit(wip);
                db.SubmitChanges();
                return "success";
            }
            catch (Exception e)
            {
                RClassLibrary.DataAccess .log("Error: orderID: " + e.Message, string.Format("DataAccess.insertWIPrenewal({0})", r.LicNum ));
                return e.Message;
            }
        }
        //
        public static string updateWIPrenewal(RClassLibrary .renewal r)
        {
            
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_work_inprogress wip = (from w in db.Tb_work_inprogress
                                                 where w.License_num == r.LicNum 
                                                 select w).Single();

                //serialize
                var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                wip.Work_data = sw.ToString();
                wip.Maint_dt = DateTime.Now;
                // don't update maint_usr, it was specified by insert routine
                db.SubmitChanges();
                return "success";

            }
            catch (Exception e)
            {
                RClassLibrary.DataAccess.log("Error: " + r.LicNum + " " + e.Message, "DataAccess.updateWIPrenewal");
                return "fail";
            }

        }
        //     
        public static Control GetPostBackControl(Page page)
        {
            Control postbackControlInstance = null;

            string postbackControlName = page.Request.Params.Get("__EVENTTARGET");
            if (postbackControlName != null && postbackControlName != string.Empty)
            {
                postbackControlInstance = page.FindControl(postbackControlName);
            }
            else
            {
                // handle the Button control postbacks
                for (int i = 0; i < page.Request.Form.Keys.Count; i++)
                {
                    postbackControlInstance = page.FindControl(page.Request.Form.Keys[i]);
                    if (postbackControlInstance is System.Web.UI.WebControls.Button)
                    {
                        return postbackControlInstance;
                    }
                }
            }
            // handle the ImageButton postbacks
            if (postbackControlInstance == null)
            {
                for (int i = 0; i < page.Request.Form.Count; i++)
                {
                    if ((page.Request.Form.Keys[i].EndsWith(".x")) || (page.Request.Form.Keys[i].EndsWith(".y")))
                    {
                        postbackControlInstance = page.FindControl(page.Request.Form.Keys[i].Substring(0, page.Request.Form.Keys[i].Length - 2));
                        return postbackControlInstance;
                    }
                }
            }
            return postbackControlInstance;
        }
        public static bool isConvFee(decimal fee) //isConvFee
        {
            return true; //always true for PA and AC
            //
            foreach (revenue_master rm in revenue_masters.revenues)
            {
                if ((fee == rm.total_fee) && (rm.end_dt == null)) return true; //September gave me date

            }
            return false;
        }
        public static string yesNo(string s)
        {
            try
            {
                if (s.Trim().ToUpper() == "Y") return "Yes";
                if (s.Trim().ToUpper() == "N") return "No";
                return s;
            }
            catch
            {
                return "";
            }
        }
        public static bool IsValidName(string name)
        {
            if (name == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            string pattern = @"^([a-zA-Z]*)$";
            Regex match = new Regex(pattern);
            return match.IsMatch(name);
        }
        public static string phoneDisplay(string num)
        {
            //assumes a 10 digit phone number
            if (num == null) return "";
            if (num.Length != 10) return num;
            string outString = num.Insert(6, "-");
            outString = outString.Insert(3, "-");
            return outString;
        }
        public static bool IsValidEmail(string num)
        {
            if (num == null) return false;
            //http://www.regular-expressions.info/email.html
            //string pattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"; //http://stackoverflow.com/questions/5342375/c-sharp-regex-email-validation
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        public static bool IsValidFax(string num)
        {
            if (num == null) return false;
            //http://www.regular-expressions.info/email.html
            string pattern = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        public static bool IsValidUSZip(string num)
        {
            if (num == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            //string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            string pattern = @"^(\d{5})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        public static bool IsValidUSZip2(string num)
        {
            if (num == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        public static bool IsValidUSZipExt(string num)
        {
            if (num == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            //string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            string pattern = @"^(\d{4})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        public static bool IsValidUSphone(string num)
        {
            if (num == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            //string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            string pattern = @"^(\d{10})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        //public static bool isEmail(string num)
        //{
        //    if (num == null) return false;
        //    // This is difficult problem that no one have solved
        //    //string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
        //    //string pattern = @"^(.*@.*\.*)$"; //simple and stupid but should work
        //    string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"; //http://stackoverflow.com/questions/5342375/c-sharp-regex-email-validation
        //    Regex match = new Regex(pattern);
        //    return match.IsMatch(num);
        //}
        public static bool IsDigits(string num)
        {
            if (num == null) return false;
            string pattern = @"^(\d*)$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }

        public static string errMsg(Exception e)
        {
            return e.TargetSite.Name + ": " + e.Message;
        }//

        public static bool isMMDDYYYY(string value)
        {
            if (value == null) return false;
            // eg 04/2000
            // string [] split = words.Split(new Char [] {' ', ',', '.', ':', '\t' });
            string[] parts = value.Split(new Char[] { '/' });
            if (parts.Length != 3) return false;
            try
            {
                int month = int.Parse(parts[0]);
                if (month < 1) return false;
                if (month > 12) return false;
                int day = int.Parse(parts[1]);
                if (day < 1) return false;
                if (day > 31) return false;
                int year = int.Parse(parts[2]);
                if (year < 1930) return false;  //shaky, really a different edit
                //01/2021
                //if (year > 2020) return false;
                DateTime today = DateTime.Now;
                int yearPlus = today.Year + 1;
                if (year > yearPlus) return false;
                //01/2021
            }
            catch
            {
                return false;
            }
            string pattern = @"^(\d{1,2}/\d{1,2}/\d{4})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(value);
        }
        public static bool isMMYYYY(string value)
        {
            if (value == null) return false;
            // eg 04/2000
            // string [] split = words.Split(new Char [] {' ', ',', '.', ':', '\t' });
            string[] parts = value.Split(new Char[] { '/' });
            if (parts.Length != 2) return false;
            try
            {
                int month = int.Parse(parts[0]);
                if (month < 1) return false;
                if (month > 12) return false;
                int year = int.Parse(parts[1]);
                if (year < 1930) return false;  //shaky, really a different edit
                //01/2021
                //if (year > 2020) return false;
                DateTime today = DateTime.Now;
                int yearPlus = today.Year + 1;
                if (year > yearPlus) return false;
                //01/2021
            }
            catch
            {
                return false;
            }
            string pattern = @"^(\d{1,2}/\d{4})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(value);
        }
        public static bool isNumeric(string value)
        {
            string pattern = @"^(\d+)$";
            Regex match = new Regex(pattern);
            return match.IsMatch(value);
        }
        public static bool isDate(string value)
        {
            if (value == null) return false;
            try
            {
                DateTime t = DateTime.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }// isNumeric
        public static bool isDecimal(string value)
        {
            try
            {
                decimal i = decimal.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }// isNumeric
        // djc Sept

        public static bool isInt(string value)
        {
            try
            {
                int i = int.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }// isNumeric
        public static string onlyNumbers(string input)
        {
            string output = "";
            foreach (char c in input)
            {
                try
                {
                    int i = int.Parse(c.ToString());
                    output += c.ToString();
                }
                catch { }
            }
            return output;
        }
        public static bool isNotValidLicNum(string licNum)
        {
            if (licNum == null) return false;
            // Allows letter followed by 4 digits eg Z1234
            string pattern = @"^([A-Z]\d{4})$";
            Regex match = new Regex(pattern);
            return !match.IsMatch(licNum);
        }        
        //todo - criteria must be gotten from Rhea
        public static string isEligible2renew(RClassLibrary.renewal r)
        {
            
            string mode = App_Code.Rconfiguration.NICMode;
            if (mode == "DEV" || mode == "TEST")
            {
                return "";
            }
            //if (DateTime.Now.Month < 9) return "Trying to register too early";            
            //rhea wants a 5 day grace period
            if (DateTime.Now > r.LicenseExpirationDate.AddDays(5)) return string.Format("Trying to register too late. Expire date: {0}", r.LicenseExpirationDate.ToShortTimeString());
            //
            return "";
        }
        //
        
        //
        public static string edit(RClassLibrary.renewal r)
        {
            string error = "";
            if ((r.arrested != "Y") && (r.arrested != "N")) //(arrestRBL1.SelectedIndex != -1)
                error = error + "You must answer Question 1 on the Questions page. <br/>";
            if ((r.cited_charged != "Y") && (r.cited_charged != "N")) //(cited_chargedRBL2.SelectedIndex != -1)
                error = error + "You must answer Question 2 on the Questions page. <br/>";
            if ((r.crim_investigation != "Y") && (r.crim_investigation != "N")) //(crim_investigationRBL3.SelectedIndex != -1)
                error = error + "You must answer Question 3 on the Questions page. <br/>";
            if ((r.convicted != "Y") && (r.convicted != "N")) //(convictedRBL4.SelectedIndex != -1)
                error = error + "You must answer Question 4 on the Questions page. <br/>";
            if ((r.investigation != "Y") && (r.investigation != "N")) //(investigationRBL5.SelectedIndex != -1)
                error = error + "You must answer Question 5 on the Questions page. <br/>";
            //if ((r.depressed != "Y") && (r.depressed != "N")) //(depressedRBL6.SelectedIndex != -1)
            //    error = error + "You must answer Question 6 on the Questions page. <br/>";
            //if ((r.alcohol != "Y") && (r.alcohol != "N")) //(alcoholRBL7.SelectedIndex != -1)
            //    error = error + "You must answer Question 7 on the Questions page. <br/>";
            if ((r.impairment != "Y") && (r.impairment != "N")) //(impairmentRBL8.SelectedIndex != -1)
                error = error + "You must answer Question 6 on the Questions page. <br/>";
            //if ((r.sexual != "Y") && (r.sexual != "N")) //(sexualRBL9.SelectedIndex != -1)
            //    error = error + "You must answer Question 9 on the Questions page. <br/>";
            if ((r.malpractice != "Y") && (r.malpractice != "N")) //(malpracticeRBL10.SelectedIndex != -1)
                error = error + "You must answer Question 10 on the Questions page. <br/>";
            if ((r.CME != "Y") && (r.convicted != "N")) //(cmeRBL11.SelectedValue != "Y")
                error = error + "<b>You must have completed your CME requirements (question 11) to be able to renew your license.</b> <br/>";
            if (r.Race == "") //((ethnicityDDL.SelectedIndex == -1) || (ethnicityDDL.SelectedIndex == 0))
                error = error + "<b>You must enter a race for question 21 on the Questions page.</b> <br/>";
            if ((r.Hispanic_flg != "Y") && (r.Hispanic_flg != "N"))
                error = error + "<b>You must have indicate if you are of hispanic origin, question 22 on the Questions page. <br/>";


            return error;
        }
        public static bool validFee(Dictionary<int, decimal> feeTable, decimal fee)
        {
            try
            {
                foreach (KeyValuePair<int, decimal> kvp in feeTable)
                {
                    if (kvp.Value == fee)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        } 
        //

        internal static RClassLibrary.renewal createNewRTrenew()
        {
            RClassLibrary.renewal r = new RClassLibrary.renewal();
            r.Race = "WHT";
            r.Hispanic_flg = "";
            r.TxHScounty = "";
            r.LicenseExpirationDate = DateTime.Parse("1/31/2016");
            r.LicNum = "RT0001";
            r.SSN = "1234";
            r.PAname = "Jane" + "R. T." + "Doe";
            r.CURRFEE = decimal.Parse("106.00");
            r.FeeCode = "";

            //addresses  - mail                  
            r.MailingAddressLine1 = "100 Main";
            r.MailingAddressLine2 = ""; ;
            r.MailingAddressCity = "Austin";
            r.MailingAddressState = "TX";
            r.MailingAddressZIP = "78745";
            r.MailingAddressCountry = "US";
            r.PhoneNum = "5124442233";
            r.EMailAddr = "Jane.Doe@gmail.com";
            r.FaxNum = "";
            //practice
            r.PracticeAddressLine1 = "300 Riverside Blvd";
            r.PracticeAddressLine2 = "";
            r.PracticeAddressCity = "Austin";
            r.PracticeAddressState = "TX";
            r.PracticeAddressZIP = "78701";
            r.PracticeAddressCountry = "USA";
            r.PPhoneNum = "5123334456";
            r.PEMailAddr = "";
            r.PFaxNum = "";
            return r;
        }

        internal static string calculate_RefID_rtRnl(int OrderID)
        {
            string month = "";
            string year = "";
            int mm = DateTime.Now.Month;
            int yy = DateTime.Now.Year;
            if (mm < 10)
                month = "0" + mm.ToString();
            else
                month = mm.ToString();
            year = (yy.ToString()).Substring(2,2); 
            return "503RW" + year + month +
                (int.Parse("1000000") + OrderID).ToString().Substring(1, 6);
        }
    }
}