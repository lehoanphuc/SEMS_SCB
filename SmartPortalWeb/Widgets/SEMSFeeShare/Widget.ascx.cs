﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using SmartPortal.Common.Utilities;
using SmartPortal.Constant;

public partial class Widgets_SEMSREGIONFEE_Widget : WidgetBase
{
    public static bool isAscend = false;
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    SmartPortal.SEMS.Common _service = new SmartPortal.SEMS.Common();
    public static int flag = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = string.Empty;
            if (!IsPostBack)
            {
                loadCombobox();
                //BindData();
            }
            if (flag == 1)
            {
                GridViewPaging.pagingClickArgs += new EventHandler(Search_GridViewPaging_click);
            }
            if (flag == 0)
            {
                GridViewPaging.pagingClickArgs += new EventHandler(AdvanceSearch_GridViewPaging_click);
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void Search_GridViewPaging_click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GridViewPaging.SelectPageChoose = "1";
        BindData();
    }
    protected void btnAdvanceSearch_click(object sender, EventArgs e)
    {
        GridViewPaging.SelectPageChoose = "1";
        BindData_SearchAdvance();
    }
    protected void AdvanceSearch_GridViewPaging_click(object sender, EventArgs e)
    {
        BindData_SearchAdvance();
    }

    
    
    private void loadCombobox_TransactionType()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = _service.GetValueList("TransactionType", "WAL_SHAREFEE", ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlTransactionType.DataSource = ds;
                    ddlTransactionType.DataValueField = "VALUEID";
                    ddlTransactionType.DataTextField = "CAPTION";
                    ddlTransactionType.DataBind();
                    ddlTransactionType.Items.Insert(0, new ListItem("All", ""));
                }
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }
    private void loadCombobox()
    {
        loadCombobox_TransactionType();
    }
    void BindData()
    {
        try
        {
            flag = 1;
            pnResult.Visible = true;
            DataSet ds = new DataSet();
            object[] searchObject = new object[] { Utility.KillSqlInjection(txtSearch.Text.Trim()), GridViewPaging.pageIndex * GridViewPaging.pageSize, GridViewPaging.pageSize };
            ds = _service.common("SEMSSHAREFEE", searchObject, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0" || IPCERRORCODE.Equals(""))
            {
                if (ds.Tables[0].Rows.Count < 1 & GridViewPaging.pageIndex > 0)
                {
                    GridViewPaging.SelectPageChoose = (int.Parse(GridViewPaging.SelectPageChoose) - 1).ToString();
                    BindData();
                }
                else
                {
                    GridViewPaging.total = ds.Tables[0].Rows.Count == 0 ? "0" : ds.Tables[0].Rows[0][IPC.TRECORDCOUNT].ToString();
                    rptData.DataSource = ds;
                    rptData.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    void BindData_SearchAdvance()
    {
        try
        {
            flag = 0;
            pnResult.Visible = true;
            DataSet ds = new DataSet();
            object[] searchObject = new object[] {  Utility.KillSqlInjection(txtFeeShareCode.Text), Utility.KillSqlInjection(txtFeeShareName.Text), Utility.KillSqlInjection(ddlTransactionType.SelectedValue),      GridViewPaging.pageIndex * GridViewPaging.pageSize, GridViewPaging.pageSize };
            ds = _service.common("SEMSSHAREFEEAVD", searchObject, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0" || IPCERRORCODE.Equals(""))
            {
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        rptData.DataSource = ds;
                        rptData.DataBind();
                        GridViewPaging.total = ds.Tables[0].Rows.Count == 0 ? "0" : ds.Tables[0].Rows[0][IPC.TRECORDCOUNT].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void rptData_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string commandName = e.CommandName;
        string commandArg = e.CommandArgument.ToString();
        if (CheckPermitPageAction(commandName))
        {
                switch (commandName)
                {
                    case IPC.ACTIONPAGE.EDIT:
                        RedirectToActionPage(IPC.ACTIONPAGE.EDIT, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                        break;
                    case IPC.ACTIONPAGE.DETAILS:
                        RedirectToActionPage(IPC.ACTIONPAGE.DETAILS, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                        break;
                    case IPC.ACTIONPAGE.DELETE:
                        deletefeeShare(commandArg);
                        AutoSwitchSearch();
                        break;
                }
        }
        else
        {
            lblError.Text = "User does not have permission.";
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (!CheckPermitPageAction(IPC.ACTIONPAGE.DELETE))
        {
            lblError.Text = "User does not have permission.";
            return;
        }
        try
        {
            string[] lst = hdCLMS_SCO_SCO_PRODUCT.Value.ToString().Split('#');
            if (lst.Length <= 1)
            {
                lblError.Text = Resources.labels.Selectoneormoretodelete;
                return;
            }
            foreach (string item in lst)
            {
                if (item.Equals("") || item.Equals("on")) continue;
                deletefeeShare(item);
                if (!IPCERRORCODE.Equals("0"))
                {
                    lblError.Text = IPCERRORDESC;
                }
            }
            if (IPCERRORCODE.Equals("0"))
            {
                lblError.Text = Resources.labels.deletesuccessfully;
                if (rptData.Items.Count == 0)
                {
                    int SelectPageNo = int.Parse(GridViewPaging.SelectPageChoose.ToString()) - 1;
                    GridViewPaging.SelectPageChoose = SelectPageNo.ToString();
                }
            }
            AutoSwitchSearch();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }

    public void deletefeeShare(string FeeShareID)
    {
        try
        {
            DataSet ds = new DataSet();
            object[] searchObject = new object[] { Utility.KillSqlInjection(FeeShareID) };
            _service.common("SEMSSHAREFEEDEL", searchObject, ref IPCERRORCODE, ref IPCERRORDESC); 
            if (IPCERRORCODE.Equals("0"))
            {
                lblError.Text = Resources.labels.deletesuccessfully;
            }
            else
            {
                lblError.Text = IPCERRORDESC;
                return;
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void btnAdd_New_Click(object sender, EventArgs e)
    {
        RedirectToActionPage(IPC.ACTIONPAGE.ADD, string.Empty);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        pnResult.Visible = false;
        txtSearch.Text = string.Empty;
        txtFeeShareCode.Text = string.Empty;
        txtFeeShareName.Text = String.Empty;
        loadCombobox();
        hdCLMS_SCO_SCO_PRODUCT.Value = string.Empty;
    }
    void AutoSwitchSearch()
    {
        if (flag == 1)
        {
            BindData();
        }
        if (flag == 0)
        {
            BindData_SearchAdvance();
        }
        hdCLMS_SCO_SCO_PRODUCT.Value = string.Empty;
    }
}
