<%@ Page Title="" Language="C#" MasterPageFile="~/mrtRenew.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="mrtRenew.payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Payment</h3>
    <asp:RadioButtonList ID="paymentRB" runat ="server"  AutoPostBack ="true" 
        onselectedindexchanged="paymentRB_SelectedIndexChanged"   >
         <asp:ListItem Text="Credit Card" Value ="C"></asp:ListItem>
         <asp:ListItem Text ="Check" Value ="K" ></asp:ListItem>
    </asp:RadioButtonList>

</asp:Content>
