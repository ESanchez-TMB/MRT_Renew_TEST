using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rspRenew
{
    public partial class success : System.Web.UI.Page
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
                        NICUSA.NICorder _no = new NICUSA.NICorder();                       
                        RClassLibrary.DataAccess.updateOrderStatus(x, 2, "mrtRnl"); //query
                        RClassLibrary.DataAccess.audit(order_id, "Success call QueryPayment: : " + token, 2);
                        //****************
                        _no.pr = NICUSA.Class1.callQueryPayment(tokenFromSession, "BasicHttpBinding_IServiceWeb");
                        //****************
                        //
                        //RClassLibrary.DataAccess.audit(x, "Success return from QP: " + token, 3);
                        RClassLibrary.DataAccess.dumpInboundReceipt(_no.pr, token, order_id, "TMB_RCP");                        //

                        string failCode = _no.pr.FAILCODE ?? "";
                        if (failCode.Trim() == "N")
                        {
                            RClassLibrary.DataAccess.audit(order_id, "Success return from QP: " + token, 3);
                            //RClassLibrary.DataAccess.updateOrderStatus(order_id, 3); //complete
                            RClassLibrary.DataAccess.updateOrderStatus(x, 3, "mrtRnl");

                            Session["Query"] = "success";
                            Session["QueryResult"] = _no.pr;

                            app.status = "Success";
                            app.TRANSACTION_DATE = DateTime.Now.ToString("yyyy/MM/dd");
                            //string answer = App_Code.DataAccess.putApplDBwip(app);
                            App_Code.Utilities.putWIPrenewal(app);

                            Server.Transfer("success3.aspx", false);
                        }
                        else
                        {
                            string s = string.Format("Error: QueryPayment bad status: {0}.", _no.pr.FAILMESSAGE);
                            RClassLibrary.DataAccess.log(s, "Success.aspx.cs");
                            Session["errMessage"] = s;
                            Server.Transfer("error.aspx");
                        }
                    }
                    else
                    {
                        // tokens don't match - 
                        RClassLibrary.DataAccess.log("Error: Tokens do not match. Token=" + token + " s=", "Success.aspx.cs");
                        Session["token"] = token;
                        Server.Transfer("success2.aspx?token=" + token, false);
                    }
                }
                else
                {
                    RClassLibrary.DataAccess.log("Error: Success.aspx page called as a postback.", "Success.aspx.cs");
                }
            }
            catch (Exception err)
            {
                RClassLibrary.DataAccess.log("Error: " + err.Message, "Success.aspx.cs");
            }
        }

    }
}