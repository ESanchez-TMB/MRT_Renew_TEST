using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mrtRenew.App_Code
{
    public class revenue_masters
    {
        public static List<revenue_master> revenues;

        static revenue_masters()
        {
            revenues = DataAccess.getRevenues();
        }
    }
    public class revenue
    {
        public string revenue_code;
        public int fee_nbr;
        public string aobj;
        public System.DateTime begin_dt;
        public System.Nullable<System.DateTime> end_dt;
        public decimal fee_amt;
        public string fund;
        public string cobj;
        public string pca;
        public string trans_code;
        public string revenue_desc;
        public string create_user;
        public System.DateTime create_dt;
        public string maint_user;
        public System.DateTime maint_dt;
    }
    public class revenue_master
    {
        public string revenue_code;
        public int fee_nbr;
        public decimal total_fee;
        public System.DateTime begin_dt;
        public System.Nullable<System.DateTime> end_dt;
        public string revenue_desc;
        public string create_user;
        public System.DateTime create_dt;
        public string maint_user;
        public System.DateTime maint_dt;
    }
    public class revenue_detail
    {
        public string revenue_code;
        public int fee_nbr;
        public string aobj;
        public System.DateTime begin_dt;
        public decimal fee_amt;
        public string fund;
        public string cobj;
        public string pca;
        public string trans_code;
        public string create_user;
        public System.DateTime create_dt;
        public string maint_user;
        public System.DateTime maint_dt;
    }
}