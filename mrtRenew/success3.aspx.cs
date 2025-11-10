using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mrtRenew
{
    public partial class success3 : System.Web.UI.Page
    {


        protected NICUSA.commonCheckOut.PaymentResult qr;
        public RClassLibrary .renewal r;
        string licenseType;
        public string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            r = (RClassLibrary.renewal )Session["renewal"];
            qr = (NICUSA.commonCheckOut.PaymentResult)Session["QueryResult"];
             name = r.PAname;            
            //
            decimal totAmt = 0.0M;
            try
            {
                totAmt = decimal.Parse(qr.TOTALAMOUNT);
                qr.TOTALAMOUNT = totAmt.ToString("C");
            }
            catch
            {
                qr.TOTALAMOUNT = "Unknown";
            }
        }   

    }
}