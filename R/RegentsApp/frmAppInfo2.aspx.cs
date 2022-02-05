using System;
using System.Linq;
using System.Web;

namespace RegentsApp
{
    public partial class frmAppInfo2 : System.Web.UI.Page
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        #endregion

        DataAccessLayer dal = new DataAccessLayer();

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

            //Remove the session variables created in the Personal Information screen
            Session.Remove("UpdateStudent");
            Session.Remove("UpdateStudentAddress");

            string userName;
            try
            {
                userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                userName = "";
            }
            if (userName != "")
            {
                Response.Redirect("frmPersonalInfo.aspx");
            }
        }

        protected void btnLoginSubmit_Click(object sender, EventArgs e)
        {
            UserName = tbLoginUserName.Text.Trim();
            Password = tbLoginPassword.Text.Trim();

            UserLogin userLogin = dal.UserLogin(UserName, Password);
            if (userLogin == null)
            {
                lblLoginError.Text = "User name or password is incorrect";
                tbLoginUserName.Focus();
            }
            else if (UserName.ToUpper() == userLogin.UserName.ToUpper() && Password == userLogin.PasswordHash)
            {
                //returns the username and date the if the application has been submitted
                HasBeenSubmitted didSubmit = dal.Submitted(UserName);
                if (didSubmit != null)
                {
                    if (didSubmit.SubmitDate <= DateTime.Now)
                    {
                        Session["UserName"] = UserName;
                        if (dal.LoginTime(UserName, false))
                        {
                            Response.Redirect("frmPostSubmit.aspx");
                        }
                    }
                }
                else
                {
                    Session["UserName"] = UserName;
                    if (dal.LoginTime(UserName, false))
                    {
                        Response.Redirect("frmPersonalInfo.aspx");
                    }
                }
            }
            else
            {
                lblLoginError.Text = "User name or password is incorrect";
                tbLoginUserName.Focus();
            }
        }
    }
}
