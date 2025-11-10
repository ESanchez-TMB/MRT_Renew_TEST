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

namespace rspRenew
{
    public partial class disclaimer4 : System.Web.UI.Page
    {


        public string totalAmount;
        decimal cfee = 0.0M;
        private string methodOfPayment;
        private decimal netFee;
        RClassLibrary.renewal r;
        string CC_BANK = "TMB RT Renew";
        renewDB.Tb_revenue R4403 = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            R4403 = RClassLibrary.DataAccess.getRevenueDB("4403");            
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


            if (r.methodOfPayment == "CC") //if ((ach == 0.0m) || (r.methodOfPayment == "CC"))
            {
                //cfee = App_Code.Utilities.getCfee(sub);
                //cfee = App_Code.Utilities.getCfee(r.CURRFEE);
                cfee = App_Code.Utilities.getCfee(r.CURRFEE + r.lateFee + R4403.Total_fee);  //10_10_16 + 10_3_23
            }
            else
            {
                cfee = (decimal)Application["ach"];
            }
            totalAmount = String.Format("{0:c}", (sub + cfee + R4403.Total_fee));



        }

        protected void continueBTN_Click(object sender, EventArgs e)
        {

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
                App_Code.DataAccess.log("Could not create order", "disclaimer continueBTN_Click");
                msgLBL.Text = "Error: Problem creating order.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            if (order.order_id == 0)
            {
                App_Code.DataAccess.log("did not create order", "disclaimer app.continueBTN_Click");
                msgLBL.Text = "Error: Problem creating order.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            //create item in order
            List<TxGov1.orderItem> orderItems = new List<TxGov1.orderItem>();
            TxGov1.orderItem item = new TxGov1.orderItem();
            item.orderID = order.order_id;

            item.item_cd = "RT_REN";  // 
            // order id is not actually used for anything -
            decimal sub = 0.0M;

            //sub = App_Code.DataAccess.getFee("6401");
            sub = r.CURRFEE;
            //r.total_amount = sub.ToString();
            item.orderItemID = 53;
            item.description = "TMB RT Renew";
            item.sku = "6403";

            //

            item.unitprice = r.CURRFEE;

            if (item.unitprice == 0)
            {
                App_Code.DataAccess.log("did not create order", "Zero amount");
                msgLBL.Text = "Error: Problem creating order - cannot pay zero amount.";
                msgLBL.ForeColor = System.Drawing.Color.Red;
                msgLBL.Visible = true;
                return;
            }
            //
            item.quantity = 1;
            orderItems.Add(item);

            if (r.lateFee > 0)
            {
                TxGov1.orderItem item_late = new TxGov1.orderItem();
                item_late.orderID = order.order_id;
                item_late.item_cd = "Resp Late Fee";
                item_late.orderItemID = 61; //arbitrary number
                item_late.description = "TMB Respiratiory Late Fee";
                item_late.unitprice = r.lateFee;
                item_late.sku = "6407";
                item_late.quantity = 1;
                orderItems.Add(item_late);
            }

            //HB1998 ************************************* all renewals get this
            //renewDB.Tb_revenue R4403 = RClassLibrary.DataAccess.getRevenueDB("4403");
            item = new TxGov1.orderItem();
            item.orderID = order.order_id;
            item.item_cd = "PHP";
            item.orderItemID = 41;  // this is not used
            item.description = R4403.Revenue_desc;
            item.sku = "4403";
            item.quantity = 1;
            item.unitprice = R4403.Total_fee;
            item.quantity = 1;
            orderItems.Add(item);
            //HB1998 *************************************

            //App_Code.DataAccess.insertOrderItems(orderItems);
            App_Code.DataAccess.insertOrderItemsWcf(orderItems);
            //            
            RClassLibrary.DataAccess.audit(order.order_id, "Success: create order: " + order.order_id.ToString(), 0);
            //create nic order

            TxGov1.NICorder no = new TxGov1.NICorder();
            //
            no.statecd = App_Code.Rconfiguration.NICstatecd;
            no.merchantid = App_Code.Rconfiguration.NICmerchantid;
            no.merchantkey = App_Code.Rconfiguration.NICmerchantkey;
            no.servicecode = App_Code.Rconfiguration.NICservicecode;
            //no.localrefid = App_Code.Utilities.calculate_RefID_rtRnl(order.order_id);
            no.localrefid = RClassLibrary .utilities .genTraceNum ("503RW", order.order_id);
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
            RClassLibrary.DataAccess.updateOrderStatus(order.order_id, 0, "RTrwl");
            //write app
            //string answer = App_Code.DataAccess.putApplDBwip(app);
            string answer = App_Code.Utilities.putWIPrenewal(r);
            if (answer != "success")
            {
                Session["error"] = "System incountered error writing to WIP from Disclaimer.";
                Server.Transfer("error.aspx", false);
            }
            //write final
            //answer = phyAppCL.DataAccess.putApplDBfinal(app, order.order_id);
            answer = App_Code.DataAccess.putFINAL(r);

            if (answer != "success")
            { RClassLibrary.DataAccess.log("Error could not write to final: " + answer, "RT renew disclaimer.aspx.cs"); }


            // ****************get the revenue records and put them into TxGov1.NICorder ie no.) ***************
            List<TxGov1.revenue> revs = new List<TxGov1.revenue>();
            foreach (TxGov1.orderItem oi in no.orderItems)
            {
                string revenue_code = oi.sku;
                revs.AddRange(RClassLibrary.DataAccess.getWcfRevenueCodes2(oi.unitprice, oi.sku));
            }// foreach orderItem 
            //2025_01_30 FDMS
            //revs.Add(RClassLibrary.DataAccess.getRevenue4CF("5022"));
            //revs.Add(RClassLibrary.DataAccess.getRevenue4CF("7000"));
            //no.revCodes = revs.ToArray();
            TxGov1.revenue rev = RClassLibrary.DataAccess.getRevenue4CF("5022");
            rev.fee_amt = cfee;
            revs.Add(rev);
            rev = RClassLibrary.DataAccess.getRevenue4CF("7000");
            rev.fee_amt = cfee;
            revs.Add(rev);
            no.revCodes = revs.ToArray();
            // done with revenue 

            //  *****   preparePay    *********
            var xs = new XmlSerializer(no.GetType());
            var sw = new StringWriter();
            xs.Serialize(sw, no);
            string s1 = sw.ToString();

            fdms.Service1Client client2 = new fdms.Service1Client("fdms");

            if (2 < 1)
            {

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
            }
            string s2 = "";
            try
            {
                //******************** prepare pay   *************
                //s2 = client.preparePay(s1);
                s2 = client2.preparePay(s1);
                //*******************  return prepare pay *************
                //put the object back together
                var xs2 = new XmlSerializer(no.GetType());
                var sr2 = new StringReader(s2);
                no = (TxGov1.NICorder)xs2.Deserialize(sr2);
            }
            catch (Exception err)
            {
                App_Code.DataAccess.log("Error RT: preparePayment failed " + err.Message, RClassLibrary.utilities.getLogInfo());
                Session["errMessage"] = "Internal error attempting to pay.";
                Response.Redirect("error.aspx");
            }
            //done with preparepay





            RClassLibrary.DataAccess.updateOrderStatus(order.order_id, 1, "rtRnl"); //preparePay
            //handle results
            if ((no.ppr.TOKEN != "0") && (no.ppr.TOKEN != null))
            {
                Session["token"] = no.ppr.TOKEN;
                Session["orderID"] = order.order_id.ToString();
                RClassLibrary.DataAccess.audit(order.order_id, " Success preparePay Token: " + no.ppr.TOKEN, 1); // 1/14/14 no.token to no.ppr.token

                //fdms
                Dictionary<string, string> trace_license = (Dictionary<string, string>)Application["trace_license"];
                locks.add(trace_license, r.trace_number, r.LicNum);    // should be thread safe on write 
                //fdms

                // eric's code 
                try
                {
                    //App_Code.DataAccess.put_outbound_payment_log(no);
                    RClassLibrary.DataAccess.putWcf_outbound_payment_log(no);
                }
                catch (Exception exc)
                {
                    App_Code.DataAccess.log("Error RT: " + exc.Message, "rt.disclaimer.continueBTN_Click");
                    //do nothing
                }
                //
                Response.Redirect(App_Code.Rconfiguration.NICDefault + no.ppr.TOKEN);
            }
            else
            {
                App_Code.DataAccess.log("Error RT: from NIC preparepay: " + no.ppr.ERRORMESSAGE, "disclaimer.aspx");
                Session["errMessage"] = no.ppr.ERRORMESSAGE;
                Server.Transfer("error.aspx");
            }
        }

    }
}