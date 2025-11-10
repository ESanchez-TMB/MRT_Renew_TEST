using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;

namespace mrtRenew
{
    public partial class success7 : System.Web.UI.Page
    {
        string sessiontoken;
        RClassLibrary.renewal app;
        TxGov1.TxGov.PaymentResult pr = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            msgLBL.Text = "";
            msgLBL.ForeColor = System.Drawing.Color.Black;
            msgLBL.Visible = false;

            string failCode = "";
            //App_Code.NICqueryResult qr = null;
            string orderid = Request["orderid"];
            string sessiontoken = Request["sessiontoken"];

            msgLBL.Text = orderid;                  //not orderid, really the trace number order id is now last 7 digits, fdms never took time to understand NIC system

            if ((orderid == null) || (orderid.Length < 15) || (orderid.Length > 15))  // these are errors
            {
                Session["Query"] = "failure";
                App_Code.DataAccess.log(string.Format("Error MRT: Trace Number {0} is wrong format.", orderid), "Success7.aspx.cs");
                Session["errMessage"] = string.Format("Error MRT: Trace Number {0} is wrong format.", orderid);
                Server.Transfer("error.aspx", false);
            }

            Dictionary<string, string> trace_license = (Dictionary<string, string>)Application["trace_license"];
            string license = "";
            try
            {
                license = trace_license[orderid];
            }
            catch (KeyNotFoundException)
            {
                App_Code.DataAccess.log(string.Format("Error MRT: No match for {0} found in the Dictionary.", orderid), "Success7.aspx.cs");
                Session["errMessage"] = string.Format("Error MRT: No match for {0} found in the Dictionary.", orderid);
                Server.Transfer("error.aspx", false);
            }

            //1. grab the app from DB - not there - error
            //2. check app for status = Success, if there - error
            //3. do queryPayment - fail - error
            //                     succeed - mark app as ready to process
            //4. display receipt page - lots of work to get here

            //1.
            try
            {
                app = App_Code.Utilities.getWIPrenewal(license);
            }
            catch (Exception e1)
            {
                App_Code.DataAccess.log(string.Format("Error MRT: Retrieving WIP {0}", e1.Message), "Success7.aspx.cs");
                Session["errMessage"] = string.Format("Error MRT: Retrieving WIP {0}", e1.Message);
                Server.Transfer("error.aspx", false);
            }
            if (app == null)
            {
                App_Code.DataAccess.log("Error MRT:   WIP is null.", "Success7.aspx.cs");
                Session["errMessage"] = "Error MRT:   WIP is null.";
                Server.Transfer("error.aspx", false);
            }
            if (app.status == "Success")
            {
                App_Code.DataAccess.log("Error MRT:  WIP is already a Success.", "Success7.aspx.cs");
                Session["errMessage"] = "Error MRT:  WIP is already a Success.";
                Server.Transfer("error.aspx", false);
            }
            if (app.trace_number.Trim() == "")
            {
                App_Code.DataAccess.log("Error MRT:  WIP has no TraceNumber.", "MP Success7.aspx.cs");
                Session["errMessage"] = "Error MRT:  WIP has no TraceNumber.";
                Server.Transfer("error.aspx", false);
            }

            //3. do queryPayment               
            string serviceCode = System.Configuration.ConfigurationManager.AppSettings["NICservicecode"];
            try
            {
                //****************       
                fdms.Service1Client client = new fdms.Service1Client("fdms");
                string s2 = client.queryPayment(serviceCode, app.methodOfPayment, app.trace_number);  // this is the new call, fdms does not have 
                //****************
                pr = new TxGov1.TxGov.PaymentResult();
                var xs2 = new XmlSerializer(pr.GetType());
                var sr2 = new StringReader(s2);
                pr = (TxGov1.TxGov.PaymentResult)xs2.Deserialize(sr2);
            }
            catch (Exception ex)
            {
                App_Code.DataAccess.log(string.Format("Error MRT: callQueryPayment({0},{1},{2}) ", serviceCode, app.methodOfPayment, app.trace_number) + ex.Message, "success7");
                Session["errMessage"] = string.Format("Error MRT: callQueryPayment({0},{1},{2}) ", serviceCode, app.methodOfPayment, app.trace_number) + ex.Message;
                Server.Transfer("error.aspx", false);
            }



            int orderID = int.Parse(orderid.Substring(8, 7));
            RClassLibrary.DataAccess.updateOrderStatus(orderID, 2, "lmpRnl"); //query
            RClassLibrary.DataAccess.audit(orderID, "Success call QueryPayment: : " + app.trace_number, 2);
            app.status = "QueryPayment";
            App_Code.Utilities.putWIPrenewal(app);

            RClassLibrary.DataAccess.dumpInboundReceiptWcf(pr, sessiontoken, orderID, "TMB_LMP");                        //

            failCode = pr.FAILCODE ?? "";
            if (failCode.Trim() == "N")
            {
                locks.remove(trace_license, orderid);   //orderid is really tracenumber                
                Application["trace_license"] = trace_license; //put back in Application  - I don't think this is necessary reference and all
                RClassLibrary.DataAccess.audit(orderID, "Success return from QP: " + sessiontoken, 3);
                //RClassLibrary.DataAccess.updateOrderStatus(order_id, 3); //complete
                RClassLibrary.DataAccess.updateOrderStatus(orderID, 3, "lmpRnl");

                Session["Query"] = "success";
                app.status = "Success";
                app.TRANSACTION_DATE = DateTime.Now.ToString("yyyy/MM/dd");
                //string answer = App_Code.DataAccess.putApplDBwip(app);
                App_Code.Utilities.putWIPrenewal(app);
                Session["renewal"] = app;                   //save wip/app in session
                pr.LOCALREFID = app.trace_number;
                pr.RECEIPTDATE = app.TRANSACTION_DATE;
                NICUSA.commonCheckOut.PaymentResult npr = fixUpPayResult(pr);
                Session["QueryResult"] = npr;

                Server.Transfer("success3.aspx", false);
            }
            else
            {
                if (pr.FAILMESSAGE != null)
                {
                    string s = string.Format("Error MRT: QueryPayment bad status: {0}.", pr.FAILMESSAGE);
                    RClassLibrary.DataAccess.log(s, "Success4.aspx.cs");
                    Session["errMessage"] = s;
                    Server.Transfer("error.aspx");
                }
                else
                {
                    string s = "Error MRT: QueryPayment bad status.";
                    RClassLibrary.DataAccess.log(s, "Success4.aspx.cs");
                    Session["errMessage"] = s;
                    Server.Transfer("error.aspx");
                }
            }

        }//page load method
        private NICUSA.commonCheckOut.PaymentResult fixUpPayResult(TxGov1.TxGov.PaymentResult pr)
        {
            NICUSA.commonCheckOut.PaymentResult npr = new NICUSA.commonCheckOut.PaymentResult()
            {
                LOCALREFID = pr.LOCALREFID,
                RECEIPTDATE = pr.RECEIPTDATE,
                PAYTYPE = pr.PAYTYPE,
                BillingName = pr.BillingName,
                //ADDRESS1 = pr.ADDRESS1,
                //STATE = pr.STATE,
                //ZIP = pr.ZIP,
                TOTALAMOUNT = pr.TOTALAMOUNT
            };
            return npr;
        }// method fixupPayResult
    }//class
}//namespace