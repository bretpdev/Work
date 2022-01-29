using System;
using System.IO;

namespace RegentsApp
{
    public partial class PostSubmit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 600000;
            MaintainScrollPositionOnPostBack = true;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLogout.aspx");
        }
    }//class
}//namespace
