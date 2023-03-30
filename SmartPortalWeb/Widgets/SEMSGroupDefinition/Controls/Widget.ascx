﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_SEMSGroupDefinition_Controls_Widget" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="subheader">
            <h1 class="subheader-title">
                <asp:Label ID="lblTitleGroupDefinition" runat="server"></asp:Label>
            </h1>
        </div>
        <div id="divError">
            <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
        </div>
         <div class="loading">
            <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                    <img src="Images/tenor.gif" style="width: 32px; height: 32px;" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="panel">
                    <div class="panel-hdr">
                        <h2>
                        </h2>
                    </div>
                    <div class="panel-container">
                        <div class="panel-content form-horizontal p-b-0" style="margin-left:2%">
                            <asp:Panel ID="pnAdd" runat="server">
                                <div class="row">
                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label required"><%=Resources.labels.accountinggroup %> </label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtGroupID" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5" >
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label required"><%=Resources.labels.modulename%> </label>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="ddModule" CssClass="form-control"  runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label required"><%=Resources.labels.groupname %> </label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtGroupname" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="panel-content div-btn rounded-bottom border-faded border-left-0 border-right-0 border-bottom-0 text-muted">
                            <asp:Button ID="btsave" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:labels, save %>" OnClick="btsave_Click" />
                            <asp:Button ID="btclear" CssClass="btn btn-secondary" runat="server" Text="<%$ Resources:labels, clear %>" OnClick="btclear_Click" />
                            <asp:Button ID="btback" CssClass="btn btn-secondary" runat="server" Text="<%$ Resources:labels, back %>" OnClick="btback_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

