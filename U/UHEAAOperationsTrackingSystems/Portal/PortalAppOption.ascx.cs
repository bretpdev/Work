using System;

namespace UHEAAOperationsTrackingSystems
{
    public partial class PortalAppOption : System.Web.UI.UserControl
    {

        public string ApplicationNameText { get; set; }
        public string ApplicationImageFileNameAndPath { get; set; }
        public string ApplicationMainPageNameAndPath { get; set; }
        public string UHEAASystem { get; set; }
        public string UHEAAAccessKey { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!(this.Page is Default) && UHEAAAccessKey != null && UHEAASystem != null) //user name session variable not set if on defualt
            {
                this.Visible = DataAccessBase.HasAccess(UHEAAAccessKey, this.Page, UHEAASystem);
            }
            LinkButtonForAppImage.OnClientClick = string.Format("javascript: MagicalOpenNewWindow('{0}');", ApplicationMainPageNameAndPath);
            base.OnLoad(e);
        }

    }
}