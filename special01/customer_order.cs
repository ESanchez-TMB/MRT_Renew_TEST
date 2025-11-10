using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace special01
    {
    public class customer_order
        {
        public int customer_id { get; set; }
        public string board_order_nbr { get; set; } //public string php_log_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int order_id { get; set; }
        public string order_status_cd { get; set; }
        public string item { get; set; }
        public decimal amount { get; set; }
        public DateTime orderDate { get; set; }
        public int compliance_officer_id { get; set; }
        }
    }
