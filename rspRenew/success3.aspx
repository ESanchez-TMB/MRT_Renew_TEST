<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="success3.aspx.cs" Inherits="rspRenew.success3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    
    <img src="images/StateSeal.jpg" alt="Texas Medical Board" style=" padding:20px 20px 20px 25px; width:auto; height:auto;"   />

    <form id="form1" runat="server">
    <div>

     
<div id="biennial"  style="color:Red">
	<b>NEW - print or download your electronic license</b>	       
	</div> 

<p>
The Texas medical Board is transitioning to electronic licenses for a more paperless experience and to allow for enhanced licensee control. 
As of <b>9/1/2019 the Board will no longer issue paper licenses aftter a completed registration/renewal for your license type.</b> 
</p>
<p>Once your registration/renewal is complete, <b>please allow 2 business days for processing.</b>  
After that you will be able to log into your <a href ="https://sso.tmb.state.tx.us/login.aspx">MyTMB</a> account and view, save or print a copy of your active license.
</p>
<br />


   <p>Receipt: <br />
Send written changes to: <br />  
Texas Medical Board<br />  
P.O. Box 2029<br />  
MC-906<br />  
Austin, TX  78768-2029<br />  
Fax:  888-790-0621<br />  
</p>

<table>
<tr><td>Trace Number</td><td><% =qr.LOCALREFID %> </td></tr>
<tr><td>Transaction Date</td><td><% =qr.RECEIPTDATE %></td></tr>
<tr><td>Pay Type</td><td><% =qr.PAYTYPE %></td></tr>
<%--<tr><td>Name:</td><td><% =r.first_name + " " + r.last_name    %></td></tr>--%>
<tr><td>Name:</td><td><% = name %></td></tr>
<tr><td>Billing Name:</td><td><% =qr.BillingName  %></td></tr>
<tr><td>Billing Address:</td><td><% =qr.ADDRESS1  %></td></tr>
<tr><td>Billing State:</td><td><% =qr.STATE  %></td></tr>
<tr><td>Billing Zip Code:</td><td><% =qr.ZIP  %></td></tr>
<%--<tr><td>Registration Fee:</td><td><% =r.AmtDue.ToString("c")  %></td></tr>--%>
<tr><td>Late Fee:</td><td><% = r.lateFee.ToString ("c")  %></td></tr>
<tr><td>Total paid:</td><td><% =qr.TOTALAMOUNT  %></td></tr>

</table>
<p></p>

    </div>
    </form>
</body>
</html>
