using System;

namespace RegentsApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Tries to maintain scroll position, doesn't work 100%
            MaintainScrollPositionOnPostBack = true;
            Session.RemoveAll();
            
            //Turn off application after deadline
            DateTime dt1 = new DateTime(2013, 2, 1, 23, 59, 59);
            DateTime dt2 = DateTime.Now;
            if (DateTime.Compare(dt2, dt1) > 0)
            {
                Response.Redirect("frmAppClosed.aspx");
            }

            DateTime dt3 = new DateTime(2011, 11, 21, 08, 00, 00);
            if ((DateTime.Now < dt3) && !Properties.Settings.Default.TestMode)
            {
                Response.Redirect("frmAppClosed.aspx");
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAppInfo.aspx");
        }
    }
}
