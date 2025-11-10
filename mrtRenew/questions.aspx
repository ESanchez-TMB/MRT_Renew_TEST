<%@ Page Title="" Language="C#" MasterPageFile="~/mrtRenew.Master" AutoEventWireup="true" CodeBehind="questions.aspx.cs" Inherits="mrtRenew.questions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Questions</h3>
<asp:Label ID="msgLBL" runat ="server" ></asp:Label>
<p>Please answer all the questions below then select "Continue" to proceed.</p>
<p>Asterisk (<span class="required">*</span>) indicates response required.</p>
<br />
<table cellpadding="3" cellspacing="0" border="0" width="600">
	<tr>
		<td>1.  Since your last registration or submission of your license application have you been arrested? (Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class="required">*</span></td>
	</tr>
	<tr>
		<td >			
            <asp:RadioButtonList ID="arrestRBL1" RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td>2.  Since your last registration or submission of your license application have you been cited or ticketed for, or charged with any violation of the law? Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed. (Unless the offense involved alcohol or drugs, you may exclude: 1) traffic tickets; and, 2) violations with fines of $250 or less.) <span class="required">*</span></td>
	</tr>
	<tr>
		<td >
            <asp:RadioButtonList ID="cited_chargedRBL2"  RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
        </asp:RadioButtonList>
		</td>
	</tr>	
	<tr>
		<td>3.  Since your last registration or submission of your license application are you currently the subject of a grand jury or criminal investigation? (Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) 
<span class="required">*</span></td>
	</tr>
	<tr>
		<td >
            <asp:RadioButtonList ID="crim_investigationRBL3"  RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
        </asp:RadioButtonList>
		</td>
	</tr>	
	<tr>
		<td>4.  Since your last registration or submission of your license application have you been convicted of an offense, placed on probation, or granted deferred adjudication or any type of pretrial diversion? Please answer the question with regard to any action taken by any state, province, territory, U.S. federal jurisdiction, or country. If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed. (Unless the offense involved alcohol or drugs, you may exclude: 1) traffic tickets; and, 2) violations with fines of $250 or less.) 
<span class="required">*</span> </td>
	</tr>
	<tr>
		<td >
            <asp:RadioButtonList ID="convictedRBL4"  RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
        </asp:RadioButtonList>
		</td>
	</tr>
	
	<tr>
		<td>5.  Since your last registration or submission of your license application, not including investigations or disciplinary actions by TMB, are there pending investigations, pending disciplinary matters or final disciplinary actions against you by any licensing agency or health-care entity? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class="required">
*</span> </td>
	</tr>
	<tr>
		<td >
            <asp:RadioButtonList ID="investigationRBL5"  RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr>
	
    <%--		
	<tr>
		<td>6.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for any of the following: Major depressive disorder, bipolar disorder, schizophrenia, schizoaffective disorder, or any severe personality disorder? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) 
<span class="required">*</span> </td>
	</tr>
	<tr>
		<td >
            <asp:RadioButtonList ID="depressedRBL6"  RepeatDirection="Horizontal" runat ="server" >
                <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                <asp:ListItem Value="N" Text ="No"></asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr>
		<tr>
			<td>7.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for either of the following: Alcohol or substance dependency or addiction? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class="required">*</span> </td>
		</tr>
		<tr>
			<td>
                <asp:RadioButtonList ID="alcoholRBL7"  RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                    <asp:ListItem Value="N" Text ="No"></asp:ListItem>
                </asp:RadioButtonList>
			</td>
		</tr>
						
		<tr>
			<td>8.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for a physical or neurological impairment? (If yes, a follow up letter  will be sent to you at your listed mailing address after all applications have been processed.) <span class="required">
*</span> </td>
		</tr>
		<tr>
			<td>
                <asp:RadioButtonList ID="impairmentRBL8"  RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                    <asp:ListItem Value="N" Text ="No"></asp:ListItem>
                </asp:RadioButtonList>								
			</td>
		</tr>
		<tr>
		<td>9.  Since your last registration or submission of your license application, have you been diagnosed, treated, or admitted to a hospital or other facility for a sexual disorder, including, but not limited to pedophilia, exhibitionism, voyeurism, frotteurism, or sexual sadism? (If yes, a follow up letter will be sent to you at your listed mailing address after all applications have been processed.) <span class="required">*</span> </td>
		</tr>
		<tr>
			<td>
                <asp:RadioButtonList ID="sexualRBL9" RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                    <asp:ListItem Value="N" Text ="No"></asp:ListItem>
                </asp:RadioButtonList>
			</td>
		</tr>
        --%>

        <tr>
			<td>6. Are you currently suffering from any condition for which you are not being appropriately treated that impairs your judgment or that would otherwise adversely affect your ability to practice medicine in a competent, ethical and professional manner?  <span class = "required">*</span></td>
        </tr>
        <tr>

                <td>
                <asp:RadioButtonList ID="impairmentRBL6"  RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                    <asp:ListItem Value="N" Text ="No"></asp:ListItem>
                </asp:RadioButtonList>
                </td>
         </tr>
         <tr><td>								
			  

