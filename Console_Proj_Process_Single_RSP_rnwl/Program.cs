using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console_Proj_Process_Single_RSP_rnwl
    {



    class Program
        {

        public static bool error;
        static logger log;


        static void Main(string[] args)
            {

            log = logger.getLogger();
            log.log("Run Console_Proj_Process_Single_RSP_rnwl");

            string strLicNUM = "RCP00057621";

            string mode = System.Configuration.ConfigurationManager.AppSettings["mode"];

            AMS_LegacyOnlineInterface m_oAMS_LegacyOnlineInterface = new AMS_LegacyOnlineInterface(mode);

            log.log("Mode = " + m_oAMS_LegacyOnlineInterface.RuntimeMode);

            processRSP(m_oAMS_LegacyOnlineInterface, strLicNUM);

            log.close();


            }

        private static void processRSP(AMS_LegacyOnlineInterface inf)
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
                            log.log("Success write: " + rsp.PAname);

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
                                log.log(string.Format("Error: {0} {1} {2}", rsp.PAname, e.Message));
                                continue;
                                }
                            //end - part2 - chuck system

                            //remove 
                            if (DataAccess.remove(rsp))
                                log.log("Success remove: " + rsp.PAname);
                            else
                                log.log("Failure remove: " + rsp.PAname);
                            }
                        else
                            log.log("Failure write: " + rsp.PAname);
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
                if (apps2.Count > 0)
                    {
                    itemCreator rspItemCreator = new RSPorderItems();
                    success = cash.handleCash(apps2, log, rspItemCreator);
                    log.log("Cash was successfully handled");
                    }
                }
            catch (Exception e)
                {
                log.log("Error: cash was bungled: " + e.Message);
                }
            }

        private static void processRSP(AMS_LegacyOnlineInterface inf, string strLicenseNumber)
            {

            string first = "";
            bool success = false;
            //

            List<RClassLibrary.renewal> apps = DataAccess.getRSPapps(strLicenseNumber);
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
                            log.log("Success write: " + rsp.PAname);

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
                                log.log(string.Format("Error: {0} {1} {2}", rsp.PAname, e.Message));
                                continue;
                                }
                            //end - part2 - chuck system

                            //remove 
                            if (DataAccess.remove(rsp))
                                log.log("Success remove: " + rsp.PAname);
                            else
                                log.log("Failure remove: " + rsp.PAname);
                            }
                        else
                            log.log("Failure write: " + rsp.PAname);
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
                if (apps2.Count > 0)
                    {
                    itemCreator rspItemCreator = new RSPorderItems();
                    success = cash.handleCash(apps2, log, rspItemCreator);
                    log.log("Cash was successfully handled");
                    }
                }
            catch (Exception e)
                {
                log.log("Error: cash was bungled: " + e.Message);
                }
            }
        }
    }
