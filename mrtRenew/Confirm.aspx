<%@ Page Title="" Language="C#" MasterPageFile="~/mrtRenew.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="mrtRenew.Confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Confirm Login</h3>
<p> <asp:Label ID ="errMsg" Text ="" runat ="server" ></asp:Label> </p>   
<p>Congratulations <b><asp:Label ID ="name1LBL" runat ="server" ></asp:Label></b>, you are eligible to register your license online.</p>
<br />
<table width="100%" border="0" cellspacing="0" cellpadding="3">
  <tr>
    <td nowrap="nowrap">Name:&nbsp;</td>
    <td><asp:Label ID ="name2LBL" runat ="server" ></asp:Label></td>
  </tr>
  <tr>
    <td nowrap="nowrap">License Number:&nbsp;</td>
    <td  ><asp:Label ID ="licenseLBL" runat ="server" ></asp:Label></td>
  </tr> 
  <tr>
    <td nowrap="nowrap">Fee Amount:&nbsp;</td>
    <td  ><asp:Label ID ="feeLBL" runat ="server" ></asp:Label></td>
  </tr> 
    <tr>
    <td nowrap="nowrap">License Type:&nbsp;</td>
    <td  >Medical Radiologic Technologist</td>
  </tr>   
</table>

<asp:Panel ID="feePanel"  runat ="server" >
  
  <table width="100%" border="0" cellspacing="0" cellpadding="3">
  <tr>
    <td width="35%" >Penalty Fee:</td>
    <td><asp:Label ID ="lateFeeLBL" runat ="server" ></asp:Label></td>
  </tr>
  <tr>
    <td>Back Fee:</td>
    <td><asp:Label ID ="backFeeLBL" runat ="server" ></asp:Label></td>
  </tr>
  </table>
  
  
  </asp:Panel>

<p>If you are not <b id="hiTxt"><asp:Label ID ="nameLBL3" runat ="server" ></asp:Label></b> please return to the Login Page using the "Return to Login" button below. </p>
<p>“Click here for more information about <a id="fl1" class ="pop" href= "http://www.tmb.state.tx.us/page/Renewal-Full-MRT" > General or Full MRT</a> registration fees, or here for information about <a id ="lf2" class="pop" href ="http://www.tmb.state.tx.us/page/Renewal-LMRT" >Limited MRT</a> registration fees.</p>
<br />
<asp:Button id="continueBTN" runat="server"  Text ="Continue" onclick="continue_Click" />
<asp:Button  id="returnBTN" runat ="server" Text="Return to Login"    onclick="return_Click"  />

<script type="text/javascript">
    $('.pop').popupWindow({
        height: 500,
        width: 800,
        top: 50,
        left: 50,
        location: 1
    }); 
</script>


</asp:Content>
