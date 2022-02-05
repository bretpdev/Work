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
using System.Collections.Generic;

namespace UHEAAOperationsTrackingSystems
{
    public partial class FlowControlReadOnly : System.Web.UI.UserControl
    {

        const string HIDE_STEPS = "Hide Steps";
        const string VIEW_STEPS = "View Steps";

        public string FlowID { get; set; }
        public string TheSystem { get; set; }
        public string Description { get; set; }
        public string ControlDisplayText { get; set; }
        public string UserInterfaceDisplayIndicator { get; set; }

        protected void btnViewSteps_Click(object sender, EventArgs e)
        {
            if (btnHideViewSteps.Text == VIEW_STEPS)
            {
                RepeaterSteps.DataSource = FlowControlDataAccess.GetStepsForSpecifiedFlow(lblFlowID.Text);
                RepeaterSteps.DataBind();
                btnHideViewSteps.Text = HIDE_STEPS;
            }
            else
            {
                RepeaterSteps.DataSource = new List<FlowStep>();
                RepeaterSteps.DataBind();
                btnHideViewSteps.Text = VIEW_STEPS;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnHideViewSteps.Text = VIEW_STEPS;
            }
        }

    }
}