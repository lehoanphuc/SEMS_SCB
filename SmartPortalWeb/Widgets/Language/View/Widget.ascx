﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_Language_View_Widget" %>
<link href="Widgets/Language/View/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
<script src="Widgets/Language/View/Scripts/JScript.js" type="text/javascript"></script>


<div style="padding:5px 0px 5px 5px;">
    <img alt="" src="widgets/Language/view/images/add.gif" style="width: 16px; height: 16px" />
    <asp:LinkButton ID="lbAddPage" Text='<%$ Resources:labels, addlanguage %>' 
        runat="server" onclick="lbAddPage_Click"></asp:LinkButton>
    &nbsp;
    <img alt="" src="widgets/Language/view/images/action_delete.gif" style="width: 16px; height: 16px" /><asp:LinkButton ID="lbDeleteSelected" 
        Text='<%$ Resources:labels, deleteselected %>' runat="server" 
        onclick="lbDeleteSelected_Click"></asp:LinkButton>
     &nbsp;
     <img alt="" src="widgets/pages/view/images/back.gif" style="width: 16px; height: 16px" />
     <a ID="btnBack" href="javascript:history.go(-1);"><%=Resources.labels.back %></a>    
     <hr />
</div>

<div style=" text-align:center; margin:5px 1px 5px 1px;">
<asp:Label ID="lblAlert" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
</div>

<div style="text-align:left; padding:5px 5px 5px 5px;">
<asp:Panel runat="server" ID="pnSearch" DefaultButton="ibSearch">
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <asp:ImageButton ID="ibSearch" runat="server" 
        ImageUrl="~/Widgets/widget/view/images/search.gif" 
        onclick="ibSearch_Click" />
</asp:Panel>
</div>


<div>
<table style="margin:5px auto 5px auto; width:100%;">
    <tr>
        <td>
            <asp:GridView ID="gvLanguage" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                ForeColor="#333333" GridLines="None" 
                 PageSize="15" Width="100%" onrowdatabound="gvLanguage_RowDataBound" 
                onpageindexchanging="gvLanguage_PageIndexChanging" onsorting="gvLanguage_Sorting">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                           <center><asp:CheckBox ID="cbxSelect" runat="server" /></center> 
                        </ItemTemplate>
                    </asp:TemplateField> 
                                     
                    <asp:TemplateField HeaderText='<%$ Resources:labels, culture %>' SortExpression="LangID">
                        <ItemTemplate>
                            <img align="bottom" src="widgets/language/view/images/language.gif" />
                           <asp:HyperLink ID="hpCulture" runat="server"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    
                    <asp:TemplateField HeaderText='<%$ Resources:labels, language %>' SortExpression="LangName">
                        <ItemTemplate>                            
                           <asp:Label ID="lblLangName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                                     
                    <asp:TemplateField HeaderText='<%$ Resources:labels, author %>' SortExpression="UserCreated">
                        <ItemTemplate>
                            <center><asp:Label ID="lblAuthor" runat="server"></asp:Label></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText='<%$ Resources:labels, datecreated %>' 
                        SortExpression="DateCreated">
                        <ItemTemplate>
                            <center><asp:Label ID="lblDateCreated" runat="server"></asp:Label></center>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    
                    <asp:TemplateField HeaderText='<%$ Resources:labels, edit %>'>
                        <ItemTemplate>
                            
                           <center><asp:HyperLink ID="hpEdit" runat="server">HyperLink</asp:HyperLink></center> 
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, delete %>'>
                        <ItemTemplate>
                            
                           <center><asp:HyperLink ID="hpDelete" runat="server">[hpDelete]</asp:HyperLink></center> 
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="pager" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#BCDFFB" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </td>
    </tr>
</table>  
</div>
<div style="padding:5px 0px 5px 5px;">
     <hr />
    <img alt="" src="widgets/Language/view/images/add.gif" style="width: 16px; height: 16px" />
    <asp:LinkButton ID="LinkButton1" Text='<%$ Resources:labels, addlanguage %>' 
        runat="server" onclick="lbAddPage_Click"></asp:LinkButton>
    &nbsp;
    <img alt="" src="widgets/Language/view/images/action_delete.gif" style="width: 16px; height: 16px" /><asp:LinkButton ID="LinkButton2" 
        Text='<%$ Resources:labels, deleteselected %>' runat="server" 
        onclick="lbDeleteSelected_Click"></asp:LinkButton>
     &nbsp;
     <img alt="" src="widgets/pages/view/images/back.gif" style="width: 16px; height: 16px" />
     <a ID="A1" href="javascript:history.go(-1);"><%=Resources.labels.back %></a>    
    
</div>