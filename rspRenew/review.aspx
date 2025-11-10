<%@ Page Title="" Language="C#" MasterPageFile="~/rspRenew.Master" AutoEventWireup="True" CodeBehind="review.aspx.cs" Inherits="rspRenew.review" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .hideRow
        {
            visibility: hidden;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3 >Review</h3>
<asp:Label ID="msgLBL" runat ="server" ></asp:Label>
<p>After you have reviewed the registration information submitted, please select the pay option at the bottom of the page to continue.  If you need to correct or add any information, please use the links on the left of the screen to navigate through the registration steps. </p>
<br /><br />
<table>
<tr><td>Name:</td><td><asp:Label ID="nameLBL" runat ="server" ></asp:Label></td></tr>
<tr><td>License Number:</td><td><asp:Label ID="licNumLBL" runat ="server" ></asp:Label></td></tr>
<tr><td>Expiration Date:</td><td><asp:Label ID="expirDateLBL" runat ="server" ></asp:Label></td></tr>
</table>
  <br />
<hr />
 <br />
 <table  class="blackBorder"  width ="400" style="border-collapse:collapse;" >
 <caption>Contact Information</caption>
 <tr><td>Email:</td><td><asp:Label ID="emailLBL" Columns="35" MaxLength ="70"    runat ="server" ></asp:Label></td></tr>
 <tr><td>Phone:</td><td><asp:Label ID="phoneLBL" runat ="server" ></asp:Label></td></tr>
 </table>
 <br />
<table class="blackBorder"  width ="400" style="border-collapse:collapse;" >
        <caption>Mailing Address</caption>
		<tr>
		<td class="label">Mailing Address 1<span class ="required">*</span>:</td>
		<td class="input"><asp:Label ID ="madd1LBL" runat ="server" Columns ="30" MaxLength ="30" ></asp:Label></td>
		</tr>
		<tr>
		<td class="label">Mailing Address 2:</td>
		<td class="input"><asp:Label ID ="madd2LBL" runat ="server" Columns ="30" MaxLength ="30"></asp:Label></td>
		</tr>
		<tr>
		<td class="label">Mailing City<span class ="required">*</span>:</td>
		<td class="input"><asp:Label ID ="mcityLBL" runat ="server" Columns ="30" MaxLength ="20" ></asp:Label></td>
		</tr>
		<tr>
		<td class="label"><label for="mailingState">Mailing State:<span class ="required">*</span></label></td>
		<td class="input"><asp:Label ID="State1LBL" runat ="server" ></asp:Label>
		</td>
		</tr>
		<tr>
		<td class="label">Mailing Zip Code<span class ="required">*</span>:</td>
		<td class="input"><asp:Label ID ="mZipLBL" runat ="server" ></asp:Label></td>
		</tr>
        <tr>
		<td>Country:<span class ="required">*</span></td>
		<td><asp:Label ID ="cntryLBL" runat ="server" ></asp:Label></td> </tr>			
        
       <%-- <tr class="hideRow"><td>Fax number:</td>
        <td><asp:Label ID="faxLBL" runat ="server" ></asp:Label></td></tr>--%>
        <%--<tr><td>Email:</td>
        <td><asp:Label ID="emailLBL" runat ="server" ></asp:Label></td></tr>--%>
		</table>
        <br /><br />
        <table class="blackBorder"  width ="400" style="border-collapse:collapse;" >
        <caption >Primary Practice Address</caption>
		<tr>
		<td>Primary Practice Address 1:</td>
		<td><asp:Label ID ="pAdd1" runat ="server" Columns ="30" MaxLength ="30"></asp:Label></td>
		</tr>
		<tr>
		<td class="label"><label for="businessAddress2">Primary Practice Address 2:</label></td>
		<td class="input"><asp:Label ID ="pAdd2" runat ="server" Columns ="30" MaxLength ="30"></asp:Label></td>
		</tr>
		<tr>
		<td class="label"><label for="businessCity">Primary Practice City:</label></td>
		<td><asp:Label ID ="pCity" runat ="server" Columns ="30" MaxLength ="20"></asp:Label></td>
		</tr>
		<tr>
		<td class="label"><label for="businessState">Primary Practice State:</label></td>
		<td class="input"><asp:Label ID="State2LBL" runat ="server" ></asp:Label>
		</td>
		</tr>
		<tr>
		<td class="label"><label for="businessZip">Primary Practice Zip Code:</label></td>
		<td class="input"><asp:Label ID ="pZip" runat ="server" ></asp:Label></td>
		</tr>
        <tr>
		<td>Country:</td>
		<td><asp:Label ID ="pcntryLBL" runat ="server" ></asp:Label></td> </tr>			
		<%--<tr class="hideRow"><td>Phone:</td><td><asp:Label ID="pPhoneLBL" runat ="server" ></asp:Label></td></tr>
        <tr class="hideRow"><td>Fax number:</td><td><asp:Label ID="pFaxLBL" runat ="server" ></asp:Label></td></tr>--%>
        <%--<tr><td>Email:</td><td><asp:Label ID="pEmailLBL" runat ="server" ></asp:Label></td></tr>--%>
        </table>

        <p><b>Questions</b></p>

<table cellpadding="3" cellspacing="0" border="0" width="600">
<tr>
<td>1.  Since your last registration or submission of your license application have you been arrested? (Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
<b><asp:Label ID="arrestLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
<td>2.  Since your last registration or submission of your license application have you been cited or ticketed for, or charged with any violation of the law? Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed. (Unless the offense involved alcohol or drugs, you may exclude: 1) traffic tickets; and, 2) violations with fines of $250 or less.) <span class ="required">*</span><br />
<b><asp:Label ID="cited_chargedLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
<td>3.  Since your last registration or submission of your license application are you currently the subject of a grand jury or criminal investigation? (Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
<b><asp:Label ID="crim_investigationsLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
<td>4.  Since your last registration or submission of your license application have you been convicted of an offense, placed on probation, or granted deferred adjudication or any type of pretrial diversion? Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed. (Unless the offense involved alcohol or drugs, you may exclude: 1) traffic tickets; and, 2) violations with fines of $250 or less.) <span class ="required">*</span><br />
<b><asp:Label ID="convictedLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
<td>5.  Since your last registration or submission of your license application, not including investigations or disciplinary actions by TMB, are there pending investigations, pending disciplinary matters or final disciplinary actions against you by any licensing agency or health-care entity? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
<b><asp:Label ID="investigationLBL" runat ="server" ></asp:Label></b></td>
</tr>
	
