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
using SmartPortal.Security;
using System.Text;
using SmartPortal.ExceptionCollection;
using SmartPortal.SEMS;
using System.Collections.Generic;
using System.Linq;

public partial class Widgets_SEMSContractList_Add_Widget : WidgetBase
{
    int i = 0;
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    string userName;
    string usernameAuthorized;
    internal int minlength = int.Parse(ConfigurationManager.AppSettings["minlengthloginname"].ToString());
    internal int maxlength = int.Parse(ConfigurationManager.AppSettings["maxlengthloginname"].ToString());
    static List<string> lsAccNo = new List<string>();

    protected void Page_Init(object sender, EventArgs e)
    {
        TabCustomerInfoHelper.LoadConfig();
    }
    public string PHONECTK
    {
        get { return ViewState["PHONECTK"] != null ? (string)ViewState["PHONECTK"] : string.Empty; }
        set { ViewState["PHONECTK"] = value; }
    }
    public string USERCTK
    {
        get { return ViewState["USERCTK"] != null ? (string)ViewState["USERCTK"] : string.Empty; }
        set { ViewState["USERCTK"] = value; }
    }
    public string USERNUQ
    {
        get { return ViewState["USERNUQ"] != null ? (string)ViewState["USERNUQ"] : string.Empty; }
        set { ViewState["USERNUQ"] = value; }
    }
    public string PHONENUQ
    {
        get { return ViewState["PHONENUQ"] != null ? (string)ViewState["PHONENUQ"] : string.Empty; }
        set { ViewState["PHONENUQ"] = value; }
    }
    public decimal LEVELCONTRACT
    {
        get { return ViewState["LEVELCONTRACT"] != null ? (decimal)ViewState["LEVELCONTRACT"] : -1; }
        set { ViewState["LEVELCONTRACT"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblError.Text = "";
            lblAlert.Text = "";
            lblAlertDSH.Text = "";

            if (rbType.Checked)
                txtIBTypeUserName.Enabled = true;
            else
                txtIBTypeUserName.Enabled = false;

            if (!IsPostBack)
            {
                ddlCustType.Items.Add(new ListItem(Resources.labels.canhan, "P"));
                ddlCustType.Items.Add(new ListItem(Resources.labels.linkage, "J"));
                ddlCustType.Items.Add(new ListItem(Resources.labels.doanhnghiep, "O"));
                ddlProduct.Items.Add(new ListItem(Resources.labels.ebankingcanhan, "PCN"));
                ddlProduct.Items.Add(new ListItem(Resources.labels.ebankingdoanhnghiep, "PDN"));
                ddlReGender.Items.Add(new ListItem(Resources.labels.male, "M"));
                ddlReGender.Items.Add(new ListItem(Resources.labels.female, "F"));
                ddlGenderNguoiUyQuyen.Items.Add(new ListItem(Resources.labels.male, "M"));
                ddlGenderNguoiUyQuyen.Items.Add(new ListItem(Resources.labels.female, "F"));

                #region Ẩn các panel
                pnOption.Visible = true;
                pnCustInfo.Visible = false;
                pnContractInfo.Visible = false;
                pnPersonal.Visible = false;
                #endregion
                #region load level contract

                LEVELCONTRACT = decimal.Parse(new SmartPortal.SEMS.Contract().GetContractLevelID(ref IPCERRORCODE, ref IPCERRORDESC));

                #endregion
                //load policy to dropdownlist
                DataSet dspolicy = new DataSet();
                string filterIB = "serviceid='IB'";
                string filterSMS = "serviceid='SMS'";
                string filterMB = "serviceid='MB'";
                string stSort = "serviceid asc";
                int ibrow = 0;
                int smsrow = 0;
                int mbrow = 0;
                int amrow = 0;
                dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE == "0")
                {
                    if (dspolicy.Tables[0].Rows.Count == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "");
                        btnStart.Enabled = false;
                        return;
                    }
                    foreach (DataRow r in dspolicy.Tables[0].Rows)
                    {
                        //if (r["serviceid"].ToString().Trim() == "IB")
                        //{
                        //    ibrow = ibrow + 1;
                        //}
                        //if (r["serviceid"].ToString().Trim() == "SMS")
                        //{
                        //    smsrow = smsrow + 1;
                        //}
                        if (r["serviceid"].ToString().Trim() == "MB")
                        {
                            mbrow = mbrow + 1;
                        }
                        if (r["serviceid"].ToString().Trim() == "AM")
                        {
                            amrow = amrow + 1;
                        }
                    }
                    //if (smsrow == 0)
                    //{
                    //    lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "SMS");
                    //    btnStart.Enabled = false;
                    //    return;
                    //}
                    if (mbrow == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "MB");
                        btnStart.Enabled = false;
                        return;
                    }
                    if (amrow == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "AM");
                        btnStart.Enabled = false;
                        return;
                    }
                    DataTable dt = dspolicy.Tables[0];
                    DataTable dtIB = dt.Select(filterIB, stSort).Any() ? dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable() : null;
                    DataTable dtSMS = dt.Select(filterSMS, stSort).Any() ? dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable() : null;
                    DataTable dtMB = dt.Select(filterMB, stSort).Any() ? dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable() : null;

                    ddlpolicyIB.DataSource = dtIB;
                    ddlpolicySMS.DataSource = dtSMS;
                    ddlpolicyMB.DataSource = dtMB;
                    ddlpolicyWL.DataSource = dtMB;

                    ddlpolicyIBco.DataSource = dtIB;
                    ddlpolicySMSco.DataSource = dtSMS;
                    ddlpolicyMBco.DataSource = dtMB;
                    ddlpolicyWLco.DataSource = dtMB;

                    ddlpolicyIB.DataTextField = "policytx";
                    ddlpolicyIB.DataValueField = "policyid";
                    ddlpolicySMS.DataTextField = "policytx";
                    ddlpolicySMS.DataValueField = "policyid";
                    ddlpolicyMB.DataTextField = "policytx";
                    ddlpolicyMB.DataValueField = "policyid";
                    ddlpolicyWL.DataTextField = "policytx";
                    ddlpolicyWL.DataValueField = "policyid";
                    ddlpolicyIBco.DataTextField = "policytx";
                    ddlpolicyIBco.DataValueField = "policyid";
                    ddlpolicySMSco.DataTextField = "policytx";
                    ddlpolicySMSco.DataValueField = "policyid";
                    ddlpolicyMBco.DataTextField = "policytx";
                    ddlpolicyMBco.DataValueField = "policyid";
                    ddlpolicyWLco.DataTextField = "policytx";
                    ddlpolicyWLco.DataValueField = "policyid";

                    ddlpolicyIB.DataBind();
                    ddlpolicySMS.DataBind();
                    ddlpolicyMB.DataBind();
                    ddlpolicyWL.DataBind();

                    ddlpolicyIBco.DataBind();
                    ddlpolicySMSco.DataBind();
                    ddlpolicyMBco.DataBind();
                    ddlpolicyWLco.DataBind();
                    //disable mb
                    ddlpolicyWL.Enabled = false;
                    ddlpolicyWLco.Enabled = false;


                }

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
    protected void txtPhoneNguoiUyQuyen_TextChanged(object sender, EventArgs e)
    {

        if (!CheckIsPhoneNumer(txtPhoneNguoiUyQuyen.Text) || txtReMobi.Text.Trim() == txtPhoneNguoiUyQuyen.Text.Trim())
        {
            lblError.Text = Resources.labels.phonenumberwrong + " for co-owner";
            return;
        }
        if (!CheckExistPhoneNumber(txtPhoneNguoiUyQuyen.Text))
        {
            lblError.Text = Resources.labels.phonenumberisalreadyregistered + " for co-owner";
            return;
        }
        else
        {
            PHONENUQ = txtPhoneNguoiUyQuyen.Text;

            txtMBPhoneNguoiUyQuyen.Text = txtSMSPhoneNguoiUyQuyen.Text = PHONENUQ;
        }
    }
    protected void btnStart_Click(object sender, EventArgs e)
    {

        if (radCustExists.Checked)
        {
            pnOption.Visible = false;
            pnCustInfo.Visible = true;

            //search customer
            BindData();
        }
        else if (radWalletOnly.Checked)
        {
            Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=1118"));
        }
        else
        {
            Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=147"));
        }

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable contractTable = (new SmartPortal.SEMS.Contract().GetContractByCustcode(hidCustID.Value.Trim(), ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

            if (IPCERRORCODE != "0")
            {
                SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnNext_Click", IPCERRORDESC, Request.Url.Query);
                SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);

            }

            if (contractTable.Rows.Count != 0)
            {
                foreach (DataRow row in contractTable.Rows)
                {
                    if (row["STATUS"].ToString().Trim() != SmartPortal.Constant.IPC.DELETE)
                    {
                        goto ALERT;
                    }
                }
            }

            //lay thong tin khach hang de chon loai hinh san pham
            DataSet ds = new SmartPortal.SEMS.Customer().GetCustomerByCustcode(hidCustID.Value, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE != "0")
            {
                SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnNext_Click", IPCERRORDESC, Request.Url.Query);
                SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);

            }
            DataTable dtCust = new DataTable();
            dtCust = ds.Tables[0];
            if (dtCust.Rows.Count != 0)
            {
                //ddlProduct.DataSource = new SmartPortal.SEMS.Product().GetProductByCondition("", "", dtCust.Rows[0]["CFTYPE"].ToString().Trim(), "", ref IPCERRORCODE, ref IPCERRORDESC);
                ddlProduct.DataSource = new SmartPortal.SEMS.Product().GetProductByCondition("", "", "P", "", ref IPCERRORCODE, ref IPCERRORDESC);
                ddlProduct.DataTextField = "PRODUCTNAME";
                ddlProduct.DataValueField = "PRODUCTID";
                ddlProduct.DataBind();
            }
            #region load usertype
            DataSet dsUserType = new DataSet();
            DataTable dtUserType = new DataTable();

            dtUserType = new SmartPortal.SEMS.Contract().LoadContractType(SmartPortal.Constant.IPC.PERSONAL, "Y");

            ddlContractType.DataSource = dtUserType;
            ddlContractType.DataTextField = "TYPENAME";
            ddlContractType.DataValueField = "USERTYPE";
            ddlContractType.DataBind();

            #endregion

            #region hien thi mac dinh ma hop dong 
            //txtContractNo.Text = SmartPortal.Constant.IPC.CONTRACTNOPREFIX + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8);
            //hien thi ma hop dong

            //txtContractNo.Text = SmartPortal.Common.Utilities.Utility.GetID("HD", dtCust.Rows[0]["CUSTCODE"].ToString().Trim(), "P",15);
            //MA HD
            //txtContractNo.Text = SmartPortal.Common.Utilities.Utility.GetID(SmartPortal.Constant.IPC.CONTRACTNOPREFIX, dtCust.Rows[0]["CUSTCODE"].ToString().Trim(), "P",15);
            txtContractNo.Text = SmartPortal.Common.Utilities.Utility.GetID(SmartPortal.Constant.IPC.CONTRACTNOPREFIX, dtCust.Rows[0]["CUSTCODE"].ToString().Trim(), dtCust.Rows[0]["CFTYPE"].ToString().Trim(), 15);

            txtContractNo.Enabled = false;
            txtStartDate.Text = Utility.FormatDatetime(SmartPortal.Constant.IPC.LoadWorkingDate(), "dd/MM/yyyy", DateTimeStyle.Date);
            txtEndDate.Text = Utility.FormatDatetime(SmartPortal.Common.Utilities.Utility.IsDateTime1(SmartPortal.Constant.IPC.LoadWorkingDate()).AddYears(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeStyle.Date);
            #endregion

            //an cac panel
            pnOption.Visible = false;
            pnCustInfo.Visible = false;
            pnContractInfo.Visible = true;


            goto EXIT;
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

    ALERT:
        SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ceec"], Request.Url.Query);
    EXIT:;

    }
    void ReleaseSession()
    {
        ViewState["CHUTAIKHOAN"] = null;
        ViewState["NGUOIUYQUYEN"] = null;

    }

    void LoadDataInTreeview(string serviceID, TreeView tvPage, string userType)
    {
        tvPage.Nodes.Clear();
        DataTable tblSS = new DataTable();
        //tblSS = new SmartPortal.SEMS.Role().GetByServiceAndUserType(serviceID, userType,"");
        tblSS = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(serviceID, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        foreach (DataRow row in tblSS.Rows)
        {
            TreeNode node = new TreeNode(row["ROLENAME"].ToString(), row["ROLEID"].ToString());
            node.ShowCheckBox = true;

            DataSet dsTransaction = new DataSet();
            DataTable tblPage = new DataTable();
            dsTransaction = new SmartPortal.SEMS.Role().GetTranOfRole(row["ROLEID"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                tblPage = dsTransaction.Tables[0];
            }
            foreach (DataRow row1 in tblPage.Rows)
            {
                TreeNode node1 = new TreeNode(row1["PAGENAME"].ToString(), row1["TRANCODE"].ToString());
                node1.ShowCheckBox = false;
                node1.ToolTip = row1["PageDescription"].ToString();

                node.ChildNodes.Add(node1);
            }
            if (tblPage.Rows.Count != 0)
            {
                tvPage.Nodes.Add(node);
            }
        }
        tvPage.Attributes.Add("onclick", "OnTreeClick(event)");
    }

    void GetRoleDefault(TreeView treeIB, TreeView treeSMS, TreeView treeMB, TreeView treePHO, TreeView treeWL)
    {
        DataTable tblRoleDefault = new DataTable();
        //lay role mac dinh IB
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.IB, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        foreach (TreeNode liIB in treeIB.Nodes)
        {
            if (tblRoleDefault.Select("ROLEID=" + liIB.Value).Length != 0)
            {
                liIB.Checked = true;

                //check node con (Trancode)
                foreach (TreeNode tnTranCode in liIB.ChildNodes)
                {
                    tnTranCode.Checked = true;
                }
            }
            else
            {
                liIB.Checked = false;
            }
        }

        //lay role mac dinh SMS
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.SMS, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        //vutt 30112016 off by default 4 sms notification right
        foreach (TreeNode liSMS in treeSMS.Nodes)
        {
            DataRow[] dr = tblRoleDefault.Select("ROLEID=" + liSMS.Value);
            if (dr.Length != 0)
            {
                bool flag = !dr[0]["RoleType"].ToString().Equals("SNO");

                liSMS.Checked = flag;

                //check node con (Trancode)
                foreach (TreeNode tnTranCode in liSMS.ChildNodes)
                {
                    tnTranCode.Checked = flag;
                }
            }
            else
            {
                liSMS.Checked = false;
            }
        }


        //lay role mac dinh MB
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.MB, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        foreach (TreeNode liMB in treeMB.Nodes)
        {
            if (tblRoleDefault.Select("ROLEID=" + liMB.Value).Length != 0)
            {
                liMB.Checked = true;

                //check node con (Trancode)
                foreach (TreeNode tnTranCode in liMB.ChildNodes)
                {
                    tnTranCode.Checked = true;
                }
            }
            else
            {
                liMB.Checked = false;
            }
        }
        //lay role mac dinh WL
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.EW, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.PRCTYPECONSUMER, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        foreach (TreeNode liWL in treeWL.Nodes)
        {
            if (tblRoleDefault.Select("ROLEID=" + liWL.Value).Length != 0)
            {
                liWL.Checked = true;

                //check node con (Trancode)
                foreach (TreeNode tnTranCode in liWL.ChildNodes)
                {
                    tnTranCode.Checked = true;
                }
            }
            else
            {
                liWL.Checked = false;
            }
        }

        //lay role mac dinh PHO
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.PHO, ddlProduct.SelectedValue, LEVELCONTRACT, SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

        foreach (TreeNode liPHO in treePHO.Nodes)
        {
            if (tblRoleDefault.Select("ROLEID=" + liPHO.Value).Length != 0)
            {
                liPHO.Checked = true;

                //check node con (Trancode)
                foreach (TreeNode tnTranCode in liPHO.ChildNodes)
                {
                    tnTranCode.Checked = true;
                }
            }
            else
            {
                liPHO.Checked = false;
            }
        }
    }

    protected void btnContractNext_Click(object sender, EventArgs e)
    {

        try
        {
            //check ngay thang
            if (SmartPortal.Common.Utilities.Utility.IsDateTimeViet(txtStartDate.Text.Trim()))
            {
            }
            else
            {
                lblError.Text = Resources.labels.datetimeinvalid;
                return;
            }

            if (SmartPortal.Common.Utilities.Utility.IsDateTimeViet(txtEndDate.Text.Trim()))
            {
            }
            else
            {
                lblError.Text = Resources.labels.datetimeinvalid;
                return;
            }

            #region lấy thông tin khách hàng qua người dùng
            string cc = "";
            string ct = "";
            DataSet dsCust = new SmartPortal.SEMS.Customer().GetCustomerByCustcode(hidCustID.Value, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE != "0")
            {
                throw new IPCException(IPCERRORDESC);
            }
            else
            {
                DataTable tblCust = dsCust.Tables[0];
                if (tblCust.Rows.Count != 0)
                {
                    //edit by vutran 22/07/2014 lấy thông tin tài khoản từ core
                    switch (tblCust.Rows[0]["CFTYPE"].ToString().Replace(" ", ""))
                    {
                        case "P":
                            cc = tblCust.Rows[0]["CUSTCODE"].ToString().Trim();
                            break;
                        case "O":
                            cc = tblCust.Rows[0]["CFCODE"].ToString().Trim();
                            break;
                        case "J":
                            cc = tblCust.Rows[0]["CFCODE"].ToString().Trim();
                            break;
                    }
                    //cc = tblCust.Rows[0]["CUSTCODE"].ToString().Trim();
                    ct = tblCust.Rows[0]["CFTYPE"].ToString().Trim();

                    txtReFullName.Text = tblCust.Rows[0]["FULLNAME"].ToString();
                    txtReBirth.Text = SmartPortal.Common.Utilities.Utility.IsDateTime2(tblCust.Rows[0]["DOB"].ToString()).ToString("dd/MM/yyyy");

                    if (!string.IsNullOrEmpty(tblCust.Rows[0]["SEX"].ToString().Trim()))
                        //ddlReGender.SelectedValue = tblCust.Rows[0]["SEX"].ToString();
                        ddlReGender.SelectedValue = tblCust.Rows[0]["SEX"].ToString().Trim() == string.Empty ? "F" : tblCust.Rows[0]["SEX"].ToString().Trim();
                    PHONECTK = txtReMobi.Text = tblCust.Rows[0]["TEL"].ToString();
                    txtReEmail.Text = tblCust.Rows[0]["EMAIL"].ToString();
                    txtReAddress.Text = tblCust.Rows[0]["ADDRRESIDENT"].ToString();

                }
            }

            txtReFullName.Enabled = false;
            txtReBirth.Enabled = false;
            ddlReGender.Enabled = false;
            txtReMobi.Enabled = false;
            txtReEmail.Enabled = false;
            txtReAddress.Enabled = false;
            #endregion

            //checkbox
            radAllAccount.Checked = true;
            radAllAccountNguoiUyQuyen.Checked = true;

            //release session
            ReleaseSession();
            //huy du lieu luoi
            gvResultChuTaiKhoan.DataSource = null;
            gvResultChuTaiKhoan.DataBind();

            gvResultNguoiUyQuyen.DataSource = null;
            gvResultNguoiUyQuyen.DataBind();


            //diable account
            ddlAccount.Enabled = false;
            ddlAccountUyQuyen.Enabled = false;

            //Get Cust infomation from core Because need NRC from generate ID
            Hashtable hasCusInfo = new Hashtable();
            hasCusInfo = new SmartPortal.SEMS.Customer().GetCustInfo(cc, ct, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (hasCusInfo[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }

            string fullname = "";
            string custIF = "";
            if (hasCusInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
                fullname = hasCusInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (hasCusInfo[SmartPortal.Constant.IPC.LICENSE] != null)
                custIF = hasCusInfo[SmartPortal.Constant.IPC.LICENSE].ToString();

            //string strCode = cc.Trim() + ct.Trim() + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 4);
            USERCTK = txtMBUsersName.Text = SmartPortal.Common.Utilities.Utility.GetID(fullname, cc, custIF);
            //txtUserNameNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), ct.Trim(),12) + "2";
            //17.9.2015 minh disable generate userib co owner
            // txtIBGenUserNameNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID(fullname, cc, custIF, txtIBGenUserName.Text);
            //edit by vutran 05082014 IB,MB sample user
            if (int.Parse(ConfigurationManager.AppSettings["MBWLSameUser"].ToString()) == 1)
            {
                txtMBPhoneNo.Text = txtWLPhoneNo.Text = PHONECTK;
                txtWLNguoiUyQuyen.Text = PHONECTK;
            }
            else
            {
                txtMBUsersName.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "", 10) + "1";
                txtMBUserNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "", 10) + "2";
            }
            if (int.Parse(ConfigurationManager.AppSettings["MBSMSSameUser"].ToString()) == 1)
            {
                txtSMSPhoneNguoiUyQuyen.Text = PHONECTK;
                txtSMSPhoneNo.Enabled = txtSMSPhoneNguoiUyQuyen.Enabled = false;
                txtSMSPhoneNo.Text = PHONECTK;
            }

            txtPHOPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "", 10) + "1";
            txtPHOPhoneNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "", 10) + "2";

            btnResetNguoiUyQuyen.Attributes.Add("onclick", "return SetUserName('" + cc.Trim() + "','" + ct.Trim() + "',12)");

            txtIBGenUserName.Enabled = false;
            txtIBGenUserNameNguoiUyQuyen.Enabled = false;


            #region Chủ tài khoản
            lblError.Text = "";

            #region lay tat ca cac account cua khach hang           


            DataSet ds = new DataSet();
            ds = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ct, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.IPC);
            }
            else
            {
                if (ds.Tables.Count == 0)
                {
                    throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTHAVEACCTNO);
                }
            }

            DataTable dtAccountQT = new DataTable();
            dtAccountQT = ds.Tables[0];

            ViewState["AccountList"] = dtAccountQT;

            if (dtAccountQT.Rows.Count == 0)
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTHAVEACCTNO);
            }
            else
            {
                ddlSMSDefaultAcctno.DataSource = dtAccountQT;
                ddlSMSDefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlSMSDefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlSMSDefaultAcctno.DataBind();

                ddlCTKDefaultAcctno.DataSource = dtAccountQT;
                ddlCTKDefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlCTKDefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlCTKDefaultAcctno.DataBind();

                ddlCTKDefaultAcctno2.DataSource = dtAccountQT;
                ddlCTKDefaultAcctno2.DataTextField = "ACCOUNTNO";
                ddlCTKDefaultAcctno2.DataValueField = "ACCOUNTNO";
                ddlCTKDefaultAcctno2.DataBind();

                ddlAccount.DataSource = dtAccountQT;
                ddlAccount.DataTextField = "ACCOUNTNO";
                ddlAccount.DataValueField = "ACCOUNTNO";
                ddlAccount.DataBind();


                ddlAccountMB.DataSource = dtAccountQT;
                ddlAccountMB.DataTextField = "ACCOUNTNO";
                ddlAccountMB.DataValueField = "ACCOUNTNO";
                ddlAccountMB.DataBind();
                ddlAccountMB.Items.Insert(0, new ListItem("ALL", "ALL"));

                //phongtt sms notification fee
                lsAccNo.Clear();
                foreach (DataRow dr in dtAccountQT.Rows)
                {
                    if (!lsAccNo.Contains(dr["ACCOUNTNO"].ToString()))
                    {
                        lsAccNo.Add(dr["ACCOUNTNO"].ToString());
                    }
                }


                //ddlAccount.Items.Insert(0,new ListItem("----------Chọn tài khoản----------",""));

            }
            #endregion

            #region Hien thi tat cac cac role theo serviceid va usertype len cay
            //load for IB
            LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIB, ddlContractType.SelectedValue);

            //load for SMS
            LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMS, ddlContractType.SelectedValue);

            //load for MB
            LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMB, ddlContractType.SelectedValue);

            //load for PHO
            LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHO, ddlContractType.SelectedValue);

            //load for Wallet
            LoadDataInTreeview(SmartPortal.Constant.IPC.EW, tvWL, ddlContractType.SelectedValue);
            #endregion

            #region Xóa hết tất cả role chọn
            //foreach (ListItem liIB in cblIB.Items)
            //{

            //    liIB.Selected = false;

            //}
            #endregion

            #region lay role mac dinh
            GetRoleDefault(tvIB, tvSMS, tvMB, tvPHO, tvWL);
            #endregion

            #region show popup chi tiet ve role


            //foreach (ListItem liIB in cblIB.Items)
            //{
            //    //onmouseover="<%=Resources.labels.pagenametip %>" onmouseout="UnTip()"
            //    //Tip('The name of page', LEFT, true, BGCOLOR, '#FF9900', FADEIN, 400)
            //    tblRoleDefault = (new SmartPortal.SEMS.Role().GetTranOfRole(liIB.Value, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
            //    //add vao thuoc tinh cua listitem
            //    string tooltip = "";
            //    foreach (DataRow row in tblRoleDefault.Rows)
            //    {
            //        tooltip += row["PAGENAME"].ToString() + "<br/>";
            //    }
            //    if (tblRoleDefault.Rows.Count != 0)
            //    {
            //        //add
            //        liIB.Attributes.Add("onmouseover", "Tip('" + tooltip + "', LEFT, true, BGCOLOR, '#FF9900', FADEIN, 400)");
            //        liIB.Attributes.Add("onmouseout", "UnTip()");
            //    }
            //}
            #endregion

            #endregion

            #region Người ủy quyền

            #region lay tat ca cac account cua khach hang

            DataTable dtAccountUyQuyen = new DataTable();
            dtAccountUyQuyen = ds.Tables[0];
            if (dtAccountUyQuyen.Rows.Count == 0)
            {
                lblError.Text = Resources.labels.khachhangnaykhongtontaitaikhoandedangky;
                return;
            }
            else
            {
                ddlSMSDefaultAcctnoUyQuyen.DataSource = dtAccountUyQuyen;
                ddlSMSDefaultAcctnoUyQuyen.DataTextField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoUyQuyen.DataValueField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoUyQuyen.DataBind();

                ddlAccountUyQuyen.DataSource = dtAccountUyQuyen;
                ddlAccountUyQuyen.DataTextField = "ACCOUNTNO";
                ddlAccountUyQuyen.DataValueField = "ACCOUNTNO";
                ddlAccountUyQuyen.DataBind();



                ddlAccUyQuyen.DataSource = dtAccountUyQuyen;
                ddlAccUyQuyen.DataTextField = "ACCOUNTNO";
                ddlAccUyQuyen.DataValueField = "ACCOUNTNO";
                ddlAccUyQuyen.DataBind();
                ddlAccUyQuyen.Items.Insert(0, new ListItem("ALL", "ALL"));
                //phongtt sms notification fee
                lsAccNo.Clear();
                foreach (DataRow dr in dtAccountUyQuyen.Rows)
                {
                    if (!lsAccNo.Contains(dr["ACCOUNTNO"].ToString()))
                    {
                        lsAccNo.Add(dr["ACCOUNTNO"].ToString());
                    }
                }

                //ddlAccountUyQuyen.Items.Insert(0, new ListItem("----------Chọn tài khoản----------", ""));

            }
            #endregion

            #region Hien thi tat cac cac role theo serviceid va usertype len cay
            //load for IB
            LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIBUyQuyen, ddlContractType.SelectedValue);

            //load for SMS
            LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSUyQuyen, ddlContractType.SelectedValue);

            //load for MB
            LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBUyQuyen, ddlContractType.SelectedValue);

            //load for PHO
            LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOUyQuyen, ddlContractType.SelectedValue);

            //load for Wallet
            LoadDataInTreeview(SmartPortal.Constant.IPC.EW, tvWLUyQuyen, ddlContractType.SelectedValue);
            #endregion

            #region lay role mac dinh
            GetRoleDefault(tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, tvWLUyQuyen);
            #endregion

            #endregion

            //an Panel
            pnCustInfo.Visible = false;
            pnPersonal.Visible = true;
            pnOption.Visible = false;
            pnContractInfo.Visible = false;


            //if(!CheckIsPhoneNumer(PHONECTK))
            //{
            //    lblError.Text = Resources.labels.phonenumberwrong;
            //    txtre
            //}
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
    void BindData()
    {
        DataSet dtCL = new DataSet();

        //dtCL = new SmartPortal.SEMS.Customer().GetCustomerByCondition(Utility.KillSqlInjection(txtCustCode.Text.Trim()), Utility.KillSqlInjection(txtFullName.Text.Trim()), Utility.KillSqlInjection(txtTel.Text.Trim()), Utility.KillSqlInjection(txtLicenseID.Text.Trim()), ddlCustType.SelectedValue, SmartPortal.Constant.IPC.ALL,"", ref IPCERRORCODE, ref IPCERRORDESC);
        dtCL = new SmartPortal.SEMS.Customer().GetCustomerByCondition(Utility.KillSqlInjection(txtCustCode.Text.Trim()), Utility.KillSqlInjection(txtFullName.Text.Trim()), Utility.KillSqlInjection(txtTel.Text.Trim()), Utility.KillSqlInjection(txtLicenseID.Text.Trim()), SmartPortal.Constant.IPC.ALL, SmartPortal.Constant.IPC.ALL, "", ref IPCERRORCODE, ref IPCERRORDESC);

        if (IPCERRORCODE != "0")
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "BindData", IPCERRORDESC, Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);

        }

        gvCustomerList.DataSource = dtCL;
        gvCustomerList.DataBind();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        BindData();

    }
    protected void gvCustomerList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RadioButton cbxSelect;
            HyperLink lblCustCode;
            Label lblCustName;
            Label lblPhone;
            Label lblIdentify;
            Label lblCustType;
            //Label lblStatus;


            DataRowView drv;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                drv = (DataRowView)e.Row.DataItem;


                cbxSelect = (RadioButton)e.Row.FindControl("cbxSelect");
                e.Row.Attributes.Add("onmousemove", "this.className='hightlight';");
                e.Row.Attributes.Add("onmouseout", "this.className='nohightlight';");


                lblCustCode = (HyperLink)e.Row.FindControl("lblCustCode");
                lblCustName = (Label)e.Row.FindControl("lblCustName");
                lblPhone = (Label)e.Row.FindControl("lblPhone");
                lblIdentify = (Label)e.Row.FindControl("lblIdentify");
                lblCustType = (Label)e.Row.FindControl("lblCustType");
                //lblStatus = (Label)e.Row.FindControl("lblStatus");



                //cbxSelect.Enabled = true;
                cbxSelect.Attributes.Add("onclick", "SelectRAD(this,'" + drv["CUSTID"].ToString() + "')");

                if (i == 0)
                {
                    cbxSelect.Checked = true;
                    hidCustID.Value = drv["CUSTID"].ToString();
                    i += 1;
                }
                lblCustCode.Text = drv["CUSTID"].ToString();
                //lblCustCode.NavigateUrl = SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=145&a=viewdetail&cid=" + drv["CUSTID"].ToString());
                lblCustName.Text = drv["FULLNAME"].ToString();
                lblPhone.Text = drv["TEL"].ToString();
                lblIdentify.Text = drv["LICENSEID"].ToString();

                switch (drv["CFTYPE"].ToString().Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        lblCustType.Text = Resources.labels.canhan;
                        break;
                    case SmartPortal.Constant.IPC.PERSONALLKG:
                        lblCustType.Text = Resources.labels.linkage;
                        break;
                    case SmartPortal.Constant.IPC.CORPORATE:
                        lblCustType.Text = Resources.labels.doanhnghiep;
                        break;
                }
                //switch (drv["STATUS"].ToString().Trim())
                //{
                //    case SmartPortal.Constant.IPC.NEW:
                //        lblStatus.Text = "Mới tạo";
                //        break;
                //    case SmartPortal.Constant.IPC.DELETE:
                //        lblStatus.Text = "Đã xóa";
                //        break;
                //    case SmartPortal.Constant.IPC.ACTIVE:
                //        lblStatus.Text = "Sử dụng";
                //        break;
                //}


            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "gvCustomerList_RowDataBound", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        try
        {
            pnOption.Visible = true;
            pnCustInfo.Visible = false;
            pnContractInfo.Visible = false;

        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "Button4_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            pnOption.Visible = false;
            pnCustInfo.Visible = true;
            pnContractInfo.Visible = false;

        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "Button1_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            pnOption.Visible = false;
            pnCustInfo.Visible = false;
            pnContractInfo.Visible = true;
            pnPersonal.Visible = false;

            SetRadio();

        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "Button2_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    void SendInfoLogin()
    {
        Antlr3.ST.StringTemplate tmpl = new Antlr3.ST.StringTemplate();
        tmpl = SmartPortal.Common.ST.GetStringTemplate("SEMSContractApprove", "ContractAttachment" + System.Globalization.CultureInfo.CurrentCulture.ToString());

        //lay thong tin hop dong de gui mail

        try
        {

            string hpcontractNo = txtContractNo.Text.Trim();
            string custID = "";
            tmpl.Reset();

            //lay thong tin hop dong boi contractno
            DataTable contractTable = (new SmartPortal.SEMS.Contract().GetContractByContractNo(hpcontractNo, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
            if (IPCERRORCODE != "0")
            {
                goto ERROR;
            }
            //gan thong tin hop dong vao stringtemplate
            if (contractTable.Rows.Count != 0)
            {
                tmpl.SetAttribute("CONTRACTNO", contractTable.Rows[0]["CONTRACTNO"].ToString());
                DataSet dsUserType = new DataSet();
                dsUserType = new SmartPortal.SEMS.Services().GetAllUserType(contractTable.Rows[0]["USERTYPE"].ToString().Trim(), "", ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE == "0")
                {
                    DataTable dtUserType = new DataTable();
                    dtUserType = dsUserType.Tables[0];

                    if (dtUserType.Rows.Count != 0)
                    {
                        tmpl.SetAttribute("CONTRACTTYPE", dtUserType.Rows[0]["TYPENAME"].ToString());
                    }
                }
                else
                {
                    throw new SmartPortal.ExceptionCollection.IPCException(IPCERRORDESC);
                }


                tmpl.SetAttribute("OPENDATE", SmartPortal.Common.Utilities.Utility.IsDateTime2(contractTable.Rows[0]["CREATEDATE"].ToString()).ToString("dd/MM/yyyy"));
                tmpl.SetAttribute("ENDDATE", SmartPortal.Common.Utilities.Utility.IsDateTime2(contractTable.Rows[0]["ENDDATE"].ToString()).ToString("dd/MM/yyyy"));


                tmpl.SetAttribute("PRODUCT", contractTable.Rows[0]["PRODUCTNAME"].ToString());


                custID = contractTable.Rows[0]["CUSTID"].ToString();
            }

            //lay thong tin khach hang
            DataTable custTable = (new SmartPortal.SEMS.Customer().GetCustomerByCustcode(custID, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
            if (IPCERRORCODE != "0")
            {
                goto ERROR;
            }
            //gan thong tin hop dong vao stringtemplate
            if (custTable.Rows.Count != 0)
            {
                tmpl.SetAttribute("CUSTID", custTable.Rows[0]["CUSTID"].ToString());
                tmpl.SetAttribute("FULLNAME", custTable.Rows[0]["FULLNAME"].ToString());
                switch (custTable.Rows[0]["CFTYPE"].ToString().Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        tmpl.SetAttribute("CUSTTYPE", Resources.labels.canhan);
                        break;
                    case SmartPortal.Constant.IPC.CORPORATE:
                        tmpl.SetAttribute("CUSTTYPE", Resources.labels.doanhnghiep);
                        break;
                }
                tmpl.SetAttribute("PHONE", custTable.Rows[0]["TEL"].ToString());
                tmpl.SetAttribute("EMAIL", custTable.Rows[0]["EMAIL"].ToString());
                tmpl.SetAttribute("CIF", custTable.Rows[0]["CUSTCODE"].ToString());
            }

            #region lay thong tin tai khoan cua chu tai khoan
            DataTable userTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.PCO, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
            if (IPCERRORCODE != "0")
            {
                goto ERROR;
            }

            StringBuilder st = new StringBuilder();
            st.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinchutaikhoan.ToUpper() + "</div>");
            //gan thong tin user vao stringtemplate
            int i = 0;
            foreach (DataRow row in userTable.Rows)
            {

                st.Append("<table style='width:100%;'>");


                st.Append("<tr>");
                st.Append("<td width='25%'>");
                st.Append(Resources.labels.tendaydu + " ");
                st.Append("</td>");
                st.Append("<td width='25%'>");
                st.Append(row["FULLNAME"].ToString());
                st.Append("</td>");
                st.Append("<td width='25%'>");
                st.Append("Email ");
                st.Append("</td>");
                st.Append("<td width='25%'>");
                st.Append(row["EMAIL"].ToString());
                st.Append("</td>");
                st.Append("</tr>");

                st.Append("<tr>");
                st.Append("<td>");
                st.Append(Resources.labels.dienthoai + " ");
                st.Append("</td>");
                st.Append("<td>");
                st.Append(row["PHONE"].ToString());
                st.Append("</td>");
                st.Append("<td>");
                st.Append("");
                st.Append("</td>");
                st.Append("<td>");
                st.Append("");
                st.Append("</td>");
                st.Append("</tr>");


                //lay het các tai khoan Ibank cua user theo userID
                DataSet accountIBDataset = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    goto ERROR;
                }

                DataTable accountIBTable = accountIBDataset.Tables[0];
                if (accountIBTable.Rows.Count != 0)
                {
                    if (accountIBTable.Rows[0]["ROLEID"].ToString().Trim() != "")
                    {
                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<br/>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<B>" + Resources.labels.internetbanking + "</B>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.tendangnhap + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountIBTable.Rows[0]["USERNAME"].ToString());
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.matkhau + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append("########");
                        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                        st.Append("</td>");
                        st.Append("</tr>");
                    }
                }

                //lay het các tai khoan SMS cua user theo userID
                DataSet accountSMSDataset = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    goto ERROR;
                }

                DataTable accountSMSTable = accountSMSDataset.Tables[0];
                if (accountSMSTable.Rows.Count != 0)
                {
                    if (accountSMSTable.Rows[0]["ROLEID"].ToString().Trim() != "")
                    {
                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<br/>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<B>" + Resources.labels.smsbanking + "</B>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.sodienthoai + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountSMSTable.Rows[0]["UN"].ToString());
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.taikhoanmacdinh + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountSMSTable.Rows[0]["DEFAULTACCTNO"].ToString());
                        st.Append("</td>");
                        st.Append("</tr>");
                    }
                }

                //lay het các tai khoan MB cua user theo userID
                DataSet accountMBDataset = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    goto ERROR;
                }

                DataTable accountMBTable = accountMBDataset.Tables[0];
                if (accountMBTable.Rows.Count != 0)
                {
                    if (accountMBTable.Rows[0]["ROLEID"].ToString().Trim() != "")
                    {
                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<br/>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<B>" + Resources.labels.mobilebanking + "</B>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.tendangnhap + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountMBTable.Rows[0]["USERNAME"].ToString());
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.matkhau + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append("########");
                        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                        st.Append("</td>");
                        st.Append("</tr>");


                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.phone + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountMBTable.Rows[0]["UN"].ToString());
                        st.Append("</td>");
                        st.Append("</tr>");

                    }

                }

                //lay het các tai khoan WL cua user theo userID
                DataSet accountWLDataset = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), SmartPortal.Constant.IPC.WAL, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    goto ERROR;
                }

                DataTable accountWLTable = accountWLDataset.Tables[0];
                if (accountWLTable.Rows.Count != 0)
                {
                    if (accountWLTable.Rows[0]["ROLEID"].ToString().Trim() != "")
                    {
                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<br/>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<B>" + Resources.labels.walletbanking + "</B>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.phone + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountWLTable.Rows[0]["UN"].ToString());
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.matkhau + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append("########");
                        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                        st.Append("</td>");
                        st.Append("</tr>");
                    }

                }

                //lay het các tai khoan PHO cua user theo userID
                DataSet accountPHODataset = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    goto ERROR;
                }

                DataTable accountPHOTable = accountPHODataset.Tables[0];
                if (accountPHOTable.Rows.Count != 0)
                {
                    if (accountPHOTable.Rows[0]["ROLEID"].ToString().Trim() != "")
                    {
                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<br/>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td colspan='4'>");
                        st.Append("<B>" + Resources.labels.phonebanking + "</B>");
                        st.Append("</td>");
                        st.Append("</tr>");

                        st.Append("<tr>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.tendangnhap + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(accountPHOTable.Rows[0]["UN"].ToString());
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append(Resources.labels.matkhau + " :");
                        st.Append("</td>");
                        st.Append("<td width='25%'>");
                        st.Append("########");
                        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                        st.Append("</td>");
                        st.Append("</tr>");
                    }
                }

                st.Append("</table>");
                //i += 1;
                //if (i == userTable.Rows.Count)
                //{
                //    st.Append("<hr/>");
                //}
            }
            tmpl.SetAttribute("USERINFO", st.ToString());

            #endregion

            if (ViewState["NGUOIUYQUYEN"] != null)
            {
                DataTable tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                if (tblNGUOIUYQUYEN.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable nuyTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.RP, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stNUY = new StringBuilder();
                    stNUY.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoidongsohuu.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in nuyTable.Rows)
                    {

                        stNUY.Append("<table style='width:100%;'>");


                        stNUY.Append("<tr>");
                        stNUY.Append("<td width='25%'>");
                        stNUY.Append(Resources.labels.tendaydu + " ");
                        stNUY.Append("</td>");
                        stNUY.Append("<td width='25%'>");
                        stNUY.Append(row["FULLNAME"].ToString());
                        stNUY.Append("</td>");
                        stNUY.Append("<td width='25%'>");
                        stNUY.Append("Email ");
                        stNUY.Append("</td>");
                        stNUY.Append("<td width='25%'>");
                        stNUY.Append(row["EMAIL"].ToString());
                        stNUY.Append("</td>");
                        stNUY.Append("</tr>");

                        stNUY.Append("<tr>");
                        stNUY.Append("<td>");
                        stNUY.Append(Resources.labels.dienthoai + " ");
                        stNUY.Append("</td>");
                        stNUY.Append("<td>");
                        stNUY.Append(row["PHONE"].ToString());
                        stNUY.Append("</td>");
                        stNUY.Append("<td>");
                        stNUY.Append("");
                        stNUY.Append("</td>");
                        stNUY.Append("<td>");
                        stNUY.Append("");
                        stNUY.Append("</td>");
                        stNUY.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetNUY = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTableNUY = accountIBDatasetNUY.Tables[0];
                        if (accountIBTableNUY.Rows.Count != 0)
                        {
                            if (accountIBTableNUY.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<br/>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<B>Internet Banking</B>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.tendangnhap + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(accountIBTableNUY.Rows[0]["USERNAME"].ToString());
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.matkhau + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetNUY = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTableNUY = accountSMSDatasetNUY.Tables[0];
                        if (accountSMSTableNUY.Rows.Count != 0)
                        {
                            if (accountSMSTableNUY.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<br/>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<B>SMS Banking</B>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.sodienthoai + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(accountSMSTableNUY.Rows[0]["UN"].ToString());
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.taikhoanmacdinh + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(accountSMSTableNUY.Rows[0]["DEFAULTACCTNO"].ToString());
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        //DataSet accountMBDatasetNUY = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), SmartPortal.Constant.IPC.NORMAL, ref IPCERRORCODE, ref IPCERRORDESC);

                        //if (IPCERRORCODE != "0")
                        //{
                        //    goto ERROR;
                        //}

                        //DataTable accountMBTableNUY = accountMBDatasetNUY.Tables[0];
                        //if (accountMBTableNUY.Rows.Count != 0)
                        //{
                        //    if (accountMBTableNUY.Rows[0]["ROLEID"].ToString().Trim() != "")
                        //    {
                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td colspan='4'>");
                        //        stNUY.Append("<br/>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");

                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td colspan='4'>");
                        //        stNUY.Append("<B>Mobile Banking</B>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");

                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(Resources.labels.username + " :");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(accountMBTableNUY.Rows[0]["USERNAME"].ToString());
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(Resources.labels.matkhau + " :");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append("########");
                        //        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");


                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(Resources.labels.phone + " :");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(accountMBTableNUY.Rows[0]["UN"].ToString());
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");
                        //    }

                        //}

                        //lay het các tai khoan WL cua user theo userID
                        //DataSet accountWLDatasetNUY = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), SmartPortal.Constant.IPC.WAL, ref IPCERRORCODE, ref IPCERRORDESC);

                        //if (IPCERRORCODE != "0")
                        //{
                        //    goto ERROR;
                        //}

                        //DataTable accountWLTableNUY = accountWLDatasetNUY.Tables[0];
                        //if (accountWLTableNUY.Rows.Count != 0)
                        //{
                        //    if (accountWLTableNUY.Rows[0]["ROLEID"].ToString().Trim() != "")
                        //    {
                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td colspan='4'>");
                        //        stNUY.Append("<br/>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");

                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td colspan='4'>");
                        //        stNUY.Append("<B>Wallet Banking</B>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");

                        //        stNUY.Append("<tr>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(Resources.labels.sodienthoai + " :");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(accountWLTableNUY.Rows[0]["UN"].ToString());
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append(Resources.labels.matkhau + " :");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("<td width='25%'>");
                        //        stNUY.Append("########");
                        //        //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                        //        stNUY.Append("</td>");
                        //        stNUY.Append("</tr>");
                        //    }

                        //}

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetNUY = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTableNUY = accountPHODatasetNUY.Tables[0];
                        if (accountPHOTableNUY.Rows.Count != 0)
                        {
                            if (accountPHOTableNUY.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<br/>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td colspan='4'>");
                                stNUY.Append("<B>Phone Banking</B>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");

                                stNUY.Append("<tr>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.sodienthoai + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(accountPHOTableNUY.Rows[0]["UN"].ToString());
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append(Resources.labels.matkhau + " :");
                                stNUY.Append("</td>");
                                stNUY.Append("<td width='25%'>");
                                stNUY.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stNUY.Append("</td>");
                                stNUY.Append("</tr>");
                            }
                        }

                        stNUY.Append("</table>");
                        j += 1;
                        if (j < nuyTable.Rows.Count)
                        {
                            stNUY.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("NGUOIUYQUYEN", stNUY.ToString());

                    #endregion

                }
            }

            //luu thong tin vao session de hien thi cho nguoi dung
            Session["tmpl"] = tmpl.ToString();

            //luu thong tin ban cung hop dong
            try
            {
                new SmartPortal.SEMS.Contract().SaveContractReview(hpcontractNo, tmpl.ToString(), Server.MapPath("~/widgets/semscontractlist/contractfile/" + hpcontractNo + ".html"));
            }
            catch
            {
            }

            goto EXIT;
        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractApprove_Widget", "SendInfoLogin", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractApprove_Widget", "SendInfoLogin", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);

        }
    ERROR:
        SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractApprove_Widget", "SendInfoLogin", IPCERRORDESC, Request.Url.Query);
        SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
    EXIT:

        ;
    }

    protected void btnThemChuTaiKhoan_Click(object sender, EventArgs e)
    {
        //nasichutaikhoan
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;
            int passlenWL = 0;


            #region check sms notify vutt 30032016
            string errDesc = string.Empty;
            if (!string.IsNullOrEmpty(txtSMSPhoneNo.Text))
            {
                //if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMS, ref errDesc, radAllAccount.Checked ? "" : ddlAccount.SelectedValue, new List<DataTable> { (DataTable)ViewState["NGUOIUYQUYEN"] }))
                //phongtt sms notification fee
                if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMS, ref errDesc, radAllAccount.Checked ? "" : ddlAccount.SelectedValue, lsAccNo, new List<DataTable> { (DataTable)ViewState["NGUOIUYQUYEN"] }))
                {
                    lblAlert.Text = errDesc;
                    return;
                }
            }
            #endregion

            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            if (RbUserDefault.Checked)
            {
                userName = txtMBUsersName.Text.Trim();
            }
            else if (RbChangeUserName.Checked)
            {
                userName = txtMBUsersName.Text.Trim();
                #region check username

                if (userName == string.Empty)
                {
                    lblAlert.Text = Resources.labels.bancannhaptendangnhap;
                    return;
                }
                else if (userName.Length < minlength || userName.Length > maxlength)
                {
                    lblAlert.Text = string.Format(Resources.labels.usernamemustbetween, minlength, maxlength);
                    return;
                }

                DataSet ds = new Customer().CheckUserName("EBA_Users_CheckUserName", new object[] { userName }, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    lblAlert.Text = IPCERRORDESC;
                    return;
                }

                if (!validateusername(txtIBTypeUserName))
                {
                    return;
                }

                #endregion
            }
            if (userName == string.Empty || userName == null)
            {
                lblAlert.Text = Resources.labels.bancannhaptendangnhap;
                return;
            }
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB'";
            if (ddlpolicyIB.SelectedValue.ToString() != "")
            {
                filterIB += " and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            }
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMS.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMB.SelectedValue.ToString() + "'";
            string filterWL = "serviceid='WL' and policyid='" + ddlpolicyWL.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dt = dspolicy.Tables[0];
                DataTable dtIB = dt.Select(filterIB, stSort).Any() ? dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable() : null;
                DataTable dtSMS = dt.Select(filterSMS, stSort).Any() ? dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable() : null;
                DataTable dtMB = dt.Select(filterMB, stSort).Any() ? dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable() : null;
                DataTable dtWL = dt.Select(filterWL, stSort).Any() ? dspolicy.Tables[0].Select(filterWL, stSort).CopyToDataTable() : null;


                passlenIB = dtIB == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = dtSMS == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = dtMB == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
                passlenWL = dtWL == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtWL.Rows[0]["minpwdlen"].ToString());


            }


            //1.4.2016 fix userid khac phoneno 
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {
                //txtMBUsersName.Text = ownerUserName;
                txtMBUsersName.Text = userName;

            }
            else
            {

            }

            if (ddlAccountMB.SelectedValue != "ALL")
            {
                // PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUsersName.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, ddlAccount.SelectedValue, ddlCTKDefaultAcctno2.SelectedValue);
                //24.2.2016 minh add encrypt pass new
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);

                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, tvWL, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, PHONECTK, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, ddlAccountMB.SelectedValue, ddlCTKDefaultAcctno2.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), "", PassTemp);
            }

            if (ddlAccountMB.SelectedValue == "ALL")
            {
                //lay tat ca tai khoan khach hang
                string cc = "";
                string ct = "";

                DataSet dsCust = new SmartPortal.SEMS.Customer().GetCustomerByCustcode(hidCustID.Value, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                else
                {
                    DataTable tblCust = dsCust.Tables[0];
                    if (tblCust.Rows.Count != 0)
                    {
                        cc = tblCust.Rows[0]["CFCODE"].ToString().Trim();
                        ct = tblCust.Rows[0]["CFTYPE"].ToString().Trim();
                    }
                }

                DataSet ds = new DataSet();
                ds = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ct, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtAccount = new DataTable();
                dtAccount = ds.Tables[0];
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                //luu tat ca account
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUsersName.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno2.SelectedValue);
                    // LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUsersName.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno2.SelectedValue, pwdreset);
                    LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, tvWL, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, PHONECTK, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno2.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), "", PassTemp);
                }


            }
            if (TabCustomerInfoHelper.TabWalletVisibility == 1)
            {
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyen("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, tvWL, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, userName, PassTemp, "", ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, "", PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNo.Text, PassTemp, PHONECTK, ddlCTKDefaultAcctno2.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), PHONECTK, PassTemp);
            }

            lblAlert.Text = Resources.labels.recordsaved;
            #endregion




            //11/9/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultChuTaiKhoan.Rows.Count == 0)
            {
                lblAlert.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnThemChuTaiKhoan_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "btnThemChuTaiKhoan_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    void LuuThongTinQuyen(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, TreeView tvWLRole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string SMSIsDefault, string SMSPincode, string MBPhone, string MBPass, string MBPincode, string PHOPhone, string PHOPass, string Account, string PHODfAcctno, string pwdreset, string WLPinCode, string WLPhone, string WLPass)
    {
        fullName = fullName.Replace("'", "''");
        if (ViewState[sessionName] == null)
        {
            #region NULL
            DataTable tblNguoiUyQuyen = new DataTable();

            DataColumn colFullName = new DataColumn("colFullName");
            DataColumn colLevel = new DataColumn("colLevel");
            DataColumn colBirthday = new DataColumn("colBirthday");
            DataColumn colGender = new DataColumn("colGender");
            DataColumn colPhone = new DataColumn("colPhone");
            DataColumn colEmail = new DataColumn("colEmail");
            DataColumn colAddress = new DataColumn("colAddress");
            DataColumn colIBUserName = new DataColumn("colIBUserName");
            DataColumn colIBPass = new DataColumn("colIBPass");
            DataColumn colSMSPhone = new DataColumn("colSMSPhone");
            DataColumn colSMSDefaultAcctno = new DataColumn("colSMSDefaultAcctno");
            DataColumn colSMSDefaultLang = new DataColumn("colSMSDefaultLang");
            DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault"); // moi them
            DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");// moi them
            DataColumn colMBPhone = new DataColumn("colMBPhone");
            DataColumn colMBPass = new DataColumn("colMBPass");
            DataColumn colMBPinCode = new DataColumn("colMBPinCode");// moi them
            DataColumn colWLPhone = new DataColumn("colWLPhone");
            DataColumn colWLPass = new DataColumn("colWLPass");
            DataColumn colWLPinCode = new DataColumn("colWLPinCode");
            DataColumn colPHOPhone = new DataColumn("colPHOPhone");
            DataColumn colPHOPass = new DataColumn("colPHOPass");
            DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");
            DataColumn colAccount = new DataColumn("colAccount");
            DataColumn colRole = new DataColumn("colRole");
            DataColumn colRoleID = new DataColumn("colRoleID");
            DataColumn colTranCode = new DataColumn("colTranCode");
            DataColumn colTranCodeID = new DataColumn("colTranCodeID");
            DataColumn colServiceID = new DataColumn("colServiceID");
            DataColumn colIBPolicy = new DataColumn("colIBPolicy");
            DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
            DataColumn colMBPolicy = new DataColumn("colMBPolicy");
            DataColumn colWLPolicy = new DataColumn("colWLPolicy");
            DataColumn colpwdreset = new DataColumn("colpwdreset");


            tblNguoiUyQuyen.Columns.Add(colFullName);
            tblNguoiUyQuyen.Columns.Add(colLevel);
            tblNguoiUyQuyen.Columns.Add(colBirthday);
            tblNguoiUyQuyen.Columns.Add(colGender);
            tblNguoiUyQuyen.Columns.Add(colPhone);
            tblNguoiUyQuyen.Columns.Add(colEmail);
            tblNguoiUyQuyen.Columns.Add(colAddress);
            tblNguoiUyQuyen.Columns.Add(colIBUserName);
            tblNguoiUyQuyen.Columns.Add(colIBPass);
            tblNguoiUyQuyen.Columns.Add(colSMSPhone);
            tblNguoiUyQuyen.Columns.Add(colSMSDefaultAcctno);
            tblNguoiUyQuyen.Columns.Add(colSMSDefaultLang);
            tblNguoiUyQuyen.Columns.Add(colSMSIsDefault);
            tblNguoiUyQuyen.Columns.Add(colSMSPinCode);
            tblNguoiUyQuyen.Columns.Add(colMBPhone);
            tblNguoiUyQuyen.Columns.Add(colMBPass);
            tblNguoiUyQuyen.Columns.Add(colMBPinCode);
            tblNguoiUyQuyen.Columns.Add(colWLPhone);
            tblNguoiUyQuyen.Columns.Add(colWLPass);
            tblNguoiUyQuyen.Columns.Add(colWLPinCode);
            tblNguoiUyQuyen.Columns.Add(colPHOPhone);
            tblNguoiUyQuyen.Columns.Add(colPHOPass);
            tblNguoiUyQuyen.Columns.Add(colPHODefaultAcctno);
            tblNguoiUyQuyen.Columns.Add(colAccount);
            tblNguoiUyQuyen.Columns.Add(colRole);
            tblNguoiUyQuyen.Columns.Add(colRoleID);
            tblNguoiUyQuyen.Columns.Add(colTranCode);
            tblNguoiUyQuyen.Columns.Add(colTranCodeID);
            tblNguoiUyQuyen.Columns.Add(colServiceID);
            tblNguoiUyQuyen.Columns.Add(colIBPolicy);
            tblNguoiUyQuyen.Columns.Add(colSMSPolicy);
            tblNguoiUyQuyen.Columns.Add(colMBPolicy);
            tblNguoiUyQuyen.Columns.Add(colWLPolicy);
            tblNguoiUyQuyen.Columns.Add(colpwdreset);

            //IB
            if (IBUserName != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu quyen IB null
                            DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                            rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                            rowNguoiUyQuyen["colLevel"] = level.Trim();
                            rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                            rowNguoiUyQuyen["colGender"] = gender.Trim();
                            rowNguoiUyQuyen["colPhone"] = phone.Trim();
                            rowNguoiUyQuyen["colEmail"] = email.Trim();
                            rowNguoiUyQuyen["colAddress"] = address.Trim();
                            rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                            rowNguoiUyQuyen["colIBPass"] = IBPass;
                            rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                            rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                            rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                    break;
                            }
                            rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                            //}
                            //else
                            //{
                            //}
                        }
                    }
                    else
                    {
                    }
                }
            }

            //SMS
            if (SMSPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvSMSRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            #region luu quyen SMS null
                            DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                            rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                            rowNguoiUyQuyen["colLevel"] = level.Trim();
                            rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                            rowNguoiUyQuyen["colGender"] = gender.Trim();
                            rowNguoiUyQuyen["colPhone"] = phone.Trim();
                            rowNguoiUyQuyen["colEmail"] = email.Trim();
                            rowNguoiUyQuyen["colAddress"] = address.Trim();
                            rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                            rowNguoiUyQuyen["colIBPass"] = IBPass;
                            rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                            rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                            rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colWLPinCode"] = WLPhone;
                            rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                            rowNguoiUyQuyen["colWLPass"] = WLPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                    break;
                            }
                            rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                        }
                    }
                    else
                    {
                    }
                }
            }

            //MB
            if (MBPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvMBRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu quyen Mobile null
                            DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                            rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                            rowNguoiUyQuyen["colLevel"] = level.Trim();
                            rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                            rowNguoiUyQuyen["colGender"] = gender.Trim();
                            rowNguoiUyQuyen["colPhone"] = phone.Trim();
                            rowNguoiUyQuyen["colEmail"] = email.Trim();
                            rowNguoiUyQuyen["colAddress"] = address.Trim();
                            rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                            rowNguoiUyQuyen["colIBPass"] = IBPass;
                            rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                            rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                            rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colWLPinCode"] = WLPinCode;
                            rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                            rowNguoiUyQuyen["colWLPass"] = WLPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                    break;
                            }
                            rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                            //}
                            //else
                            //{
                            //}
                        }
                    }
                    else
                    {
                    }
                }
            }

            //PHO
            if (PHOPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvPHORole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu quyen Phone khi null
                            DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                            rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                            rowNguoiUyQuyen["colLevel"] = level.Trim();
                            rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                            rowNguoiUyQuyen["colGender"] = gender.Trim();
                            rowNguoiUyQuyen["colPhone"] = phone.Trim();
                            rowNguoiUyQuyen["colEmail"] = email.Trim();
                            rowNguoiUyQuyen["colAddress"] = address.Trim();
                            rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                            rowNguoiUyQuyen["colIBPass"] = IBPass;
                            rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                            rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                            rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                            rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                            //}
                            //else
                            //{
                            //}
                        }
                    }
                    else
                    {
                    }
                }
            }


            #region them giao dich WL
            //PHO
            if (WLPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvWLRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {

                            DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                            rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                            rowNguoiUyQuyen["colLevel"] = level.Trim();
                            rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                            rowNguoiUyQuyen["colGender"] = gender.Trim();
                            rowNguoiUyQuyen["colPhone"] = phone.Trim();
                            rowNguoiUyQuyen["colEmail"] = email.Trim();
                            rowNguoiUyQuyen["colAddress"] = address.Trim();
                            rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                            rowNguoiUyQuyen["colIBPass"] = IBPass;
                            rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                            rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                            rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colWLPinCode"] = WLPinCode;
                            rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                            rowNguoiUyQuyen["colWLPass"] = WLPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.EW;
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                    break;
                            }
                            rowNguoiUyQuyen["colpwdreset"] = pwdreset;
                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);

                        }
                    }
                    else
                    {
                    }
                }
            }
            #endregion
            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
            #endregion
        }
        else
        {
            DataTable tblNguoiUyQuyen = (DataTable)ViewState[sessionName];
            ////minh add 26/8/2015 de fix truong hop ban dau ko co email+sms , sau do nhap them email

            if (tblNguoiUyQuyen.Rows.Count != 0)
                foreach (DataRow r in tblNguoiUyQuyen.Rows)
                {
                    if (r["colEmail"].ToString() != email)
                    {
                        r["colEmail"] = email;
                    }
                }

            //IB
            if (IBUserName != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {
                                #region luu quyen IB khac null
                                DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                                rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                                rowNguoiUyQuyen["colLevel"] = level.Trim();
                                rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                                rowNguoiUyQuyen["colGender"] = gender.Trim();
                                rowNguoiUyQuyen["colPhone"] = phone.Trim();
                                rowNguoiUyQuyen["colEmail"] = email.Trim();
                                rowNguoiUyQuyen["colAddress"] = address.Trim();
                                rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                                rowNguoiUyQuyen["colIBPass"] = IBPass;
                                rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                                rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                                rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                        break;
                                }
                                rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                                tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                                #endregion
                            }

                            //}
                            //else
                            //{
                            //    if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                            //    {
                            //        foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                            //        {
                            //            tblNguoiUyQuyen.Rows.Remove(r);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {

                            }

                            //}
                            else
                            {
                                if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                                {
                                    foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                                    {
                                        tblNguoiUyQuyen.Rows.Remove(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //SMS
            if (SMSPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvSMSRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {
                                #region luu quyen SMS khac null
                                DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                                rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                                rowNguoiUyQuyen["colLevel"] = level.Trim();
                                rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                                rowNguoiUyQuyen["colGender"] = gender.Trim();
                                rowNguoiUyQuyen["colPhone"] = phone.Trim();
                                rowNguoiUyQuyen["colEmail"] = email.Trim();
                                rowNguoiUyQuyen["colAddress"] = address.Trim();
                                rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                                rowNguoiUyQuyen["colIBPass"] = IBPass;
                                rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                                rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                                rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//

                                rowNguoiUyQuyen["colWLPinCode"] = WLPinCode;
                                rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                                rowNguoiUyQuyen["colWLPass"] = WLPass;

                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                        break;
                                }
                                rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                                tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                                #endregion
                            }

                            //}
                            //else
                            //{
                            //    if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                            //    {
                            //        foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                            //        {
                            //            tblNguoiUyQuyen.Rows.Remove(r);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {

                            }

                            //}
                            else
                            {
                                if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                                {
                                    foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                                    {
                                        tblNguoiUyQuyen.Rows.Remove(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //MB
            if (MBPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvMBRole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {
                                #region luu quyen MB khac null
                                DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                                rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                                rowNguoiUyQuyen["colLevel"] = level.Trim();
                                rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                                rowNguoiUyQuyen["colGender"] = gender.Trim();
                                rowNguoiUyQuyen["colPhone"] = phone.Trim();
                                rowNguoiUyQuyen["colEmail"] = email.Trim();
                                rowNguoiUyQuyen["colAddress"] = address.Trim();
                                rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                                rowNguoiUyQuyen["colIBPass"] = IBPass;
                                rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                                rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                                rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                                rowNguoiUyQuyen["colWLPinCode"] = WLPinCode;
                                rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                                rowNguoiUyQuyen["colWLPass"] = WLPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                        break;
                                }
                                rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                                tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                                #endregion
                            }

                            //}
                            //else
                            //{
                            //    if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                            //    {
                            //        foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                            //        {
                            //            tblNguoiUyQuyen.Rows.Remove(r);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {

                            }

                            //}
                            else
                            {
                                if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                                {
                                    foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                                    {
                                        tblNguoiUyQuyen.Rows.Remove(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //PHO
            if (PHOPhone != "")
            {
                foreach (TreeNode nodeRoleIBNguoiUyQuyen in tvPHORole.Nodes)
                {
                    if (nodeRoleIBNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {
                                #region luu quyen PHO khac null
                                DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                                rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                                rowNguoiUyQuyen["colLevel"] = level.Trim();
                                rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                                rowNguoiUyQuyen["colGender"] = gender.Trim();
                                rowNguoiUyQuyen["colPhone"] = phone.Trim();
                                rowNguoiUyQuyen["colEmail"] = email.Trim();
                                rowNguoiUyQuyen["colAddress"] = address.Trim();
                                rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                                rowNguoiUyQuyen["colIBPass"] = IBPass;
                                rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                                rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                                rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault.Trim();//
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;   //
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPincode;//
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                                tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                                #endregion
                            }

                            //}
                            //else
                            //{
                            //    if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                            //    {
                            //        foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                            //        {
                            //            tblNguoiUyQuyen.Rows.Remove(r);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleIBNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {

                            }

                            //}
                            else
                            {
                                if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                                {
                                    foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                                    {
                                        tblNguoiUyQuyen.Rows.Remove(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            #region them giao dich Wl truong hop khac NULL
            //MB
            if (WLPhone != "")
            {
                foreach (TreeNode nodeRoleWLNguoiUyQuyen in tvWLRole.Nodes)
                {
                    if (nodeRoleWLNguoiUyQuyen.Checked)
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleWLNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName.Replace("'", "''") + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleWLNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {
                                #region chi tiet
                                DataRow rowNguoiUyQuyen = tblNguoiUyQuyen.NewRow();
                                rowNguoiUyQuyen["colFullName"] = fullName.Trim();
                                rowNguoiUyQuyen["colLevel"] = level.Trim();
                                rowNguoiUyQuyen["colBirthday"] = birthday.Trim();
                                rowNguoiUyQuyen["colGender"] = gender.Trim();
                                rowNguoiUyQuyen["colPhone"] = phone.Trim();
                                rowNguoiUyQuyen["colEmail"] = email.Trim();
                                rowNguoiUyQuyen["colAddress"] = address.Trim();
                                rowNguoiUyQuyen["colIBUserName"] = IBUserName.Trim();
                                rowNguoiUyQuyen["colIBPass"] = IBPass;
                                rowNguoiUyQuyen["colSMSPhone"] = SMSPhone.Trim();
                                rowNguoiUyQuyen["colSMSDefaultAcctno"] = SMSDefaultAcctno.Trim();
                                rowNguoiUyQuyen["colSMSDefaultLang"] = SMSDefaultLang.Trim();
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPincode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colWLPinCode"] = WLPinCode;
                                rowNguoiUyQuyen["colWLPhone"] = WLPhone.Trim();
                                rowNguoiUyQuyen["colWLPass"] = WLPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODfAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleWLNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleWLNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.EW;
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWL.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBco.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colWLPolicy"] = ddlpolicyWLco.SelectedValue.ToString();
                                        break;
                                }

                                rowNguoiUyQuyen["colpwdreset"] = pwdreset;
                                tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                                #endregion
                            }

                            //}
                            //else
                            //{
                            //    if (tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                            //    {
                            //        foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName + "' and colRoleID='" + nodeRoleIBNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                            //        {
                            //            tblNguoiUyQuyen.Rows.Remove(r);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodeTrancodeIBNguoiUyQuyen in nodeRoleWLNguoiUyQuyen.ChildNodes)
                        {
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            if (tblNguoiUyQuyen.Select("colFullName='" + fullName.Replace("'", "''") + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleWLNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length == 0)
                            {

                            }

                            //}
                            else
                            {
                                if (tblNguoiUyQuyen.Select("colFullName='" + fullName.Replace("'", "''") + "' and colAccount='" + Account + "' and colRoleID='" + nodeRoleWLNguoiUyQuyen.Value + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'").Length != 0)
                                {
                                    foreach (DataRow r in tblNguoiUyQuyen.Select("colFullName='" + fullName.Replace("'", "''") + "' and colRoleID='" + nodeRoleWLNguoiUyQuyen.Value + "' and colAccount='" + Account + "' and colTranCode='" + nodeTrancodeIBNguoiUyQuyen.Text + "'"))
                                    {
                                        tblNguoiUyQuyen.Rows.Remove(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion


            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
        }
    }

    protected void btnThemNguoiUyQuyen_Click(object sender, EventArgs e)
    {
        //nasinguoiuyquyen
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;
            int passlenWL = 0;


            if (!CheckExistPhoneNumber(txtPhoneNguoiUyQuyen.Text))
            {
                lblAlertDSH.Text = Resources.labels.phonenumberassigned;
                return;
            }
            else
            {
                PHONENUQ = txtPhoneNguoiUyQuyen.Text.Trim().ToString();
            }
            #region check sms notify vutt 30032016
            string errDesc = string.Empty;
            if (!string.IsNullOrEmpty(txtSMSPhoneNguoiUyQuyen.Text))
            {
                //if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMSUyQuyen, ref errDesc, radAllAccountNguoiUyQuyen.Checked ? "" : ddlAccountUyQuyen.SelectedValue, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOAN"] }))
                //phongtt sms notification fee
                if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMSUyQuyen, ref errDesc, radAllAccountNguoiUyQuyen.Checked ? "" : ddlAccountUyQuyen.SelectedValue, lsAccNo, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOAN"] }))
                {
                    lblAlertDSH.Text = errDesc;
                    return;
                }
            }

            #endregion

            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            if (rbMBGenerateNguoiUyQuyen.Checked)
            {
                usernameAuthorized = txtMBUserNguoiUyQuyen.Text.Trim();
            }
            else if (rbMBTypeNguoiUyQuyen.Checked)
            {
                usernameAuthorized = txtMBGenUserNameNguoiUyQuyen.Text.Trim();
                #region check usernameAuthorized

                if (usernameAuthorized == string.Empty)
                {
                    lblAlertDSH.Text = Resources.labels.bancannhaptendangnhap;
                    return;
                }
                else if (usernameAuthorized.Length < minlength || usernameAuthorized.Length > maxlength)
                {
                    lblAlertDSH.Text = string.Format(Resources.labels.usernamemustbetween, minlength, maxlength);
                }

                DataSet ds = new Customer().CheckUserName("EBA_Users_CheckUserName", new object[] { usernameAuthorized }, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    lblAlertDSH.Text = IPCERRORDESC;
                    return;
                }
                if (!validateusername(txtIBTypeUserNameNguoiUyQuyen))
                {
                    return;
                }

                #endregion
            }
            if (usernameAuthorized == string.Empty || usernameAuthorized == null)
            {
                lblAlertDSH.Text = Resources.labels.bancannhaptendangnhap;
                return;
            }
            //minh add 11/9/2015 check trung so dt
            #region check phone number and default account

            string phoneNumber = txtSMSPhoneNguoiUyQuyen.Text.Trim();
            string defaultAcc = ddlSMSDefaultAcctnoUyQuyen.Text.Trim();
            DataTable dt1 = new SmartPortal.SEMS.Customer().CheckPhoneNumber(phoneNumber, defaultAcc);
            if (dt1.Rows.Count != 0)
            {

                lblAlertDSH.Text = Resources.labels.phonenumberassigned;
                return;
            }


            #endregion
            //minh add 11/9/2015 validate thong tin
            if (string.IsNullOrEmpty(txtFullnameNguoiUyQuyen.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtFullnameNguoiUyQuyen.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneNguoiUyQuyen.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoainguoidongsohuu);

                txtPhoneNguoiUyQuyen.Focus();
                return;
            }


            string pattern = Resources.labels.emailpattern;

            //truong hop individual email = "" chap nhan, corporate validate ca truong hop rong


            if (!string.IsNullOrEmpty(txtEmailNguoiUyQuyen.Text.Trim()))
            {
                if (!(System.Text.RegularExpressions.Regex.IsMatch(txtEmailNguoiUyQuyen.Text, pattern)))
                {
                    ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                    //lblAlert.Text = Resources.labels.emailkhongdinhdang1;
                    txtEmailNguoiUyQuyen.Focus();
                    return;

                }
            }
            //1.4.2016 fix userid khac phoneno 
            //if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            //{
            //    //txtMBUsersName.Text = ownerUserName;
            //    txtMBUserNguoiUyQuyen.Text = usernameAuthorized;


            //}
            //else
            //{

            //}
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB'";
            if (ddlpolicyIB.SelectedValue.ToString() != "")
            {
                filterIB += " and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            }
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMSco.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMBco.SelectedValue.ToString() + "'";
            string filterWL = "serviceid='WL' and policyid='" + ddlpolicyMBco.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";
            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {


                DataTable dt = dspolicy.Tables[0];
                DataTable dtIB = dt.Select(filterIB, stSort).Any() ? dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable() : null;
                DataTable dtSMS = dt.Select(filterSMS, stSort).Any() ? dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable() : null;
                DataTable dtMB = dt.Select(filterMB, stSort).Any() ? dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable() : null;
                DataTable dtWL = dt.Select(filterWL, stSort).Any() ? dspolicy.Tables[0].Select(filterWL, stSort).CopyToDataTable() : null;
                passlenIB = dtIB == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = dtSMS == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = dtMB == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
                passlenWL = dtWL == null ? int.Parse(ConfigurationManager.AppSettings["minpwdlen_default"].ToString()) : Convert.ToInt32(dtWL.Rows[0]["minpwdlen"].ToString());

            }

            if (ddlAccUyQuyen.SelectedValue != "ALL")
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUserNguoiUyQuyen.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, ddlAccountUyQuyen.SelectedValue, ddlCTKDefaultAcctno.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, usernameAuthorized);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                // LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUserNguoiUyQuyen.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, ddlAccountUyQuyen.SelectedValue, ddlCTKDefaultAcctno.SelectedValue, pwdreset);
                LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, tvWLUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, PHONENUQ, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, ddlAccUyQuyen.SelectedValue, ddlAccUyQuyen.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), "", PassTemp);




            }
            if (ddlAccUyQuyen.SelectedValue == "ALL")
            {
                //lay tat ca tai khoan khach hang
                string cc = "";
                string ct = "";

                DataSet dsCust = new SmartPortal.SEMS.Customer().GetCustomerByCustcode(hidCustID.Value, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                else
                {
                    DataTable tblCust = dsCust.Tables[0];
                    if (tblCust.Rows.Count != 0)
                    {
                        //17.9.2015 minh modify this: add trim()
                        cc = tblCust.Rows[0]["CFCODE"].ToString().Trim();
                        ct = tblCust.Rows[0]["CFTYPE"].ToString().Trim();
                    }
                }

                DataSet ds = new DataSet();
                ds = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ct, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtAccount = new DataTable();
                dtAccount = ds.Tables[0];

                //luu tat ca account
                //// PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // string pwdreset = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(pwdreset, usernameAuthorized);
                //24.2.2016 minh add encrypt pass new
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, usernameAuthorized);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUserNguoiUyQuyen.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno.SelectedValue);
                    //LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBUserNguoiUyQuyen.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno.SelectedValue, pwdreset);
                    LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, tvWLUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, PHONENUQ, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ddlCTKDefaultAcctno.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), "", PassTemp);

                }


            }

            if (TabCustomerInfoHelper.TabWalletVisibility == 1)
            {
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenMB, passlenMB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, usernameAuthorized);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                LuuThongTinQuyen("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, tvWLUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, usernameAuthorized, PassTemp, "", ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, ((cbCTKTKMDNUQ.Checked == true) ? "Y" : "N"), pwdresetSMS, "", PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNguoiUyQuyen.Text, PassTemp, PHONECTK, ddlAccountUyQuyen.SelectedValue, pwdreset, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), PHONENUQ, PassTemp);

            }
            lblAlertDSH.Text = Resources.labels.recordsaved;
            #endregion


            //28/8/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultNguoiUyQuyen.Rows.Count == 0)
            {

                lblAlertDSH.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnThemNguoiUyQuyen_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "btnThemNguoiUyQuyen_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    protected void gvResultNguoiUyQuyen_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultNguoiUyQuyen.PageIndex = e.NewPageIndex;
            gvResultNguoiUyQuyen.DataSource = (DataTable)ViewState["NGUOIUYQUYEN"];
            gvResultNguoiUyQuyen.DataBind();
        }
        catch
        {
        }
    }

    protected void gvResultNguoiUyQuyen_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultNguoiUyQuyen = (DataTable)ViewState["NGUOIUYQUYEN"];

            tblResultNguoiUyQuyen.Rows.RemoveAt(e.RowIndex + (gvResultNguoiUyQuyen.PageIndex * gvResultNguoiUyQuyen.PageSize));

            ViewState["NGUOIUYQUYEN"] = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataSource = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataBind();

            lblAlertDSH.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void gvResultChuTaiKhoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultChuTaiKhoan.PageIndex = e.NewPageIndex;
            gvResultChuTaiKhoan.DataSource = (DataTable)ViewState["CHUTAIKHOAN"];
            gvResultChuTaiKhoan.DataBind();
        }
        catch
        {
        }
    }

    protected void gvResultChuTaiKhoan_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultChuTaiKhoan = (DataTable)ViewState["CHUTAIKHOAN"];

            tblResultChuTaiKhoan.Rows.RemoveAt(e.RowIndex + (gvResultChuTaiKhoan.PageIndex * gvResultChuTaiKhoan.PageSize));

            ViewState["CHUTAIKHOAN"] = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataSource = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataBind();

            lblAlert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void btnResetNguoiUyQuyen_Click(object sender, EventArgs e)
    {
        try
        {
            #region reset thong tin nguoi uy quyen
            txtFullnameNguoiUyQuyen.Text = "";
            txtBirthNguoiUyQuyen.Text = "";
            txtPhoneNguoiUyQuyen.Text = "";
            txtEmailNguoiUyQuyen.Text = "";
            txtAddressNguoiUyQuyen.Text = "";

            txtSMSPhoneNguoiUyQuyen.Text = "";
            txtMBUserNguoiUyQuyen.Text = "";
            txtPHOPhoneNguoiUyQuyen.Text = "";

            //lay thong tin custcode và cust type
            string cc = "";
            string ct = "";
            DataTable dtCustInfo = new DataTable();
            DataSet dsUserInfo = new SmartPortal.SEMS.Customer().GetCustomerByCustcode(hidCustID.Value.Trim(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {
                dtCustInfo = dsUserInfo.Tables[0];
                if (dtCustInfo.Rows.Count != 0)
                {
                    cc = dtCustInfo.Rows[0]["CUSTCODE"].ToString();
                    ct = dtCustInfo.Rows[0]["CFTYPE"].ToString();
                }
                else
                {
                    throw new IPCException(IPCERRORDESC);
                }
            }
            else
            {
                throw new IPCException(IPCERRORDESC);
            }

            string strCode = cc.Trim() + ct.Trim() + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 6, 4);
            txtIBGenUserNameNguoiUyQuyen.Text = strCode;
            txtIBTypeUserNameNguoiUyQuyen.Text = "";

            SetRadio();
            #endregion
        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnResetNguoiUyQuyen_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }
    }
    void SetRadio()
    {
        radAllAccount.Checked = true;
        radAccount.Checked = false;

        radAllAccountNguoiUyQuyen.Checked = true;
        radAccountNguoiUyQuyen.Checked = false;
    }
    protected void btnCustSave_Click(object sender, EventArgs e)
    {
        //nasiall
        try
        {
            if (ViewState["CHUTAIKHOAN"] == null)
            {
                lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoan;
                return;
            }

            DataTable tblCHUTAIKHOAN = new DataTable();
            DataTable tblNGUOIUYQUYEN = new DataTable();
            DataTable tblLEVEL2 = new DataTable();

            if (ViewState["CHUTAIKHOAN"] != null)
            {
                tblCHUTAIKHOAN = (DataTable)ViewState["CHUTAIKHOAN"];
                if (tblCHUTAIKHOAN.Rows.Count == 0)
                {
                    lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoan;
                    return;
                }
            }
            //minh add 11/9/2015 them dk bat buoc phai co email hoac sms phone
            string emailctk = "";
            string smsphonectk = "";
            emailctk = tblCHUTAIKHOAN.Rows[0]["colEmail"].ToString();
            //smsphonectk = tblCHUTAIKHOAN.Rows[0]["colSMSPhone"].ToString();
            foreach (DataRow r in tblCHUTAIKHOAN.Rows)
            {
                if (r["colServiceID"].ToString() == SmartPortal.Constant.IPC.SMS)
                {
                    smsphonectk = r["colSMSPhone"].ToString();
                    break;
                }
            }
            if (ViewState["NGUOIUYQUYEN"] != null)
            {
                tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                string emailnuq = "";
                string smsphonenuq = "";
                emailnuq = tblNGUOIUYQUYEN.Rows[0]["colEmail"].ToString();
                //string smsphonenuq = "";
                foreach (DataRow r in tblNGUOIUYQUYEN.Rows)
                {
                    if (r["colServiceID"].ToString() == SmartPortal.Constant.IPC.SMS)
                    {
                        smsphonenuq = r["colSMSPhone"].ToString();
                        break;
                    }
                }
                if (emailnuq == string.Empty && smsphonenuq == string.Empty)
                {
                    lblAlert.Text = Resources.labels.nguoiuyquyenphaidangkyitnhatemailhoacsmsservice;
                    return;
                }
            }



            #region Tạo 1 bảng tổng hợp 3 user trên
            DataTable tblSUM = new DataTable();

            DataColumn colFullName = new DataColumn("colFullName");
            DataColumn colLevel = new DataColumn("colLevel");
            DataColumn colBirthday = new DataColumn("colBirthday");
            DataColumn colGender = new DataColumn("colGender");
            DataColumn colPhone = new DataColumn("colPhone");
            DataColumn colEmail = new DataColumn("colEmail");
            DataColumn colAddress = new DataColumn("colAddress");
            DataColumn colUT = new DataColumn("colUT");
            DataColumn colIBUserName = new DataColumn("colIBUserName");
            DataColumn colIBPass = new DataColumn("colIBPass");
            DataColumn colSMSPhone = new DataColumn("colSMSPhone");
            DataColumn colSMSDefaultAcctno = new DataColumn("colSMSDefaultAcctno");
            DataColumn colSMSDefaultLang = new DataColumn("colSMSDefaultLang");
            DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault"); // moi them
            DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");// moi them
            DataColumn colMBPhone = new DataColumn("colMBPhone");
            DataColumn colMBPass = new DataColumn("colMBPass");
            DataColumn colMBPinCode = new DataColumn("colMBPinCode");// moi them
            DataColumn colWLPhone = new DataColumn("colWLPhone");
            DataColumn colWLPass = new DataColumn("colWLPass");
            DataColumn colWLPinCode = new DataColumn("colWLPinCode");
            DataColumn colPHOPhone = new DataColumn("colPHOPhone");
            DataColumn colPHOPass = new DataColumn("colPHOPass");
            DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");
            DataColumn colAccount = new DataColumn("colAccount");
            DataColumn colRole = new DataColumn("colRole");
            DataColumn colRoleIDS = new DataColumn("colRoleID");
            DataColumn colTranCodeS = new DataColumn("colTranCode");
            DataColumn colTranCodeID = new DataColumn("colTranCodeID");
            DataColumn colServiceIDS = new DataColumn("colServiceID");
            DataColumn colIBPolicy = new DataColumn("colIBPolicy");
            DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
            DataColumn colMBPolicy = new DataColumn("colMBPolicy");
            DataColumn colWLPolicy = new DataColumn("colWLPolicy");
            DataColumn colpwdreset = new DataColumn("colpwdreset");


            tblSUM.Columns.Add(colFullName);
            tblSUM.Columns.Add(colLevel);
            tblSUM.Columns.Add(colBirthday);
            tblSUM.Columns.Add(colGender);
            tblSUM.Columns.Add(colPhone);
            tblSUM.Columns.Add(colEmail);
            tblSUM.Columns.Add(colAddress);
            tblSUM.Columns.Add(colUT);
            tblSUM.Columns.Add(colIBUserName);
            tblSUM.Columns.Add(colIBPass);
            tblSUM.Columns.Add(colSMSPhone);
            tblSUM.Columns.Add(colSMSDefaultAcctno);
            tblSUM.Columns.Add(colSMSDefaultLang);
            tblSUM.Columns.Add(colSMSIsDefault);
            tblSUM.Columns.Add(colSMSPinCode);
            tblSUM.Columns.Add(colMBPhone);
            tblSUM.Columns.Add(colMBPass);
            tblSUM.Columns.Add(colMBPinCode);
            tblSUM.Columns.Add(colWLPhone);
            tblSUM.Columns.Add(colWLPass);
            tblSUM.Columns.Add(colWLPinCode);
            tblSUM.Columns.Add(colPHOPhone);
            tblSUM.Columns.Add(colPHOPass);
            tblSUM.Columns.Add(colPHODefaultAcctno);
            tblSUM.Columns.Add(colAccount);
            tblSUM.Columns.Add(colRole);
            tblSUM.Columns.Add(colRoleIDS);
            tblSUM.Columns.Add(colTranCodeS);
            tblSUM.Columns.Add(colTranCodeID);
            tblSUM.Columns.Add(colServiceIDS);
            tblSUM.Columns.Add(colIBPolicy);
            tblSUM.Columns.Add(colSMSPolicy);
            tblSUM.Columns.Add(colMBPolicy);
            tblSUM.Columns.Add(colWLPolicy);
            tblSUM.Columns.Add(colpwdreset);

            //lấy thông tin trong bảng CHUTAIKHOAN
            foreach (DataRow dongCTK in tblCHUTAIKHOAN.Rows)
            {
                DataRow rowNguoiUyQuyen = tblSUM.NewRow();
                rowNguoiUyQuyen["colFullName"] = dongCTK["colFullName"].ToString();
                rowNguoiUyQuyen["colLevel"] = dongCTK["colLevel"].ToString();
                rowNguoiUyQuyen["colBirthday"] = dongCTK["colBirthday"].ToString();
                rowNguoiUyQuyen["colGender"] = dongCTK["colGender"].ToString();
                rowNguoiUyQuyen["colPhone"] = dongCTK["colPhone"].ToString();
                rowNguoiUyQuyen["colEmail"] = dongCTK["colEmail"].ToString();
                rowNguoiUyQuyen["colAddress"] = dongCTK["colAddress"].ToString();
                rowNguoiUyQuyen["colUT"] = SmartPortal.Constant.IPC.PCO;
                rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                rowNguoiUyQuyen["colWLPhone"] = dongCTK["colWLPhone"].ToString();
                rowNguoiUyQuyen["colWLPass"] = dongCTK["colWLPass"].ToString();
                rowNguoiUyQuyen["colWLPinCode"] = dongCTK["colWLPinCode"].ToString();
                rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();
                rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
                rowNguoiUyQuyen["colWLPolicy"] = dongCTK["colWLPolicy"].ToString();
                rowNguoiUyQuyen["colpwdreset"] = dongCTK["colpwdreset"].ToString();

                tblSUM.Rows.Add(rowNguoiUyQuyen);
            }

            //lấy thông tin trong bảng NGUOIUYQUYEN
            foreach (DataRow dongCTK in tblNGUOIUYQUYEN.Rows)
            {
                DataRow rowNguoiUyQuyen = tblSUM.NewRow();
                rowNguoiUyQuyen["colFullName"] = dongCTK["colFullName"].ToString();
                rowNguoiUyQuyen["colLevel"] = dongCTK["colLevel"].ToString();
                rowNguoiUyQuyen["colBirthday"] = dongCTK["colBirthday"].ToString();
                rowNguoiUyQuyen["colGender"] = dongCTK["colGender"].ToString();
                rowNguoiUyQuyen["colPhone"] = dongCTK["colPhone"].ToString();
                rowNguoiUyQuyen["colEmail"] = dongCTK["colEmail"].ToString();
                rowNguoiUyQuyen["colAddress"] = dongCTK["colAddress"].ToString();
                rowNguoiUyQuyen["colUT"] = SmartPortal.Constant.IPC.RP;
                rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                rowNguoiUyQuyen["colWLPhone"] = dongCTK["colWLPhone"].ToString();
                rowNguoiUyQuyen["colWLPass"] = dongCTK["colWLPass"].ToString();
                rowNguoiUyQuyen["colWlPinCode"] = dongCTK["colWLPinCode"].ToString();
                rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();
                rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
                rowNguoiUyQuyen["colWLPolicy"] = dongCTK["colWLPolicy"].ToString();
                rowNguoiUyQuyen["colpwdreset"] = dongCTK["colpwdreset"].ToString();


                tblSUM.Rows.Add(rowNguoiUyQuyen);
            }


            #endregion

            #region Biến thông tin Customer và Contract
            string custID = SmartPortal.Common.Utilities.Utility.KillSqlInjection(hidCustID.Value.Trim());
            string branchID = Session["branch"].ToString();

            string contractNo = SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtContractNo.Text.Trim());
            string contractType = SmartPortal.Common.Utilities.Utility.KillSqlInjection(ddlContractType.SelectedValue.Trim());
            string productID = SmartPortal.Common.Utilities.Utility.KillSqlInjection(ddlProduct.SelectedValue.Trim());
            string startDate = SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtStartDate.Text.Trim());
            string endDate = SmartPortal.Common.Utilities.Utility.KillSqlInjection(txtEndDate.Text.Trim());
            string lastModify = SmartPortal.Common.Utilities.Utility.KillSqlInjection(DateTime.Now.ToString("dd/MM/yyyy"));
            string userCreate = SmartPortal.Common.Utilities.Utility.KillSqlInjection(Session["userName"].ToString().Trim());
            string userLastModify = SmartPortal.Common.Utilities.Utility.KillSqlInjection(Session["userName"].ToString().Trim());
            string userApprove = "";
            string status = SmartPortal.Constant.IPC.NEW;
            string allAcct = "Y";
            string isSpecialMan = "";
            #endregion


            string userType = SmartPortal.Common.Utilities.Utility.KillSqlInjection(ddlContractType.SelectedValue);
            string deptID = "";
            string uCreateDate = DateTime.Now.ToString("dd/MM/yyyy");
            string tokenID = "";
            string tokenIssueDate = "01/01/1900";
            string smsOTP = "";

            string IBUserName = "";
            string IBPassword = "";

            string SMSPhoneNo = "";
            string SMSDefaultAcctno = "";
            string SMSDefaultLang = "";

            string MBPhoneNo = "";
            string MBPass = "";

            string PHOPhoneNo = "";
            string PHOPass = "";

            #region Tạo bảng chứa thông tin user
            DataTable tblUser = new DataTable();
            DataColumn colUserID = new DataColumn("colUserID");
            DataColumn colUContractNo = new DataColumn("colUContractNo");
            DataColumn colUFullName = new DataColumn("colUFullName");
            DataColumn colUGender = new DataColumn("colUGender");
            DataColumn colUAddress = new DataColumn("colUAddress");
            DataColumn colUEmail = new DataColumn("colUEmail");
            DataColumn colUPhone = new DataColumn("colUPhone");
            DataColumn colUStatus = new DataColumn("colUStatus");
            DataColumn colUUserCreate = new DataColumn("colUUserCreate");
            DataColumn colUDateCreate = new DataColumn("colUDateCreate");
            DataColumn colUUserModify = new DataColumn("colUUserModify");
            DataColumn colULastModify = new DataColumn("colULastModify");
            DataColumn colUUserApprove = new DataColumn("colUUserApprove");
            DataColumn colUserType = new DataColumn("colUserType");
            DataColumn colUserLevel = new DataColumn("colUserLevel");
            DataColumn colDeptID = new DataColumn("colDeptID");
            DataColumn colTokenID = new DataColumn("colTokenID");
            DataColumn colTokenIssueDate = new DataColumn("colTokenIssueDate");
            DataColumn colSMSOTP = new DataColumn("colSMSOTP");
            DataColumn colSMSBirthday = new DataColumn("colSMSBirthday");
            DataColumn colTypeID = new DataColumn("colTypeID");

            //add vào table
            tblUser.Columns.Add(colUserID);
            tblUser.Columns.Add(colUContractNo);
            tblUser.Columns.Add(colUFullName);
            tblUser.Columns.Add(colUGender);
            tblUser.Columns.Add(colUAddress);
            tblUser.Columns.Add(colUEmail);
            tblUser.Columns.Add(colUPhone);
            tblUser.Columns.Add(colUStatus);
            tblUser.Columns.Add(colUUserCreate);
            tblUser.Columns.Add(colUDateCreate);
            tblUser.Columns.Add(colUUserModify);
            tblUser.Columns.Add(colULastModify);
            tblUser.Columns.Add(colUUserApprove);
            tblUser.Columns.Add(colUserType);
            tblUser.Columns.Add(colUserLevel);
            tblUser.Columns.Add(colDeptID);
            tblUser.Columns.Add(colTokenID);
            tblUser.Columns.Add(colTokenIssueDate);
            tblUser.Columns.Add(colSMSOTP);
            tblUser.Columns.Add(colSMSBirthday);
            tblUser.Columns.Add(colTypeID);

            //tao 1 dong du lieu
            string UID = "";
            for (int i = 0; i < tblSUM.Rows.Count; i++)
            {
                if (tblSUM.Rows[i]["colIBUserName"].ToString().Trim() != UID.Trim())
                {
                    DataRow row2 = tblUser.NewRow();

                    row2["colUserID"] = tblSUM.Rows[i]["colIBUserName"].ToString();
                    row2["colUContractNo"] = contractNo;
                    row2["colUFullName"] = tblSUM.Rows[i]["colFullName"].ToString();
                    row2["colUGender"] = tblSUM.Rows[i]["colGender"].ToString();
                    row2["colUAddress"] = tblSUM.Rows[i]["colAddress"].ToString();
                    row2["colUEmail"] = tblSUM.Rows[i]["colEmail"].ToString();
                    row2["colUPhone"] = tblSUM.Rows[i]["colPhone"].ToString();
                    row2["colUStatus"] = status;
                    row2["colUUserCreate"] = userCreate;
                    row2["colUDateCreate"] = uCreateDate;
                    row2["colUUserModify"] = userCreate;
                    row2["colULastModify"] = lastModify;

                    row2["colUUserApprove"] = userApprove;
                    row2["colUserType"] = tblSUM.Rows[i]["colUT"].ToString();
                    row2["colUserLevel"] = tblSUM.Rows[i]["colLevel"].ToString();
                    row2["colDeptID"] = deptID;
                    row2["colTokenID"] = tokenID;
                    row2["colTokenIssueDate"] = tokenIssueDate;
                    row2["colSMSOTP"] = smsOTP;
                    row2["colSMSBirthday"] = tblSUM.Rows[i]["colBirthday"].ToString();
                    row2["colTypeID"] = "";

                    tblUser.Rows.Add(row2);

                    UID = tblSUM.Rows[i]["colIBUserName"].ToString().Trim();
                }
            }

            #endregion

            #region Tạo bảng chứa user Ibank
            DataTable tblIbankUser = new DataTable();
            DataColumn colUserName = new DataColumn("colUserName");
            DataColumn colIBUserID = new DataColumn("colIBUserID");
            DataColumn colIBPassword = new DataColumn("colIBPassword");
            DataColumn colLastLoginTime = new DataColumn("colLastLoginTime");
            DataColumn colIBStatus = new DataColumn("colIBStatus");
            DataColumn colIBUserCreate = new DataColumn("colIBUserCreate");
            DataColumn colIBDateCreate = new DataColumn("colIBDateCreate");
            DataColumn colIBUserModify = new DataColumn("colIBUserModify");
            DataColumn colIBLastModify = new DataColumn("colIBLastModify");
            DataColumn colIBUserApprove = new DataColumn("colIBUserApprove");
            DataColumn colIBIsLogin = new DataColumn("colIBIsLogin");
            DataColumn colIBDateExpire = new DataColumn("colIBDateExpire");
            DataColumn colIBExpireTime = new DataColumn("colIBExpireTime");
            DataColumn colIBPolicyusr = new DataColumn("colIBPolicyusr");
            DataColumn colpwdresetIB = new DataColumn("colpwdresetIB");


            //add vào table
            tblIbankUser.Columns.Add(colUserName);
            tblIbankUser.Columns.Add(colIBUserID);
            tblIbankUser.Columns.Add(colIBPassword);
            tblIbankUser.Columns.Add(colLastLoginTime);
            tblIbankUser.Columns.Add(colIBStatus);
            tblIbankUser.Columns.Add(colIBUserCreate);
            tblIbankUser.Columns.Add(colIBDateCreate);
            tblIbankUser.Columns.Add(colIBUserModify);
            tblIbankUser.Columns.Add(colIBLastModify);
            tblIbankUser.Columns.Add(colIBUserApprove);
            tblIbankUser.Columns.Add(colIBIsLogin);
            tblIbankUser.Columns.Add(colIBDateExpire);
            tblIbankUser.Columns.Add(colIBExpireTime);
            tblIbankUser.Columns.Add(colIBPolicyusr);
            tblIbankUser.Columns.Add(colpwdresetIB);


            //tao 1 dong du lieu
            string UN = "";
            //foreach (DataRow rIBU in tblSUM.Rows)
            //{
            //    if (rIBU["colIBUserName"].ToString().Trim() != UN.Trim() && rIBU["colIBUserName"].ToString().Trim() != "")
            //    {
            //        DataRow row3 = tblIbankUser.NewRow();
            //        row3["colUserName"] = rIBU["colIBUserName"].ToString();
            //        row3["colIBUserID"] = rIBU["colIBUserName"].ToString();
            //        row3["colIBPassword"] = rIBU["colIBPass"].ToString();
            //        row3["colLastLoginTime"] = uCreateDate;
            //        row3["colIBStatus"] = status;
            //        row3["colIBUserCreate"] = userCreate;
            //        row3["colIBDateCreate"] = uCreateDate;
            //        row3["colIBUserModify"] = userCreate;
            //        row3["colIBLastModify"] = lastModify;
            //        row3["colIBUserApprove"] = userApprove;
            //        row3["colIBIsLogin"] = "0";
            //        row3["colIBDateExpire"] = endDate;
            //        row3["colIBExpireTime"] = startDate;
            //        row3["colIBPolicyusr"] = rIBU["colIBPolicy"].ToString();
            //        row3["colpwdresetIB"] = rIBU["colpwdreset"].ToString();



            //        tblIbankUser.Rows.Add(row3);

            //        //There is no IB tab

            //        UN = rIBU["colIBUserName"].ToString();
            //    }
            //}
            //tblIbankUser = new DataTable();
            #endregion

            #region Tạo bảng chứa User SMS
            DataTable tblSMSUser = new DataTable();
            DataColumn colSMSUserID = new DataColumn("colSMSUserID");
            DataColumn colSMSPhoneNo = new DataColumn("colSMSPhoneNo");
            DataColumn colSMSContractNo = new DataColumn("colSMSContractNo");
            DataColumn colSMSIsBroadcast = new DataColumn("colSMSIsBroadcast");
            DataColumn colSMSDefaultAcctnoU = new DataColumn("colSMSDefaultAcctno");
            DataColumn colSMSDefaultLangU = new DataColumn("colSMSDefaultLang");
            DataColumn colSMSIsDefault1 = new DataColumn("colSMSIsDefault");
            DataColumn colSMSPinCode1 = new DataColumn("colSMSPinCode");
            DataColumn colSMSStatus = new DataColumn("colSMSStatus");
            DataColumn colSMSPhoneType = new DataColumn("colSMSPhoneType");
            DataColumn colSMSUserCreate = new DataColumn("colSMSUserCreate");
            DataColumn colSMSUserModify = new DataColumn("colSMSUserModify");
            DataColumn colSMSUserApprove = new DataColumn("colSMSUserApprove");
            DataColumn colSMSLastModify = new DataColumn("colSMSLastModify");
            DataColumn colSMSDateCreate = new DataColumn("colSMSDateCreate");
            DataColumn colSMSDateExpire = new DataColumn("colSMSDateExpire");
            DataColumn colSMSPolicyusr = new DataColumn("colSMSPolicyusr");
            DataColumn colpwdresetSMS = new DataColumn("colpwdresetSMS");


            //add vào table
            tblSMSUser.Columns.Add(colSMSUserID);
            tblSMSUser.Columns.Add(colSMSPhoneNo);
            tblSMSUser.Columns.Add(colSMSContractNo);
            tblSMSUser.Columns.Add(colSMSIsBroadcast);
            tblSMSUser.Columns.Add(colSMSDefaultAcctnoU);
            tblSMSUser.Columns.Add(colSMSDefaultLangU);
            tblSMSUser.Columns.Add(colSMSIsDefault1);
            tblSMSUser.Columns.Add(colSMSPinCode1);
            tblSMSUser.Columns.Add(colSMSStatus);
            tblSMSUser.Columns.Add(colSMSPhoneType);
            tblSMSUser.Columns.Add(colSMSUserCreate);
            tblSMSUser.Columns.Add(colSMSUserModify);
            tblSMSUser.Columns.Add(colSMSUserApprove);
            tblSMSUser.Columns.Add(colSMSLastModify);
            tblSMSUser.Columns.Add(colSMSDateCreate);
            tblSMSUser.Columns.Add(colSMSDateExpire);
            tblSMSUser.Columns.Add(colSMSPolicyusr);
            tblSMSUser.Columns.Add(colpwdresetSMS);

            //tao 1 dong du lieu
            string SMSP = "";
            foreach (DataRow rSMSU in tblSUM.Rows)
            {
                if (rSMSU["colSMSPhone"].ToString().Trim() != SMSP.Trim() && rSMSU["colSMSPhone"].ToString().Trim() != "")
                {
                    DataRow row4 = tblSMSUser.NewRow();
                    row4["colSMSUserID"] = rSMSU["colIBUserName"].ToString();
                    row4["colSMSPhoneNo"] = rSMSU["colSMSPhone"].ToString();
                    row4["colSMSContractNo"] = contractNo;
                    row4["colSMSIsBroadcast"] = "N";
                    row4["colSMSDefaultAcctno"] = rSMSU["colSMSDefaultAcctno"].ToString();
                    row4["colSMSDefaultLang"] = rSMSU["colSMSDefaultLang"].ToString();
                    row4["colSMSIsDefault"] = rSMSU["colSMSIsDefault"].ToString();
                    row4["colSMSPinCode"] = SmartPortal.SEMS.O9Encryptpass.sha_sha256(rSMSU["colSMSPinCode"].ToString(), rSMSU["colSMSPhone"].ToString().Trim());
                    row4["colSMSStatus"] = status;
                    row4["colSMSPhoneType"] = "";
                    row4["colSMSUserCreate"] = userCreate;
                    row4["colSMSUserModify"] = userCreate;
                    row4["colSMSUserApprove"] = userApprove;
                    row4["colSMSLastModify"] = lastModify;
                    row4["colSMSDateCreate"] = uCreateDate;
                    row4["colSMSDateExpire"] = endDate;
                    row4["colSMSPolicyusr"] = rSMSU["colSMSPolicy"].ToString();
                    row4["colpwdresetSMS"] = Encryption.Encrypt(rSMSU["colSMSPinCode"].ToString());


                    tblSMSUser.Rows.Add(row4);

                    SMSP = rSMSU["colSMSPhone"].ToString();
                }
            }
            #endregion

            #region Tạo bảng chứa User MB
            DataTable tblMBUser = new DataTable();
            DataColumn colMBUserID = new DataColumn("colMBUserID");
            DataColumn colMBPhoneNo = new DataColumn("colMBPhoneNo");
            DataColumn colMBPassU = new DataColumn("colMBPass");
            DataColumn colMBStatus = new DataColumn("colMBStatus");
            DataColumn colMBPinCode1 = new DataColumn("colMBPinCode1");
            DataColumn colMBPolicyusr = new DataColumn("colMBPolicyusr");
            DataColumn colpwdresetMB = new DataColumn("colpwdresetMB");

            //add vào table
            tblMBUser.Columns.Add(colMBUserID);
            tblMBUser.Columns.Add(colMBPhoneNo);
            tblMBUser.Columns.Add(colMBPassU);
            tblMBUser.Columns.Add(colMBStatus);
            tblMBUser.Columns.Add(colMBPinCode1);
            tblMBUser.Columns.Add(colMBPolicyusr);
            tblMBUser.Columns.Add(colpwdresetMB);

            //tao 1 dong du lieu
            string MBP = "";
            foreach (DataRow rMBP in tblSUM.Rows)
            {
                if (rMBP["colMBPhone"].ToString().Trim() != MBP && rMBP["colMBPhone"].ToString().Trim() != "")
                {
                    DataRow row5 = tblMBUser.NewRow();
                    row5["colMBUserID"] = rMBP["colIBUserName"].ToString();
                    row5["colMBPhoneNo"] = rMBP["colMBPhone"].ToString();
                    row5["colMBPass"] = rMBP["colMBPass"].ToString();
                    row5["colMBStatus"] = status;
                    row5["colMBPinCode1"] = SmartPortal.SEMS.O9Encryptpass.sha_sha256(rMBP["colMBPinCode"].ToString(), rMBP["colMBPhone"].ToString().Trim());
                    row5["colMBPolicyusr"] = rMBP["colMBPolicy"].ToString();
                    row5["colpwdresetMB"] = rMBP["colpwdreset"].ToString();

                    tblMBUser.Rows.Add(row5);

                    MBP = rMBP["colMBPhone"].ToString();
                }
            }
            #endregion

            #region Tạo bảng chứa User PHO
            DataTable tblPHOUser = new DataTable();
            DataColumn colPHOUserID = new DataColumn("colPHOUserID");
            DataColumn colPHOPhoneNo = new DataColumn("colPHOPhoneNo");
            DataColumn colPHOPassU = new DataColumn("colPHOPass");
            DataColumn colPHOStatus = new DataColumn("colPHOStatus");
            DataColumn colPHODefaultAcctnoU = new DataColumn("colPHODefaultAcctno");

            //kuquyen
            //add vào table
            tblPHOUser.Columns.Add(colPHOUserID);
            tblPHOUser.Columns.Add(colPHOPhoneNo);
            tblPHOUser.Columns.Add(colPHOPassU);
            tblPHOUser.Columns.Add(colPHOStatus);
            tblPHOUser.Columns.Add(colPHODefaultAcctnoU);


            //tao 1 dong du lieu
            string PHOP = "";
            foreach (DataRow rPHOP in tblSUM.Rows)
            {
                if (rPHOP["colPHOPhone"].ToString().Trim() != PHOP && rPHOP["colPHOPhone"].ToString().Trim() != "")
                {
                    DataRow row6 = tblPHOUser.NewRow();
                    row6["colPHOUserID"] = rPHOP["colIBUserName"].ToString();
                    row6["colPHOPhoneNo"] = rPHOP["colPHOPhone"].ToString();
                    row6["colPHOPass"] = rPHOP["colPHOPass"].ToString();
                    row6["colPHODefaultAcctno"] = rPHOP["colPHODefaultAcctno"].ToString();
                    row6["colPHOStatus"] = status;

                    tblPHOUser.Rows.Add(row6);
                    PHOP = rPHOP["colPHOPhone"].ToString();
                }
            }
            #endregion

            #region Tạo bảng chứa User Wallet
            DataTable tblWLUser = new DataTable();
            DataColumn colWLUserID = new DataColumn("colWLUserID");
            DataColumn colWLPhoneNo = new DataColumn("colWLPhoneNo");
            DataColumn colWLPassU = new DataColumn("colWLPass");
            DataColumn colWLStatus = new DataColumn("colWLStatus");
            DataColumn colWLPinCode1 = new DataColumn("colWLPinCode1");
            DataColumn colWLPolicyusr = new DataColumn("colWLPolicyusr");
            DataColumn colpwdresetWL = new DataColumn("colpwdresetWL");

            //add vào table
            tblWLUser.Columns.Add(colWLUserID);
            tblWLUser.Columns.Add(colWLPhoneNo);
            tblWLUser.Columns.Add(colWLPassU);
            tblWLUser.Columns.Add(colWLStatus);
            tblWLUser.Columns.Add(colWLPinCode1);
            tblWLUser.Columns.Add(colWLPolicyusr);
            tblWLUser.Columns.Add(colpwdresetWL);

            //tao 1 dong du lieu
            string WLP = "";
            foreach (DataRow rWLP in tblSUM.Rows)
            {
                if (rWLP["colWLPhone"].ToString().Trim() != WLP && rWLP["colWLPhone"].ToString().Trim() != "")
                {
                    DataRow row5 = tblWLUser.NewRow();
                    row5["colWLUserID"] = rWLP["colIBUserName"].ToString();
                    row5["colWLPhoneNo"] = rWLP["colWLPhone"].ToString();
                    row5["colWLPass"] = rWLP["colWLPass"].ToString();
                    row5["colWLStatus"] = status;
                    row5["colWLPinCode1"] = rWLP["colWLPinCode"].ToString();
                    row5["colWLPolicyusr"] = rWLP["colWLPolicy"].ToString();
                    row5["colpwdresetWL"] = rWLP["colpwdreset"].ToString();

                    tblWLUser.Rows.Add(row5);

                    WLP = rWLP["colWLPhone"].ToString();
                }
            }
            #endregion

            #region Tạo bảng chứa quyền user Ibank
            //tao bang chua thong tin customer
            DataTable tblIbankUserRight = new DataTable();
            DataColumn colIBUserNameRight = new DataColumn("colIBUserNameRight");
            DataColumn colIBRoleID = new DataColumn("colIBRoleID");

            //add vào table
            tblIbankUserRight.Columns.Add(colIBUserNameRight);
            tblIbankUserRight.Columns.Add(colIBRoleID);

            //tao 1 dong du lieu
            DataRow[] arrIBR = tblSUM.Select("colServiceID='" + SmartPortal.Constant.IPC.IB + "'");
            foreach (DataRow rIBR in arrIBR)
            {
                if (tblIbankUserRight.Select("colIBUserNameRight='" + rIBR["colIBUserName"].ToString() + "' and colIBRoleID='" + rIBR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowIBR = tblIbankUserRight.NewRow();
                    newRowIBR["colIBUserNameRight"] = rIBR["colIBUserName"].ToString();
                    newRowIBR["colIBRoleID"] = rIBR["colRoleID"].ToString();

                    tblIbankUserRight.Rows.Add(newRowIBR);
                }
            }
            #endregion

            #region Tạo bảng chứa quyền user SMS
            //tao bang chua thong tin customer
            DataTable tblSMSUserRight = new DataTable();
            DataColumn colSMSUserIDR = new DataColumn("colSMSUserID");
            DataColumn colSMSRoleID = new DataColumn("colSMSRoleID");

            //add vào table
            tblSMSUserRight.Columns.Add(colSMSUserIDR);
            tblSMSUserRight.Columns.Add(colSMSRoleID);

            //tao 1 dong du lieu
            DataRow[] arrSMSR = tblSUM.Select("colServiceID='" + SmartPortal.Constant.IPC.SMS + "'");
            foreach (DataRow rSMSR in arrSMSR)
            {
                if (tblSMSUserRight.Select("colSMSUserID='" + rSMSR["colIBUserName"].ToString() + "' and colSMSRoleID='" + rSMSR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowSMSR = tblSMSUserRight.NewRow();
                    newRowSMSR["colSMSUserID"] = rSMSR["colIBUserName"].ToString();
                    newRowSMSR["colSMSRoleID"] = rSMSR["colRoleID"].ToString();

                    tblSMSUserRight.Rows.Add(newRowSMSR);
                }
            }
            #endregion

            #region Tạo bảng chứa quyền user MB
            //tao bang chua thong tin customer
            DataTable tblMBUserRight = new DataTable();
            DataColumn colMBPhoneNoR = new DataColumn("colMBPhoneNo");
            DataColumn colMBRoleID = new DataColumn("colMBRoleID");

            //add vào table
            tblMBUserRight.Columns.Add(colMBPhoneNoR);
            tblMBUserRight.Columns.Add(colMBRoleID);

            //tao 1 dong du lieu
            DataRow[] arrMBR = tblSUM.Select("colServiceID='" + SmartPortal.Constant.IPC.MB + "'");
            foreach (DataRow rMBR in arrMBR)
            {
                if (tblMBUserRight.Select("colMBPhoneNo='" + rMBR["colMBPhone"].ToString() + "' and colMBRoleID='" + rMBR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowMBR = tblMBUserRight.NewRow();
                    newRowMBR["colMBPhoneNo"] = rMBR["colMBPhone"].ToString();
                    newRowMBR["colMBRoleID"] = rMBR["colRoleID"].ToString();

                    tblMBUserRight.Rows.Add(newRowMBR);
                }
            }
            #endregion

            #region Tạo bảng chứa quyền user PHO
            //tao bang chua thong tin customer
            DataTable tblPHOUserRight = new DataTable();
            DataColumn colPHOPhoneNoR = new DataColumn("colPHOPhoneNo");
            DataColumn colPHORoleID = new DataColumn("colPHORoleID");

            //add vào table
            tblPHOUserRight.Columns.Add(colPHOPhoneNoR);
            tblPHOUserRight.Columns.Add(colPHORoleID);

            //tao 1 dong du lieu
            DataRow[] arrPHOR = tblSUM.Select("colServiceID='" + SmartPortal.Constant.IPC.PHO + "'");
            foreach (DataRow rPHOR in arrPHOR)
            {
                if (tblPHOUserRight.Select("colPHOPhoneNo='" + rPHOR["colPHOPhone"].ToString() + "' and colPHORoleID='" + rPHOR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowPHOR = tblPHOUserRight.NewRow();
                    newRowPHOR["colPHOPhoneNo"] = rPHOR["colPHOPhone"].ToString();
                    newRowPHOR["colPHORoleID"] = rPHOR["colRoleID"].ToString();

                    tblPHOUserRight.Rows.Add(newRowPHOR);
                }
            }
            #endregion

            #region Tạo bảng chứa quyền user WL
            //tao bang chua thong tin customer
            DataTable tblWLUserRight = new DataTable();
            DataColumn colWLPhoneNoR = new DataColumn("colWLPhoneNo");
            DataColumn colWLRoleID = new DataColumn("colWLRoleID");

            //add vào table
            tblWLUserRight.Columns.Add(colWLPhoneNoR);
            tblWLUserRight.Columns.Add(colWLRoleID);

            //tao 1 dong du lieu
            DataRow[] arrWLR = tblSUM.Select("colServiceID='" + SmartPortal.Constant.IPC.EW + "'");
            foreach (DataRow rWLR in arrWLR)
            {
                if (tblWLUserRight.Select("colWLPhoneNo='" + rWLR["colWLPhone"].ToString() + "' and colWLRoleID='" + rWLR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowWLR = tblWLUserRight.NewRow();

                    newRowWLR["colWLPhoneNo"] = rWLR["colWLPhone"].ToString();
                    newRowWLR["colWLRoleID"] = rWLR["colRoleID"].ToString();
                    tblWLUserRight.Rows.Add(newRowWLR);

                    DataRow newRowMBR = tblMBUserRight.NewRow();
                    newRowMBR["colMBPhoneNo"] = rWLR["colWLPhone"].ToString();
                    newRowMBR["colMBRoleID"] = rWLR["colRoleID"].ToString();

                    tblMBUserRight.Rows.Add(newRowMBR);
                }
            }
            #endregion

            #region Tạo bảng chứa quyền cho Contract
            //tao bang chua thong tin customer
            DataTable tblContractRoleDetail = new DataTable();
            DataColumn colContractNo = new DataColumn("colContractNo");
            DataColumn colRoleID = new DataColumn("colRoleID");

            //add vào table
            tblContractRoleDetail.Columns.Add(colContractNo);
            tblContractRoleDetail.Columns.Add(colRoleID);

            //tao 1 dong du lieu
            DataTable tblroleContract = new SmartPortal.SEMS.Product().GetRoleOfProduct(ddlProduct.SelectedValue);
            foreach (DataRow rCR in tblroleContract.Rows)
            {
                if (tblContractRoleDetail.Select("colContractNo='" + contractNo + "' and colRoleID='" + rCR["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowCR = tblContractRoleDetail.NewRow();
                    newRowCR["colContractNo"] = contractNo;
                    newRowCR["colRoleID"] = rCR["colRoleID"].ToString();

                    tblContractRoleDetail.Rows.Add(newRowCR);
                }
            }
            #endregion

            #region Tạo bảng chứa Account của Contract

            #region lay tat ca cac account cua khach hang
            //DataSet ds = new DataSet();
            //ds = new SmartPortal.SEMS.Customer().GetAcctNo(txtCustCodeInfo.Text.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
            //if (IPCERRORCODE != "0")
            //{
            //    //goto ERROR;
            //}
            //DataTable dtAccount = new DataTable();
            //dtAccount = ds.Tables[0];
            #endregion

            DataTable tblContractList = (DataTable)ViewState["AccountList"];

            //tao bang chua thong tin account
            DataTable tblContractAccount = new DataTable();
            DataColumn colAContractNo = new DataColumn("colAContractNo");
            DataColumn colAcctNo = new DataColumn("colAcctNo");
            DataColumn colAcctType = new DataColumn("colAcctType");
            DataColumn colCCYID = new DataColumn("colCCYID");
            DataColumn colStatus = new DataColumn("colStatus");
            DataColumn colBranchID = new DataColumn("colBranchID");

            //add vào table
            tblContractAccount.Columns.Add(colAContractNo);
            tblContractAccount.Columns.Add(colAcctNo);
            tblContractAccount.Columns.Add(colAcctType);
            tblContractAccount.Columns.Add(colCCYID);
            tblContractAccount.Columns.Add(colStatus);
            tblContractAccount.Columns.Add(colBranchID);

            //add cung cho giao dich lay account IPC
            foreach (DataRow rCA in tblSUM.Rows)
            {
                if (tblContractAccount.Select("colAContractNo='" + contractNo + "' and colAcctNo='" + rCA["colAccount"].ToString() + "'").Length == 0)
                {
                    DataRow newRowCA = tblContractAccount.NewRow();
                    newRowCA["colAContractNo"] = contractNo;
                    newRowCA["colAcctNo"] = rCA["colAccount"].ToString();
                    DataRow[] ro = tblContractList.Select("ACCOUNTNO='" + rCA["colAccount"].ToString().Trim() + "'");
                    if (ro.Length != 0)
                    {
                        newRowCA["colAcctType"] = ro[0]["ACCOUNTTYPE"].ToString();
                        newRowCA["colCCYID"] = ro[0]["CCYID"].ToString();
                        newRowCA["colStatus"] = ro[0]["STATUS"].ToString();
                        newRowCA["colBranchID"] = SmartPortal.Common.Utilities.Utility.FormatStringCore(ro[0]["BRANCHID"].ToString());
                    }
                    else
                    {
                        //newRowCA["colAcctType"] = "DD";
                        //newRowCA["colCCYID"] = Resources.labels.mmk;
                        //newRowCA["colStatus"] = "A";
                        if (rCA["colServiceID"].ToString() == "WL")
                        {
                            newRowCA["colAcctType"] = "WL";
                            newRowCA["colCCYID"] = Resources.labels.lak;
                            newRowCA["colStatus"] = "A";
                        }
                        else
                        {
                            newRowCA["colAcctType"] = "DD";
                            newRowCA["colCCYID"] = Resources.labels.lak;
                            newRowCA["colStatus"] = "A";
                        }

                    }
                    tblContractAccount.Rows.Add(newRowCA);
                }
            }

            #endregion

            #region Tạo bảng chứa tranright
            DataTable tblTranrightDetail = new DataTable();
            DataColumn colUserIDTR = new DataColumn("colUserID");
            DataColumn colAcctNoTR = new DataColumn("colAcctNoTR");
            DataColumn colTranCode = new DataColumn("colTranCode");
            DataColumn colServiceID = new DataColumn("colServiceID");
            DataColumn colLimit = new DataColumn("colLimit");

            //add vào table
            tblTranrightDetail.Columns.Add(colUserIDTR);
            tblTranrightDetail.Columns.Add(colAcctNoTR);
            tblTranrightDetail.Columns.Add(colTranCode);
            tblTranrightDetail.Columns.Add(colServiceID);
            tblTranrightDetail.Columns.Add(colLimit);

            //add cung cho giao dich lay account IPC
            foreach (DataRow rTR in tblSUM.Rows)
            {
                if (tblTranrightDetail.Select("colUserID='" + rTR["colIBUserName"].ToString() + "' and colAcctNoTR='" + rTR["colAccount"].ToString() + "' and colTranCode='" + rTR["colTranCodeID"].ToString() + "'").Length == 0)
                {
                    if (rTR["colIBUserName"].ToString().Trim() != "")
                    {
                        DataRow newRowTR = tblTranrightDetail.NewRow();
                        newRowTR["colUserID"] = rTR["colIBUserName"].ToString();
                        newRowTR["colAcctNoTR"] = rTR["colAccount"].ToString();
                        newRowTR["colTranCode"] = rTR["colTranCodeID"].ToString();
                        newRowTR["colServiceID"] = rTR["colServiceID"].ToString();
                        newRowTR["colLimit"] = "0";

                        tblTranrightDetail.Rows.Add(newRowTR);
                    }
                }
            }
            #endregion

            #region Tạo bảng chứa UserAccount
            DataTable tblUserAccount = new DataTable();
            DataColumn colUserIDUC = new DataColumn("colUserIDUC");
            DataColumn colAcctNoUC = new DataColumn("colAcctNoUC");
            DataColumn colRoleIDUC = new DataColumn("colRoleIDUC");
            DataColumn colUseFull = new DataColumn("colUseFull");
            DataColumn colDesc = new DataColumn("colDesc");
            DataColumn colIsDefault = new DataColumn("colIsDefault");
            DataColumn colSst = new DataColumn("colStatus");

            //add vào table
            tblUserAccount.Columns.Add(colUserIDUC);
            tblUserAccount.Columns.Add(colAcctNoUC);
            tblUserAccount.Columns.Add(colRoleIDUC);
            tblUserAccount.Columns.Add(colUseFull);
            tblUserAccount.Columns.Add(colDesc);
            tblUserAccount.Columns.Add(colIsDefault);
            tblUserAccount.Columns.Add(colSst);

            //add cung cho giao dich lay account IPC
            foreach (DataRow rUA in tblSUM.Rows)
            {
                if (tblUserAccount.Select("colUserIDUC='" + rUA["colIBUserName"].ToString() + "' and colAcctNoUC='" + rUA["colAccount"].ToString() + "' and colRoleIDUC='" + rUA["colRoleID"].ToString() + "'").Length == 0)
                {
                    DataRow newRowUA = tblUserAccount.NewRow();
                    newRowUA["colUserIDUC"] = rUA["colIBUserName"].ToString();
                    newRowUA["colAcctNoUC"] = rUA["colAccount"].ToString();
                    newRowUA["colRoleIDUC"] = rUA["colRoleID"].ToString();
                    newRowUA["colUseFull"] = "N";
                    newRowUA["colDesc"] = "";
                    newRowUA["colIsDefault"] = "Y";
                    newRowUA["colStatus"] = "Y";

                    tblUserAccount.Rows.Add(newRowUA);
                }
            }
            #endregion

            #region vutt Tạo bảng chứa thông tin sms notify 04022016

            DataTable tblSMSNotify = ContractControl.CreateSMSNotifyDetailTable(tblUserAccount, tblContractAccount, "I", contractNo);

            //return;
            #endregion

            #region INSERT
            //new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount,null,null, ref IPCERRORCODE, ref IPCERRORDESC);
            //08.12.2015 minh add corptype add value=2 for individual
            //new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, (chkRenew.Checked ? "Y" : "N"),SmartPortal.Constant.IPC.CONTRACTINDIVIDUAL, tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount, null, null, ref IPCERRORCODE, ref IPCERRORDESC);
            //19042016 vutt add smsnotify table
            //new SmartPortal.SEMS.Contract().InsertCustomerExists(branchID, "", "", custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, (chkRenew.Checked ? "Y" : "N"), SmartPortal.Constant.IPC.CONTRACTINDIVIDUAL, tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount, null, null, tblSMSNotify, ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE != "0")
            {
                throw new IPCException(IPCERRORDESC);
            }
            else
            {
                SendInfoLogin();
                ReleaseSession();
            }

            #endregion


        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnCustSave_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractList_Add_Widget", "btnCustSave_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }

        Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=183&returnURL=" + SmartPortal.Common.Encrypt.EncryptData(Request.Url.Query)));
    }
    protected void btnContractPrevious_Click(object sender, EventArgs e)
    {
        pnPersonal.Visible = false;
        pnOption.Visible = false;
        pnCustInfo.Visible = true;
        pnContractInfo.Visible = false;
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultChuTaiKhoan = (DataTable)ViewState["CHUTAIKHOAN"];

            tblResultChuTaiKhoan.Rows.Clear();

            ViewState["CHUTAIKHOAN"] = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataSource = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataBind();

            lblAlert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnHuyDSH_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultNguoiUyQuyen = (DataTable)ViewState["NGUOIUYQUYEN"];

            tblResultNguoiUyQuyen.Rows.Clear();

            ViewState["NGUOIUYQUYEN"] = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataSource = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataBind();

            lblAlertDSH.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnCoreownerDetail_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCoownerCode.Text.Trim().Equals(string.Empty))
            {
                ShowPopUpMsg(Resources.labels.makhachhangkhonghople);
                txtCoownerCode.Focus();
                return;
            }
            lblError.Text = "";

            txtFullnameNguoiUyQuyen.Text = string.Empty;
            txtEmailNguoiUyQuyen.Text = string.Empty;
            txtPhoneNguoiUyQuyen.Text = string.Empty;
            ddlGenderNguoiUyQuyen.SelectedIndex = 0;
            txtBirthNguoiUyQuyen.Text = string.Empty;
            txtAddressNguoiUyQuyen.Text = string.Empty;

            Hashtable hasCoownerInfo = new Hashtable();
            string ctmType = SmartPortal.Constant.IPC.PERSONAL;
            hasCoownerInfo = new SmartPortal.SEMS.Customer().GetCustInfo(txtCoownerCode.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (hasCoownerInfo[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                lblError.Text = Resources.labels.thiscustomerdoesnotexists;
                return;
            }

            if (hasCoownerInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtFullnameNguoiUyQuyen.Text = hasCoownerInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (hasCoownerInfo[SmartPortal.Constant.IPC.EMAIL] != null)
                txtEmailNguoiUyQuyen.Text = hasCoownerInfo[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (hasCoownerInfo[SmartPortal.Constant.IPC.PHONE] != null)
            {
                string phoneNguoiUyQuyen = hasCoownerInfo[SmartPortal.Constant.IPC.PHONE].ToString();
                PHONENUQ = phoneNguoiUyQuyen;
                if (!CheckExistPhoneNumber(PHONENUQ))
                {
                    lblError.Text = Resources.labels.phonenumberisalreadyregistered + " co-owner";
                    btnNext.Visible = false;
                }
                if (!CheckIsPhoneNumer(PHONENUQ))
                {
                    lblError.Text = Resources.labels.phonenumberwrong + " co-owner";
                }
                txtPhoneNguoiUyQuyen.Text = PHONENUQ;
            }

            if (hasCoownerInfo[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    if (hasCoownerInfo[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddlGenderNguoiUyQuyen.Enabled = true;
                    }
                    else
                    {
                        ddlGenderNguoiUyQuyen.Enabled = false;
                        //17.9.2015 minhmodify
                        //ddlGenderNguoiUyQuyen.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(hasCoownerInfo[SmartPortal.Constant.IPC.SEX].ToString()).ToString());
                        ddlGenderNguoiUyQuyen.SelectedValue = hasCoownerInfo[SmartPortal.Constant.IPC.SEX].ToString();
                    }
                }
                catch
                {
                }
            }
            if (hasCoownerInfo[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    //17.9.2015 minh modify
                    //string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(hasCoownerInfo[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
                    string birthDate = hasCoownerInfo[SmartPortal.Constant.IPC.DOB].ToString();
                    txtBirthNguoiUyQuyen.Text = birthDate.Equals("01/01/1900") ? "" : birthDate;

                    if (txtBirthNguoiUyQuyen.Text.Trim() == "")
                    {
                        txtBirthNguoiUyQuyen.Enabled = true;
                    }
                    else
                    {
                        txtBirthNguoiUyQuyen.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasCoownerInfo[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtAddressNguoiUyQuyen.Text = hasCoownerInfo[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }
            //11/9/2015 minh add this to generate usercoowner and mbusercoowner
            if (hasCoownerInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
            {
                txtMBUserNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID(hasCoownerInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString(), txtCoownerCode.Text.Trim(), hasCoownerInfo[SmartPortal.Constant.IPC.LICENSE].ToString(), txtIBGenUserName.Text);
                //if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
                //{
                //    //txtMBUsersName.Text = ownerUserName;
                //    txtMBUserNguoiUyQuyen.Text = txtIBGenUserNameNguoiUyQuyen.Text;
                //}
                //else
                //{
                //    //txtMBUsersName.Text = SmartPortal.Common.Utilities.Utility.GetID("", txtCustCodeInfo.Text.Trim(), "", 10) + "1";
                //    txtMBUserNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", txtCoownerCode.Text.Trim(), "", 10) + "2";
                //}
            }

        }
        catch (Exception ex)
        {

        }
    }
    private void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }
    private bool validateusername(TextBox tx)
    {
        string usernamepattern = System.Configuration.ConfigurationManager.AppSettings["validateusername"].ToString();
        if (!(System.Text.RegularExpressions.Regex.IsMatch(tx.Text, usernamepattern)))
        {

            ShowPopUpMsg(Resources.labels.tendangnhapchichophepsovachulatin);

            tx.Focus();
            return false;

        }
        return true;
    }


    protected void ddlpolicyIB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlpolicyMB.SelectedValue = ddlpolicyIB.SelectedValue;
        }
        catch
        { }
    }
    protected void ddlpolicyIBco_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlpolicyMBco.SelectedValue = ddlpolicyIBco.SelectedValue;
        }
        catch
        { }
    }
    protected void ddlpolicyMBco_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlpolicyWLco.SelectedValue = ddlpolicyMBco.SelectedValue;
        }
        catch
        { }
    }
    public bool CheckIsPhoneNumer(string phone)
    {
        string result = new Customer().CheckPhoneTeLCo(phone, ref IPCERRORCODE, ref IPCERRORDESC);
        if (result == SmartPortal.Constant.IPC.TRANSTATUS.BEGIN)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckExistPhoneNumber(string phone)
    {
        string resultInfo = new Customer().CheckPhoneNumberCustInfo(phone,SmartPortal.Constant.IPC.CONTRACTINDIVIDUAL, ref IPCERRORCODE, ref IPCERRORDESC);
        if (resultInfo.Equals("1"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