<p>The Texas Physician Health Program (TXPHP) is a confidential program that promotes wellness and the treatment of health conditions that 
may compromise the ability to practice with reasonable skill and safety.  TXPHP is a resource available for all licensees who may suffer 
from a condition that is or could impair their ability to practice.</p>

<p>TXPHP does not itself treat those who participate, but facilitates a participant’s treatment and provides monitoring as needed.
Examples of conditions that TXPHP can monitor include: substance abuse and addiction issues, mental health issues, and other 
medical conditions that may interrupt a licensee’s practice.  In addition to monitoring, TXPHP provides education, 
recognition, and assistance in diagnosis, treatment, and management of licensees’ potentially impairing conditions.
</p>

<p>You may contact TXPHP for further information on the program by calling (512) 305-7462 or via email at 
<a href = "mailto:info@txphp.state.tx.us">info@txphp.state.tx.us</a>. Downloadable self-report forms can be found on the TXPHP website, 
<a href ="http://www.txphp.state.tx.us/" target="_blank">http://www.txphp.state.tx.us/</a>, under the “Forms” section of the website.
</p>  
</td>
		</tr>

         <tr>
		    <td >7. Blank at this time.
        </td>
	    </tr>

		<tr>
			<td>8.  Blank at this time.
            </td>			
		</tr>

		<tr>
		    <td>9. Blank at this time. </td>
		</tr>

        <tr>
		<td>10.  Have you had a professional liability claim or malpractice claim filed against you? <span class="required">*</span> </td>
		</tr>
		<tr>
			<td>
                <asp:RadioButtonList ID="malpracticeRBL10" RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
                    <asp:ListItem Value="N" Text ="No"></asp:ListItem>
                </asp:RadioButtonList>
			</td>
		</tr>
        <tr>

		<td>11. I certify that since my last registration, I have met current continuing education (CE) requirements for Medical Radiologic Technologists or Limited Medical Radiologic Technologists, as appropriate.<span class="required">*</span>
        </td>
		</tr>
		<tr>
			<td>
                <asp:RadioButtonList ID="cmeRBL11" RepeatDirection="Horizontal" runat ="server" >
                    <asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>                    
                </asp:RadioButtonList>
			</td>
		</tr>
</table>
<br />
<table cellpadding="3" cellspacing="0" border="0" width="600">
<tr><td ><b>Primary Practice Information</b></td></tr>
<tr>
	<td >12.  Indicate the total number of hours, on average, you practice each week (select one):	</td>
</tr>
<tr>
	<td>
        <asp:DropDownList ID="PRACTICE_HOURS" runat ="server" >
        <asp:ListItem Value ="" Text="Please Select" ></asp:ListItem>                            
        <asp:ListItem Value ="1" Text ="40+" ></asp:ListItem>
        <asp:ListItem Value ="2" Text ="20-39" ></asp:ListItem>
        <asp:ListItem Value ="3" Text ="11-19" ></asp:ListItem>
        <asp:ListItem Value ="4" Text ="1-10" ></asp:ListItem>
        <asp:ListItem Value ="5" Text ="Not Applicable" ></asp:ListItem>         
        </asp:DropDownList>
	</td>
</tr>
<tr>
	<td>13. Of your total practice hours, how many hours per week are spent in your primary practice location (select one): </td>
</tr>
<tr>
	<td>		
        <asp:DropDownList ID="PRIMARY_PRAC_HOURS" runat ="server" >        
        <asp:ListItem Value ="" Text="Please Select" ></asp:ListItem>                            
        <asp:ListItem Value ="1" Text ="40+" ></asp:ListItem>
        <asp:ListItem Value ="2" Text ="20-39" ></asp:ListItem>
        <asp:ListItem Value ="3" Text ="11-19" ></asp:ListItem>
        <asp:ListItem Value ="4" Text ="1-10" ></asp:ListItem>
        <asp:ListItem Value ="5" Text ="Not Applicable" ></asp:ListItem>                             
        </asp:DropDownList>
	</td>
</tr>
<tr>
	<td> 14.  Enter Primary Practice Location County (Texas Only):<br/>						
        <asp:DropDownList ID="ppCOUNTY" runat ="server" >
        </asp:DropDownList>
	</td>
</tr>
<tr>
	<td align="left">
		15.  Select the Primary Practice Setting (select one): 
NOTE: If the option "Partnership/Group" is selected from the drop-down list, please fill in "Number in Group." 

	</td>
