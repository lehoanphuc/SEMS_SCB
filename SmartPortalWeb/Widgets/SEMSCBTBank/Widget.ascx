﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Widget.ascx.cs" Inherits="Widgets_SEMSCBTBank_Widget" %>
<%@ Import Namespace="SmartPortal.Constant" %>
<%@ Register Src="~/Controls/GirdViewPaging/GridViewPaging.ascx" TagPrefix="uc1" TagName="GridViewPaging" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="subheader">
            <h1 class="subheader-title">
                <%= Resources.labels.danhsachnganhang %>
            </h1>
        </div>
        <div id="divError">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="row">
            <div class="col-sm-12 col-xs-12">
                <div class="panel">
                    <div class="panel-container">
                        <div class="panel-content form-horizontal p-b-0">
                            <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnSearch">
                                <div class="row">
                                    <div class="col-sm-5 col-xs-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.swiftcode %></label>
                                            <div class="col-sm-8 col-xs-12">
                                                <asp:TextBox ID="txtSwiftCode" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5 col-xs-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.lcynm %></label>
                                            <div class="col-sm-8 col-xs-12">
                                                <asp:DropDownList ID="ddlCurrency" CssClass="form-control select2" Width="100%" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-2 col-xs-12">
                                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:labels, timkiem %>" OnClick="btnSearch_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5 col-xs-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.BankName %></label>
                                            <div class="col-sm-8 col-xs-12">
                                                <asp:TextBox ID="txtBankName" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5 col-xs-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.nostro %></label>
                                            <div class="col-sm-8 col-xs-12">
                                                <asp:DropDownList ID="ddlNostro" CssClass="form-control select2" Width="100%" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5 col-xs-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-xs-12 control-label"><%=Resources.labels.country %></label>
                                            <div class="col-sm-8 col-xs-12">
                                                <asp:DropDownList ID="ddlCountry" CssClass="form-control select2" Width="100%" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divToolbar">
            <asp:Button ID="btnAdd_New" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:labels, themmoi %>" OnClientClick="Loading();" OnClick="btnAdd_New_Click" />
            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-secondary" Text="<%$ Resources:labels, delete %>" OnClientClick="Loading(); return ConfirmDelete2();" OnClick="btnDelete_Click" />
        </div>
        <div id="divResult" runat="server">
            <asp:Literal ID="litError" runat="server"></asp:Literal>
            <asp:GridView ID="gvBankList" CssClass="table table-hover" runat="server" BackColor="White"
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" PageSize="15"
                Width="100%" AllowPaging="True" AutoGenerateColumns="False" OnRowDataBound="gvBankList_RowDataBound" AllowSorting="True" OnRowCommand="gvBankList_RowCommand" OnRowDeleting="gvBankList_RowDeleting">
                <RowStyle ForeColor="#000000" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbxSelect" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblBankId" runat="server" CommandName='<%#IPC.ACTIONPAGE.DETAILS %>' CommandArgument='<%#Eval("BankID") %>' OnClientClick="Loading();"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, swiftcode %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblSwiftCode" runat="server" CommandName='<%#IPC.ACTIONPAGE.DETAILS %>' CommandArgument='<%#Eval("BankID") %>' OnClientClick="Loading();"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, BankName %>">
                        <ItemTemplate>
                            <asp:Label ID="lblBankName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, nostro %>">
                        <ItemTemplate>
                            <asp:Label ID="lblNostro" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, bcynm %>">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, address %>">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, country %>">
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, edit %> ">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-primary" CommandName='<%#IPC.ACTIONPAGE.EDIT %>' CommandArgument='<%#Eval("BankID") %>' OnClientClick="Loading();">Edit</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:labels, delete %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-secondary" CommandName='<%#IPC.ACTIONPAGE.DELETE %>' CommandArgument='<%#Eval("BankID") %>' OnClientClick="Loading(); return Confirm();">Delete</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle HorizontalAlign="Center" CssClass="pager" />
            </asp:GridView>
            <uc1:GridViewPaging runat="server" ID="GridViewPaging" />
            <asp:HiddenField ID="hdCounter" Value="0" runat="server" />
            <asp:HiddenField ID="hdPageSize" Value="15" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script src="/JS/Common.js"></script>
<script type="text/javascript">
    function SelectCbx(obj) {
        Counter = 0;
        var hdf = document.getElementById("<%= hdCounter.ClientID %>");
        TotalChkBx = parseInt(document.getElementById('<%=hdPageSize.ClientID %>').value);
        var count = document.getElementById('<%=gvBankList.ClientID %>').rows.length;
        var elements = document.getElementById('<%=gvBankList.ClientID %>').rows;
        if (obj.checked) {
            for (i = 0; i < count; i++) {
                if (elements[i].cells[0].children[0].type == 'checkbox' && elements[i].cells[0].children[0].id != 'ctl00_ctl20_gvBankList_ctl01_cbxSelectAll') {
                    elements[i].cells[0].children[0].checked = true;
                    Counter++;
                }
            }

        } else {
            for (i = 0; i < count; i++) {
                if (elements[i].cells[0].children[0].type == 'checkbox' && elements[i].cells[0].children[0].id != 'ctl00_ctl20_gvBankList_ctl01_cbxSelectAll') {
                    elements[i].cells[0].children[0].checked = false;
                    if (Counter > 0)
                        Counter--;
                }
            }
        }
        hdf.value = Counter.toString();
    }

    var TotalChkBx;
    var Counter;

    window.onload = function () {
        document.getElementById('<%=hdCounter.ClientID %>').value = '0';
    }

    function ChildClick(CheckBox) {
        Counter = parseInt(document.getElementById('<%=hdCounter.ClientID %>').value);
        TotalChkBx = parseInt(document.getElementById('<%=hdPageSize.ClientID %>').value);

        var grid = document.getElementById('<%= gvBankList.ClientID %>');
        var cbHeader = grid.rows[0].cells[0].childNodes[0];

        if (CheckBox.checked)
            Counter++;
        else if (Counter > 0)
            Counter--;

        if (Counter < TotalChkBx)
            cbHeader.checked = false;
        else if (Counter == TotalChkBx)
            cbHeader.checked = true;
        document.getElementById('<%=hdCounter.ClientID %>').value = Counter.toString();
    }

    function Loading() {
        if (document.getElementById('<%=lblError.ClientID%>').innerHTML != '') {
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
        }
    }

    function ConfirmDelete2() {
        var hdf = document.getElementById("<%= hdCounter.ClientID %>");
        if (hdf.value == 0) {
            alert('<%=Resources.labels.pleaseselectbeforedeleting %>');
            return false;
        } else {
            return confirm('<%=Resources.labels.banchacchanmuonxoa %>');
        }
    }

    function Confirm() {
        return confirm('<%=Resources.labels.banchacchanmuonxoa %>');
    }
</script>
