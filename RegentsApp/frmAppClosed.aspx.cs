using System;

namespace RegentsApp
{
    public partial class AppClosed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dt3 = new DateTime(2011, 11, 21, 08, 00, 00);
            if (DateTime.Now < dt3)
            {
                closed.Text = "The Regents' Scholarship online application will be available Monday Novemeber 21, 2011 at 8:00 am.";
            }
        }
    }
}
