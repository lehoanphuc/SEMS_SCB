using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartPortal.Constant;


public partial class Widgets_SEMSTransactionsApprove_Widget : WidgetBase
{
    string IPCERRORCODE;
    string IPCERRORDESC;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            //    Session["tranID"] = null;
            try
            {
                DataSet dsResult = new SmartPortal.SEMS.Product().GetTranNameByTrancode(ref IPCERRORCODE, ref IPCERRORDESC);
                ddlTransaction.DataSource = dsResult;
                ddlTransaction.DataTextField = "PAGENAME";
                ddlTransaction.DataValueField = "TRANCODE";
                ddlTransaction.DataBind();
                ddlTransaction.Items.Insert(0, new ListItem(IPC.ALL, IPC.ALL));

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //        DataSet ds = new SmartPortal.SEMS.Transactions().LoadTranApp(ref IPCERRORCODE, ref IPCERRORDESC);
                //        if (IPCERRORCODE != "0")
                //        {
                //            goto ERROR;
                //        }
                //        DataTable dtTran = new DataTable();
                //        dtTran = ds.Tables[0];

                //        ddlTransaction.DataSource = dtTran;
                //        ddlTransaction.DataTextField = "PAGENAME";
                //        ddlTransaction.DataValueField = "TRANCODE";
                //        ddlTransaction.DataBind();

                //        ddlTransaction.Items.Remove(ddlTransaction.Items.FindByValue("IB000499"));

                //        goto EXIT;
            }
            catch (Exception ex)
            {
                SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSTransactionsApprove_Widget", "Page_Load", ex.ToString(), Request.Url.Query);
                SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

            }
            //ERROR:
            //    SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSTransactionsApprove_Widget", "Page_Load", IPCERRORDESC, Request.Url.Query);
            //    SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
            //EXIT: ;
        }
    }
    //protected void btsaveandcont_Click(object sender, EventArgs e)
    //{
    //    string linkApprove="";
    //    try
    //    {
    //        DataSet ds = new SmartPortal.SEMS.Transactions().LoadTranAppByTrancode(ddlTransaction.SelectedValue,ref IPCERRORCODE, ref IPCERRORDESC);
    //        if (IPCERRORCODE != "0")
    //        {
    //            goto ERROR;
    //        }
    //        DataTable dtTran = new DataTable();
    //        dtTran = ds.Tables[0];

    //        if (dtTran.Rows.Count != 0)
    //        {
    //            linkApprove = dtTran.Rows[0]["LINKAPPROVE"].ToString();
    //            linkApprove += "&tc=" + ddlTransaction.SelectedValue;
    //        }

    //        goto EXIT;
    //    }
    //    catch (Exception ex)
    //    {
    //        SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSTransactionsApprove_Widget", "btsaveandcont_Click", ex.ToString(), Request.Url.Query);
    //        SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

    //    }
    //ERROR:
    //    SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSTransactionsApprove_Widget", "btsaveandcont_Click", IPCERRORDESC, Request.Url.Query);
    //    SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
    //EXIT: 
    //   Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx"+linkApprove));
    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ((TextBox)GridViewPaging.FindControl("SelectedPageNo")).Text = "1";
            ((HiddenField)GridViewPaging.FindControl("hdfCurrentPage")).Value = "1";
            gvLTWA.PageSize = Convert.ToInt32(((DropDownList)GridViewPaging.FindControl("PageRowSize")).SelectedValue);
            string SelectedPageNo = ((TextBox)GridViewPaging.FindControl("SelectedPageNo")).Text;
            gvLTWA.PageIndex = !string.IsNullOrEmpty(SelectedPageNo) ? Convert.ToInt32(SelectedPageNo) - 1 : 0;
            BindData();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }

    private void BindData()
    {
        try
        {
            divResult.Visible = true;
            ltrError.Text = string.Empty;
            DataTable dtTran = new DataTable();
            DataSet dsTran = new SmartPortal.SEMS.Transactions().GetListAppTransForBankStaff("",
                ddlTransaction.SelectedValue, 
                SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtFromDate.Text.Trim()), 
                SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtToDate.Text.Trim()), 
                IPC.TRANSTATUS.WAITING_APPROVE, "", IPC.TRANSTATUS.WAITING_APPROVE, "", "", "", "", "", string.Empty, "", "", 
                gvLTWA.PageSize, gvLTWA.PageIndex * gvLTWA.PageSize,
                ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                dtTran = dsTran.Tables[0];
                gvLTWA.DataSource = dsTran;
                gvLTWA.DataBind();
            }
            else
            {
                lblError.Text = IPCERRORDESC;
            }
            if (dsTran.Tables[0].Rows.Count > 0)
            {
                ltrError.Text = string.Empty;
                GridViewPaging.Visible = true;
                ((HiddenField)GridViewPaging.FindControl("TotalRows")).Value = dsTran.Tables[0].Rows[0]["TRECORDCOUNT"].ToString();
            }
            else
            {
                ltrError.Text = "<p class='divDataNotFound'>" + Resources.labels.datanotfound + "</p>";
                GridViewPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }

    protected void gvUserLimit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvLTWA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            LinkButton lbTranID, hpApprove, hpReject;
            Label lblDate, lblTrantype, lblAmount, lblCCYID, lblAccount, lblDesc, lblStatus, lblResult, lblBatchRef, lblRefCore, lblcustcodecore, lblErrDesc, lblFee, lblTotalAmount;
            DataRowView drv;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                drv = (DataRowView)e.Row.DataItem;
                lbTranID = (LinkButton)e.Row.FindControl("lbTranID");
                hpApprove = (LinkButton)e.Row.FindControl("hpApprove");
                hpReject = (LinkButton)e.Row.FindControl("hpReject");

                lblDate = (Label)e.Row.FindControl("lblDate");
                lblTrantype = (Label)e.Row.FindControl("lblTrantype");
                lblAmount = (Label)e.Row.FindControl("lblAmount");
                lblCCYID = (Label)e.Row.FindControl("lblCCYID");
                lblAccount = (Label)e.Row.FindControl("lblAccount");
                lblFee = (Label)e.Row.FindControl("lblFee");
                lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
                lblDesc = (Label)e.Row.FindControl("lblDesc");
                lblStatus = (Label)e.Row.FindControl("lblStatus");
                lblResult = (Label)e.Row.FindControl("lblResult");
                lblBatchRef = (Label)e.Row.FindControl("lblBatchRef");
                lblRefCore = (Label)e.Row.FindControl("lblRefCore");
                lblcustcodecore = (Label)e.Row.FindControl("lblcustcodecore");
                lblErrDesc = (Label)e.Row.FindControl("lblErrDesc");

                lbTranID.Text = drv["IPCTRANSID"].ToString();
                lblTrantype.Text = drv["PAGENAME"].ToString();
                lblDesc.Text = SmartPortal.Common.Utilities.Utility.KillSqlInjection(drv["TRANDESC"].ToString());
                lblRefCore.Text = drv["CHAR20"].ToString();
                lblAccount.Text = drv["CHAR01"].ToString();
                lblBatchRef.Text = drv["BATCHREF"].ToString();
                lblAmount.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(drv["NUM01"].ToString(), drv["CCYID"].ToString().Trim());
                lblFee.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(drv["NUM02"].ToString(), drv["CCYID"].ToString().Trim());
                double totalAmount;
                switch (drv["IPCTRANCODE"].ToString().Trim())
                {
                    case "C_CANCELCASHCODE":
                    case "SB_CANCELCASHCODE":
                    case "SW_CANCELCASHCODE":
                        totalAmount = Math.Abs(SmartPortal.Common.Utilities.Utility.isDouble(lblAmount.Text, true)) - SmartPortal.Common.Utilities.Utility.isDouble(lblFee.Text, true);
                        break;
                    default:
                        totalAmount = Math.Abs(SmartPortal.Common.Utilities.Utility.isDouble(lblAmount.Text, true)) + SmartPortal.Common.Utilities.Utility.isDouble(lblFee.Text, true);
                        break;
                }

                lblTotalAmount.Text = SmartPortal.Common.Utilities.Utility.FormatMoney(totalAmount.ToString(), drv["CCYID"].ToString().Trim());
                lblCCYID.Text = drv["CCYID"].ToString();
                lblDate.Text = SmartPortal.Common.Utilities.Utility.IsDateTime2(drv["IPCTRANSDATE"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                lblcustcodecore.Text = drv["CUSTCODE"].ToString();
                lblErrDesc.Text = SmartPortal.Common.Utilities.Utility.KillSqlInjection(drv["ERRORDESC"].ToString());

                switch (drv["STATUS"].ToString().Trim())
                {
                    case SmartPortal.Constant.IPC.TRANSTATUS.BEGIN:
                        lblStatus.Text = Resources.labels.dangxuly;
                        lblStatus.Attributes.Add("class", "label-warning");
                        break;
                    case SmartPortal.Constant.IPC.TRANSTATUS.PAYMENTFAIL:
                        lblStatus.Text = Resources.labels.thanhtoanthatbai;
                        lblStatus.Attributes.Add("class", "label-warning");
                        break;
                    case SmartPortal.Constant.IPC.TRANSTATUS.FINISH:
                        lblStatus.Text = Resources.labels.hoanthanh;
                        lblStatus.Attributes.Add("class", "label-success");
                        switch (drv["APPRSTS"].ToString().Trim())
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
                        lblStatus.Attributes.Add("class", "label-warning");
                        switch (drv["APPRSTS"].ToString().Trim())
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
                                lblResult.Text = Resources.labels.dahoantien;
                                break;
                        }
                        break;
                    case SmartPortal.Constant.IPC.TRANSTATUS.ERROR:
                        lblStatus.Text = Resources.labels.loi;
                        lblStatus.Attributes.Add("class", "label-warning");
                        switch (drv["APPRSTS"].ToString().Trim())
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
                                lblResult.Text = Resources.labels.dahoantien;
                                break;
                        }
                        break;
                    case SmartPortal.Constant.IPC.TRANSTATUS.REJECTED:
                        lblStatus.Text = Resources.labels.khongduyet;
                        lblStatus.Attributes.Add("class", "label-warning");
                        break;
                    case "5":
                        lblStatus.Text = Resources.labels.conpending;
                        lblStatus.Attributes.Add("class", "label-warning");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }

    protected void gvLTWA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string commandName = e.CommandName;
        string commandArg = e.CommandArgument.ToString();
        if (CheckPermitPageAction(commandName))
        {
            switch (commandName)
            {
                case IPC.ACTIONPAGE.DETAILS:
                    RedirectToActionPage(IPC.ACTIONPAGE.DETAILS, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                    break;
                case IPC.ACTIONPAGE.APPROVE:
                    RedirectToActionPage(IPC.ACTIONPAGE.APPROVE, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                    break;
                case IPC.ACTIONPAGE.REJECT:
                    RedirectToActionPage(IPC.ACTIONPAGE.REJECT, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                    break;
            }
        }
    }
}
