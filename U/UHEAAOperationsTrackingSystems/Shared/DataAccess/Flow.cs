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
    public class Flow
    {

        public string FlowID { get; set; }
        public string System { get; set; }
        public string Description { get; set; }
        public string ControlDisplayText { get; set; }
        public string UserInterfaceDisplayIndicator { get; set; }

    }
}
