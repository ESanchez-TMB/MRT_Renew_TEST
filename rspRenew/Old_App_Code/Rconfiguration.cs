using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace rspRenew.App_Code
{
    public class Rconfiguration
    {

        // NIC
        public static string NICNCservicecode
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCservicecode"];
            }
        }
        public static string NICNCmerchantid
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCmerchantid"];
            }
        }
        public static string NICNCmerchantkey
        {
            get
            {
                //return @"8JQW$p0o@6zeM$#cs5";  // ConfigurationManager.AppSettings["NICNCmerchantid"];
                return ConfigurationManager.AppSettings["NICNCmerchantkey"];
            }
        }
        public static string NICACservicecode
        {
            get
            {
                return ConfigurationManager.AppSettings["NICACservicecode"];
            }
        }
        public static string NICACmerchantid
        {
            get
            {
                return ConfigurationManager.AppSettings["NICACmerchantid"];
            }
        }
        public static string NICACmerchantkey
        {
            get
            {
                return ConfigurationManager.AppSettings["NICACmerchantkey"];
            }
        }
        public static string NICservicecode
        {
            get
            {
                return ConfigurationManager.AppSettings["NICservicecode"];
            }
        }
        public static string NICmerchantid
        {
            get
            {
                return ConfigurationManager.AppSettings["NICmerchantid"];
            }
        }
        public static string NICmerchantkey
        {
            get
            {
                return ConfigurationManager.AppSettings["NICmerchantkey"];
            }
        }
        public static string NICstatecd
        {
            get
            {
                return ConfigurationManager.AppSettings["NICstatecd"];
            }
        }
        public static string NIClocalrefid
        {
            get
            {
                return ConfigurationManager.AppSettings["NIClocalrefid"];
            }
        }
        public static string NICcompanyname
        {
            get
            {
                return ConfigurationManager.AppSettings["NICcompanyname"];
            }
        }
        public static string NICcancel
        {
            get
            {
                return ConfigurationManager.AppSettings["NICcancel"];
            }
        }
        public static string NICduplicate
        {
            get
            {
                return ConfigurationManager.AppSettings["NICduplicate"];
            }
        }
        public static string NICfailure
        {
            get
            {
                return ConfigurationManager.AppSettings["NICfailure"];
            }
        }
        public static string NICsuccess
        {
            get
            {
                return ConfigurationManager.AppSettings["NICsuccess"];
            }
        }
        public static string NICNCcancel
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCcancel"];
            }
        }
        public static string NICNCduplicate
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCduplicate"];
            }
        }
        public static string NICNCfailure
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCfailure"];
            }
        }
        public static string NICNCsuccess
        {
            get
            {
                return ConfigurationManager.AppSettings["NICNCsuccess"];
            }
        }
        public static string DownLoadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DownLoadPath"];
            }
        }
        public static string NICDefault
        {
            get
            {
                return ConfigurationManager.AppSettings["NICDefault"];
            }
        }
        public static string NICMode
        {
            get
            {
                return ConfigurationManager.AppSettings["NICMode"];
            }
        }
    }
}