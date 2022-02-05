using System;
using MailBee.SmtpMail;
using MailBee.Mime;

namespace RegentsApp
{
    public partial class AppSubmit : System.Web.UI.Page
    {
        DataAccessLayer dal = new DataAccessLayer();
        string _userName;

        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 600000;
            MaintainScrollPositionOnPostBack = true;

            try
            {
                _userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeout.aspx");
            }

            // Check the document table to see if the application has already been submitted
            HasBeenSubmitted didSubmit = dal.Submitted(_userName);
            if (didSubmit != null)
            {
                if (didSubmit.SubmitDate <= DateTime.Now)
                {
                    Response.Redirect("frmPostSubmit.aspx");
                }
            }
        }

        protected void btnSaveReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLogout.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAppReview.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get the username from the user cookie and the email and first name from the database
            //string userName = Request.Cookies.Get("RegentsUser").Values.Get("UserName").ToString();
            string email = dal.GetEmail(_userName);
            string firstName = dal.GetFirstName(_userName);

            // Set up and send the outgoing mail message
            Smtp.LicenseKey = "MN100-4A2E2CBC2EB52E352EE8E592264B-1B79";
            Smtp mailer = new Smtp();
            SmtpServer server = new SmtpServer();
            server.Name = "owa.utahsbr.edu";
            mailer.SmtpServers.Clear();
            mailer.DnsServers.Clear();
            mailer.SmtpServers.Add(server);
            MailMessage msg = new MailMessage();
            msg.From.AsString = "regentsscholarship@utahsbr.edu";
            msg.To.AsString = email;
            msg.Subject = "Regents' Scholarship Application - Follow-up Documents Needed";
            msg.BodyHtmlText = Properties.Resources.Regents_Email.Replace("[FirstName]", firstName);
            mailer.Message = msg;
            try
            {
                mailer.Send();
            }
            catch (Exception) {}

            if (dal.Submit(_userName))
                Response.Redirect("frmPostSubmit.aspx");
            else
                Response.Redirect("error.aspx");
        }
    }
}
