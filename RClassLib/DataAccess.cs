using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text;

namespace RClassLibrary
{
    public class DataAccess
    {
        public static void putWcf_outbound_payment_log(TxGov1.NICorder no)
        {
            string s1 = utilities.getLogInfo();
            if (s1.Length > 50)
                s1 = s1.Substring(0, 49);

            renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

            renewDB.Tb_outbound_payment_log pl = new renewDB.Tb_outbound_payment_log
            {
                Order_id = no.orderID,
                Statecd = "", //ord.statecd,
                //Name = Name, //ord.name,
                //Companyname = Companyname, //request.companyname,
                //Email = Email,//email,
                //Hrefcancel = Hrefcancel, //hrefcancel,
                //Hrefduplicate = Hrefduplicate, //hrefduplicate,
                //Hrefsuccess = Hrefsuccess,//hrefsuccess,
                //Hreffailure = Hreffailure,
                //Address1 = Address1,
                //Address2 = Address2,
                //City = City,
                //State = State,
                //Zip = Zip,
                //Country = Country,
                //Phone = Phone,
                //Fax = Fax,
                //Cid = Cid,
                Token = no.ppr.TOKEN,
                Err_msg = no.ppr.ERRORMESSAGE,
                Paytype = no.paytype,
                Uniquetransid = no.uniquetransid,
                Description = no.description,
                Localrefid = no.localrefid,
                Merchantid = no.merchantid,
                Merchantkey = no.merchantkey,
                Servicecode = no.servicecode,
                Amount = decimal.Parse(no.amount),
                Conv_fee = decimal.Parse(no.cfee),
                Create_dt = DateTime.Now,
                Create_user =s1,
                Maint_dt = DateTime.Now,
                Maint_user = s1
            };
            db.Tb_outbound_payment_log.InsertOnSubmit(pl);
            db.SubmitChanges();
        }
        //
        public static TxGov1.revenue getRevenue4CF(string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.Tb_revenue_detail rev = (from r in db.Tb_revenue_detail
                                                where r.Revenue_code == revenue_code
                                                select r).SingleOrDefault<renewDB.Tb_revenue_detail>();
                if (rev != null)
                {
                    TxGov1.revenue u = new TxGov1.revenue();
                    u.revenue_code = rev.Revenue_code;
                    u.fund = rev.Fund;
                    u.aobj = rev.Aobj;
                    u.cobj = rev.Cobj;
                    u.pca = rev.Pca;
                    u.trans_code = rev.Trans_code;
                    return u;
                }
                else return new TxGov1.revenue();
            }
            catch (Exception e)
            {
                //log(utilities.errMsg(e), "DataAccess.getUsasInfo2");
                return new TxGov1.revenue();
            }

        }
        //
        public static int getOrderIDbyToken(string token)
        {
            // is this really the best way to do this?
            //there might not be a successful outbound payment log
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                //create order 

                renewDB.Tb_outbound_payment_log lg = (from l in db.Tb_outbound_payment_log  //    .GetTable<renewDB.Tb_order>()
                                                     where l.Token == token
                                                     select l).SingleOrDefault();
                if (lg != null) return lg.Order_id;
                else return 0;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "SB 202 renewal DataAccess.getOrderIDby(token)");
                return 0;
            }

        }
        //
        public static void updateOrderStatus(int orderID, int status, string app)
        {
            string[] OrderStatuses = 
            {     
            "INIT",    // 0
            "PREP",    // 1
            "QUERY",   // 2
            "CMPLT",   // 3
            "CANX",    // 4
            "FAIL",    // 5
            "DUP",     // 6
            "Unk7",    // 7
            "Unk8",    // 8
            "CANX"     // 9       //was cancel ? maybe
            };
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_order ord = (from o in db.Tb_order //    .GetTable<linqdba.Tb_order>()
                                       where o.Order_id == orderID
                                       select o).Single();
                if (ord != null)
                {
                    ord.Maint_dt = DateTime.Now;
                    ord.Maint_user = app;
                    ord.Order_status_cd = OrderStatuses[status]; //up to 5 chars
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "RClassLibrary DataAccess.updateOrderStatus(" + orderID.ToString() + ", " + status.ToString());
            }
        }
        //
        public static decimal getFee(string rc)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"];
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(dbname);
                renewDB.Tb_revenue rev = (from r in db.Tb_revenue
                                          where r.Revenue_code == rc && r.End_dt == null
                                          select r).SingleOrDefault<renewDB.Tb_revenue>();
                return rev.Total_fee;
            }

            catch (Exception e)
            {
                DataAccess.log(e.Message, "RClassLibrary.DataAccess.getFee revenue code: " + rc);
                return 0.0M;
            }
        }
        //
        public static void audit(int orderID, string message, int messageNumber)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_audit aud = new renewDB.Tb_audit
                {
                    Order_id = orderID,
                    Message = message,
                    Message_nbr = messageNumber,
                    Create_dt = DateTime.Now,
                    Create_user = ""
                };
                db.Tb_audit.InsertOnSubmit(aud);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                log("Audit failed: " + e.Message, "DataAccess.createAudit");
            }
        }
        //
        public static void dumpPPI(NICUSA.commonCheckOut.PaymentInfo ppi, int wip_id)
        {
            try
            {
                //serialize
                var xs = new XmlSerializer(ppi.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, ppi);
                //

                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_object_log ol = new renewDB.Tb_object_log
                {
                    License_num = "", //we can't know the license number here
                    Work_inprogress_id = wip_id,
                    Object_type_cd = "ppi",
                    Object_data = sw.ToString(),
                    Create_dt = DateTime.Now,
                    //Create_user = "appRCP",
                    Create_user = ppi.MERCHANTID,
                    Maint_dt = DateTime.Now,
                    //Maint_user = "appRCP"
                    Maint_user = ppi.MERCHANTID                    
                };
                db.GetTable<renewDB.Tb_object_log>().InsertOnSubmit(ol);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                log(e.Message, string.Format("dumpPPI(PPI, wip_id:{0})", wip_id));
            }
        }
        //        
        public static NICUSA.usasCodes getUsasInfo2(string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.Tb_revenue_detail rev = (from r in db.Tb_revenue_detail
                                                where r.Revenue_code == revenue_code
                                                select r).SingleOrDefault<renewDB.Tb_revenue_detail>();
                if (rev != null)
                {
                    NICUSA.usasCodes u = new NICUSA.usasCodes();
                    u.RevenueCode = rev.Revenue_code;
                    u.fund = rev.Fund;
                    u.aobj = rev.Aobj;
                    u.cobj = rev.Cobj;
                    u.pca = rev.Pca;
                    u.transCode = rev.Trans_code;
                    return u;
                }
                else return new NICUSA.usasCodes();
            }
            catch (Exception e)
            {
                //log(utilities.errMsg(e), "DataAccess.getUsasInfo2");
                return new NICUSA.usasCodes();
            }

        }
        //
        public static List<NICUSA.revenue> getRevenueCodes2(decimal fee, string revenue_code)
        {
            try
            {
                renewDB.Tb_revenue rv = getRevenue(fee, revenue_code);
                //return dbAccess.getCodeBlock2(rv);
                return getCodeBlock2(rv, fee);
            }
            catch (Exception e)
            {
                log("Error, getRevenueCodes2(fee,revenue_code): " + e.Message, " ");
                return new List<NICUSA.revenue>();
            }
        }
        public static renewDB.Tb_revenue getRevenue(decimal fee, string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var rv = (from r in db.Tb_revenue
                          where r.Total_fee == fee && r.Revenue_code == revenue_code
                          select r).Single<renewDB.Tb_revenue>();
                return rv;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getRevenue(int fee_nbr");
                return null;
            }
        }
        public static List<NICUSA.revenue> getCodeBlock2(renewDB.Tb_revenue rv, decimal fee)
        {
            List<NICUSA.revenue> codingBlock = new List<NICUSA.revenue>();
            try
            {
                List<NICUSA.revenue> rvs = new List<NICUSA.revenue>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                IQueryable<renewDB.Tb_revenue_detail> rcs = from r in db.Tb_revenue_detail
                                                           where r.Fee_nbr == rv.Fee_nbr && r.Revenue_code == rv.Revenue_code
                                                           select r;

                foreach (renewDB.Tb_revenue_detail rc in rcs)
                {
                    NICUSA.revenue r = new NICUSA.revenue();
                    r.aobj = rc.Aobj;
                    r.begin_dt = rv.Begin_dt;
                    r.cobj = rc.Cobj;
                    r.create_dt = rc.Create_dt;
                    r.create_user = rc.Create_user;
                    r.end_dt = rv.End_dt;
                    r.fee_amt = rc.Fee_amt;
                    //r.fee_amt = fee;
                    r.fee_nbr = rc.Fee_nbr;
                    r.fund = rc.Fund;
                    r.maint_dt = rc.Maint_dt;
                    r.maint_user = rc.Maint_user;
                    r.pca = rc.Pca;
                    r.revenue_code = rc.Revenue_code;
                    r.revenue_desc = rv.Revenue_desc;
                    r.trans_code = rc.Trans_code;

                    rvs.Add(r);

                }
                return rvs;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message, "DataAccess.getcodeBlock()");
                return null;
            }
        }
        //        
        public static decimal getAddon()
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Ts_globals glob = (from g in db.Ts_globals   //    .GetTable<renewDB.Tb_order>()
                                          where g.Parm_group == "CONV_FEE" && g.Parm_key == "ADDON"
                                          select g).SingleOrDefault();
                if (glob != null)
                {
                    return decimal.Parse(glob.Parm_value);
                }
                return 0.0m;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getAddon");
                return 0.0m;
            }
        }
        // 
        public static decimal getPercentage()
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Ts_globals glob = (from g in db.Ts_globals   //    .GetTable<renewDB.Tb_order>()
                                          where g.Parm_group == "CONV_FEE" && g.Parm_key == "PERCENTAGE"
                                          select g).SingleOrDefault();
                if (glob != null)
                {
                    return decimal.Parse(glob.Parm_value);
                }
                return 0.0m;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAcess.getPercentage");
                return 0.0m;
            }
        }
        //
       
        
        protected static bool presDelegation(Tracerdb.PA_SUP_PHY p)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                Tracerdb.PA PA = (from pa in db.PA
                                  where pa.ID_NUM == p.ID_NUM
                                  select pa).Single<Tracerdb.PA>();


                var ds = from d in db.DELEGATION
                         where d.ID_NUM == PA.ID_NUM && p.PA_SUP_PHY_LIC_NUM == d.PD_SUP_LIC_NUM && ((d.PD_TERM_DT == null) || (d.PD_TERM_DT > DateTime.Now)) && d.PD_APPR_DT != null
                         select d;
                if (ds.Count() > 0) return true;
                else return false;
            }
            catch
            {
                return false;
            }

        }
        public static string getPhyName(string lic_num)
        {
            try
            {

                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                           where i.LICENSE_NUM == lic_num
                                           select i).Single<Tracerdb.INDIVIDUAL>();

                return ind.LN + " ," + ind.FN + " " + ind.SUF + " " + ind.DEG;
            }
            catch (Exception e)
            {
                log("error: RClassLibrary.DataAccess.getPhyName", e.Message);
                return "";
            }
        }
        //            
        public static void log(string msg, string user)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                //linqdba.TOLS_DEV01 db = new linqdba.TOLS_DEV01(TMBstoreConfiguration.DbConnectionStringA);
                renewDB.Tb_log log = new renewDB.Tb_log
                {
                    Create_dt = DateTime.Now,
                    Log_msg = msg,
                    Create_user = user
                };
                db.Tb_log.InsertOnSubmit(log);
                db.SubmitChanges();



            }
            catch (Exception e)
            {
                //don't do nothin
            }
        }//log
        //public static int getIDnumFromLicense(string license)
        //{
        //    try
        //    {
        //        string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
        //        Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
        //        Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
        //                                   where
        //                                   i.LICENSE_NUM == license
        //                                   select i).SingleOrDefault<Tracerdb.INDIVIDUAL>();
        //        if (ind != null) return ind.ID_NUM;
        //        else return 0;

        //    }
        //    catch (Exception e)
        //    {
        //        string s1 = e.Message;
        //        return 0;
        //    }
        //}
        public static string getEmergencyPhone(Tracerdb.INDIVIDUAL ind)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.Tb_ind_phone> phones = from p in db.Tb_ind_phone
                                                           where p.Id_num == ind.ID_NUM && p.Phone_type_cd == "ECP" && p.Active_flg == "Y"
                                                           select p;
                //int ivalue = phones.Count <Tracerdb .Tb_ind_phone >();
                if (phones.Count<Tracerdb.Tb_ind_phone>() == 0) return "";
                Tracerdb.Tb_ind_phone old_phone = phones.First<Tracerdb.Tb_ind_phone>();
                if (phones.Count<Tracerdb.Tb_ind_phone>() == 1)
                    return old_phone.Area_code + old_phone.Phone_num;
                foreach (Tracerdb.Tb_ind_phone p in phones)
                {
                    if (p.Maint_dt > old_phone.Maint_dt)
                        old_phone = p;
                }

                return old_phone.Area_code + old_phone.Phone_num;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return "";
            }
        }
        public static string getBirthPlace(Tracerdb.INDIVIDUAL ind)
        {
            if (ind.POB_ST_CNTRY_CD == "S")
            {
                try
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.STATES st = (from s in db.STATES
                                          where s.STATE_CD == ind.POB_ST_CNTRY
                                          select s).Single();
                    return st.STATES1.Trim();
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    return "";
                }
            }
            if (ind.POB_ST_CNTRY_CD == "C")
            {
                try
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.TBL_CNTRY cntry = (from c in db.TBL_CNTRY
                                                where c.CNTRY_KEY == ind.POB_ST_CNTRY
                                                select c).Single();
                    return cntry.CNTRY_NAME;
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    return "";
                }
            }
            return "";
        }
        //public static  void handleAwards(int id_num,renewalPA r)
        //{
        //    try
        //    {
        //        string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
        //        Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
        //        List<Tracerdb.ProfileAwardHonor> awards = (from a in db.ProfileAwardHonor
        //                                                   where a.ID_NUM == id_num
        //                                                   select a).ToList<Tracerdb.ProfileAwardHonor>();

        //        if (awards.Count > 0) r.Award1 = awards[0].Description;
        //        else r.Award1 = "";
        //        if (awards.Count > 1) r.Award2 = awards[1].Description;
        //        else r.Award2 = "";
        //        if (awards.Count > 2) r.Award3 = awards[2].Description;
        //        else r.Award3 = "";
        //        if (awards.Count > 3) r.Award4 = awards[3].Description;
        //        else r.Award4 = "";
        //        if (awards.Count > 4) r.Award5 = awards[4].Description;
        //        else r.Award5 = "";
        //    }
        //    catch (Exception e)
        //    {
        //        string s = e.Message;
        //    }
        //}
        public static string getPState(Tracerdb.INDIVIDUAL ind)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.TBL_CNTRY cntry = (from c in db.TBL_CNTRY
                                            where c.CNTRY_KEY == ind.PST
                                            select c).SingleOrDefault<Tracerdb.TBL_CNTRY>();

                return cntry.CNTRY_NAME;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return "";
            }
        }
        public static string getCountry(Tracerdb.INDIVIDUAL ind)
        {
            if (ind.MZIP != "")
            {
                return "USA";
            }
            else
            {
                try
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.TBL_CNTRY cntry = (from c in db.TBL_CNTRY
                                                where c.CNTRY_KEY == ind.MST
                                                select c).SingleOrDefault<Tracerdb.TBL_CNTRY>();

                    return cntry.CNTRY_NAME;
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    return "";
                }
            }
        }
        public static string getState(Tracerdb.INDIVIDUAL ind)
        {
            if (ind.MZIP != "")
            {
                return ind.MST;
            }
            else
            {
                try
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.TBL_CNTRY cntry = (from c in db.TBL_CNTRY
                                                where c.CNTRY_KEY == ind.MST
                                                select c).SingleOrDefault<Tracerdb.TBL_CNTRY>();

                    return cntry.CNTRY_NAME;
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    return "";
                }
            }
        }
        public static Tracerdb.ANNREG getANNREG(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.ANNREG ar = (from a in db.ANNREG
                                      where a.ID_NUM == id_num
                                      select a).SingleOrDefault<Tracerdb.ANNREG>();

                return ar;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
        }
        public static Tracerdb.NC getNC(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.NC nc = (from n in db.NC
                                  where n.ID_NUM == id_num
                                  select n).SingleOrDefault<Tracerdb.NC>();

                return nc;
            }
            catch (Exception e)
            {
                string s = e.Message;
                log(string.Format("Error in dataAccess.getNC: for {0}, {1}", id_num.ToString(), s), " ");
                return null;
            }
        }
        public static Tracerdb.INDIVIDUAL getIndividual(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                           where i.ID_NUM == id_num
                                           select i).SingleOrDefault<Tracerdb.INDIVIDUAL>();

                return ind;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
        }
        public static int getIDnumFromLicenseAC(string license)
        {

            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.ACUPUNCTURE ac = (from a in db.ACUPUNCTURE
                                           where
                                           a.AC_LIC_NUM == license
                                           select a).SingleOrDefault<Tracerdb.ACUPUNCTURE>();
                if (ac != null) return ac.ID_NUM ?? 0;
                else return 0;
            }
            catch (Exception e)
            {
                string s1 = e.Message;
                log(string.Format("license number: {0}", license) + e.Message, "app_code.getIDnumFromLicenseAC");
                return 0;
            }
        }
        public static Tracerdb.PA getPA(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.PA pa = (from p in db.PA
                                  where p.ID_NUM == id_num
                                  select p).SingleOrDefault<Tracerdb.PA>();

                return pa;
            }
            catch (Exception e)
            {
                string s = e.Message;
                log("error: getPA", e.Message);
                return null;
            }
        }
        public static Tracerdb.PARENEWAL getPAR(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.PARENEWAL par = (from p in db.PARENEWAL
                                          where p.ID_NUM == id_num
                                          select p).SingleOrDefault<Tracerdb.PARENEWAL>();

                return par;
            }
            catch (Exception e)
            {
                string s = e.Message;
                log("error: getPAR", e.Message);
                return null;
            }
        }
        //
        public static Tracerdb.ACUPUNCTURE getAC(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.ACUPUNCTURE ac = (from a in db.ACUPUNCTURE
                                           where a.ID_NUM == id_num
                                           select a).SingleOrDefault<Tracerdb.ACUPUNCTURE>();

                return ac;
            }
            catch (Exception e)
            {
                string s = e.Message;
                log("error: getAC", e.Message);
                return null;
            }
        }
        //
        public static bool getLackInfo(int id_num)
        {
            //ACUPUNCTURE.ID_NUM Not In (select id_num from ac_renew_lack 
            //where ac_renew_lack_dt is NULL or ac_renew_lack_dt = '')


            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                var acls = from a in db.AC_RENEW_LACK
                           where a.ID_NUM == id_num
                           select a;

                if (acls.Count() == 0) return false; //false is eligible
                foreach (Tracerdb.AC_RENEW_LACK acl in acls)
                {
                    if (acl.AC_RENEW_LACK_DT == null) return true; //not eligible                   
                }
                return false;
            }
            catch (Exception e)
            {
                string s = e.Message;
                log("error: getLackInfo", e.Message);
                return false; //
            }

        }
        
        //
        public static bool deleteCash(int cashid)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.CASH cash = (from c in db.CASH
                                      where c.CASHID == cashid
                                      select c).Single<Tracerdb.CASH>();
                db.CASH.DeleteOnSubmit(cash);
                db.SubmitChanges();
                return true;

            }
            catch (Exception e)
            {
                string s1 = e.Message;
                log(string.Format("cashid: {0}", cashid) + " " + e.Message, "deleteCash(int cashid)");
                return false;
            }
        }
        //

        public static renewDB.Tb_work_inprogress getWIP(string license_number)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_work_inprogress wip  = (from w in db.Tb_work_inprogress //    .GetTable<linqdba.Tb_order>()
                                        where w.License_num == license_number 
                                        select w).Single();
                return wip;
               
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "RClassLibrary DataAccess.getWIP(" + license_number  + ", " );
                return null;
            }
             
        }
        //
        public static bool dumpInboundReceiptWcf(TxGov1.TxGov.PaymentResult paymentResult, string token, int order_id, string appType)
        {

            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_inbound_receipt_log pr = new renewDB.Tb_inbound_receipt_log();
                pr.Failcode = paymentResult.FAILCODE ?? "N";
                pr.Failmessage = paymentResult.FAILMESSAGE;
                pr.Token = token;
                pr.Address1 = paymentResult.ADDRESS1;
                pr.Address2 = paymentResult.ADDRESS2;
                pr.Authcode = paymentResult.AUTHCODE;
                pr.Avsresponse = paymentResult.AVSResponse;
                pr.Billingname = paymentResult.BillingName;
                pr.City = paymentResult.CITY;
                pr.Country = paymentResult.COUNTRY;
                if (paymentResult.CreditCardType != null)
                {
                    if (paymentResult.CreditCardType.Length > 4) pr.Creditcardtype = paymentResult.CreditCardType.Substring(0, 4);
                    else pr.Creditcardtype = paymentResult.CreditCardType;
                }
                pr.Cvvresponse = paymentResult.CVVResponse;
                if (paymentResult.EMAIL == null)
                    pr.Email = "";
                else
                    pr.Email = paymentResult.EMAIL;
                pr.Email1 = paymentResult.EMAIL1;
                pr.Email2 = paymentResult.EMAIL2;
                pr.Email3 = paymentResult.EMAIL3;
                if (paymentResult.ExpirationDate != DateTime.MinValue) pr.Expirationdate = paymentResult.ExpirationDate;
                pr.Cardnumber = paymentResult.LAST4NUMBER;
                pr.Localrefid = paymentResult.LOCALREFID;
                pr.Name = paymentResult.NAME;
                pr.Orderid = paymentResult.ORDERID.ToString();
                pr.Paytype = paymentResult.PAYTYPE;
                pr.Phone = paymentResult.PHONE;
                pr.Fax = paymentResult.FAX;
                pr.Receiptdate = paymentResult.RECEIPTDATE;
                pr.Receipttime = paymentResult.RECEIPTTIME;
                pr.State = paymentResult.STATE;
                pr.Totalamount = paymentResult.TOTALAMOUNT;
                pr.Zip = paymentResult.ZIP;
                pr.Create_dt = DateTime.Now;
                pr.Create_user = appType;
                pr.Maint_dt = DateTime.Now;
                pr.Maint_user = appType;
                if (paymentResult.ADDRESS2 != null) pr.Address2 = paymentResult.ADDRESS2;
                else pr.Address2 = "";
                if (paymentResult.FAX != null) pr.Fax = paymentResult.FAX;
                else pr.Fax = "";
                //

                db.Tb_inbound_receipt_log.InsertOnSubmit(pr);
                db.SubmitChanges();


                // also create tb_receivable
                renewDB.Tb_receivable rec = new renewDB.Tb_receivable
                {
                    //get our order_id not nic's, nic's order_id is in inbound record                      
                    Order_id = order_id, //(int)result.ORDERID,                    
                    Localrefid = paymentResult.LOCALREFID,
                    Payment_type_cd = paymentResult.PAYTYPE,
                    Uniquetransid = token,
                    Receipt_dt = DateTime.Parse(paymentResult.RECEIPTDATE),
                    Receipt_amount = decimal.Parse(paymentResult.TOTALAMOUNT),
                    Create_dt = DateTime.Now,
                    Create_user = appType,
                    Maint_dt = DateTime.Now,
                    Maint_user = appType
                };

                db.Tb_receivable.InsertOnSubmit(rec);
                db.SubmitChanges();
                //
                return true; //normal                    
            }
            catch (Exception e)
            {
                //this doesn't necessarily mean that the trans fails
                log("Error: " + e.Message, "RClassLibrary.DataAccess.dumpInboundReceipt");
                return false;
            }
        }
        //

        public static bool dumpInboundReceipt(NICUSA.commonCheckOut.PaymentResult paymentResult, string token, int order_id, string appType)
        {
            
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_inbound_receipt_log pr = new renewDB.Tb_inbound_receipt_log();
                pr.Failcode = paymentResult.FAILCODE ?? "N";
                pr.Failmessage = paymentResult.FAILMESSAGE;
                pr.Token = token;
                pr.Address1 = paymentResult.ADDRESS1;
                pr.Address2 = paymentResult.ADDRESS2;
                pr.Authcode = paymentResult.AUTHCODE;
                pr.Avsresponse = paymentResult.AVSResponse;
                pr.Billingname = paymentResult.BillingName;
                pr.City = paymentResult.CITY;
                pr.Country = paymentResult.COUNTRY;
                if (paymentResult.CreditCardType != null)
                {
                    if (paymentResult.CreditCardType.Length > 4) pr.Creditcardtype = paymentResult.CreditCardType.Substring(0, 4);
                    else pr.Creditcardtype = paymentResult.CreditCardType;
                }
                pr.Cvvresponse = paymentResult.CVVResponse;
                if (paymentResult.EMAIL == null)
                    pr.Email = "";
                else
                    pr.Email = paymentResult.EMAIL;
                pr.Email1 = paymentResult.EMAIL1;
                pr.Email2 = paymentResult.EMAIL2;
                pr.Email3 = paymentResult.EMAIL3;
                if (paymentResult.ExpirationDate != DateTime.MinValue) pr.Expirationdate = paymentResult.ExpirationDate;
                pr.Cardnumber = paymentResult.LAST4NUMBER;
                pr.Localrefid = paymentResult.LOCALREFID;
                pr.Name = paymentResult.NAME;
                pr.Orderid = paymentResult.ORDERID.ToString();
                pr.Paytype = paymentResult.PAYTYPE;
                pr.Phone = paymentResult.PHONE;
                pr.Fax = paymentResult.FAX;
                pr.Receiptdate = paymentResult.RECEIPTDATE;
                pr.Receipttime = paymentResult.RECEIPTTIME;
                pr.State = paymentResult.STATE;
                pr.Totalamount = paymentResult.TOTALAMOUNT;
                pr.Zip = paymentResult.ZIP;
                pr.Create_dt = DateTime.Now;
                pr.Create_user = appType ;
                pr.Maint_dt = DateTime.Now;
                pr.Maint_user = appType;
                if (paymentResult.ADDRESS2 != null) pr.Address2 = paymentResult.ADDRESS2;
                else pr.Address2 = "";
                if (paymentResult.FAX != null) pr.Fax = paymentResult.FAX;
                else pr.Fax = "";
                //

                db.Tb_inbound_receipt_log.InsertOnSubmit(pr);
                db.SubmitChanges();


                // also create tb_receivable
                renewDB.Tb_receivable rec = new renewDB.Tb_receivable
                {
                    //get our order_id not nic's, nic's order_id is in inbound record                      
                    Order_id = order_id, //(int)result.ORDERID,                    
                    Localrefid = paymentResult.LOCALREFID,
                    Payment_type_cd = paymentResult.PAYTYPE,
                    Uniquetransid = token,
                    Receipt_dt = DateTime.Parse(paymentResult.RECEIPTDATE),
                    Receipt_amount = decimal.Parse(paymentResult.TOTALAMOUNT),
                    Create_dt = DateTime.Now,
                    Create_user = appType,
                    Maint_dt = DateTime.Now,
                    Maint_user = appType 
                };

                db.Tb_receivable.InsertOnSubmit(rec);
                db.SubmitChanges();
                //
                return true; //normal                    
            }
            catch (Exception e)
            {
                //this doesn't necessarily mean that the trans fails
                log("Error: " + e.Message, "phyAppCL.DataAccess.dumpInboundReceipt");               
                return false;
            }
        }
        //
        public static List<TxGov1.revenue> getWcfCodeBlock2(renewDB.Tb_revenue rv, decimal fee)
        {
            List<TxGov1.revenue> codingBlock = new List<TxGov1.revenue>();
            try
            {
                List<TxGov1.revenue> rvs = new List<TxGov1.revenue>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                IQueryable<renewDB.Tb_revenue_detail> rcs = from r in db.Tb_revenue_detail
                                                           where r.Fee_nbr == rv.Fee_nbr && r.Revenue_code == rv.Revenue_code
                                                           select r;

                foreach (renewDB.Tb_revenue_detail rc in rcs)
                {
                    TxGov1.revenue r = new TxGov1.revenue();
                    r.aobj = rc.Aobj;
                    r.begin_dt = rv.Begin_dt;
                    r.cobj = rc.Cobj;
                    r.create_dt = rc.Create_dt;
                    r.create_user = rc.Create_user;
                    r.end_dt = rv.End_dt;
                    r.fee_amt = rc.Fee_amt;
                    //r.fee_amt = fee;
                    r.fee_nbr = rc.Fee_nbr;
                    r.fund = rc.Fund;
                    r.maint_dt = rc.Maint_dt;
                    r.maint_user = rc.Maint_user;
                    r.pca = rc.Pca;
                    r.revenue_code = rc.Revenue_code;
                    r.revenue_desc = rv.Revenue_desc;
                    r.trans_code = rc.Trans_code;

                    rvs.Add(r);

                }
                return rvs;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, utilities.getLogInfo());
                return null;
            }
        }
        //
        public static renewDB.Tb_revenue getWcfRevenue(decimal fee, string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var rvs = from r in db.Tb_revenue
                          where r.Total_fee == fee && r.Revenue_code == revenue_code
                          select r;


                DateTime dt = DateTime.Now;
                foreach (renewDB.Tb_revenue r in rvs)
                {
                    if (r.End_dt == null)
                    {
                        if (dt > r.Begin_dt)
                        {
                            return r;
                        }
                    }
                    else
                    {
                        if ((dt > r.Begin_dt) && (dt < r.End_dt))
                        {
                            return r;
                        }
                    }
                }

                return null; //this is bad
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, utilities.getLogInfo());
                return null;
            }
        }
        //

        public static IEnumerable<TxGov1.revenue> getWcfRevenueCodes2(decimal fee, string revenue_code)
        {
            try
            {
                renewDB.Tb_revenue rv = getWcfRevenue(fee, revenue_code);
                //return dbAccess.getCodeBlock2(rv);
                return getWcfCodeBlock2(rv, fee);
            }
            catch (Exception e)
            {
                log("Error, getRevenueCodes2(fee,revenue_code): " + e.Message, utilities.getLogInfo());
                return new List<TxGov1.revenue>();
            }
        }
        //
        public static renewDB.Tb_revenue getRevenueDB(string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var revs = from r in db.Tb_revenue
                           where r.Revenue_code == revenue_code
                           select r;
                foreach (renewDB.Tb_revenue r in revs)
                {
                    if ((r.Begin_dt < DateTime.Now) && (r.End_dt == null))
                        return r;
                }
                log("Error, getRevenueDB: revenue_code: " + revenue_code, utilities.getLogInfo());
                return new renewDB.Tb_revenue();
            }
            catch (Exception e)
            {
                log("Error, getRevenueDB(revenue_code): " + revenue_code  + e.Message, utilities.getLogInfo());
                return new renewDB.Tb_revenue();
            }
        }
        //
    }
}
