using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rspRenew.App_Code
{
    public class cust_order
    {
        public int customer_id { get; set; }
        public int id_num { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public int order_id { get; set; }
        public string order_status_cd { get; set; }
        //public string Order_verified_flg { get; set; }

        public static List<cust_order> getAll()
        {
            {
                try
                {
                    renewDB.ARO_DEV01 db = new renewDB.ARO_DEV01(System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                    var customers = from c in db.Tb_customer
                                    from order in db.Tb_order
                                    where c.Customer_id == order.Customer_id
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
                    return new List<cust_order>();
                }
            }
        }
    }
}