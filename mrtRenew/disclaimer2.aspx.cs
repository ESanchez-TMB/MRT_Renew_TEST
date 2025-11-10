using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace mrtRenew
{
    public partial class disclaimer2 : System.Web.UI.Page
    {
        public string totalAmount;
        decimal cfee = 0.0M;
        private string methodOfPayment;
        private decimal netFee;
        RClassLibrary.renewal r;
        string CC_BANK = "TMB MRT Renew";

        protected void Page_Load(object sender, EventArgs e)
        {
            msgLBL.Visible = false;
            r = (RClassLibrary.renewal)Session["renewal"];
            if (r != null)
            {
                if (r.status.ToUpper().Trim() == "SUCCESS")
                {
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
            }
            else
            {
                Server.Transfer("login.aspx", false); //time out, no app record in session
            }
            methodOfPayment = r.methodOfPayment;
            //

            if (r != null)
            {
                populateControls();
            }
            else
            {
                msgLBL.Text = "Please Login again.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }

        }
        //       
        protected void populateControls()
        {
            decimal sub = 0.0M;
            try
            {
                sub = r.CURRFEE;
            }
            catch { }
            //                                DQ
            if (r.lateFee > 0)              //DQ
            {                               //DQ
                sub = sub + r.lateFee;      //DQ
            }                               //DQ
            //            
            if (methodOfPayment == "CC")
                CC_BANKlbl.Text = CC_BANK + " CC";
            else
                CC_BANKlbl.Text = CC_BANK + " ACH";

            //           
            decimal ach = (decimal)System.Web.HttpContext.Current.Application["ach"];
            if (r.methodOfPayment == "CC")  // if ((ach == 0.0m) || (r.methodOfPayment == "CC"))   9_1_18
            {
                //cfee = App_Code.Utilities.getCfee(sub);
                cfee = App_Code.Utilities.getCfee(r.CURRFEE + r.lateFee);  //10_10_16
            }
            else
            {
                cfee = (decimal)Application["ach"];
            }
            totalAmount = String.Format("{0:c}", (sub + cfee));


        }

        protected void continueBTN_Click(object sender, EventArgs e)
        {
            // this code uses TxGov WCF
            if (!acceptCB.Checked)
            {
                msgLBL.Text = "You cannot proceed until you check this box.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            //
            App_Code.order order = App_Code.order.createOrder(r);  //write 2 db,  2 get id #
            //
            if (order == null)
            {
                App_Code.DataAccess.log("MRT Could not create order", "disclaimer continueBTN_Click");
                msgLBL.Text = "Error: Problem creating order.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            if (order.order_id == 0)
            {
                App_Code.DataAccess.log("MRT did not create order", "disclaimer app.continueBTN_Click");
                msgLBL.Text = "Error: Problem creating order.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            //create item in order
            List<TxGov1.orderItem> orderItems = new List<TxGov1.orderItem>();
            TxGov1.orderItem item = new TxGov1.orderItem();
            item.orderID = order.order_id;
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
            if (item.unitprice == 0)
            {
                App_Code.DataAccess.log("MRT did not create order", "Zero amount");
                msgLBL.Text = "Error: Problem creating order - cannot pay zero amount.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            //
            item.quantity = 1;
            orderItems.Add(item);
            // add penalty (late) fee, if any 
            if (r.lateFee > 0)
            {
                if (r.LicNum.Substring(0, 3) == "GMR")
                {
                    TxGov1.orderItem item_late = new TxGov1.orderItem();
                    item_late.orderID = order.order_id;
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
                    TxGov1.orderItem item_late = new TxGov1.orderItem();
                    item_late.orderID = order.order_id;
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
            //App_Code.DataAccess.insertOrderItems(orderItems);
            App_Code.DataAccess.insertOrderItemsWcf(orderItems);
            //            
            RClassLibrary.DataAccess.audit(order.order_id, "Success: create order: " + order.order_id.ToString(), 0);


            //create nic order
            //string mode = App_Code.Rconfiguration.NICMode;
            //          

            TxGov1.NICorder no = new TxGov1.NICorder();
            //
            no.statecd = App_Code.Rconfiguration.NICstatecd;
            no.merchantid = App_Code.Rconfiguration.NICmerchantid;
            no.merchantkey = App_Code.Rconfiguration.NICmerchantkey;
            no.servicecode = App_Code.Rconfiguration.NICservicecode;
            no.localrefid = App_Code.Utilities.calculate_RefID_mrtRnl(order.order_id);
            //
            r.trace_number = no.localrefid;
            //
            no.uniquetransid = System.Guid.NewGuid().ToString();
            no.hrefcancel = App_Code.Rconfiguration.NICcancel;
            no.hrefduplicate = App_Code.Rconfiguration.NICduplicate;
            no.hrefsuccess = App_Code.Rconfiguration.NICsuccess;
            no.hreffailure = App_Code.Rconfiguration.NICfailure;
            no.orderID = order.order_id;
            no.orderItems = orderItems.ToArray<TxGov1.orderItem>();
            //11_20-17
            no.email = r.EMailAddr;
            //
            decimal total = 0;
            foreach (TxGov1.orderItem oi in no.orderItems)
            {
                total = total + (oi.unitprice * oi.quantity);
            }
            //
            no.cfee = cfee.ToString("f");
            no.amount = (total + cfee).ToString("f");
            r.totDue = total + cfee;

            no.paytype = methodOfPayment;
            //call preparePay -status
            //App_Code.DataAccess.updateOrderStatus(order.order_id, 0); //preparePay           
            RClassLibrary.DataAccess.updateOrderStatus(order.order_id, 0, "mrtRnl");
            //write app
            //string answer = App_Code.DataAccess.putApplDBwip(app);
            string answer = App_Code.Utilities.putWIPrenewal(r);
            if (answer != "success")
            {
                RClassLibrary.DataAccess.log("MRT Error could not write to final: " + answer, "MRT renew disclaimer2.aspx.cs");
                Session["error"] = "System incountered error writing to WIP from Disclaimer.";
                Server.Transfer("error.aspx", false);
            }
            //write final
            //answer = phyAppCL.DataAccess.putApplDBfinal(app, order.order_id);
            answer = App_Code.DataAccess.putFINAL(r);

            if (answer != "success")
            { RClassLibrary.DataAccess.log("Error could not write to final: " + answer, "MRT renew disclaimer2.aspx.cs"); }


            // ****************get the revenue records and put them into NICorder ie no.) ***************
            List<TxGov1.revenue> revs = new List<TxGov1.revenue>();
            foreach (TxGov1.orderItem oi in no.orderItems)
            {
                string revenue_code = oi.sku;
                revs.AddRange(RClassLibrary.DataAccess.getWcfRevenueCodes2(oi.unitprice, oi.sku));
            }// foreach orderItem 
            revs.Add(RClassLibrary.DataAccess.getRevenue4CF("5022"));
            revs.Add(RClassLibrary.DataAccess.getRevenue4CF("7000"));
            no.revCodes = revs.ToArray();
            // done with revenue 


            //  *****   preparePay    *********
            var xs = new XmlSerializer(no.GetType());
            var sw = new StringWriter();
            xs.Serialize(sw, no);
            string s1 = sw.ToString();

            TxGov.Service1Client client = null;

            string mode = App_Code.Rconfiguration.NICMode;
            if (mode == "PROD")
            {
                client = new TxGov.Service1Client("prod");
            }
            else
            {
                client = new TxGov.Service1Client("dev");
            }
            string s2 = "";
            try
            {
                //******************** prepare pay   *************
                s2 = client.preparePay(s1);
                //*******************  return prepare pay *************
                //put the object back together
                var xs2 = new XmlSerializer(no.GetType());
                var sr2 = new StringReader(s2);
                no = (TxGov1.NICorder)xs2.Deserialize(sr2);
            }
            catch (Exception err)
            {
                App_Code.DataAccess.log("Error MRT: preparePayment failed " + err.Message, RClassLibrary.utilities.getLogInfo());
                Session["errMessage"] = "Internal error attempting to pay.";
                Response.Redirect("error.aspx");
            }


            //update order
            RClassLibrary.DataAccess.updateOrderStatus(order.order_id, 1, "mrtRnl"); //preparePay

            //handle results
            if ((no.ppr.TOKEN != "0") && (no.ppr.TOKEN != null))
            {
                Session["token"] = no.ppr.TOKEN;
                Session["orderID"] = order.order_id.ToString();
                RClassLibrary.DataAccess.audit(order.order_id, " Success preparePay Token: " + no.ppr.TOKEN, 1); // 1/14/14 no.token to no.ppr.token
                // eric's code 
                try
                {
                    //App_Code.DataAccess.put_outbound_payment_log(no);
                    RClassLibrary.DataAccess.putWcf_outbound_payment_log(no);
                }
                catch (Exception exc)
                {
                    App_Code.DataAccess.log("Error MRT: " + exc.Message, "mrt.disclaimer2.continueBTN_Click");
                    //do nothing
                }
                //
                Response.Redirect(App_Code.Rconfiguration.NICDefault + no.ppr.TOKEN);
            }
            else
            {
                App_Code.DataAccess.log("Error MRT: from NIC preparepay: " + no.ppr.ERRORMESSAGE, "disclaimer.aspx");
                Session["errMessage"] = no.ppr.ERRORMESSAGE;
                Server.Transfer("error.aspx");
            }

        }
        //

    }
}