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
    public partial class success4 : System.Web.UI.Page
    {
        string tokenFromSession;
        string orderID;
        int order_id;
        RClassLibrary.renewal app;

        protected void Page_Load(object sender, EventArgs e)
        {
            msgLBL.Text = "";
            msgLBL.ForeColor = System.Drawing.Color.Black;
            msgLBL.Visible = false;

            try
            {
                app = (RClassLibrary.renewal)Session["renewal"];
                //
                if ((app != null) && (app.status.ToUpper().Trim() == "SUCCESS"))
                {
                    Session["errMessage"] = "No changes are allowed after payment is made.";
                    Server.Transfer("error.aspx", false);
                }
                //
                if (!IsPostBack)
                {
                    //need to get the token
                    string token = Request["token"];
                    tokenFromSession = (string)Session["token"];
                    orderID = (string)Session["orderID"];
                    int x = RClassLibrary.DataAccess.getOrderIDbyToken(token);

                    if (token == tokenFromSession)
                    {
                        order_id = int.Parse(orderID);
                        //NICUSA.NICorder _no = new NICUSA.NICorder();
                        RClassLibrary.DataAccess.updateOrderStatus(x, 2, "mrtRnl"); //query
                        RClassLibrary.DataAccess.audit(order_id, "Success call QueryPayment: : " + token, 2);

                        app.status = "QueryPayment";
                        App_Code.Utilities.putWIPrenewal(app);

                        //****************                        
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
                        TxGov1.TxGov.PaymentResult pr = null;
                        try
                        {
                            //****************
                            s2 = client.queryPayment(token);
                            //****************

                            pr = new TxGov1.TxGov.PaymentResult();
                            var xs2 = new XmlSerializer(pr.GetType());
                            var sr2 = new StringReader(s2);
                            pr = (TxGov1.TxGov.PaymentResult)xs2.Deserialize(sr2);

                        }
                        catch (Exception ex)
                        {
                            App_Code.DataAccess.log("error MRT callQueryPayment: " + ex.Message, RClassLibrary.utilities.getLogInfo());
                            pr = null;
                        }
                        //********************



                        RClassLibrary.DataAccess.dumpInboundReceiptWcf(pr, token, order_id, "TMB_MRT");                        //

                        string failCode = pr.FAILCODE ?? "";
                        if (failCode.Trim() == "N")
                        {
                            RClassLibrary.DataAccess.audit(order_id, "Success return from QP: " + token, 3);
                            //RClassLibrary.DataAccess.updateOrderStatus(order_id, 3); //complete
                            RClassLibrary.DataAccess.updateOrderStatus(x, 3, "mrtRnl");

                            Session["Query"] = "success";

                            NICUSA.commonCheckOut.PaymentResult npr = fixUpPayResult(pr);
                            Session["QueryResult"] = npr;

                            app.status = "Success";
                            app.TRANSACTION_DATE = DateTime.Now.ToString("yyyy/MM/dd");
                            //string answer = App_Code.DataAccess.putApplDBwip(app);
                            App_Code.Utilities.putWIPrenewal(app);

                            Server.Transfer("success3.aspx", false);
                        }
                        else
                        {
                            string s = string.Format("Error MRT: QueryPayment bad status: {0}.", pr.FAILMESSAGE);
                            RClassLibrary.DataAccess.log(s, "Success4.aspx.cs");
                            Session["errMessage"] = s;
                            Server.Transfer("error.aspx");
                        }
                    }
                    else
                    {
                        // tokens don't match - 
                        RClassLibrary.DataAccess.log("Error MRT: Tokens do not match. Token=" + token + " s=" + tokenFromSession, "Success.aspx.cs");
                        Session["token"] = token;
                        Server.Transfer("success2.aspx?token=" + token, false); //todo: there is no success2
                    }
                }
                else
                {
                    RClassLibrary.DataAccess.log("Error MRT : Success.aspx page called as a postback.", "Success.aspx.cs");
                }
            }
            catch (Exception err)
            {
                RClassLibrary.DataAccess.log("Error MRT: " + err.Message, "Success4.aspx.cs");
            }
        }
        //
        private NICUSA.commonCheckOut.PaymentResult fixUpPayResult(TxGov1.TxGov.PaymentResult pr)
        {
            NICUSA.commonCheckOut.PaymentResult npr = new NICUSA.commonCheckOut.PaymentResult()
            {
                LOCALREFID = pr.LOCALREFID,
                RECEIPTDATE = pr.RECEIPTDATE,
                PAYTYPE = pr.PAYTYPE,
                BillingName = pr.BillingName,
                ADDRESS1 = pr.ADDRESS1,
                STATE = pr.STATE,
                ZIP = pr.ZIP,
                TOTALAMOUNT = pr.TOTALAMOUNT
            };
            return npr;

        }
    }
}