<%--<tr>
<td>6.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for any of the following: Major depressive disorder, bipolar disorder, schizophrenia, schizoaffective disorder, or any severe personality disorder? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
<b><asp:Label ID="depressedLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
	<td>7.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for either of the following: Alcohol or substance dependency or addiction? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
    <b><asp:Label ID="alcoholLBL" runat ="server" ></asp:Label></b></td>
</tr>
						
<tr>
	<td>8.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for a physical or neurological impairment? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
    <b><asp:Label ID="impairmentLBL" runat ="server" ></asp:Label></b></td>
</tr>

<tr>
<td>9.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for a sexual disorder, including, but not limited to pedophilia, exhibitionism, voyeurism, frotteurism, or sexual sadism? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class ="required">*</span><br />
<b><asp:Label ID="sexualLBL" runat ="server" ></asp:Label></b></td>
</tr>--%>

<tr>
        <td>6. Are you currently suffering from any condition for which you are not being appropriately treated that impairs your judgment or that would otherwise adversely affect your ability to practice medicine in a competent, ethical and professional manner?  <span class = "required">*</span><br />
            <b><asp:Label ID="impairmentLBL" runat ="server" ></asp:Label></b></td>
        </tr>    

		<tr>
			<td>7.  Blank at this time.
            </td>			
		</tr>
						
		<tr>
			<td>8. Blank at this time. 
        </td></tr>
       
		<tr>
		    <td>9. Blank at this time. </td>
		</tr>


<tr>
<td>10.  Have you had a professional liability claim or malpractice claim filed against you? <span class ="required">*</span><br /><b>
<b><asp:Label ID="malpracticeLBL" runat ="server" ></asp:Label></b></td> 
</tr>

<tr>
<td>11.  I certify that since my last registration, I have met current continuing education (CE)requirements for Respiratory Care Practitioners.<span class ="required">*</span><br />
<b><asp:Label ID="cmeLBL" runat ="server" ></asp:Label></b></td> 
</tr>

</table>
<br />
<div class="greyOne" >
<table  cellpadding="3" width="600">
<tr><td ><b>Primary Practice Information</b></td></tr>
<tr>
<td >12.  Indicate the total number of hours, on average, you practice each week (select one):<br />
<b><asp:Label ID="practice_hoursLBL" runat ="server" ></asp:Label></b></td> 
</tr>

<tr>
<td>13. Of your total practice hours, how many hours per week are spent in your primary practice location (select one):<br />
<b><asp:Label ID="primary_prac_hoursLBL" runat ="server" ></asp:Label></b></td> 
</tr>

<tr>
<td> 14. Enter Primary Practice Location County (Texas Only):<br/>
<b><asp:Label ID="ppCountyLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr>
<td>
15.  Select the Primary Practice Setting (select one): 
NOTE: If the option "Partnership/Group" is selected from the drop-down list, please fill in "Number in Group."<br />
<b><asp:Label ID="ppPracticeInfoLBL" runat ="server" ></asp:Label></b></td> 
</tr>
<tr>
<td>16.  If Partnership/Group, what is the number in the group? (1-3 digits max.) <br />
<b><asp:Label ID="ppNumber_in_groupLBL" runat ="server" ></asp:Label></b></td> 
</tr>

</table>
</div>
<div class ="greyOne ">
<table  cellpadding="3" width="600">
<tr><td ><b>Secondary Practice Information</b></td></tr>
<tr>
<td>17.  Enter Secondary Practice Location Zip Code (e.g., 12345): <br />
<b><asp:Label ID="spZipLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr><td>18.  Enter Secondary Practice Location County (Texas Only):<br />
<b><asp:Label ID="spCountyLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr><td>19.  Select the Secondary Practice Setting (select one): 
NOTE: If the option "Partnership/Group" is selected from the drop-down list, please fill in "Number in Group." <br />
<b><asp:Label ID="spPracticeInfoLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr><td>20.  If Partnership/Group, what is the number in the group? (1-3 digits max.)<br />
<b><asp:Label ID="spNumber_in_groupLBL" runat ="server" ></asp:Label></b></td>
</tr>
</table> 
</div>
<table  cellpadding="3" width="600">
<tr><td>21.  Select your race from the list provided: <span class ="required">*</span> <br />
<b><asp:Label ID="ethnicityLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr><td>22.  Are you of Hispanic origin? <span class ="required">*</span> <br />
<b><asp:Label ID="hispanic_originLBL" runat ="server" ></asp:Label></b></td>
</tr>
<tr><td>23.  If you are a Texas high school graduate, please select the county where your high school is located.<br />
<b><asp:Label ID="highSchoolCountyLBL" runat ="server" ></asp:Label></b></td>
</tr>
</table>
<br />
<asp:Button ID="goPayBTN" runat ="server" Text ="Go Pay" onclick="goPayBTN_Click" />
         


</asp:Content>
