﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Web;

public partial class Widgets_SEMSUserTypeIsBackOffice_Widget : WidgetBase
{
    public static bool isAscend = false;
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    
    SmartPortal.SEMS.Common _service = new SmartPortal.SEMS.Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.ForeColor = System.Drawing.Color.Red;
        try
        {
            lblError.Text = string.Empty;
            if (!IsPostBack)
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }
    void setControlDefault()
    {
        lblError.Text = string.Empty;
        txtTransactionNumber.Text = string.Empty;
        txtTransactionDate.Text = string.Empty;
        txtUserName.Text = string.Empty;
        txtUserCode.Text = string.Empty;
        loadCombobox();
    }

    void loadCombobox_UserType()
    {
        DataSet ds = new DataSet();
        ds = _service.GetValueList("WAL_Value_List", "USR", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddUserType.DataSource = ds;
                ddUserType.DataValueField = "VALUE_ID";
                ddUserType.DataTextField = "CAPTION";
                ddUserType.DataBind();
            }
        }
    }
    void loadCombobox()
    {
        DataSet ds = new DataSet();
        ds = _service.GetValueList("EBA_Contract", "STT", ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE == "0")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDestinationStatus.DataSource = ds;
                ddlDestinationStatus.DataValueField = "VALUE_ID";
                ddlDestinationStatus.DataTextField = "CAPTION";
                ddlDestinationStatus.DataBind();

                ddlSourceStatus.DataSource = ds;
                ddlSourceStatus.DataValueField = "VALUE_ID";
                ddlSourceStatus.DataTextField = "CAPTION";
                ddlSourceStatus.DataBind();
            }
        }
    }

    void disableControl()
    {
        ddlSourceStatus.Enabled = false;
        txtTransactionDate.Enabled = false;
        txtTransactionNumber.Enabled = false;
        txtUserName.Enabled = false;
    }
    void BindData()
    {
        try
        {
            loadCombobox();
            loadCombobox_UserType();
            ddUserType.SelectedValue = "BO";
            txtUserCode.SetUserType = ddUserType.SelectedValue;
            disableControl();
            
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }
    protected void loadInfo(object sender, EventArgs e)
    {
        txtUserCode.SetUserType = ddUserType.SelectedValue;
        txtUserCode.refreshPopup();
    }
    void setPara(Dictionary<object, object> inforConsumer)
    {
        
        //inforConsumer.Add(IPC.IPCTRANSID, "");
        //inforConsumer.Add("USERID", HttpContext.Current.Session["userID"].ToString());
        //inforConsumer.Add("LEVEL_CODE", ddlSourceWalletLevel.SelectedValue);
        //inforConsumer.Add("LEVEL_CHANGE_CODE", ddlDestinationWalletLevel.SelectedValue);
    }

    void defaultColor()
    {
        txtUserName.BorderColor = System.Drawing.Color.Empty;
    }
    protected void btnAccept_click(object sender, EventArgs e)
    {
        try
        {
            #region validation
            defaultColor();
            if (txtUserName.Text.Equals(string.Empty))
            {
                lblError.Text = Resources.labels.tendangnhap + " is not null";
                txtUserName.BorderColor = System.Drawing.Color.Red;
                txtUserName.Focus();
                return;
            }
            if (txtUserCode.Text.Equals(string.Empty))
            {
                lblError.Text = Resources.labels.fullname + " is not null";
                txtUserName.BorderColor = System.Drawing.Color.Red;
                txtUserName.Focus();
                return;
            }
            #endregion
            DataSet ds = new DataSet();
            Dictionary<object, object> inforConsumer = new Dictionary<object, object>();
            setPara(inforConsumer);
            ds = _service.CallStore("SEMS_BO_CHANGE_WALLV", inforConsumer,"Change wallet account level","N", ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTransactionNumber.Text = ds.Tables[0].Rows[0]["TXREFID"].ToString();
                    txtTransactionDate.Text = ds.Tables[0].Rows[0]["TXDT"].ToString();
                    lblError.Text = Resources.labels.success;
                }
            }
            else
            {
                lblError.Text = IPCERRORDESC;
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }
    protected void btnClear_click(object sender, EventArgs e)
    {
        setControlDefault();
        defaultColor();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=129"));
    }
}