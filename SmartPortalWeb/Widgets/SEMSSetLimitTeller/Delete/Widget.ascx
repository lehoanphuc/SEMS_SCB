﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_SEMSSetLimitTeller_Delete_Widget" %>

<link href="widgets/SEMSUser/CSS/tab-view.css" rel="stylesheet" type="text/css" />
<link href="widgets/SEMSUser/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />

<script src="widgets/SEMSUser/JS/ajax.js" type="text/javascript"></script>

<script src="widgets/SEMSUser/JS/tab-view.js" type="text/javascript"></script>
<script src="widgets/IBTransactionHistory1/JS/jscal2.js" type="text/javascript"></script>
<script src="widgets/IBTransactionHistory1/JS/lang/en.js" type="text/javascript"></script>

<link href="widgets/IBTransactionHistory1/CSS/jscal2.css" rel="stylesheet" type="text/css" />
<link href="widgets/IBTransactionHistory1/CSS/border-radius.css" rel="stylesheet" type="text/css" />

<link href="widgets/SEMSCustomerList/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="widgets/SEMSUser/js/common.js"> </script> 
<script type="text/javascript" src="widgets/SEMSUser/js/subModal.js"> </script> 
<script type="text/javascript" src="widgets/SEMSUser/js/commonjs.js"> </script> 

<link href="widgets/SEMSUser/css/style.css" rel="stylesheet" type="text/css"> 
<!-- Add this to have a specific theme--> 
<link href="widgets/SEMSUser/css/subModal.css" rel="stylesheet" type="text/css"> 

<br />
<div id="divCustHeader">
    <img alt="" src="widgets/SEMSSetLimitTeller/Images/tellerlimit.png" style="width: 30px; height: 32px; margin-bottom:10px;" align="middle" /> 
    <%=Resources.labels.xoahanmucgiaodich %>

</div>
<div id="divError">
</div>
<asp:Panel runat="server" ID="pnRole">
    <div class="divGetInfoCust">
        <div class="divHeaderStyle">
            <%=Resources.labels.thongtinxacnhan %>
        </div>
        <table class="style1" cellspacing="0" cellpadding="5">
            <tr>
                <td align="center" class="style2">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:labels, banchacchanmuonxoahanmucgiaodichkhong %>"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: center; padding-top: 10px;">
        &nbsp;<asp:Button ID="btsaveandcont" runat="server" OnClick="btsaveandcont_Click" Text="<%$ Resources:labels, delete %>" Width="71px" />
        &nbsp;<asp:Button ID="btback" runat="server" PostBackUrl="javascript:history.go(-1)" Text="<%$ Resources:labels, back %>" />
    </div>
</asp:Panel>
    
    
   