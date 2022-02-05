using System;
using System.Collections.Generic;
using CrystalDecisions.Shared;
using RegentsApp.Reporting;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace RegentsApp
{
    public partial class AppReview : System.Web.UI.Page
    {
        DataAccessLayer dal = new DataAccessLayer();
        string _userName;

        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 900000;
            try
            {
                _userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeout.aspx");
            }
            CompletedApplication report = new CompletedApplication();
            report.SetDataSource(dal.GetAllData(_userName));
            ApplicationViewer.ReportSource = report;
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAppSubmit.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLanguageCredit.aspx");
        }

        protected void btnSaveReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLogout.aspx");
        }
    }
}
