using System;

namespace UHEAAOperationsTrackingSystems
{
    public partial class Default : BaseContentPage
    {

        private const string BAD_LOGIN_INFORMATION_MSG = "Either an invalid user name or password was provided.  Please try again.";
        private const string INVALID_SESSION_MSG = "Either your session timed out or you never logged into the portal.  If you were logged into the portal and your session timed out then please close all portal browser windows (all apps and main portal) and log back in.";

        protected override void OnInit(EventArgs e)
        {
            txtUserName.Focus();
            _systemLook = LookCoordinator.SystemLook.DefaultAndPortal;
            base.OnInit(e);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //if (PortalDataAccess.HasAccessToPortal(txtUserName.Text, (new StringEncryption()).Encrypt(txtPassword.Text)))
            LdapInteraction ActiveDirectoryProcessor = new LdapInteraction();
            if (ActiveDirectoryProcessor.IsAuthenticated(txtUserName.Text, txtPassword.Text))
            {
				Session[SQL_USER_ID] = DataAccessBase.GetSqlUserId(txtUserName.Text);
                //send to portal page
                Response.Redirect("~/Portal/Portal.aspx");
            }
            else
            {
                lblBadLoginInfo.Text = BAD_LOGIN_INFORMATION_MSG;
                lblBadLoginInfo.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["DisplayInformationMessage"] != null && Request["DisplayInformationMessage"].ToString() == "True")
                {
                    //either the user tried to back door the portal or their session timed out
                    lblBadLoginInfo.Text = INVALID_SESSION_MSG;
                    lblBadLoginInfo.Visible = true;
                }
            }
        }

    }
}
