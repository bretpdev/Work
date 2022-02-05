using System;

namespace RegentsApp
{
    public partial class Error : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            // Tries to maintain scroll position, doesn't work 100%
            MaintainScrollPositionOnPostBack = true;
        }
    }
}
