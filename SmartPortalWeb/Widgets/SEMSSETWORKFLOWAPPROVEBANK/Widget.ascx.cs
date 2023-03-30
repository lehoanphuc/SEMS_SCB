using SmartPortal.Common.Utilities;
using SmartPortal.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Widgets_SEMSSETWORKFLOWAPPROVEBANK_Widget : WidgetBase
{
    string IPCERRORCODE = string.Empty;
    string IPCERRORDESC = string.Empty;
    DataTable tmp = new DataTable();
    private DataTable DTGRIDTRANSACTION
    {
        get { return ViewState["DTGRIDTRANSACTION"] as DataTable; }
        set
        {
            ViewState["DTGRIDTRANSACTION"] = value;
        }
    }
    private string WORKFLOWID
    {
        get
        {
            return ViewState["WORKFLOWID"] == null ? string.Empty : ViewState["WORKFLOWID"].ToString();
        }
        set { ViewState["WORKFLOWID"] = value; }
    }
    private string ACTION
    {
        get
        {
            return ViewState["ACTION"] == null ? string.Empty : ViewState["ACTION"].ToString();
        }
        set { ViewState["ACTION"] = value; }

    }
    private string TRANSID
    {
        get
        {
            return ViewState["TRANSID"] == null ? string.Empty : ViewState["TRANSID"].ToString();
        }
        set { ViewState["TRANSID"] = value; }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = string.Empty;
            //ACTION = GetActionPage();
            if (!IsPostBack)
            {
                LoadTransaction();
                loadGroupUser();

            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void gvTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = DTGRIDTRANSACTION;


        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void colTransactionDelete_onclick(object sender, EventArgs e)
    {
        ACTION = "DELETE";

        try
        {
            if (DTGRIDTRANSACTION == null) { return; }
            string userlevel = ((sender as LinkButton).CommandArgument).ToString();
            GridViewRow row = (sender as LinkButton).Parent.Parent as GridViewRow;

            DataTable dt = DTGRIDTRANSACTION;
            Label ord = (Label)row.FindControl("colOrd");

            DataRow[] rows = dt.Select("Ord = '" + userlevel + "'");
            DataRow dataRow = dt.Select("Ord = '" + ord.Text + "'").FirstOrDefault();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dt.Rows[i][j] = dt.Rows[i][j].ToString();
                }
            }
            int idx = dt.Rows.IndexOf(dataRow);


            if (rows.Length > 0)
            {
                dt.Rows.Remove(rows[0]);
                if (TRANSID.ToString() == "")
                {
                    TRANSID = dt.Rows[0]["TransactionID"].ToString();

                }
                //dt.Rows.InsertAt(rows[0], idx - 1);

            }
            DTGRIDTRANSACTION = dt;
            DataTable dtWorkflow = DTGRIDTRANSACTION.Copy();


            BindDataGVTransaction();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void ddlTransaction_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable loadData = new SmartPortal.SEMS.Transactions().GetWorkflowBankStaffByTrans(Utility.KillSqlInjection(ddlTransaction.SelectedValue));

            DTGRIDTRANSACTION = loadData.Copy();
            if (DTGRIDTRANSACTION.Rows.Count > 0)
            {
                gvTransaction.DataSource = DTGRIDTRANSACTION;
                gvTransaction.DataBind();
            }
            else
            {
                gvTransaction.DataSource = new DataTable();
                gvTransaction.DataBind();
            }


        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"],
           this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], "");
        }
    }
    protected void colDownArrow_onclick(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = (sender as LinkButton).Parent.Parent as GridViewRow;
            Label ord = (Label)row.FindControl("colOrd");
            DataTable dt = DTGRIDTRANSACTION;
            DataRow dataRow = dt.Select("Ord = '" + ord.Text + "'").FirstOrDefault();
            int idx = dt.Rows.IndexOf(dataRow);
            DataRow newRow = dt.NewRow();
            newRow.ItemArray = dataRow.ItemArray.Clone() as object[];
            dt.Rows.Remove(dataRow);
            dt.Rows.InsertAt(newRow, idx + 1);
            DTGRIDTRANSACTION = dt;
            //ReOrderApprovalList();
            BindDataGVTransaction();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void ddlCountryId_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void colUpArrow_onclick(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = (sender as LinkButton).Parent.Parent as GridViewRow;
            Label ord = (Label)row.FindControl("colOrd");
            DataTable dt = DTGRIDTRANSACTION;
            DataRow dataRow = dt.Select("Ord = '" + ord.Text + "'").FirstOrDefault();
            int idx = dt.Rows.IndexOf(dataRow);
            DataRow newRow = dt.NewRow();
            newRow.ItemArray = dataRow.ItemArray.Clone() as object[];
            dt.Rows.Remove(dataRow);
            dt.Rows.InsertAt(newRow, idx - 1);
            DTGRIDTRANSACTION = dt;
            //ReOrderApprovalList();
            BindDataGVTransaction();
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
    protected void btnAddOrder_onclick(object sender, EventArgs e)
    {
        ACTION = "ADD";
        DataTable dt = DTGRIDTRANSACTION;
        if (dt == null)
        {
            InitDataTableTransaction();
            dt = DTGRIDTRANSACTION;


        }

        DataRow row = dt.NewRow();

        row["TransactionID"] = ddlTransaction.SelectedValue;
        row["GroupName"] = ddBankStaff.SelectedItem.Text.Trim();
        row["RoleID"] = ddBankStaff.SelectedValue;
        row["Ord"] = dt.Rows.Count + 1;
        row["RoleID"] = ddBankStaff.SelectedValue;
        if (dt.Rows.Count > 0)
        {
            //row["RoleNext"] = dt.Ro
            string RoleID = ddBankStaff.SelectedValue;
            string RoleNext = dt.Rows[dt.Rows.Count - 1]["RoleID"].ToString();
            DataRow dr = dt.Select("RoleID=" + RoleNext).FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
            if (dr != null)
            {
                dr["RoleNext"] = RoleID;
            }
            row["RoleNext"] = "";


        }
        else
        {
            row["RoleNext"] = "";

        }
        row["UserModified"] = Session["userName"].ToString();
        row["ServiceID"] = IPC.SOURCEIDVALUE;
        row["Status"] = "1";
        row["CreateBy"] = Session["userName"].ToString();
        row["CreateDated"] = DateTime.Now.ToString();





        DataRow[] result = dt.Select("RoleID='" + row["RoleID"] + "'");
        if (result.Length > 0)
        {
            lblError.Text = Resources.labels.formulainvalid;
            return;
        }
        else
        {
            dt.Rows.Add(row);
            DTGRIDTRANSACTION = dt;


            BindDataGVTransaction();

        }

    }
    private void BindDataGVTransaction()
    {
        if (DTGRIDTRANSACTION != null)
        {
            gvTransaction.DataSource = DTGRIDTRANSACTION;
            gvTransaction.DataBind();
            if (DTGRIDTRANSACTION.Rows.Count > 0)
            {
                gvTransaction.Visible = true;
            }
            else
            {
                gvTransaction.Visible = false;
            }
        }
        else
        {
            gvTransaction.Visible = false;
        }
    }
    protected void btnBack_OnClick(object sender, EventArgs e)
    {
        RedirectBackToMainPage();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string transaction = ddlTransaction.SelectedValue;
            //DataTable dtWorkflow = SaveDataWorkFlow(transaction);
            DataTable dtWorkflow = DTGRIDTRANSACTION.Copy();

            switch (ACTION)
            {
                case "ADD":

                    new SmartPortal.SEMS.ApprovalWorkflow().WorkflowInsertAllBankStaff(dtWorkflow, ref IPCERRORCODE, ref IPCERRORDESC);
                    if (IPCERRORCODE == "0")
                    {
                        lblError.Text = Resources.labels.insertworkflowsuccessful;
                        DTGRIDTRANSACTION = new DataTable();
                    }
                    else
                    {
                        lblError.Text = IPCERRORDESC;
                        return;
                    }
                    break;
                case "DELETE":
                    //DataRow row = dtWorkflow.NewRow();
                    //row["TransactionID"] = TRANSID;
                    //dtWorkflow.Rows.Add(row);
                    //DTGRIDTRANSACTION = dt;
                    pushdataforDelete(TRANSID);
                    DataTable deleteWorkflow = DTGRIDTRANSACTION.Copy();
                    new SmartPortal.SEMS.ApprovalWorkflow().WorkflowDeleteAllBankStaff(deleteWorkflow, ref IPCERRORCODE, ref IPCERRORDESC);
                    if (IPCERRORCODE == "0")
                    {

                        lblError.Text = Resources.labels.deleteworkflowsuccessful;
                        new SmartPortal.SEMS.ApprovalWorkflow().WorkflowInsertAllBankStaff(dtWorkflow, ref IPCERRORCODE, ref IPCERRORDESC);
                        if (IPCERRORCODE == "0")
                        {
                            lblError.Text = Resources.labels.insertworkflowsuccessful;

                        }
                    }
                    else
                    {
                        lblError.Text = IPCERRORDESC;
                        return;
                    }
                    break;


            }




        }

        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }

    }
    private void pushdataforDelete(string transactionID)
    {
        DataTable dtWorkflow = DTGRIDTRANSACTION.Copy();

        DataRow row = dtWorkflow.NewRow();
        row["TransactionID"] = TRANSID;
        row["GroupName"] = "";
        row["RoleID"] = "";
        row["Ord"] = "";
        row["RoleID"] = "";
        row["RoleNext"] = "";
        row["UserModified"] = "";
        row["ServiceID"] = "";
        row["Status"] = "1";
        row["CreateBy"] = "";
        row["CreateDated"] = DateTime.Now.ToString();

        dtWorkflow.Rows.Add(row);
        DTGRIDTRANSACTION = dtWorkflow.Copy();

    }
    private DataTable AddColHeaderForTable(DataTable dt, string[] arr)
    {
        if (dt == null) dt = new DataTable();
        if (arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dt.Columns.Add(new DataColumn(arr[i]));
            }
        }
        return dt;
    }
    private void InitDataTableTransaction()
    {

    }
    void LoadTransaction()
    {
        DataSet dsTranApp = new SmartPortal.SEMS.Transactions().LoadTranApp(ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE != "0")
        {
            throw new SmartPortal.ExceptionCollection.IPCException(IPCERRORDESC);
        }
        DataTable dtTranApp = new DataTable();
        dtTranApp = dsTranApp.Tables[0];

        ddlTransaction.DataSource = dtTranApp;
        ddlTransaction.DataTextField = "PAGENAME";
        ddlTransaction.DataValueField = "TRANCODE";
        ddlTransaction.DataBind();
        ddlTransaction.Items.Insert(0, new ListItem(IPC.ALL, IPC.ALL));
    }
    void loadGroupUser()
    {
        DataSet dsTranApp = new SmartPortal.SEMS.Transactions().LoadGroupName(ref IPCERRORCODE, ref IPCERRORDESC);
        if (IPCERRORCODE != "0")
        {
            throw new SmartPortal.ExceptionCollection.IPCException(IPCERRORDESC);
        }
        DataTable dtTranApp = new DataTable();
        dtTranApp = dsTranApp.Tables[0];

        ddBankStaff.DataSource = dtTranApp;
        ddBankStaff.DataTextField = "RoleName";
        ddBankStaff.DataValueField = "RoleID";
        ddBankStaff.DataBind();
    }
    protected void gvTransaction_onRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton down = (LinkButton)e.Row.FindControl("colUpArrow");
                LinkButton up = (LinkButton)e.Row.FindControl("colDownArrow");
                int idx = DTGRIDTRANSACTION.Rows.IndexOf(drv.Row);
                if (idx == 0) down.Visible = false;
                else if (idx == DTGRIDTRANSACTION.Rows.Count - 1) up.Visible = false;
            }
        }
        catch (Exception ex)
        {
            SmartPortal.Common.Log.RaiseError(System.Configuration.ConfigurationManager.AppSettings["sysec"], this.GetType().BaseType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), Request.Url.Query);
            SmartPortal.Common.Log.GoToErrorPage(System.Configuration.ConfigurationManager.AppSettings["sysec"], Request.Url.Query);
        }
    }
}