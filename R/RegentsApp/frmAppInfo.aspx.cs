using System;

namespace RegentsApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            //Turn off application after deadline
            DateTime dt1 = new DateTime(2013, 2, 1, 23, 59, 59);
            DateTime dt2 = DateTime.Now;
            if (DateTime.Compare(dt2, dt1) > 0)
            {
                Response.Redirect("frmAppClosed.aspx");
            }

            timer.Interval = 900000;

            MaintainScrollPositionOnPostBack = true;
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAppInfo2.aspx");
        }
    }
}
