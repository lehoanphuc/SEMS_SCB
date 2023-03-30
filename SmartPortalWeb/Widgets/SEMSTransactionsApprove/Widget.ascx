<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_SEMSTransactionsApprove_Widget" %>
<%@ Import Namespace="SmartPortal.Constant" %>
<%@ Register Src="~/Controls/GirdViewPaging/GridViewPaging.ascx" TagPrefix="uc1" TagName="GridViewPaging" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<br />
<%--<div id="divCustHeader">
    <img alt="" src="widgets/SEMSTransactionsApprove/Images/icon_transactions.jpg" style="width: 40px; height: 32px; margin-bottom: 10px;" align="middle" />
    <%=Resources.labels.danhsachcacgiaodich %>
</div>--%>
<%--<asp:Panel runat="server" ID="pnRole">
    <div class="divGetInfoCust">
        <div class="divHeaderStyle">
            <%=Resources.labels.chongiaodich %>
        </div>
        <table class="style1" cellspacing="0" cellpadding="5">
            <tr>
                <td class="style3">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:labels, loaigiaodich %>"></asp:Label>
                </td>
                <td align="center" class="style2">
                    <asp:DropDownList ID="ddlTransaction" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: center; padding-top: 10px;">
        &nbsp;<asp:Button ID="btsaveandcont" runat="server" OnClick="btsaveandcont_Click" OnClientClick="return validate();" Text="<%$ Resources:labels, xemchitiet %>" />
        &nbsp;<asp:Button ID="btback" runat="server" PostBackUrl="javascript:history.go(-1)" Text="<%$ Resources:labels, back %>" />
    </div>

</asp:Panel>--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="subheader">
            <h1 class="subheader-title">
                <%=Resources.labels.listtranapprove %>
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
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="row">
            <div class="col-sm-12 col-xs-12">
                <div class="panel">
                    <div class="panel-container">
                        <div class="panel-content form-horizontal p-b-0">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                <div class="row">
                                    <div class="col-sm-10 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.transaction %></label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <asp:DropDownList ID="ddlTransaction" CssClass="form-control select2" runat="server" Width="100%"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.tungay %></label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <asp:TextBox ID="txtFromDate" CssClass="form-control datetimepicker" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.denngay %></label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <asp:TextBox ID="txtToDate" CssClass="form-control datetimepicker" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-2 col-xs-12">
                                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:labels, timkiem %>" OnClientClick="Loading();" OnClick="btnSearch_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divResult" runat="server" class="table-responsive">
            <asp:Literal runat="server" ID="ltrError"></asp:Literal>
            <asp:GridView ID="gvLTWA" CssClass="table table-hover" runat="server" AutoGenerateColumns="False"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="5" Width="100%" OnRowDataBound="gvLTWA_RowDataBound" OnRowCommand="gvLTWA_RowCommand" PageSize="15">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, refnumber%>">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbTranID" runat="server" CommandName='<%#IPC.ACTIONPAGE.DETAILS %>' CommandArgument='<%#Eval("IPCTRANSID")%>' OnClientClick="Loading();"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, ngaygiogiaodich %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, loaigiaodich %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblTrantype" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, debitaccount %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblAccount" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, sotien %>' ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, phi %>' ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblFee" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, totalAmount %>' ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, loaitien %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblCCYID" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, errordesc %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblErrDesc" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, mota %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblDesc" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, malo %>' Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblBatchRef" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, sogiaodichcore %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblRefCore" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, customercodecore %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblcustcodecore" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:labels, trangthai %>'>
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, trangthaiduyet %>" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblResult" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, duyet %> ">
                        <ItemTemplate>
                            <asp:LinkButton ID="hpApprove" runat="server" CssClass="btn btn-primary" CommandName='<%#IPC.ACTIONPAGE.APPROVE %>' CommandArgument='<%#Eval("IPCTRANSID")%>'  OnClientClick="Loading()">Approve</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="gvHeader" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, conreject %> ">
                        <ItemTemplate>
                            <asp:LinkButton ID="hpReject" runat="server" CssClass="btn btn-primary" CommandName='<%#IPC.ACTIONPAGE.REJECT %>' CommandArgument='<%#Eval("IPCTRANSID")%>'  OnClientClick="Loading()">Reject</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="gvHeader" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <uc1:GridViewPaging Visible="false" runat="server" ID="GridViewPaging" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
