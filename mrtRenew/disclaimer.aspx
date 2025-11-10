<%@ Page Title="" Language="C#" MasterPageFile="~/mrtRenew.Master" AutoEventWireup="true" CodeBehind="disclaimer.aspx.cs" Inherits="mrtRenew.disclaimer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<h2>Disclaimer</h2>
<asp:Label ID="msgLBL" runat ="server" ></asp:Label>
<p>
    In order to complete the payment for your application, you will leave the TMB website and be 
    directed to the Texas.gov payment processing site. Texas.gov, the official website of 
    the State of Texas, processes online transactions on behalf of State Agencies. Your bill will
    indicate that this transaction has been charged to <asp:Label ID="CC_BANKlbl" runat ="server"  ></asp:Label>.
</p>
<p>No financial information is seen, processed, or stored by the Texas Medical Board.</p>
<p>
<strong>The payment portion of the online application system is handled by <a href="http://Texas.gov" >Texas.gov</a>,
 the official website of Texas. The price of this service includes funds that support the ongoing 
 operations and enhancements of <a href="http://Texas.gov" >Texas.gov</a>,
   which is provided by a third party in partnership with the State, as well as processing fees.
     <a href="http://Texas.gov" >Texas.gov</a> will remit the amount paid to the Texas Medical Board on your behalf.  Please note that
      the <a href="http://Texas.gov" >Texas.gov</a> portion is non-refundable.</strong>
</p>
<p><strong >The total amount you will pay will be <%= totalAmount %>.</strong></p>
<p>By checking here, I certify that I understand and accept the above terms of payment. In addition, I certify that the information I have provided on this form is true and correct.  Further, I have provided all necessary corrections and updates on this form.  I further understand that it is a violation of the Texas Penal Code, section 37.10, to submit a false statement to a government agency.
<asp:CheckBox ID ="acceptCB" runat ="server" AutoPostBack="true" />
</p> 
<p>
Please press the continue button to begin entering payment information (NOTE: the payment process may take several minutes to finish.  Please be patient and DO NOT click the back button or close your browser).
<asp:Button ID="continueBTN" runat ="server" Text ="Press to continue" 
        onclick="continueBTN_Click" />
</p>


</asp:Content>
