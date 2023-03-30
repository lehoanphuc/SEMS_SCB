﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SmartPortal.Common.Utilities;
using SmartPortal.ExceptionCollection;



public partial class Widgets_SEMSBank_Controls_Widget :System.Web.UI.UserControl
{
    string ACTION = "";
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
   
    public string _IMAGE
    {
        get { return imgLoGo.ImageUrl; }
        set { imgLoGo.ImageUrl = value; }
    }

    public string _TITLE
    {
        get { return lblTitleProduct.Text; }
        set { lblTitleProduct.Text = value; }
    }
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ACTION = SmartPortal.Common.Encrypt.GetURLParam(System.Web.HttpContext.Current.Request.RawUrl)["a"].ToString().Trim();

            //lblErro.Text = "";
            pnAdd.Visible = true;
            pnResult.Visible = false;
           if (!IsPostBack)
            {
                 BindData();
                      
            }
        }
        catch (Exception ex)
        {
        }
     
    }
    void BindData() 
    {

        try
        {



            switch (ACTION)
            {
                case "add":
                    break;
                default:
                    #region Lấy thông tin san pham

                    DataTable productTable = new DataTable();
                    productTable = new SmartPortal.SEMS.Bank().LoadAllBank(SmartPortal.Common.Encrypt.GetURLParam(System.Web.HttpContext.Current.Request.RawUrl)["bid"].ToString().Trim(),"");
                    if (productTable.Rows.Count != 0)
                    {
                        txtBank.Text = productTable.Rows[0]["BANKNAME"].ToString();
                    }
                    #endregion


                    break;
            }
            #region Enable/Disable theo Action
            switch (ACTION)
            {
                case "viewdetail":
                    txtBank.Enabled = false;
                    break;
                case "edit":
                    txtBank.Enabled = true;

                    break;
            }
            #endregion 

        }
        catch
        {
        }
    }
    protected void btsave_Click(object sender, EventArgs e)
    {
        string bankname = SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtBank.Text.Trim());
      try
        {
        switch (ACTION)
        {
            case "add":
                    int insert = -1;

                    insert = new SmartPortal.SEMS.Bank().InsertBank(bankname);

                    if (insert != -1)
                    {
                        throw new IPCException("");
                    }
                    else
                    {
                        lbResult.Text = Resources.labels.themnganhangthanhcong;
                        pnAdd.Visible = false;
                        pnResult.Visible = true;
                        btsave.Visible = false;

                    }

                    break;

                break;

            case "edit":
                int insertt = -1;

                insertt =    new SmartPortal.SEMS.Bank().EditBank(SmartPortal.Common.Encrypt.GetURLParam(System.Web.HttpContext.Current.Request.RawUrl)["bid"].ToString().Trim(), bankname);

                    if (insertt != -1)
                    {
                        throw new IPCException("");
                    }
                    else
                    {
                        lbResult.Text = Resources.labels.themnganhangthanhcong;
                        pnAdd.Visible = false;
                        pnResult.Visible = true;
                        btsave.Visible = false;
                        
                    }
                       
                    break;
        }
        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(IPCex.ToString(), this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(IPCex.Message, Request.Url.Query);

        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
        
      
    }
    protected void btback_Click(object sender, EventArgs e)
    {
       Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/default.aspx?p=403"));
    }
}
