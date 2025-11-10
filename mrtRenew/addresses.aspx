<%@ Page Title="" Language="C#" MasterPageFile="~/mrtRenew.Master" AutoEventWireup="true" CodeBehind="addresses.aspx.cs" Inherits="mrtRenew.addresses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideRow
        {
            visibility: hidden;
        }
    </style>
    <script type="text/javascript">

        function popUp() {
            window.open('https://sso.tmb.state.tx.us/login.aspx', 'eligible', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=650,height=350');
        }
   
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h2>Address and Contact Information</h2>
<p><asp:Label ID ="msgLBL" Text ="" runat ="server" ></asp:Label> </p>	 
<h3>Contact Information</h3>
<p>Please review the Contact information below.</p>

<p>Asterisk (<span class="required">*</span>) indicates response required.</p>
<p>To update your email address or contact phone number, please log into your <a href =javascript:popUp()><b>MyTMB</b></a> account and update your account information.  You can use this system to update your information at any time.  Please note that if you make those changes at this time, your renewal screen may not refresh to show the updated information.</p>


 <br />
<table class="tdData" cellpadding="3" cellspacing="0" border="0" width="100%">
<caption>Contact Information</caption>
<%--<tr><td>Email:<span class="required">*</span></td><td><asp:TextBox ID="emailTB" Columns="35" MaxLength ="70"    runat ="server" ></asp:TextBox></td></tr>
<tr><td>Phone:<span class="required">*</span> (999-999-9999)</td><td><asp:TextBox ID="phoneTB" runat ="server" ></asp:TextBox></td></tr>--%>
<tr><td>Email:</td><td><asp:Label ID="emailLBL"    runat ="server"  BackColor = "Cornsilk"></asp:Label></td></tr>
<tr><td>Phone: (999-999-9999)</td><td><asp:Label ID="phoneLBL" runat ="server" BackColor="Cornsilk" ></asp:Label></td></tr>
</table>
<hr />
 <br />
<h3>Address Information</h3>
<p>Please review the Address information below.</p>
<p>The renewed license will be sent to your Mailing Address. When entering a foreign address, select "Other" for State, and provide a Country.</p>
<p>Please note that licensee mailing/contact <span style="font-weight:bold">addresses</span> are considered public information and may be provided as part of an open records request, a TMB data product available for purchase or the TMB online verification.</p>
<p>Asterisk (<span class="required">*</span>) indicates response required.</p>            
	<table class="tdData" cellpadding="3" cellspacing="0" border="0" width="100%">
        <caption>Mailing Address</caption>
		<tr>
		<td class="label"><label for="mailingAddress1">Mailing Address 1:<span class="required">*</span></label></td>
		<td class="input"><asp:TextBox ID ="madd1TB" runat ="server" Columns ="30" MaxLength ="30" ></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="mailingAddress2">Mailing Address 2:</label></td>
		<td class="input"><asp:TextBox ID ="madd2TB" runat ="server" Columns ="30" MaxLength ="30"></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="mailingCity">Mailing City:<span class="required">*</span></label></td>
		<td class="input"><asp:TextBox ID ="mcityTB" runat ="server" Columns ="30" MaxLength ="20" ></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="mailingState">Mailing State:<span class="required">*</span></label></td>
		<td class="input"><asp:DropDownList ID="State1" runat ="server" ></asp:DropDownList>
		</td>
		</tr>
		<tr>
		<td class="label"><label for="mailingZip">Mailing Zip Code:<span class="required">*</span></label></td>
		<td class="input"><asp:TextBox ID ="mZipTB" MaxLength ="5"  runat ="server" ></asp:TextBox></td></tr>
        <tr>		
		<td>Country:<span class="required">*</span></td>
		<td><asp:DropDownList ID ="cntryDDL" runat ="server" ></asp:DropDownList></td> </tr>		
        
        <tr class="hideRow"><td>Fax: (999-999-9999)</td><td><asp:TextBox ID="faxTB" runat ="server" ></asp:TextBox></td></tr>
       <%-- <tr><td>Email:</td><td><asp:TextBox ID="emailTB" runat ="server" ></asp:TextBox></td></tr>--%>
		</table>
        <br /><br />
        <table class="tdData" cellpadding="3" cellspacing="0" border="0" width="100%">
        <caption >Primary Practice Address</caption>
		<tr>
		<td class="label"><label for="businessAddress1">Primary Practice Address 1:</label></td>
		<td class="input"><asp:TextBox ID ="pAdd1" runat ="server" Columns ="30" MaxLength ="30"></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="businessAddress2">Primary Practice Address 2:</label></td>
		<td class="input"><asp:TextBox ID ="pAdd2" runat ="server" Columns ="30" MaxLength ="30"></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="businessCity">Primary Practice City:</label></td>
		<td><asp:TextBox ID ="pCity" runat ="server" Columns ="30" MaxLength ="20"></asp:TextBox></td>
		</tr>
		<tr>
		<td class="label"><label for="businessState">Primary Practice State:</label></td>
		<td class="input"><asp:DropDownList ID="State2" runat ="server" ></asp:DropDownList>
		</td>
		</tr>
		<tr>
		<td class="label"><label for="businessZip">Primary Practice Zip Code:</label></td>
		<td class="input"><asp:TextBox ID ="pZip" MaxLength="5"   runat ="server" ></asp:TextBox></td>
		</tr>
        <tr>
		<td>Country:</td>
		<td><asp:DropDownList ID ="pcntryDDL" runat ="server" ></asp:DropDownList></td> </tr>		
		<tr class="hideRow"><td>Phone: (999-999-9999)</td><td><asp:TextBox ID="pPhoneTB" runat ="server" ></asp:TextBox></td></tr>
        <tr class="hideRow"><td>Fax: (999-999-9999)</td><td><asp:TextBox ID="pFaxTB" runat ="server" ></asp:TextBox></td></tr>
       <%-- <tr><td>Email:</td><td><asp:TextBox ID="pEmailTB" runat ="server" ></asp:TextBox></td></tr>--%>
        </table> 
        <br />
        <p>Upon completion of address information, select "Continue".</p>
        <asp:Button ID="continueBTN" Text ="Continue" runat ="server" 
        onclick="continueBTN_Click" />

</asp:Content>
