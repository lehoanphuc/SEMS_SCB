﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_WidgetHTML_GridViewPaging : System.Web.UI.UserControl
{
    public EventHandler pagingClickArgs;
    public int pageSize = 15;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GridViewPagingError.Visible = false;
            pageSize = int.Parse(PageRowSize.SelectedValue);
            if (!IsPostBack)
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = "1";
            }
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (!IsValid()) { return; };
        GetPageDisplaySummary();
    }

    private void checkSelectedPageNo()
    {
        if (SelectedPageNo.Text.Equals(string.Empty))
        {
            SelectedPageNo.Text = hdfCurrentPage.Value = "1";
        }
    }
    protected void First_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValid()) { return; };
            SelectedPageNo.Text = hdfCurrentPage.Value = "1";
            if (Convert.ToInt32(TotalRows.Value) < pageIndex * pageSize) return;
            if (pagingClickArgs != null)
            {
                pagingClickArgs(sender, e);
            }
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }
    protected void Previous_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValid()) { return; };
            if (Convert.ToInt32(SelectedPageNo.Text) > 1)
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = (Convert.ToInt32(SelectedPageNo.Text) - 1).ToString();
            }
            pagingClickArgs(sender, e);
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }
    public string total
    {
        get
        {
            return TotalRows.Value;
        }
        set
        {
            TotalRows.Value = value;
            this.Visible = !total.Equals("0");
        }
    }
    public string SelectPageChoose
    {
        get
        {
            return SelectedPageNo.Text;
        }
        set
        {
            SelectedPageNo.Text = value;
        }
    }
    public int pageIndex
    {
        get
        {
            int index = 0;
            try
            {
                if (int.Parse(SelectedPageNo.Text) > 0) index = int.Parse(SelectedPageNo.Text) - 1;
            }
            catch { }
            return index;
        }
    }
    protected void SelectedPageNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            checkSelectedPageNo();
            if (!IsValid()) {  GridViewPagingError.Visible = false; return;  };
            int currentPageNo = !string.IsNullOrEmpty(SelectedPageNo.Text.Trim()) ? Convert.ToInt32(SelectedPageNo.Text) : Convert.ToInt32(hdfCurrentPage.Value);
            if (currentPageNo <= GetTotalPagesCount())
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = (currentPageNo).ToString();
            }
            else
            {
                SelectedPageNo.Text = hdfCurrentPage.Value;
            }
            pagingClickArgs(sender, e);
        }
        catch (Exception ex)
        {
            

        }
    }

    protected void PageRowSize_onclick(object sender, System.EventArgs e)
    {
        try
        {
            if (!IsValid()) { return; };
            pageSize = int.Parse(PageRowSize.SelectedValue.ToString());
            SelectedPageNo.Text = hdfCurrentPage.Value = "1";
            pagingClickArgs(sender, e);
            GetPageDisplaySummary();
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValid()) { return; };
            int currentPageNo = Convert.ToInt32(SelectedPageNo.Text);
            if (currentPageNo < GetTotalPagesCount())
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = (currentPageNo + 1).ToString();
            }
            if (Convert.ToInt32(TotalRows.Value) < pageIndex * pageSize) return;
            if (pagingClickArgs != null)
            {
                pagingClickArgs(sender, e);
            }
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }
    protected void Last_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValid()) { return; };
            int currentPageNo = Convert.ToInt32(SelectedPageNo.Text);
            if (currentPageNo < GetTotalPagesCount())
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = GetTotalPagesCount().ToString();
            }
            pagingClickArgs(sender, e);
        }
        catch (Exception ex)
        {
            ShowGridViewPagingErrorMessage(ex.Message.ToString());
        }
    }
    private int GetTotalPagesCount()
    {
        try
        {
            int totalPages;
            int totalRecords = Convert.ToInt32(TotalRows.Value);
            if (totalRecords != -1)
            {
                totalPages = Convert.ToInt32(TotalRows.Value) / Convert.ToInt32(PageRowSize.SelectedValue);
                // total page item to be displyed
                int pageItemRemain = Convert.ToInt32(TotalRows.Value) % Convert.ToInt32(PageRowSize.SelectedValue);
                // remaing no of pages
                if (pageItemRemain > 0)// set total No of pages
                {
                    totalPages = totalPages + 1;
                }
                else
                {
                    totalPages = totalPages + 0;
                }
            }
            else
            {
                totalPages = 100000000;
            }
            return totalPages;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void GetPageDisplaySummary()
    {
        try
        {
            PageDisplaySummary.Text = Resources.labels.page + " " + SelectedPageNo.Text.ToString() + " of " + GetTotalPagesCount().ToString();
            int startRow = (Convert.ToInt32(PageRowSize.SelectedValue) * (Convert.ToInt32(SelectedPageNo.Text) - 1)) + 1;
            int endRow = Convert.ToInt32(PageRowSize.SelectedValue) * Convert.ToInt32(SelectedPageNo.Text);
            int totalRecords = Convert.ToInt32(TotalRows.Value);
            if (totalRecords >= endRow)
            {
                RecordDisplaySummary.Text = Resources.labels.danghienthi + ": " + startRow.ToString() + " - " + endRow.ToString() + " " + Resources.labels.cua + " " + totalRecords.ToString();
            }
            else
            {
                RecordDisplaySummary.Text = Resources.labels.danghienthi + ": " + startRow.ToString() + " - " + totalRecords.ToString() + " " + Resources.labels.cua + " " + totalRecords.ToString();
            }
        }
        catch (Exception ex)
        {

            //throw ex;
        }
    }
    private bool IsValid()
    {
        try
        {
            if (!IsNumeric(SelectedPageNo.Text))
            {
                ShowGridViewPagingErrorMessage("Please Insert Valid Page No.");
                return false;
            }
            else if (String.IsNullOrEmpty(SelectedPageNo.Text.Trim()) || (SelectedPageNo.Text == "0"))
            {
                SelectedPageNo.Text = hdfCurrentPage.Value = "1";
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (FormatException)
        {
            return false;
        }
    }
    private bool IsNumeric(string PageNo)
    {
        try
        {
            int i = Convert.ToInt32(PageNo);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    private void ShowGridViewPagingErrorMessage(string msg)
    {
        GridViewPagingError.Visible = true;
        GridViewPagingError.Text = "Error: " + msg;
    }

}