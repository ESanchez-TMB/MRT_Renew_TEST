using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace processRenewApps
{
    class Items
    {
        
    }

    public interface itemCreator
    {
        List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r);
            
    }

    public class LMPorderItems : itemCreator
    {
        public List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r)
        {
            
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            //
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(r.trace_number.Length - 6));
            item.orderID = order_id;
            item.item_cd = "LMP_REN";
            //
            int count = 0;
            if (r.DRP) count++;
            if (r.TRP) count++;
            if (r.MNP) count++;
            if (r.MHP) count++;
            // order id is not actually used for anything -
            decimal sub = 0.0M;
            //sub = App_Code.DataAccess.getFee("6401");
            sub = r.CURRFEE;
            //r.total_amount = sub.ToString();
            item.orderItemID = 52;
            item.description = "TMB Physicist Renew";
            item.sku = "6102";
            item.unitprice = r.CURRFEE;
            //
            item.quantity = 1;
            orderItems.Add(item);
            //deal with additional specialities
            NICUSA.orderItem item_specialty = createOrderItemAddSpecialty(order_id); //this is only a lmp item (this might be difficult to maintain)

            for (int i = 1; i < count; i++)
            {
                orderItems.Add(item_specialty);
            }
            // add penalty (late) fee, if any - begin
            if (r.lateFee > 0)
            {
                NICUSA.orderItem item_late = new NICUSA.orderItem();
                item_late.orderID = order_id;
                item_late.item_cd = "MP LATE FEE";
                item_late.orderItemID = 60; //arbitrary number
                item_late.description = "TMB Physicist Late Fee";
                item_late.unitprice = r.lateFee;
                item_late.sku = "6115";
                item_late.quantity = 1;
                orderItems.Add(item_late);
            }
            // add penalty (late) fee - end

            //HB1998
            //if (DateTime.Now > DateTime.Parse("9/1/2023"))
            //{
            //    renewDB.Tb_revenue R4404 = RClassLibrary.DataAccess.getRevenueDB("4404"); //not sure this will work because of transaction
            //    NICUSA.orderItem lpPHP = new NICUSA.orderItem();
            //    lpPHP.orderID = order_id;
            //    lpPHP.item_cd = "PHP";
            //    lpPHP.orderItemID = 41; //arbitrary number
            //    lpPHP.description = R4404.Revenue_desc;
            //    lpPHP.unitprice = R4404.Total_fee;
            //    lpPHP.sku = "4404";
            //    lpPHP.quantity = 1;
            //    orderItems.Add(lpPHP);               
            //}
            //HB1998

            return orderItems;

        }
        //        
        public static NICUSA.orderItem createOrderItemAddSpecialty(int orderID)
        {
            NICUSA.orderItem item = new NICUSA.orderItem();
            item.orderID = orderID;
            item.item_cd = "LMP_RNW";
            item.orderItemID = 48;
            item.description = "LMP Add Spec";
            item.sku = "6106";
            item.quantity = 1;
            item.unitprice = RClassLibrary.DataAccess.getFee("6106");
            return item;
        }
    }

    public class MRTorderItems : itemCreator
    {
        public List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r)
        {
            //create item in order
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(r.trace_number.Length - 6));
            item.orderID = order_id;
            
            item.item_cd = "MRT_REN";
            decimal sub = 0.0M;
            sub = r.CURRFEE;
            //r.total_amount = sub.ToString();
            item.orderItemID = 50;
            item.description = "TMB MRT renew";
            if (r.LicNum.Substring(0, 3) == "GMR")
                item.sku = "6202";
            else if (r.LicNum.Substring(0, 3) == "LMR")
                item.sku = "6224";
            //
            item.unitprice = r.CURRFEE;
            //
            
            item.quantity = 1;
            orderItems.Add(item);

            //2025_06_10 hard to keep up, but as of today we are correcting this fee, so we need to write a cash record for it.
            // I chg this by removing the comments making it live again.
            //
            //hb1998
            if (DateTime.Now > DateTime.Parse("9/1/2023"))
            {
                renewDB.Tb_revenue R4403 = RClassLibrary.DataAccess.getRevenueDB("4403"); //not sure this will work because of transaction
                NICUSA.orderItem mrtPHP = new NICUSA.orderItem();
                mrtPHP.orderID = order_id;
                mrtPHP.item_cd = "PHP";
                mrtPHP.orderItemID = 41; //arbitrary number
                mrtPHP.description = R4403.Revenue_desc;
                mrtPHP.unitprice = R4403.Total_fee;
                mrtPHP.sku = "4403";
                mrtPHP.quantity = 1;
                orderItems.Add(mrtPHP);
            }
            //hb1998
            //2025_06_10

            // add penalty (late) fee, if any 
            if (r.lateFee > 0)
            {
                if (r.LicNum.Substring(0, 3) == "GMR")
                {
                    NICUSA.orderItem item_late = new NICUSA.orderItem();
                    item_late.orderID = order_id;
                    item_late.item_cd = "GMR LATE FEE";
                    item_late.orderItemID = 61; //arbitrary number
                    item_late.description = "TMB Radiologist Late Fee";
                    item_late.unitprice = r.lateFee;
                    item_late.sku = "6218";
                    item_late.quantity = 1;
                    orderItems.Add(item_late);
                }
                else if (r.LicNum.Substring(0, 3) == "LMR")
                {
                    NICUSA.orderItem item_late = new NICUSA.orderItem();
                    item_late.orderID = order_id;
                    item_late.item_cd = "LMR LATE FEE";
                    item_late.orderItemID = 62; //arbitrary number
                    item_late.description = "TMB Radiologist Late Fee";
                    item_late.unitprice = r.lateFee;
                    item_late.sku = "6225";
                    item_late.quantity = 1;
                    orderItems.Add(item_late);
                }
            }
            // add penalty (late) fee 
            return orderItems;
        }

    }
    public class NCTorderItems : itemCreator
    {
        public List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r)
        {
            //create item in order
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(r.trace_number.Length - 6));
            item.orderID = order_id;

            item.item_cd = "NR_REN";
            // order id is not actually used for anything -
            decimal sub = 0.0M;

            //sub = App_Code.DataAccess.getFee("6401");
            sub = r.CURRFEE;
            //r.total_amount = sub.ToString();
            item.orderItemID = 50;
            item.description = "TMB NCT reg renew";
            item.sku = "6214";
            item.unitprice = r.CURRFEE;
            //           
            item.quantity = 1;
            orderItems.Add(item);

            //hb1998
            if (DateTime.Now > DateTime.Parse("9/1/2023"))
            {
                renewDB.Tb_revenue R4403 = RClassLibrary.DataAccess.getRevenueDB("4403"); //not sure this will work because of transaction
                NICUSA.orderItem nctPHP = new NICUSA.orderItem();
                nctPHP.orderID = order_id;
                nctPHP.item_cd = "PHP";
                nctPHP.orderItemID = 41; //arbitrary number
                nctPHP.description = R4403.Revenue_desc;
                nctPHP.unitprice = R4403.Total_fee;
                nctPHP.sku = "4403";
                nctPHP.quantity = 1;
                orderItems.Add(nctPHP);
            }
            //hb1998

            // DQ no applied to nct reg for some reason 3_1_16 ????? who is driving???????
            if (r.lateFee > 0)
            {
                NICUSA.orderItem item_late = new NICUSA.orderItem();
                item_late.orderID = order_id;
                item_late.item_cd = "NCT R LATE FEE";
                item_late.orderItemID = 61; //arbitrary number
                item_late.description = "TMB NCTR Late Fee";
                item_late.unitprice = r.lateFee;
                item_late.sku = "6219";
                item_late.quantity = 1;
                orderItems.Add(item_late);
            }
            return orderItems;
        }
        
    }
    //
    public class PRForderItems : itemCreator
    {
        public List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r)
        {
            //create item in order
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(r.trace_number.Length - 6));
            item.orderID = order_id; // order id is not actually used for anything -
            item.item_cd = "PF_REN";              
            decimal sub = 0.0M;           
            sub = r.CURRFEE;           
            item.orderItemID = 51;
            item.description = "TMB Perfusion Renew";
            item.sku = "6304";
            item.unitprice = r.CURRFEE;             
            item.quantity = 1;
            orderItems.Add(item);

            //hb1998
            //if (DateTime.Now > DateTime.Parse("9/1/2023"))
            //{
            //    renewDB.Tb_revenue R4403 = RClassLibrary.DataAccess.getRevenueDB("4403"); //not sure this will work because of transaction
            //    NICUSA.orderItem pfPHP = new NICUSA.orderItem();
            //    pfPHP.orderID = order_id;
            //    pfPHP.item_cd = "PHP";
            //    pfPHP.orderItemID = 41; //arbitrary number
            //    pfPHP.description = R4403.Revenue_desc;
            //    pfPHP.unitprice = R4403.Total_fee;
            //    pfPHP.sku = "4403";
            //    pfPHP.quantity = 1;
            //    orderItems.Add(pfPHP);
            //}
            //hb1998

            if (r.lateFee > 0)
            {
                NICUSA.orderItem item_late = new NICUSA.orderItem();
                item_late.orderID = order_id;
                item_late.item_cd = "Perfusionist Late Fee";
                item_late.orderItemID = 61; //arbitrary number
                item_late.description = "TMB Perfusionist Late Fee";
                item_late.unitprice = r.lateFee;
                item_late.sku = "6306";
                item_late.quantity = 1;
                orderItems.Add(item_late);
            }
            return orderItems;
        }
    }
    public class RSPorderItems : itemCreator
    {
        public List<NICUSA.orderItem> createOrderItems(RClassLibrary.renewal r)
        {
            logger log;
            log = logger.getLogger();

            //create item in order
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(r.trace_number.Length - 6));
            item.orderID = order_id; // order id is not actually used for anything -
            item.item_cd = "RT_REN";  // 
            // order id is not actually used for anything -
            decimal sub = 0.0M;
            //sub = App_Code.DataAccess.getFee("6401");
            sub = r.CURRFEE;
            //r.total_amount = sub.ToString();
            item.orderItemID = 53;
            item.description = "TMB RT Renew";
            item.sku = "6403";
            item.unitprice = r.CURRFEE;
            item.quantity = 1;
            orderItems.Add(item);

            //hb1998
            renewDB.Tb_revenue R4403 = RClassLibrary.DataAccess.getRevenueDB("4403");   //this valueis not being picked up correctly unitprice
            log.log("R4403: Total_fee:" + R4403 .Total_fee.ToString ("C"));
            NICUSA.orderItem rtPHP = new NICUSA.orderItem();
            rtPHP.orderID = order_id;
            rtPHP.item_cd = "PHP";
            rtPHP.orderItemID = 41; //arbitrary number
            rtPHP.description = R4403.Revenue_desc;

            rtPHP.unitprice = R4403.Total_fee;
            log.log (string.Format("unitprice: {0}, total_fee: {1}", rtPHP.unitprice .ToString ("C"), R4403 .Total_fee .ToString ("C")));

            rtPHP.sku = "4403";
            rtPHP.quantity = 1;
            orderItems.Add(rtPHP);
            //hb1998

            if (r.lateFee > 0)
            {
                NICUSA.orderItem item_late = new NICUSA.orderItem();
                item_late.orderID = order_id;
                item_late.item_cd = "Resp Late Fee";
                item_late.orderItemID = 61; //arbitrary number
                item_late.description = "TMB Respiratiory Late Fee";
                item_late.unitprice = r.lateFee;
                item_late.sku = "6407";
                item_late.quantity = 1;
                orderItems.Add(item_late);
            }
            return orderItems;

        }

    }
}
