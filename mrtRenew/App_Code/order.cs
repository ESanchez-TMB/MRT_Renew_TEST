using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mrtRenew.App_Code
{
    public class order
    {
        public int order_id;
        public int customer_id;         
        public string order_status_cd;
        public string Order_verified_flg;


        public static order createOrder(RClassLibrary .renewal r)
        {   // this routine needs to insert an order, and order_items into db 
            // it returns the order
            try  
            {
                renewDB.Tb_customer customer =  DataAccess.getCustomer(r);
                if (customer == null) return null;
                renewDB.ARO_DEV01  db = new renewDB.ARO_DEV01 (System.Configuration.ConfigurationManager.AppSettings["DbConnectionA"]);
                renewDB.Tb_order newOrd = new renewDB.Tb_order()
                {
                    Customer_id = customer .Customer_id ,
                    Order_status_cd = "INIT",
                    Order_verified_flg = "N",
                    Create_dt = DateTime.Now,
                    Maint_dt = DateTime.Now,
                    Create_user = "mrtRnl",
                    Maint_user = "mrtRnl"
                };
                db.GetTable<renewDB.Tb_order>().InsertOnSubmit(newOrd);
                db.SubmitChanges();

                order o = new order();
                o.customer_id = newOrd.Customer_id;              
                o.order_id = newOrd.Order_id;
                o.order_status_cd = newOrd.Order_status_cd;
                o.Order_verified_flg = newOrd.Order_verified_flg;
                return o;
            }
            catch (Exception e)
            {
                DataAccess.log(e.Message, "mrt renew order.createOrder()");
                return null;
            }
        }
    }
}