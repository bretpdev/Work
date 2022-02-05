using System;
using System.Linq;
using System.Web.UI;

namespace RegentsApp
{
    public partial class Setup : System.Web.UI.Page
    {
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

            Session.RemoveAll();
            timer.Interval = 900000;
            MaintainScrollPositionOnPostBack = true;
            lblNameUsed.Visible = false;
            lblEmailUsed.Visible = false;
            lblQuestionsNeeded.Visible = false;
            if (Properties.Settings.Default.TestMode)
            {
                dsSecQuestion.ConnectionString = Properties.Resources.ConnStringTest;
            }
            if (!IsPostBack)
            {
                tbUserName.Text = "";
                tbPassword.Text = "";
                tbPassConf.Text = "";
            }

            if (Properties.Settings.Default.TestMode)
            {
                mainDiv.Style.Add(HtmlTextWriterStyle.Display, "block");
            }
        }

        protected void btnSubmitSetup_Click(object sender, EventArgs e)
        {
            CheckUserName checkUserName = dal.CheckName(tbUserName.Text.Replace("'", "''"));
            CheckEmail checkEmail = dal.CheckEmail(tbEmlAddy.Text);
            int question1 = Convert.ToInt32(ddlSecQ1.SelectedItem.Value.ToString());
            int question2 = Convert.ToInt32(ddlSecQ2.SelectedItem.Value.ToString());
            bool valid = true;
            if (checkUserName != null)
            {
                tbPassword.Text = "";
                tbPassConf.Text = "";
                lblNameUsed.Visible = true;
                lblNameUsed.Text = "User name is already in use";
                valid = false;
            }
            if (tbUserName.Text.Length < 6)
            {
                tbPassword.Text = "";
                tbPassConf.Text = "";
                lblNameUsed.Visible = true;
                lblNameUsed.Text = "User name must be at least 6 characters";
                valid = false;
            }
            if (checkEmail != null)
            {
                if (Properties.Settings.Default.TestMode == false)
                {
                    tbPassword.Text = "";
                    tbPassConf.Text = "";
                    lblEmailUsed.Visible = true;
                    valid = false;
                }
            }
            if (tbUserName.Text.Contains(' ') || tbPassword.Text.Contains(' '))
            {
                tbPassword.Text = "";
                tbPassConf.Text = "";
                lblNameUsed.Visible = true;
                lblNameUsed.Text = "Username and password can not contain spaces";
                valid = false;
            }
            if (question1 == 0 || question2 == 0)
            {
                lblQuestionsNeeded.Text = "Please choose two questions";
                lblQuestionsNeeded.Visible = true;
                valid = false;
            }
            if (valid)
            {
                lblNameUsed.Visible = false;

                if (dal.CreateUser(tbUserName.Text.ToUpper().Trim().Replace("'", "''"), tbPassword.Text.Trim(), tbEmlAddy.Text, question1, question2, tbAns1.Text.Replace("'", "''").Trim(), tbAns2.Text.Replace("'", "''").Trim(), false, txtFirstName.Text.Replace("'", "''"), txtMiddleName.Text.Replace("'", "''"), txtLastName.Text.Replace("'", "''"), Convert.ToDateTime(txtDOB.Text)))
                {
                    Session["UserName"] = tbUserName.Text;
                    if (dal.LoginTime(Session["UserName"].ToString(), true))
                    {
                        Session["NewAccount"] = "True";
                        Response.Redirect("frmPersonalInfo.aspx");
                    }
                }
            }
            else
            {
                mainDiv.Style.Add(HtmlTextWriterStyle.Display, "inline");
                setup.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
        }
    }//End Class
}//End Namespace
