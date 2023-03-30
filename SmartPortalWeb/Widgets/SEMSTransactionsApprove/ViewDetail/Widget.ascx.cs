using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using SmartPortal.IB;
using System.Globalization;
using SmartPortal.Constant;
using SmartPortal.Common.Utilities;
using SmartPortal.SEMS;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using QRCoder;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Widgets_SEMSTransactionsApprove_ViewDetail_Widget : WidgetBase
{

    public DataTable TBLDOCUMENT
    {
        get { return ViewState["TBLDOCUMENT"] != null ? (DataTable)ViewState["TBLDOCUMENT"] : new DataTable(); }
        set { ViewState["TBLDOCUMENT"] = TBLDOCUMENT; }
    }
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    string BILLERNAME = string.Empty;
    SmartPortal.SEMS.Common _service = new SmartPortal.SEMS.Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindData();
                //BindData2();
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    void BindData()
    {
        try
        {
            string tranID = GetParamsPage(IPC.ID)[0].Trim();
            
            Hashtable hasPrint = new Hashtable();

            DataSet ds = new DataSet();
            ds = new SmartPortal.SEMS.Transactions().GetTranByTranID(tranID, ref IPCERRORCODE, ref IPCERRORDESC);
            DataSet dsDocument = new DataSet();
            dsDocument = new SmartPortal.SEMS.Transactions().GetDocumentByTranID(tranID, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                DataTable dtDocument = new DataTable();

                DataColumn documentnamelink = new DataColumn("DOCUMENTNAME");
                DataColumn documenttypelink = new DataColumn("DOCUMENTTYPE");
                DataColumn filelink = new DataColumn("FILE");

                dtDocument.Columns.Add(documentnamelink);
                dtDocument.Columns.Add(documenttypelink);
                dtDocument.Columns.Add(filelink);
                //dtDocument = dsDocument.Tables[0];
                if (ds.Tables[5] != null)
                {
                    if (ds.Tables[5].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                        {
                            string dctype = ds.Tables[5].Rows[i]["DocumentType"].ToString();
                            string dcname = "Document " + ds.Tables[5].Rows[i]["ID"].ToString();
                            string base64 = ds.Tables[5].Rows[i]["LINK"].ToString();
                            dtDocument.Rows.Add(dcname, dctype, base64);
                        }

                        pnlDocument.Visible = true;
                        rptDocument.DataSource = dtDocument;
                        rptDocument.DataBind();
                        ViewState["TBLDOCUMENT"] = dtDocument;
                    }
                }
                if (dt.Rows.Count != 0)
                {
                    pnDefault.Visible = true;

                    lblTransID.Text = dt.Rows[0]["IPCTRANSID"].ToString();
                    lblAccountSender.Text = SmartPortal.Common.Utilities.Utility.FormatStringCore(dt.Rows[0]["CHAR01"].ToString());
                    lblAccountReceiver.Text = dt.Rows[0]["CHAR02"].ToString();
                    lblAmount.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(dt.Rows[0]["NUM01"].ToString(), dt.Rows[0]["CCYID"].ToString().Trim());
                    string ccyid = dt.Rows[0]["CCYID"].ToString();
                    lblstbc.Text = SmartPortal.Common.Utilities.Utility.NumtoWords(Convert.ToDouble(lblAmount.Text, new CultureInfo("en-US").NumberFormat));
                    lblCCYID.Text = ccyid;
                    lblPagename.Text = dt.Rows[0]["PageName"].ToString();
                    lblReftype.Text = dt.Rows[0]["CHAR20"].ToString();
                    lblTelco.Text = dt.Rows[0]["TELCONAME"].ToString();
                    lblPhone.Text = dt.Rows[0]["CHAR07"].ToString().Trim();
                    lblCardAmount.Text = lblAmount.Text + " " + lblCCYID.Text;
                    BILLERNAME = dt.Rows[0]["CHAR23"].ToString().Trim();

                    if (dt.Rows[0]["ISBATCH"].ToString() == "Y")
                    {
                        string recieverAccount = dt.Rows[0]["CHAR02"].ToString();
                        lblAccountReceiver.Text = recieverAccount.Substring(0, 4) + "*****" + recieverAccount.Substring(recieverAccount.Length - 4, 4);
                        lblAmount.Text = "*****";
                        lblstbc.Text = "*****";
                    }

                    DataTable tblU = new SmartPortal.SEMS.User().GetUBID(dt.Rows[0]["USERCURAPP"].ToString().Trim());

                    if (tblU.Rows.Count != 0)
                    {
                        lblLastApp.Text = tblU.Rows[0]["FULLNAME"].ToString();
                    }
                    lblCCYIDPhi.Text = ccyid;
                    lblCCYIDVAT.Text = ccyid;
                    lblDate.Text = SmartPortal.Common.Utilities.Utility.IsDateTime2(dt.Rows[0]["IPCTRANSDATE"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");

                    string[] c = dt.Rows[0]["TRANDESC"].ToString().Split('|');
                    lblDesc.Text = c[0].ToString();
                    double feeNum = SmartPortal.Common.Utilities.Utility.isDouble(dt.Rows[0]["NUM02"].ToString(), false);

                    string f = (feeNum).ToString();
                    string v = (0).ToString();
                    lblFee.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(f, ccyid);
                    lblVAT.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(v, dt.Rows[0]["CCYID"].ToString().Trim());

                    if (!dt.Rows[0]["CHAR02"].ToString().Equals("") || !dt.Rows[0]["CHAR04"].ToString().Equals(""))
                    {
                        pnReceiver.Visible = true;
                        if (dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("IBINTERBANKTRANSFER")
                            || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("MB_TRANSFEROTHBANK") || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("IBCBTRANSFER") || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("MBCBTRANSFER") || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("INTERBANK247") || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("MBINTERBANK247QR") || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("MBINTERBANK247"))
                        {
                            lblReceiver.Text = Resources.labels.taikhoanbaoco;
                            lblAccountReceiver.Text = dt.Rows[0]["CHAR04"].ToString();
                        }
                        else
                        {
                            lblReceiver.Text = dt.Rows[0]["CHAR02"].ToString().Length < 13 ? "Receiver Phone" : Resources.labels.taikhoanbaoco;
                        }

                    }
                    else
                    {
                        pnReceiver.Visible = false;
                    }
                    pnTopup.Visible = !dt.Rows[0]["TELCONAME"].ToString().Equals("");
                    if (!dt.Rows[0]["CHAR01"].ToString().Equals(""))
                    {
                        pnSender.Visible = true;
                        lblSender.Text = dt.Rows[0]["CHAR01"].ToString().Length < 13 ? "Sender Phone" : Resources.labels.debitaccount;
                    }
                    else
                    {
                        pnSender.Visible = false;
                    }
                    switch (dt.Rows[0]["STATUS"].ToString().Trim())
                    {
                        case SmartPortal.Constant.IPC.TRANSTATUS.BEGIN:
                            lblStatus.Text = Resources.labels.dangxuly;
                            break;
                        case SmartPortal.Constant.IPC.TRANSTATUS.PAYMENTFAIL:
                            lblStatus.Text = Resources.labels.thanhtoanthatbai;
                            break;
                        case SmartPortal.Constant.IPC.TRANSTATUS.FINISH:
                            lblStatus.Text = Resources.labels.thanhcong;
                            switch (dt.Rows[0]["APPRSTS"].ToString().Trim())
                            {
                                case SmartPortal.Constant.IPC.APPROVESTATUS.APPROVED:
                                    lblResult.Text = Resources.labels.duyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.REJECTED:
                                    lblResult.Text = Resources.labels.khongduyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.WAITTINGCUST:
                                    lblResult.Text = Resources.labels.choduyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.BEGIN:
                                    lblResult.Text = Resources.labels.dangxuly;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.DEPOSIT:
                                    lblStatus.Text = Resources.labels.dahoantien;
                                    break;
                            }
                            break;
                        case SmartPortal.Constant.IPC.TRANSTATUS.WAITING_APPROVE:
                            lblStatus.Text = Resources.labels.choduyet;
                            switch (dt.Rows[0]["APPRSTS"].ToString().Trim())
                            {
                                case SmartPortal.Constant.IPC.APPROVESTATUS.APPROVED:
                                    lblResult.Text = Resources.labels.duyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.REJECTED:
                                    lblResult.Text = Resources.labels.huy;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.WAITTINGCUST:
                                    lblResult.Text = Resources.labels.choduyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.BEGIN:
                                    lblResult.Text = Resources.labels.dangxuly;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.DEPOSIT:
                                    lblResult.Text = Resources.labels.dahoantien;
                                    break;
                            }
                            break;
                        case SmartPortal.Constant.IPC.TRANSTATUS.ERROR:
                            lblStatus.Text = Resources.labels.loi;
                            switch (dt.Rows[0]["APPRSTS"].ToString().Trim())
                            {
                                case SmartPortal.Constant.IPC.APPROVESTATUS.APPROVED:
                                    lblResult.Text = Resources.labels.duyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.REJECTED:
                                    lblResult.Text = Resources.labels.huy;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.WAITTINGCUST:
                                    lblResult.Text = Resources.labels.choduyet;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.BEGIN:
                                    lblResult.Text = Resources.labels.dangxuly;
                                    break;
                                case SmartPortal.Constant.IPC.APPROVESTATUS.DEPOSIT:
                                    lblResult.Text = Resources.labels.dahoantien;
                                    break;
                            }
                            break;
                        case SmartPortal.Constant.IPC.TRANSTATUS.REJECTED:
                            lblStatus.Text = Resources.labels.khongduyet;
                            break;

                    }

                    //get detail
                    lblSenderName.Text = string.Empty;
                    lblRefindex1.Text = string.Empty;
                    lblRefvalue1.Text = string.Empty;
                    lblRefindex2.Text = string.Empty;
                    lblRefvalue2.Text = string.Empty;
                    lblServiceName.Text = string.Empty;
                    lblCorpName.Text = string.Empty;


                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lblSenderName.Text = ds.Tables[1].Rows[0][0].ToString();
                    }

                    //load account name GL account
                    if (dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("SB_CANCELCASHCODE")
                        || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("C_CANCELCASHCODE")
                        || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("SW_CANCELCASHCODE")
                    )
                    {
                        object[] objects = new object[] { dt.Rows[0]["CHAR01"].ToString() };
                        DataSet dsGLAccount = _service.common("SEMS_ACC_ACCHRT_VIEW", objects, ref IPCERRORCODE, ref IPCERRORDESC);
                        if (dsGLAccount.Tables[0] != null)
                        {
                            lblSenderName.Text = dsGLAccount.Tables[0].Rows[0]["ACName"].ToString();
                        }
                    }


                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        lblReceiverName.Text = ds.Tables[2].Rows[0][0].ToString();
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        if (dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("SB_CANCELCASHCODE")
                            || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("C_CANCELCASHCODE")
                            || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("SW_CANCELCASHCODE")
                        )
                        {
                            lblUserCreate.Text = dt.Rows[0]["USERID"].ToString().Trim();
                        }
                        else
                        {
                            lblUserCreate.Text = string.IsNullOrEmpty(ds.Tables[3].Rows[0][0].ToString()) ? dt.Rows[0]["USERID"].ToString().Trim() : ds.Tables[3].Rows[0][0].ToString();
                        }
                    }
                    if (dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("MBBPMBANKACT")
                      || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("IBBPMBANKACT")
                      || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("AMBPMWALLET")
                      || dt.Rows[0]["IPCTRANCODE"].ToString().Trim().Equals("WLBPMWALLET")
                        )
                    {
                        pnBillPayment.Visible = true;
                        DataTable bt = new Biller().GetBillDetailsById(dt.Rows[0]["CHAR23"].ToString().Trim(), ref IPCERRORCODE, ref IPCERRORDESC).Tables[0];
                        if (bt.Rows.Count != 0)
                        {
                            lblBillerName.Text = bt.Rows[0]["BILLERNAME"].ToString();
                        }
                    }
                }
            }

            string action = GetParamsPage("a")[0].Trim();
            switch (action)
            {
                case IPC.ACTIONPAGE.APPROVE:
                    divReject.Visible = false;
                    btnReject.Visible = false;
                    btnApprove.Visible = true;
                    divApprove.Visible = true;
                    break;
                case IPC.ACTIONPAGE.REJECT:
                    divApprove.Visible = false;
                    divReject.Visible = true;
                    btnReject.Visible = true;
                    btnApprove.Visible = false;
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    private Hashtable AddPrint(Hashtable hasPrint, string ipcTrancode, string tranid, string tranDate, string senderName, string senderAccount, string recieverName
        , string recieverAccount, string amount, string ccyid, string amountchu, string feeAmount, string desc, string status
        )
    {
        hasPrint["IPCTRANCODE"] = ipcTrancode;
        hasPrint["tranID"] = tranid;
        hasPrint["tranDate"] = tranDate;
        hasPrint["senderName"] = senderName;
        hasPrint["senderAccount"] = senderAccount;
        hasPrint["recieverName"] = recieverName;
        hasPrint["recieverAccount"] = recieverAccount;
        hasPrint["amount"] = amount;
        hasPrint["ccyid"] = ccyid;
        hasPrint["amountchu"] = amountchu;
        hasPrint["feeAmount"] = feeAmount;
        hasPrint["desc"] = desc;
        hasPrint["status"] = status;

        return hasPrint;
    }


    protected void btback_OnClickck_Click(object sender, EventArgs e)
    {
        RedirectBackToMainPage();
    }

    protected void rptDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        HyperLink HyperLinkDocument;
        HyperLinkDocument = (HyperLink)rptDocument.FindControl("HyperLinkDocument");
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["TBLDOCUMENT"];
        string base64 = dt.Rows[e.Item.ItemIndex]["FILE"].ToString();
        string filename = dt.Rows[e.Item.ItemIndex]["DOCUMENTNAME"].ToString() + dt.Rows[e.Item.ItemIndex]["DOCUMENTTYPE"].ToString().ToLower();
        if (!base64.StartsWith("http"))
        {
            byte[] bytes = System.Convert.FromBase64String(base64);
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            HttpContext.Current.Response.AddHeader("Content-Length", Convert.ToString(bytes.Length));
            HttpContext.Current.Response.BinaryWrite(bytes);
        }
        else
        {
            HttpContext.Current.Response.Redirect(base64);
        }
    }
    private void BindData2()
    {
        try
        {
            string tranID = GetParamsPage(IPC.ID)[0].Trim();
            DataSet ds = new DataSet();
            ds = new SmartPortal.IB.Transactions().GetApprovalTranByTranID(tranID, ref IPCERRORCODE, ref IPCERRORDESC);
            List<DataTable> result = ds.Tables[2].AsEnumerable()
                  .GroupBy(row => row.Field<string>("WORKFLOWID"))
                  .Select(g => g.CopyToDataTable())
                  .ToList();
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            StringBuilder sT = new StringBuilder();
            string[] lstWF_Step = ds.Tables[0].Rows[0]["USERCURAPP"].ToString().Split('|');
            if (result.Count > 0)
            {
                foreach (DataTable dtWF in result)
                {

                    sT.Append("<table class='style1 table table-bordered table-hover footable' cellspacing='0' cellpadding='5'>");
                    sT.Append("<thead style='background-color: #7A58BF; color: #FFF;'>");
                    sT.Append("<tr>");
                    sT.Append("<th class='thtdf'>");
                    sT.Append("Approval workflow ID");
                    sT.Append("</th>");
                    sT.Append("<th class='thtdf'>");
                    sT.Append(Resources.labels.order);
                    sT.Append("</th>");
                    sT.Append("<th class='thtdf'>");
                    sT.Append("Formula");
                    sT.Append("</th>");
                    sT.Append("<th class='thtdf'>");
                    sT.Append(Resources.labels.status);
                    sT.Append("</th>");
                    sT.Append("</tr>");
                    sT.Append("</thead>");
                    sT.Append("<tbody>");

                    foreach (DataRow row in dtWF.Rows)
                    {
                        int iStep = 0;
                        foreach (string sWFLID in lstWF_Step)
                        {
                            if (sWFLID.Split('#')[0].ToString() == row["WORKFLOWID"].ToString())
                            {
                                iStep = Convert.ToInt32(sWFLID.Split('#')[1].ToString());
                                break;
                            }
                        }
                        sT.Append("<tr class='trtd'>");
                        sT.Append("<td>");
                        sT.Append(row["WORKFLOWID"].ToString());
                        sT.Append("</td>");
                        sT.Append("<td>");
                        sT.Append(row["ORD"].ToString());
                        sT.Append("</td>");
                        sT.Append("<td>");

                        #region Status: 3
                        string formula = row["FmlFormat"].ToString();
                        // to mau cac buoc lon hon buoc hien tai(store chua xu ly => xu ly trong day)
                        if (dt.Rows[0]["STATUS"].ToString().Trim().Equals(SmartPortal.Constant.IPC.TRANSTATUS.WAITING_APPROVE))
                        {
                            Regex regex = new Regex(@"[\d]{1,2}[A-Z]{1}");
                            var matches = regex.Matches(formula);
                            string status = string.Empty;
                            if (int.Parse(row["ORD"].ToString()) > iStep)
                            {
                                if (matches.Count > 0)
                                {
                                    regex = new Regex(@"[\d]{1,2}[A-Z]{1}");
                                    matches = regex.Matches(formula);

                                    List<string> lsCon = matches.Cast<Match>().Select(x => x.Value).Distinct().ToList();
                                    foreach (string match in lsCon)
                                    {
                                        formula = formula.Replace(match, "<b><font color='red'>" + match + "</font></b>");
                                    }
                                }
                            }
                        }
                        #endregion

                        sT.Append(formula.ToUpper());
                        sT.Append("</td>");
                        sT.Append("<td>");
                        sT.Append(row["STATUS"].ToString());
                        sT.Append("</td>");
                        sT.Append("</tr>");
                    }

                    sT.Append("</tbody>");
                    sT.Append("</table>");
                    if (!result.LastOrDefault().Equals(dtWF))
                        sT.Append("<p><b><font color='#003366'>OR</font></b></p>");
                }
            }
            else
            {
                sT.Append("<table class='style1 table table-bordered table-hover footable' cellspacing='0' cellpadding='5'>");
                sT.Append("<thead>");
                sT.Append("<tr>");
                sT.Append("<th class='thtdf'>");
                sT.Append("Approval workflow ID");
                sT.Append("</th>");
                sT.Append("<th class='thtdf'>");
                sT.Append(Resources.labels.order);
                sT.Append("</th>");
                sT.Append("<th class='thtdf'>");
                sT.Append("Formula");
                sT.Append("</th>");
                sT.Append("</tr>");
                sT.Append("</thead>");
                sT.Append("</table>");
            }

            ltrWF.Text = sT.ToString();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSTransactionsApprove_ViewDetails_Widget", "BindData2", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (ddlTransType.SelectedValue.Equals("F"))
        {
            new SmartPortal.IB.Transactions().BankStaffApp(lblTransID.Text.Trim(), "", "", Session["userID"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE.Equals("0"))
            {
                lblError.Text = Resources.labels.approveuserotpsuccess;
            }
            else
            {
                lblError.Text = IPCERRORDESC;
            }
        }
        else
        {
            lblError.Text = "Service is under development";
        }
        ddlTransType.Enabled = false;
        btnApprove.Visible = false;
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtDescription.Text.ToString()))
        {
            lblError.Text = Resources.labels.banvuilongnhaplydokhongduyetgiaodichnay;
            return;
        }
        new SmartPortal.IB.Transactions().BankStaffDestroyTran(lblTransID.Text.Trim(), txtDescription.Text.ToString(),Session["username"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE.Equals("0")){
            lblError.Text = Resources.labels.huythanhcong;
        }
        else
        {
            lblError.Text = Resources.labels.huythatbai;
        }
        btnApprove.Visible = false;
        btnReject.Visible = false;
    }
}
