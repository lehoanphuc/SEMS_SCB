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
using SmartPortal.Model;
using System.Reflection;


public partial class Widgets_SEMSContractListCorp_Add_Widget : WidgetBase
{
    int i = 0;
    string IPCERRORCODE = "";
    string IPCERRORDESC = "";
    string userName;
    internal int minlength = int.Parse(ConfigurationManager.AppSettings["minlengthloginname"].ToString());
    internal int maxlength = int.Parse(ConfigurationManager.AppSettings["maxlengthloginname"].ToString());
    static List<string> lsAccNo = new List<string>();


    protected void Page_Init(object sender, EventArgs e)
    {
        TabCustomerInfoHelper.LoadConfig();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {



            lblError.Text = "";
            lblAlertQTHT.Text = "";
            lblAlertCTK.Text = "";
            lblAlertNUY.Text = "";
            lblAlertL2.Text = "";
            ddlCustType.Items.Add(new ListItem(Resources.labels.doanhnghiep, "O"));
            ddlCustType.Items.Add(new ListItem(Resources.labels.canhan, "P"));
            ddlCustType.Items.Add(new ListItem(Resources.labels.linkage, "J"));

            if (!IsPostBack)
            {

                //ddlGenderLevel2.Items.Add(new ListItem(Resources.labels.male, "M"));
                //ddlGenderLevel2.Items.Add(new ListItem(Resources.labels.female, "F"));
                //ddlGenderQT.Items.Add(new ListItem(Resources.labels.male, "M"));
                //ddlGenderQT.Items.Add(new ListItem(Resources.labels.female, "F"));

                //ddlReGender.Items.Add(new ListItem(Resources.labels.male, "M"));
                //ddlReGender.Items.Add(new ListItem(Resources.labels.female, "F"));

                //ddlGenderNguoiUyQuyen.Items.Add(new ListItem(Resources.labels.male, "M"));
                //ddlGenderNguoiUyQuyen.Items.Add(new ListItem(Resources.labels.female, "F"));

                //ddlGenderLevel2.Items.Add(new ListItem(Resources.labels.male, "M"));
                //ddlGenderLevel2.Items.Add(new ListItem(Resources.labels.female, "F"));
                #region Ẩn các panel
                pnOption.Visible = true;
                pnCustInfo.Visible = false;
                pnContractInfo.Visible = false;
                pnPersonal.Visible = false;
                pnPersonalSimple.Visible = false;
                pnLuu.Visible = false;
                //lannth - 121018 - an panel corp matrix
                pnCorpMatrix.Visible = false;

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
                        if (r["serviceid"].ToString().Trim() == "IB")
                        {
                            ibrow = ibrow + 1;
                        }
                        if (r["serviceid"].ToString().Trim() == "SMS")
                        {
                            smsrow = smsrow + 1;
                        }
                        if (r["serviceid"].ToString().Trim() == "MB")
                        {
                            mbrow = mbrow + 1;
                        }
                    }
                    //check  policy
                    if (ibrow == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "IB");
                        btnStart.Enabled = false;
                        return;
                    }
                    if (smsrow == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "SMS");
                        btnStart.Enabled = false;
                        return;
                    }
                    if (mbrow == 0)
                    {
                        lblError.Text = string.Format(Resources.labels.bancanthempolicychodichvutruoc, "MB");
                        btnStart.Enabled = false;
                        return;
                    }
                    DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                    DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                    DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();

                    //advance contract
                    ddlpolicyIB.DataSource = dtIB;
                    ddlpolicyIBqt.DataSource = dtIB;
                    ddlpolicyIBau.DataSource = dtIB;
                    ddlpolicyIBl2.DataSource = dtIB;

