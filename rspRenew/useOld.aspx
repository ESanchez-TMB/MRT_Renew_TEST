<%@ Page Title="" Language="C#" MasterPageFile="~/rspRenew.Master" AutoEventWireup="True" CodeBehind="useOld.aspx.cs" Inherits="rspRenew.useOld" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<p>
Our records indicate that within the last 15 days, you began the online registration process.
  Any updates made at that time were saved.  Would you like to use this saved information or start
   the registration process over?   Click the “Use Recent Updates” button to use updates made within
    the last 15 days, otherwise click the “Start Over” button. 
</p>
<asp:Label ID="msgLBL" runat ="server" ></asp:Label>
<asp:Button  id="usePreviousBTN" runat ="server" Text="Use Recent Updates"    onclick="usePrevious_Click"  />
<asp:Button  id="beginBTN" runat ="server" Text="Start Over"    onclick="begin_Click"  />

</asp:Content>
