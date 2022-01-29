using System;

namespace UHEAAOperationsTrackingSystems.Shared.CustomControls
{
    public class ExtendedLinkButton : System.Web.UI.WebControls.LinkButton
    {

        public string UHEAAAccessKey { get; set; }
        public string UHEAASystem { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            if (!(this.Page is Default)) //user name session variable not set if on defualt
            {
                this.Visible = DataAccessBase.HasAccess(UHEAAAccessKey, this.Page, UHEAASystem);
            }
            base.OnLoad(e);
        }

    }
}