                    ddlpolicyIB.DataTextField = "policytx";
                    ddlpolicyIB.DataValueField = "policyid";
                    ddlpolicyIBqt.DataTextField = "policytx";
                    ddlpolicyIBqt.DataValueField = "policyid";
                    ddlpolicyIBau.DataTextField = "policytx";
                    ddlpolicyIBau.DataValueField = "policyid";
                    ddlpolicyIBl2.DataTextField = "policytx";
                    ddlpolicyIBl2.DataValueField = "policyid";
                    ddlpolicyIB.DataBind();
                    ddlpolicyIBqt.DataBind();
                    ddlpolicyIBau.DataBind();
                    ddlpolicyIBl2.DataBind();
                    //sms advance
                    ddlpolicySMS.DataSource = dtSMS;
                    ddlpolicySMSqt.DataSource = dtSMS;
                    ddlpolicySMSau.DataSource = dtSMS;
                    ddlpolicySMSl2.DataSource = dtSMS;
                    ddlpolicySMS.DataTextField = "policytx";
                    ddlpolicySMS.DataValueField = "policyid";
                    ddlpolicySMSqt.DataTextField = "policytx";
                    ddlpolicySMSqt.DataValueField = "policyid";
                    ddlpolicySMSau.DataTextField = "policytx";
                    ddlpolicySMSau.DataValueField = "policyid";
                    ddlpolicySMSl2.DataTextField = "policytx";
                    ddlpolicySMSl2.DataValueField = "policyid";
                    ddlpolicySMS.DataBind();
                    ddlpolicySMSqt.DataBind();
                    ddlpolicySMSau.DataBind();
                    ddlpolicySMSl2.DataBind();
                    //mb advance
                    ddlpolicyMB.DataSource = dtMB;
                    ddlpolicyMBqt.DataSource = dtMB;
                    ddlpolicyMBau.DataSource = dtMB;
                    ddlpolicyMBl2.DataSource = dtMB;
                    ddlpolicyMB.DataTextField = "policytx";
                    ddlpolicyMB.DataValueField = "policyid";
                    ddlpolicyMBqt.DataTextField = "policytx";
                    ddlpolicyMBqt.DataValueField = "policyid";
                    ddlpolicyMBau.DataTextField = "policytx";
                    ddlpolicyMBau.DataValueField = "policyid";
                    ddlpolicyMBl2.DataTextField = "policytx";
                    ddlpolicyMBl2.DataValueField = "policyid";
                    ddlpolicyMB.DataBind();
                    ddlpolicyMBqt.DataBind();
                    ddlpolicyMBau.DataBind();
                    ddlpolicyMBl2.DataBind();
                    //simple contract:
                    ddlpolicyIBs.DataSource = dtIB;
                    ddlpolicyIBsl4.DataSource = dtIB;
                    ddlpolicyIBsl5.DataSource = dtIB;
                    ddlpolicyIBs.DataTextField = "policytx";
                    ddlpolicyIBs.DataValueField = "policyid";
                    ddlpolicyIBsl4.DataTextField = "policytx";
                    ddlpolicyIBsl4.DataValueField = "policyid";
                    ddlpolicyIBsl5.DataTextField = "policytx";
                    ddlpolicyIBsl5.DataValueField = "policyid";
                    ddlpolicyIBs.DataBind();
                    ddlpolicyIBsl4.DataBind();
                    ddlpolicyIBsl5.DataBind();
                    //sms simple
                    ddlpolicySMSs.DataSource = dtSMS;
                    ddlpolicySMSsl4.DataSource = dtSMS;
                    ddlpolicySMSsl5.DataSource = dtSMS;
                    ddlpolicySMSs.DataTextField = "policytx";
                    ddlpolicySMSs.DataValueField = "policyid";
                    ddlpolicySMSsl4.DataTextField = "policytx";
                    ddlpolicySMSsl4.DataValueField = "policyid";
                    ddlpolicySMSsl5.DataTextField = "policytx";
                    ddlpolicySMSsl5.DataValueField = "policyid";
                    ddlpolicySMSs.DataBind();
                    ddlpolicySMSsl4.DataBind();
                    ddlpolicySMSsl5.DataBind();
                    //mbsimple
                    ddlpolicyMBs.DataSource = dtMB;
                    ddlpolicyMBsl4.DataSource = dtMB;
                    ddlpolicyMBsl5.DataSource = dtMB;
                    ddlpolicyMBs.DataTextField = "policytx";
                    ddlpolicyMBs.DataValueField = "policyid";
                    ddlpolicyMBsl4.DataTextField = "policytx";
                    ddlpolicyMBsl4.DataValueField = "policyid";
                    ddlpolicyMBsl5.DataTextField = "policytx";
                    ddlpolicyMBsl5.DataValueField = "policyid";
                    ddlpolicyMBs.DataBind();
                    ddlpolicyMBsl4.DataBind();
                    ddlpolicyMBsl5.DataBind();
                    //disable policy user phu-> dung chung policy voi user chinh
                    //ddlpolicyIB.Enabled = false;
                    //ddlpolicyIBau.Enabled = false;
                    //ddlpolicyIBl2.Enabled = false;
                    //ddlpolicyIBsl4.Enabled = false;
                    //ddlpolicyIBsl5.Enabled = false;
                    ddlpolicyMBs.Enabled = false;
                    ddlpolicyMBsl4.Enabled = false;
                    ddlpolicyMBsl5.Enabled = false;

                }

            }
            else if (ddlCorpType.SelectedValue.Equals(SmartPortal.Constant.IPC.CONTRACTCORPMATRIX))
            {
                loadCorpMatrix();
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

    protected void btnStart_Click(object sender, EventArgs e)
    {

        if (radCustExists.Checked)
        {
            pnOption.Visible = false;
            pnCustInfo.Visible = true;
            pnContractInfo.Visible = false;


            //search customer
            BindData();
        }
        else
        {
            Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=225"));
        }

    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            RadioButton cbxSelect;
            foreach (GridViewRow gvr in gvCustomerList.Rows)
            {
                cbxSelect = (RadioButton)gvr.Cells[0].FindControl("cbxSelect");
                if (cbxSelect.Checked == true)
                {
                    hidCustID.Value = ((HyperLink)gvr.Cells[1].FindControl("lblCustCode")).Text;
                    ViewState["cusID"] = ((HyperLink)gvr.Cells[1].FindControl("lblCustCode")).Text;
                    break;
                }
            }


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

            #region load usertype
            DataSet dsUserType = new DataSet();
            DataTable dtUserType = new DataTable();

            dtUserType = new SmartPortal.SEMS.Contract().LoadContractType(SmartPortal.Constant.IPC.CORPORATE, "Y");

            ddlContractType.DataSource = dtUserType;
            ddlContractType.DataTextField = "TYPENAME";
            ddlContractType.DataValueField = "USERTYPE";
            ddlContractType.DataBind();

            #endregion

            #region lay thong tin khach hang de chon loai hinh san pham
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
                ddlProduct.DataSource = new SmartPortal.SEMS.Product().GetProductByCondition("", "", SmartPortal.Constant.IPC.CORPORATE, "", ref IPCERRORCODE, ref IPCERRORDESC);
                ddlProduct.DataTextField = "PRODUCTNAME";
                ddlProduct.DataValueField = "PRODUCTID";
                ddlProduct.DataBind();
                //15/9/2015 minh assign ddlcustype.selectedvalue
                ddlCustType.SelectedValue = dtCust.Rows[0]["CFTYPE"].ToString().Trim();
                hidCFcodeS.Value = dtCust.Rows[0]["cfcode"].ToString().Trim();
            }
            #endregion

            #region hien thi mac dinh ma hop dong
            //txtContractNo.Text = SmartPortal.Constant.IPC.CONTRACTNOPREFIX + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8);
            //hien thi ma hop dong
            //txtContractNo.Text = SmartPortal.Common.Utilities.Utility.GetID("HD", dtCust.Rows[0]["CUSTCODE"].ToString().Trim(), "O",15);
            //MA HD
            txtContractNo.Text = SmartPortal.Common.Utilities.Utility.GetID(SmartPortal.Constant.IPC.CONTRACTNOPREFIX, dtCust.Rows[0]["CUSTCODE"].ToString().Trim(), "O", 15);



            txtContractNo.Enabled = false;
            txtStartDate.Text = Utility.FormatDatetime(SmartPortal.Constant.IPC.LoadWorkingDate(), "dd/MM/yyyy", DateTimeStyle.Date);
            txtEndDate.Text = Utility.FormatDatetime(SmartPortal.Common.Utilities.Utility.IsDateTime1(SmartPortal.Constant.IPC.LoadWorkingDate()).AddYears(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeStyle.Date);
            #endregion

            //lannth - 121018 - an panel corp matrix
            pnCorpMatrix.Visible = false;

            //an cac panel
            pnOption.Visible = false;
            pnCustInfo.Visible = false;
            pnContractInfo.Visible = true;
            pnPersonal.Visible = false;

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

    protected void btnContractNext_Click(object sender, EventArgs e)
    {

        try
        {
            lblError.Text = "";

            //check ngay thang
            if (SmartPortal.Common.Utilities.Utility.IsDateTimeViet(txtStartDate.Text.Trim()))
            {
            }
            else
            {
                lblError.Text = Resources.labels.datetimeinvalid;
                return;
            }

            //lannth - 121018 - neu corp type laf matrix, load user control rieng.
            if (ddlCorpType.SelectedValue.Equals(SmartPortal.Constant.IPC.CONTRACTCORPMATRIX))
            {
                pnPersonal.Visible = false;
                pnPersonalSimple.Visible = false;
                pnCorpMatrix.Visible = true;
                pnCorpMatrix.Controls.Clear();
                loadCorpMatrix();
            }
            else
            {
                //lannth - an panel corp matrix
                pnCorpMatrix.Visible = false;

                //checkbox
                radAllAccount.Checked = true;
                radAllAccountNguoiUyQuyen.Checked = true;
                radAllAccountLevel2.Checked = true;
                radAllAccountQT.Checked = true;

                //release session
                ReleaseSession();
                //huy du lieu luoi
                gvResultChuTaiKhoan.DataSource = null;
                gvResultChuTaiKhoan.DataBind();

                gvResultNguoiUyQuyen.DataSource = null;
                gvResultNguoiUyQuyen.DataBind();

                gvResultLevel2.DataSource = null;
                gvResultLevel2.DataBind();

                gvResultQuanTri.DataSource = null;
                gvResultQuanTri.DataBind();

                //diable account
                ddlAccount.Enabled = false;
                ddlAccountUyQuyen.Enabled = false;
                ddlAccountLevel2.Enabled = false;
                ddlAccountQT.Enabled = false;

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
                        cc = dtCustInfo.Rows[0]["CFCODE"].ToString().Trim();
                        ct = dtCustInfo.Rows[0]["CFTYPE"].ToString().Trim();
                    }
                    else
                    {
                        throw new IPCException(IPCERRORCODE);
                    }


                }
                else
                {
                    throw new IPCException(IPCERRORCODE);
                }



                //string strCode = cc.Trim() + ct.Trim() + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 4);
                //08.12.2015 minh bo generate user:
                //txtIBUserName.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), ct.Trim(),12) + "1";
                //txtUserNameNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), ct.Trim(),12) + "2";
                //txtUserNameLevel2.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), ct.Trim(),12) + "3";
                //txtIBUserNameQT.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), ct.Trim(),12) + "4";

                //txtMBPhoneQT.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "1";
                //txtPHOPhoneQT.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "1";

                //txtMBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "2";
                //txtPHOPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "2";

                //txtMBPhoneNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "3";
                //txtPHOPhoneNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "3";

                //txtMBPhoneLevel2.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "4";
                //txtPHOPhoneLevel2.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc.Trim(), "",10) + "4";


                btnResetNguoiUyQuyen.Attributes.Add("onclick", "return SetUserNameNUY('" + cc.Trim() + "','" + ct.Trim() + "',12)");
                btnResetLevel2.Attributes.Add("onclick", "return SetUserNameL2('" + cc.Trim() + "','" + ct.Trim() + "',12)");


                txtIBUserName.Enabled = false;
                txtUserNameNguoiUyQuyen.Enabled = false;
                txtUserNameLevel2.Enabled = false;
                txtIBUserNameQT.Enabled = false;

                #region Người quản trị
                lblError.Text = "";
                //simple corp
                txtIBGenUserNameS.Enabled = false;
                txtlv4IBGenUserName.Enabled = false;
                txtlv5IBGenUserName.Enabled = false;

                txtIBGenUserNameS.Enabled = false;
                txtlv4IBGenUserName.Enabled = false;
                txtlv5IBGenUserName.Enabled = false;
                radAllAccountS.Checked = true;
                radlv4AllAccount.Checked = true;
                radlv5AllAccount.Checked = true;

                gvResultChuTaiKhoanS.DataSource = null;
                gvResultChuTaiKhoanS.DataBind();

                gvResultlv4.DataSource = null;
                gvResultlv4.DataBind();
                ddlAccountS.Enabled = false;
                ddllv4Account.Enabled = false;
                ddllv5Account.Enabled = false;
                //02.12.2015 minh bỏ phần generate lv4,5, tạo lại owner username

                txtIBGenUserNameS.Text = SmartPortal.Common.Utilities.Utility.GetID(dtCustInfo.Rows[0]["FULLNAME"].ToString().Trim(), dtCustInfo.Rows[0][SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), dtCustInfo.Rows[0]["LICENSEID"].ToString().Trim()) + "O1";
                txtIBUserName.Text = SmartPortal.Common.Utilities.Utility.GetID(dtCustInfo.Rows[0]["FULLNAME"].ToString().Trim(), dtCustInfo.Rows[0][SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), dtCustInfo.Rows[0]["LICENSEID"].ToString().Trim()) + "O1";
                if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
                {

                    txtMBPhoneNoS.Text = txtIBGenUserNameS.Text;
                    txtPHOPhoneNoS.Text = txtMBPhoneNoS.Text;
                    txtMBPhoneNo.Text = txtIBUserName.Text;
                    txtPHOPhoneNo.Text = txtMBPhoneNo.Text;
                }
                else
                {

                    txtMBPhoneNoS.Text = SmartPortal.Common.Utilities.Utility.GetID("", dtCustInfo.Rows[0][SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), "", 10) + "O1";
                    txtPHOPhoneNoS.Text = txtMBPhoneNoS.Text;
                    txtMBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", dtCustInfo.Rows[0][SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), "", 10) + "O1";
                    txtPHOPhoneNo.Text = txtMBPhoneNo.Text;
                }
                //txtIBGenUserNameS.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "O", 12) + "1";
                //txtlv4IBGenUserName.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "O", 12) + "2";
                //txtlv5IBGenUserName.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "O", 12) + "3";

                //txtMBPhoneNoS.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "2";
                //txtPHOPhoneNoS.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "2";

                //txtlv4MBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "5";
                //txtlv4PHOPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "5";

                //txtlv5MBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "5";
                //txtlv5PHOPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", cc, "", 10) + "5";
                //txtIBGenUserNameS.Enabled = false;
                //txtlv4IBGenUserName.Enabled = false;
                //txtlv5IBGenUserName.Enabled = false;






                #region lay tat ca cac account cua khach hang


                DataSet ds = new DataSet();
                ds = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ct, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtAccount = new DataTable();
                dtAccount = ds.Tables[0];
                ViewState["AccountList"] = dtAccount;

                if (dtAccount.Rows.Count == 0)
                {
                    lblError.Text = "Khách hàng này không tồn tại tài khoản để đăng ký.";
                    return;
                }
                else
                {
                    ddlDefaultAccountQT.DataSource = dtAccount;
                    ddlDefaultAccountQT.DataTextField = "ACCOUNTNO";
                    ddlDefaultAccountQT.DataValueField = "ACCOUNTNO";
                    ddlDefaultAccountQT.DataBind();

                    ddlAccountQT.DataSource = dtAccount;
                    ddlAccountQT.DataTextField = "ACCOUNTNO";
                    ddlAccountQT.DataValueField = "ACCOUNTNO";
                    ddlAccountQT.DataBind();


                    ddlQTHTDefaultAcctno.DataSource = dtAccount;
                    ddlQTHTDefaultAcctno.DataTextField = "ACCOUNTNO";
                    ddlQTHTDefaultAcctno.DataValueField = "ACCOUNTNO";
                    ddlQTHTDefaultAcctno.DataBind();

                    //ddlAccount.Items.Insert(0,new ListItem("----------Chọn tài khoản----------",""));

                }
                #endregion

                #region Hien thi tat cac cac role theo serviceid va usertype len cay
                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIBQT, SmartPortal.Constant.IPC.QUANTRIHETHONG);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSQT, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSQT, SmartPortal.Constant.IPC.QUANTRIHETHONG);

                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBQT, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBQT, SmartPortal.Constant.IPC.QUANTRIHETHONG);


                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOQT, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOQT, SmartPortal.Constant.IPC.QUANTRIHETHONG);

                #endregion

                #region Xóa hết tất cả role chọn
                //foreach (ListItem liIB in cblIB.Items)
                //{

                //    liIB.Selected = false;

                //}
                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvIBQT, tvSMSQT, tvMBQT, tvPHOQT);
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

                #region Chủ tài khoản
                lblError.Text = "";

                #region lay tat ca cac account cua khach hang
                //lay thong tin ma khach hang tu database


                ddlSMSDefaultAcctno.DataSource = dtAccount;
                ddlSMSDefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlSMSDefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlSMSDefaultAcctno.DataBind();

                ddlAccount.DataSource = dtAccount;
                ddlAccount.DataTextField = "ACCOUNTNO";
                ddlAccount.DataValueField = "ACCOUNTNO";
                ddlAccount.DataBind();

                //phongtt sms notification
                lsAccNo.Clear();
                foreach (DataRow dr in dtAccount.Rows)
                {
                    if (!lsAccNo.Contains(dr["ACCOUNTNO"].ToString()))
                    {
                        lsAccNo.Add(dr["ACCOUNTNO"].ToString());
                    }
                }


                ddlCTKDefaultAcctno.DataSource = dtAccount;
                ddlCTKDefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlCTKDefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlCTKDefaultAcctno.DataBind();

                //simple
                ddlSMSDefaultAcctnoS.DataSource = dtAccount;
                ddlSMSDefaultAcctnoS.DataTextField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoS.DataValueField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoS.DataBind();

                ddlCTKPHODefaultAcctnoS.DataSource = dtAccount;
                ddlCTKPHODefaultAcctnoS.DataTextField = "ACCOUNTNO";
                ddlCTKPHODefaultAcctnoS.DataValueField = "ACCOUNTNO";
                ddlCTKPHODefaultAcctnoS.DataBind();

                ddlAccountS.DataSource = dtAccount;
                ddlAccountS.DataTextField = "ACCOUNTNO";
                ddlAccountS.DataValueField = "ACCOUNTNO";
                ddlAccountS.DataBind();

                //phongtt sms notification
                lsAccNo.Clear();
                foreach (DataRow dr in dtAccount.Rows)
                {
                    if (!lsAccNo.Contains(dr["ACCOUNTNO"].ToString()))
                    {
                        lsAccNo.Add(dr["ACCOUNTNO"].ToString());
                    }
                }


                //ddlAccount.Items.Insert(0,new ListItem("----------Chọn tài khoản----------",""));


                #endregion

                //#region Lay thong tin chu tai khoan
                //Hashtable hasOwner = new SmartPortal.IB.Customer().GetOwner(ddlAccount.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                //if (IPCERRORCODE == "0")
                //{
                //    try
                //    {
                //        txtReFullName.Text = hasOwner[SmartPortal.Constant.IPC.CUSTOMERNAME].ToString();

                //        if (txtReFullName.Text.Trim() == "")
                //        {
                //            txtReFullName.Enabled = true;
                //        }
                //        else
                //        {
                //            txtReFullName.Enabled = false;
                //        }
                //        // simple
                //        txtReFullNameS.Text = hasOwner[SmartPortal.Constant.IPC.CUSTOMERNAME].ToString();
                //        if (txtReFullNameS.Text.Trim() == "")
                //        {
                //            txtReFullNameS.Enabled = true;
                //        }
                //        else
                //        {
                //            txtReFullNameS.Enabled = false;
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
                //else
                //{
                //    //throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.IPC);
                //}
                //#endregion
                //15.9.2015  minh thay doi cach lay thong tin chu tai khoan
                #region lay thong tin chu tai khoan
                txtReFullName.Text = dtCustInfo.Rows[0]["FULLNAME"].ToString();
                txtReBirth.Text = SmartPortal.Common.Utilities.Utility.IsDateTime2(dtCustInfo.Rows[0]["DOB"].ToString()).ToString("dd/MM/yyyy");

                if (dtCustInfo.Rows[0]["SEX"].ToString().Trim().Equals("M") || dtCustInfo.Rows[0]["SEX"].ToString().Trim().Equals("F"))
                {
                    ddlReGender.SelectedValue = dtCustInfo.Rows[0]["SEX"].ToString();
                    ddlReGenderS.SelectedValue = dtCustInfo.Rows[0]["SEX"].ToString();
                }
                else
                {

                    ddlReGender.SelectedValue = "F";
                    ddlReGenderS.SelectedValue = "F";
                }
                txtReMobi.Text = dtCustInfo.Rows[0]["TEL"].ToString();
                txtReEmail.Text = dtCustInfo.Rows[0]["EMAIL"].ToString();
                txtReAddress.Text = dtCustInfo.Rows[0]["ADDR_RESIDENT"].ToString();
                //simple
                txtReFullNameS.Text = dtCustInfo.Rows[0]["FULLNAME"].ToString();
                txtReBirthS.Text = SmartPortal.Common.Utilities.Utility.IsDateTime2(dtCustInfo.Rows[0]["DOB"].ToString()).ToString("dd/MM/yyyy");

                txtReMobiS.Text = dtCustInfo.Rows[0]["TEL"].ToString();
                txtReEmailS.Text = dtCustInfo.Rows[0]["EMAIL"].ToString();
                txtReAddressS.Text = dtCustInfo.Rows[0]["ADDR_RESIDENT"].ToString();
                #endregion
                #region Hien thi tat cac cac role theo serviceid va usertype len cay
                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIB, SmartPortal.Constant.IPC.CHUTAIKHOAN);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMS, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMS, SmartPortal.Constant.IPC.CHUTAIKHOAN);
                //

                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMB, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMB, SmartPortal.Constant.IPC.CHUTAIKHOAN);
                //

                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHO, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHO, SmartPortal.Constant.IPC.CHUTAIKHOAN);
                //simple-----------------

                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIBS, SmartPortal.Constant.IPC.CHUTAIKHOAN);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMS, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSS, SmartPortal.Constant.IPC.CHUTAIKHOAN);


                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMB, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBS, SmartPortal.Constant.IPC.CHUTAIKHOAN);


                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHO, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOS, SmartPortal.Constant.IPC.CHUTAIKHOAN);
                #endregion

                #region Xóa hết tất cả role chọn
                //foreach (ListItem liIB in cblIB.Items)
                //{

                //    liIB.Selected = false;

                //}
                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvIB, tvSMS, tvMB, tvPHO);
                //simple
                GetRoleDefault(tvIBS, tvSMSS, tvMBS, tvPHOS);
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
                //lay thong tin ma khach hang tu database

                ddlSMSDefaultAcctnoUyQuyen.DataSource = dtAccount;
                ddlSMSDefaultAcctnoUyQuyen.DataTextField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoUyQuyen.DataValueField = "ACCOUNTNO";
                ddlSMSDefaultAcctnoUyQuyen.DataBind();

                ddlAccountUyQuyen.DataSource = dtAccount;
                ddlAccountUyQuyen.DataTextField = "ACCOUNTNO";
                ddlAccountUyQuyen.DataValueField = "ACCOUNTNO";
                ddlAccountUyQuyen.DataBind();

                ddlNUQDefaultAcctno.DataSource = dtAccount;
                ddlNUQDefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlNUQDefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlNUQDefaultAcctno.DataBind();

                //ddlAccount.Items.Insert(0,new ListItem("----------Chọn tài khoản----------",""));


                #endregion

                #region Hien thi tat cac cac role theo serviceid va usertype len cay
                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIBUyQuyen, SmartPortal.Constant.IPC.NGUOIUYQUYEN);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSUyQuyen, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSUyQuyen, SmartPortal.Constant.IPC.NGUOIUYQUYEN);


                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBUyQuyen, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBUyQuyen, SmartPortal.Constant.IPC.NGUOIUYQUYEN);


                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOUyQuyen, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOUyQuyen, SmartPortal.Constant.IPC.NGUOIUYQUYEN);

                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen);
                #endregion

                #endregion

                #region Người dùng Level 2

                #region lay tat ca cac account cua khach hang
                //lay thong tin ma khach hang tu database

                ddlDefaultAccountLevel2.DataSource = dtAccount;
                ddlDefaultAccountLevel2.DataTextField = "ACCOUNTNO";
                ddlDefaultAccountLevel2.DataValueField = "ACCOUNTNO";
                ddlDefaultAccountLevel2.DataBind();

                ddlAccountLevel2.DataSource = dtAccount;
                ddlAccountLevel2.DataTextField = "ACCOUNTNO";
                ddlAccountLevel2.DataValueField = "ACCOUNTNO";
                ddlAccountLevel2.DataBind();

                ddlNDC2DefaultAcctno.DataSource = dtAccount;
                ddlNDC2DefaultAcctno.DataTextField = "ACCOUNTNO";
                ddlNDC2DefaultAcctno.DataValueField = "ACCOUNTNO";
                ddlNDC2DefaultAcctno.DataBind();

                //ddlAccount.Items.Insert(0,new ListItem("----------Chọn tài khoản----------",""));


                #endregion

                #region Hien thi tat cac cac role theo serviceid va usertype len cay
                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvIBLevel2, SmartPortal.Constant.IPC.NGUOIDUNGCAP2);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSLevel2, SmartPortal.Constant.IPC.NGUOIDUNGCAP2);

                //
                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBLevel2, SmartPortal.Constant.IPC.NGUOIDUNGCAP2);

                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOLevel2, SmartPortal.Constant.IPC.NGUOIDUNGCAP2);

                #endregion

                #region Xóa hết tất cả role chọn
                //foreach (ListItem liIB in cblIB.Items)
                //{

                //    liIB.Selected = false;

                //}
                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvIBLevel2, tvSMSLevel2, tvMBLevel2, tvPHOLevel2);
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

                //corp simple
                #region Người dùng Level 4
                lblError.Text = "";

                #region lay tat ca cac account cua khach hang
                DataSet dsLevel4 = new DataSet();
                if (System.Configuration.ConfigurationManager.AppSettings["AYACorporate"].ToString().Equals("0"))
                {
                    dsLevel4 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                }
                else
                {
                    switch (ddlCustType.SelectedValue.Trim())
                    {
                        case SmartPortal.Constant.IPC.PERSONAL:
                            dsLevel4 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                        case SmartPortal.Constant.IPC.PERSONALLKG:
                            dsLevel4 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                        case SmartPortal.Constant.IPC.CORPORATE:
                            dsLevel4 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                    }
                }
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.IPC);
                }
                DataTable dtLevel4 = new DataTable();
                dtLevel4 = dsLevel4.Tables[0];
                if (dtLevel4.Rows.Count == 0)
                {
                    lblError.Text = Resources.labels.khachhangnaykhongtontaitaikhoandedangky;
                    return;

                }
                else
                {
                    ddllv4SMSDefaultAcctno.DataSource = dtLevel4;
                    ddllv4SMSDefaultAcctno.DataTextField = "ACCOUNTNO";
                    ddllv4SMSDefaultAcctno.DataValueField = "ACCOUNTNO";
                    ddllv4SMSDefaultAcctno.DataBind();

                    ddllv4PHODefaultAcctno.DataSource = dtLevel4;
                    ddllv4PHODefaultAcctno.DataTextField = "ACCOUNTNO";
                    ddllv4PHODefaultAcctno.DataValueField = "ACCOUNTNO";
                    ddllv4PHODefaultAcctno.DataBind();

                    ddllv4Account.DataSource = dtLevel4;
                    ddllv4Account.DataTextField = "ACCOUNTNO";
                    ddllv4Account.DataValueField = "ACCOUNTNO";
                    ddllv4Account.DataBind();

                    //phongtt sms notification fee
                    lsAccNo.Clear();
                    foreach (DataRow dr in dtLevel4.Rows)
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
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvlv4IB, SmartPortal.Constant.IPC.QUANLYTAICHINH);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvlv4SMS, SmartPortal.Constant.IPC.QUANLYTAICHINH);


                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvlv4MB, SmartPortal.Constant.IPC.QUANLYTAICHINH);


                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvlv4PHO, SmartPortal.Constant.IPC.QUANLYTAICHINH);

                #endregion

                #region Xóa hết tất cả role chọn
                //foreach (ListItem liIB in cblIB.Items)
                //{

                //    liIB.Selected = false;

                //}
                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvlv4IB, tvlv4SMS, tvlv4MB, tvlv4PHO);
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

                #region Người dùng Level 5
                lblError.Text = "";

                #region lay tat ca cac account cua khach hang
                DataSet dsLevel5 = new DataSet();
                if (System.Configuration.ConfigurationManager.AppSettings["AYACorporate"].ToString().Equals("0"))
                {
                    dsLevel5 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                }
                else
                {
                    switch (ddlCustType.SelectedValue.Trim())
                    {
                        case SmartPortal.Constant.IPC.PERSONAL:
                            dsLevel5 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                        case SmartPortal.Constant.IPC.PERSONALLKG:
                            dsLevel5 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                        case SmartPortal.Constant.IPC.CORPORATE:
                            dsLevel5 = new SmartPortal.SEMS.Customer().GetAcctNo(cc, ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                            break;
                    }
                }
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.IPC);
                }
                DataTable dtLevel5 = new DataTable();
                dtLevel5 = dsLevel5.Tables[0];
                if (dtLevel5.Rows.Count == 0)
                {
                    lblError.Text = Resources.labels.khachhangnaykhongtontaitaikhoandedangky;
                    return;

                }
                else
                {
                    ddllv5SMSDefaultAcctno.DataSource = dtLevel5;
                    ddllv5SMSDefaultAcctno.DataTextField = "ACCOUNTNO";
                    ddllv5SMSDefaultAcctno.DataValueField = "ACCOUNTNO";
                    ddllv5SMSDefaultAcctno.DataBind();

                    ddllv5PHODefaultAcctno.DataSource = dtLevel5;
                    ddllv5PHODefaultAcctno.DataTextField = "ACCOUNTNO";
                    ddllv5PHODefaultAcctno.DataValueField = "ACCOUNTNO";
                    ddllv5PHODefaultAcctno.DataBind();

                    ddllv5Account.DataSource = dtLevel5;
                    ddllv5Account.DataTextField = "ACCOUNTNO";
                    ddllv5Account.DataValueField = "ACCOUNTNO";
                    ddllv5Account.DataBind();

                    //ddlAccountUyQuyen.Items.Insert(0, new ListItem("----------Chọn tài khoản----------", ""));

                }
                #endregion

                #region Hien thi tat cac cac role theo serviceid va usertype len cay
                //load for IB
                LoadDataInTreeview(SmartPortal.Constant.IPC.IB, tvlv5IB, SmartPortal.Constant.IPC.KETOAN);

                //load for SMS
                //LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvSMSLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.SMS, tvlv5SMS, SmartPortal.Constant.IPC.KETOAN);


                //load for MB
                //LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvMBLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.MB, tvlv5MB, SmartPortal.Constant.IPC.KETOAN);


                //load for PHO
                //LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvPHOLevel2, ddlContractType.SelectedValue);
                LoadDataInTreeview(SmartPortal.Constant.IPC.PHO, tvlv5PHO, SmartPortal.Constant.IPC.KETOAN);

                #endregion

                #region Xóa hết tất cả role chọn
                //foreach (ListItem liIB in cblIB.Items)
                //{

                //    liIB.Selected = false;

                //}
                #endregion

                #region lay role mac dinh
                GetRoleDefault(tvlv5IB, tvlv5SMS, tvlv5MB, tvlv5PHO);
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
                pnOption.Visible = false;
                pnCustInfo.Visible = false;
                pnContractInfo.Visible = false;
                pnPersonal.Visible = true;
                pnLuu.Visible = true;
                if (ddlCorpType.SelectedValue.Equals(SmartPortal.Constant.IPC.CONTRACTCORPADVANCE))
                {
                    pnPersonal.Visible = true;
                    pnPersonalSimple.Visible = false;
                }
                else
                {
                    pnPersonal.Visible = false;
                    pnPersonalSimple.Visible = true;
                }

                goto EXIT;
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
    EXIT:;
    }
    void loadCorpMatrix()
    {
        pnContractInfo.Visible = false;
        ContractModel model = new ContractModel();
        model.isNew = false;
        model.contractNo = txtContractNo.Text;

        model.cusID = ViewState["cusID"] != null ? ViewState["cusID"].ToString() : string.Empty;//hidCustID.Value.Trim();
        model.cusType = ddlCustType.SelectedValue;

        model.fullName = txtFullName.Text.Trim();
        model.contractNo = txtContractNo.Text.Trim();
        model.contractType = ddlContractType.SelectedValue;
        model.createDate = txtStartDate.Text;
        model.endDate = txtEndDate.Text;
        model.corpType = ddlCorpType.SelectedValue;
        model.productId = ddlProduct.SelectedValue;
        model.status = chkRenew.Checked ? "Y" : "N";


        UserControl matrix = LoadControl("~/Widgets/SEMSCustomerListCorp/Controls/CorporateMatrix.ascx",
            model, new EventHandler(btnBack_CorpMatrix));
        matrix.ID = "cl_Matrix";
        pnCorpMatrix.Controls.Add(matrix);
        pnLuu.Visible = false;
    }
    //lannth - 121018 - tao su kien back cho form corp matrix
    protected void btnBack_CorpMatrix(object sender, EventArgs e)
    {
        pnOption.Visible = false;
        pnCustInfo.Visible = false;
        pnContractInfo.Visible = true;
        pnPersonal.Visible = false;

        pnCorpMatrix.Visible = false;

    }
    private bool IsSuccess()
    {
        return IPCERRORCODE.Equals("0") ? true : false;
    }

    private bool IsDataSetNotNull(DataSet ds)
    {
        return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? true : false;
    }

    private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
    {
        List<Type> constParamTypes = new List<Type>();
        foreach (object constParam in constructorParameters)
        {
            constParamTypes.Add(constParam.GetType());
        }
        UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;
        ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

        if (constructor == null)
        {
            throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
        }
        else
        {
            constructor.Invoke(ctl, constructorParameters);
        }
        return ctl;
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
                //cbxSelect.Attributes.Add("onclick", "SelectRAD(this,'" + drv["CUSTID"].ToString() + "')");

                if (i == 0)
                {
                    cbxSelect.Checked = true;
                    ViewState["cusID"] = drv["CUSTID"].ToString();
                    hidCustID.Value = drv["CUSTID"].ToString();
                    i += 1;
                }
                lblCustCode.Text = drv["CUSTID"].ToString();
                lblCustCode.NavigateUrl = SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=145&a=viewdetail&cid=" + drv["CUSTID"].ToString());
                lblCustName.Text = drv["FULLNAME"].ToString();
                lblPhone.Text = drv["TEL"].ToString();
                lblIdentify.Text = drv["LICENSEID"].ToString();

                switch (drv["CFTYPE"].ToString().Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        lblCustType.Text = Resources.labels.canhan;
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
            //Lannth - 121018 - an panel corp matrix
            pnCorpMatrix.Visible = false;

        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(ex.Message, this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, Request.Url.Query);
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
            pnPersonalSimple.Visible = false;
            pnLuu.Visible = false;

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

    protected void btnThemChuTaiKhoan_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            //minh add 11/9/2015 validate thong tin
            if (string.IsNullOrEmpty(txtReFullName.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtReFullName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtReMobi.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoaichutaikhoan);

                txtReMobi.Focus();
                return;
            }


            string pattern = Resources.labels.emailpattern;

            //truong hop individual email = "" chap nhan, corporate validate ca truong hop rong

            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtReEmail.Text, pattern)))
            {
                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblAlert.Text = Resources.labels.emailkhongdinhdang1;
                txtReEmail.Focus();
                return;

            }


            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMS.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMB.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radAccount.Checked)
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // LuuThongTinQuyenadvance("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, txtIBUserName.Text, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, txtMBPhoneNo.Text, PassTemp, txtPHOPhoneNo.Text, PassTemp, ddlAccount.SelectedValue, ((cbCTKTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlCTKDefaultAcctno.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtIBUserName.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyenadvance("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, txtIBUserName.Text, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, txtMBPhoneNo.Text, PassTemp, txtPHOPhoneNo.Text, PassTemp, ddlAccount.SelectedValue, ((cbCTKTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlCTKDefaultAcctno.SelectedValue, pwdreset);
            }


            if (radAllAccount.Checked)
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

                //luu tat ca account
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtIBUserName.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyenadvance("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, txtIBUserName.Text, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, txtMBPhoneNo.Text, PassTemp, txtPHOPhoneNo.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbCTKTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlCTKDefaultAcctno.SelectedValue);
                    LuuThongTinQuyenadvance("CHUTAIKHOAN", gvResultChuTaiKhoan, tvIB, tvSMS, tvMB, tvPHO, txtReFullName.Text, lblLevel.Text, txtReBirth.Text, ddlReGender.SelectedValue, txtReMobi.Text, txtReEmail.Text, txtReAddress.Text, txtIBUserName.Text, PassTemp, txtSMSPhoneNo.Text, ddlSMSDefaultAcctno.SelectedValue, ddlLanguage.SelectedValue, txtMBPhoneNo.Text, PassTemp, txtPHOPhoneNo.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbCTKTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlCTKDefaultAcctno.SelectedValue, pwdreset);
                }

            }
            lblAlertCTK.Text = Resources.labels.recordsaved;
            #endregion
            //11/9/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultChuTaiKhoan.Rows.Count == 0)
            {

                lblAlertCTK.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractListCorp_Add_Widget", "btnThemChuTaiKhoan_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractListCorp_Add_Widget", "btnThemChuTaiKhoan_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
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

    //void LuuThongTinQuyen(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string MBPhone, string MBPass, string PHOPhone, string PHOPass, string Account, string SMSIsDefault, string SMSPinCode, string MBPinCode, string PHODefaultAcctno)
    //void LuuThongTinQuyen(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string SMSIsDefault, string SMSPinCode, string MBPhone, string MBPass, string MBPinCode, string PHOPhone, string PHOPass, string PHODefaultAcctno, string Account)
    void LuuThongTinQuyen(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string SMSIsDefault, string SMSPinCode, string MBPhone, string MBPass, string MBPinCode, string PHOPhone, string PHOPass, string PHODefaultAcctno, string Account, string pwdreset)
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
            DataColumn colMBPhone = new DataColumn("colMBPhone");
            DataColumn colMBPass = new DataColumn("colMBPass");
            DataColumn colPHOPhone = new DataColumn("colPHOPhone");
            DataColumn colPHOPass = new DataColumn("colPHOPass");
            DataColumn colAccount = new DataColumn("colAccount");
            DataColumn colRole = new DataColumn("colRole");
            DataColumn colRoleID = new DataColumn("colRoleID");
            DataColumn colTranCode = new DataColumn("colTranCode");
            DataColumn colTranCodeID = new DataColumn("colTranCodeID");
            DataColumn colServiceID = new DataColumn("colServiceID");

            DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault"); // moi them
            DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");// moi them
            DataColumn colMBPinCode = new DataColumn("colMBPinCode");// moi them
            DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");// moi them
            DataColumn colIBPolicy = new DataColumn("colIBPolicy");
            DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
            DataColumn colMBPolicy = new DataColumn("colMBPolicy");
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
            tblNguoiUyQuyen.Columns.Add(colPHOPhone);
            tblNguoiUyQuyen.Columns.Add(colPHOPass);
            tblNguoiUyQuyen.Columns.Add(colPHODefaultAcctno);
            tblNguoiUyQuyen.Columns.Add(colAccount);
            tblNguoiUyQuyen.Columns.Add(colRole);
            tblNguoiUyQuyen.Columns.Add(colRoleID);
            tblNguoiUyQuyen.Columns.Add(colTranCode);
            tblNguoiUyQuyen.Columns.Add(colTranCodeID);
            tblNguoiUyQuyen.Columns.Add(colIBPolicy);
            tblNguoiUyQuyen.Columns.Add(colSMSPolicy);
            tblNguoiUyQuyen.Columns.Add(colMBPolicy);
            tblNguoiUyQuyen.Columns.Add(colpwdreset);

            tblNguoiUyQuyen.Columns.Add(colServiceID);

            //tblNguoiUyQuyen.Columns.Add(colSMSIsDefault);
            //tblNguoiUyQuyen.Columns.Add(colSMSPinCode);
            //tblNguoiUyQuyen.Columns.Add(colMBPinCode);
            //tblNguoiUyQuyen.Columns.Add(colPHODefaultAcctno);

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
                            #region luu thong tin IB khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu thong tin sms khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            #region luu thong tin MB khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            #region luu thong tin PHO khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();

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

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
            #endregion
        }
        else
        {
            DataTable tblNguoiUyQuyen = (DataTable)ViewState[sessionName];

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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
                                        break;
                                }
                                rowNguoiUyQuyen["colpwdreset"] = pwdreset;

                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                                #region luu thong tin quyen SMS khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                                #region luu thong tin quyen MB khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();

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
                                #region luu thong tin quyen PHO khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();

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

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
        }
    }
    protected void ddlpolicyIBs_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlpolicyMBs.SelectedValue = ddlpolicyIBs.SelectedValue;
        }
        catch
        { }
        //ddlpolicyIBsl4.SelectedValue = ddlpolicyIBs.SelectedValue;
        //ddlpolicyIBsl5.SelectedValue = ddlpolicyIBs.SelectedValue;
    }
    protected void ddlpolicyIBqt_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlpolicyIB.SelectedValue = ddlpolicyIBqt.SelectedValue;
        //ddlpolicyIBau.SelectedValue = ddlpolicyIBqt.SelectedValue;
        //ddlpolicyIBl2.SelectedValue = ddlpolicyIBqt.SelectedValue;
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

            lblAlertCTK.Text = Resources.labels.recorddeleted;
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
            txtMBPhoneNguoiUyQuyen.Text = "";
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
            txtUserNameNguoiUyQuyen.Text = strCode;

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
        radAllAccount.Checked = false;

        radAllAccountNguoiUyQuyen.Checked = true;
        radAccountNguoiUyQuyen.Checked = false;

        radAllAccountLevel2.Checked = true;
        radAccountLevel2.Checked = false;

        radAllAccountQT.Checked = true;
        radAccountQT.Checked = false;
    }
    protected void btnThemNguoiUyQuyen_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            //minh add 11/9/2015 validate thong tin
            if (string.IsNullOrEmpty(txtAuthorizerCode.Text.Trim()))
            {
                //lblError.Visible = true;

                ShowPopUpMsg(Resources.labels.machutaikhoankhongduoctrong);
                txtAuthorizerCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFullnameNguoiUyQuyen.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptennguoiuyquyen);
                txtFullnameNguoiUyQuyen.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneNguoiUyQuyen.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoainguoiuyquyen);

                txtPhoneNguoiUyQuyen.Focus();
                return;
            }


            string pattern = Resources.labels.emailpattern;

            //truong hop individual email = "" chap nhan, corporate validate ca truong hop rong



            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtEmailNguoiUyQuyen.Text, pattern)))
            {
                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblAlert.Text = Resources.labels.emailkhongdinhdang1;
                txtEmailNguoiUyQuyen.Focus();
                return;

            }
            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMS.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMB.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radAccountNguoiUyQuyen.Checked)
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                //LuuThongTinQuyenadvance("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, txtUserNameNguoiUyQuyen.Text, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, txtMBPhoneNguoiUyQuyen.Text, PassTemp, txtPHOPhoneNguoiUyQuyen.Text, PassTemp, ddlAccountUyQuyen.SelectedValue, ((cbNUQTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNUQDefaultAcctno.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtUserNameNguoiUyQuyen.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyenadvance("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, txtUserNameNguoiUyQuyen.Text, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, txtMBPhoneNguoiUyQuyen.Text, PassTemp, txtPHOPhoneNguoiUyQuyen.Text, PassTemp, ddlAccountUyQuyen.SelectedValue, ((cbNUQTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNUQDefaultAcctno.SelectedValue, pwdreset);

            }
            if (radAllAccountNguoiUyQuyen.Checked)
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

                //luu tat ca account
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtUserNameNguoiUyQuyen.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyenadvance("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, txtUserNameNguoiUyQuyen.Text, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, txtMBPhoneNguoiUyQuyen.Text, PassTemp, txtPHOPhoneNguoiUyQuyen.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbNUQTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNUQDefaultAcctno.SelectedValue);
                    LuuThongTinQuyenadvance("NGUOIUYQUYEN", gvResultNguoiUyQuyen, tvIBUyQuyen, tvSMSUyQuyen, tvMBUyQuyen, tvPHOUyQuyen, txtFullnameNguoiUyQuyen.Text, lblLevelNguoiUyQuyen.Text, txtBirthNguoiUyQuyen.Text, ddlGenderNguoiUyQuyen.SelectedValue, txtPhoneNguoiUyQuyen.Text, txtEmailNguoiUyQuyen.Text, txtAddressNguoiUyQuyen.Text, txtUserNameNguoiUyQuyen.Text, PassTemp, txtSMSPhoneNguoiUyQuyen.Text, ddlSMSDefaultAcctnoUyQuyen.SelectedValue, ddlDefaultLanguageNguoiUyQuyen.SelectedValue, txtMBPhoneNguoiUyQuyen.Text, PassTemp, txtPHOPhoneNguoiUyQuyen.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbNUQTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNUQDefaultAcctno.SelectedValue, pwdreset);
                }


            }
            lblAlertNUY.Text = Resources.labels.recordsaved;
            #endregion
            //11/9/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultNguoiUyQuyen.Rows.Count == 0)
            {

                lblAlertNUY.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractListCorp_Add_Widget", "btnThemNguoiUyQuyen_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSContractListCorp_Add_Widget", "btnThemNguoiUyQuyen_Click", ex.ToString(), Request.Url.Query);
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

            lblAlertNUY.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void btnResetLevel2_Click(object sender, EventArgs e)
    {
        try
        {
            #region reset thong tin nguoi uy quyen
            txtFullNameLevel2.Text = "";
            txtBirthdayLevel2.Text = "";
            txtPhoneLevel2.Text = "";
            txtEmailLevel2.Text = "";
            txtAddresslevel2.Text = "";

            txtSMSPhoneLevel2.Text = "";
            txtMBPhoneLevel2.Text = "";
            txtPHOPhoneLevel2.Text = "";

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
            txtUserNameLevel2.Text = strCode;

            SetRadio();
            #endregion
        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSContractList_Add_Widget", "btnResetLevel2_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }
    }

    protected void btnThemQuyenLevel2_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            //minh add 11/9/2015 validate thong tin
            if (string.IsNullOrEmpty(txtSecondUserCode.Text.Trim()))
            {
                //lblError.Visible = true;

                ShowPopUpMsg(Resources.labels.machutaikhoankhongduoctrong);
                txtSecondUserCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFullNameLevel2.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptennguoidungcaphai);
                txtFullNameLevel2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneLevel2.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoainguoidungcaphai);

                txtPhoneLevel2.Focus();
                return;
            }



            string pattern = Resources.labels.emailpattern;

            //truong hop individual email = "" chap nhan, corporate validate ca truong hop rong



            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtEmailLevel2.Text, pattern)))
            {
                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblAlert.Text = Resources.labels.emailkhongdinhdang1;
                txtEmailLevel2.Focus();
                return;

            }
            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMS.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMB.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radAccountLevel2.Checked)
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                // LuuThongTinQuyenadvance("LEVEL2", gvResultLevel2, tvIBLevel2, tvSMSLevel2, tvMBLevel2, tvPHOLevel2, txtFullNameLevel2.Text, lblLevelLevel2.Text, txtBirthdayLevel2.Text, ddlGenderLevel2.SelectedValue, txtPhoneLevel2.Text, txtEmailLevel2.Text, txtReAddress.Text, txtUserNameLevel2.Text, PassTemp, txtSMSPhoneLevel2.Text, ddlDefaultAccountLevel2.SelectedValue, ddlDefaultLangLevel2.SelectedValue, txtMBPhoneLevel2.Text, PassTemp, txtPHOPhoneLevel2.Text, PassTemp, ddlAccountLevel2.SelectedValue, ((cbNDC2TKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNDC2DefaultAcctno.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtUserNameLevel2.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyenadvance("LEVEL2", gvResultLevel2, tvIBLevel2, tvSMSLevel2, tvMBLevel2, tvPHOLevel2, txtFullNameLevel2.Text, lblLevelLevel2.Text, txtBirthdayLevel2.Text, ddlGenderLevel2.SelectedValue, txtPhoneLevel2.Text, txtEmailLevel2.Text, txtReAddress.Text, txtUserNameLevel2.Text, PassTemp, txtSMSPhoneLevel2.Text, ddlDefaultAccountLevel2.SelectedValue, ddlDefaultLangLevel2.SelectedValue, txtMBPhoneLevel2.Text, PassTemp, txtPHOPhoneLevel2.Text, PassTemp, ddlAccountLevel2.SelectedValue, ((cbNDC2TKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNDC2DefaultAcctno.SelectedValue, pwdreset);
            }

            if (radAllAccountLevel2.Checked)
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

                //luu tat ca account
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtUserNameLevel2.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyenadvance("LEVEL2", gvResultLevel2, tvIBLevel2, tvSMSLevel2, tvMBLevel2, tvPHOLevel2, txtFullNameLevel2.Text, lblLevelLevel2.Text, txtBirthdayLevel2.Text, ddlGenderLevel2.SelectedValue, txtPhoneLevel2.Text, txtEmailLevel2.Text, txtReAddress.Text, txtUserNameLevel2.Text, PassTemp, txtSMSPhoneLevel2.Text, ddlDefaultAccountLevel2.SelectedValue, ddlDefaultLangLevel2.SelectedValue, txtMBPhoneLevel2.Text, PassTemp, txtPHOPhoneLevel2.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbNDC2TKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNDC2DefaultAcctno.SelectedValue);
                    LuuThongTinQuyenadvance("LEVEL2", gvResultLevel2, tvIBLevel2, tvSMSLevel2, tvMBLevel2, tvPHOLevel2, txtFullNameLevel2.Text, lblLevelLevel2.Text, txtBirthdayLevel2.Text, ddlGenderLevel2.SelectedValue, txtPhoneLevel2.Text, txtEmailLevel2.Text, txtReAddress.Text, txtUserNameLevel2.Text, PassTemp, txtSMSPhoneLevel2.Text, ddlDefaultAccountLevel2.SelectedValue, ddlDefaultLangLevel2.SelectedValue, txtMBPhoneLevel2.Text, PassTemp, txtPHOPhoneLevel2.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbNDC2TKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlNDC2DefaultAcctno.SelectedValue, pwdreset);
                }

            }

            lblAlertL2.Text = Resources.labels.recordsaved;
            #endregion
            //11/9/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultLevel2.Rows.Count == 0)
            {

                lblAlertL2.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemQuyenLevel2_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemQuyenLevel2_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    protected void gvResultLevel2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultLevel2.PageIndex = e.NewPageIndex;
            gvResultLevel2.DataSource = (DataTable)ViewState["LEVEL2"];
            gvResultLevel2.DataBind();
        }
        catch
        {
        }
    }

    protected void gvResultLevel2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultLevl2 = (DataTable)ViewState["LEVEL2"];

            tblResultLevl2.Rows.RemoveAt(e.RowIndex + (gvResultLevel2.PageIndex * gvResultLevel2.PageSize));

            ViewState["LEVEL2"] = tblResultLevl2;
            gvResultLevel2.DataSource = tblResultLevl2;
            gvResultLevel2.DataBind();

            lblAlertL2.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void btnCustSave_Click(object sender, EventArgs e)
    {
        try
        {
            //15.9.2015 minh tach advance/simple
            if (ddlCorpType.SelectedValue.Equals(SmartPortal.Constant.IPC.CONTRACTCORPADVANCE))
            {
                #region advance
                //Check session
                if (ViewState["NGUOIQUANTRI"] == null)
                {
                    lblError.Text = Resources.labels.bancandiendayduthongtinnguoiquantrihethongdetaohopdong;
                    return;
                }
                if (ViewState["CHUTAIKHOAN"] == null)
                {
                    lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoandetaohopdong;
                    return;
                }
                DataTable tblCHUTAIKHOAN = new DataTable();
                DataTable tblNGUOIUYQUYEN = new DataTable();
                DataTable tblLEVEL2 = new DataTable();
                DataTable tblNGUOIQUANTRI = new DataTable();

                if (ViewState["CHUTAIKHOAN"] != null)
                {
                    tblCHUTAIKHOAN = (DataTable)ViewState["CHUTAIKHOAN"];
                    if (tblCHUTAIKHOAN.Rows.Count == 0)
                    {
                        lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoandetaohopdong;
                        return;
                    }
                }

                if (ViewState["NGUOIUYQUYEN"] != null)
                {
                    tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                }

                if (ViewState["LEVEL2"] != null)
                {
                    tblLEVEL2 = (DataTable)ViewState["LEVEL2"];
                }

                if (ViewState["NGUOIQUANTRI"] != null)
                {
                    tblNGUOIQUANTRI = (DataTable)ViewState["NGUOIQUANTRI"];
                    if (tblNGUOIQUANTRI.Rows.Count == 0)
                    {
                        lblError.Text = Resources.labels.bancandiendayduthongtinnguoiquantrihethongdetaohopdong;
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
                DataColumn colIBUserName = new DataColumn("colIBUserName");
                DataColumn colIBPass = new DataColumn("colIBPass");
                DataColumn colSMSPhone = new DataColumn("colSMSPhone");
                DataColumn colSMSDefaultAcctno = new DataColumn("colSMSDefaultAcctno");
                DataColumn colSMSDefaultLang = new DataColumn("colSMSDefaultLang");
                DataColumn colMBPhone = new DataColumn("colMBPhone");
                DataColumn colMBPass = new DataColumn("colMBPass");
                DataColumn colPHOPhone = new DataColumn("colPHOPhone");
                DataColumn colPHOPass = new DataColumn("colPHOPass");
                DataColumn colAccount = new DataColumn("colAccount");
                DataColumn colRole = new DataColumn("colRole");
                DataColumn colRoleIDS = new DataColumn("colRoleID");
                DataColumn colTranCodeS = new DataColumn("colTranCode");
                DataColumn colTranCodeID = new DataColumn("colTranCodeID");
                DataColumn colServiceIDS = new DataColumn("colServiceID");

                DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault"); // moi them
                DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");// moi them
                DataColumn colMBPinCode = new DataColumn("colMBPinCode");// moi them
                DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");// moi them
                                                                                       //thêm TypeID
                DataColumn colTypeID = new DataColumn("colTypeID");
                DataColumn colIBPolicy = new DataColumn("colIBPolicy");
                DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
                DataColumn colMBPolicy = new DataColumn("colMBPolicy");
                DataColumn colpwdreset = new DataColumn("colpwdreset");

                tblSUM.Columns.Add(colFullName);
                tblSUM.Columns.Add(colLevel);
                tblSUM.Columns.Add(colBirthday);
                tblSUM.Columns.Add(colGender);
                tblSUM.Columns.Add(colPhone);
                tblSUM.Columns.Add(colEmail);
                tblSUM.Columns.Add(colAddress);
                tblSUM.Columns.Add(colIBUserName);
                tblSUM.Columns.Add(colIBPass);
                tblSUM.Columns.Add(colSMSPhone);
                tblSUM.Columns.Add(colSMSDefaultAcctno);
                tblSUM.Columns.Add(colSMSDefaultLang);
                tblSUM.Columns.Add(colSMSIsDefault);//
                tblSUM.Columns.Add(colSMSPinCode);//
                tblSUM.Columns.Add(colMBPhone);
                tblSUM.Columns.Add(colMBPass);
                tblSUM.Columns.Add(colMBPinCode);//
                tblSUM.Columns.Add(colPHOPhone);
                tblSUM.Columns.Add(colPHOPass);
                tblSUM.Columns.Add(colPHODefaultAcctno);//
                tblSUM.Columns.Add(colAccount);
                tblSUM.Columns.Add(colRole);
                tblSUM.Columns.Add(colRoleIDS);
                tblSUM.Columns.Add(colTranCodeS);
                tblSUM.Columns.Add(colTranCodeID);
                tblSUM.Columns.Add(colServiceIDS);
                //thêm TypeID
                tblSUM.Columns.Add(colTypeID);
                tblSUM.Columns.Add(colIBPolicy);
                tblSUM.Columns.Add(colSMSPolicy);
                tblSUM.Columns.Add(colMBPolicy);
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
                    rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                    rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                    rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                    rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                    rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                    rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                    rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                    rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                    rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                    rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                    rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                    rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                    rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();

                    rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                    rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                    rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                    rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                    //thêm TypeID
                    rowNguoiUyQuyen["colTypeID"] = SmartPortal.Constant.IPC.CHUTAIKHOAN;
                    rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                    rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                    rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
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
                    rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                    rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                    rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                    rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                    rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                    rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                    rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                    rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                    rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                    rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                    rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                    rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                    rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();

                    rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                    rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                    rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                    rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                    //thêm TypeID
                    rowNguoiUyQuyen["colTypeID"] = SmartPortal.Constant.IPC.NGUOIUYQUYEN;
                    rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                    rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                    rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
                    rowNguoiUyQuyen["colpwdreset"] = dongCTK["colpwdreset"].ToString();

                    tblSUM.Rows.Add(rowNguoiUyQuyen);
                }
                //lấy thông tin trong bảng NGUOIDUNGCAP2
                foreach (DataRow dongCTK in tblLEVEL2.Rows)
                {
                    DataRow rowNguoiUyQuyen = tblSUM.NewRow();
                    rowNguoiUyQuyen["colFullName"] = dongCTK["colFullName"].ToString();
                    rowNguoiUyQuyen["colLevel"] = dongCTK["colLevel"].ToString();
                    rowNguoiUyQuyen["colBirthday"] = dongCTK["colBirthday"].ToString();
                    rowNguoiUyQuyen["colGender"] = dongCTK["colGender"].ToString();
                    rowNguoiUyQuyen["colPhone"] = dongCTK["colPhone"].ToString();
                    rowNguoiUyQuyen["colEmail"] = dongCTK["colEmail"].ToString();
                    rowNguoiUyQuyen["colAddress"] = dongCTK["colAddress"].ToString();
                    rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                    rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                    rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                    rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                    rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                    rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                    rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                    rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                    rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                    rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                    rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                    rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                    rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();

                    rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                    rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                    rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                    rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                    //thêm TypeID
                    rowNguoiUyQuyen["colTypeID"] = SmartPortal.Constant.IPC.NGUOIDUNGCAP2;
                    rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                    rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                    rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
                    rowNguoiUyQuyen["colpwdreset"] = dongCTK["colpwdreset"].ToString();

                    tblSUM.Rows.Add(rowNguoiUyQuyen);
                }

                //lấy thông tin trong bảng NGUOIQUANTRI
                foreach (DataRow dongCTK in tblNGUOIQUANTRI.Rows)
                {
                    DataRow rowNguoiUyQuyen = tblSUM.NewRow();
                    rowNguoiUyQuyen["colFullName"] = dongCTK["colFullName"].ToString();
                    rowNguoiUyQuyen["colLevel"] = dongCTK["colLevel"].ToString();
                    rowNguoiUyQuyen["colBirthday"] = dongCTK["colBirthday"].ToString();
                    rowNguoiUyQuyen["colGender"] = dongCTK["colGender"].ToString();
                    rowNguoiUyQuyen["colPhone"] = dongCTK["colPhone"].ToString();
                    rowNguoiUyQuyen["colEmail"] = dongCTK["colEmail"].ToString();
                    rowNguoiUyQuyen["colAddress"] = dongCTK["colAddress"].ToString();
                    rowNguoiUyQuyen["colIBUserName"] = dongCTK["colIBUserName"].ToString();
                    rowNguoiUyQuyen["colIBPass"] = dongCTK["colIBPass"].ToString();
                    rowNguoiUyQuyen["colSMSPhone"] = dongCTK["colSMSPhone"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultAcctno"] = dongCTK["colSMSDefaultAcctno"].ToString();
                    rowNguoiUyQuyen["colSMSDefaultLang"] = dongCTK["colSMSDefaultLang"].ToString();
                    rowNguoiUyQuyen["colMBPhone"] = dongCTK["colMBPhone"].ToString();
                    rowNguoiUyQuyen["colMBPass"] = dongCTK["colMBPass"].ToString();
                    rowNguoiUyQuyen["colPHOPhone"] = dongCTK["colPHOPhone"].ToString();
                    rowNguoiUyQuyen["colPHOPass"] = dongCTK["colPHOPass"].ToString();
                    rowNguoiUyQuyen["colAccount"] = dongCTK["colAccount"].ToString();
                    rowNguoiUyQuyen["colRole"] = dongCTK["colRole"].ToString();
                    rowNguoiUyQuyen["colRoleID"] = dongCTK["colRoleID"].ToString();
                    rowNguoiUyQuyen["colTranCode"] = dongCTK["colTranCode"].ToString();
                    rowNguoiUyQuyen["colTranCodeID"] = dongCTK["colTranCodeID"].ToString();
                    rowNguoiUyQuyen["colServiceID"] = dongCTK["colServiceID"].ToString();

                    rowNguoiUyQuyen["colSMSIsDefault"] = dongCTK["colSMSIsDefault"].ToString();
                    rowNguoiUyQuyen["colSMSPinCode"] = dongCTK["colSMSPinCode"].ToString();
                    rowNguoiUyQuyen["colMBPinCode"] = dongCTK["colMBPinCode"].ToString();
                    rowNguoiUyQuyen["colPHODefaultAcctno"] = dongCTK["colPHODefaultAcctno"].ToString();
                    //thêm TypeID
                    rowNguoiUyQuyen["colTypeID"] = SmartPortal.Constant.IPC.QUANTRIHETHONG;
                    rowNguoiUyQuyen["colIBPolicy"] = dongCTK["colIBPolicy"].ToString();
                    rowNguoiUyQuyen["colSMSPolicy"] = dongCTK["colSMSPolicy"].ToString();
                    rowNguoiUyQuyen["colMBPolicy"] = dongCTK["colMBPolicy"].ToString();
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
                DataColumn colNewTypeID = new DataColumn("colNewTypeID");

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
                tblUser.Columns.Add(colNewTypeID);

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
                        row2["colUserType"] = userType;
                        row2["colUserLevel"] = tblSUM.Rows[i]["colLevel"].ToString();
                        row2["colDeptID"] = deptID;
                        row2["colTokenID"] = tokenID;
                        row2["colTokenIssueDate"] = tokenIssueDate;
                        row2["colSMSOTP"] = smsOTP;
                        row2["colSMSBirthday"] = tblSUM.Rows[i]["colBirthday"].ToString();
                        row2["colNewTypeID"] = tblSUM.Rows[i]["colTypeID"].ToString();

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
                foreach (DataRow rIBU in tblSUM.Rows)
                {
                    if (rIBU["colIBUserName"].ToString().Trim() != UN.Trim() && rIBU["colIBUserName"].ToString().Trim() != "")
                    {
                        DataRow row3 = tblIbankUser.NewRow();
                        row3["colUserName"] = rIBU["colIBUserName"].ToString();
                        row3["colIBUserID"] = rIBU["colIBUserName"].ToString();
                        row3["colIBPassword"] = rIBU["colIBPass"].ToString();
                        row3["colLastLoginTime"] = uCreateDate;
                        row3["colIBStatus"] = status;
                        row3["colIBUserCreate"] = userCreate;
                        row3["colIBDateCreate"] = uCreateDate;
                        row3["colIBUserModify"] = userCreate;
                        row3["colIBLastModify"] = lastModify;
                        row3["colIBUserApprove"] = userApprove;
                        row3["colIBIsLogin"] = "0";
                        row3["colIBDateExpire"] = endDate;
                        row3["colIBExpireTime"] = startDate;
                        row3["colIBPolicyusr"] = rIBU["colIBPolicy"].ToString();
                        row3["colpwdresetIB"] = rIBU["colpwdreset"].ToString();



                        tblIbankUser.Rows.Add(row3);

                        UN = rIBU["colIBUserName"].ToString();
                    }
                }
                #endregion

                #region Tạo bảng chứa User SMS
                DataTable tblSMSUser = new DataTable();
                DataColumn colSMSUserID = new DataColumn("colSMSUserID");
                DataColumn colSMSPhoneNo = new DataColumn("colSMSPhoneNo");
                DataColumn colSMSContractNo = new DataColumn("colSMSContractNo");
                DataColumn colSMSIsBroadcast = new DataColumn("colSMSIsBroadcast");
                DataColumn colSMSDefaultAcctnoU = new DataColumn("colSMSDefaultAcctno");
                DataColumn colSMSDefaultLangU = new DataColumn("colSMSDefaultLang");
                DataColumn colSMSIsDefaultU = new DataColumn("colSMSIsDefault");
                DataColumn colSMSPinCodeU = new DataColumn("colSMSPinCode");
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
                tblSMSUser.Columns.Add(colSMSIsDefaultU);
                tblSMSUser.Columns.Add(colSMSPinCodeU);
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
                        row4["colpwdresetSMS"] = Encryption.Encrypt(rSMSU["colSMSPinCode"].ToString());// rSMSU["colpwdreset"].ToString();


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
                DataColumn colMBPinCodeU = new DataColumn("colMBPinCode");
                DataColumn colMBPolicyusr = new DataColumn("colMBPolicyusr");
                DataColumn colpwdresetMB = new DataColumn("colpwdresetMB");



                //add vào table
                tblMBUser.Columns.Add(colMBUserID);
                tblMBUser.Columns.Add(colMBPhoneNo);
                tblMBUser.Columns.Add(colMBPassU);
                tblMBUser.Columns.Add(colMBStatus);
                tblMBUser.Columns.Add(colMBPinCodeU);
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
                        row5["colMBPinCode"] = rMBP["colMBPinCode"].ToString();
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
                        row6["colPHOStatus"] = status;
                        row6["colPHODefaultAcctno"] = rPHOP["colPHODefaultAcctno"].ToString();

                        tblPHOUser.Rows.Add(row6);
                        PHOP = rPHOP["colPHOPhone"].ToString();
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
                            newRowCA["colAcctType"] = "DD";
                            newRowCA["colCCYID"] = Resources.labels.lak;
                            newRowCA["colStatus"] = "A";
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

                //add vào table
                tblUserAccount.Columns.Add(colUserIDUC);
                tblUserAccount.Columns.Add(colAcctNoUC);
                tblUserAccount.Columns.Add(colRoleIDUC);
                tblUserAccount.Columns.Add(colUseFull);
                tblUserAccount.Columns.Add(colDesc);

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

                        tblUserAccount.Rows.Add(newRowUA);
                    }
                }
                #endregion

                #region Tạo bảng chứa phòng ban mặc định
                DataTable tblDeptDefault = new DataTable();

                DataColumn colDeptID1 = new DataColumn("colDeptID1");
                DataColumn colDeptName = new DataColumn("colDeptName");
                DataColumn colDeptDesc = new DataColumn("colDeptDesc");
                DataColumn colDeptContractNo = new DataColumn("colDeptContractNo");

                tblDeptDefault.Columns.Add(colDeptID1);
                tblDeptDefault.Columns.Add(colDeptName);
                tblDeptDefault.Columns.Add(colDeptDesc);
                tblDeptDefault.Columns.Add(colDeptContractNo);

                DataRow rowDept = tblDeptDefault.NewRow();
                rowDept["colDeptID1"] = SmartPortal.Constant.IPC.DEPTIDPREFIX + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8);
                rowDept["colDeptName"] = Resources.labels.all;
                rowDept["colDeptDesc"] = "Chứa thông tin tất cả người dùng doanh nghiệp";
                rowDept["colDeptContractNo"] = contractNo;

                tblDeptDefault.Rows.Add(rowDept);

                #endregion

                #region Tạo bảng tạo quyền mặc định cho hợp đồng
                DataTable tblRoleDefault = new DataTable();

                DataColumn colRoleName = new DataColumn("colRoleName");
                DataColumn colRoleDesc = new DataColumn("colRoleDesc");
                DataColumn colUserCreated = new DataColumn("colUserCreated");
                DataColumn colRoleContractNo = new DataColumn("colRoleContractNo");
                DataColumn colRoleServiceID = new DataColumn("colRoleServiceID");
                DataColumn colRoleProductID = new DataColumn("colRoleProductID");

                tblRoleDefault.Columns.Add(colRoleName);
                tblRoleDefault.Columns.Add(colRoleDesc);
                tblRoleDefault.Columns.Add(colUserCreated);
                tblRoleDefault.Columns.Add(colRoleContractNo);
                tblRoleDefault.Columns.Add(colRoleServiceID);
                tblRoleDefault.Columns.Add(colRoleProductID);

                DataRow rowRoleIB = tblRoleDefault.NewRow();
                rowRoleIB["colRoleName"] = "Group role of Internet Banking";
                rowRoleIB["colRoleDesc"] = "Group role of Internet Banking";
                rowRoleIB["colUserCreated"] = userCreate;
                rowRoleIB["colRoleContractNo"] = contractNo;
                rowRoleIB["colRoleServiceID"] = SmartPortal.Constant.IPC.IB;
                rowRoleIB["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRoleSMS = tblRoleDefault.NewRow();
                rowRoleSMS["colRoleName"] = "Group role of SMS Banking";
                rowRoleSMS["colRoleDesc"] = "Group role of SMS Banking";
                rowRoleSMS["colUserCreated"] = userCreate;
                rowRoleSMS["colRoleContractNo"] = contractNo;
                rowRoleSMS["colRoleServiceID"] = SmartPortal.Constant.IPC.SMS;
                rowRoleSMS["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRoleMB = tblRoleDefault.NewRow();
                rowRoleMB["colRoleName"] = "Group role of Mobile Banking";
                rowRoleMB["colRoleDesc"] = "Group role of Mobile Banking";
                rowRoleMB["colUserCreated"] = userCreate;
                rowRoleMB["colRoleContractNo"] = contractNo;
                rowRoleMB["colRoleServiceID"] = SmartPortal.Constant.IPC.MB;
                rowRoleMB["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRolePHO = tblRoleDefault.NewRow();
                rowRolePHO["colRoleName"] = "Group role of Phone Banking";
                rowRolePHO["colRoleDesc"] = "Group role of Phone Banking";
                rowRolePHO["colUserCreated"] = userCreate;
                rowRolePHO["colRoleContractNo"] = contractNo;
                rowRolePHO["colRoleServiceID"] = SmartPortal.Constant.IPC.PHO;
                rowRolePHO["colRoleProductID"] = ddlProduct.SelectedValue;

                tblRoleDefault.Rows.Add(rowRoleIB);
                tblRoleDefault.Rows.Add(rowRoleSMS);
                tblRoleDefault.Rows.Add(rowRoleMB);
                tblRoleDefault.Rows.Add(rowRolePHO);
                #endregion

                #region INSERT
                //new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount,tblDeptDefault,tblRoleDefault, ref IPCERRORCODE, ref IPCERRORDESC);
                //08.12.2015 minh add corptype
                new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, (chkRenew.Checked ? "Y" : "N"), ddlCorpType.SelectedValue.ToString(), tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount, tblDeptDefault, tblRoleDefault, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                else
                {

                }

                #endregion
                #endregion
            }
            else
            #region simple
            {
                //Check session
                if (ViewState["CHUTAIKHOANS"] == null)
                {
                    lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoandetaohopdong;
                    return;
                }

                DataTable tblCHUTAIKHOANS = new DataTable();
                DataTable tblQUANLYTAICHINH = new DataTable();
                DataTable tblKETOAN = new DataTable();

                if (ViewState["CHUTAIKHOANS"] != null)
                {
                    tblCHUTAIKHOANS = (DataTable)ViewState["CHUTAIKHOANS"];
                    if (tblCHUTAIKHOANS.Rows.Count == 0)
                    {
                        lblError.Text = Resources.labels.bancandiendayduthongtinchutaikhoandetaohopdong;
                        return;
                    }
                }

                if (ViewState["QUANLYTAICHINH"] != null)
                {
                    tblQUANLYTAICHINH = (DataTable)ViewState["QUANLYTAICHINH"];
                }

                if (ViewState["KETOAN"] != null)
                {
                    tblKETOAN = (DataTable)ViewState["KETOAN"];
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
                DataColumn colIBUserName = new DataColumn("colIBUserName");
                DataColumn colIBPass = new DataColumn("colIBPass");
                DataColumn colSMSPhone = new DataColumn("colSMSPhone");
                DataColumn colSMSDefaultAcctno = new DataColumn("colSMSDefaultAcctno");
                DataColumn colSMSDefaultLang = new DataColumn("colSMSDefaultLang");
                DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault");
                DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");
                DataColumn colMBPhone = new DataColumn("colMBPhone");
                DataColumn colMBPass = new DataColumn("colMBPass");
                DataColumn colMBPinCode = new DataColumn("colMBPinCode");
                DataColumn colPHOPhone = new DataColumn("colPHOPhone");
                DataColumn colPHOPass = new DataColumn("colPHOPass");
                DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");
                DataColumn colAccount = new DataColumn("colAccount");
                DataColumn colRole = new DataColumn("colRole");
                DataColumn colRoleIDS = new DataColumn("colRoleID");
                DataColumn colTranCodeS = new DataColumn("colTranCode");
                DataColumn colTranCodeID = new DataColumn("colTranCodeID");
                DataColumn colServiceIDS = new DataColumn("colServiceID");
                //thêm TypeID
                DataColumn colTypeID = new DataColumn("colTypeID");
                DataColumn colIBPolicy = new DataColumn("colIBPolicy");
                DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
                DataColumn colMBPolicy = new DataColumn("colMBPolicy");
                DataColumn colpwdreset = new DataColumn("colpwdreset");


                tblSUM.Columns.Add(colFullName);
                tblSUM.Columns.Add(colLevel);
                tblSUM.Columns.Add(colBirthday);
                tblSUM.Columns.Add(colGender);
                tblSUM.Columns.Add(colPhone);
                tblSUM.Columns.Add(colEmail);
                tblSUM.Columns.Add(colAddress);
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
                tblSUM.Columns.Add(colPHOPhone);
                tblSUM.Columns.Add(colPHOPass);
                tblSUM.Columns.Add(colPHODefaultAcctno);
                tblSUM.Columns.Add(colAccount);
                tblSUM.Columns.Add(colRole);
                tblSUM.Columns.Add(colRoleIDS);
                tblSUM.Columns.Add(colTranCodeS);
                tblSUM.Columns.Add(colTranCodeID);
                tblSUM.Columns.Add(colServiceIDS);
                //thêm TypeID
                tblSUM.Columns.Add(colTypeID);
                tblSUM.Columns.Add(colIBPolicy);
                tblSUM.Columns.Add(colSMSPolicy);
                tblSUM.Columns.Add(colMBPolicy);
                tblSUM.Columns.Add(colpwdreset);


                //lấy thông tin trong bảng CHUTAIKHOANS
                foreach (DataRow CTK in tblCHUTAIKHOANS.Rows)
                {
                    DataRow rowChuTaiKhoan = tblSUM.NewRow();
                    rowChuTaiKhoan["colFullName"] = CTK["colFullName"].ToString();
                    rowChuTaiKhoan["colLevel"] = CTK["colLevel"].ToString();
                    rowChuTaiKhoan["colBirthday"] = CTK["colBirthday"].ToString();
                    rowChuTaiKhoan["colGender"] = CTK["colGender"].ToString();
                    rowChuTaiKhoan["colPhone"] = CTK["colPhone"].ToString();
                    rowChuTaiKhoan["colEmail"] = CTK["colEmail"].ToString();
                    rowChuTaiKhoan["colAddress"] = CTK["colAddress"].ToString();
                    rowChuTaiKhoan["colIBUserName"] = CTK["colIBUserName"].ToString();
                    rowChuTaiKhoan["colIBPass"] = CTK["colIBPass"].ToString();
                    rowChuTaiKhoan["colSMSPhone"] = CTK["colSMSPhone"].ToString();
                    rowChuTaiKhoan["colSMSDefaultAcctno"] = CTK["colSMSDefaultAcctno"].ToString();
                    rowChuTaiKhoan["colSMSDefaultLang"] = CTK["colSMSDefaultLang"].ToString();
                    rowChuTaiKhoan["colSMSIsDefault"] = CTK["colSMSIsDefault"].ToString();
                    rowChuTaiKhoan["colSMSPinCode"] = CTK["colSMSPinCode"].ToString();
                    rowChuTaiKhoan["colMBPhone"] = CTK["colMBPhone"].ToString();
                    rowChuTaiKhoan["colMBPass"] = CTK["colMBPass"].ToString();
                    rowChuTaiKhoan["colMBPinCode"] = CTK["colMBPinCode"].ToString();
                    rowChuTaiKhoan["colPHOPhone"] = CTK["colPHOPhone"].ToString();
                    rowChuTaiKhoan["colPHOPass"] = CTK["colPHOPass"].ToString();
                    rowChuTaiKhoan["colPHODefaultAcctno"] = CTK["colPHODefaultAcctno"].ToString();
                    rowChuTaiKhoan["colAccount"] = CTK["colAccount"].ToString();
                    rowChuTaiKhoan["colRole"] = CTK["colRole"].ToString();
                    rowChuTaiKhoan["colRoleID"] = CTK["colRoleID"].ToString();
                    rowChuTaiKhoan["colTranCode"] = CTK["colTranCode"].ToString();
                    rowChuTaiKhoan["colTranCodeID"] = CTK["colTranCodeID"].ToString();
                    rowChuTaiKhoan["colServiceID"] = CTK["colServiceID"].ToString();
                    //thêm TypeID
                    rowChuTaiKhoan["colTypeID"] = SmartPortal.Constant.IPC.CHUTAIKHOAN;
                    rowChuTaiKhoan["colIBPolicy"] = CTK["colIBPolicy"].ToString();
                    rowChuTaiKhoan["colSMSPolicy"] = CTK["colSMSPolicy"].ToString();
                    rowChuTaiKhoan["colMBPolicy"] = CTK["colMBPolicy"].ToString();
                    rowChuTaiKhoan["colpwdreset"] = CTK["colpwdreset"].ToString();


                    tblSUM.Rows.Add(rowChuTaiKhoan);
                }

                //lấy thông tin trong bảng QUANLYTAICHINH
                foreach (DataRow dongQLTC in tblQUANLYTAICHINH.Rows)
                {
                    DataRow rowQuanLyTaiChinh = tblSUM.NewRow();
                    rowQuanLyTaiChinh["colFullName"] = dongQLTC["colFullName"].ToString();
                    rowQuanLyTaiChinh["colLevel"] = dongQLTC["colLevel"].ToString();
                    rowQuanLyTaiChinh["colBirthday"] = dongQLTC["colBirthday"].ToString();
                    rowQuanLyTaiChinh["colGender"] = dongQLTC["colGender"].ToString();
                    rowQuanLyTaiChinh["colPhone"] = dongQLTC["colPhone"].ToString();
                    rowQuanLyTaiChinh["colEmail"] = dongQLTC["colEmail"].ToString();
                    rowQuanLyTaiChinh["colAddress"] = dongQLTC["colAddress"].ToString();
                    rowQuanLyTaiChinh["colIBUserName"] = dongQLTC["colIBUserName"].ToString();
                    rowQuanLyTaiChinh["colIBPass"] = dongQLTC["colIBPass"].ToString();
                    rowQuanLyTaiChinh["colSMSPhone"] = dongQLTC["colSMSPhone"].ToString();
                    rowQuanLyTaiChinh["colSMSDefaultAcctno"] = dongQLTC["colSMSDefaultAcctno"].ToString();
                    rowQuanLyTaiChinh["colSMSDefaultLang"] = dongQLTC["colSMSDefaultLang"].ToString();
                    rowQuanLyTaiChinh["colSMSIsDefault"] = dongQLTC["colSMSIsDefault"].ToString();
                    rowQuanLyTaiChinh["colSMSPinCode"] = dongQLTC["colSMSPinCode"].ToString();
                    rowQuanLyTaiChinh["colMBPhone"] = dongQLTC["colMBPhone"].ToString();
                    rowQuanLyTaiChinh["colMBPass"] = dongQLTC["colMBPass"].ToString();
                    rowQuanLyTaiChinh["colMBPinCode"] = dongQLTC["colMBPinCode"].ToString();
                    rowQuanLyTaiChinh["colPHOPhone"] = dongQLTC["colPHOPhone"].ToString();
                    rowQuanLyTaiChinh["colPHOPass"] = dongQLTC["colPHOPass"].ToString();
                    rowQuanLyTaiChinh["colPHODefaultAcctno"] = dongQLTC["colPHODefaultAcctno"].ToString();
                    rowQuanLyTaiChinh["colAccount"] = dongQLTC["colAccount"].ToString();
                    rowQuanLyTaiChinh["colRole"] = dongQLTC["colRole"].ToString();
                    rowQuanLyTaiChinh["colRoleID"] = dongQLTC["colRoleID"].ToString();
                    rowQuanLyTaiChinh["colTranCode"] = dongQLTC["colTranCode"].ToString();
                    rowQuanLyTaiChinh["colTranCodeID"] = dongQLTC["colTranCodeID"].ToString();
                    rowQuanLyTaiChinh["colServiceID"] = dongQLTC["colServiceID"].ToString();
                    //thêm TypeID
                    rowQuanLyTaiChinh["colTypeID"] = SmartPortal.Constant.IPC.QUANLYTAICHINH;
                    rowQuanLyTaiChinh["colIBPolicy"] = dongQLTC["colIBPolicy"].ToString();
                    rowQuanLyTaiChinh["colSMSPolicy"] = dongQLTC["colSMSPolicy"].ToString();
                    rowQuanLyTaiChinh["colMBPolicy"] = dongQLTC["colMBPolicy"].ToString();
                    rowQuanLyTaiChinh["colpwdreset"] = dongQLTC["colpwdreset"].ToString();


                    tblSUM.Rows.Add(rowQuanLyTaiChinh);
                }
                //lấy thông tin trong bảng KETOAN
                foreach (DataRow ketoan in tblKETOAN.Rows)
                {
                    DataRow rowKeToan = tblSUM.NewRow();
                    rowKeToan["colFullName"] = ketoan["colFullName"].ToString();
                    rowKeToan["colLevel"] = ketoan["colLevel"].ToString();
                    rowKeToan["colBirthday"] = ketoan["colBirthday"].ToString();
                    rowKeToan["colGender"] = ketoan["colGender"].ToString();
                    rowKeToan["colPhone"] = ketoan["colPhone"].ToString();
                    rowKeToan["colEmail"] = ketoan["colEmail"].ToString();
                    rowKeToan["colAddress"] = ketoan["colAddress"].ToString();
                    rowKeToan["colIBUserName"] = ketoan["colIBUserName"].ToString();
                    rowKeToan["colIBPass"] = ketoan["colIBPass"].ToString();
                    rowKeToan["colSMSPhone"] = ketoan["colSMSPhone"].ToString();
                    rowKeToan["colSMSDefaultAcctno"] = ketoan["colSMSDefaultAcctno"].ToString();
                    rowKeToan["colSMSDefaultLang"] = ketoan["colSMSDefaultLang"].ToString();
                    rowKeToan["colSMSIsDefault"] = ketoan["colSMSIsDefault"].ToString();
                    rowKeToan["colSMSPinCode"] = ketoan["colSMSPinCode"].ToString();
                    rowKeToan["colMBPhone"] = ketoan["colMBPhone"].ToString();
                    rowKeToan["colMBPass"] = ketoan["colMBPass"].ToString();
                    rowKeToan["colMBPinCode"] = ketoan["colMBPinCode"].ToString();
                    rowKeToan["colPHOPhone"] = ketoan["colPHOPhone"].ToString();
                    rowKeToan["colPHOPass"] = ketoan["colPHOPass"].ToString();
                    rowKeToan["colPHODefaultAcctno"] = ketoan["colPHODefaultAcctno"].ToString();
                    rowKeToan["colAccount"] = ketoan["colAccount"].ToString();
                    rowKeToan["colRole"] = ketoan["colRole"].ToString();
                    rowKeToan["colRoleID"] = ketoan["colRoleID"].ToString();
                    rowKeToan["colTranCode"] = ketoan["colTranCode"].ToString();
                    rowKeToan["colTranCodeID"] = ketoan["colTranCodeID"].ToString();
                    rowKeToan["colServiceID"] = ketoan["colServiceID"].ToString();
                    //thêm TypeID
                    rowKeToan["colTypeID"] = SmartPortal.Constant.IPC.KETOAN;
                    rowKeToan["colIBPolicy"] = ketoan["colIBPolicy"].ToString();
                    rowKeToan["colSMSPolicy"] = ketoan["colSMSPolicy"].ToString();
                    rowKeToan["colMBPolicy"] = ketoan["colMBPolicy"].ToString();
                    rowKeToan["colpwdreset"] = ketoan["colpwdreset"].ToString();


                    tblSUM.Rows.Add(rowKeToan);
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
                DataColumn colNewTypeID = new DataColumn("colNewTypeID");

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
                tblUser.Columns.Add(colNewTypeID);

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
                        row2["colUserType"] = userType;
                        row2["colUserLevel"] = tblSUM.Rows[i]["colLevel"].ToString();
                        row2["colDeptID"] = deptID;
                        row2["colTokenID"] = tokenID;
                        row2["colTokenIssueDate"] = tokenIssueDate;
                        row2["colSMSOTP"] = smsOTP;
                        row2["colSMSBirthday"] = tblSUM.Rows[i]["colBirthday"].ToString();
                        row2["colNewTypeID"] = tblSUM.Rows[i]["colTypeID"].ToString();

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
                DataColumn colIBPolicyusrs = new DataColumn("colIBPolicyusrs");
                DataColumn colpwdresetIBs = new DataColumn("colpwdresetIBs");



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
                tblIbankUser.Columns.Add(colIBPolicyusrs);
                tblIbankUser.Columns.Add(colpwdresetIBs);


                //tao 1 dong du lieu
                string UN = "";
                foreach (DataRow rIBU in tblSUM.Rows)
                {
                    if (rIBU["colIBUserName"].ToString().Trim() != UN.Trim() && rIBU["colIBUserName"].ToString().Trim() != "")
                    {
                        DataRow row3 = tblIbankUser.NewRow();
                        row3["colUserName"] = rIBU["colIBUserName"].ToString();
                        row3["colIBUserID"] = rIBU["colIBUserName"].ToString();
                        row3["colIBPassword"] = rIBU["colIBPass"].ToString();
                        row3["colLastLoginTime"] = uCreateDate;
                        row3["colIBStatus"] = status;
                        row3["colIBUserCreate"] = userCreate;
                        row3["colIBDateCreate"] = uCreateDate;
                        row3["colIBUserModify"] = userCreate;
                        row3["colIBLastModify"] = lastModify;
                        row3["colIBUserApprove"] = userApprove;
                        row3["colIBIsLogin"] = "0";
                        row3["colIBDateExpire"] = endDate;
                        row3["colIBExpireTime"] = startDate;
                        row3["colIBPolicyusrs"] = rIBU["colIBPolicy"].ToString();
                        row3["colpwdresetIBs"] = rIBU["colpwdreset"].ToString();


                        tblIbankUser.Rows.Add(row3);

                        UN = rIBU["colIBUserName"].ToString();
                    }
                }
                #endregion

                #region Tạo bảng chứa User SMS
                DataTable tblSMSUser = new DataTable();
                DataColumn colSMSUserID = new DataColumn("colSMSUserID");
                DataColumn colSMSPhoneNo = new DataColumn("colSMSPhoneNo");
                DataColumn colSMSContractNo = new DataColumn("colSMSContractNo");
                DataColumn colSMSIsBroadcast = new DataColumn("colSMSIsBroadcast");
                DataColumn colSMSDefaultAcctnoU = new DataColumn("colSMSDefaultAcctno");
                DataColumn colSMSDefaultLangU = new DataColumn("colSMSDefaultLang");
                DataColumn colSMSIsDefault1 = new DataColumn("colSMSIsDefault1");
                DataColumn colSMSPinCode1 = new DataColumn("colSMSPinCode1");
                DataColumn colSMSStatus = new DataColumn("colSMSStatus");
                DataColumn colSMSPhoneType = new DataColumn("colSMSPhoneType");
                DataColumn colSMSUserCreate = new DataColumn("colSMSUserCreate");
                DataColumn colSMSUserModify = new DataColumn("colSMSUserModify");
                DataColumn colSMSUserApprove = new DataColumn("colSMSUserApprove");
                DataColumn colSMSLastModify = new DataColumn("colSMSLastModify");
                DataColumn colSMSDateCreate = new DataColumn("colSMSDateCreate");
                DataColumn colSMSDateExpire = new DataColumn("colSMSDateExpire");
                DataColumn colSMSPolicyusrs = new DataColumn("colSMSPolicyusrs");
                DataColumn colpwdresetSMSs = new DataColumn("colpwdresetSMSs");


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
                tblSMSUser.Columns.Add(colSMSPolicyusrs);
                tblSMSUser.Columns.Add(colpwdresetSMSs);


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
                        row4["colSMSIsBroadcast"] = "Y";
                        row4["colSMSDefaultAcctno"] = rSMSU["colSMSDefaultAcctno"].ToString();
                        row4["colSMSDefaultLang"] = rSMSU["colSMSDefaultLang"].ToString();
                        row4["colSMSIsDefault1"] = rSMSU["colSMSIsDefault"].ToString();
                        row4["colSMSPinCode1"] = SmartPortal.SEMS.O9Encryptpass.sha_sha256(rSMSU["colSMSPinCode"].ToString(), rSMSU["colSMSPhone"].ToString().Trim());
                        row4["colSMSStatus"] = status;
                        row4["colSMSPhoneType"] = "";
                        row4["colSMSUserCreate"] = userCreate;
                        row4["colSMSUserModify"] = userCreate;
                        row4["colSMSUserApprove"] = userApprove;
                        row4["colSMSLastModify"] = lastModify;
                        row4["colSMSDateCreate"] = uCreateDate;
                        row4["colSMSDateExpire"] = endDate;
                        row4["colSMSPolicyusrs"] = rSMSU["colSMSPolicy"].ToString();
                        row4["colpwdresetSMSs"] = Encryption.Encrypt(rSMSU["colSMSPinCode"].ToString());


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
                DataColumn colMBPolicyusrs = new DataColumn("colMBPolicyusrs");
                DataColumn colpwdresetMBs = new DataColumn("colpwdresetMBs");


                //add vào table
                tblMBUser.Columns.Add(colMBUserID);
                tblMBUser.Columns.Add(colMBPhoneNo);
                tblMBUser.Columns.Add(colMBPassU);
                tblMBUser.Columns.Add(colMBStatus);
                tblMBUser.Columns.Add(colMBPinCode1);
                tblMBUser.Columns.Add(colMBPolicyusrs);
                tblMBUser.Columns.Add(colpwdresetMBs);

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
                        row5["colMBPinCode1"] = rMBP["colMBPinCode"].ToString();
                        row5["colMBPolicyusrs"] = rMBP["colMBPolicy"].ToString();
                        row5["colpwdresetMBs"] = rMBP["colpwdreset"].ToString();


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
                DataColumn colPHODefaultAcctno1 = new DataColumn("colPHODefaultAcctno1");

                //add vào table
                tblPHOUser.Columns.Add(colPHOUserID);
                tblPHOUser.Columns.Add(colPHOPhoneNo);
                tblPHOUser.Columns.Add(colPHOPassU);
                tblPHOUser.Columns.Add(colPHOStatus);
                tblPHOUser.Columns.Add(colPHODefaultAcctno1);

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
                        row6["colPHOStatus"] = status;
                        row6["colPHODefaultAcctno1"] = rPHOP["colPHODefaultAcctno"].ToString();

                        tblPHOUser.Rows.Add(row6);
                        PHOP = rPHOP["colPHOPhone"].ToString();
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

                //#region lay tat ca cac account cua khach hang
                //DataSet ds = new DataSet();
                //if (System.Configuration.ConfigurationManager.AppSettings["AYACorporate"].ToString().Equals("0"))
                //{
                //    ds = new SmartPortal.SEMS.Customer().GetAcctNo(txtCustCode.Text.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                //}
                //else
                //{
                //    switch (ddlCustType.SelectedValue.Trim())
                //    {
                //        case SmartPortal.Constant.IPC.PERSONAL:
                //            ds = new SmartPortal.SEMS.Customer().GetAcctNo(txtCustCodeInfo.Text.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                //            break;
                //        case SmartPortal.Constant.IPC.PERSONALLKG:
                //            ds = new SmartPortal.SEMS.Customer().GetAcctNo(txtLkgCode.Text.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                //            break;
                //        case SmartPortal.Constant.IPC.CORPORATE:
                //            ds = new SmartPortal.SEMS.Customer().GetAcctNo(txtGrpCode.Text.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                //            break;
                //    }
                //}

                //if (IPCERRORCODE != "0")
                //{
                //    //goto ERROR;
                //}
                //DataTable dtAccount = new DataTable();
                //dtAccount = ds.Tables[0];
                //#endregion

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
                            newRowCA["colAcctType"] = "DD";
                            newRowCA["colCCYID"] = Resources.labels.lak;
                            newRowCA["colStatus"] = "A";
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

                //add vào table
                tblUserAccount.Columns.Add(colUserIDUC);
                tblUserAccount.Columns.Add(colAcctNoUC);
                tblUserAccount.Columns.Add(colRoleIDUC);
                tblUserAccount.Columns.Add(colUseFull);
                tblUserAccount.Columns.Add(colDesc);

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

                        tblUserAccount.Rows.Add(newRowUA);
                    }
                }
                #endregion

                #region Tạo bảng chứa phòng ban mặc định
                DataTable tblDeptDefault = new DataTable();

                DataColumn colDeptID1 = new DataColumn("colDeptID1");
                DataColumn colDeptName = new DataColumn("colDeptName");
                DataColumn colDeptDesc = new DataColumn("colDeptDesc");
                DataColumn colDeptContractNo = new DataColumn("colDeptContractNo");

                tblDeptDefault.Columns.Add(colDeptID1);
                tblDeptDefault.Columns.Add(colDeptName);
                tblDeptDefault.Columns.Add(colDeptDesc);
                tblDeptDefault.Columns.Add(colDeptContractNo);

                DataRow rowDept = tblDeptDefault.NewRow();
                rowDept["colDeptID1"] = SmartPortal.Constant.IPC.DEPTIDPREFIX + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8);
                rowDept["colDeptName"] = Resources.labels.all;
                rowDept["colDeptDesc"] = Resources.labels.chuathongtintatcanguoidungdoanhnghiep;
                rowDept["colDeptContractNo"] = contractNo;

                tblDeptDefault.Rows.Add(rowDept);

                #endregion

                #region Tạo bảng tạo quyền mặc định cho hợp đồng
                DataTable tblRoleDefault = new DataTable();

                DataColumn colRoleName = new DataColumn("colRoleName");
                DataColumn colRoleDesc = new DataColumn("colRoleDesc");
                DataColumn colUserCreated = new DataColumn("colUserCreated");
                DataColumn colRoleContractNo = new DataColumn("colRoleContractNo");
                DataColumn colRoleServiceID = new DataColumn("colRoleServiceID");
                DataColumn colRoleProductID = new DataColumn("colRoleProductID");

                tblRoleDefault.Columns.Add(colRoleName);
                tblRoleDefault.Columns.Add(colRoleDesc);
                tblRoleDefault.Columns.Add(colUserCreated);
                tblRoleDefault.Columns.Add(colRoleContractNo);
                tblRoleDefault.Columns.Add(colRoleServiceID);
                tblRoleDefault.Columns.Add(colRoleProductID);

                DataRow rowRoleIB = tblRoleDefault.NewRow();
                rowRoleIB["colRoleName"] = "Nhóm quyền sử dụng Internet Banking";
                rowRoleIB["colRoleDesc"] = "Nhóm quyền sử dụng Internet Banking";
                rowRoleIB["colUserCreated"] = userCreate;
                rowRoleIB["colRoleContractNo"] = contractNo;
                rowRoleIB["colRoleServiceID"] = SmartPortal.Constant.IPC.IB;
                rowRoleIB["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRoleSMS = tblRoleDefault.NewRow();
                rowRoleSMS["colRoleName"] = "Nhóm quyền sử dụng SMS Banking";
                rowRoleSMS["colRoleDesc"] = "Nhóm quyền sử dụng SMS Banking";
                rowRoleSMS["colUserCreated"] = userCreate;
                rowRoleSMS["colRoleContractNo"] = contractNo;
                rowRoleSMS["colRoleServiceID"] = SmartPortal.Constant.IPC.SMS;
                rowRoleSMS["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRoleMB = tblRoleDefault.NewRow();
                rowRoleMB["colRoleName"] = "Nhóm quyền sử dụng Mobile Banking";
                rowRoleMB["colRoleDesc"] = "Nhóm quyền sử dụng Mobile Banking";
                rowRoleMB["colUserCreated"] = userCreate;
                rowRoleMB["colRoleContractNo"] = contractNo;
                rowRoleMB["colRoleServiceID"] = SmartPortal.Constant.IPC.MB;
                rowRoleMB["colRoleProductID"] = ddlProduct.SelectedValue;

                DataRow rowRolePHO = tblRoleDefault.NewRow();
                rowRolePHO["colRoleName"] = "Nhóm quyền sử dụng Phone Banking";
                rowRolePHO["colRoleDesc"] = "Nhóm quyền sử dụng Phone Banking";
                rowRolePHO["colUserCreated"] = userCreate;
                rowRolePHO["colRoleContractNo"] = contractNo;
                rowRolePHO["colRoleServiceID"] = SmartPortal.Constant.IPC.PHO;
                rowRolePHO["colRoleProductID"] = ddlProduct.SelectedValue;

                tblRoleDefault.Rows.Add(rowRoleIB);
                tblRoleDefault.Rows.Add(rowRoleSMS);
                tblRoleDefault.Rows.Add(rowRoleMB);
                tblRoleDefault.Rows.Add(rowRolePHO);
                #endregion

                #region vutt Tạo bảng chứa thông tin sms notify 04022016

                DataTable tblSMSNotify = ContractControl.CreateSMSNotifyDetailTable(tblUserAccount, tblContractAccount, "I", contractNo);

                #endregion

                #region INSERT
                //08.12.2015 minh add corptype
                //new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, (chkRenew.Checked ? "Y" : "N"),ddlCorpType.SelectedValue.ToString(), tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount, tblDeptDefault, tblRoleDefault, ref IPCERRORCODE, ref IPCERRORDESC);
                //01102016 sms notification
                new SmartPortal.SEMS.Contract().InsertCorp(branchID, custID, contractNo, contractType, productID, startDate, endDate, lastModify, userCreate, userLastModify, userApprove, status, allAcct, isSpecialMan, (chkRenew.Checked ? "Y" : "N"), ddlCorpType.SelectedValue.ToString(), tblUser, tblIbankUser, tblSMSUser, tblMBUser, tblPHOUser, tblIbankUserRight, tblSMSUserRight, tblMBUserRight, tblPHOUserRight, tblContractRoleDetail, tblContractAccount, tblTranrightDetail, tblUserAccount, tblDeptDefault, tblRoleDefault, tblSMSNotify, ref IPCERRORCODE, ref IPCERRORDESC);

                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                else
                {

                }

                #endregion
                #endregion

            }



        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnCustSave_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnCustSave_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
        SendInfoLogin();
        ReleaseSession();
        Response.Redirect(SmartPortal.Common.Encrypt.EncryptURL("~/Default.aspx?p=183&returnURL=" + SmartPortal.Common.Encrypt.EncryptData(Request.Url.Query)));
    }

    void SendInfoLoginold()
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

            if (ViewState["NGUOIQUANTRI"] != null)
            {
                DataTable tblNGUOIQUANTRI = (DataTable)ViewState["NGUOIQUANTRI"];
                if (tblNGUOIQUANTRI.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi quan tri
                    DataTable nqtTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.QUANTRIHETHONG, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stNQT = new StringBuilder();
                    stNQT.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiquantrihethong.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in nqtTable.Rows)
                    {

                        stNQT.Append("<table style='width:100%;'>");


                        stNQT.Append("<tr>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(Resources.labels.tendaydu + " ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(row["FULLNAME"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append("Email ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(row["EMAIL"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("</tr>");

                        stNQT.Append("<tr>");
                        stNQT.Append("<td>");
                        stNQT.Append(Resources.labels.dienthoai + " ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append(row["PHONE"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append("");
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append("");
                        stNQT.Append("</td>");
                        stNQT.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(),string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);
                        
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTableNQT = accountIBDatasetNQT.Tables[0];
                        if (accountIBTableNQT.Rows.Count != 0)
                        {
                            if (accountIBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.internetbanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountIBTableNQT.Rows[0]["USERNAME"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTableNQT = accountSMSDatasetNQT.Tables[0];
                        if (accountSMSTableNQT.Rows.Count != 0)
                        {
                            if (accountSMSTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.smsbanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.sodienthoai + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountSMSTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.taikhoanmacdinh + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountSMSTableNQT.Rows[0]["DEFAULTACCTNO"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTableNQT = accountMBDatasetNQT.Tables[0];
                        if (accountMBTableNQT.Rows.Count != 0)
                        {
                            if (accountMBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.mobilebanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountMBTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }

                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTableNQT = accountPHODatasetNQT.Tables[0];
                        if (accountPHOTableNQT.Rows.Count != 0)
                        {
                            if (accountPHOTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.phonebanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountPHOTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        stNQT.Append("</table>");
                        j += 1;
                        if (j < nqtTable.Rows.Count)
                        {
                            stNQT.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("USERINFO", stNQT.ToString());

                    #endregion

                }
            }


            if (ViewState["CHUTAIKHOAN"] != null)
            {
                DataTable tblCHUTAIKHOAN = (DataTable)ViewState["CHUTAIKHOAN"];
                if (tblCHUTAIKHOAN.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable ctkTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.CHUTAIKHOAN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stctk = new StringBuilder();
                    stctk.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinchutaikhoan.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in ctkTable.Rows)
                    {

                        stctk.Append("<table style='width:100%;'>");


                        stctk.Append("<tr>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(Resources.labels.tendaydu + " ");
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(row["FULLNAME"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append("Email ");
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(row["EMAIL"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("</tr>");

                        stctk.Append("<tr>");
                        stctk.Append("<td>");
                        stctk.Append(Resources.labels.dienthoai + " ");
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append(row["PHONE"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append("");
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append("");
                        stctk.Append("</td>");
                        stctk.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablectk = accountIBDatasetctk.Tables[0];
                        if (accountIBTablectk.Rows.Count != 0)
                        {
                            if (accountIBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Internet Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountIBTablectk.Rows[0]["USERNAME"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablectk = accountSMSDatasetctk.Tables[0];
                        if (accountSMSTablectk.Rows.Count != 0)
                        {
                            if (accountSMSTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>SMS Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.sodienthoai + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountSMSTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.taikhoanmacdinh + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountSMSTablectk.Rows[0]["DEFAULTACCTNO"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablectk = accountMBDatasetctk.Tables[0];
                        if (accountMBTablectk.Rows.Count != 0)
                        {
                            if (accountMBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Mobile Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountMBTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }

                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablectk = accountPHODatasetctk.Tables[0];
                        if (accountPHOTablectk.Rows.Count != 0)
                        {
                            if (accountPHOTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Phone Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountPHOTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        stctk.Append("</table>");
                        j += 1;
                        if (j < ctkTable.Rows.Count)
                        {
                            stctk.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("NGUOIUYQUYEN", stctk.ToString());

                    #endregion

                }
            }


            if (ViewState["NGUOIUYQUYEN"] != null)
            {
                DataTable tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                if (tblNGUOIUYQUYEN.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable nuqTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIUYQUYEN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stnuq = new StringBuilder();
                    stnuq.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiuyquyen.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in nuqTable.Rows)
                    {

                        stnuq.Append("<table style='width:100%;'>");


                        stnuq.Append("<tr>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(Resources.labels.tendaydu + " ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(row["FULLNAME"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append("Email ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(row["EMAIL"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("</tr>");

                        stnuq.Append("<tr>");
                        stnuq.Append("<td>");
                        stnuq.Append(Resources.labels.dienthoai + " ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append(row["PHONE"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append("");
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append("");
                        stnuq.Append("</td>");
                        stnuq.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablenuq = accountIBDatasetnuq.Tables[0];
                        if (accountIBTablenuq.Rows.Count != 0)
                        {
                            if (accountIBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Internet Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountIBTablenuq.Rows[0]["USERNAME"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablenuq = accountSMSDatasetnuq.Tables[0];
                        if (accountSMSTablenuq.Rows.Count != 0)
                        {
                            if (accountSMSTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>SMS Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.sodienthoai + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountSMSTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.taikhoanmacdinh + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountSMSTablenuq.Rows[0]["DEFAULTACCTNO"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablenuq = accountMBDatasetnuq.Tables[0];
                        if (accountMBTablenuq.Rows.Count != 0)
                        {
                            if (accountMBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Mobile Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountMBTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                            }
                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablenuq = accountPHODatasetnuq.Tables[0];
                        if (accountPHOTablenuq.Rows.Count != 0)
                        {
                            if (accountPHOTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Phone Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountPHOTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        stnuq.Append("</table>");
                        j += 1;
                        if (j < nuqTable.Rows.Count)
                        {
                            stnuq.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("NUQ", stnuq.ToString());

                    #endregion

                }
            }

            if (ViewState["LEVEL2"] != null)
            {
                DataTable tblLEVEL2 = (DataTable)ViewState["LEVEL2"];
                if (tblLEVEL2.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable l2Table = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIDUNGCAP2, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stl2 = new StringBuilder();
                    stl2.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoidungcaphai.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in l2Table.Rows)
                    {

                        stl2.Append("<table style='width:100%;'>");


                        stl2.Append("<tr>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(Resources.labels.tendaydu + " ");
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(row["FULLNAME"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append("Email ");
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(row["EMAIL"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("</tr>");

                        stl2.Append("<tr>");
                        stl2.Append("<td>");
                        stl2.Append(Resources.labels.dienthoai + " ");
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append(row["PHONE"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append("");
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append("");
                        stl2.Append("</td>");
                        stl2.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablel2 = accountIBDatasetl2.Tables[0];
                        if (accountIBTablel2.Rows.Count != 0)
                        {
                            if (accountIBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Internet Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountIBTablel2.Rows[0]["USERNAME"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablel2 = accountSMSDatasetl2.Tables[0];
                        if (accountSMSTablel2.Rows.Count != 0)
                        {
                            if (accountSMSTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>SMS Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.sodienthoai + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountSMSTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.taikhoanmacdinh + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountSMSTablel2.Rows[0]["DEFAULTACCTNO"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablel2 = accountMBDatasetl2.Tables[0];
                        if (accountMBTablel2.Rows.Count != 0)
                        {
                            if (accountMBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Mobile Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountMBTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                            }
                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablel2 = accountPHODatasetl2.Tables[0];
                        if (accountPHOTablel2.Rows.Count != 0)
                        {
                            if (accountPHOTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Phone Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountPHOTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        stl2.Append("</table>");
                        j += 1;
                        if (j < l2Table.Rows.Count)
                        {
                            stl2.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("LEVEL2", stl2.ToString());

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
    void SendInfoLoginold02122015()
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

            if (ViewState["NGUOIQUANTRI"] != null)
            {
                DataTable tblNGUOIQUANTRI = (DataTable)ViewState["NGUOIQUANTRI"];
                if (tblNGUOIQUANTRI.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi quan tri
                    DataTable nqtTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.QUANTRIHETHONG, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stNQT = new StringBuilder();
                    stNQT.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiquantrihethong.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in nqtTable.Rows)
                    {

                        stNQT.Append("<table style='width:100%;'>");


                        stNQT.Append("<tr>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(Resources.labels.tendaydu + " ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(row["FULLNAME"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append("Email ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td width='25%'>");
                        stNQT.Append(row["EMAIL"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("</tr>");

                        stNQT.Append("<tr>");
                        stNQT.Append("<td>");
                        stNQT.Append(Resources.labels.dienthoai + " ");
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append(row["PHONE"].ToString());
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append("");
                        stNQT.Append("</td>");
                        stNQT.Append("<td>");
                        stNQT.Append("");
                        stNQT.Append("</td>");
                        stNQT.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTableNQT = accountIBDatasetNQT.Tables[0];
                        if (accountIBTableNQT.Rows.Count != 0)
                        {
                            if (accountIBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.internetbanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountIBTableNQT.Rows[0]["USERNAME"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTableNQT = accountSMSDatasetNQT.Tables[0];
                        if (accountSMSTableNQT.Rows.Count != 0)
                        {
                            if (accountSMSTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.smsbanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.sodienthoai + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountSMSTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.taikhoanmacdinh + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountSMSTableNQT.Rows[0]["DEFAULTACCTNO"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTableNQT = accountMBDatasetNQT.Tables[0];
                        if (accountMBTableNQT.Rows.Count != 0)
                        {
                            if (accountMBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.mobilebanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountMBTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }

                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTableNQT = accountPHODatasetNQT.Tables[0];
                        if (accountPHOTableNQT.Rows.Count != 0)
                        {
                            if (accountPHOTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<br/>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td colspan='4'>");
                                stNQT.Append("<B>" + Resources.labels.phonebanking + "</B>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");

                                stNQT.Append("<tr>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.tendangnhap + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(accountPHOTableNQT.Rows[0]["UN"].ToString());
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append(Resources.labels.matkhau + " :");
                                stNQT.Append("</td>");
                                stNQT.Append("<td width='25%'>");
                                stNQT.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stNQT.Append("</td>");
                                stNQT.Append("</tr>");
                            }
                        }

                        stNQT.Append("</table>");
                        j += 1;
                        if (j < nqtTable.Rows.Count)
                        {
                            stNQT.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("USERINFO", stNQT.ToString());

                    #endregion

                }
            }


            if (ViewState["CHUTAIKHOAN"] != null)
            {
                DataTable tblCHUTAIKHOAN = (DataTable)ViewState["CHUTAIKHOAN"];
                if (tblCHUTAIKHOAN.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable ctkTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.CHUTAIKHOAN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stctk = new StringBuilder();
                    stctk.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinchutaikhoan.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in ctkTable.Rows)
                    {

                        stctk.Append("<table style='width:100%;'>");


                        stctk.Append("<tr>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(Resources.labels.tendaydu + " ");
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(row["FULLNAME"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append("Email ");
                        stctk.Append("</td>");
                        stctk.Append("<td width='25%'>");
                        stctk.Append(row["EMAIL"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("</tr>");

                        stctk.Append("<tr>");
                        stctk.Append("<td>");
                        stctk.Append(Resources.labels.dienthoai + " ");
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append(row["PHONE"].ToString());
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append("");
                        stctk.Append("</td>");
                        stctk.Append("<td>");
                        stctk.Append("");
                        stctk.Append("</td>");
                        stctk.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablectk = accountIBDatasetctk.Tables[0];
                        if (accountIBTablectk.Rows.Count != 0)
                        {
                            if (accountIBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Internet Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountIBTablectk.Rows[0]["USERNAME"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablectk = accountSMSDatasetctk.Tables[0];
                        if (accountSMSTablectk.Rows.Count != 0)
                        {
                            if (accountSMSTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>SMS Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.sodienthoai + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountSMSTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.taikhoanmacdinh + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountSMSTablectk.Rows[0]["DEFAULTACCTNO"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablectk = accountMBDatasetctk.Tables[0];
                        if (accountMBTablectk.Rows.Count != 0)
                        {
                            if (accountMBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Mobile Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountMBTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }

                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablectk = accountPHODatasetctk.Tables[0];
                        if (accountPHOTablectk.Rows.Count != 0)
                        {
                            if (accountPHOTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<br/>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td colspan='4'>");
                                stctk.Append("<B>Phone Banking</B>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");

                                stctk.Append("<tr>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.tendangnhap + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(accountPHOTablectk.Rows[0]["UN"].ToString());
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append(Resources.labels.matkhau + " :");
                                stctk.Append("</td>");
                                stctk.Append("<td width='25%'>");
                                stctk.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stctk.Append("</td>");
                                stctk.Append("</tr>");
                            }
                        }

                        stctk.Append("</table>");
                        j += 1;
                        if (j < ctkTable.Rows.Count)
                        {
                            stctk.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("NGUOIUYQUYEN", stctk.ToString());

                    #endregion

                }
            }


            if (ViewState["NGUOIUYQUYEN"] != null)
            {
                DataTable tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                if (tblNGUOIUYQUYEN.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable nuqTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIUYQUYEN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stnuq = new StringBuilder();
                    stnuq.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiuyquyen.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in nuqTable.Rows)
                    {

                        stnuq.Append("<table style='width:100%;'>");


                        stnuq.Append("<tr>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(Resources.labels.tendaydu + " ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(row["FULLNAME"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append("Email ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td width='25%'>");
                        stnuq.Append(row["EMAIL"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("</tr>");

                        stnuq.Append("<tr>");
                        stnuq.Append("<td>");
                        stnuq.Append(Resources.labels.dienthoai + " ");
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append(row["PHONE"].ToString());
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append("");
                        stnuq.Append("</td>");
                        stnuq.Append("<td>");
                        stnuq.Append("");
                        stnuq.Append("</td>");
                        stnuq.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablenuq = accountIBDatasetnuq.Tables[0];
                        if (accountIBTablenuq.Rows.Count != 0)
                        {
                            if (accountIBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Internet Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountIBTablenuq.Rows[0]["USERNAME"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablenuq = accountSMSDatasetnuq.Tables[0];
                        if (accountSMSTablenuq.Rows.Count != 0)
                        {
                            if (accountSMSTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>SMS Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.sodienthoai + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountSMSTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.taikhoanmacdinh + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountSMSTablenuq.Rows[0]["DEFAULTACCTNO"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablenuq = accountMBDatasetnuq.Tables[0];
                        if (accountMBTablenuq.Rows.Count != 0)
                        {
                            if (accountMBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Mobile Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountMBTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                            }
                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablenuq = accountPHODatasetnuq.Tables[0];
                        if (accountPHOTablenuq.Rows.Count != 0)
                        {
                            if (accountPHOTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<br/>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td colspan='4'>");
                                stnuq.Append("<B>Phone Banking</B>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");

                                stnuq.Append("<tr>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.tendangnhap + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(accountPHOTablenuq.Rows[0]["UN"].ToString());
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append(Resources.labels.matkhau + " :");
                                stnuq.Append("</td>");
                                stnuq.Append("<td width='25%'>");
                                stnuq.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stnuq.Append("</td>");
                                stnuq.Append("</tr>");
                            }
                        }

                        stnuq.Append("</table>");
                        j += 1;
                        if (j < nuqTable.Rows.Count)
                        {
                            stnuq.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("NUQ", stnuq.ToString());

                    #endregion

                }
            }

            if (ViewState["LEVEL2"] != null)
            {
                DataTable tblLEVEL2 = (DataTable)ViewState["LEVEL2"];
                if (tblLEVEL2.Rows.Count != 0)
                {

                    #region lay thong tin tai khoan cua nguoi dong so huu
                    DataTable l2Table = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIDUNGCAP2, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                    if (IPCERRORCODE != "0")
                    {
                        goto ERROR;
                    }

                    StringBuilder stl2 = new StringBuilder();
                    stl2.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoidungcaphai.ToUpper() + "</div>");
                    //gan thong tin user vao stringtemplate
                    int j = 0;
                    foreach (DataRow row in l2Table.Rows)
                    {

                        stl2.Append("<table style='width:100%;'>");


                        stl2.Append("<tr>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(Resources.labels.tendaydu + " ");
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(row["FULLNAME"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append("Email ");
                        stl2.Append("</td>");
                        stl2.Append("<td width='25%'>");
                        stl2.Append(row["EMAIL"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("</tr>");

                        stl2.Append("<tr>");
                        stl2.Append("<td>");
                        stl2.Append(Resources.labels.dienthoai + " ");
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append(row["PHONE"].ToString());
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append("");
                        stl2.Append("</td>");
                        stl2.Append("<td>");
                        stl2.Append("");
                        stl2.Append("</td>");
                        stl2.Append("</tr>");


                        //lay het các tai khoan Ibank cua user theo userID
                        DataSet accountIBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountIBTablel2 = accountIBDatasetl2.Tables[0];
                        if (accountIBTablel2.Rows.Count != 0)
                        {
                            if (accountIBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Internet Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountIBTablel2.Rows[0]["USERNAME"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan SMS cua user theo userID
                        DataSet accountSMSDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountSMSTablel2 = accountSMSDatasetl2.Tables[0];
                        if (accountSMSTablel2.Rows.Count != 0)
                        {
                            if (accountSMSTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>SMS Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.sodienthoai + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountSMSTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.taikhoanmacdinh + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountSMSTablel2.Rows[0]["DEFAULTACCTNO"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        //lay het các tai khoan MB cua user theo userID
                        DataSet accountMBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountMBTablel2 = accountMBDatasetl2.Tables[0];
                        if (accountMBTablel2.Rows.Count != 0)
                        {
                            if (accountMBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Mobile Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountMBTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }

                        }

                        //lay het các tai khoan PHO cua user theo userID
                        DataSet accountPHODatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        DataTable accountPHOTablel2 = accountPHODatasetl2.Tables[0];
                        if (accountPHOTablel2.Rows.Count != 0)
                        {
                            if (accountPHOTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                            {
                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<br/>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td colspan='4'>");
                                stl2.Append("<B>Phone Banking</B>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");

                                stl2.Append("<tr>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.tendangnhap + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(accountPHOTablel2.Rows[0]["UN"].ToString());
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append(Resources.labels.matkhau + " :");
                                stl2.Append("</td>");
                                stl2.Append("<td width='25%'>");
                                stl2.Append("########");
                                //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                stl2.Append("</td>");
                                stl2.Append("</tr>");
                            }
                        }

                        stl2.Append("</table>");
                        j += 1;
                        if (j < l2Table.Rows.Count)
                        {
                            stl2.Append("<hr/>");
                        }
                    }
                    tmpl.SetAttribute("LEVEL2", stl2.ToString());

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
            string typeid = string.Empty;
            string usertype = string.Empty;
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

            //29.10.2015 minh tach advance and simple
            if (ddlCorpType.SelectedValue.Equals(SmartPortal.Constant.IPC.CONTRACTCORPADVANCE))
            {
                #region advance


                if (ViewState["NGUOIQUANTRI"] != null)
                {
                    DataTable tblNGUOIQUANTRI = (DataTable)ViewState["NGUOIQUANTRI"];
                    if (tblNGUOIQUANTRI.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi quan tri
                        DataTable nqtTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.QUANTRIHETHONG, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stNQT = new StringBuilder();
                        stNQT.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiquantrihethong.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in nqtTable.Rows)
                        {

                            stNQT.Append("<table style='width:100%;'>");


                            stNQT.Append("<tr>");
                            stNQT.Append("<td width='25%'>");
                            stNQT.Append(Resources.labels.tendaydu + " ");
                            stNQT.Append("</td>");
                            stNQT.Append("<td width='25%'>");
                            stNQT.Append(row["FULLNAME"].ToString());
                            stNQT.Append("</td>");
                            stNQT.Append("<td width='25%'>");
                            stNQT.Append("Email ");
                            stNQT.Append("</td>");
                            stNQT.Append("<td width='25%'>");
                            stNQT.Append(row["EMAIL"].ToString());
                            stNQT.Append("</td>");
                            stNQT.Append("</tr>");

                            stNQT.Append("<tr>");
                            stNQT.Append("<td>");
                            stNQT.Append(Resources.labels.dienthoai + " ");
                            stNQT.Append("</td>");
                            stNQT.Append("<td>");
                            stNQT.Append(row["PHONE"].ToString());
                            stNQT.Append("</td>");
                            stNQT.Append("<td>");
                            stNQT.Append("");
                            stNQT.Append("</td>");
                            stNQT.Append("<td>");
                            stNQT.Append("");
                            stNQT.Append("</td>");
                            stNQT.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTableNQT = accountIBDatasetNQT.Tables[0];
                            if (accountIBTableNQT.Rows.Count != 0)
                            {
                                if (accountIBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<br/>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<B>" + Resources.labels.internetbanking + "</B>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.tendangnhap + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(accountIBTableNQT.Rows[0]["USERNAME"].ToString());
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.matkhau + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTableNQT = accountSMSDatasetNQT.Tables[0];
                            if (accountSMSTableNQT.Rows.Count != 0)
                            {
                                if (accountSMSTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<br/>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<B>" + Resources.labels.smsbanking + "</B>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.sodienthoai + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(accountSMSTableNQT.Rows[0]["UN"].ToString());
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(accountSMSTableNQT.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTableNQT = accountMBDatasetNQT.Tables[0];
                            if (accountMBTableNQT.Rows.Count != 0)
                            {
                                if (accountMBTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<br/>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<B>" + Resources.labels.mobilebanking + "</B>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.tendangnhap + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(accountMBTableNQT.Rows[0]["UN"].ToString());
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.matkhau + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");
                                }

                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetNQT = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTableNQT = accountPHODatasetNQT.Tables[0];
                            if (accountPHOTableNQT.Rows.Count != 0)
                            {
                                if (accountPHOTableNQT.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<br/>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td colspan='4'>");
                                    stNQT.Append("<B>" + Resources.labels.phonebanking + "</B>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");

                                    stNQT.Append("<tr>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.tendangnhap + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(accountPHOTableNQT.Rows[0]["UN"].ToString());
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append(Resources.labels.matkhau + " :");
                                    stNQT.Append("</td>");
                                    stNQT.Append("<td width='25%'>");
                                    stNQT.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stNQT.Append("</td>");
                                    stNQT.Append("</tr>");
                                }
                            }

                            stNQT.Append("</table>");
                            j += 1;
                            if (j < nqtTable.Rows.Count)
                            {
                                stNQT.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("USERINFO", stNQT.ToString());

                        #endregion

                    }
                }


                if (ViewState["CHUTAIKHOAN"] != null)
                {
                    DataTable tblCHUTAIKHOAN = (DataTable)ViewState["CHUTAIKHOAN"];
                    if (tblCHUTAIKHOAN.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable ctkTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.CHUTAIKHOAN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stctk = new StringBuilder();
                        stctk.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinchutaikhoan.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in ctkTable.Rows)
                        {

                            stctk.Append("<table style='width:100%;'>");


                            stctk.Append("<tr>");
                            stctk.Append("<td width='25%'>");
                            stctk.Append(Resources.labels.tendaydu + " ");
                            stctk.Append("</td>");
                            stctk.Append("<td width='25%'>");
                            stctk.Append(row["FULLNAME"].ToString());
                            stctk.Append("</td>");
                            stctk.Append("<td width='25%'>");
                            stctk.Append("Email ");
                            stctk.Append("</td>");
                            stctk.Append("<td width='25%'>");
                            stctk.Append(row["EMAIL"].ToString());
                            stctk.Append("</td>");
                            stctk.Append("</tr>");

                            stctk.Append("<tr>");
                            stctk.Append("<td>");
                            stctk.Append(Resources.labels.dienthoai + " ");
                            stctk.Append("</td>");
                            stctk.Append("<td>");
                            stctk.Append(row["PHONE"].ToString());
                            stctk.Append("</td>");
                            stctk.Append("<td>");
                            stctk.Append("");
                            stctk.Append("</td>");
                            stctk.Append("<td>");
                            stctk.Append("");
                            stctk.Append("</td>");
                            stctk.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablectk = accountIBDatasetctk.Tables[0];
                            if (accountIBTablectk.Rows.Count != 0)
                            {
                                if (accountIBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<br/>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<B>Internet Banking</B>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.tendangnhap + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(accountIBTablectk.Rows[0]["USERNAME"].ToString());
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.matkhau + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablectk = accountSMSDatasetctk.Tables[0];
                            if (accountSMSTablectk.Rows.Count != 0)
                            {
                                if (accountSMSTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<br/>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<B>SMS Banking</B>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.sodienthoai + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(accountSMSTablectk.Rows[0]["UN"].ToString());
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(accountSMSTablectk.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablectk = accountMBDatasetctk.Tables[0];
                            if (accountMBTablectk.Rows.Count != 0)
                            {
                                if (accountMBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<br/>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<B>Mobile Banking</B>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.tendangnhap + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(accountMBTablectk.Rows[0]["UN"].ToString());
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.matkhau + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");
                                }

                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablectk = accountPHODatasetctk.Tables[0];
                            if (accountPHOTablectk.Rows.Count != 0)
                            {
                                if (accountPHOTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<br/>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td colspan='4'>");
                                    stctk.Append("<B>Phone Banking</B>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");

                                    stctk.Append("<tr>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.tendangnhap + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(accountPHOTablectk.Rows[0]["UN"].ToString());
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append(Resources.labels.matkhau + " :");
                                    stctk.Append("</td>");
                                    stctk.Append("<td width='25%'>");
                                    stctk.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stctk.Append("</td>");
                                    stctk.Append("</tr>");
                                }
                            }

                            stctk.Append("</table>");
                            j += 1;
                            if (j < ctkTable.Rows.Count)
                            {
                                stctk.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("NGUOIUYQUYEN", stctk.ToString());

                        #endregion

                    }
                }


                if (ViewState["NGUOIUYQUYEN"] != null)
                {
                    DataTable tblNGUOIUYQUYEN = (DataTable)ViewState["NGUOIUYQUYEN"];
                    if (tblNGUOIUYQUYEN.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable nuqTable = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIUYQUYEN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stnuq = new StringBuilder();
                        stnuq.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiuyquyen.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in nuqTable.Rows)
                        {

                            stnuq.Append("<table style='width:100%;'>");


                            stnuq.Append("<tr>");
                            stnuq.Append("<td width='25%'>");
                            stnuq.Append(Resources.labels.tendaydu + " ");
                            stnuq.Append("</td>");
                            stnuq.Append("<td width='25%'>");
                            stnuq.Append(row["FULLNAME"].ToString());
                            stnuq.Append("</td>");
                            stnuq.Append("<td width='25%'>");
                            stnuq.Append("Email ");
                            stnuq.Append("</td>");
                            stnuq.Append("<td width='25%'>");
                            stnuq.Append(row["EMAIL"].ToString());
                            stnuq.Append("</td>");
                            stnuq.Append("</tr>");

                            stnuq.Append("<tr>");
                            stnuq.Append("<td>");
                            stnuq.Append(Resources.labels.dienthoai + " ");
                            stnuq.Append("</td>");
                            stnuq.Append("<td>");
                            stnuq.Append(row["PHONE"].ToString());
                            stnuq.Append("</td>");
                            stnuq.Append("<td>");
                            stnuq.Append("");
                            stnuq.Append("</td>");
                            stnuq.Append("<td>");
                            stnuq.Append("");
                            stnuq.Append("</td>");
                            stnuq.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablenuq = accountIBDatasetnuq.Tables[0];
                            if (accountIBTablenuq.Rows.Count != 0)
                            {
                                if (accountIBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<br/>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<B>Internet Banking</B>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.tendangnhap + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(accountIBTablenuq.Rows[0]["USERNAME"].ToString());
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.matkhau + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablenuq = accountSMSDatasetnuq.Tables[0];
                            if (accountSMSTablenuq.Rows.Count != 0)
                            {
                                if (accountSMSTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<br/>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<B>SMS Banking</B>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.sodienthoai + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(accountSMSTablenuq.Rows[0]["UN"].ToString());
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(accountSMSTablenuq.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablenuq = accountMBDatasetnuq.Tables[0];
                            if (accountMBTablenuq.Rows.Count != 0)
                            {
                                if (accountMBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<br/>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<B>Mobile Banking</B>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.tendangnhap + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(accountMBTablenuq.Rows[0]["UN"].ToString());
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.matkhau + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                }
                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablenuq = accountPHODatasetnuq.Tables[0];
                            if (accountPHOTablenuq.Rows.Count != 0)
                            {
                                if (accountPHOTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<br/>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td colspan='4'>");
                                    stnuq.Append("<B>Phone Banking</B>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");

                                    stnuq.Append("<tr>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.tendangnhap + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(accountPHOTablenuq.Rows[0]["UN"].ToString());
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append(Resources.labels.matkhau + " :");
                                    stnuq.Append("</td>");
                                    stnuq.Append("<td width='25%'>");
                                    stnuq.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stnuq.Append("</td>");
                                    stnuq.Append("</tr>");
                                }
                            }

                            stnuq.Append("</table>");
                            j += 1;
                            if (j < nuqTable.Rows.Count)
                            {
                                stnuq.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("NUQ", stnuq.ToString());

                        #endregion

                    }
                }

                if (ViewState["LEVEL2"] != null)
                {
                    DataTable tblLEVEL2 = (DataTable)ViewState["LEVEL2"];
                    if (tblLEVEL2.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable l2Table = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.NGUOIDUNGCAP2, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stl2 = new StringBuilder();
                        stl2.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoidungcaphai.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in l2Table.Rows)
                        {

                            stl2.Append("<table style='width:100%;'>");


                            stl2.Append("<tr>");
                            stl2.Append("<td width='25%'>");
                            stl2.Append(Resources.labels.tendaydu + " ");
                            stl2.Append("</td>");
                            stl2.Append("<td width='25%'>");
                            stl2.Append(row["FULLNAME"].ToString());
                            stl2.Append("</td>");
                            stl2.Append("<td width='25%'>");
                            stl2.Append("Email ");
                            stl2.Append("</td>");
                            stl2.Append("<td width='25%'>");
                            stl2.Append(row["EMAIL"].ToString());
                            stl2.Append("</td>");
                            stl2.Append("</tr>");

                            stl2.Append("<tr>");
                            stl2.Append("<td>");
                            stl2.Append(Resources.labels.dienthoai + " ");
                            stl2.Append("</td>");
                            stl2.Append("<td>");
                            stl2.Append(row["PHONE"].ToString());
                            stl2.Append("</td>");
                            stl2.Append("<td>");
                            stl2.Append("");
                            stl2.Append("</td>");
                            stl2.Append("<td>");
                            stl2.Append("");
                            stl2.Append("</td>");
                            stl2.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablel2 = accountIBDatasetl2.Tables[0];
                            if (accountIBTablel2.Rows.Count != 0)
                            {
                                if (accountIBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<br/>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<B>Internet Banking</B>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.tendangnhap + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(accountIBTablel2.Rows[0]["USERNAME"].ToString());
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.matkhau + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablel2 = accountSMSDatasetl2.Tables[0];
                            if (accountSMSTablel2.Rows.Count != 0)
                            {
                                if (accountSMSTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<br/>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<B>SMS Banking</B>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.sodienthoai + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(accountSMSTablel2.Rows[0]["UN"].ToString());
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(accountSMSTablel2.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablel2 = accountMBDatasetl2.Tables[0];
                            if (accountMBTablel2.Rows.Count != 0)
                            {
                                if (accountMBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<br/>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<B>Mobile Banking</B>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.tendangnhap + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(accountMBTablel2.Rows[0]["UN"].ToString());
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.matkhau + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");
                                }

                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablel2 = accountPHODatasetl2.Tables[0];
                            if (accountPHOTablel2.Rows.Count != 0)
                            {
                                if (accountPHOTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<br/>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td colspan='4'>");
                                    stl2.Append("<B>Phone Banking</B>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");

                                    stl2.Append("<tr>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.tendangnhap + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(accountPHOTablel2.Rows[0]["UN"].ToString());
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append(Resources.labels.matkhau + " :");
                                    stl2.Append("</td>");
                                    stl2.Append("<td width='25%'>");
                                    stl2.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stl2.Append("</td>");
                                    stl2.Append("</tr>");
                                }
                            }

                            stl2.Append("</table>");
                            j += 1;
                            if (j < l2Table.Rows.Count)
                            {
                                stl2.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("LEVEL2", stl2.ToString());

                        #endregion

                    }
                }

                #endregion
            }
            else
            {
                #region simple
                if (ViewState["CHUTAIKHOANS"] != null)
                {
                    DataTable tblCHUTAIKHOANs = (DataTable)ViewState["CHUTAIKHOANS"];
                    if (tblCHUTAIKHOANs.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable ctkTables = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.CHUTAIKHOAN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stctks = new StringBuilder();
                        stctks.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinchutaikhoan.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in ctkTables.Rows)
                        {
                            typeid = row["FULLNAME"] != null ? row["FULLNAME"].ToString().Trim() : "";
                            switch (typeid)
                            {
                                case SmartPortal.Constant.IPC.CHUTAIKHOAN:
                                    userName = Resources.labels.chutaikhoan;

                                    break;
                                case SmartPortal.Constant.IPC.QUANLYTAICHINH:
                                    userName = Resources.labels.quanlytaichinh;
                                    break;
                                case SmartPortal.Constant.IPC.KETOAN:
                                    userName = Resources.labels.ketoan;
                                    break;

                            }

                            stctks.Append("<table style='width:100%;'>");


                            stctks.Append("<tr>");
                            stctks.Append("<td width='25%'>");
                            stctks.Append(Resources.labels.tendaydu + " ");
                            stctks.Append("</td>");
                            stctks.Append("<td width='25%'>");
                            stctks.Append(row["FULLNAME"].ToString());
                            stctks.Append("</td>");
                            stctks.Append("<td width='25%'>");
                            stctks.Append("Email ");
                            stctks.Append("</td>");
                            stctks.Append("<td width='25%'>");
                            stctks.Append(row["EMAIL"].ToString());
                            stctks.Append("</td>");
                            stctks.Append("</tr>");

                            stctks.Append("<tr>");
                            stctks.Append("<td>");
                            stctks.Append(Resources.labels.dienthoai + " ");
                            stctks.Append("</td>");
                            stctks.Append("<td>");
                            stctks.Append(row["PHONE"].ToString());
                            stctks.Append("</td>");
                            stctks.Append("<td>");
                            //stctks.Append("");
                            stctks.Append(Resources.labels.kieunguoidung + " ");
                            stctks.Append("</td>");
                            stctks.Append("<td>");
                            stctks.Append(usertype);
                            stctks.Append("</td>");
                            stctks.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablectk = accountIBDatasetctk.Tables[0];
                            if (accountIBTablectk.Rows.Count != 0)
                            {
                                if (accountIBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<br/>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<B>Internet Banking</B>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.tendangnhap + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(accountIBTablectk.Rows[0]["USERNAME"].ToString());
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.matkhau + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablectk = accountSMSDatasetctk.Tables[0];
                            if (accountSMSTablectk.Rows.Count != 0)
                            {
                                if (accountSMSTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<br/>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<B>SMS Banking</B>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.sodienthoai + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(accountSMSTablectk.Rows[0]["UN"].ToString());
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(accountSMSTablectk.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablectk = accountMBDatasetctk.Tables[0];
                            if (accountMBTablectk.Rows.Count != 0)
                            {
                                if (accountMBTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<br/>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<B>Mobile Banking</B>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.tendangnhap + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(accountMBTablectk.Rows[0]["UN"].ToString());
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.matkhau + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");
                                }

                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetctk = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablectk = accountPHODatasetctk.Tables[0];
                            if (accountPHOTablectk.Rows.Count != 0)
                            {
                                if (accountPHOTablectk.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<br/>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td colspan='4'>");
                                    stctks.Append("<B>Phone Banking</B>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");

                                    stctks.Append("<tr>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.tendangnhap + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(accountPHOTablectk.Rows[0]["UN"].ToString());
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append(Resources.labels.matkhau + " :");
                                    stctks.Append("</td>");
                                    stctks.Append("<td width='25%'>");
                                    stctks.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stctks.Append("</td>");
                                    stctks.Append("</tr>");
                                }
                            }

                            stctks.Append("</table>");
                            j += 1;
                            if (j < ctkTables.Rows.Count)
                            {
                                stctks.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("USERINFO", stctks.ToString());

                        #endregion

                    }
                }


                if (ViewState["QUANLYTAICHINH"] != null)
                {
                    DataTable tblNGUOIUYQUYENs = (DataTable)ViewState["QUANLYTAICHINH"];
                    if (tblNGUOIUYQUYENs.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable nuqTables = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.QUANLYTAICHINH, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stnuqs = new StringBuilder();
                        stnuqs.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinnguoiquanlytiente.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in nuqTables.Rows)
                        {

                            stnuqs.Append("<table style='width:100%;'>");


                            stnuqs.Append("<tr>");
                            stnuqs.Append("<td width='25%'>");
                            stnuqs.Append(Resources.labels.tendaydu + " ");
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td width='25%'>");
                            stnuqs.Append(row["FULLNAME"].ToString());
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td width='25%'>");
                            stnuqs.Append("Email ");
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td width='25%'>");
                            stnuqs.Append(row["EMAIL"].ToString());
                            stnuqs.Append("</td>");
                            stnuqs.Append("</tr>");

                            stnuqs.Append("<tr>");
                            stnuqs.Append("<td>");
                            stnuqs.Append(Resources.labels.dienthoai + " ");
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td>");
                            stnuqs.Append(row["PHONE"].ToString());
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td>");
                            //stnuqs.Append("");
                            //stnuqs.Append("</td>");
                            //stnuqs.Append("<td>");
                            //stnuqs.Append("");
                            //stnuqs.Append("</td>");
                            //stnuqs.Append("</tr>");
                            stnuqs.Append(Resources.labels.kieunguoidung + " ");
                            stnuqs.Append("</td>");
                            stnuqs.Append("<td>");
                            stnuqs.Append(usertype);
                            stnuqs.Append("</td>");
                            stnuqs.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablenuq = accountIBDatasetnuq.Tables[0];
                            if (accountIBTablenuq.Rows.Count != 0)
                            {
                                if (accountIBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<br/>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<B>Internet Banking</B>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.tendangnhap + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(accountIBTablenuq.Rows[0]["USERNAME"].ToString());
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.matkhau + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablenuq = accountSMSDatasetnuq.Tables[0];
                            if (accountSMSTablenuq.Rows.Count != 0)
                            {
                                if (accountSMSTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<br/>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<B>SMS Banking</B>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.sodienthoai + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(accountSMSTablenuq.Rows[0]["UN"].ToString());
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(accountSMSTablenuq.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablenuq = accountMBDatasetnuq.Tables[0];
                            if (accountMBTablenuq.Rows.Count != 0)
                            {
                                if (accountMBTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<br/>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<B>Mobile Banking</B>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.tendangnhap + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(accountMBTablenuq.Rows[0]["UN"].ToString());
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.matkhau + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                }
                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetnuq = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablenuq = accountPHODatasetnuq.Tables[0];
                            if (accountPHOTablenuq.Rows.Count != 0)
                            {
                                if (accountPHOTablenuq.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<br/>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td colspan='4'>");
                                    stnuqs.Append("<B>Phone Banking</B>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");

                                    stnuqs.Append("<tr>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.tendangnhap + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(accountPHOTablenuq.Rows[0]["UN"].ToString());
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append(Resources.labels.matkhau + " :");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("<td width='25%'>");
                                    stnuqs.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stnuqs.Append("</td>");
                                    stnuqs.Append("</tr>");
                                }
                            }

                            stnuqs.Append("</table>");
                            j += 1;
                            if (j < nuqTables.Rows.Count)
                            {
                                stnuqs.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("NGUOIUYQUYEN", stnuqs.ToString());

                        #endregion

                    }
                }

                if (ViewState["KETOAN"] != null)
                {
                    DataTable tblLEVEL2s = (DataTable)ViewState["KETOAN"];
                    if (tblLEVEL2s.Rows.Count != 0)
                    {

                        #region lay thong tin tai khoan cua nguoi dong so huu
                        DataTable l2Tables = (new SmartPortal.SEMS.Contract().GetUserByContractNo(hpcontractNo, SmartPortal.Constant.IPC.KETOAN, "", ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];
                        if (IPCERRORCODE != "0")
                        {
                            goto ERROR;
                        }

                        StringBuilder stl2s = new StringBuilder();
                        stl2s.Append("<div style='font-weight: bold; background-color: #C0C0C0;width:100%;height:16px;'>" + Resources.labels.thongtinketoan.ToUpper() + "</div>");
                        //gan thong tin user vao stringtemplate
                        int j = 0;
                        foreach (DataRow row in l2Tables.Rows)
                        {

                            stl2s.Append("<table style='width:100%;'>");


                            stl2s.Append("<tr>");
                            stl2s.Append("<td width='25%'>");
                            stl2s.Append(Resources.labels.tendaydu + " ");
                            stl2s.Append("</td>");
                            stl2s.Append("<td width='25%'>");
                            stl2s.Append(row["FULLNAME"].ToString());
                            stl2s.Append("</td>");
                            stl2s.Append("<td width='25%'>");
                            stl2s.Append("Email ");
                            stl2s.Append("</td>");
                            stl2s.Append("<td width='25%'>");
                            stl2s.Append(row["EMAIL"].ToString());
                            stl2s.Append("</td>");
                            stl2s.Append("</tr>");

                            stl2s.Append("<tr>");
                            stl2s.Append("<td>");
                            stl2s.Append(Resources.labels.dienthoai + " ");
                            stl2s.Append("</td>");
                            stl2s.Append("<td>");
                            stl2s.Append(row["PHONE"].ToString());
                            stl2s.Append("</td>");
                            stl2s.Append("<td>");
                            //stl2s.Append("");
                            //stl2s.Append("</td>");
                            //stl2s.Append("<td>");
                            //stl2s.Append("");
                            //stl2s.Append("</td>");
                            //stl2s.Append("</tr>");
                            stl2s.Append(Resources.labels.kieunguoidung + " ");
                            stl2s.Append("</td>");
                            stl2s.Append("<td>");
                            stl2s.Append(usertype);
                            stl2s.Append("</td>");
                            stl2s.Append("</tr>");


                            //lay het các tai khoan Ibank cua user theo userID
                            DataSet accountIBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.IB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountIBTablel2 = accountIBDatasetl2.Tables[0];
                            if (accountIBTablel2.Rows.Count != 0)
                            {
                                if (accountIBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<br/>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<B>Internet Banking</B>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.tendangnhap + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(accountIBTablel2.Rows[0]["USERNAME"].ToString());
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.matkhau + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountIBTable.Rows[0]["PASSWORD"].ToString()) + "</b>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan SMS cua user theo userID
                            DataSet accountSMSDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.SMS, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountSMSTablel2 = accountSMSDatasetl2.Tables[0];
                            if (accountSMSTablel2.Rows.Count != 0)
                            {
                                if (accountSMSTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<br/>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<B>SMS Banking</B>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.sodienthoai + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(accountSMSTablel2.Rows[0]["UN"].ToString());
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.taikhoanmacdinh + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(accountSMSTablel2.Rows[0]["DEFAULTACCTNO"].ToString());
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");
                                }
                            }

                            //lay het các tai khoan MB cua user theo userID
                            DataSet accountMBDatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.MB, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountMBTablel2 = accountMBDatasetl2.Tables[0];
                            if (accountMBTablel2.Rows.Count != 0)
                            {
                                if (accountMBTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<br/>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<B>Mobile Banking</B>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.tendangnhap + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(accountMBTablel2.Rows[0]["UN"].ToString());
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.matkhau + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountMBTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");
                                }

                            }

                            //lay het các tai khoan PHO cua user theo userID
                            DataSet accountPHODatasetl2 = new SmartPortal.SEMS.User().GetUserRoleByServiceID(SmartPortal.Constant.IPC.PHO, row["USERID"].ToString().Trim(), string.Empty, ref IPCERRORCODE, ref IPCERRORDESC);

                            if (IPCERRORCODE != "0")
                            {
                                goto ERROR;
                            }

                            DataTable accountPHOTablel2 = accountPHODatasetl2.Tables[0];
                            if (accountPHOTablel2.Rows.Count != 0)
                            {
                                if (accountPHOTablel2.Rows[0]["ROLEID"].ToString().Trim() != "")
                                {
                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<br/>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td colspan='4'>");
                                    stl2s.Append("<B>Phone Banking</B>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");

                                    stl2s.Append("<tr>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.tendangnhap + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(accountPHOTablel2.Rows[0]["UN"].ToString());
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append(Resources.labels.matkhau + " :");
                                    stl2s.Append("</td>");
                                    stl2s.Append("<td width='25%'>");
                                    stl2s.Append("########");
                                    //st.Append("<b>" + SmartPortal.Security.Encryption.Decrypt(accountPHOTable.Rows[0]["PASS"].ToString()) + "</b>");
                                    stl2s.Append("</td>");
                                    stl2s.Append("</tr>");
                                }
                            }

                            stl2s.Append("</table>");
                            j += 1;
                            if (j < l2Tables.Rows.Count)
                            {
                                stl2s.Append("<hr/>");
                            }
                        }
                        tmpl.SetAttribute("NUQ", stl2s.ToString());
                        #endregion
                    }
                }
                #endregion
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
    void ReleaseSession()
    {
        ViewState["CHUTAIKHOAN"] = null;
        ViewState["NGUOIUYQUYEN"] = null;
        ViewState["LEVEL2"] = null;
        ViewState["NGUOIQUANTRI"] = null;
    }

    void LoadDataInTreeview(string serviceID, TreeView tvPage, string userType)
    {
        tvPage.Nodes.Clear();
        DataTable tblSS = new DataTable();
        tblSS = new SmartPortal.SEMS.Role().GetByServiceAndUserType(serviceID, userType, ddlProduct.SelectedValue);

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

    void GetRoleDefault(TreeView treeIB, TreeView treeSMS, TreeView treeMB, TreeView treePHO)
    {
        DataTable tblRoleDefault = new DataTable();
        //lay role mac dinh IB
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.IB, ddlProduct.SelectedValue, 0, string.Empty, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

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
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.SMS, ddlProduct.SelectedValue, 0, string.Empty, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

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
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.MB, ddlProduct.SelectedValue, 0, string.Empty, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

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

        //lay role mac dinh PHO
        tblRoleDefault = (new SmartPortal.SEMS.Role().GetRoleDefaultByServiceID(SmartPortal.Constant.IPC.PHO, ddlProduct.SelectedValue, 0, string.Empty, ref IPCERRORCODE, ref IPCERRORDESC)).Tables[0];

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

    protected void btnThemNQT_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            //minh add 11/9/2015 validate thong tin
            if (string.IsNullOrEmpty(txtAdministratorCode.Text.Trim()))
            {
                //lblError.Visible = true;

                ShowPopUpMsg(Resources.labels.machutaikhoankhongduoctrong);
                txtAdministratorCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFullNameQT.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtFullNameQT.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneQT.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoainguoiquantrihethong);

                txtPhoneQT.Focus();
                return;
            }



            string pattern = Resources.labels.emailpattern;

            //truong hop individual email = "" chap nhan, corporate validate ca truong hop rong



            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtEmailQT.Text, pattern)))
            {
                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblAlert.Text = Resources.labels.emailkhongdinhdang1;
                txtEmailQT.Focus();
                return;

            }

            #region Tao bang chua cac thong tin nguoi quan tri
            string PassTemp = "";
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIB.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMS.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMB.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radAccountQT.Checked)
            {
                // PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                //  LuuThongTinQuyenadvance("NGUOIQUANTRI", gvResultQuanTri, tvIBQT, tvSMSQT, tvMBQT, tvPHOQT, txtFullNameQT.Text, ddlLevelQT.Text, txtBirthQT.Text, ddlGenderQT.SelectedValue, txtPhoneQT.Text, txtEmailQT.Text, txtAddressQT.Text, txtIBUserNameQT.Text, PassTemp, txtSMSPhoneQT.Text, ddlDefaultAccountQT.SelectedValue, ddlDefaultLangQT.SelectedValue, txtMBPhoneQT.Text, PassTemp, txtPHOPhoneQT.Text, PassTemp, ddlAccountQT.SelectedValue, ((cbQTHTTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlQTHTDefaultAcctno.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtIBUserNameQT.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyenadvance("NGUOIQUANTRI", gvResultQuanTri, tvIBQT, tvSMSQT, tvMBQT, tvPHOQT, txtFullNameQT.Text, ddlLevelQT.Text, txtBirthQT.Text, ddlGenderQT.SelectedValue, txtPhoneQT.Text, txtEmailQT.Text, txtAddressQT.Text, txtIBUserNameQT.Text, PassTemp, txtSMSPhoneQT.Text, ddlDefaultAccountQT.SelectedValue, ddlDefaultLangQT.SelectedValue, txtMBPhoneQT.Text, PassTemp, txtPHOPhoneQT.Text, PassTemp, ddlAccountQT.SelectedValue, ((cbQTHTTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlQTHTDefaultAcctno.SelectedValue, pwdreset);
            }

            if (radAllAccountQT.Checked)
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

                //luu tat ca account
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, txtIBUserNameQT.Text.Trim());
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    //LuuThongTinQuyenadvance("NGUOIQUANTRI", gvResultQuanTri, tvIBQT, tvSMSQT, tvMBQT, tvPHOQT, txtFullNameQT.Text, ddlLevelQT.Text, txtBirthQT.Text, ddlGenderQT.SelectedValue, txtPhoneQT.Text, txtEmailQT.Text, txtAddressQT.Text, txtIBUserNameQT.Text, PassTemp, txtSMSPhoneQT.Text, ddlDefaultAccountQT.SelectedValue, ddlDefaultLangQT.SelectedValue, txtMBPhoneQT.Text, PassTemp, txtPHOPhoneQT.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbQTHTTKMD.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlQTHTDefaultAcctno.SelectedValue);
                    LuuThongTinQuyenadvance("NGUOIQUANTRI", gvResultQuanTri, tvIBQT, tvSMSQT, tvMBQT, tvPHOQT, txtFullNameQT.Text, ddlLevelQT.Text, txtBirthQT.Text, ddlGenderQT.SelectedValue, txtPhoneQT.Text, txtEmailQT.Text, txtAddressQT.Text, txtIBUserNameQT.Text, PassTemp, txtSMSPhoneQT.Text, ddlDefaultAccountQT.SelectedValue, ddlDefaultLangQT.SelectedValue, txtMBPhoneQT.Text, PassTemp, txtPHOPhoneQT.Text, PassTemp, rowAccount["ACCOUNTNO"].ToString(), ((cbQTHTTKMD.Checked == true) ? "Y" : "N"), pwdresetSMS, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), ddlQTHTDefaultAcctno.SelectedValue, pwdreset);
                }


            }
            lblAlertQTHT.Text = Resources.labels.recordsaved;
            #endregion
            //11/9/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultQuanTri.Rows.Count == 0)
            {

                lblAlertQTHT.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemNQT_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemNQT_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    protected void gvResultQuanTri_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultQuanTri.PageIndex = e.NewPageIndex;
            gvResultQuanTri.DataSource = (DataTable)ViewState["NGUOIQUANTRI"];
            gvResultQuanTri.DataBind();
        }
        catch
        {
        }
    }

    protected void gvResultQuanTri_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultQT = (DataTable)ViewState["NGUOIQUANTRI"];

            tblResultQT.Rows.RemoveAt(e.RowIndex + (gvResultQuanTri.PageIndex * gvResultQuanTri.PageSize));

            ViewState["NGUOIQUANTRI"] = tblResultQT;
            gvResultQuanTri.DataSource = tblResultQT;
            gvResultQuanTri.DataBind();

            lblAlertQTHT.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnHuyQTHT_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultQT = (DataTable)ViewState["NGUOIQUANTRI"];

            tblResultQT.Rows.Clear();

            ViewState["NGUOIQUANTRI"] = tblResultQT;
            gvResultQuanTri.DataSource = tblResultQT;
            gvResultQuanTri.DataBind();

            lblAlertQTHT.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnHuyCTK_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultChuTaiKhoan = (DataTable)ViewState["CHUTAIKHOAN"];

            tblResultChuTaiKhoan.Rows.Clear();

            ViewState["CHUTAIKHOAN"] = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataSource = tblResultChuTaiKhoan;
            gvResultChuTaiKhoan.DataBind();

            lblAlertCTK.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnNUY_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultNguoiUyQuyen = (DataTable)ViewState["NGUOIUYQUYEN"];

            tblResultNguoiUyQuyen.Rows.Clear();

            ViewState["NGUOIUYQUYEN"] = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataSource = tblResultNguoiUyQuyen;
            gvResultNguoiUyQuyen.DataBind();

            lblAlertNUY.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnHuyL2_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultLevl2 = (DataTable)ViewState["LEVEL2"];

            tblResultLevl2.Rows.Clear();

            ViewState["LEVEL2"] = tblResultLevl2;
            gvResultLevel2.DataSource = tblResultLevl2;
            gvResultLevel2.DataBind();

            lblAlertL2.Text = Resources.labels.recorddeleted;
        }
        catch
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

    protected void btnThemChuTaiKhoanS_Click(object sender, EventArgs e)
    {
        try
        {

            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            #region Tao bang chua cac thong tin nguoi uy quyen
            string PassTemp = "";
            if (rbGenerateS.Checked)
            {
                userName = txtIBGenUserNameS.Text.Trim();
            }
            else if (rbTypeS.Checked)
            {
                userName = txtIBTypeUserNameS.Text.Trim();
                #region check username

                if (userName == string.Empty)
                {
                    lblAlertCTKS.Text = Resources.labels.bancannhaptendangnhap;
                }
                else if (userName.Length < minlength || userName.Length > maxlength)
                {
                    lblAlertCTKS.Text = string.Format(Resources.labels.usernamemustbetween, minlength, maxlength);
                }

                DataSet ds = new Customer().CheckUserName("EBA_Users_CheckUserName", new object[] { userName }, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    lblAlertCTKS.Text = IPCERRORDESC;
                    return;
                }
                if (!validateusername(txtIBTypeUserNameS))
                {
                    return;
                }
                #endregion
            }
            //27/8/2015 minh add to validate information for owner acount

            if (string.IsNullOrEmpty(txtReFullNameS.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtReFullNameS.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtReMobiS.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoaichutaikhoan);

                txtReMobiS.Focus();
                return;
            }


            string pattern = Resources.labels.emailpattern;

            //if (!string.IsNullOrEmpty(txtReEmailS.Text.Trim()))
            //{
            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtReEmailS.Text, pattern)))
            {

                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblError.Text = Resources.labels.emailkhongdinhdang1;
                txtReEmailS.Focus();
                return;

            }
            //}

            ////28/5/2015 minh add to add fix force contract must have email or sms service
            //if (txtReEmailS.Text.Trim() != txtEmail.Text.Trim())
            //{
            //    txtEmail.Text = txtReEmailS.Text.Trim();

            //minh add 13/5/2015 1 số điện thoại chỉ được dùng cho 1 tài khoản sms mặc định
            #region check phone number and default account

            string phoneNumber = txtSMSPhoneNoS.Text.Trim();
            string defaultAcc = ddlSMSDefaultAcctnoS.Text.Trim();
            DataTable dt1 = new SmartPortal.SEMS.Customer().CheckPhoneNumber(phoneNumber, defaultAcc);
            if (dt1.Rows.Count != 0)
            {
                lblAlertCTKS.Text = Resources.labels.phonenumberassigned;
                return;
            }
            #endregion

            #region check sms notify vutt 30032016
            string errDesc = string.Empty;
            if (!string.IsNullOrEmpty(txtSMSPhoneNoS.Text))
            {
                //if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMSS, ref errDesc, radAllAccountS.Checked ? "" : ddlAccountS.SelectedValue, new List<DataTable> { (DataTable)ViewState["KETOAN"], (DataTable)ViewState["QUANLYTAICHINH"] }))
                //phongtt sms notification fee
                if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvSMSS, ref errDesc, radAllAccountS.Checked ? "" : ddlAccountS.SelectedValue, lsAccNo, new List<DataTable> { (DataTable)ViewState["KETOAN"], (DataTable)ViewState["QUANLYTAICHINH"] }))
                {
                    lblAlertCTKS.Text = errDesc;
                    return;
                }
            }
            #endregion

            //}
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIBs.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMSs.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMBs.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radAccountS.Checked)
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                //LuuThongTinQuyen("CHUTAIKHOANS", gvResultChuTaiKhoanS, tvIBS, tvSMSS, tvMBS, tvPHOS, txtReFullNameS.Text, lblLevelS.Text, txtReBirthS.Text, ddlReGenderS.SelectedValue, txtReMobiS.Text, txtReEmailS.Text, txtReAddressS.Text, userName, PassTemp, txtSMSPhoneNoS.Text, ddlSMSDefaultAcctnoS.SelectedValue, ddlLanguageS.SelectedValue, ((cbCTKIsDefaultS.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBPhoneNoS.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBPhoneNoS.Text, PassTemp, ddlCTKPHODefaultAcctnoS.SelectedValue, ddlAccountS.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyen("CHUTAIKHOANS", gvResultChuTaiKhoanS, tvIBS, tvSMSS, tvMBS, tvPHOS, txtReFullNameS.Text, lblLevelS.Text, txtReBirthS.Text, ddlReGenderS.SelectedValue, txtReMobiS.Text, txtReEmailS.Text, txtReAddressS.Text, userName, PassTemp, txtSMSPhoneNoS.Text, ddlSMSDefaultAcctnoS.SelectedValue, ddlLanguageS.SelectedValue, ((cbCTKIsDefaultS.Checked == true) ? "Y" : "N"), pwdresetSMS, txtMBPhoneNoS.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBPhoneNoS.Text, PassTemp, ddlCTKPHODefaultAcctnoS.SelectedValue, ddlAccountS.SelectedValue, pwdreset);
            }


            if (radAllAccountS.Checked)
            {
                //lay tat ca tai khoan khach hang
                DataSet ds = new DataSet();
                switch (ddlCustType.SelectedValue.Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.PERSONALLKG:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.CORPORATE:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                }
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtAccountS = new DataTable();
                dtAccountS = ds.Tables[0];

                //luu tat ca account
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtAccountS.Rows)
                {
                    //LuuThongTinQuyen("CHUTAIKHOANS", gvResultChuTaiKhoanS, tvIBS, tvSMSS, tvMBS, tvPHOS, txtReFullNameS.Text, lblLevelS.Text, txtReBirthS.Text, ddlReGenderS.SelectedValue, txtReMobiS.Text, txtReEmailS.Text, txtReAddressS.Text, userName, PassTemp, txtSMSPhoneNoS.Text, ddlSMSDefaultAcctnoS.SelectedValue, ddlLanguageS.SelectedValue, ((cbCTKIsDefaultS.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtMBPhoneNoS.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNoS.Text, PassTemp, ddlCTKPHODefaultAcctnoS.SelectedValue, rowAccount["ACCOUNTNO"].ToString());
                    LuuThongTinQuyen("CHUTAIKHOANS", gvResultChuTaiKhoanS, tvIBS, tvSMSS, tvMBS, tvPHOS, txtReFullNameS.Text, lblLevelS.Text, txtReBirthS.Text, ddlReGenderS.SelectedValue, txtReMobiS.Text, txtReEmailS.Text, txtReAddressS.Text, userName, PassTemp, txtSMSPhoneNoS.Text, ddlSMSDefaultAcctnoS.SelectedValue, ddlLanguageS.SelectedValue, ((cbCTKIsDefaultS.Checked == true) ? "Y" : "N"), pwdresetSMS, txtMBPhoneNoS.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtPHOPhoneNoS.Text, PassTemp, ddlCTKPHODefaultAcctnoS.SelectedValue, rowAccount["ACCOUNTNO"].ToString(), pwdreset);
                }

            }
            lblAlertCTKS.Text = Resources.labels.recordsaved;
            #endregion
            //28/8/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultChuTaiKhoanS.Rows.Count == 0)
            {
                lblAlertCTKS.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }

        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemChuTaiKhoanS_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemChuTaiKhoanS_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void btnHuyCTKS_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblResultChuTaiKhoan = (DataTable)ViewState["CHUTAIKHOANS"];

            tblResultChuTaiKhoan.Rows.Clear();

            ViewState["CHUTAIKHOANS"] = tblResultChuTaiKhoan;
            gvResultChuTaiKhoanS.DataSource = tblResultChuTaiKhoan.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultChuTaiKhoanS.DataBind();

            lblAlertCTKS.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void gvResultChuTaiKhoanS_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultChuTaiKhoanS.PageIndex = e.NewPageIndex;
            gvResultChuTaiKhoanS.DataSource = (DataTable)ViewState["CHUTAIKHOANS"];
            gvResultChuTaiKhoanS.DataBind();
        }
        catch
        {
        }
    }
    protected void gvResultChuTaiKhoanS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultChuTaiKhoanS = (DataTable)ViewState["CHUTAIKHOANS"];

            DataRow[] delRow = tblResultChuTaiKhoanS.Select("colIBUserName='" + gvResultChuTaiKhoanS.Rows[e.RowIndex].Cells[1].Text + "' AND colAccount='" + gvResultChuTaiKhoanS.Rows[e.RowIndex].Cells[2].Text + "' AND colRole='" + gvResultChuTaiKhoanS.Rows[e.RowIndex].Cells[3].Text + "'");
            foreach (DataRow r in delRow)
            {
                //tblResultQT.Rows.RemoveAt(e.RowIndex + (gvResultQuanTri.PageIndex * gvResultQuanTri.PageSize));
                tblResultChuTaiKhoanS.Rows.Remove(r);
            }
            //tblResultChuTaiKhoan.Rows.RemoveAt(e.RowIndex + (gvResultChuTaiKhoan.PageIndex * gvResultChuTaiKhoan.PageSize));

            ViewState["CHUTAIKHOANS"] = tblResultChuTaiKhoanS;
            gvResultChuTaiKhoanS.DataSource = tblResultChuTaiKhoanS.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultChuTaiKhoanS.DataBind();

            lblAlertCTKS.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void btnlv4Detail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            txtlv4FullName.Text = string.Empty;
            txtlv4Email.Text = string.Empty;
            txtlv4Mobi.Text = string.Empty;
            ddllv4Gender.SelectedIndex = 0;
            txtlv4Birth.Text = string.Empty;
            txtlv4Address.Text = string.Empty;

            Hashtable haslv4Info = new Hashtable();
            string ctmType = "P";
            haslv4Info = new SmartPortal.SEMS.Customer().GetCustInfo(txtlv4Code.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (haslv4Info[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                ShowPopUpMsg(Resources.labels.makhachhangkhongtontaitronghethong);

                //throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
                return;
            }

            if (haslv4Info[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtlv4FullName.Text = haslv4Info[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (haslv4Info[SmartPortal.Constant.IPC.EMAIL] != null)
                txtlv4Email.Text = haslv4Info[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (haslv4Info[SmartPortal.Constant.IPC.PHONE] != null)
                txtlv4Mobi.Text = haslv4Info[SmartPortal.Constant.IPC.PHONE].ToString();
            if (haslv4Info[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    ddllv4Gender.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(haslv4Info[SmartPortal.Constant.IPC.SEX].ToString()).ToString());

                    if (haslv4Info[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddllv4Gender.Enabled = true;
                    }
                    else
                    {
                        ddllv4Gender.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (haslv4Info[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(haslv4Info[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
                    txtlv4Birth.Text = birthDate.Equals("01/01/1900") ? "" : birthDate;

                    if (txtlv4Birth.Text.Trim() == "")
                    {
                        txtlv4Birth.Enabled = true;
                    }
                    else
                    {
                        txtlv4Birth.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (haslv4Info[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtlv4Address.Text = haslv4Info[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }
            //02.12.2015 minh add  to generate username lv4
            txtlv4IBGenUserName.Text = SmartPortal.Common.Utilities.Utility.GetID(haslv4Info[SmartPortal.Constant.IPC.CUSTNAME].ToString(), haslv4Info[SmartPortal.Constant.IPC.CUSTCODE].ToString(), haslv4Info[SmartPortal.Constant.IPC.LICENSE].ToString()) + "O2";
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {

                txtlv4MBPhoneNo.Text = txtlv4IBGenUserName.Text;
                txtlv4PHOPhoneNo.Text = txtlv4MBPhoneNo.Text;
            }
            else
            {

                txtlv4MBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", haslv4Info[SmartPortal.Constant.IPC.CUSTCODE].ToString(), "", 10) + "O2";
                txtlv4PHOPhoneNo.Text = txtlv4MBPhoneNo.Text;
            }


        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(IPCex.ToString(), this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(IPCex.Message, Request.Url.Query);

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnlv5Detail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            txtlv5FullName.Text = string.Empty;
            txtlv5Email.Text = string.Empty;
            txtlv5Mobi.Text = string.Empty;
            ddllv5Gender.SelectedIndex = 0;
            txtlv5Birth.Text = string.Empty;
            txtlv5Address.Text = string.Empty;

            Hashtable haslv5Info = new Hashtable();
            string ctmType = "P";
            haslv5Info = new SmartPortal.SEMS.Customer().GetCustInfo(txtlv5Code.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (haslv5Info[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                ShowPopUpMsg(Resources.labels.makhachhangkhongtontaitronghethong);

                //throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
                return;
            }

            if (haslv5Info[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtlv5FullName.Text = haslv5Info[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (haslv5Info[SmartPortal.Constant.IPC.EMAIL] != null)
                txtlv5Email.Text = haslv5Info[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (haslv5Info[SmartPortal.Constant.IPC.PHONE] != null)
                txtlv5Mobi.Text = haslv5Info[SmartPortal.Constant.IPC.PHONE].ToString();
            if (haslv5Info[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    ddllv5Gender.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(haslv5Info[SmartPortal.Constant.IPC.SEX].ToString()).ToString());

                    if (haslv5Info[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddllv5Gender.Enabled = true;
                    }
                    else
                    {
                        ddllv5Gender.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (haslv5Info[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(haslv5Info[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
                    txtlv5Birth.Text = birthDate.Equals("01/01/1900") ? "" : birthDate;

                    if (txtlv5Birth.Text.Trim() == "")
                    {
                        txtlv5Birth.Enabled = true;
                    }
                    else
                    {
                        txtlv5Birth.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (haslv5Info[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtlv5Address.Text = haslv5Info[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }
            //02.12.2015 minh add to generate username:
            txtlv5IBGenUserName.Text = SmartPortal.Common.Utilities.Utility.GetID(haslv5Info[SmartPortal.Constant.IPC.CUSTNAME].ToString(), haslv5Info[SmartPortal.Constant.IPC.CUSTCODE].ToString(), haslv5Info[SmartPortal.Constant.IPC.LICENSE].ToString()) + "O3";
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {

                txtlv5MBPhoneNo.Text = txtlv5IBGenUserName.Text;
                txtlv5PHOPhoneNo.Text = txtlv5MBPhoneNo.Text;
            }
            else
            {

                txtlv5MBPhoneNo.Text = SmartPortal.Common.Utilities.Utility.GetID("", haslv5Info[SmartPortal.Constant.IPC.CUSTCODE].ToString(), "", 10) + "O3";
                txtlv5PHOPhoneNo.Text = txtlv5MBPhoneNo.Text;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(IPCex.ToString(), this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(IPCex.Message, Request.Url.Query);

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnThemlv4_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            #region Tao bang chua cac thong tin nguoi uy quyen
            //27/8/2015 minh add to validate information for owner acount
            if (string.IsNullOrEmpty(txtlv4Code.Text.Trim()))
            {
                //lblError.Visible = true;

                ShowPopUpMsg(Resources.labels.machutaikhoankhongduoctrong);
                txtlv4Code.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtlv4FullName.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtlv4FullName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtlv4Mobi.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoaichutaikhoan);

                txtlv4Mobi.Focus();
                return;
            }


            string pattern = Resources.labels.emailpattern;

            //if (!string.IsNullOrEmpty(txtReEmailS.Text.Trim()))
            //{
            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtlv4Email.Text, pattern)))
            {

                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblError.Text = Resources.labels.emailkhongdinhdang1;
                txtlv4Email.Focus();
                return;

            }
            //}

            string PassTemp = "";
            if (rblv4Generate.Checked)
            {
                userName = txtlv4IBGenUserName.Text.Trim();
            }
            else if (rblv4Type.Checked)
            {
                userName = txtlv4IBTypeUserName.Text.Trim();
                #region check username

                if (userName == string.Empty)
                {
                    lbllv4Alert.Text = Resources.labels.bancannhaptendangnhap;
                }
                else if (userName.Length < minlength || userName.Length > maxlength)
                {
                    lbllv4Alert.Text = string.Format(Resources.labels.usernamemustbetween, minlength, maxlength);
                }

                DataSet ds = new Customer().CheckUserName("EBA_Users_CheckUserName", new object[] { userName }, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    lbllv4Alert.Text = IPCERRORDESC;
                    return;
                }
                if (!validateusername(txtlv4IBTypeUserName))
                {
                    return;
                }
                #endregion
            }
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIBsl4.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMSsl4.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMBsl4.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());

            }

            //minh add 13/5/2015 1 số điện thoại chỉ được dùng cho 1 tài khoản sms mặc định
            #region check phone number and default account

            string phoneNumber = txtlv4SMSPhoneNo.Text.Trim();
            string defaultAcc = ddllv4SMSDefaultAcctno.Text.Trim();
            DataTable dt1 = new SmartPortal.SEMS.Customer().CheckPhoneNumber(phoneNumber, defaultAcc);
            if (dt1.Rows.Count != 0)
            {
                lbllv4Alert.Text = Resources.labels.phonenumberassigned;
                return;
            }
            #endregion

            #region check sms notify vutt 30032016
            string errDesc = string.Empty;
            if (!string.IsNullOrEmpty(txtlv4SMSPhoneNo.Text))
            {
                //if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvlv4SMS, ref errDesc, radlv4AllAccount.Checked ? "" : ddllv4Account.SelectedValue, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOANS"], (DataTable)ViewState["KETOAN"] }))
                //phongtt sms notification fee
                if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvlv4SMS, ref errDesc, radlv4AllAccount.Checked ? "" : ddllv4Account.SelectedValue, lsAccNo, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOANS"], (DataTable)ViewState["KETOAN"] }))
                {
                    lbllv4Alert.Text = errDesc;
                    return;
                }
            }
            #endregion

            if (radlv4Account.Checked)
            {
                // PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                //LuuThongTinQuyen("QUANLYTAICHINH", gvResultlv4, tvlv4IB, tvlv4SMS, tvlv4MB, tvlv4PHO, txtlv4FullName.Text, lbllv4Level.Text, txtlv4Birth.Text, ddllv4Gender.SelectedValue, txtlv4Mobi.Text, txtlv4Email.Text, txtlv4Address.Text, userName, PassTemp, txtlv4SMSPhoneNo.Text, ddllv4SMSDefaultAcctno.SelectedValue, ddllv4Language.SelectedValue, ((cblv4CTKIsDefault.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4MBPhoneNo.Text, PassTemp, ddllv4PHODefaultAcctno.SelectedValue, ddllv4Account.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);

                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyen("QUANLYTAICHINH", gvResultlv4, tvlv4IB, tvlv4SMS, tvlv4MB, tvlv4PHO, txtlv4FullName.Text, lbllv4Level.Text, txtlv4Birth.Text, ddllv4Gender.SelectedValue, txtlv4Mobi.Text, txtlv4Email.Text, txtlv4Address.Text, userName, PassTemp, txtlv4SMSPhoneNo.Text, ddllv4SMSDefaultAcctno.SelectedValue, ddllv4Language.SelectedValue, ((cblv4CTKIsDefault.Checked == true) ? "Y" : "N"), pwdresetSMS, txtlv4MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4MBPhoneNo.Text, PassTemp, ddllv4PHODefaultAcctno.SelectedValue, ddllv4Account.SelectedValue, pwdreset);
            }

            if (radlv4AllAccount.Checked)
            {
                //lay tat ca tai khoan khach hang
                DataSet ds = new DataSet();
                switch (ddlCustType.SelectedValue.Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.PERSONALLKG:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.CORPORATE:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                }
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtlv4Account = new DataTable();
                dtlv4Account = ds.Tables[0];

                //luu tat ca account
                // PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtlv4Account.Rows)
                {
                    //LuuThongTinQuyen("QUANLYTAICHINH", gvResultlv4, tvlv4IB, tvlv4SMS, tvlv4MB, tvlv4PHO, txtlv4FullName.Text, lbllv4Level.Text, txtlv4Birth.Text, ddllv4Gender.SelectedValue, txtlv4Mobi.Text, txtlv4Email.Text, txtlv4Address.Text, userName, PassTemp, txtlv4SMSPhoneNo.Text, ddllv4SMSDefaultAcctno.SelectedValue, ddllv4Language.SelectedValue, ((cblv4CTKIsDefault.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4PHOPhoneNo.Text, PassTemp, ddllv4PHODefaultAcctno.SelectedValue, rowAccount["ACCOUNTNO"].ToString());
                    LuuThongTinQuyen("QUANLYTAICHINH", gvResultlv4, tvlv4IB, tvlv4SMS, tvlv4MB, tvlv4PHO, txtlv4FullName.Text, lbllv4Level.Text, txtlv4Birth.Text, ddllv4Gender.SelectedValue, txtlv4Mobi.Text, txtlv4Email.Text, txtlv4Address.Text, userName, PassTemp, txtlv4SMSPhoneNo.Text, ddllv4SMSDefaultAcctno.SelectedValue, ddllv4Language.SelectedValue, ((cblv4CTKIsDefault.Checked == true) ? "Y" : "N"), pwdresetSMS, txtlv4MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv4PHOPhoneNo.Text, PassTemp, ddllv4PHODefaultAcctno.SelectedValue, rowAccount["ACCOUNTNO"].ToString(), pwdreset);
                }

            }
            lbllv4Alert.Text = Resources.labels.recordsaved;
            #endregion
            //28/8/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultlv4.Rows.Count == 0)
            {
                lbllv4Alert.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemlv4_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemlv4_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }

    protected void btnThemlv5_Click(object sender, EventArgs e)
    {
        try
        {
            int passlenIB = 0;
            int passlenSMS = 0;
            int passlenMB = 0;

            #region Tao bang chua cac thong tin nguoi uy quyen
            //27/8/2015 minh add to validate information for owner acount
            if (string.IsNullOrEmpty(txtlv5Code.Text.Trim()))
            {
                //lblError.Visible = true;

                ShowPopUpMsg(Resources.labels.machutaikhoankhongduoctrong);
                txtlv5Code.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtlv5FullName.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhaptenchutaikhoan);
                txtlv5FullName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtlv5Mobi.Text.Trim()))
            {
                ShowPopUpMsg(Resources.labels.bannhapsodienthoaichutaikhoan);

                txtlv5Mobi.Focus();
                return;
            }



            string pattern = Resources.labels.emailpattern;

            //if (!string.IsNullOrEmpty(txtReEmailS.Text.Trim()))
            //{
            if (!(System.Text.RegularExpressions.Regex.IsMatch(txtlv5Email.Text, pattern)))
            {

                ShowPopUpMsg(Resources.labels.emailkhongdinhdang1);
                //lblError.Text = Resources.labels.emailkhongdinhdang1;
                txtlv5Email.Focus();
                return;

            }
            //}
            string PassTemp = "";
            if (rblv5Generate.Checked)
            {
                userName = txtlv5IBGenUserName.Text.Trim();
            }
            else if (rblv5Type.Checked)
            {
                userName = txtlv5IBTypeUserName.Text.Trim();
                #region check username

                if (userName == string.Empty)
                {
                    lbllv5Alert.Text = Resources.labels.bancannhaptendangnhap;
                }
                else if (userName.Length < minlength || userName.Length > maxlength)
                {
                    lbllv5Alert.Text = string.Format(Resources.labels.usernamemustbetween, minlength, maxlength);
                }

                DataSet ds = new Customer().CheckUserName("EBA_Users_CheckUserName", new object[] { userName }, ref IPCERRORCODE, ref IPCERRORDESC);
                if (IPCERRORCODE != "0")
                {
                    lbllv5Alert.Text = IPCERRORDESC;
                    return;
                }
                if (!validateusername(txtlv5IBTypeUserName))
                {
                    return;
                }
                #endregion
            }

            //minh add 13/5/2015 1 số điện thoại chỉ được dùng cho 1 tài khoản sms mặc định
            #region check phone number and default account

            string phoneNumber = txtlv5SMSPhoneNo.Text.Trim();
            string defaultAcc = ddllv5SMSDefaultAcctno.Text.Trim();
            DataTable dt1 = new SmartPortal.SEMS.Customer().CheckPhoneNumber(phoneNumber, defaultAcc);
            if (dt1.Rows.Count != 0)
            {
                lbllv5Alert.Text = Resources.labels.phonenumberassigned;
                return;
            }
            #endregion


            #region check sms notify vutt 30032016
            string errDesc = string.Empty;
            if (!string.IsNullOrEmpty(txtlv5SMSPhoneNo.Text))
            {
                //if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvlv5SMS, ref errDesc, radlv5AllAccount.Checked ? "" : ddllv5Account.SelectedValue, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOANS"], (DataTable)ViewState["QUANLYTAICHINH"] }))
                //phongtt sms notification fee
                if (!ContractControl.ValidateSMSNotifyCurrentUser(ddlProduct.SelectedValue, tvlv4SMS, ref errDesc, radlv4AllAccount.Checked ? "" : ddllv4Account.SelectedValue, lsAccNo, new List<DataTable> { (DataTable)ViewState["CHUTAIKHOANS"], (DataTable)ViewState["KETOAN"] }))
                {
                    lbllv5Alert.Text = errDesc;
                    return;
                }
            }
            #endregion
            //21.3.2016 load infor about policy selected to get len of pass generate
            DataSet dspolicy = new DataSet();
            string filterIB = "serviceid='IB' and policyid='" + ddlpolicyIBsl5.SelectedValue.ToString() + "'";
            string filterSMS = "serviceid='SMS'  and policyid='" + ddlpolicySMSsl5.SelectedValue.ToString() + "'";
            string filterMB = "serviceid='MB' and policyid='" + ddlpolicyMBsl5.SelectedValue.ToString() + "'";
            string stSort = "serviceid asc";

            dspolicy = new SmartPortal.SEMS.USERPOLICY().GetPolicybyCondition(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Session["userName"].ToString(), ref IPCERRORCODE, ref IPCERRORDESC);
            if (IPCERRORCODE == "0")
            {

                DataTable dtIB = dspolicy.Tables[0].Select(filterIB, stSort).CopyToDataTable();
                DataTable dtSMS = dspolicy.Tables[0].Select(filterSMS, stSort).CopyToDataTable();
                DataTable dtMB = dspolicy.Tables[0].Select(filterMB, stSort).CopyToDataTable();
                passlenIB = Convert.ToInt32(dtIB.Rows[0]["minpwdlen"].ToString());
                passlenSMS = Convert.ToInt32(dtSMS.Rows[0]["minpwdlen"].ToString());
                passlenMB = Convert.ToInt32(dtMB.Rows[0]["minpwdlen"].ToString());
            }

            if (radlv5Account.Checked)
            {
                //PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                //  LuuThongTinQuyen("KETOAN", gvResultlv5, tvlv5IB, tvlv5SMS, tvlv5MB, tvlv5PHO, txtlv5FullName.Text, lbllv5Level.Text, txtlv5Birth.Text, ddllv5Gender.SelectedValue, txtlv5Mobi.Text, txtlv5Email.Text, txtlv5Address.Text, userName, PassTemp, txtlv5SMSPhoneNo.Text, ddllv5SMSDefaultAcctno.SelectedValue, ddllv5Language.SelectedValue, ((cblv5CTKIsDefault.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5MBPhoneNo.Text, PassTemp, ddllv5PHODefaultAcctno.SelectedValue, ddllv5Account.SelectedValue);
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);
                LuuThongTinQuyen("KETOAN", gvResultlv5, tvlv5IB, tvlv5SMS, tvlv5MB, tvlv5PHO, txtlv5FullName.Text, lbllv5Level.Text, txtlv5Birth.Text, ddllv5Gender.SelectedValue, txtlv5Mobi.Text, txtlv5Email.Text, txtlv5Address.Text, userName, PassTemp, txtlv5SMSPhoneNo.Text, ddllv5SMSDefaultAcctno.SelectedValue, ddllv5Language.SelectedValue, ((cblv5CTKIsDefault.Checked == true) ? "Y" : "N"), pwdresetSMS, txtlv5MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5MBPhoneNo.Text, PassTemp, ddllv5PHODefaultAcctno.SelectedValue, ddllv5Account.SelectedValue, pwdreset);
            }

            if (radlv5AllAccount.Checked)
            {
                //lay tat ca tai khoan khach hang
                DataSet ds = new DataSet();
                switch (ddlCustType.SelectedValue.Trim())
                {
                    case SmartPortal.Constant.IPC.PERSONAL:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.PERSONALLKG:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                    case SmartPortal.Constant.IPC.CORPORATE:
                        ds = new SmartPortal.SEMS.Customer().GetAcctNo(hidCFcodeS.Value.Trim(), ddlCustType.SelectedValue, ref IPCERRORCODE, ref IPCERRORDESC);
                        break;
                }
                if (IPCERRORCODE != "0")
                {
                    throw new IPCException(IPCERRORDESC);
                }
                DataTable dtlv5Account = new DataTable();
                dtlv5Account = ds.Tables[0];

                //luu tat ca account
                // PassTemp = Encryption.Encrypt(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 8));
                string passreveal = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenIB, passlenIB);
                string pwdreset = Encryption.Encrypt(passreveal);
                PassTemp = SmartPortal.SEMS.O9Encryptpass.sha_sha256(passreveal, userName);
                string pwdresetSMS = DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - passlenSMS, passlenSMS);

                foreach (DataRow rowAccount in dtlv5Account.Rows)
                {
                    //LuuThongTinQuyen("KETOAN", gvResultlv5, tvlv5IB, tvlv5SMS, tvlv5MB, tvlv5PHO, txtlv5FullName.Text, lbllv5Level.Text, txtlv5Birth.Text, ddllv5Gender.SelectedValue, txtlv5Mobi.Text, txtlv5Email.Text, txtlv5Address.Text, userName, PassTemp, txtlv5SMSPhoneNo.Text, ddllv5SMSDefaultAcctno.SelectedValue, ddllv5Language.SelectedValue, ((cblv5CTKIsDefault.Checked == true) ? "Y" : "N"), DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5PHOPhoneNo.Text, PassTemp, ddllv5PHODefaultAcctno.SelectedValue, rowAccount["ACCOUNTNO"].ToString());
                    LuuThongTinQuyen("KETOAN", gvResultlv5, tvlv5IB, tvlv5SMS, tvlv5MB, tvlv5PHO, txtlv5FullName.Text, lbllv5Level.Text, txtlv5Birth.Text, ddllv5Gender.SelectedValue, txtlv5Mobi.Text, txtlv5Email.Text, txtlv5Address.Text, userName, PassTemp, txtlv5SMSPhoneNo.Text, ddllv5SMSDefaultAcctno.SelectedValue, ddllv5Language.SelectedValue, ((cblv5CTKIsDefault.Checked == true) ? "Y" : "N"), pwdresetSMS, txtlv5MBPhoneNo.Text, PassTemp, DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8, 6), txtlv5MBPhoneNo.Text, PassTemp, ddllv5PHODefaultAcctno.SelectedValue, ddllv5Account.SelectedValue, pwdreset);
                }

            }
            lbllv5Alert.Text = Resources.labels.recordsaved;
            #endregion
            //28/8/2015 minh fixed truong hop khong chon dich vu ma bam nut add
            if (gvResultlv5.Rows.Count == 0)
            {
                lbllv5Alert.Text = Resources.labels.banchuadangkydichvu;
                return;
            }

        }
        catch (IPCException IPCex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["ipcec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemlv5_Click", IPCex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["ipcec"], Request.Url.Query);
        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], "Widgets_SEMSCustomerList_Add_Widget", "btnThemlv5_Click", ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void btnlv4Huy_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tbllv4Result = (DataTable)ViewState["QUANLYTAICHINH"];

            tbllv4Result.Rows.Clear();

            ViewState["QUANLYTAICHINH"] = tbllv4Result;
            gvResultlv4.DataSource = tbllv4Result.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultlv4.DataBind();

            lbllv4Alert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void btnlv5Huy_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tbllv5Result = (DataTable)ViewState["KETOAN"];

            tbllv5Result.Rows.Clear();

            ViewState["KETOAN"] = tbllv5Result;
            gvResultlv5.DataSource = tbllv5Result.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultlv5.DataBind();

            lbllv5Alert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    protected void gvResultlv4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultlv4.PageIndex = e.NewPageIndex;
            gvResultlv4.DataSource = (DataTable)ViewState["QUANLYTAICHINH"];
            gvResultlv4.DataBind();
        }
        catch
        {
        }
    }

    protected void gvResultlv5_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvResultlv5.PageIndex = e.NewPageIndex;
            gvResultlv5.DataSource = (DataTable)ViewState["KETOAN"];
            gvResultlv5.DataBind();
        }
        catch
        {
        }
    }
    protected void gvResultlv4_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultlv4 = (DataTable)ViewState["QUANLYTAICHINH"];

            DataRow[] delRow = tblResultlv4.Select("colIBUserName='" + gvResultlv4.Rows[e.RowIndex].Cells[1].Text + "' AND colAccount='" + gvResultlv4.Rows[e.RowIndex].Cells[2].Text + "' AND colRole='" + gvResultlv4.Rows[e.RowIndex].Cells[3].Text + "'");
            foreach (DataRow r in delRow)
            {
                //tblResultQT.Rows.RemoveAt(e.RowIndex + (gvResultQuanTri.PageIndex * gvResultQuanTri.PageSize));
                tblResultlv4.Rows.Remove(r);
            }
            //tblResultChuTaiKhoan.Rows.RemoveAt(e.RowIndex + (gvResultChuTaiKhoan.PageIndex * gvResultChuTaiKhoan.PageSize));

            ViewState["QUANLYTAICHINH"] = tblResultlv4;
            gvResultlv4.DataSource = tblResultlv4.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultlv4.DataBind();

            lbllv4Alert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }

    protected void gvResultlv5_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblResultlv5 = (DataTable)ViewState["KETOAN"];

            DataRow[] delRow = tblResultlv5.Select("colIBUserName='" + gvResultlv5.Rows[e.RowIndex].Cells[1].Text + "' AND colAccount='" + gvResultlv5.Rows[e.RowIndex].Cells[2].Text + "' AND colRole='" + gvResultlv5.Rows[e.RowIndex].Cells[3].Text + "'");
            foreach (DataRow r in delRow)
            {
                //tblResultQT.Rows.RemoveAt(e.RowIndex + (gvResultQuanTri.PageIndex * gvResultQuanTri.PageSize));
                tblResultlv5.Rows.Remove(r);
            }
            //tblResultChuTaiKhoan.Rows.RemoveAt(e.RowIndex + (gvResultChuTaiKhoan.PageIndex * gvResultChuTaiKhoan.PageSize));

            ViewState["KETOAN"] = tblResultlv5;
            gvResultlv5.DataSource = tblResultlv5.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResultlv5.DataBind();

            lbllv5Alert.Text = Resources.labels.recorddeleted;
        }
        catch
        {
        }
    }
    void LuuThongTinQuyen_sai(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string isDefault, string SMSPinCode, string MBPhone, string MBPass, string MBPinCode, string PHOPhone, string PHOPass, string PHODefaultAcctno, string Account)
    {
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
            DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault");
            DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");
            DataColumn colMBPhone = new DataColumn("colMBPhone");
            DataColumn colMBPass = new DataColumn("colMBPass");
            DataColumn colMBPinCode = new DataColumn("colMBPinCode");
            DataColumn colPHOPhone = new DataColumn("colPHOPhone");
            DataColumn colPHOPass = new DataColumn("colPHOPass");
            DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");
            DataColumn colAccount = new DataColumn("colAccount");
            DataColumn colRole = new DataColumn("colRole");
            DataColumn colRoleID = new DataColumn("colRoleID");
            DataColumn colTranCode = new DataColumn("colTranCode");
            DataColumn colTranCodeID = new DataColumn("colTranCodeID");
            DataColumn colServiceID = new DataColumn("colServiceID");

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
            tblNguoiUyQuyen.Columns.Add(colPHOPhone);
            tblNguoiUyQuyen.Columns.Add(colPHOPass);
            tblNguoiUyQuyen.Columns.Add(colPHODefaultAcctno);
            tblNguoiUyQuyen.Columns.Add(colAccount);
            tblNguoiUyQuyen.Columns.Add(colRole);
            tblNguoiUyQuyen.Columns.Add(colRoleID);
            tblNguoiUyQuyen.Columns.Add(colTranCode);
            tblNguoiUyQuyen.Columns.Add(colTranCodeID);
            tblNguoiUyQuyen.Columns.Add(colServiceID);

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
                            #region luu quyen ib khi null
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
                            rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;

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
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu quyen SMS khi null
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
                            rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                            //}
                            //else
                            //{
                            //}
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
                            #region luu quyen MB khi null
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
                            rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;

                            tblNguoiUyQuyen.Rows.Add(rowNguoiUyQuyen);
                            #endregion
                            //}
                            //else
                            //{
                            //}
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
                            #region luu quyen PHO khi null
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
                            rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
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
                }
            }

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole"); ;
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
                                #region luu quyen IB ton tai
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
                                rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;

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
                                #region luu quyen SMS ton tai session
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
                                rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;

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
                                #region luu quyen MB ton tai session
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
                                rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;

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
                                #region luu thong tin PHO khi ton tai session
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
                                rowNguoiUyQuyen["colSMSIsDefault"] = isDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno;
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

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole"); ;
            gvResult.DataBind();
        }
    }
    protected void btnAuthorizerDetail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            txtFullnameNguoiUyQuyen.Text = string.Empty;
            txtEmailNguoiUyQuyen.Text = string.Empty;
            txtPhoneNguoiUyQuyen.Text = string.Empty;
            ddlGenderNguoiUyQuyen.SelectedIndex = 0;
            txtBirthNguoiUyQuyen.Text = string.Empty;
            txtAddressNguoiUyQuyen.Text = string.Empty;

            Hashtable hasAuthorizerInfo = new Hashtable();
            string ctmType = "P";
            hasAuthorizerInfo = new SmartPortal.SEMS.Customer().GetCustInfo(txtAuthorizerCode.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }

            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtFullnameNguoiUyQuyen.Text = hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.EMAIL] != null)
                txtEmailNguoiUyQuyen.Text = hasAuthorizerInfo[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.PHONE] != null)
                txtPhoneNguoiUyQuyen.Text = hasAuthorizerInfo[SmartPortal.Constant.IPC.PHONE].ToString();
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    ddlGenderNguoiUyQuyen.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(hasAuthorizerInfo[SmartPortal.Constant.IPC.SEX].ToString()).ToString());

                    if (hasAuthorizerInfo[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddlGenderNguoiUyQuyen.Enabled = true;
                    }
                    else
                    {
                        ddlGenderNguoiUyQuyen.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(hasAuthorizerInfo[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
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
            if (hasAuthorizerInfo[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtAddressNguoiUyQuyen.Text = hasAuthorizerInfo[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }

            //29.10.2015 minh add to generate userid
            txtUserNameNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID(hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString(), hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString(), hasAuthorizerInfo[SmartPortal.Constant.IPC.LICENSE].ToString()) + "O2";
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {

                txtMBPhoneNguoiUyQuyen.Text = txtUserNameNguoiUyQuyen.Text;
                txtPHOPhoneNguoiUyQuyen.Text = txtMBPhoneNguoiUyQuyen.Text;
            }
            else
            {

                txtMBPhoneNguoiUyQuyen.Text = SmartPortal.Common.Utilities.Utility.GetID("", hasAuthorizerInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), "", 10) + "O2";
                txtPHOPhoneNguoiUyQuyen.Text = txtMBPhoneNguoiUyQuyen.Text;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSecondUserDetail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            txtFullNameLevel2.Text = string.Empty;
            txtEmailLevel2.Text = string.Empty;
            txtPhoneLevel2.Text = string.Empty;
            ddlGenderLevel2.SelectedIndex = 0;
            txtBirthdayLevel2.Text = string.Empty;
            txtAddresslevel2.Text = string.Empty;

            Hashtable hasSecondUserInfo = new Hashtable();
            string ctmType = "P";
            hasSecondUserInfo = new SmartPortal.SEMS.Customer().GetCustInfo(txtSecondUserCode.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }

            if (hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtFullNameLevel2.Text = hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.EMAIL] != null)
                txtEmailLevel2.Text = hasSecondUserInfo[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.PHONE] != null)
                txtPhoneLevel2.Text = hasSecondUserInfo[SmartPortal.Constant.IPC.PHONE].ToString();
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    ddlGenderLevel2.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(hasSecondUserInfo[SmartPortal.Constant.IPC.SEX].ToString()).ToString());

                    if (hasSecondUserInfo[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddlGenderLevel2.Enabled = true;
                    }
                    else
                    {
                        ddlGenderLevel2.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(hasSecondUserInfo[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
                    txtBirthdayLevel2.Text = birthDate.Equals("01/01/1900") ? "" : birthDate;

                    if (txtBirthdayLevel2.Text.Trim() == "")
                    {
                        txtBirthdayLevel2.Enabled = true;
                    }
                    else
                    {
                        txtBirthdayLevel2.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasSecondUserInfo[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtAddresslevel2.Text = hasSecondUserInfo[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }
            //29.10.2015 minh add to generate userid
            txtUserNameLevel2.Text = SmartPortal.Common.Utilities.Utility.GetID(hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString(), hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString(), hasSecondUserInfo[SmartPortal.Constant.IPC.LICENSE].ToString()) + "O3";
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {

                txtMBPhoneLevel2.Text = txtUserNameLevel2.Text;
                txtPHOPhoneLevel2.Text = txtMBPhoneLevel2.Text;
            }
            else
            {

                txtMBPhoneLevel2.Text = SmartPortal.Common.Utilities.Utility.GetID("", hasSecondUserInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), "", 10) + "O3";
                txtPHOPhoneLevel2.Text = txtMBPhoneLevel2.Text;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAdministratorDetail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            txtFullNameQT.Text = string.Empty;
            txtEmailQT.Text = string.Empty;
            txtPhoneQT.Text = string.Empty;
            ddlGenderQT.SelectedIndex = 0;
            txtBirthQT.Text = string.Empty;
            txtAddressQT.Text = string.Empty;

            Hashtable hasAdministratorInfo = new Hashtable();
            string ctmType = "P";
            hasAdministratorInfo = new SmartPortal.SEMS.Customer().GetCustInfo(txtAdministratorCode.Text.Trim(), ctmType, ref IPCERRORCODE, ref IPCERRORDESC);

            if (IPCERRORCODE != "0")
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTCODE] == null)
            {
                throw new IPCException(SmartPortal.Constant.IPC.ERRORCODE.CUSTNOTEXIST);
            }

            if (hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTNAME] != null)
                txtFullNameQT.Text = hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString();
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.EMAIL] != null)
                txtEmailQT.Text = hasAdministratorInfo[SmartPortal.Constant.IPC.EMAIL].ToString();
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.PHONE] != null)
                txtPhoneQT.Text = hasAdministratorInfo[SmartPortal.Constant.IPC.PHONE].ToString();
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.SEX] != null)
            {
                try
                {
                    ddlGenderQT.SelectedValue = SmartPortal.Common.Utilities.Utility.FormatStringCore(int.Parse(hasAdministratorInfo[SmartPortal.Constant.IPC.SEX].ToString()).ToString());

                    if (hasAdministratorInfo[SmartPortal.Constant.IPC.SEX].ToString().Trim() == "")
                    {
                        ddlGenderQT.Enabled = true;
                    }
                    else
                    {
                        ddlGenderQT.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.DOB] != null)
            {
                try
                {
                    string birthDate = SmartPortal.Common.Utilities.Utility.IsDateTime2(hasAdministratorInfo[SmartPortal.Constant.IPC.DOB].ToString()).ToString("dd/MM/yyyy");
                    txtBirthQT.Text = birthDate.Equals("01/01/1900") ? "" : birthDate;

                    if (txtBirthQT.Text.Trim() == "")
                    {
                        txtBirthQT.Enabled = true;
                    }
                    else
                    {
                        txtBirthQT.Enabled = false;
                    }
                }
                catch
                {
                }
            }
            if (hasAdministratorInfo[SmartPortal.Constant.IPC.ADDRESS] != null)
            {
                txtAddressQT.Text = hasAdministratorInfo[SmartPortal.Constant.IPC.ADDRESS].ToString();
            }
            //29.10.2015 minh add to generate userid
            txtIBUserNameQT.Text = SmartPortal.Common.Utilities.Utility.GetID(hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTNAME].ToString(), hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString(), hasAdministratorInfo[SmartPortal.Constant.IPC.LICENSE].ToString()) + "O4";
            if (int.Parse(ConfigurationManager.AppSettings["IBMBSameUser"].ToString()) == 1)
            {

                txtMBPhoneQT.Text = txtIBUserNameQT.Text;
                txtPHOPhoneQT.Text = txtMBPhoneQT.Text;
            }
            else
            {

                txtMBPhoneQT.Text = SmartPortal.Common.Utilities.Utility.GetID("", hasAdministratorInfo[SmartPortal.Constant.IPC.CUSTCODE].ToString().Trim(), "", 10) + "O1";
                txtPHOPhoneQT.Text = txtMBPhoneQT.Text;
            }

        }
        catch (Exception ex)
        {
        }
    }
    //void LuuThongTinQuyenadvance(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string MBPhone, string MBPass, string PHOPhone, string PHOPass, string Account, string SMSIsDefault, string SMSPinCode, string MBPinCode, string PHODefaultAcctno)
    void LuuThongTinQuyenadvance(string sessionName, GridView gvResult, TreeView tvRole, TreeView tvSMSRole, TreeView tvMBRole, TreeView tvPHORole, string fullName, string level, string birthday, string gender, string phone, string email, string address, string IBUserName, string IBPass, string SMSPhone, string SMSDefaultAcctno, string SMSDefaultLang, string MBPhone, string MBPass, string PHOPhone, string PHOPass, string Account, string SMSIsDefault, string SMSPinCode, string MBPinCode, string PHODefaultAcctno, string pwdreset)
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
            DataColumn colMBPhone = new DataColumn("colMBPhone");
            DataColumn colMBPass = new DataColumn("colMBPass");
            DataColumn colPHOPhone = new DataColumn("colPHOPhone");
            DataColumn colPHOPass = new DataColumn("colPHOPass");
            DataColumn colAccount = new DataColumn("colAccount");
            DataColumn colRole = new DataColumn("colRole");
            DataColumn colRoleID = new DataColumn("colRoleID");
            DataColumn colTranCode = new DataColumn("colTranCode");
            DataColumn colTranCodeID = new DataColumn("colTranCodeID");
            DataColumn colServiceID = new DataColumn("colServiceID");

            DataColumn colSMSIsDefault = new DataColumn("colSMSIsDefault"); // moi them
            DataColumn colSMSPinCode = new DataColumn("colSMSPinCode");// moi them
            DataColumn colMBPinCode = new DataColumn("colMBPinCode");// moi them
            DataColumn colPHODefaultAcctno = new DataColumn("colPHODefaultAcctno");// moi them
            DataColumn colIBPolicy = new DataColumn("colIBPolicy");
            DataColumn colSMSPolicy = new DataColumn("colSMSPolicy");
            DataColumn colMBPolicy = new DataColumn("colMBPolicy");
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
            tblNguoiUyQuyen.Columns.Add(colpwdreset);

            //tblNguoiUyQuyen.Columns.Add(colSMSIsDefault);
            //tblNguoiUyQuyen.Columns.Add(colSMSPinCode);
            //tblNguoiUyQuyen.Columns.Add(colMBPinCode);
            //tblNguoiUyQuyen.Columns.Add(colPHODefaultAcctno);

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
                            #region luu thong tin IB khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            //if (nodeTrancodeIBNguoiUyQuyen.Checked)
                            //{
                            #region luu thong tin sms khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            #region luu thong tin MB khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;
                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                            switch (sessionName)
                            {
                                case "CHUTAIKHOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                    break;
                                case "NGUOIUYQUYEN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                    break;
                                case "LEVEL2":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                    break;
                                case "NGUOIQUANTRI":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                    break;
                                case "CHUTAIKHOANS":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                    break;
                                case "QUANLYTAICHINH":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                    break;
                                case "KETOAN":
                                    rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                    rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                            #region luu thong tin PHO khi null
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
                            rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                            rowNguoiUyQuyen["colMBPass"] = MBPass;
                            rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                            rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                            rowNguoiUyQuyen["colAccount"] = Account;
                            rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                            rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                            rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                            //new
                            rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                            rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                            rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                            rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();

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

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
            #endregion
        }
        else
        {
            DataTable tblNguoiUyQuyen = (DataTable)ViewState[sessionName];

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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.IB;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                                #region luu thong tin quyen SMS khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.SMS;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                                #region luu thong tin quyen MB khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.MB;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();
                                switch (sessionName)
                                {
                                    case "CHUTAIKHOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIB.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMS.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMB.SelectedValue.ToString();
                                        break;
                                    case "NGUOIUYQUYEN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSau.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBau.SelectedValue.ToString();
                                        break;
                                    case "LEVEL2":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSl2.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBl2.SelectedValue.ToString();
                                        break;
                                    case "NGUOIQUANTRI":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSqt.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBqt.SelectedValue.ToString();

                                        break;
                                    case "CHUTAIKHOANS":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSs.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBs.SelectedValue.ToString();
                                        break;
                                    case "QUANLYTAICHINH":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl4.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl4.SelectedValue.ToString();
                                        break;
                                    case "KETOAN":
                                        rowNguoiUyQuyen["colIBPolicy"] = ddlpolicyIBsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colSMSPolicy"] = ddlpolicySMSsl5.SelectedValue.ToString();
                                        rowNguoiUyQuyen["colMBPolicy"] = ddlpolicyMBsl5.SelectedValue.ToString();
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
                                #region luu thong tin quyen PHO khac null
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
                                rowNguoiUyQuyen["colMBPhone"] = MBPhone.Trim();
                                rowNguoiUyQuyen["colMBPass"] = MBPass;
                                rowNguoiUyQuyen["colPHOPhone"] = PHOPhone.Trim();
                                rowNguoiUyQuyen["colPHOPass"] = PHOPass;
                                rowNguoiUyQuyen["colAccount"] = Account;
                                rowNguoiUyQuyen["colRole"] = nodeRoleIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colRoleID"] = nodeRoleIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colTranCode"] = nodeTrancodeIBNguoiUyQuyen.Text;
                                rowNguoiUyQuyen["colTranCodeID"] = nodeTrancodeIBNguoiUyQuyen.Value;
                                rowNguoiUyQuyen["colServiceID"] = SmartPortal.Constant.IPC.PHO;

                                //new
                                rowNguoiUyQuyen["colSMSIsDefault"] = SMSIsDefault;
                                rowNguoiUyQuyen["colSMSPinCode"] = SMSPinCode;
                                rowNguoiUyQuyen["colMBPinCode"] = MBPinCode;
                                rowNguoiUyQuyen["colPHODefaultAcctno"] = PHODefaultAcctno.Trim();

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

            ViewState[sessionName] = tblNguoiUyQuyen;
            gvResult.DataSource = tblNguoiUyQuyen.DefaultView.ToTable(true, "colFullName", "colIBUserName", "colAccount", "colRole");
            gvResult.DataBind();
        }
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


    protected void ddlpolicyIBsl4_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlpolicyMBsl4.SelectedValue = ddlpolicyIBsl4.SelectedValue;
        }
        catch
        { }
    }
    protected void ddlpolicyIBsl5_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlpolicyMBsl5.SelectedValue = ddlpolicyIBsl5.SelectedValue;
        }
        catch
        { }
    }
}
