﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_Theme_Controls_Widget" %>
<link href="Widgets/theme/Controls/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function rolevalidate() {
        if (document.getElementById('<%=txtThemeName.ClientID %>').value == '') {
            alert('<%=Resources.labels.themenamerequire %>');
            return false;
        }
        else {
            return true;
        }
    }
</script>

<div style="padding:5px 0px 5px 5px; text-align:center;">
   
    <asp:Image runat="server" ID="imgSave" AlternateText="" ImageUrl="~/widgets/pages/controls/images/save.gif" style="width: 16px; height: 16px" />
    <asp:LinkButton ID="btnSave1" runat="server" Text='<%$ Resources:labels, save %>' 
                OnClientClick="return rolevalidate();" onclick="btnSave_Click" />
    &nbsp;
    <img alt="" src="widgets/pages/controls/images/exit.gif" style="width: 16px; height: 16px" />
     <asp:LinkButton ID="LinkButton2" runat="server" Text='<%$ Resources:labels, exit %>' 
                CausesValidation="False" onclick="btnExit_Click"/>
     &nbsp;
     <img alt="" src="widgets/pages/view/images/back.gif" style="width: 16px; height: 16px" />
     <a ID="A1" href="javascript:history.go(-1);"><%=Resources.labels.back %></a>    
      <hr />
</div>
<div style=" text-align:left; padding:5px 1px 5px 1px; height:auto;">
<asp:Label ID="lblAlert" runat="server" Font-Bold="False" SkinID="lblImportant"></asp:Label>
</div>
<div style=" text-align:right; margin:5px 1px 5px 1px; padding-right:5px;">
<asp:Label ID="Label7" runat="server" Font-Bold="False" SkinID="lblImportant">*</asp:Label>
<asp:Label ID="Label8" runat="server" SkinID="lblImportant" Text='<%$ Resources:labels, requiredfield %>'></asp:Label>
</div>

<asp:Panel runat="server" ID="pnFocus" DefaultButton="btnSave">
<table id="pageadd" cellspacing="1">
    <tr>
        <td class="tdleft">
            <asp:Label ID="Label9" runat="server" SkinID="lblImportant" Text="*"></asp:Label>
            <asp:Label ID="Label4" runat="server" SkinID="lblImportant" 
                Text="<%$ Resources:labels, themename %>"></asp:Label>
            :</td>
        <td>
            <asp:TextBox ID="txtThemeName" runat="server" SkinID="txtTwoColumn"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdleft">
            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:labels, filename %>"></asp:Label>
            :</td>
        <td>
            <asp:FileUpload ID="fuThemeFile" runat="server" />
            &nbsp;<img alt="" onmouseout="UnTip()" 
                onmouseover="<%=Resources.labels.onlyzipfile %>" 
                src="App_Themes/Bank2/images/help.gif" style="width: 17px; height: 17px" />
        </td>
    </tr>
    <tr>
        <td class="tdleft" valign="top">
            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:labels, desc %>"></asp:Label>
            :</td>
        <td>
            <asp:TextBox ID="txtThemeDescription" runat="server" TextMode="MultiLine" 
                Height="58px" SkinID="txtTwoColumn"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    
</table>
<div style="padding:5px 0px 5px 5px; text-align:center;">
    <hr />
    <asp:Image runat="server" ID="imgSave1" AlternateText="" ImageUrl="~/widgets/pages/controls/images/save.gif" style="width: 16px; height: 16px" />
    <asp:LinkButton ID="btnSave" runat="server" Text='<%$ Resources:labels, save %>' 
                OnClientClick="return rolevalidate();" onclick="btnSave_Click" />
    &nbsp;
    <img alt="" src="widgets/pages/controls/images/exit.gif" style="width: 16px; height: 16px" />
     <asp:LinkButton ID="btnExit" runat="server" Text='<%$ Resources:labels, exit %>' 
                CausesValidation="False" onclick="btnExit_Click"/>
     &nbsp;
     <img alt="" src="widgets/pages/view/images/back.gif" style="width: 16px; height: 16px" />
     <a ID="A2" href="javascript:history.go(-1);"><%=Resources.labels.back %></a>    
     
</div>
</asp:Panel>