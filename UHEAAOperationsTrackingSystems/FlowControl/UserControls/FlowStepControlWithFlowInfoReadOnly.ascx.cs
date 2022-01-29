using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
    public partial class FlowStepControlWithFlowInfoReadOnly : System.Web.UI.UserControl
    {

        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public string AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public string NotificationKey { get; set; }
        public string StaffAssignment { get; set; }
        public string ControlDisplayText { get; set; }
        public string StepDescription { get; set; }
        public string FlowDescription { get; set; }
        public string TheSystem { get; set; }
        public string Status { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}