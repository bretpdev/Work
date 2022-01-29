using System;

namespace RegentsApp
{
    public partial class CreditInstructions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 600000;
            string userName;
            try
            {
                userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeout");
            }
        }

        protected void btnInfoSaveReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLogout.aspx");
        }

        protected void btnInfoNextPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEnglishCredit.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmHighSchoolInfo.aspx");
        }
    }
}
