<%@ Page Title="" Language="C#" MasterPageFile="~/rspRenew.Master" AutoEventWireup="True" CodeBehind="Login.aspx.cs" Inherits="rspRenew.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script  type ="text/javascript" >
    function submitForm() {
        var msg = "";
        document.thisForm.LICENSE_NUMBER.value = document.thisForm.LICENSE_NUMBER.value.toUpperCase();
        var licLength = document.thisForm.LICENSE_NUMBER.value.length;
        var ssnLength = document.thisForm.SOCIAL_SECURITY_NUMBER.value.length;    
        if (licLength == 0) {
            msg += "License Number required\n";
        }

        if (ssnLength == 0) {
            msg += "Social Security Number required\n";
        }
        if (ssnLength != 4) {
            msg += "Social Security Number must be at least 4 characters\n";
        }
        if (isNaN(document.thisForm.SOCIAL_SECURITY_NUMBER.value - 1)) {
            msg += "Social Security Number must contain all numbers - e.g. 1234\n";
        }

        if (!msg) {
            document.thisForm.submit();

        }
        else {
            alert(msg);
            return;
        }
    }

    function isValidLength(s, length) {
        if (s == null || s.length != length)
            return true;
        for (var i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if ((c == ' ') || (c == '\n') || (c == '\t')) return true;
        }
        return false;
    }

    function isLengthLessThan(s, length) {
        if (s == null || s.length < length)
            return true;
        for (var i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if ((c == ' ') || (c == '\n') || (c == '\t')) return true;
        }
        return false;
    }
    function reset() {
        document.thisForm.LICENSE_NUMBER.value = "";
        document.thisForm.SOCIAL_SECURITY_NUMBER.value = "";
    }


    function popUp() {
        window.open('http://www.tmb.state.tx.us/page/renewal-eligibility', 'eligible', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=650,height=350');
    }


    //    function popUp() {
    //        window.open('eligibility.htm', 'eligible', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=650,height=350');
    //    }

    //    function popUpPA() {
    //        window.open('eligibilityAC.htm', 'eligible', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=650,height=350');
    //    }
 
</script>

<h3  >Login</h3>
       <p> <asp:Label ID ="errMsg" Text ="" runat ="server"></asp:Label> </p>
        <p>Ready to Register your license?  Check <a href="javascript:popUp()" ><b>eligibility</b></a>, and then enter your license number and your Social Security Number below.</p>
		<b>There are five simple steps to submit your online license renewal:</b>
		<ol>
		  <li>Enter your License Number and the last 4 digits of your Social Security Number (do not add spaces).</li>
		  <li>Review and confirm your contact information.</li>
		  <li>Review and update your license information.</li>
		  <li>Pay registration fees with MasterCard, Visa, Discover, American Express, or Electronic Check.</li>
		  <li>View and print receipt.</li>
		</ol>	
		<p>The State of Texas defines an "Occupational license" to mean "a license, certificate, registration, permit, or other form of authorization, including a renewal of the authorization, that a person must obtain to practice or engage in a particular business, occupation, or profession; or a facility must obtain before a particular business, occupation, or profession is practiced or engaged in within the facility". The use of the term "license" within this application is used as a blanket substitute for the terms certificate, registration, permit, or other form of authorization. Its use does not modify any rights, authorities, or responsibilities as provided for under the original document type.</p>

<asp:Panel ID="devPanel" runat ="server" >
<h2>This application is connected to TEST payment engine. 
Applications will not be processed by TMB. 
If this is a real application, please contact the Texas Medical Board for further instructions.</h2>
</asp:Panel> 

<div id="biennial" style="color:Red">
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

		
        <span class="input">License Number – you may use your DSHS legacy number, or the TMB format (e.g. 12345, RCP00012345)</span>
		<br />
        <asp:TextBox id="LICENSE_NUMBER"  size="11" value="" maxlength="20" tabindex="1" runat="server" > </asp:TextBox>
		<br /><br />
		Last 4 digits of Social&nbsp;Security&nbsp;Number&nbsp;(numbers&nbsp;only&nbsp;-&nbsp;e.g.&nbsp;1234)
		<br />
        <asp:TextBox  TextMode="Password" id="SOCIAL_SECURITY_NUMBER" runat="server"  size="10" value="" maxlength="4" tabindex="2"> </asp:TextBox>
		<br /><br />

		<asp:Button id="continue" runat="server"  Text ="Continue"  onclick="continue_Click" />


</asp:Content>
