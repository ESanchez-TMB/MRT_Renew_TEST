using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;


namespace special01
{
    class DataAccess
    {
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
                return new renewDB.Tb_revenue();
            }
            catch
            {
                return new renewDB.Tb_revenue();
            }
        }
        //
        internal static RClassLibrary.renewal getFinal(string TraceNum)
        {
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_20160218;Persist Security Info=True;  User ID=usr_ARO; Password=kdln#57f; Connection Timeout=600;");      
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_Dev01_old;Persist Security Info=True;  User ID=usr_ARO; Password=kdln#57f; Connection Timeout=600;");      
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_20160218;Persist Security Info=True;  Integrated Security=SSPI; Connection Timeout=600;");
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_backup_20160218;Persist Security Info=True;  Integrated Security=SSPI; Connection Timeout=600;");
            renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = WEBSQLPROD; Initial Catalog=ARO_PROD01;Persist Security Info=True;   User ID=usr_ARO;Password=kdln#57f; Connection Timeout=600;");  //this is production

           
            try
            {
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal)); 

                var final = (from wip in db.Tb_work_final                           
                           where wip.Work_data.Contains(TraceNum)
                           select wip).Single();

                var sr = new StringReader(final.Work_data);
                RClassLibrary.renewal r = (RClassLibrary.renewal)xs.Deserialize(sr);
                return r;

            }
            catch (Exception e)
            {
                Program.log.log("Error getFinal " + e.Message);
                return null;
            }
        }
        //
        internal static List<RClassLibrary.renewal> getRCPapps4Test()  //get apps from production to process against test
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = WEBSQLPROD; Initial Catalog=ARO_PROD01;Persist Security Info=True;   User ID=usr_ARO;Password=kdln#57f; Connection Timeout=600;");  //this is production
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("RC")  //|| w.License_num.StartsWith("LM")
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
                Program.log.log("getRCPapps4Test Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }
        //
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
                Program.log.log("getMRTapps4Test Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }

        }

        public static List<RClassLibrary.renewal> getRenewals()
        {
            renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
            try
            {


                var wips = from wip in db.Tb_work_inprogress
                           where wip.License_num.Substring(0,2) == "MP" || wip.License_num.Substring(0,2) == "MR" ||wip.License_num.Substring(0,2) == "PF" ||wip.License_num.Substring(0,2) == "RC" 
                           select wip;

                List<RClassLibrary.renewal> renewals = new List<RClassLibrary.renewal>();

                var xs = new XmlSerializer(typeof(RClassLibrary.renewal));
                foreach (var w in wips)
                {                    
                    var sr = new StringReader(w.Work_data);
                    RClassLibrary.renewal r = (RClassLibrary.renewal)xs.Deserialize(sr);
                    if (r.status == "Success")
                        renewals.Add(r);
                }

                return renewals;          //renewDB.ARO_DEV01.Tb_work_inprogress                            
                
            }
            catch (Exception e)
            {
                 Program .log .log ("Error " + e.Message);
                return null;
            }
        }

        internal static List <renewDB .Tb_work_inprogress>  getNctWIPs()
        {
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_20160218;Persist Security Info=True;  User ID=usr_ARO; Password=kdln#57f; Connection Timeout=600;");      
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_Dev01_old;Persist Security Info=True;  User ID=usr_ARO; Password=kdln#57f; Connection Timeout=600;");      
            //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_20160218;Persist Security Info=True;  Integrated Security=SSPI; Connection Timeout=600;");
            renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = PIERCE; Initial Catalog=ARO_backup_20160218;Persist Security Info=True;  Integrated Security=SSPI; Connection Timeout=600;"); 
            
            try
            {


                var wips = from wip in db.Tb_work_inprogress
                           where wip.License_num.Substring(0, 2) == "NC" 
                           select wip;

                return wips.ToList();                                        

            }
            catch (Exception e)
            {
                Program.log.log("Error " + e.Message);
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
                        RClassLibrary.DataAccess.log("Error: updateAppDBwip failed.", string.Format("DataAccess.putWIPrenewal({0})", r.LicNum));
                        return "error";
                    }
                }
                else
                {// first wip
                    status = insertWIPrenewal(r);
                    if (status != "success")
                    {
                        RClassLibrary.DataAccess.log("Error: insertAppDBwip failed.", "DataAccess.putApplDBwip(phyAppCL.phyApp r)");
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
        public static RClassLibrary.renewal getWIPrenewal(string license)
        {   // will be 1 or 0 
            //as of 11_26 ln_fn_dob will actually be a email + dob
            renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
            try
            {
                //get the order

                license = license.ToUpper();
                renewDB.Tb_work_inprogress wip = (from w in db.Tb_work_inprogress
                                                  where w.License_num == license
                                                  select w).Single<renewDB.Tb_work_inprogress>();
                if (wip != null)
                {
                    var xs = new XmlSerializer(typeof(RClassLibrary.renewal));
                    var sr = new StringReader(wip.Work_data);
                    RClassLibrary.renewal r = (RClassLibrary.renewal)xs.Deserialize(sr);
                    return r;
                }
                else
                    return null;
            }

            catch (Exception e)
            {
                RClassLibrary.DataAccess.log("Error: " + e.Message, string.Format("getWIPrenew({0})", license));
                return null;
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
                    License_num = r.LicNum,
                    Work_data = sw.ToString(),
                    Create_dt = DateTime.Now,
                    Maint_dt = DateTime.Now
                };
                wip.Create_user = "mpRnl";
                wip.Maint_user = "mpRnl";


                db.Tb_work_inprogress.InsertOnSubmit(wip);
                db.SubmitChanges();
                return "success";
            }
            catch (Exception e)
            {
                RClassLibrary.DataAccess.log("Error: orderID: " + e.Message, string.Format("DataAccess.insertWIPrenewal({0})", r.LicNum));
                return e.Message;
            }
        }
        //
        public static string updateWIPrenewal(RClassLibrary.renewal r)
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

        internal static List<RClassLibrary.renewal> getNCTapps4Test()
        {
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                //renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01("Data Source = WEBSQLPROD; Initial Catalog=ARO_PROD01;Persist Security Info=True;   User ID=usr_ARO;Password=kdln#57f; Connection Timeout=600;");  //this is production
                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("NC")  //|| w.License_num.StartsWith("LM")
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
                Program.log.log("getNCTapps4Test Error: " + e.Message);
                return new List<RClassLibrary.renewal>();
            }
        }
        //

        //
        public static List<renewDB.Tb_audit> getAudits4orderID (int orderID)
            {
            try
                {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"];
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(dbname);
                //audit codes to be excluded
                var excludeCodes = new int[] { 10, 11, 12 };
                var audits = from a in db.Tb_audit
                             where a.Order_id == orderID
                             where !excludeCodes.Contains(a.Message_nbr)
                             //where a.Message_nbr != 10
                             //where a.Message_nbr != 11
                             //where a.Message_nbr != 12
                             orderby a.Message_nbr
                             select a;
                return audits.ToList<renewDB.Tb_audit>();
                }
            catch
                {
                //log("Error: " + e.Message + "emailCO.App_Code.dbAccess getAudits4orderID", "getAudits4orderID");
                return new List<renewDB.Tb_audit>();
                }
            }

        //
        public static string createEmailBody (NICUSA.commonCheckOut.PaymentResult queryResult, customer_order customerOrder)
            {
            string strPayment_Type;
            string strPayment_Media_Type;
            string strPayment_Number_Type;

            if (queryResult.PAYTYPE == "ACH")
                {
                strPayment_Type = "Electronic Check";
                strPayment_Media_Type = string.Empty;
                strPayment_Number_Type = "Account Number";
                }
            else
                {
                strPayment_Type = "Credit Card";
                strPayment_Media_Type = queryResult.PAYTYPE;
                strPayment_Number_Type = "Credit Card Number";
                }
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append("<h3> Transaction Summary - Compliance Officer Copy</h3>");
            emailBody.Append("<table align='center' cellspacing='0' border='1' style='border: 1px Solid #888888; width: 50em; font-size: 1em'>");
            emailBody.Append("<thead><tr><th align='left' style='background-color: #AAAAAA; font-size: 1em'>Description</th><th style='background-color: #AAAAAA; font-size: .7em'>&nbsp;</th>");
            emailBody.Append("<th align='right' style='background-color: #AAAAAA; font-size: 1em'>Amount</th></tr></thead>");
            emailBody.Append("<tbody><tr><td>TMB Fee/Pen Paymts</td><td align='right'>Texas.gov Price</td><td align='right'>");
            emailBody.Append(string.Format("{0}</td></tr></tbody></table>", queryResult.TOTALAMOUNT));
            emailBody.Append("<table align='center' cellpadding='0' cellspacing='0'><tr><td valign='top'>");
            emailBody.Append("<table style='width: 30em; font-size: 1em'>");
            emailBody.Append("<caption style='text-align:left; background-color: #888888; padding: 2px;'>Customer Information</caption>");
            emailBody.Append(string.Format("<tr><td>Customer Name</td><td>{0}</td></tr>", queryResult.NAME));
            emailBody.Append(string.Format("<tr><td>SQLTracer ID Number</td><td>{0}</td></tr>", customerOrder.customer_id));
            emailBody.Append(string.Format("<tr><td>Local Reference ID</td><td>{0}</td></tr>", queryResult.LOCALREFID));
            emailBody.Append(string.Format("<tr><td>Receipt Date</td><td>{0}</td></tr>", queryResult.RECEIPTDATE));
            emailBody.Append(string.Format("<tr><td>Receipt Time</td><td>{0}</td></tr>", queryResult.RECEIPTTIME));
            emailBody.Append("</table></td><td valign='top'><table style='width: 30em; font-size: 1em'>");
            emailBody.Append("<caption style='text-align:left; background-color: #888888; padding: 2px;'>Payment Information </caption>");
            emailBody.Append(string.Format("<tr><td>Payment Type</td><td>{0}</td></tr>", strPayment_Type));
            emailBody.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td></td><td></td></tr>", strPayment_Media_Type, queryResult.CreditCardType));
            emailBody.Append(string.Format("<tr><td></td><td></td></tr><tr><td>Order ID</td><td>{0}</td></tr>", queryResult.ORDERID));
            emailBody.Append(string.Format("<tr><td>{0}</td><td>****{1}</td></tr></table></td></tr></table>", strPayment_Number_Type, queryResult.LAST4NUMBER));
            emailBody.Append("<table align='center' cellpadding='0' cellspacing='0'>");
            emailBody.Append("<caption style='text-align:left; background-color: #888888; padding: 2px;'>Billing Information</caption>");
            emailBody.Append("<tr><td valign='top'><table style='width: 30em; font-size: 1em'><tr><td>Billing Address</td>");
            emailBody.Append(string.Format("<td>{0}<br>{1}</td></tr><tr>", queryResult.ADDRESS1, queryResult.ADDRESS2));
            emailBody.Append(string.Format("<td>Billing City, State</td><td>{0}, {1}</td></tr>", queryResult.CITY, queryResult.STATE));
            emailBody.Append(string.Format("<tr><td>ZIP/Postal Code</td><td>{0}</td></tr>", queryResult.ZIP));
            emailBody.Append(string.Format("<tr><td>Country</td><td>{0}</td></tr></table></td>", queryResult.COUNTRY));
            emailBody.Append("<td valign='top'><table style='width: 30em; font-size: 1em'><tr>");
            emailBody.Append(string.Format("<td>Phone Number</td><td>{0}</td></tr>", queryResult.PHONE));
            emailBody.Append("<tr><td colspan='2'>This receipt has been emailed to the address below.</td></tr>");
            emailBody.Append(string.Format("<tr><td valign='top'>Email Address</td><td valign='top'>{0}<br>", queryResult.EMAIL));
            emailBody.Append(string.Format("{0}<br>{1}<br>{2}</td></tr></table></td></tr></table>", queryResult.EMAIL1, queryResult.EMAIL2, queryResult.EMAIL3));

            return emailBody.ToString();
            }

        //
        public static string calcDetails (NICUSA.commonCheckOut.PaymentResult paymentQueryResult)
            {
            string orderID = paymentQueryResult.LOCALREFID.Substring(9, 6);
            int intOrderID = Convert.ToInt32(orderID);
            List<renewDB.Tb_order_item> orderItems = getOrderItems(intOrderID);
            decimal subTotalOI = 0.0M;

            List<receiptDetail> rDetails = new List<receiptDetail>();
            foreach (renewDB.Tb_order_item oi in orderItems)
                {
                receiptDetail rDet = new receiptDetail();

                subTotalOI = subTotalOI + oi.Unit_price_amt;
                rDet.description = getItemCdDescription(oi.Item_cd);
                rDet.identifier = oi.Item_cd;
                rDet.amount = oi.Unit_price_amt;
                rDetails.Add(rDet);
                }

            List<string> recptDetails = new List<string>();
            foreach (receiptDetail item in rDetails)
                {
                recptDetails.Add(string.Format("Item Description: {0}  for order/plan #: {1}  Amount: {2}", item.description, item.identifier, item.amount.ToString("C")));
                }

            decimal ConvFee = Convert.ToDecimal(paymentQueryResult.TOTALAMOUNT) - subTotalOI;
            recptDetails.Add(string.Format(" Convenience Fee for Texas.Gov: {0}", ConvFee.ToString("C")));

            StringBuilder sb = new StringBuilder();

            foreach (string rd in recptDetails)
                {
                sb.Append(rd + "<br />");
                }

            string rDetailsBody = sb.ToString();
            return rDetailsBody;
            }
        //
        private static string getItemCdDescription (string FEPPSType)
            {
            try
                {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Ts_code_value glob = (from g in db.Ts_code_value   //    .GetTable<applDB.Tb_order>()
                                                where g.Code_category == "item_cd" && g.Code_value == FEPPSType
                                                select g).SingleOrDefault();
                if (glob != null)
                    {
                    return glob.Code_desc;
                    }
                return string.Empty;
                }
            catch 
                {
               // log("Error " + e.Message + "variable FEPPSType: " + FEPPSType, "webFPP.App_Code.dbAccess.decodeFEPPSType");
                return string.Empty;
                }
            }
        //

        public static List<renewDB.Tb_order_item> getOrderItems (int orderID)
            {
            List<renewDB.Tb_order_item> ois;
            try
                {
                string dbname = System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"];
               renewDB.ARO_DEV01 db  = new renewDB.ARO_DEV01(dbname);
                var oi = from o in db.Tb_order_item
                         where o.Order_id == orderID
                         select o;
                ois = oi.ToList();
                return ois;
                }
            catch 
                {
                return null;
                }
            }


        internal static void emailNotice2(string bodyOfEmail, string strSenderEmail, string local_ref_id)
            {
            throw new NotImplementedException();
            }
    }
}
