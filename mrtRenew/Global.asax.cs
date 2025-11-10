using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace mrtRenew
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            Dictionary<string, string> trace_license = new Dictionary<string, string>();
            Application["trace_license"] = trace_license;
            //fdms

            System.AppDomain.CurrentDomain.AssemblyResolve += new System.ResolveEventHandler(CustomResolveEventHandler);            


            // Code that runs on application startup
            Dictionary<int, decimal> PAfees;
            PAfees = App_Code.DataAccess.getPAfees("4486");
            Application["PAfees"] = PAfees;
            Dictionary<string, string> ethnicity;
            ethnicity = App_Code.DataAccess.getEthnicity();
            Application["ethnicity"] = ethnicity;
            Dictionary<string, string> texasCounties = App_Code.DataAccess.getCounties();
            Application["texasCounties"] = texasCounties;
            Dictionary<string, string> states = new Dictionary<string, string> {
           {"ALABAMA", "AL"},
           {"ALASKA", "AK"},
           {"ARIZONA", "AZ"},
           {"ARKANSAS", "AR"},           
           {"CALIFORNIA", "CA"},
           {"COLORADO", "CO"},
           {"CONNECTICUT", "CT"},
           {"DELAWARE", "DE"},
           {"DISTRICT OF COLUMBIA", "DC"},
           {"FLORIDA", "FL"},
           {"GEORGIA", "GA"},
           {"GUAM", "GQ"},
           {"HAWAII", "HI"},
           {"IDAHO", "ID"},
           {"ILLINOIS", "IL"},
           {"INDIANA", "IN"},
           {"IOWA", "IA"},
           {"KANSAS", "KS"},
           {"KENTUCKY", "KY"},
           {"LOUISIANA", "LA"},
           {"MARYLAND", "MD"},
           {"MAINE", "ME"},
           {"MASSACHUSETTS", "MA"},
           {"MICHIGAN", "MI"},
           {"MINNESOTA", "MN"},
           {"MISSISSIPPI", "MS"},
           {"MISSOURI", "MO"},
           {"MONTANA", "MT"},
           {"NEBRASKA", "NE"},
           {"NEVADA", "NV"},
           {"NEW HAMPSHIRE", "NH"},
           {"NEW JERSEY", "NJ"},
           {"NEW MEXICO", "NM"},
           {"NEW YORK", "NY"},
           {"NORTH CAROLINA", "NC"},
           {"NORTH DAKOTA", "ND"},
           {"OHIO", "OH"},
           {"OKLAHOMA", "OK"},
           {"OREGON", "OR"},
           {"PENNSYLVANIA", "PA"},
           {"PUERTO RICO", "PR"},
           {"RHODE ISLAND", "RI"},
           {"SOUTH CAROLINA", "SC"},
           {"SOUTH DAKOTA", "SD"},
           {"TENNESSEE", "TN"},
           {"TEXAS", "TX"},
           {"UTAH", "UT"},
           {"VERMONT", "VT"},
           {"VIRGINIA", "VA"},
           {"WASHINGTON", "WA"},
           {"WEST VIRGINIA", "WV"},
           {"WISCONSIN", "WI"},
           {"WYOMING", "WY"},          
           {"Armed Forces (Africa,Canada,Europe,Middle East)", "AE"},
           {"Armed Forces America (Except Canada)", "AA"},
           {"Armed Forces Pacific", "AP"}};
            Application["states"] = states;
            //

            Dictionary<string, string> practiceHours = new Dictionary<string, string>()
            {
                {"1","40+"},
                {"2","20-39"},
                {"3","11-19"},
                {"4","1-10"},
                {"5","Not Applicable"}
            };
            Application["practiceHours"] = practiceHours;
            Dictionary<string, string> practiceSetting = new Dictionary<string, string>()
              {
                {"0",""},
                {"1","Military"},
                {"2","VA"},
                {"3","PHS"},
                {"4","HMO"},
                {"5","Hospital Based"},
                {"6","Solo"},
                {"7","Partnership/Group"},
                {"8","Other"},
                {"9","Research"},
                {"10","Medical School Faculty"},
                {"11","Direct Medical Care"},
                {"12","Not Applicable"} 
              };
            Application["practiceSetting"] = practiceSetting;

            //ACH
            decimal percentage = App_Code.DataAccess.getPercentage();
            decimal addon = App_Code.DataAccess.getAddon();
            decimal ach = App_Code.DataAccess.getAch();
            Application["percentage"] = percentage;
            Application["addon"] = addon;
            Application["ach"] = ach;


        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
        public static System.Reflection.Assembly CustomResolveEventHandler(object sender, System.ResolveEventArgs args)
        {
            System.Reflection.Assembly oReturn = null;
            string strMode = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("NICMode");

            string strAssemblyName = null;
            try
            {
                strAssemblyName = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            }
            catch
            {
                strAssemblyName = args.Name;
                try
                {
                    if (!args.Name.ToUpper().EndsWith(".DLL"))
                    {
                        strAssemblyName += ".dll";
                    }
                }
                catch { }
            }

            if (strAssemblyName != null)
            {
                if (strAssemblyName.ToUpper() == "AMS_MIDDLEWARE.DLL")
                {
                    string strAssemblyPath = null;

                    if (strMode == "DEV")
                    {
                        strAssemblyPath = @"\\software\TMB_Apps_Dev\Shared_DLLs\" + strAssemblyName;
                    }
                    else if (strMode == "TEST")
                    {
                        strAssemblyPath = @"\\software\TMB_Apps_Test\Shared_DLLs\" + strAssemblyName;
                    }
                    else if (strMode == "PROD")
                    {
                        strAssemblyPath = @"\\software\TMB_Apps\Shared_DLLs\" + strAssemblyName;
                    }

                    if (strAssemblyPath != null)
                    {
                        if (System.IO.File.Exists(strAssemblyPath))
                        {
                            oReturn = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(strAssemblyPath));
                        }
                    }
                }
                else if (strAssemblyName.ToUpper() == "DBO.DLL" || strAssemblyName.ToUpper() == "SHAREDASSEMBLYRESOLVER.DLL")  // SharedAssemblyResolver
                {
                    oReturn = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(@"\\software\TMB_Apps\Shared_DLLs\" + strAssemblyName));
                }
            }

            return oReturn;
        }

    }
}
