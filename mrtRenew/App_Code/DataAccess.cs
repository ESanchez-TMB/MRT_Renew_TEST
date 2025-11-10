using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.Web;

namespace mrtRenew.App_Code
{
    public class DataAccess
    {
        public static void insertOrderItemsWcf(List<TxGov1.orderItem> orderItems)
        {
            try
            {
                foreach (TxGov1.orderItem item in orderItems)
                {
                    insertOrderItem(item);
                }
            }
            catch { } // do nothing
        }
        //
        public static void insertOrderItem(TxGov1.orderItem item)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                // 8_15 what was this code for???
                //if (orderItemID == 0)
                //{
                renewDB.Tb_order_item newOrdItem = new renewDB.Tb_order_item()
                {
                    Order_id = item.orderID,
                    Item_cd = item.item_cd,
                    Unit_price_amt = item.unitprice,
                    Quantity = item.quantity,
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl"
                };
                db.GetTable<renewDB.Tb_order_item>().InsertOnSubmit(newOrdItem);

                db.SubmitChanges();

            }
            catch (Exception e)
            {
                log(e.Message, "insertOrderItem MRT renew");
            }
        }
        //
        public static void insertOrderItems(List<NICUSA.orderItem> orderItems)
        {
            try
            {
                foreach (NICUSA.orderItem item in orderItems)
                {
                    insertOrderItem(item);
                }
            }
            catch { } // do nothing
        }
        //
        public static RClassLibrary.renewal getRrecord(string license, string ssn)
        {
            string mode = System.Configuration.ConfigurationManager.AppSettings["NICMode"];
            if (mode == "DEV") mode = "TEST";     
            AMS_LegacyOnlineInterface m_oAMS_LegacyOnlineInterface = new AMS_LegacyOnlineInterface(mode);

            RClassLibrary.renewal r = null;
            try
            {
                r = m_oAMS_LegacyOnlineInterface.AMS_GetRenewalInfo(license, ssn);
                return r;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "\n Source: " + e.Source + "\n lmpRenew.App_Code Method:" + e.TargetSite + "\n Contact the Helpdesk and give them the preceding information.");
                return null;
            }
                
            

        }
        //
        public static string putFINAL(RClassLibrary.renewal r)
        {
            try
            {

                //serialize
                var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                //
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.Tb_work_final wip = new renewDB.Tb_work_final
                {
                    License_num = r.LicNum,
                    Work_data = sw.ToString(),
                    Create_dt = DateTime.Now,
                    Maint_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_user = "mrtRnl"
                };
                //

                db.Tb_work_final.InsertOnSubmit(wip);
                db.SubmitChanges();
                return "success";
            }
            catch (Exception e)
            {
                DataAccess.log("Error: license: " + r.LicNum + " " + e.Message, "DataAccess.putFINAL");
                return e.Message;
            }



        } 
        //
        public static void insertOrderItem(NICUSA.orderItem item)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                // 8_15 what was this code for???
                //if (orderItemID == 0)
                //{
                renewDB.Tb_order_item newOrdItem = new renewDB.Tb_order_item()
                {
                    Order_id = item.orderID,
                    Item_cd = item.item_cd,
                    Unit_price_amt = item.unitprice,
                    Quantity = item.quantity,
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl"
                };
                db.GetTable<renewDB.Tb_order_item>().InsertOnSubmit(newOrdItem);

                db.SubmitChanges();

            }
            catch (Exception e)
            {
                log(e.Message, "insertOrderItem MRT renew");
            }
        }
        // ACH
        public static decimal getAch()
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Ts_globals glob = (from g in db.Ts_globals   //    .GetTable<applDB.Tb_order>()
                                           where g.Parm_group == "CONV_FEE" && g.Parm_key == "ACH"
                                           select g).Single();
                if (glob != null)
                {
                    return decimal.Parse(glob.Parm_value);
                }
                return 0.0m;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getAch");
                return 0.0m;
            }
        }
        //
        //the r record will be handled differently
        // it will exist only in memory until user purchases
        // then it will be written to disk and database
        //
        public static string findLackCodeInPaRenewLack(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                var lacks = from l in db.PA_RENEW_LACK
                            where l.ID_NUM == id_num
                            select l;
                foreach (Tracerdb.PA_RENEW_LACK l in lacks)
                {
                    if (l.PA_REN_LACK_DT == null) return l.PA_REN_LACK_CD;
                }
                return "";
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, string.Format("DataAcccess.findLackCodeInPaRenewLack({0})", id_num.ToString())); //, "DataAccess.getInsCompanies()");
                return "";
            }
        }
        //
        public static DateTime getACexpDate()
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.SYS_AC_LIC_PARAM p = (from s in db.SYS_AC_LIC_PARAM
                                               select s).Single<Tracerdb.SYS_AC_LIC_PARAM>();
                return p.RenewalExpDate ?? DateTime.MinValue;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAcccess.etACexpDate"); //, "DataAccess.getInsCompanies()");
                return DateTime.MinValue;
            }
        }
        public static string putRenewDB(RClassLibrary.renewal r, int orderID)
        {
            try
            {
                //get the order
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_order order = (from o in db.Tb_order
                                          where o.Order_id == orderID
                                          select o).SingleOrDefault<renewDB.Tb_order>();


                //serialize
                var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; database=ARO_DEV01; Integrated Security=SSPI;Persist Security Info=True;Connection Timeout=600;");

                renewDB.Tb_work_inprogress wip = new renewDB.Tb_work_inprogress
                {

                    Customer_id = order.Customer_id,
                    License_num = r.LicNum,
                    Work_data = sw.ToString(),
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl"
                };
                db.Tb_work_inprogress.InsertOnSubmit(wip);
                db.SubmitChanges();
                return "success";
            }
            catch (Exception e)
            {
                App_Code.DataAccess.log("Error: orderID: " + orderID.ToString() + " " + e.Message, "Utilities.putRenewDB");
                return e.Message;
            }
        }
        //
        public static List<renewDB.Tb_audit> getAudits4orderID(int orderID)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var audits = from a in db.Tb_audit
                             where a.Order_id == orderID
                             select a;
                return audits.ToList<renewDB.Tb_audit>();
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAcccess.getAudits4orderID"); //, "DataAccess.getInsCompanies()");
                return new List<renewDB.Tb_audit>();
            }
        }

        public static List<cust_order> getCOcustID(int customer_id)
        {
            {
                try
                {
                    renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                    var customers = from c in db.Tb_customer
                                    from order in db.Tb_order
                                    where (c.Customer_id == order.Customer_id) && (c.Customer_id == customer_id)
                                    select new { c.Customer_id, c.Id_num, c.Last_name, c.First_name, order.Order_id, order.Order_status_cd };

                    List<cust_order> c_os = new List<cust_order>();
                    foreach (var c1 in customers)
                    {
                        cust_order c_o = new cust_order
                        {
                            customer_id = c1.Customer_id,
                            id_num = (int)c1.Id_num,
                            last_name = c1.Last_name,
                            first_name = c1.First_name,
                            order_id = c1.Order_id,
                            order_status_cd = c1.Order_status_cd
                        };
                        c_os.Add(c_o);
                    }
                    return c_os;
                }
                catch (Exception e)
                {
                    log("Error: " + e.Message, "DataAcccess.getCOcustID"); //, "DataAccess.getInsCompanies()");
                    return new List<cust_order>();
                }
            }
        }

        public static string getLast4PA(string s) //s is license #
        {
            try
            {
                int id_num = getIDnumFromLicensePA(s);
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                var inds = from i in db.INDIVIDUAL
                           where i.ID_NUM == id_num
                           select i;

                if (inds.Count<Tracerdb.INDIVIDUAL>() != 1) return "";
                Tracerdb.INDIVIDUAL ind = inds.First<Tracerdb.INDIVIDUAL>();
                if (ind.SSN.Length != 9) return "";
                return ind.SSN.Substring(5, 4);

            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAcccess.getLast4"); //, "DataAccess.getInsCompanies()");
                return "";
            }

        }
        public static List<malpractice> getMalpractice(int id_num)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                var complaints = db.GetTable<Tracerdb.COMPLAINT>();
                var map = db.GetTable<Tracerdb.INV_MAP>();

                var query = from c in complaints
                            from m in map
                            where (m.Inv_log_num == c.INV_LOG_NUM && c.ID_NUM == id_num && (m.INV_MPA_CD == "3.08(20)(B)" || m.INV_MPA_CD == "3.08(20)" || m.INV_MPA_CD == "3.08(20)b" || m.INV_MPA_CD == "164.051(a)(8)(B)" || m.INV_MPA_CD == "164.051(a)(8)") && c.ENF_DISP_CD != "760.0" && c.ENF_DISP_CD != null)
                            select new { c.ENF_DISP_DT, c.ENF_DISP_CD };
                List<App_Code.malpractice> mpl = new List<App_Code.malpractice>();
                if (query != null)
                {
                    foreach (var cm in query)
                    {
                        App_Code.malpractice mp = new App_Code.malpractice();
                        if (App_Code.Utilities.isDecimal(cm.ENF_DISP_CD))
                        {
                            decimal cd = decimal.Parse(cm.ENF_DISP_CD);
                            if (((cd > 601.0m) && (cd <= 750.0m)) || (cd == 408.0m) || ((cd > 8001.0m) && (cd <= 10527.0m)))
                            {
                                mp.Action = "Dismissed";
                                mp.Enf_Disp_DT = cm.ENF_DISP_DT ?? DateTime.MinValue;
                            }
                            else
                            {
                                mp.Action = "Action Taken-See TMB Actions & License Restrictions Section";
                                mp.Enf_Disp_DT = cm.ENF_DISP_DT ?? DateTime.MinValue;
                            }
                        }
                        mpl.Add(mp);
                    }

                }
                return mpl;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAcccess.getMalpractice"); //, "DataAccess.getInsCompanies()");
                return null;
            }
        }
        //
        public static string getLicenseFromIDnum(int id_num)
        {
            string license_nums = "";
            int count = 0;
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.NC nc = (from n in db.NC
                                  where
                                  n.ID_NUM == id_num
                                  select n).SingleOrDefault<Tracerdb.NC>();
                if (nc != null)
                {
                    license_nums = nc.NC_PRM_NUM;
                    count++;
                }
                //
                Tracerdb.ACUPUNCTURE ac = (from a in db.ACUPUNCTURE
                                           where
                                          a.ID_NUM == id_num
                                           select a).SingleOrDefault<Tracerdb.ACUPUNCTURE>();
                if (ac != null)
                {
                    if (count > 0) license_nums = " " + ac.AC_LIC_NUM;
                    else license_nums = ac.AC_LIC_NUM;
                }
                //
                Tracerdb.PA pa = (from p in db.PA
                                  where
                                  p.ID_NUM == id_num
                                  select p).SingleOrDefault<Tracerdb.PA>();
                if (pa != null)
                {
                    if (count > 0) license_nums = " " + pa.PA_LIC_NUM;
                    else license_nums = pa.PA_LIC_NUM;
                }
                return license_nums;

            }
            catch (Exception e)
            {
                string s1 = e.Message;
                log(string.Format("id_num number: {0}", id_num.ToString()) + e.Message, "app_code.getLicenseFromIDnum");
                return "";
            }
        }
        //
        public static int getIDnumFromLicenseNC(string license)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.NC nc = (from n in db.NC
                                  where
                                  n.NC_PRM_NUM == license
                                  select n).SingleOrDefault<Tracerdb.NC>();
                if (nc != null) return nc.ID_NUM ?? 0;
                else return 0;
            }
            catch (Exception e)
            {
                string s1 = e.Message;
                log(string.Format("license number: {0}", license) + e.Message, "app_code.getIDnumFromLicenseNC");
                return 0;
            }
        }
        //
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
        public static int getIDnumFromLicensePA(string license)
        {
            //return RClassLibrary.DataAccess.getIDnumFromLicense(license);
            //PA licenses r not stored in the Individual record 
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.PA pa = (from p in db.PA
                                  where
                                  p.PA_LIC_NUM == license
                                  select p).SingleOrDefault<Tracerdb.PA>();
                if (pa != null) return pa.ID_NUM ?? 0;
                else return 0;
            }
            catch (Exception e)
            {
                string s1 = e.Message;
                log(string.Format("license number: {0}", license) + e.Message, "app_code.getIDnumFromLicensePA");
                return 0;
            }
        }
        //public static Dictionary<string, string> getCountries()
        //{
        //    return renewClassLib.DataAccess.getCountries();
        //}

        public static Tracerdb.ANNREG getAnnreg(int id_num) //public static string getText4Type(string LicenseType)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.ANNREG annreg = (from a in db.ANNREG
                                          where a.ID_NUM == id_num
                                          select a).SingleOrDefault<Tracerdb.ANNREG>();
                return annreg;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAcccess.getAnnreg"); //, "DataAccess.getInsCompanies()");
                return null;
            }
        }
        public static string getText4Type(string LicenseType)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.Ts_com_code_value ccv = (from c in db.Ts_com_code_value
                                                  where c.Code_value == LicenseType && c.Code_category == "license_category_subtype\\PHY"
                                                  select c).SingleOrDefault<Tracerdb.Ts_com_code_value>();

                return ccv.Code_desc;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getText4Type"); //, "DataAccess.getInsCompanies()");
                return "";
            }
        }
        public static Dictionary<string, string> getInsCo2() //wow, 1--13-11
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.INS_COMP_TABLE> ins = from i in db.INS_COMP_TABLE
                                                          select i;
                Dictionary<string, string> insList = new Dictionary<string, string>();
                foreach (Tracerdb.INS_COMP_TABLE i in ins)
                {
                    insList.Add(i.ML_INS_COMP_CD, i.ML_INS_COMP_NAME);
                }

                return insList;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getInsCo"); //, "DataAccess.getInsCompanies()");
                return null;
            }
        }

        public static Dictionary<string, string> getInsCo()
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.Ts_com_code_value> ins = from i in db.Ts_com_code_value
                                                             where i.Code_category == "insurance_company"
                                                             select i;
                Dictionary<string, string> insList = new Dictionary<string, string>();
                foreach (Tracerdb.Ts_com_code_value i in ins)
                {
                    insList.Add(i.Code_value, i.Code_desc);
                }

                return insList;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getInsCo"); //, "DataAccess.getInsCompanies()");
                return null;
            }
        }
        public static Dictionary<string, string> getInsCompanies()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.INS_COMP_TABLE> inss = from i in db.INS_COMP_TABLE
                                                           select i;

                // 104 Aetna Casualty & Surety
                //get rid of leading spaces,digits

                String matchpattern = @"^\d*\s\d*";
                String replacementpattern = @"";
                //Console.WriteLine(Regex.Replace(sourcestring, matchpattern, replacementpattern));


                foreach (Tracerdb.INS_COMP_TABLE ins in inss)
                {
                    string name = ins.ML_INS_COMP_NAME;
                    string nameOut = Regex.Replace(name, matchpattern, replacementpattern);
                    if (ins.ML_INS_COMP_CD != "0")
                        d.Add(ins.ML_INS_COMP_CD, nameOut.Trim() + ", " + ins.ML_INS_COMP_ADD1 + ", " + ins.ML_INS_COMP_CITY + ", " + ins.ML_INS_COMP_ST);
                }
                var sortedDict = (from entry in d orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                return sortedDict;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getInsCompanies"); //, "DataAccess.getInsCompanies()");
                return d;
            }
        }
        public static Dictionary<int, decimal> createFeeTable2()
        {
            //long story, tables r put together in irritating way
            //tb_revenue,fee numbers r unique except for stinking convenience fee fee_nbr of 0
            List<revenue> codingBlock = new List<revenue>();
            try
            {
                Dictionary<int, decimal> feeTable = new Dictionary<int, decimal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                IQueryable<renewDB.Tb_revenue> rcs = from r in db.Tb_revenue
                                                     select r;

                foreach (renewDB.Tb_revenue rc in rcs)
                {
                    if (rc.Fee_nbr != 0) feeTable.Add(rc.Fee_nbr, rc.Total_fee);
                }
                return feeTable;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.createFeeTable2()");
                return null;
            }
        }
        //public static Dictionary<int, decimal> createFeeTable()
        //{
        //    List<revenue> codingBlock = new List<revenue>();
        //    try
        //    {
        //        Dictionary<int, decimal> feeTable = new Dictionary<int, decimal>();
        //        decimal total = 0;
        //        int oldFeeNbr = 0; 
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
        //        IQueryable<renewDB.Tb_revenue_code> rcs = from r in db.Tb_revenue_code                                                          
        //                                                  select r;

        //        foreach (renewDB.Tb_revenue_code rc in rcs)
        //        {
        //            if (rc.Fee_nbr != oldFeeNbr)
        //            {
        //                if (oldFeeNbr != 0)
        //                    feeTable.Add(oldFeeNbr, total);
        //                total = 0;
        //                oldFeeNbr = rc.Fee_nbr;
        //            }
        //            total = total + rc.Fee_amt;
        //        }
        //        return feeTable;
        //    }
        //    catch (Exception e)
        //    {
        //        log("Error: " + e.Message, "DataAccess.createFeeTable()");
        //        return null;
        //    }
        //}
        public static List<revenue_master> getRevenues()
        {
            try
            {
                List<revenue_master> rs = new List<revenue_master>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                IQueryable<renewDB.Tb_revenue> rvs = from r in db.Tb_revenue
                                                     select r;

                foreach (renewDB.Tb_revenue rv in rvs)
                {
                    revenue_master r = new revenue_master();
                    r.begin_dt = rv.Begin_dt;
                    r.end_dt = rv.End_dt;
                    r.create_dt = rv.Create_dt;
                    r.create_user = rv.Create_user;
                    r.fee_nbr = rv.Fee_nbr;
                    r.maint_dt = rv.Maint_dt;
                    r.maint_user = rv.Maint_user;
                    r.revenue_code = rv.Revenue_code;
                    r.revenue_desc = rv.Revenue_desc;
                    r.total_fee = rv.Total_fee;
                    rs.Add(r);
                }
                return rs;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getRevenues()");
                return null;
            }
        }
        public static List<revenue> getCodeBlock2(renewDB.Tb_revenue rv)
        {
            List<revenue> codingBlock = new List<revenue>();
            try
            {
                List<revenue> rvs = new List<revenue>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                IQueryable<renewDB.Tb_revenue_detail> rcs = from r in db.Tb_revenue_detail
                                                            where r.Fee_nbr == rv.Fee_nbr && r.Revenue_code == rv.Revenue_code
                                                            select r;

                foreach (renewDB.Tb_revenue_detail rc in rcs)
                {
                    revenue r = new revenue();
                    r.aobj = rc.Aobj;
                    r.begin_dt = rv.Begin_dt;
                    r.cobj = rc.Cobj;
                    r.create_dt = rc.Create_dt;
                    r.create_user = rc.Create_user;
                    r.end_dt = rv.End_dt;
                    r.fee_amt = rc.Fee_amt;
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
                log("Error: " + e.Message, "DataAccess.getcodeBlock()");
                return null;
            }
        }
        public static renewDB.Tb_revenue getRevenue(decimal fee, string revenue_code)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_revenue rv = (from r in db.Tb_revenue
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
        public static renewDB.Tb_revenue getRevenue(int fee_nbr)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_revenue rv = (from r in db.Tb_revenue
                                         where r.Fee_nbr == fee_nbr
                                         select r).Single<renewDB.Tb_revenue>();
                return rv;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getRevenue(int fee_nbr");
                return null;
            }
        }
        //public static List<revenue> getCodeBlock(int feeNumber)
        //{
        //    List<revenue> codingBlock = new List<revenue>();
        //    try
        //    {
        //        List <revenue > rvs = new List<revenue>();
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
        //        IQueryable <renewDB.Tb_revenue_code>  rcs = from r in db.Tb_revenue_code 
        //            where r.Fee_nbr == feeNumber 
        //            select r;

        //        foreach (renewDB .Tb_revenue_code  rc in rcs)
        //        {
        //            revenue r = new revenue();
        //            r.aobj = rc.Aobj;
        //            r.begin_dt = rc.Begin_dt;
        //            r.cobj = rc.Cobj;
        //            r.create_dt = rc.Create_dt;
        //            r.create_user = rc.Create_user;
        //            r.end_dt = rc.End_dt;
        //            r.fee_amt = rc.Fee_amt;
        //            r.fee_nbr = rc.Fee_nbr;
        //            r.fund = rc.Fund;
        //            r.maint_dt = rc.Maint_dt;
        //            r.maint_user = rc.Maint_user;
        //            r.pca = rc.Pca;
        //            r.revenue_code = rc.Revenue_code;
        //            r.revenue_desc = rc.Revenue_desc;
        //            r.trans_code = rc.Trans_code;                   

        //            rvs.Add(r);

        //        }
        //        return rvs;
        //    }
        //    catch (Exception e)
        //    {
        //        log("Error: " + e.Message, "DataAccess.getcodeBlock()");
        //        return null;
        //    }
        //}
        public static Dictionary<string, string> getCountries()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.TBL_CNTRY> cntrys = from c in db.TBL_CNTRY
                                                        orderby c.CNTRY_NAME
                                                        select c;

                foreach (Tracerdb.TBL_CNTRY cntry in cntrys)
                {
                    //d.Add(cntry.CNTRY_NAME, cntry.CNTRY_CD);
                    d.Add(cntry.CNTRY_NAME, cntry.CNTRY_KEY);
                }
                return d;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getCountries()");
                return d;
            }
        }
        //public static Dictionary<string, string> getCertDictionary()
        //{
        //    return RClassLibrary.DataAccess.getCerts();
        //}
        public static void log(string msg, string user)
        {
            RClassLibrary.DataAccess.log(msg, user);
        }
        public static void updatePPwithResult(string token, string error_msg, string orderID)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_outbound_payment_log pp = (from p in db.Tb_outbound_payment_log
                                                      where p.Order_id == int.Parse(orderID)
                                                      select p).Single<renewDB.Tb_outbound_payment_log>();
                if (token != null) pp.Token = token;
                else pp.Token = "";
                if (error_msg != null) pp.Err_msg = error_msg;
                else pp.Err_msg = "";
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.UpdatePPwithResult");
            }

        }// getPPbyOrderID
        //public static void updateOrderStatus(int orderID, int status)
        //{
        //    try
        //    {
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

        //        renewDB.Tb_order ord = (from o in db.Tb_order //    .GetTable<linqdba.Tb_order>()
        //                                where o.Order_id == orderID
        //                                select o).Single();
        //        if (ord != null)
        //        {
        //            ord.Order_status_cd = NICorder.OrderStatuses[status]; //up to 5 chars
        //            db.SubmitChanges();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log("Error " + e.Message, "DataAccess.updateOrderStatus(" + orderID.ToString() + ", " + status.ToString());
        //    }
        //}
        public static RClassLibrary.renewal getRenewalByToken(string token)
        {
            try
            {
                renewDB.Tb_order order = getOrderByToken(token);
                if (order == null) return null;
                renewDB.Tb_customer customer = getCustomerByID(order.Customer_id);
                if (customer == null) return null;
                int id_num = customer.Id_num ?? 0;
                if (id_num == 0) return null;
                Tracerdb.INDIVIDUAL individual = getIndividualByID(id_num);
                if (individual == null) return null;
                string s = individual.LICENSE_NUM;
                //RClassLibrary.renewal r = Utilities.getRenew(s);
                //RClassLibrary.renewal r = Utilities.getRenewXML(s);
                RClassLibrary.renewal r = App_Code.Utilities.getWIPrenewal(s);
                return r;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getRenewalByToken");
                return null;
            }
        }//getRenewalByToken(token);
        
        //
        public static Tracerdb.INDIVIDUAL getIndividualByID(int id_num)
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
                log("Error " + e.Message, "DataAccess.getIndividualByID(id_num)");
                return null;
            }
        }//getIndividualByID(int id_num)

        public static renewDB.Tb_customer getCustomerByID(int customerID)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.Tb_customer customer = (from c in db.Tb_customer
                                                where c.Customer_id == customerID
                                                select c).SingleOrDefault();
                return customer;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getCustomerByID(customerID)");
                return null;
            }
        }//getCustomerByID(int customerID)
        public static renewDB.Tb_order getOrderByID(string orderId)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                //create order 


                renewDB.Tb_order ord = (from o in db.Tb_order
                                        where o.Order_id == int.Parse(orderId)
                                        select o).SingleOrDefault();
                return ord;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getOrderByID(token)");
                return null;
            }
        }
        public static renewDB.Tb_order getOrderByToken(string token)
        {// is this really the best way to do this?
            //there might not be a successful outbound payment log
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                //create order 

                renewDB.Tb_outbound_payment_log lg = (from l in db.Tb_outbound_payment_log  //    .GetTable<renewDB.Tb_order>()
                                                      where l.Token == token
                                                      select l).SingleOrDefault();
                renewDB.Tb_order ord = (from o in db.Tb_order
                                        where o.Order_id == lg.Order_id
                                        select o).SingleOrDefault();
                return ord;
            }
            catch (Exception e)
            {
                log("Error " + e.Message, "DataAccess.getOrderByToken(token)");
                return null;
            }
        }
        public static int getOrderIDbyToken(string token)
        {// is this really the best way to do this?
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
                log("Error " + e.Message, "DataAccess.getOrderIDby(token)");
                return 0;
            }
        }
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
                    Create_user = "mrtRnl"
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
        public static renewDB.Tb_customer getCustomer(RClassLibrary .renewal r)
        {
            string connectInfo = System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"];
            renewDB.ARO_DEV01 db2 = new renewDB.ARO_DEV01(connectInfo);
            try
            {
                //has to be re-written because this person should exist in AMS database to which I have no direct
                //link 

                renewDB.Tb_customer customer = (from c in db2.Tb_customer
                                                where c.Id_num == r.id_num
                                                select c).Single<renewDB.Tb_customer>();
                if (customer != null)
                {
                    return customer;
                }
            }
            catch
            {
                renewDB.Tb_customer cust = new renewDB.Tb_customer();
                cust.UserId = new System.Guid("00000000-0000-0000-0000-000000000000"); //don't care
                cust.Create_dt = DateTime.Now;
                cust.Create_user = "mrtRnl";
                string[] Name = r.PAname.Split(' ');
                cust.First_name = Name[0];
                cust.Last_name = Name.Last();

                cust.Id_num = r.id_num;

                cust.Maint_dt = DateTime.Now;
                cust.Maint_user = "mrtRnl";
                db2.Tb_customer.InsertOnSubmit(cust);
                try
                {
                    db2.SubmitChanges();
                    return cust;
                }
                catch (Exception e)
                {
                    log(string .Format ("Error, could not creat customer record for {0}", r.id_num .ToString ()), " getCustomer()");
                    return null;
                }
                 
            }
            return null; // will never get here, stupid compiler
                    
               
            
        }
        //public static usasCodes getUsasInfo2(string revenue_code)
        //{
        //    try
        //    {
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

        //        renewDB.Tb_revenue_detail rev = (from r in db.Tb_revenue_detail
        //                                         where r.Revenue_code == revenue_code
        //                                         select r).SingleOrDefault<renewDB.Tb_revenue_detail>();
        //        if (rev != null)
        //        {
        //            usasCodes u = new usasCodes();
        //            u.RevenueCode = rev.Revenue_code;
        //            u.fund = rev.Fund;
        //            u.aobj = rev.Aobj;
        //            u.cobj = rev.Cobj;
        //            u.pca = rev.Pca;
        //            u.transCode = rev.Trans_code;
        //            return u;
        //        }
        //        else return new usasCodes();
        //    }
        //    catch (Exception e)
        //    {
        //        log(Utilities.errMsg(e), "DataAccess.getUsasInfo2");
        //        return new usasCodes();
        //    }

        //}//getUsasInfo
        //public static usasCodes getUsasInfo(string s)
        //{
        //    try
        //    {
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

        //        renewDB.Tb_revenue_code rev = (from r in db.Tb_revenue_code
        //                                       where r.Revenue_code == s
        //                                       select r).SingleOrDefault<renewDB.Tb_revenue_code>();
        //        if (rev != null)
        //        {
        //            usasCodes u = new usasCodes();
        //            u.RevenueCode = rev.Revenue_code;
        //            u.fund = rev.Fund;
        //            u.aobj = rev.Aobj;
        //            u.cobj = rev.Cobj;
        //            u.pca = rev.Pca;
        //            u.transCode = rev.Trans_code;
        //            return u;
        //        }
        //        else return new usasCodes();
        //    }
        //    catch (Exception e)
        //    {
        //        log(Utilities.errMsg(e), "DataAccess.getUsasInfo");
        //        return new usasCodes();
        //    }

        //}//getUsasInfo
        //
        //public static bool dumpInboundReceipt(NICqueryResult r)
        //{
        //    try
        //    {
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
        //        renewDB.Tb_inbound_receipt_log pr = new renewDB.Tb_inbound_receipt_log();
        //        pr.Failcode = r.FAILCODE ?? "N";
        //        pr.Failmessage = r.FAILMESSAGE;
        //        pr.Token = r.token;
        //        pr.Address1 = r.ADDRESS1;
        //        pr.Address2 = r.ADDRESS2;
        //        pr.Authcode = r.AUTHCODE;
        //        pr.Avsresponse = r.AVSResponse;
        //        pr.Billingname = r.BillingName;
        //        pr.City = r.CITY;
        //        pr.Country = r.COUNTRY;
        //        if (r.CreditCardType != null)
        //        {
        //            if (r.CreditCardType.Length > 4) pr.Creditcardtype = r.CreditCardType.Substring(0, 4);
        //            else pr.Creditcardtype = r.CreditCardType;
        //        }
        //        pr.Cvvresponse = r.CVVResponse;
        //        pr.Email = r.EMAIL;
        //        pr.Email1 = r.EMAIL1;
        //        pr.Email2 = r.EMAIL2;
        //        pr.Email3 = r.EMAIL3;
        //        if (r.ExpirationDate != DateTime.MinValue) pr.Expirationdate = r.ExpirationDate;
        //        pr.Cardnumber = r.LAST4NUMBER;
        //        pr.Localrefid = r.LOCALREFID;
        //        pr.Name = r.NAME;
        //        pr.Orderid = r.ORDERID.ToString();
        //        pr.Paytype = r.PAYTYPE;
        //        pr.Phone = r.PHONE;
        //        pr.Fax = r.FAX;
        //        pr.Receiptdate = r.RECEIPTDATE;
        //        pr.Receipttime = r.RECEIPTTIME;
        //        pr.State = r.STATE;
        //        pr.Totalamount = r.TOTALAMOUNT;
        //        pr.Zip = r.ZIP;
        //        pr.Create_dt = DateTime.Now;
        //        pr.Create_user = "renewPHY";
        //        pr.Maint_dt = DateTime.Now;
        //        pr.Maint_user = "renewPHY";
        //        if (r.ADDRESS2 != null) pr.Address2 = r.ADDRESS2;
        //        else pr.Address2 = "";
        //        if (r.FAX != null) pr.Fax = r.FAX;
        //        else pr.Fax = "";
        //        //

        //        db.Tb_inbound_receipt_log.InsertOnSubmit(pr);
        //        db.SubmitChanges();


        //        // also create tb_receivable
        //        renewDB.Tb_receivable rec = new renewDB.Tb_receivable
        //        {
        //            //get our order_id not nic's, nic's order_id is in inbound record                      
        //            Order_id = r.orderId, //(int)result.ORDERID,                    
        //            Localrefid = r.LOCALREFID,
        //            Payment_type_cd = r.PAYTYPE,
        //            Uniquetransid = r.token,
        //            Receipt_dt = DateTime.Parse(r.RECEIPTDATE),
        //            Receipt_amount = decimal.Parse(r.TOTALAMOUNT),
        //            Create_dt = DateTime.Now,
        //            Create_user = "renewPHY",
        //            Maint_dt = DateTime.Now,
        //            Maint_user = "renewPHY"
        //        };

        //        db.Tb_receivable.InsertOnSubmit(rec);
        //        db.SubmitChanges();
        //        //
        //        return true; //normal                    
        //    }
        //    catch (Exception e)
        //    {
        //        //this doesn't necessarily mean that the trans fails
        //        log("Error: " + e.Message, "DataAccess.dumpInboundReceipt");
        //        audit(r.orderId, "Error: dumpInboundReceipt " + e.Message, 1);
        //        return false;
        //    }
        //}

        public static int dumpPPInfo(
        string OrderID,
        string Merchantid,
        string Merchantkey,
        string Servicecode,
        string Amount,
        string Localrefid,
        string Uniquetransid,
        string Paytype,
        string Cid,
        string Statecd,
        string Name,
        string Companyname,
        string Phone,
        string Fax,
        string Address1,
        string Address2,
        string City,
        string State,
        string Zip,
        string Country,
        string Email,
        string Description,
        string Hrefcancel,
        string Hrefduplicate,
        string Hrefsuccess,
        string Hreffailure,
        string Cfee)
        {
            try
            {
                // dump request - can't display to console anymore - dump to db
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.Tb_outbound_payment_log pl = new renewDB.Tb_outbound_payment_log
                {
                    Order_id = int.Parse(OrderID),
                    Statecd = Statecd, //ord.statecd,
                    Name = Name, //ord.name,
                    Companyname = Companyname, //request.companyname,
                    Email = Email,//email,
                    Hrefcancel = Hrefcancel, //hrefcancel,
                    Hrefduplicate = Hrefduplicate, //hrefduplicate,
                    Hrefsuccess = Hrefsuccess,//hrefsuccess,
                    Hreffailure = Hreffailure,
                    Address1 = Address1,
                    Address2 = Address2,
                    City = City,
                    State = State,
                    Zip = Zip,
                    Country = Country,
                    Phone = Phone,
                    Fax = Fax,
                    Cid = Cid,
                    Paytype = Paytype,
                    Uniquetransid = Uniquetransid,
                    Description = Description,
                    Localrefid = Localrefid,
                    Merchantid = Merchantid,
                    Merchantkey = Merchantkey,
                    Servicecode = Servicecode,
                    Amount = decimal.Parse(Amount),
                    Conv_fee = decimal.Parse(Cfee),
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl"
                };
                db.Tb_outbound_payment_log.InsertOnSubmit(pl);
                db.SubmitChanges();
                return pl.Payment_log_id;
            }
            catch (Exception e)
            {
                log(e.Message, "DataAccess.dumpPPInfo");
                return 0;
            }
        }
        //dumpPPInfo
        public static void dumpPPlineItems(int pay_log_id,
                       int orderID,
                       string Description,
                       int ItemID,
                       int Quantitiy,
                       string SKU,
                       decimal Unitprice)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_opl_lineitem li = new renewDB.Tb_opl_lineitem
                {
                    Order_id = orderID,
                    Description = Description,
                    Itemid = ItemID,
                    Quantity = (short)Quantitiy,
                    Sku = SKU,
                    Unitprice = Unitprice,
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl",
                    Payment_log_id = pay_log_id
                };
                db.Tb_opl_lineitem.InsertOnSubmit(li);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                log(e.Message, "DataAccess.dumpPPlineItems");
            }
        }//dumpPPlineItems
        public static void dumpPPusas(
           int pay_log_id,
           string attributename,
           string attributevalue,
           int orderID)
        {
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_opl_lineitem_attribute lia = new renewDB.Tb_opl_lineitem_attribute
                {
                    Order_id = orderID,
                    Payment_log_id = pay_log_id,
                    Attributename = attributename,
                    Attributevalue = attributevalue,
                    Create_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_dt = DateTime.Now,
                    Maint_user = "mrtRnl"
                };
                db.Tb_opl_lineitem_attribute.InsertOnSubmit(lia);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                // audit
                log("Error: " + e.Message, "DataAccess.DumpPPusas");
            }
        }//dumpPPlineItems
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
        }// getPercentage
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
        }// getPercentage
        public static Dictionary<string, string> getCourts()
        {
            Dictionary<string, string> courts = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                var cs = from c in db.COURT_TABLE
                         select c;

                foreach (Tracerdb.COURT_TABLE c in cs)
                {
                    courts.Add(c.ML_COURT_DC, c.ML_COURT_CD);
                }
                return courts;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getcourts");
                return courts;
            }
        }
        public static Dictionary<string, string> getCounties()
        {
            Dictionary<string, string> counties = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                var cs = from c in db.Ts_com_code_value
                         where c.Code_category == "county"
                         select c;

                foreach (Tracerdb.Ts_com_code_value c in cs)
                {
                    counties.Add(c.Code_desc, c.Code_value);
                }
                return counties;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getcounties");
                return counties;
            }
        }
        public static Dictionary<int, decimal> getPAfees(string revenue_code)
        {
            Dictionary<int, decimal> paFees = new Dictionary<int, decimal>();
            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var fees = from r in db.Tb_revenue
                           where r.Revenue_code == revenue_code
                           select r;
                foreach (renewDB.Tb_revenue rev in fees)
                {
                    paFees.Add(rev.Fee_nbr, rev.Total_fee);
                }
                return paFees;


            }
            catch (Exception e)
            {
                string msg = e.Message;
                return null;
            }
        }
        //
        public static Dictionary<string, string> getEthnicity()
        {
            Dictionary<string, string> ethnicities = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);

                var es = from e in db.Ts_com_code_value
                         where e.Code_category == "race"
                         select e;

                foreach (Tracerdb.Ts_com_code_value e in es)
                {
                    ethnicities.Add(e.Code_value, e.Code_desc);
                }
                return ethnicities;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "DataAccess.getEthnicity");
                return ethnicities;
            }
        }
        public static Dictionary<string, string> getMedSchCountries()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);


                IEnumerable<string> schools = (from m in db.MED_SCH_TABLE
                                               orderby m.CNTRY_KEY
                                               select m.CNTRY_KEY).Distinct();

                foreach (string s in schools)
                {
                    string cntryName = (from c in db.TBL_CNTRY
                                        where c.CNTRY_KEY == s

                                        select c.CNTRY_NAME).SingleOrDefault<string>();

                    if (cntryName != null) d.Add(cntryName, s);
                }
                //need to sort by cntryNames
                //var sortedDict = (from entry in d orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                var sortedDict = (from entry in d orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                return sortedDict;
            }
            catch (Exception e)
            {
                log("Error: " + e.Message, "getMedSchCountries");
                return d;
            }
        }
        public static int getIDnumFromLicense(string license)
        {
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                           where
                                           i.LICENSE_NUM == license
                                           select i).SingleOrDefault<Tracerdb.INDIVIDUAL>();
                if (ind != null) return ind.ID_NUM;
                else return 0;

            }
            catch (Exception e)
            {
                string s1 = e.Message;
                return 0;
            }
        }
        public static Dictionary<string, string> getMedSch(string cd)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            try
            {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                IQueryable<Tracerdb.MED_SCH_TABLE> schools = from m in db.MED_SCH_TABLE
                                                             where m.CNTRY_KEY == cd
                                                             orderby m.SCH_DESC
                                                             select m;

                foreach (Tracerdb.MED_SCH_TABLE s in schools)
                {
                    if (Utilities.isNumeric(s.SCH_CD))
                    {

                        int value = int.Parse(s.SCH_CD);
                        if ((value % 100) == 0) { /*nada*/ }
                        else
                        {
                            d.Add(s.SCH_DESC, s.SCH_CD);
                        }
                    }
                }
                return d;
            }
            catch (Exception e)
            {
                log("Error: CD=" + cd + "  " + e.Message, "getMedSch");
                return d;
            }
        }
        public static string getLast4(string s) //s is license #
        {
            try
            {
                if (App_Code.Utilities.licenseType(s, "PA"))
                {
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.PA pa = (from p in db.PA
                                      where s == p.PA_LIC_NUM
                                      select p).Single<Tracerdb.PA>();

                    Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                               where i.ID_NUM == pa.ID_NUM
                                               select i).Single<Tracerdb.INDIVIDUAL>();
                    return ind.SSN.Substring(5, 4);
                }
                if (App_Code.Utilities.licenseType(s, "AC"))
                {
                    {
                        string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                        Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                        Tracerdb.ACUPUNCTURE ac = (from a in db.ACUPUNCTURE
                                                   where s == a.AC_LIC_NUM
                                                   select a).Single<Tracerdb.ACUPUNCTURE>();

                        Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                                   where i.ID_NUM == ac.ID_NUM
                                                   select i).Single<Tracerdb.INDIVIDUAL>();
                        return ind.SSN.Substring(5, 4);
                    }
                }
                if (App_Code.Utilities.licenseType(s, "NC"))
                {
                    {
                        string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                        Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                        Tracerdb.NC nc = (from n in db.NC
                                          where s == n.NC_PRM_NUM
                                          select n).Single<Tracerdb.NC>();

                        Tracerdb.INDIVIDUAL ind = (from i in db.INDIVIDUAL
                                                   where i.ID_NUM == nc.ID_NUM
                                                   select i).Single<Tracerdb.INDIVIDUAL>();
                        return ind.SSN.Substring(5, 4);
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                App_Code.DataAccess.log("Error " + e.Message, "Utilities.getLast4");
                return "";
            }

        }
        //public static bool writeRenewal2DB(RClassLibrary.renewalPA r)
        //{
        //    try
        //    {
        //        //write Online_master_r
        //        string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
        //        Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
        //        Tracerdb.OnLine_Master_R mr = new Tracerdb.OnLine_Master_R();
        //        mr.LicenseNum = r.LicNum.Trim();
        //        mr.TRACE_NUMBER = r.trace_number.Trim();
        //        mr.TRANSACTION_DATE = r.TRANSACTION_DATE;
        //        mr.OBA_Fee = r.OBA_Fee;
        //        mr.TotalFee = r.TotalDue;
        //        //mr.DocName = r.DocName.Trim();
        //        //mr.Degree = r.Degree.Trim();
        //        //mr.Gender = r.Gender.Trim();
        //        //mr.LicenseExpirationDate = r.LicenseExpirationDate.Trim();
        //        //mr.ThroughDate = r.ThroughDate.Trim();
        //        mr.MAddr1 = r.MailingAddressLine1.Trim();
        //        mr.MAddr2 = r.MailingAddressLine2.Trim();
        //        mr.MCity = r.MailingAddressCity.Trim();
        //        mr.MState = r.MailingAddressState.Trim();
        //        mr.MZIP = r.MailingAddressZip.Trim();
        //        mr.MZIP_Extension = r.MailingAddressZipExt.Trim();
        //        mr.MCountry = r.MailingAddressCountry.Trim();
        //        //
        //        mr.NotPractice = r.notPractice.Trim();
        //        mr.PAddr1 = r.PracticeAddressLine1.Trim();
        //        mr.PAddr2 = r.PracticeAddressLine2.Trim();
        //        mr.PCity = r.PracticeAddressCity.Trim();
        //        mr.PState = r.PracticeAddressState.Trim();
        //        mr.PZIP = r.PracticeAddressZip.Trim();
        //        mr.PZIP_Extension = r.PracticeAddressZipExt.Trim();
        //        mr.PCountry = r.PracticeAddressCountry.Trim();
        //        //
        //        mr.FaxNumber = r.FaxNum.Trim();
        //        mr.EmailNumber = r.EmailAddr.Trim();
        //        mr.PracticeType = r.practiceType.Trim();
        //        mr.PracticeSetting = r.practiceSetting.Trim();
        //        mr.NumberInGroup = r.numberInGroup.Trim();
        //        mr.Anesthesia = r.anesthesia.Trim();
        //        mr.OBA_Type = r.oba_type.Trim();
        //        mr.PracticeHr = r.practiceHr.Trim();

        //        mr.Prac_hrs_cd_primary = r.prac_hrs_cd_primary.Trim();
        //        mr.Prac_set_cd_secondary = r.prac_set_cd_secondary.Trim();
        //        try
        //        {
        //            mr.Prac_set_grp_num_secondary = int.Parse(r.prac_set_grp_num_secondary);
        //        }
        //        catch
        //        {
        //            mr.Prac_set_grp_num_secondary = 0;
        //        }
        //        mr.County_cd_secondary = r.county_cd_secondary.Trim();
        //        mr.Secondary_pzip = r.secondary_pzip.Trim();
        //        mr.Secondary_pzip2 = r.secondary_pzip2.Trim();
        //        mr.CME = r.cme.Trim();
        //        mr.PendingInv = r.pendingInv.Trim();
        //        mr.ChargedOrConv = r.chargedOrConv.Trim();
        //        mr.ConditionBehavior = r.conditionBehavior.Trim();
        //        mr.DEA_DPSReg = r.DEA_DPSreg.Trim();
        //        mr.YearPracUS = r.YrPracticeInUSA.Trim();
        //        mr.YearPracTX = r.YrPracticeInTx.Trim();
        //        mr.PrimarySpec = r.PrimarySpec.Trim();
        //        mr.SecondSpec = r.SecondarySpec.Trim();
        //        mr.FifthPWay = r.FifthPwayDate.Trim();
        //        mr.LanguageTS = r.LanguImpairSrv.Trim();
        //        mr.MedicaidProgram = r.medicaidProgram.Trim();
        //        mr.Accessibility = r.accessibility.Trim();
        //        mr.AwardsH1 = r.Award1.Trim();
        //        mr.AwardsH2 = r.Award2.Trim();
        //        mr.AwardsH3 = r.Award3.Trim();
        //        mr.AwardsH4 = r.Award4.Trim();
        //        mr.AwardsH5 = r.Award5.Trim();
        //        mr.EthnicityInProfile = r.ethnicityInProfile.Trim();
        //        mr.BirthPlaceInProfile = r.BirthPlaceInProfile.Trim();
        //        if (r.newDiscAction.Length > 0)
        //            mr.DiscAction = r.discAction.Trim() + " " + r.newDiscAction.Trim();
        //        else
        //            mr.DiscAction = r.discAction.Trim();
        //        if (r.newCrimChargeConv.Length > 0)
        //            mr.CrimChargeConv = r.crimChargeConv.Trim() + " " + r.newCrimChargeConv.Trim();
        //        else
        //            mr.CrimChargeConv = r.crimChargeConv.Trim();
        //        if (r.newMedMLJury.Length > 0)
        //            mr.MedMLJury = r.MedMLJury.Trim() + " " + r.newMedMLJury;
        //        else
        //            mr.MedMLJury = r.MedMLJury.Trim();
        //        mr.Hispanic_origin_flg = r.hispanic_origin_fl.Trim();
        //        mr.Race_cd = r.race_cd.Trim();
        //        mr.County_cd_high_school = r.county_cd_high_school.Trim();
        //        mr.Emergency_phone = r.EmgrPhoneNum.Trim();
        //        mr.Emergency_fax = r.emergency_fax.Trim();
        //        if (r.NonEmgrCommFlag) mr.Communication_flg = "Y";
        //        mr.Communication_flg = "N";
        //        mr.Medi_fraud = r.medi_fraud.Trim();
        //        mr.Drug_alc_offense = r.drug_alc_offense.Trim();
        //        mr.Sex_assault = r.sex_assualt.Trim();
        //        mr.Tax_fraud = r.tax_fraud.Trim();
        //        //mr.BirthPlace = r.BirthPlace;
        //        //mr.Ethnicity  = r.ethnicity;
        //        //mr.county_cd_primary = r.county_cd_primary;
        //        //mr.PL_rural = r.PL_rural;
        //        //mr.PL_MedicallUS = r.PL_MedicallUS;
        //        //mr.PL_HPSA = r.PL_HPSA;
        //        //mr.PL_NotApp = r.PL_NotApp;
        //        //mr.LicenseType = r.LicenseType;
        //        //mr.RegistrationStatus = r.RegistrationStatus;
        //        //mr.DisciplinaryStatus = r.DisciplinaryStatus;
        //        //mr.LicensureStatus = r.LicensureStatus;
        //        db.OnLine_Master_R.InsertOnSubmit(mr);

        //        //foreach (RClassLibrary.SP sp in r.sps)
        //        //{
        //        //    Tracerdb.OnLine_BDCert_SP spdb = new Tracerdb.OnLine_BDCert_SP();
        //        //    spdb.IssuBD = sp.IssueBoard;
        //        //    spdb.IssuDate = sp.DateEffective;
        //        //    spdb.LicenseNum = sp.license_num;
        //        //    spdb.SepcCert = sp.CERTDesc;
        //        //    db.OnLine_BDCert_SP.InsertOnSubmit(spdb);
        //        //}

        //        //foreach (RClassLibrary.GM gm in r.gms)
        //        //{
        //        //    Tracerdb.OnLine_GraduMed_GM gmdb = new Tracerdb.OnLine_GraduMed_GM();
        //        //    gmdb.BeginDate = gm.BeginDate;
        //        //    gmdb.CityState = gm.CityStateCountry;
        //        //    gmdb.EndDate = gm.EndDate;
        //        //    gmdb.LicenseNum = gm.LICENSE_NUMBER;
        //        //    gmdb.ProgramName = gm.ProgramName;
        //        //    gmdb.Specialty = gm.Specialty;
        //        //    gmdb.Type = gm.ProgramType;

        //        //    db.OnLine_GraduMed_GM.InsertOnSubmit(gmdb);
        //        //}

        //        //foreach (RClassLibrary.HP hp in r.hps)
        //        //{
        //        //    Tracerdb.OnLine_HospPrv_HP hpdb = new Tracerdb.OnLine_HospPrv_HP();
        //        //    hpdb.City = hp.HOSPITAL_CITY;
        //        //    hpdb.HospName = hp.HOSPITAL_NAME;
        //        //    hpdb.LicenseNum = hp.LICENSE_NUMBER;
        //        //    db.OnLine_HospPrv_HP.InsertOnSubmit(hpdb);
        //        //}

        //        //foreach (RClassLibrary.LR lr in r.lrs)
        //        //{
        //        //    Tracerdb.OnLine_Lawsuit_LR lrdb = new Tracerdb.OnLine_Lawsuit_LR();
        //        //    lrdb.CauseNum = lr.CAUSENUM.Trim();
        //        //    lrdb.ClaimReportDate = lr.CLAIMDT.Trim();
        //        //    lrdb.CountySuit = lr.COUNTYSUIT.Trim();
        //        //    lrdb.Court = lr.COURT.Trim();
        //        //    lrdb.IniReserveAmount = lr.ReserveAmount.Trim();
        //        //    lrdb.LicenseNum = lr.LICENSE_NUM.Trim();
        //        //    lrdb.PlaintiffName = lr.PlaintiffName.Trim();
        //        //    lrdb.PolicyNumber = lr.POLICYNUM.Trim();
        //        //    lrdb.PrimaryInsurerCode = lr.PRIMARYINSURER.Trim();
        //        //    db.OnLine_Lawsuit_LR.InsertOnSubmit(lrdb);
        //        //}

        //        //foreach (RClassLibrary.MS ms in r.mss)
        //        //{
        //        //    Tracerdb.OnLine_MedSchool_MS msdb = new Tracerdb.OnLine_MedSchool_MS();
        //        //    msdb.CityStateCountry = ms.MEDICAL_SCHOOL_ADDRESS.Trim();
        //        //    msdb.GraduDate = ms.MEDICAL_SCHOOL_GRAD_DATE.Trim();
        //        //    msdb.LicenseNum = ms.LICENSE_NUMBER.Trim();
        //        //    msdb.MedSchName = ms.MEDICAL_SCHOOL_NAME.Trim();
        //        //    db.OnLine_MedSchool_MS.InsertOnSubmit(msdb);
        //        //}

        //        //foreach (RClassLibrary.OA oa in r.obas)
        //        //{
        //        //    Tracerdb.OnLine_OBAAddr_OA oadb = new Tracerdb.OnLine_OBAAddr_OA();
        //        //    oadb.Address1 = oa.OBA_ADDRESS1.Trim();
        //        //    oadb.Address2 = oa.OBA_ADDRESS2.Trim();
        //        //    oadb.City = oa.OBA_CITY.Trim();
        //        //    oadb.LicenseNum = oa.OBA_LICENSE.Trim();
        //        //    oadb.OBA_Type = oa.OBA_TYPE.Trim();
        //        //    oadb.ZIP_Extension = oa.OBA_ZIP_CODE_EXT.Trim();
        //        //    oadb.ZipCode = oa.OBA_ZIP_CODE.Trim();
        //        //    db.OnLine_OBAAddr_OA.InsertOnSubmit(oadb);
        //        //}

        //        //foreach (RClassLibrary.SR sr in r.srs)
        //        //{
        //        //    Tracerdb.OnLine_Settlement_SR srdb = new Tracerdb.OnLine_Settlement_SR();
        //        //    srdb.Appeal = sr.APPEAL.Trim();
        //        //    srdb.CauseNum = sr.CAUSE_NUMBER.Trim();
        //        //    srdb.CountySuit = sr.COUNTY_OF_SUIT.Trim();
        //        //    srdb.Court = sr.COURT.Trim();
        //        //    srdb.IfSettlOther = sr.IF_OTHER.Trim();
        //        //    srdb.IndemnityAmt = sr.AMOUNT_OF_INDEMNITY.Trim();
        //        //    srdb.LicenseNum = sr.LICENSE_NUMBER.Trim();
        //        //    srdb.Party = sr.WHICH_PARTY.Trim();
        //        //    srdb.PlaintiffName = sr.PLAINTIFF_NAME.Trim();
        //        //    srdb.SettlementDate = sr.DATE_OF_SETTLEMENT.Trim();
        //        //    srdb.SettlementType = sr.TYPE_OF_SETTLEMENT.Trim();
        //        //    db.OnLine_Settlement_SR.InsertOnSubmit(srdb);
        //        //}

        //        //foreach (RClassLibrary.PH ph in r.phs)
        //        //{
        //        //    Tracerdb.Online_reg_answer_detail dtldb = new Tracerdb.Online_reg_answer_detail();
        //        //    dtldb.Answer_detail_cd = ph.type;
        //        //    dtldb.Answer_detail_text = ph.details;
        //        //    dtldb.License_num = ph.license_nbr;
        //        //    db.Online_reg_answer_detail.InsertOnSubmit(dtldb);
        //        //}

        //        db.SubmitChanges();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        string s1 = e.Message;
        //        return false;
        //    }

        //}//writeRenewal2Db
        //public static decimal getAddon()
        //{
        //    try
        //    {
        //        //linqdba.TOLS_DEV01 db = new linqdba.TOLS_DEV01(TMBstoreConfiguration.DbConnectionStringA);
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
        //        //create order 

        //        renewDB.Ts_globals glob = (from g in db.Ts_globals   //    .GetTable<linqdba.Tb_order>()
        //                                   where g.Parm_group == "CONV_FEE" && g.Parm_key == "ADDON"
        //                                   select g).SingleOrDefault();
        //        if (glob != null)
        //        {
        //            return decimal.Parse(glob.Parm_value);
        //        }
        //        return 0.0m;
        //    }
        //    catch (Exception e)
        //    {
        //         log("Error: " + e.Message ,"dataAccess.getAddon");
        //        return 0.0m;
        //    }
        //}// getPercentage
        //public static decimal getPercentage()
        //{
        //    try
        //    {
        //        //linqdba.TOLS_DEV01 db = new linqdba.TOLS_DEV01(TMBstoreConfiguration.DbConnectionStringA);
        //        renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
        //        //create order 

        //        renewDB.Ts_globals glob = (from g in db.Ts_globals   //    .GetTable<linqdba.Tb_order>()
        //                                   where g.Parm_group == "CONV_FEE" && g.Parm_key == "PERCENTAGE"
        //                                   select g).SingleOrDefault();
        //        if (glob != null)
        //        {
        //            return decimal.Parse(glob.Parm_value);
        //        }
        //        return 0.0m;
        //    }
        //    catch (Exception e)
        //    {
        //         log("Error: " + e.Message , "DataAccess.getPercentage");
        //        return 0.0m;
        //    }
        //}// getPercentage
        //
        internal static void put_outbound_payment_log(NICUSA.NICorder no)
        {

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
                Create_user = "mrtRnl",
                Maint_dt = DateTime.Now,
                Maint_user = "mrtRnl"
            };
            db.Tb_outbound_payment_log.InsertOnSubmit(pl);
            db.SubmitChanges();
        }
    }//class
    public class malpractice
    {
        public string Action { get; set; }
        public DateTime Enf_Disp_DT { get; set; }
    }
}