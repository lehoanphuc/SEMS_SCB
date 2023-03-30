﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using SmartPortal.Common.Utilities;
using SmartPortal.Constant;

public partial class Widgets_SEMSReasons_Widget : WidgetBase
{
    public static bool isAscend = false;
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    public static int flag = 1;
    SmartPortal.SEMS.Common _common = new SmartPortal.SEMS.Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (flag == 1)
            {
                GridViewPaging.pagingClickArgs += new EventHandler(Search_GridViewPaging_click);
            }
            if (flag == 0)
            {
                GridViewPaging.pagingClickArgs += new EventHandler(AdvanceSearch_GridViewPaging_click);
            }
            lblError.Text = string.Empty;
            //if (!IsPostBack)
            //{
            //    BindData();
            //}
            loadCombobox();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    private void loadReasonType()
    {
        DataSet ds = new DataSet();
        ds = _common.GetValueList("WAL_REASON_DEFINITION", "TYPE", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddreasontype.DataSource = ds;
                ddreasontype.DataValueField = "VALUEID";
                ddreasontype.DataTextField = "CAPTION";
                ddreasontype.DataBind();
                ddreasontype.Items.Insert(0, new ListItem(Resources.labels.tatca, ""));
            }
        }
    }
    private void loadReasonAction()
    {
        DataSet ds = new DataSet();
        ds = _common.GetValueList("WAL_REASON_DEFINITION", "ACTION", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddreasonaction.DataSource = ds;
                ddreasonaction.DataValueField = "VALUEID";
                ddreasonaction.DataTextField = "CAPTION";
                ddreasonaction.DataBind();
                ddreasonaction.Items.Insert(0, new ListItem(Resources.labels.tatca, ""));
            }
        }
    }
    private void loadEvent()
    {
        DataSet ds = new DataSet();
        ds = _common.GetValueList("WAL_REASON_DEFINITION", "EVENT", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddEvent.DataSource = ds;
                ddEvent.DataValueField = "VALUEID";
                ddEvent.DataTextField = "CAPTION";
                ddEvent.DataBind();
                ddEvent.Items.Insert(0, new ListItem(Resources.labels.tatca, ""));
            }
        }
    }
    private void loadStatus()
    {
        DataSet ds = new DataSet();
        ds = _common.GetValueList("WAL_REASON_DEFINITION", "STT", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddstatus.DataSource = ds;
                ddstatus.DataValueField = "VALUEID";
                ddstatus.DataTextField = "CAPTION";
                ddstatus.DataBind();
                ddstatus.Items.Insert(0, new ListItem(Resources.labels.tatca, ""));
            }
        }
    }

    private void loadCombobox()
    {
        loadReasonAction();
        loadReasonType();
        loadEvent();
        loadStatus();
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
    protected void btnClear_click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        txtReasonName.Text = string.Empty;
        txtReasonCode.Text = string.Empty;
        ddEvent.SelectedValue = string.Empty;
        ddstatus.SelectedValue = string.Empty;
        ddreasonaction.SelectedValue = string.Empty;
        ddreasontype.SelectedValue = string.Empty;
        rptData.DataSource = null;
        rptData.DataBind();
        p.Visible = false;
        hdCLMS_SCO_SCO_PRODUCT.Value = string.Empty;
    }


    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        //if (CheckPermitPageAction(IPC.ACTIONPAGE.ADD))
        //{
        //    RedirectToActionPage(IPC.ACTIONPAGE.ADD, string.Empty);
        //}
        RedirectToActionPage(IPC.ACTIONPAGE.ADD, string.Empty);
    }

    void BindData()
    {
        try
        {
            p.Visible = true;
            flag = 1;
            rptData.DataSource = null;
            loadCombobox();
            DataSet ds = new DataSet();
            object[] searchObject = new object[] { Utility.KillSqlInjection(txtSearch.Text.Trim()), GridViewPaging.pageIndex * GridViewPaging.pageSize, GridViewPaging.pageSize };
            ds = _common.common("SEMS_REASON_SEARCH", searchObject, ref IPCERRORCODE, ref IPCERRORDESC);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
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
            p.Visible = true;
            rptData.DataSource = null;
            flag = 0;
            DataSet ds = new DataSet();
            object[] searchObject = new object[] { Utility.KillSqlInjection(txtReasonCode.Text.Trim()), Utility.KillSqlInjection(txtReasonName.Text.Trim()), Utility.KillSqlInjection(ddreasonaction.SelectedValue), Utility.KillSqlInjection(ddreasontype.SelectedValue), Utility.KillSqlInjection(ddEvent.SelectedValue), Utility.KillSqlInjection(ddstatus.SelectedValue), GridViewPaging.pageIndex * GridViewPaging.pageSize, GridViewPaging.pageSize };
            ds = _common.common("SEMS_REASON_ADV", searchObject, ref IPCERRORCODE, ref IPCERRORDESC);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
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
    protected void rptData_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
    }

    protected void rptData_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string commandName = e.CommandName;
        string commandArg = e.CommandArgument.ToString();
        //if (CheckPermitPageAction(commandName))
        //{
        switch (commandName)
        {
            case IPC.ACTIONPAGE.EDIT:
                RedirectToActionPage(IPC.ACTIONPAGE.EDIT, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                break;
            case IPC.ACTIONPAGE.DETAILS:
                RedirectToActionPage(IPC.ACTIONPAGE.DETAILS, "&" + SmartPortal.Constant.IPC.ID + "=" + commandArg);
                break;

            case IPC.ACTIONPAGE.DELETE:
                deleteFunction(commandArg);
                AutoSwitchSearch();
                break;
        }
        //}
    }
    protected void btnDelete_Click(object sender, EventArgs e)
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
            deleteFunction(item);
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
    public void deleteFunction(string departID)
    {
        try
        {
            DataSet ds = new DataSet();
            object[] searchObject = new object[] { Utility.KillSqlInjection(departID) };
            _common.common("SEMS_REASON_DEL", searchObject, ref IPCERRORCODE, ref IPCERRORDESC);
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