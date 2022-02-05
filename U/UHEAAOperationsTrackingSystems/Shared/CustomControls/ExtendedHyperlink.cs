using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace UHEAAOperationsTrackingSystems
{
    public class ExtendedHyperlink : System.Web.UI.WebControls.HyperLink
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
