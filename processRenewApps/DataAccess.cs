using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace processRenewApps
{
    class DataAccess
    {
         static logger log;

        static DataAccess()
        {
            log = logger.getLogger();
        }
        //
        public static Tracerdb.SYS_COMMON_PARAM getCommon(Tracerdb.SQLTracerDev db)
        {
            try
            {

                Tracerdb.SYS_COMMON_PARAM common = (from s in db.SYS_COMMON_PARAM
                                                    select s).Single();
                return common;
            }
            catch (Exception e)
            {
                log.log("Error: " + e.Message + " " + new StackFrame(0).GetMethod().Name);
                return null;
            }
        }
        //
        public static Tracerdb.SYS_TXONLINE_PARAM getOnline(Tracerdb.SQLTracerDev db)
        {
            try
            {

                Tracerdb.SYS_TXONLINE_PARAM online = (from s in db.SYS_TXONLINE_PARAM
                                                      select s).Single();
                return online;
            }
            catch (Exception e)
            {
                log.log("Error: " + e.Message + " " + new StackFrame(0).GetMethod().Name);
                return null;
            }
        }
        //
        public static Tracerdb.SYS_CASH_PARAM getSCP(Tracerdb.SQLTracerDev db)
        {
            try
            {

                Tracerdb.SYS_CASH_PARAM scp = (from s in db.SYS_CASH_PARAM
                                               select s).Single();
                return scp;
            }
            catch (Exception e)
            {
                log.log("Error " +  new StackFrame(0).GetMethod().Name + ": " + e.Message);
                return null;
            }
        }
        //

        internal static bool write2DB(RClassLibrary.renewal r)
        {
            //this routine with write the sb202 renewal stuff to the "staging" tables
            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                    Tracerdb.Onlinerenewals or = new Tracerdb.Onlinerenewals()
                    {
                        Lictype = r.LicType,
                        License_number = r.LicNum,
                        Tracenumber = r.trace_number,
                        Transaction_dt = DateTime.Parse(r.TRANSACTION_DATE),
                        Race_cd = r.Race,
                        Hispanic_origin_cd = r.Hispanic_flg,
                        High_school_county_cd = r.TxHScounty,
                        Continuing_education_flg = r.CME,
                        Renewal_fee_paid = r.CURRFEE,
                        Total_amount_paid = r.totDue,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"

                    };
                    db.Onlinerenewals.InsertOnSubmit(or);
                    //mail
                    Tracerdb.Onlinerenewal_addresses ma = new Tracerdb.Onlinerenewal_addresses()
                    {
                        Tracenumber = r.trace_number,
                        Line1 = r.MailingAddressLine1,
                        Line2 = r.MailingAddressLine2,
                        City = r.MailingAddressCity,
                        State_cd = r.MailingAddressState,
                        Postalcode = r.MailingAddressZIP,
                        Country_cd = r.MailingAddressCountry,
                        //International_locality_cd  what is this?????
                        Location_type_cd = "MAIL",
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_addresses.InsertOnSubmit(ma);
                    //
                    if (r.PracticeAddressLine1.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_addresses pa = new Tracerdb.Onlinerenewal_addresses()
                        {
                            Tracenumber = r.trace_number,
                            Line1 = r.PracticeAddressLine1,
                            Line2 = r.PracticeAddressLine2,
                            City = r.PracticeAddressCity,
                            State_cd = r.PracticeAddressState,
                            Postalcode = r.PracticeAddressZIP,
                            Country_cd = r.PracticeAddressCountry,
                            //International_locality_cd  what is this?????
                            Location_type_cd = "PRAC",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };
                        db.Onlinerenewal_addresses.InsertOnSubmit(pa);
                    }
                    //
                    if (r.PhoneNum.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_phone_numbers pn = new Tracerdb.Onlinerenewal_phone_numbers()
                        {
                            Tracenumber = r.trace_number,
                            Phone_number = r.PhoneNum,
                            Phone_type_cd = "HOME",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };

                        db.Onlinerenewal_phone_numbers.InsertOnSubmit(pn);
                    }
                    //
                    if (r.PPhoneNum.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_phone_numbers pn = new Tracerdb.Onlinerenewal_phone_numbers()
                        {
                            Tracenumber = r.trace_number,
                            Phone_number = r.PPhoneNum,
                            Phone_type_cd = "BUS",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };

                        db.Onlinerenewal_phone_numbers.InsertOnSubmit(pn);
                    }
                    //
                    if (r.FaxNum.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_phone_numbers fn = new Tracerdb.Onlinerenewal_phone_numbers()
                        {
                            Tracenumber = r.trace_number,
                            Phone_number = r.FaxNum,
                            Phone_type_cd = "HOMEFAX",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };
                        db.Onlinerenewal_phone_numbers.InsertOnSubmit(fn);
                    }
                    //
                    if (r.PFaxNum.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_phone_numbers fn = new Tracerdb.Onlinerenewal_phone_numbers()
                        {
                            Tracenumber = r.trace_number,
                            Phone_number = r.PFaxNum,
                            Phone_type_cd = "BUSFAX",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };
                        db.Onlinerenewal_phone_numbers.InsertOnSubmit(fn);
                    }
                    //
                    //                    total_practice_hours_cd values = 1, 2, 3, 4, 5  
                    //                    (I think you already have this but 1 = 40+, 2 = 20-39, 3 = 11-19, 4 = 1-10, 5 = N/A)

                    //                    primary_practice_hours_cd values = 1, 2, 3, 4, 5  

                    //                    primary_practice_setting_cd values = 0,1,2,3,4,5,6,7,8,9,10,11,12
                    //                      (I think you already have this but 0 = Did not answer, 1 = Military, 2 = VA, 3 = PHS, 4 = HMO, 5 = Hospital Based, 6 = Solo, 7 = Partnership/Group, 8 = Other, 9 = Research, 10 = Medical School Faculty, 11 = Direct Medical Care, 12 = Not Applicable)

                    //                    secondary_practice_setting_cd values = 0,1,2,3,4,5,6,7,8,9,10,11,12
                    if (r.EMailAddr.Trim() != "")
                    {
                        Tracerdb.Onlinerenewal_email_addresses ea = new Tracerdb.Onlinerenewal_email_addresses()
                        {
                            Tracenumber = r.trace_number,
                            Email_address = r.EMailAddr,
                            Location_type_cd = "HOME",
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };
                        db.Onlinerenewal_email_addresses.InsertOnSubmit(ea);
                    }
                    //
                    int num_in_group = 0;
                    try
                    {
                        if (r.primGrpNbr != "")
                            num_in_group = int.Parse(r.primGrpNbr);
                    }
                    catch
                    {
                        num_in_group = 0;
                    }
                    int sec_num_in_grp = 0;
                    try
                    {
                        if (r.SecPracGrpNbr != "")
                            sec_num_in_grp = int.Parse(r.SecPracGrpNbr);
                    }
                    catch
                    {
                        sec_num_in_grp = 0;
                    }
                    //
                    int hours = 5; // 1 = 40+, 2 = 20-39, 3 = 11-19, 4 = 1-10, 5 = N/A
                    try
                    {
                        hours = int.Parse(r.total_hrs);
                    }
                    catch
                    {
                        hours = 5;
                    }

                    if ((r.total_hrs != "") ||
                        (r.primary_hrs != "") ||
                        (r.primCounty != "") ||
                        (r.primSetting != "") ||
                        (r.primGrpNbr != "") || 
                        (r.SecPracZip != "") ||
                        (r.SecPracCounty != "") ||
                        (r.SecPracSetting != "") ||
                        (r.SecPracGrpNbr != ""))
                    {
                        Tracerdb.Onlinerenewal_practice_information pi = new Tracerdb.Onlinerenewal_practice_information()
                        {
                            Tracenumber = r.trace_number,
                            Total_practice_hours_cd = r.total_hrs,
                            Primary_practice_hours_cd = r.primary_hrs,
                            Primary_practice_county_cd = r.primCounty,
                            Primary_practice_setting_cd = r.primSetting,
                            Primary_practice_num_in_group = num_in_group,
                            Secondary_practice_postalcode = r.SecPracZip,
                            Secondary_practice_county_cd = r.SecPracCounty,
                            Secondary_practice_setting_cd = r.SecPracSetting,
                            Secondary_practice_num_in_group = sec_num_in_grp,
                            Create_dt = DateTime.Now,
                            Create_user = "processRenewApps"
                        };
                        db.Onlinerenewal_practice_information.InsertOnSubmit(pi);
                    }
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans1 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "arrested"),
                        Answer = r.arrested,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans1);
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans2 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "convicted"),
                        Answer = r.convicted,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans2);
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans3 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "crim_investigation"),
                        Answer = r.crim_investigation,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans3);
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans4 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "cited_charged"),
                        Answer = r.cited_charged,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans4);
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans5 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "investigation"),
                        Answer = r.investigation,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans5);
                    //
                    // 2020_06_03 question swap 
                    //Tracerdb.Onlinerenewal_enforcement_answers ans6 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    //{
                    //    Tracenumber = r.trace_number,
                    //    Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "depressed"),
                    //    Answer = r.depressed,
                    //    Create_dt = DateTime.Now,
                    //    Create_user = "processRenewApps"
                    //};
                    //db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans6);
                    // 2020_06_03 question swap -  may not be ans7 anymore ??? oes it mater 
                    Tracerdb.Onlinerenewal_enforcement_answers ans7 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "impairment"),
                        Answer = r.impairment,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    // 2020_06_03 question swap 
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans7);
                    //
                    //Tracerdb.Onlinerenewal_enforcement_answers ans8 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    //{
                    //    Tracenumber = r.trace_number,
                    //    Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "alcohol"),
                    //    Answer = r.malpractice,
                    //    Create_dt = DateTime.Now,
                    //    Create_user = "processRenewApps"
                    //};
                    //db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans8);
                    ////
                    //Tracerdb.Onlinerenewal_enforcement_answers ans9 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    //{
                    //    Tracenumber = r.trace_number,
                    //    Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "sexual"),
                    //    Answer = r.sexual,
                    //    Create_dt = DateTime.Now,
                    //    Create_user = "processRenewApps"
                    //};
                    //db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans9);
                    // 2020_06_03 question swap 
                    //
                    Tracerdb.Onlinerenewal_enforcement_answers ans10 = new Tracerdb.Onlinerenewal_enforcement_answers()
                    {
                        Tracenumber = r.trace_number,
                        Licensure_question_id = DataAccess.getLicensureQuestionID(db, r.LicNum, "malpractice"),
                        Answer = r.malpractice,
                        Create_dt = DateTime.Now,
                        Create_user = "processRenewApps"
                    };
                    db.Onlinerenewal_enforcement_answers.InsertOnSubmit(ans10);
                    //
                    db.SubmitChanges();
                    log.log("success: write2DB");
                    scope.Complete(); //this is automatically a transaction for all the insertOnSubmit in this context
                                      //but this doesn't hurt anything
                }
                return true;

            }
            catch (Exception e)
            {
                log.log("Error on write2DB: " + r.PAname + " " + e.Message);
                return false;
            }

        }

        private static int getLicensureQuestionID(Tracerdb .SQLTracerDev db,              string license, string question)
        {
            //string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
            //Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
           
            try
            {
                string license_type = "";
                string prefix = license .Substring (0,2);
                switch (prefix)
                {
                    case "GM":
                        license_type = "MRT";
                       break;
                    case "LM":
                        license_type = "MRT";
                        break;
                    case "FM":
                        license_type = "MP";
                        break;
                    case "FP":
                        license_type = "PF";
                        break;
                    case "RC":
                        license_type = "RCP";
                        break;
                    case "NC":
                        license_type = "NCR";
                        break;
                    default :
                        return 0;                       
                }
                var qu = (from q in db.Onlinerenewal_enforcement_questions
                                          where q.License_type == license_type && q.Short_description == question && q.Active_flg == "Y"
                                          select q).Single();
                return qu.Licensure_question_id;

            }
            catch (Exception e)
            {
                log.log("error processRenewApps.DataAccess. getLicensureQuestionID: " + e.Message);
                return 0;
            }            
        }
       
            //
        public static bool remove(RClassLibrary .renewal r)
        {
            try
            {
                renewDB .ARO_DEV01  db = new renewDB .ARO_DEV01 (System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wip = (from w in db.Tb_work_inprogress
                           where w.License_num  == r.LicNum
                           select w).Single ();
                
                // delete the guy
                db.Tb_work_inprogress.DeleteOnSubmit(wip);
                db.SubmitChanges();
                return true;

            }
            catch (Exception e)
            {
                log.log(string.Format("Error: DataAccess.remove ({0}:  {1} ) ", r.LicNum , e.Message));
                return false;
            }
        }
        //
        internal static List<RClassLibrary.renewal> getNCRapps()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("NCR")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getLMPapps Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }
        //       
        internal static List<RClassLibrary.renewal> getLMPapps()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("FMP")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getLMPapps Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }
        //
        internal static List<RClassLibrary.renewal> getPRFapps()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("FPF")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getPRFapps Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }

        internal static List<RClassLibrary.renewal> getRSPapps()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("RC")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getRSPapps Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }

        internal static List<RClassLibrary.renewal> getMRTapps()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("GM") || w.License_num .StartsWith ("LM")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getMRTapps Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }

        public static bool deleteOld(int days, string license_type)
        {
            //
            try
            {

                renewDB .ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num .StartsWith (license_type )
                           select w;
                //
                DateTime dt = DateTime.Now;
                dt = dt.AddDays(-days);
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    if (wip.Create_dt < dt)
                    {
                        db.Tb_work_inprogress.DeleteOnSubmit(wip);
                        log.log(string.Format("Delete old: {0}", wip.License_num ));
                    }
                }

                db.SubmitChanges();
                return true;

            }
            catch (Exception e)
            {
                log.log(string.Format("Error deleteOld({0}, {1}): ", days.ToString(), license_type ) + e.Message);
                return false;
            }
        }

        internal static List<RClassLibrary.renewal> getMRTapps4Test()  //get apps from production to process against test
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = WEBSQLPROD; Initial Catalog=ARO_PROD01;Persist Security Info=True;   User ID=usr_ARO;Password=kdln#57f; Connection Timeout=600;");  //this is production
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("GM") || w.License_num.StartsWith("LM")
                           select w;
                //
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); //this is the place the bad stuff happens
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal app = null;
                    try
                    {
                        app = (RClassLibrary.renewal)xs.Deserialize(sr);
                        apps.Add(app);
                    }
                    catch { }
                }
                return apps;
            }
            catch (Exception e)
            {
                log.log("getMRTapps4Test Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
             
        }
    }
}