</tr>
<tr>
	<td >
    <asp:DropDownList ID ="ppPracticeInfo" runat ="server" >
    <asp:ListItem Value ="0" Text="" ></asp:ListItem>
    <asp:ListItem Value ="1" Text="Military" ></asp:ListItem>
    <asp:ListItem Value ="2" Text="VA" ></asp:ListItem>
    <asp:ListItem Value ="3" Text="PHS" ></asp:ListItem>
    <asp:ListItem Value ="4" Text="HMO" ></asp:ListItem>
    <asp:ListItem Value ="5" Text="Hospital Based" ></asp:ListItem>
    <asp:ListItem Value ="6" Text="Solo" ></asp:ListItem>
    <asp:ListItem Value ="7" Text="Partnership/Group" ></asp:ListItem>
    <asp:ListItem Value ="8" Text="Other" ></asp:ListItem>
    <asp:ListItem Value ="9" Text="Research" ></asp:ListItem>
    <asp:ListItem Value ="10" Text="Medical School Faculty" ></asp:ListItem>
    <asp:ListItem Value ="11" Text="Direct Medical Care" ></asp:ListItem>
    <asp:ListItem Value ="12" Text="Not Applicable" ></asp:ListItem>
    </asp:DropDownList>
    </td> 
    </tr>
    <tr><td>16.  If Partnership/Group, what is the number in the group? (1-3 digits max.) <br /> 	
        <asp:TextBox ID="ppNumber_in_group" MaxLength ="3" runat ="server" ></asp:TextBox>
	</td>
    </tr>
    <tr><td ><b>Secondary Practice Information</b></td></tr>
    <tr><td >Please answer the following regarding your secondary practice information. If applicable, answer each question to the best of your knowledge.</td></tr>
    <tr><td>17. Enter Secondary Practice Location Zip Code (e.g., 12345): <br />
    <asp:TextBox ID="spZip" MaxLength="5" runat ="server" ></asp:TextBox>
    </td></tr>
    <tr><td>18. Enter Secondary Practice Location County (Texas Only):<br />
    <asp:DropDownList ID="spCOUNTY" runat ="server" >
    </asp:DropDownList> 
</td></tr>
    <tr><td>19. Select the Secondary Practice Setting (select one): 
NOTE: If the option "Partnership/Group" is selected from the drop-down list, please fill in "Number in Group." <br />
<asp:DropDownList ID ="spPracticeInfo" runat ="server" >
    <asp:ListItem Value ="0" Text="" ></asp:ListItem>
    <asp:ListItem Value ="1" Text="Military" ></asp:ListItem>
    <asp:ListItem Value ="2" Text="VA" ></asp:ListItem>
    <asp:ListItem Value ="3" Text="PHS" ></asp:ListItem>
    <asp:ListItem Value ="4" Text="HMO" ></asp:ListItem>
    <asp:ListItem Value ="5" Text="Hospital Based" ></asp:ListItem>
    <asp:ListItem Value ="6" Text="Solo" ></asp:ListItem>
    <asp:ListItem Value ="7" Text="Partnership/Group" ></asp:ListItem>
    <asp:ListItem Value ="8" Text="Other" ></asp:ListItem>
    <asp:ListItem Value ="9" Text="Research" ></asp:ListItem>
    <asp:ListItem Value ="10" Text="Medical School Faculty" ></asp:ListItem>
    <asp:ListItem Value ="11" Text="Direct Medical Care" ></asp:ListItem>
    <asp:ListItem Value ="12" Text="Not Applicable" ></asp:ListItem>
    </asp:DropDownList>
</td></tr>
    <tr><td>20. If Partnership/Group, what is the number in the group? (1-3 digits max.)<br />
    <asp:TextBox ID="spNumber_in_group" MaxLength ="3" runat ="server" ></asp:TextBox> 
</td></tr>
<tr><td>21.  Select your race from the list provided: <span class="required">*</span> <br />
<asp:DropDownList ID="ethnicityDDL" runat ="server" >                               
</asp:DropDownList></td>
</tr>
<tr><td>22.  Are you of Hispanic origin? <span class="required">*</span> <br />
<asp:RadioButtonList ID="hispanic_originRBL" RepeatDirection="Horizontal" runat ="server" >
<asp:ListItem Value="Y" Text ="Yes"></asp:ListItem>
<asp:ListItem Value="N" Text ="No" Selected ="True" ></asp:ListItem>
</asp:RadioButtonList></td></tr>
<tr><td>23.  If you are a Texas high school graduate, please select the county where your high school is located.<br />
<asp:DropDownList id="highSchoolCountyDDL" runat ="server" >
</asp:DropDownList></td></tr>
</table>
<br />
<asp:Button id="continue" runat="server"  Text ="Continue" onclick="continue_Click" />


</asp:Content>
