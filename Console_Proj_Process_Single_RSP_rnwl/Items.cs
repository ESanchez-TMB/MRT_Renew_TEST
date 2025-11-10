using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console_Proj_Process_Single_RSP_rnwl
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
            order_id = int.Parse(r.trace_number.Substring(8, 6));
            item.orderID = order_id;
            item.item_cd = "LMP_REN";
            //
            int count = 0;
            if (r.DRP)
                count++;
            if (r.TRP)
                count++;
            if (r.MNP)
                count++;
            if (r.MHP)
                count++;
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
            order_id = int.Parse(r.trace_number.Substring(8, 6));
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
            order_id = int.Parse(r.trace_number.Substring(8, 6));
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
            order_id = int.Parse(r.trace_number.Substring(8, 6));
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
            //create item in order
            List<NICUSA.orderItem> orderItems = new List<NICUSA.orderItem>();
            NICUSA.orderItem item = new NICUSA.orderItem();
            int order_id = 0;
            order_id = int.Parse(r.trace_number.Substring(8, 6));
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
