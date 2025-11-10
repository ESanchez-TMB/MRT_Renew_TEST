using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace special01
{
    class Program
    {
        public static logger log;
        private static string orderID;
        
        public static customer_order co { get; set; }

        public static string[] lmpTraces = 
        {
            "503TW2009579929"
        };
        //
        public static string[] TraceTable = {
            "503MW2009579920",
            "503MW2009580025",
            "503MW2009579923",
            "503MW2009579876",
            "503MW2009580050",
            "503MW2009579914",
            "503MW2009579971",
            "503MW2009579868",
            "503MW2009579918"
                                            };
        //

        public static string[] rcpTraces = 
        {
            "503RW2009579892", 
"503RW2009579939", 
"503RW2009580022", 
"503RW2009579997", 
"503RW2009579985", 
"503RW2009580012", 
"503RW2009580029", 
"503RW2009579952", 
"503RW2009579894", 
"503RW2009579956", 
"503RW2009580038", 
"503RW2009579955", 
"503RW2009579945", 
"503RW2009579880", 
"503RW2009580001", 
"503RW2009579983", 
"503RW2009579901", 
"503RW2009579867", 
"503RW2009579980", 
"503RW2009579936", 
"503RW2009579934", 
"503RW2009580065", 
"503RW2009580080", 
"503RW2009579986", 
"503RW2009580060", 
"503RW2009579700", 
"503RW2009579863", 
"503RW2009579865", 
"503RW2009579873", 
"503RW2009579974", 
"503RW2009579861", 
"503RW2009580075", 
"503RW2009579878", 
"503RW2009579933", 
"503RW2009580039", 
"503RW2009579883", 
"503RW2009580062", 
"503RW2009580044", 
"503RW2009580061", 
"503RW2009579976", 
"503RW2009579972", 
"503RW2009580035", 
"503RW2009580004", 
"503RW2009580006", 
"503RW2009580024", 
"503RW2009580046", 
"503RW2009579862", 
"503RW2009579977", 
"503RW2009579967", 
"503RW2009580037", 
"503RW2009579970", 
"503RW2009580054", 
"503RW2009579893", 
"503RW2009580034", 
"503RW2009579931", 
"503RW2009579864", 
"503RW2009579947", 
"503RW2009579896", 
"503RW2009579950", 
"503RW2009579935", 
"503RW2009579953", 
"503RW2009579903", 
"503RW2009580068", 
"503RW2009580079", 
"503RW2009580055", 
"503RW2009579916", 
"503RW2009579928", 
"503RW2009579958", 
"503RW2009580031", 
"503RW2009579909", 
"503RW2009579981", 
"503RW2009580059", 
"503RW2009580058", 
"503RW2009580057", 
"503RW2009579994", 
"503RW2009580043" 
};



        static void Main(string[] args)
        {
            log = logger.getLogger();
            log.log("Start Special01");

            //arbitoryTest();


            //dumpSuccessRenewal();

            recoverWip();

            //expunction();

            //copyProd2TestMRT(); //this must be done pointing at test
            //copyProd2TestRCP();  //this must be done pointing at test
            //copyProd2TestNCT();  //this must be done pointing at test
            //deleteWIPs();

            //recoverCash();
            log.log("Bye");
            log.close();

        }
        private static void arbitoryTest()
        {
            //renewDB.Tb_revenue rev = DataAccess.getRevenueDB("4403");  //works
            renewDB.Tb_revenue rev = RClassLibrary.DataAccess.getRevenueDB("4403"); // both work

            Console.WriteLine("rev.unitprice: " + rev.Total_fee.ToString ("C"));
            decimal x = RClassLibrary.DataAccess.getFee("4403");
            Console.WriteLine("rev.unitprice: " + x.ToString("C"));
        }
        //
        private static void recoverCash()
        {
            //this is nightmare - stupid database sort of works but can't handle cash
            //this attempts to recover cash - must use manually table to know what to recover
            //each program must be recovered separately med phy, mrt, rcp, etc. 


            List<RClassLibrary.renewal> RL = new List<RClassLibrary.renewal>();
            //foreach (string traceNum in TraceTable)            
            //foreach (string traceNum in rcpTraces )
            foreach (string traceNum in lmpTraces)
            {
                RClassLibrary.renewal r = DataAccess.getFinal(traceNum);
                if (r != null)
                {
                    r.TRANSACTION_DATE = DateTime.Parse("09/06/2020").ToString("yyyy/MM/dd");
                    RL.Add(r);
                }
            }

            Console.WriteLine("RL.count = " + RL.Count().ToString());

            //create creator 
            //processRenewApps.itemCreator mrtItemCreator = new processRenewApps.MRTorderItems();
            //processRenewApps.itemCreator mrtItemCreator = new processRenewApps.RSPorderItems();
            processRenewApps.itemCreator mrtItemCreator = new processRenewApps.LMPorderItems();
            processRenewApps.logger logr = processRenewApps.logger.getLogger();

            bool success = processRenewApps.cash.handleCash(RL, logr, mrtItemCreator);
            log.log("Cash was successfully handled");

           
            //so far this seems to work 


        }
        //
        private static void deleteWIPs()
        {
            deleteMPs();
            deleteMRTs();
            deleteNCRs();
            deletePFs();
            deleteRTs();

        }
        //
        private static void deleteMRTs()
        {
            log.log("delete the MRTs");
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var wips = from w in db.Tb_work_inprogress
                           where w.License_num .StartsWith ("LM") || w.License_num .StartsWith ("GM")
                           select w;

                // now delete all these wips
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    Console.WriteLine("I want to remove " + wip.License_num );
                    log.log("I want to remove " + wip.License_num );
                    db.Tb_work_inprogress.DeleteOnSubmit(wip);
                }
                db.SubmitChanges();      //comment for test
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleteLMPwip: " + e.Message);
                return;
            }
        }
        //
        private static void deleteMPs()
        {
            log.log("delete the MPs");
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("FM")  
                           select w;

                // now delete all these wips
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    Console.WriteLine("I want to remove " + wip.License_num);
                    log.log("I want to remove " + wip.License_num);
                    db.Tb_work_inprogress.DeleteOnSubmit(wip);
                }
                db.SubmitChanges();      //comment for test
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleteMPwip: " + e.Message);
                return;
            }
        }
        //
        private static void deleteNCRs()
        {
            log.log("delete the NCRs");
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("NC") 
                           select w;

                // now delete all these wips
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    Console.WriteLine("I want to remove " + wip.License_num);
                    log.log("I want to remove " + wip.License_num);
                    db.Tb_work_inprogress.DeleteOnSubmit(wip);
                }
                db.SubmitChanges();      //comment for test
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleteNCRwip: " + e.Message);
                return;
            }
        }
        //
        private static void deletePFs()
        {
            log.log("delete the PFs");
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("FP")
                           select w;

                // now delete all these wips
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    Console.WriteLine("I want to remove " + wip.License_num);
                    log.log("I want to remove " + wip.License_num);
                    db.Tb_work_inprogress.DeleteOnSubmit(wip);
                }
                db.SubmitChanges();      //comment for test
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deletePFwip: " + e.Message);
                return;
            }
        }
        //
        private static void deleteRTs()
        {
            log.log("delete the RTs");
            try
            {
                List<RClassLibrary.renewal> apps = new List<RClassLibrary.renewal>();
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var wips = from w in db.Tb_work_inprogress
                           where w.License_num.StartsWith("RCP")  
                           select w;

                // now delete all these wips
                foreach (renewDB.Tb_work_inprogress wip in wips)
                {
                    Console.WriteLine("I want to remove " + wip.License_num);
                    log.log("I want to remove " + wip.License_num);
                    db.Tb_work_inprogress.DeleteOnSubmit(wip);
                }
                db.SubmitChanges();      //comment for test
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleteLRTwip: " + e.Message);
                return;
            }
        }
        //
        private static void makeCopy202Receipt()
        {
        string token;
        string strTotalAmount;
        string strReceipt_Details;
        decimal totAmount;
        string local_ref_id;
        string tokenFromSession = string.Empty;
        string strSenderEmail = string.Empty;


        int order_id = int.Parse(orderID);

        List<renewDB.Tb_audit> orderItems = DataAccess.getAudits4orderID(order_id);

        
        NICUSA.NICorder _no = new NICUSA.NICorder();
        

        //****************
        _no.pr = NICUSA.Class1.callQueryPayment(tokenFromSession, "BasicHttpBinding_IServiceWeb");
        //****************

        if (_no.pr.FAILCODE == "N") //_no.pr = qr
            {
            log.log("callQueryPayment successfull");
            totAmount = Convert.ToDecimal(_no.pr.TOTALAMOUNT);
            strTotalAmount = String.Format("{0:C}", totAmount);
            local_ref_id = _no.pr.LOCALREFID;
           // strReceipt_Details = DataAccess.calcDetails(_no.pr);

            // create body of receipt
            log.log("create body of receipt");
            string bodyOfEmail = DataAccess.createEmailBody(_no.pr, co);


            // send email to Comp. Officer
            log.log("send email to comp officer");
            DataAccess.emailNotice2(bodyOfEmail, strSenderEmail, local_ref_id);


            }
        else
            {
            log.log("callQueryPayment failed.");
            //totOrders.Add(string.Format("Failure-FEPPS,  ID Number: {0}, Name: {1}, amount {2}, ,order ID {3}, order_date {4}, compliance officer {5}", co.customer_id, co.first_name + " " + co.last_name, co.amount.ToString("C"), co.order_id, co.orderDate.ToShortDateString(), co.compliance_officer_id.ToString()));

            }



        //RClassLibrary.DataAccess.dumpInboundReceipt(_no.pr, token, order_id, "TMB_MRT");        
        }
        //
        private static void copyProd2TestNCT()
        {
            //*************************
            //       you must set the database to ARO test for this to work
            //       if ARO  <add key="DbConnectionA" set to production
            //       it will put duplicates in wip - messing up produciton processing
            //**********************************************
            //get mrt's from prod
            List<RClassLibrary.renewal> apps = DataAccess.getNCTapps4Test(); // test - get apps from production to process agains test

            //write 2 test - important set dbconnectionA to dev before you run this
            foreach (RClassLibrary.renewal renewal in apps)
            {
                string answer = DataAccess.putWIPrenewal(renewal);
                if (answer != "success")
                {
                    log.log("WIP write was successful");
                }
            }
        }
        
        //
        private static void copyProd2TestRCP()
        {
            //*************************
            //       you must set the database to ARO test for this to work
            //       if ARO  <add key="DbConnectionA" set to production
            //       it will put duplicates in wip - messing up produciton processing
            //**********************************************
            //get mrt's from prod
            List<RClassLibrary.renewal> apps = DataAccess.getRCPapps4Test(); // test - get apps from production to process agains test

            //write 2 test - important set dbconnectionA to dev before you run this
            foreach (RClassLibrary.renewal renewal in apps)
            {
                string answer = DataAccess.putWIPrenewal(renewal);
                if (answer != "success")
                {
                    log.log("WIP write was successful");
                }
            }
        }
        //
        private static void copyProd2TestMRT()
        {
            //*************************
            //       you must set the database to ARO test for this to work
            //       if ARO  <add key="DbConnectionA" set to production
            //       it will put duplicates in wip - messing up produciton processing
            //**********************************************
            //get mrt's from prod
            List<RClassLibrary.renewal> apps = DataAccess.getMRTapps4Test(); // test - get apps from production to process agains test

            //write 2 test - important set dbconnectionA to dev before you run this
            foreach (RClassLibrary.renewal renewal in apps)
            {
                string answer = DataAccess.putWIPrenewal(renewal);
                if (answer != "success")
                {
                    log.log("WIP write was successful");
                }
            }
        }
        //
        private static void expunction()
        {
            try
            {
                //read final
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);

                var final = (from f in db.Tb_work_final
                             where f.Work_final_id == 78714
                             select f).Single();

                //put the object back together
                var xs = new XmlSerializer(typeof(RClassLibrary.renewal));
                var sr = new StringReader(final.Work_data);
                RClassLibrary.renewal r = (RClassLibrary.renewal)xs.Deserialize(sr);
                r.arrested = "N";
                r.convicted = "N";
                r.cited_charged = "N";
                final.Maint_dt = DateTime.Now;
                final.Maint_user = "Special01";

                //var xs = new XmlSerializer(r.GetType());
                var sw = new StringWriter();
                xs.Serialize(sw, r);
                final.Work_data = sw.ToString();
                
                db.SubmitChanges();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void recoverWip()
        {
            //at this point all the renewal records r same 
            //the purpose of this dude is to take a final record and turn it into a wip
            //14146 9/21/16
            // hey dude, this guy does not try to overlay a wip it will create a second wip which will break other routines
            // please remember to delete the orig. wip after running this routine
            try
            {
                //read final
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                var final = (from f in db.Tb_work_final
                             where f.Work_final_id == 205660 //180979 //180408 //179472 //160702  //148251 //145893 //139385 //139056 //114286 //103259 //102131 //103546  //102820 //100626 //98862  //95982  //92541 //96097 //90936 //72753 //62241 //60936 // 62948   //60948  //57335  //52971 //42233 //37899 //36097    // 30363 //25845 //19470 //23175 19459 // 19149 //19450 //20081 //19497 //16842 //14146 //11016 //11661 //7507  // 281
                             select f).Single();
                //create wip
                renewDB.Tb_work_inprogress wip = new renewDB.Tb_work_inprogress()
                {
                    Create_dt = DateTime.Now,
                    Maint_dt = DateTime.Now,
                    Create_user = "Special01",
                    Maint_user = "Special01",
                    License_num = final.License_num,
                    Work_data = final.Work_data
                };

                db.Tb_work_inprogress.InsertOnSubmit(wip);
                db.SubmitChanges();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //
        private static void recoverNCTwip()
        {
            var nctWIPs = DataAccess.getNctWIPs();

            try
            {
                renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                foreach (var wip in nctWIPs)
                {
                    renewDB.Tb_work_inprogress wip2 = new renewDB.Tb_work_inprogress();
                    wip2.Create_dt = wip.Create_dt;
                    wip2.Create_user = wip.Create_user;
                    wip2.Customer_id = wip.Customer_id;
                    wip2.License_num = wip.License_num;
                    wip2.Maint_dt = DateTime.Now;
                    wip2.Maint_user = "recover";
                    wip2.Customer_id = wip.Customer_id;
                    wip2.Work_data = wip.Work_data;

                    db.Tb_work_inprogress.InsertOnSubmit(wip2);
                     
                }
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
        //
        private static void dumpSuccessRenewal()
        {
            List<RClassLibrary.renewal> renewals = DataAccess.getRenewals();
            foreach ( RClassLibrary .renewal r in renewals )
            {
                ObjectDumper.Write (r);
            }
        }

       
    }
}
