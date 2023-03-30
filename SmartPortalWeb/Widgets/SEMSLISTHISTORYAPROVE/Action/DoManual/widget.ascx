﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets_SEMSLISTHISTORYAPROVE_Action_DoManual_widget" %>
<link href="widgets/SEMSLISTHISTORYAPROVE/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />

<div class="subheader">
    <h1 class="subheader-title">
        <%=Resources.labels.domanualtrans %>
    </h1>
</div>
<div id="divError">
    <asp:Label ID="lblError" runat="server"></asp:Label>
</div>
<asp:Panel runat="server" ID="pnDefault">
    <div class="row">
        <div class="col-sm-12 col-xs-12">
            <div class="panel">
                <div class="panel-hdr">
                    <h2>
                        <%=Resources.labels.chitietgiaodich%>
                    </h2>
                </div>

                <div class="panel-container">
                    <div class="panel-content form-horizontal p-b-0">

                        <table class="table">
                            <tbody>
                                <tr>
                                    <td><%=Resources.labels.refnumber %></td>
                                    <td>
                                        <asp:Label ID="lblTransID" runat="server"></asp:Label></td>
                                    <td><%=Resources.labels.ngaygiogiaodich %></td>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td><%=Resources.labels.loaigiaodich %></td>
                                    <td>
                                        <asp:Label ID="lblPagename" runat="server"></asp:Label></td>
                                    <td><%=Resources.labels.sogiaodichcore %></td>
                                    <td>
                                        <asp:Label ID="lblReftype" runat="server"></asp:Label></td>
                                </tr>
                                <asp:Panel ID="pnSender" runat="server">
                                    <tr>
                                        <td class="fontb"><%=Resources.labels.thongtinnguoichuyen %></td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.hotennguoitratien %></td>
                                        <td>
                                            <asp:Label ID="lblSenderName" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSender" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAccountSender" runat="server"></asp:Label></td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnReceiver" runat="server">
                                    <tr>
                                        <td class="fontb"><%=Resources.labels.thongtinnguoinhan %></td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.hotennguoinhantien %></td>
                                        <td>
                                            <asp:Label ID="lblReceiverName" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblReceiver" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAccountReceiver" runat="server"></asp:Label></td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnTopup" runat="server">
                                    <tr>
                                        <td class="fontb"><%=Resources.labels.thongtintopup %></td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.phone %></td>
                                        <td>
                                            <asp:Label ID="lblPhone" runat="server"></asp:Label></td>
                                        <td><%=Resources.labels.telco %></td>
                                        <td>
                                            <asp:Label ID="lblTelco" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.cardamount %></td>
                                        <td>
                                            <asp:Label ID="lblCardAmount" runat="server"></asp:Label></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnBillPayment" runat="server" Visible="false">
                                    <tr>
                                        <td class="fontb"><%=Resources.labels.thongtinhoadon %></td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.billername %></td>
                                        <td>
                                            <asp:Label ID="lblBillerName" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td><%=Resources.labels.corporates %></td>
                                        <td>
                                            <asp:Label ID="lblCorpName" runat="server"></asp:Label></td>
                                        <td><%=Resources.labels.dichvu %></td>
                                        <td>
                                            <asp:Label ID="lblServiceName" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblRefindex1" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblRefvalue1" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRefindex2" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblRefvalue2" runat="server"></asp:Label></td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td class="fontb"><%=Resources.labels.noidungthanhtoan %></td>
                                </tr>
                                <asp:Panel ID="pnAmount" runat="server">
                                    <tr>
                                        <td><%=Resources.labels.sotien %></td>
                                        <td>
                                            <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                            <asp:Label ID="lblCCYID" runat="server"></asp:Label>
                                        </td>
                                        <td><%=Resources.labels.sotienphi %></td>
                                        <td>
                                            <asp:Label ID="lblFee" runat="server"></asp:Label>
                                            &nbsp;<asp:Label ID="lblCCYIDPhi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.sotienbangchu %></td>
                                        <td>
                                            <asp:Label ID="lblstbc" runat="server"></asp:Label>
                                        </td>
                                        <td><%=Resources.labels.vat %></td>
                                        <td>
                                            <asp:Label ID="lblVAT" runat="server"></asp:Label>
                                            &nbsp;<asp:Label ID="lblCCYIDVAT" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.mota %></td>
                                        <td>
                                            <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=Resources.labels.nguoithuchien %></td>
                                        <td>
                                            <asp:Label ID="lblUserCreate" runat="server"></asp:Label>
                                        </td>
                                        <td><%=Resources.labels.trangthai %></td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="hidden">
                                        <td><%=Resources.labels.nguoiduyetcuoi %></td>
                                        <td>
                                            <asp:Label ID="lblLastApp" runat="server"></asp:Label>
                                        </td>
                                        <td><%=Resources.labels.ketqua %></td>
                                        <td>
                                            <asp:Label ID="lblResult" runat="server"></asp:Label></td>
                                    </tr>


                                </asp:Panel>



                                <!--TrungTQ-->
                                <asp:Panel ID="pnlDocument" runat="server" Visible="false">
                                    <tr>
                                        <td class="fontb">Document</td>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rptDocument" OnItemCommand="rptDocument_ItemCommand">
                                        <HeaderTemplate>
                                            <div class="pane">
                                                <div class="table-responsive">
                                                    <table class="table table-hover footable c_list">
                                                        <thead style="background-color: #7A58BF; color: #FFF;">
                                                            <tr>
                                                                <th class="title-repeater">Document Name</th>
                                                                <th class="title-repeater">Document Type</th>
                                                                <th class="title-repeater">View</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="title-repeater">
                                                    <%#Eval("DOCUMENTNAME") %>
                                                </td>
                                                <td class="title-repeater">
                                                    <%#Eval("DOCUMENTTYPE") %>
                                                </td>
                                                <td class="title-repeater">
                                                    <asp:LinkButton ID="lblDownload" runat="server" OnClientClick="aspnetForm.target ='_blank';" CommandName="Download"><%= Resources.labels.view %></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                        </table>                                       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:Panel>


                            </tbody>
                        </table>
                    </div>
                    <div>
                    </div>
                    <div class="panel-content div-btn rounded-bottom border-faded border-left-0 border-right-0 border-bottom-0 text-muted">
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text='<%$ Resources:labels, inketqua %>' Visible="false" OnClientClick="javascript:return poponload();" />
                        <asp:Button ID="btnPrintBill" CssClass="btn btn-primary" runat="server" Visible="false" Text='<%$ Resources:labels, inketquabill %>' OnClientClick="javascript:return poponloadbill();" />
                        <asp:Button ID="btnManual" CssClass="btn btn-manual" runat="server" Text='<%$ Resources:labels, doManual %>' OnClick="btnManual_Click" />
                        <asp:Button ID="btnNext" CssClass="btn btn-primary" Visible="false" runat="server" Text="<%$ Resources:labels, next %>" OnClientClick="Loading();" OnClick="btnNext_Click" />
                        <asp:Button ID="btnBack" CssClass="btn btn-secondary" runat="server" Text="<%$ Resources:labels, back %>" OnClientClick="Loading();" OnClick="btback_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<script type="text/javascript">
    function poponload() {
        testwindow = window.open("widgets/SEMSLISTHISTORYAPROVE/print.aspx?pt=P&cul=" +'<%=System.Globalization.CultureInfo.CurrentCulture.ToString()%>', "BienLai",
            "menubar=1,scrollbars=1,width=800,height=650");
        testwindow.moveTo(0, 0);
        return false;
    }


    function poponloadbill() {
        testwindow = window.open("widgets/SEMSLISTHISTORYAPROVE/printbill.aspx?pt=P&cul=" + '<%=System.Globalization.CultureInfo.CurrentCulture.ToString()%>', "BienLai",
            "menubar=1,scrollbars=1,width=800,height=650");
        testwindow.moveTo(0, 0);
        return false;
    }
</script>
<style>
    .panel-content {
        padding: 0 !important;
    }

    .fontb {
        font-weight: bold;
    }

    .table > tbody > tr > td {
        border: 1px solid #ebebeb;
    }

        .table > tbody > tr > td:nth-child(1) {
            width: 20%;
        }

        .table > tbody > tr > td:nth-child(3) {
            width: 20%;
        }

    .div-btn {
        border: none;
        margin-bottom: 15px;
    }

    .table > tbody > tr > td {
        padding: 15px 10px;
    }
</style>
