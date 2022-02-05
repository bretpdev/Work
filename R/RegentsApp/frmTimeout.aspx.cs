using System;

namespace RegentsApp
{
    public partial class Timeout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
            DataAccessLayer dal = new DataAccessLayer();
            try
            {
                dal.LogoutTime(Session["UserName"].ToString());
            }
            catch (Exception)
            {
                //Intentionally left blank to stop the website from crashing
            }
            Session.Abandon();
        }
    }
}
