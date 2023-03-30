<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_SEMSSETWORKFLOWAPPROVEBANK_Widget" %>
<%@ Import Namespace="SmartPortal.Constant" %>
<%@ Register TagPrefix="uc1" TagName="GridViewPaging" Src="~/Controls/GirdViewPaging/GridViewPaging.ascx" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="subheader">
            <h1 class="subheader-title">
                <asp:Label ID="lblTitleGroup" runat="server"></asp:Label>
            </h1>
        </div>
          <div class="loading">
            <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                    <img src="Images/tenor.gif" style="width: 32px; height: 32px;" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>

        <div id="divError">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
        <asp:Panel runat="server" ID="pnWorkflow">
            <asp:Panel ID="pnAdd" runat="server">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="panel">
                            <div class="panel-hdr">
                                <h2>
                                    <%=Resources.labels.setApproveWorkflow%>
                                </h2>
                            </div>
                            <div class="panel-container">
                                <div class="panel-content form-horizontal p-b-0">
                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="row">
                                                <div class="col-sm-5 col-xs-12">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.transaction %></label>
                                                        <div class="col-sm-8 col-xs-12">
                                                            <asp:DropDownList ID="ddlTransaction" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransaction_OnSelectedIndexChanged">
<%--                                                                          <asp:ListItem Value="IB000215" Text="IB Schedule Transfer" Selected></asp:ListItem>--%>

                                                                </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnApproval" runat="server">
            <div class="row">
                <div class="col-sm-12 col-xs-12">
                    <div class="panel">
                        <div class="panel-hdr">
                            <h2>
                                <%=Resources.labels.quitrinhduyet%>
                            </h2>
                        </div>
                        <div class="panel-container">
                            <div class="panel-content form-horizontal p-b-0">
                                <div class="row">
                                    <div class="col-sm-10 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.groupname %></label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <asp:DropDownList ID="ddBankStaff" CssClass="form-control select2" Width="100%" runat="server">
                                                        <%--    <asp:ListItem Value="1" Text="Bank Staff Checker"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Bank Staff Approval"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--  <div class="col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.desc %></label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <asp:TextBox ID="txtOrderDesc" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-2 col-xs-12">
                                        <div class="form-group">
                                            <div class="col-sm-12 col-xs-12">
                                                <asp:Button ID="btnAddOrder" runat="server" Text="<%$ Resources:labels, them %>" CssClass="btn btn-primary" OnClick="btnAddOrder_onclick" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnTransaction" runat="server">
            <asp:GridView ID="gvTransaction" CssClass="table table-hover" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="3" Width="100%" AllowPaging="True" AutoGenerateColumns="False" PageSize="15" OnPageIndexChanging="gvTransaction_PageIndexChanging" OnRowDataBound="gvTransaction_onRowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="Group Name">
                        <ItemTemplate>
                            <asp:Label ID="colGroupName" runat="server" Text='<%#Eval("GroupName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Order">
                        <ItemTemplate>
                            <asp:Label ID="colOrd" runat="server" Text='<%#Eval("Ord") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TransactionID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="TransactionID" runat="server" Text='<%#Eval("TransactionID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  <asp:TemplateField HeaderText="Id" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="id" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="RoleID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="roleID" runat="server" Text='<%#Eval("RoleID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RoleNext" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="RoleNext" runat="server" Text='<%#Eval("RoleNext") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UserModified" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="UserModified" runat="server" Text='<%#Eval("UserModified") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ServerID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="ServerID" runat="server" Text='<%#Eval("ServiceID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="Status" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CreateBy" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="CreateBy" runat="server" Text='<%#Eval("CreateBy") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CreateDated" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="CreateDated" runat="server" Text='<%#Eval("CreateDated") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, sort %>" ItemStyle-Width="15px">
                        <ItemTemplate>
                            <asp:LinkButton ID="colUpArrow" runat="server" CommandArgument='<%# Eval("Ord") %>' OnClick="colUpArrow_onclick">
                                    <i class="fa fa-chevron-up"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="colDownArrow" runat="server" CommandArgument='<%# Eval("Ord") %>' OnClick="colDownArrow_onclick"> 
                                    <i class="fa fa-chevron-down"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, delete %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="colTransactionDelete" runat="server" CssClass="btn btn-secondary" CommandName='<%#IPC.ACTIONPAGE.DELETE %>' CommandArgument='<%#Eval("Ord") %>' OnClick="colTransactionDelete_onclick" OnClientClick="Loading(); return Confirm();">Delete</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="gvFooterStyle" />
                <PagerStyle HorizontalAlign="Center" CssClass="pager" />
                <SelectedRowStyle />
                <HeaderStyle CssClass="gvHeader" />
            </asp:GridView>
            <asp:Literal ID="litPager" runat="server"></asp:Literal>
        </asp:Panel>
        <div class="panel-content div-btn border-left-0 border-right-0 border-bottom-0 text-muted">
            <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:labels, save %>"  OnClick="btnSave_Click" OnClientClick="Loading();"/>
            <asp:Button ID="btnBack" CssClass="btn btn-secondary" runat="server" Text="<%$ Resources:labels, back %>" OnClick="btnBack_OnClick" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script src="/JS/Common.js"></script>
<script type="text/javascript">
    function Loading() {
        if (document.getElementById('<%=lblError.ClientID%>').innerHTML != '') {
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            location.reload(true); 
        }
    }
</script>