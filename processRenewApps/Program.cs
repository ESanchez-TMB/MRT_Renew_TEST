using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace processRenewApps
{
    class Program
    {        
        public static bool error;
        static logger log;

        static void Main(string[] args)
        {
            log = logger.getLogger();
            log.log("createDocument");

            string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];

            //SharedAssemblyResolver oSharedAssemblyResolver = new SharedAssemblyResolver(mode, new string[] { "AMS_Middleware" }); 

            AMS_LegacyOnlineInterface m_oAMS_LegacyOnlineInterface = new AMS_LegacyOnlineInterface(mode);

            log.log("Mode = " + m_oAMS_LegacyOnlineInterface.RuntimeMode);

            processMRT(m_oAMS_LegacyOnlineInterface);
            processRSP(m_oAMS_LegacyOnlineInterface);
            processPRF(m_oAMS_LegacyOnlineInterface);
            processLMP(m_oAMS_LegacyOnlineInterface);
            processNCR(m_oAMS_LegacyOnlineInterface);  //ncr reg renew


            DataAccess.deleteOld(15, "GM");
            DataAccess.deleteOld(15, "LM");
            DataAccess.deleteOld(15, "RC");
            DataAccess.deleteOld(15, "MP");
            DataAccess.deleteOld(15, "PF");
            DataAccess.deleteOld(15, "NCR"); //ncr is the start of a nct reg license
            log.close();
        }
        //
        private static void processNCR(AMS_LegacyOnlineInterface inf)        
        {
            string first = "";
            bool success = false;
            //get MRT's
            List<RClassLibrary.renewal> apps = DataAccess.getNCRapps();
            List<RClassLibrary.renewal> apps2 = new List<RClassLibrary.renewal>();  

            foreach (RClassLibrary.renewal ncr in apps)
            {               
                Console.WriteLine("Trace number: " + ncr.trace_number);

                if (ncr.status == "Success")
                {
                    if (ncr.trace_number != "")
                    {
                        log.log("NCR TRACENUMBER: " + ncr.trace_number + " License: " + ncr.LicNum);
                        if ((DataAccess.write2DB(ncr)))
                        {
                            log.log("Success write: " + ncr.PAname);

                            apps2.Add(ncr);

                            success = true;
                            if (first == "")
                                first = ncr.trace_number;                           
                            try
                            {
                                string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];                               
                                inf.AMS_FinalizeRenewal(ncr.trace_number);
                                log.log ("FinalizeRenewal " + ncr.trace_number);
                            }
                            catch (Exception e)
                            {
                                log.log(string.Format("Error: {0} {1} {2}", ncr.PAname, e.Message));
                                continue;
                            }
                            //end - part2 - chuck system
                            if (DataAccess.remove(ncr))
                                log.log("Success remove NCR: " + ncr.PAname);
                            else
                                log.log("Error: remove NCR: " + ncr.PAname);
                        }
                        else log.log("Error: write NCR: " + ncr.PAname);
                    }
                    else
                    {
                        log.log("Error: blank trace number ncr. " + ncr.PAname + " " + ncr.trace_number);
                        error = true;
                    }
                }
            }
            try
            {
                if (success)
                {
                    inf.AMS_FinalizeRenewalBatch(first);
                    log.log("FinalizeBatch: " + first);
                }
            }
            catch (Exception e)
            {
                log.log("error ams_finalizeBatch: " + e.Message);
            }
            //
            try
            {
                if (apps2.Count > 0)
                {
                    itemCreator nctItemCreator = new NCTorderItems();
                    success = cash.handleCash(apps2, log, nctItemCreator);
                    if (success)
                        log.log("Cash was successfully handled");
                    else
                        log.log("Cash failed");
                }
            }
            catch (Exception e)
            {
                log.log("Error: cash was bungled: " + e.Message);
            }
        }
        //
        private static void processLMP(AMS_LegacyOnlineInterface inf)
        //private static void processLMP()
        {
            string first = "";
            bool success = false;
            //get MRT's
            List<RClassLibrary.renewal> apps = DataAccess.getLMPapps();
            List<RClassLibrary.renewal> apps2 = new List<RClassLibrary.renewal>();

            foreach (RClassLibrary.renewal lmp in apps)
            {
                //Thread.Sleep(2000);
                Console.WriteLine(lmp.trace_number);
                if (lmp.status == "Success")
                {
                    if (lmp.trace_number != "")
                    {
                        log.log("TRACENUMBER: " + lmp.trace_number + " License: " + lmp.LicNum );                        
                        if ((DataAccess.write2DB(lmp)))
                        {
                            log.log("Success write: " + lmp.PAname);

                            apps2.Add(lmp);


                            success = true;
                            if (first == "")
                                first = lmp.trace_number;
                            //part2 - chuck system
                            try
                            {
                                string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];
                                //AMS_LegacyOnlineInterface.AMS_FinalizeApplication(lmp.trace_number , mode);
                                inf.AMS_FinalizeRenewal(lmp.trace_number);
                            }
                            catch (Exception e)
                            {
                                log.log(string.Format("Error: {0} {1} {2}", lmp.PAname , e.Message));
                                continue;
                            }
                            //end - part2 - chuck system
                            //remove 
                            if (DataAccess.remove(lmp))
                                log.log("Success remove: " + lmp.PAname);
                            else
                                log.log("Failure remove: " + lmp.PAname );
                        }
                        else log.log("Failure write: " + lmp.PAname );
                    }
                    else
                    {
                        log.log("Error: blank trace number lmp. " + lmp.PAname + " " + lmp.trace_number);
                        error = true;
                    }
                }
            }
            try
            {
                if (success)
                {
                    inf.AMS_FinalizeRenewalBatch(first);
                    log.log("Finalize: " + first);
                }
            }
            catch (Exception e)
            {
                log.log("error ams_finalizeBatch: " + e.Message);
            }
            //
            try
            {
                if (apps2.Count > 0)
                {
                    itemCreator lmpItemCreator = new LMPorderItems();
                    success = cash.handleCash(apps2, log, lmpItemCreator);
                    if (success)
                        log.log("Cash was successfully handled");
                    else
                        log.log("Cash failed");
                }
            }
            catch (Exception e)
            {
                log.log("Error: cash was bungled: " + e.Message);
            }
        }
        //
        private static void processPRF(AMS_LegacyOnlineInterface inf)         
        {
            string first = "";
            bool success = false;
            //get MRT's
            List<RClassLibrary.renewal> apps = DataAccess.getPRFapps();
            List<RClassLibrary.renewal> apps2 = new List<RClassLibrary.renewal>();

            foreach (RClassLibrary.renewal prf in apps)
            {
                //Thread.Sleep(2000);
                Console.WriteLine(prf.trace_number);

                if (prf.status == "Success")
                {
                    if (prf.trace_number != "")
                    {
                        log.log("TRACENUMBER: " + prf.trace_number + " License: " + prf.LicNum);
                        if ((DataAccess.write2DB(prf)))
                        {
                            log.log("Success write: " + prf.PAname);

                            apps2.Add(prf);

                            success = true;
                            if (first == "")
                                first = prf.trace_number;
                        
                        //    //part2 - chuck system
                            try
                            {
                                string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];
                                //AMS_LegacyOnlineInterface.AMS_FinalizeApplication(prf.trace_number, mode);
                                inf.AMS_FinalizeRenewal(prf.trace_number);
                            }
                            catch (Exception e)
                            {
                                log.log(string.Format("Error: {0} {1} {2}", prf.PAname , e.Message));
                                continue;
                            }
                            //end - part2 - chuck system
                            if (DataAccess.remove(prf))
                                log.log("Success remove: " + prf.PAname );
                            else
                                log.log("Failure remove: " + prf.PAname );
                        }
                        else log.log("Failure write: " + prf.PAname );
                    }
                    else
                    {
                        log.log("Error: blank trace number prf. " + prf.PAname + " " + prf.trace_number);
                        error = true;
                    }
                }
            }
            try
            {
                if (success)
                {
                    inf.AMS_FinalizeRenewalBatch(first);
                    log.log("Finalize: " + first);
                }
            }
            catch (Exception e)
            {
                log.log("error ams_finalizeBatch: " + e.Message);
            }
            //
            try
            {
                if (apps2.Count > 0)
                {
                    itemCreator prfItemCreator = new PRForderItems();
                    success = cash.handleCash(apps2, log, prfItemCreator);
                    if (success)
                        log.log("Cash was successfully handled");
                    else
                        log.log("Cash failed");
                }
            }
            catch (Exception e)
            {
                log.log("Error: cash was bungled: " + e.Message);
            }
        }
        //
        private static void processRSP(AMS_LegacyOnlineInterface inf)
        //private static void processRSP()
        {
            string first = "";
            bool success = false;
            //
            List<RClassLibrary.renewal> apps = DataAccess.getRSPapps();
            List<RClassLibrary.renewal> apps2 = new List<RClassLibrary.renewal>();


            foreach (RClassLibrary.renewal rsp in apps)
            {
                //Thread.Sleep(2000);
                Console.WriteLine(rsp.trace_number);
                if (rsp.status == "Success")
                {
                    if (rsp.trace_number != "")
                    {
                        log.log("TRACENUMBER: " + rsp.trace_number + " License: " + rsp.LicNum);


                        if ((DataAccess.write2DB(rsp)))
                        {
                            log.log("Success write: " + rsp.PAname );

                            apps2.Add(rsp);

                            success = true;
                            if (first == "")
                                first = rsp.trace_number;
                            //part2 - chuck system
                            try
                            {
                                string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];
                                //AMS_LegacyOnlineInterface.AMS_FinalizeApplication(rsp.trace_number, mode);
                                inf.AMS_FinalizeRenewal(rsp.trace_number);
                            }
                            catch (Exception e)
                            {
                                log.log(string.Format("Error: {0} {1} {2}", rsp.PAname , e.Message));
                                continue;
                            }
                            //end - part2 - chuck system

                            //remove 
                            if (DataAccess.remove(rsp))
                                log.log("Success remove: " + rsp.PAname);
                            else
                                log.log("Failure remove: " + rsp.PAname);
                        }
                        else log.log("Failure write: " + rsp.PAname);
                    }
                    else
                    {
                        log.log("Error: blank trace number rsp. " + rsp.PAname + " " + rsp.trace_number);
                        error = true;
                    }
                }
            }
            try
            {
                if (success)
                {
                    inf.AMS_FinalizeRenewalBatch(first);
                    log.log("Finalize: " + first);
                }
            }
            catch (Exception e)
            {
                log.log("error ams_finalizeBatch: " + e.Message);
            }
            //
            try
            {
                if (apps2.Count > 0)  //why is this test here???? 19_10_29
                {
                    itemCreator rspItemCreator = new RSPorderItems();
                    success = cash.handleCash(apps2, log, rspItemCreator);
                    if (success)
                        log.log("Cash was successfully handled");
                    else
                        log.log("Cash failed");
                }
            }
            catch (Exception e)
            {
                log.log("Error: cash was bungled: " + e.Message);
            }
        }
        //
        private static void processMRT(AMS_LegacyOnlineInterface inf)       
        {
            string first = "";
            bool success = false;
            //get MRT's
            List<RClassLibrary .renewal > apps = DataAccess.getMRTapps(); // production            
            List<RClassLibrary.renewal> apps2 = new List<RClassLibrary.renewal>();
           
            

            foreach (RClassLibrary .renewal  mrt in apps)
            {
                //Thread.Sleep(2000);
                Console.WriteLine(mrt.trace_number);
                if (mrt.status == "Success")
                {
                    if (mrt.trace_number != "")
                    {
                        log.log("TRACENUMBER: " + mrt.trace_number + " License: " + mrt.LicNum);                        
                        if ((DataAccess.write2DB(mrt)))
                        {
                            log.log("Success write: " + mrt.PAname);
                            success = true;

                            apps2.Add(mrt);

                            if (first == "")
                                first = mrt.trace_number;
                            //remove 
                            //part2 - chuck system
                            try
                            {
                                string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];
                                //AMS_LegacyOnlineInterface.AMS_FinalizeApplication(mrt.trace_number, mode);
                                inf.AMS_FinalizeRenewal (mrt.trace_number);
                            }
                            catch (Exception e)
                            {
                                log.log(string.Format("Error: {0} {1} {2}", mrt.PAname , e.Message));
                                continue;
                            }
                            //end - part2 - chuck system
                            if (DataAccess.remove(mrt)) 
                                log.log("Success remove: " + mrt.PAname );
                            else
                                log.log("Failure remove: " + mrt.PAname);
                        }
                        else log.log("Failure write: " + mrt.PAname );
                    }
                    else
                    {
                        log.log("Error: blank trace number MRT. " + mrt.PAname + " " + mrt.trace_number);
                        error = true;
                    }
                }

            }
            try
            {
                if (success)
                {
                    //inf.AMS_FinalizeBatch(first);  //chuck wants me to call this
                     inf.AMS_FinalizeRenewalBatch (first);  
                    log.log("Finalize: " + first);
                }
            }
            catch (Exception e)
            {
                log.log("error ams_finalizeBatch: " + e.Message);
            }
            //
            try
            {
                if (apps2.Count > 0)
                {
                    itemCreator mrtItemCreator = new MRTorderItems();
                    success = cash.handleCash(apps2, log, mrtItemCreator);
                    log.log("Cash was successfully handled");
                }
            }
            catch (Exception e)
            {
                log.log("Error: cash was bungled: " + e.Message);
            }
        }
        //
    }
}
