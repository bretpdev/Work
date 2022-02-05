using System;
using System.Linq;

namespace RegentsApp
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        DataAccessLayer dal = new DataAccessLayer();
        string tempUserName;
        CheckUserNameForgot user;
        CheckUserNameByEmail email;
        GetQuestions q1;
        GetQuestions q2;
        string answer1;
        string answer2;

        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 300000;
            MaintainScrollPositionOnPostBack = true;
            lblInvalidAccount.Text = "USER NAME OR EMAIL ADDRESS NOT ON FILE";
            tbUserName.Focus();
            try
            {
                tempUserName = Session["TempUserName"].ToString();
            }
            catch (Exception)
            {
            }
        }

        protected void btnFindUser_Click(object sender, EventArgs e)
        {
            lblSeq1.Visible = false;
            lblSeq2.Visible = false;
            lblAnswer1.Visible = false;
            lblAnswer2.Visible = false;
            lblQuestion1.Visible = false;
            lblQuestion2.Visible = false;
            tbAnswer1.Visible = false;
            tbAnswer2.Visible = false;
            btnSecuritySubmit.Visible = false;
            lblInvalidAccount.Visible = false;
            lblInvalidAccount.Text = "Please enter your username or email address";
            if (tbEmail.Text == "" && tbUserName.Text == "")
            {
                lblInvalidAccount.Visible = true;
                return;
            }
            else if (tbEmail.Text != "" && tbUserName.Text != "")
            {
                lblInvalidAccount.Text = "Please enter a username or email address but not both.";
                lblInvalidAccount.Visible = true;
                return;
            }
            if (tbUserName.Text != "")
            {
                user = dal.CheckForgotUserName(tbUserName.Text.Replace("'", "''"));
            }
            else if (tbEmail.Text != "")
            {
                email = dal.CheckUserNameByEmail(tbEmail.Text);
            }
            if (user != null || email != null)
            {

                if (user != null)
                {
                    q1 = dal.GetQuestions(user.SecurityCode1);
                    q2 = dal.GetQuestions(user.SecurityCode2);
                    Session["TempUserName"] = user.UserName;
                    answer1 = user.Answer1;
                    answer2 = user.Answer2;
                    lblQuestion1.Text = q1.Description;
                    lblQuestion2.Text = q2.Description;
                }
                else if (email != null)
                {
                    q1 = dal.GetQuestions(email.SecurityCode1);
                    q2 = dal.GetQuestions(email.SecurityCode2);
                    Session["TempUserName"] = email.UserName;
                    answer1 = email.Answer1;
                    answer2 = email.Answer2;
                    lblQuestion1.Text = q1.Description;
                    lblQuestion2.Text = q2.Description;
                }
                lblSeq1.Visible = true;
                lblSeq2.Visible = true;
                lblAnswer1.Visible = true;
                lblAnswer2.Visible = true;
                lblQuestion1.Visible = true;
                lblQuestion2.Visible = true;
                tbAnswer1.Visible = true;
                tbAnswer2.Visible = true;
                btnSecuritySubmit.Visible = true;
                btnFindUser.Enabled = false;
                tbAnswer1.Focus();
            }
            else
            {
                lblInvalidAccount.Visible = true;
                tbUserName.Text = "";
                tbEmail.Text = "";
                return;
            }
        }

        protected void btnSecuritySubmit_Click(object sender, EventArgs e)
        {
            btnSecuritySubmit.Focus();
            lblInvalidAnswer1.Visible = false;
            lblInvalidAnswer2.Visible = false;
            user = dal.CheckForgotUserName(Session["TempUserName"].ToString());
            if (tbAnswer1.Text.ToUpper() != user.Answer1.ToUpper())
            {
                lblInvalidAnswer1.Visible = true;
                tbAnswer1.Text = "";
            }
            if (tbAnswer2.Text.ToUpper() != user.Answer2.ToUpper())
            {
                lblInvalidAnswer2.Visible = true;
                tbAnswer2.Text = "";
            }
            if (tbAnswer1.Text.ToUpper() == user.Answer1.ToUpper() && tbAnswer2.Text.ToUpper() == user.Answer2.ToUpper())
            {
                tbUserNameFound.Visible = true;
                tbNewPass.Visible = true;
                tbConfPass.Visible = true;
                tbUserNameFound.Text = Session["TempUserName"].ToString();
                lblUserNameFound.Visible = true;
                lblNewPass.Visible = true;
                lblConfPass.Visible = true;
                lblResetPass.Visible = true;
                btnLogin.Visible = true;
                btnSecuritySubmit.Enabled = false;
                tbNewPass.Focus();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Focus();
            vaNewPass.Enabled = true;
            vaConfPass.Enabled = true;
            if (tbNewPass.Text == "")
            {
                vaNewPass.IsValid = false;
                return;
            }
            else if (tbNewPass.Text.Contains(' '))
            {
                tbNewPass.Text = "";
                tbConfPass.Text = "";
                vaNewPass.Text = "Spaces not allowed in password";
                vaNewPass.IsValid = false;
            }
            else if (dal.UpdatePassword(tbUserNameFound.Text, tbNewPass.Text))
            {
                HasBeenSubmitted didSubmit = dal.Submitted(tbUserNameFound.Text);
                if (didSubmit != null)
                {
                    if (didSubmit.SubmitDate <= DateTime.Now)
                    {
                        Session["UserName"] = Session["TempUserName"].ToString();
                        Response.Redirect("frmPostSubmit.aspx");
                    }
                }
                else
                {
                    Session["UserName"] = Session["TempUserName"].ToString();
                    Response.Redirect("frmPersonalInfo.aspx");
                }
            }
            else
                Response.Redirect("error.aspx");
        }
    }
}
