﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_IBCorpUser_Edit_Widget" %>
<%@ Register src="../Controls/Widget.ascx" tagname="Widget" tagprefix="uc1" %>
<div>
<style>
.th
{
	font-weight:bold;
	padding-left:5px;
	padding-top:10px;
	padding-bottom:10px;
}
</style>
<div class="th">
<img alt="" src="widgets/SEMSCustomerList/Images/messenger.png" style="width: 32px; height: 32px; margin-bottom:10px;" align="middle" />
<span><%=Resources.labels.suadoithongtinnguoidung %></span><br />

</div>
    <uc1:Widget ID="ucEdit" runat="server" />

</div>
