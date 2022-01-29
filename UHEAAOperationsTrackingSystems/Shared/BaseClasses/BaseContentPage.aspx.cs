using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace UHEAAOperationsTrackingSystems
{
    public partial class BaseContentPage : System.Web.UI.Page
    {

        private Look _myLook;
        protected LookCoordinator.SystemLook _systemLook = LookCoordinator.SystemLook.NotSet; //give default value so exception is thrown if value isn't set.
        public const string WINDOWS_USER_NAME_SESSION_VARIABLE_TEXT = "UserName";
        public const string ACCESS_LIST_SESSION_VARIABLE_TEXT = "AccessList";
		public const string SQL_USER_ID = "SqlUserId";

        protected override void OnPreInit(EventArgs e)
        {
            //check for user authentication (must be done in preinit because the controls expect valid session vars to be in place.)
			if ((Session[SQL_USER_ID] == null || Session[SQL_USER_ID].ToString().Length == 0) && !(this is Default))
            {
                //redirect to login screen if not authenticated
                Response.Redirect("/Default.aspx?DisplayInformationMessage=True");
            }
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            //set up css and look
            _myLook = LookCoordinator.GetLook(_systemLook);
            //add CSS code for master page elements
            Literal css = new Literal();
            css.Text = _myLook.CSSText;
            base.Page.Header.Controls.Add(css);
            //change title text
            ((UHEAAOperationsTrackingSystems)this.Master).TitleText = _myLook.TitleText;
            base.OnInit(e);
        }

    }
}
