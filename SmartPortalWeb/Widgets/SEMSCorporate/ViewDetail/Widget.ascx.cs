﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Widgets_SEMS_Corporate_ViewDetail_Widget : WidgetBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Widget._TITLE = Resources.labels.viewCorporate;
        Widget._IMAGE = "~/widgets/SEMSCorporate/Images/Bank.png";
    }
}