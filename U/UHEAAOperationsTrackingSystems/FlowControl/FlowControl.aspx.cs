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
using System.Collections.Generic;

namespace UHEAAOperationsTrackingSystems
{
    public partial class FlowControl : BaseContentPageWithSecurity
    {
        protected override void OnInit(EventArgs e)
        {
            _uheaaAccessKey = "Portal Access";
            _uheaaSystem = "ACDC Flows";
            _systemLook = LookCoordinator.SystemLook.FlowControl;
			IEnumerable<string> systems = FlowControlDataAccess.GetSystems();
			cmbAddFlow_System.DataSource = systems;
			cmbAddFlow_System.DataBind();
			cmbChangeFlow_System.DataSource = systems;
			cmbChangeFlow_System.DataBind();
			cmbSystemSteps_System.DataSource = systems;
			cmbSystemSteps_System.DataBind();
            base.OnInit(e);
        }

        #region "Flow Management"

        protected void btnAddFlow_Click(object sender, EventArgs e)
        {
            Flow flowToAdd = new Flow();
            flowToAdd.FlowID = txtAddFlow_FlowID.Text;
            flowToAdd.Description = txtAddFlow_Description.Text;
            flowToAdd.ControlDisplayText = txtAddFlow_ControlTextDisplay.Text;
            flowToAdd.UserInterfaceDisplayIndicator = txtAddFlow_UserInterfaceDisplayId.Text;
            flowToAdd.System = cmbAddFlow_System.SelectedValue;
            try 
            {
                FlowControlDataAccess.AddFlow(flowToAdd);
                lblAddFlowResult.Text = "Flow has been added to the database.";
                txtAddFlow_FlowID.Text = string.Empty;
                txtAddFlow_ControlTextDisplay.Text = string.Empty;
                txtAddFlow_Description.Text = string.Empty;
                txtAddFlow_UserInterfaceDisplayId.Text = string.Empty;
                cmbAddFlow_System.SelectedIndex = 0;
                //refresh list
                cmbChangeFlow_FlowID.Items.Clear();
                cmbChangeFlow_FlowID.Items.Add("Please Select...");
                cmbChangeFlow_FlowID.DataBind();

            }
            catch (FlowAlreadyExistsInDatabaseException ex)
            {
                lblAddFlowResult.Text = ex.Message;
            }
            finally
            {
                lblAddFlowResult.Visible = true;
            }
        }

        protected void cmbChangeFlow_FlowID_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblChangeFlowResponse.Visible = false;
            if (cmbChangeFlow_FlowID.SelectedValue != "Please Select...")
            {
                Flow flowToChange = FlowControlDataAccess.GetSpecifiedFlow(cmbChangeFlow_FlowID.SelectedValue);
                cmbChangeFlow_System.SelectedValue = flowToChange.System;
                txtChangeFlow_ControlDisplayText.Text = flowToChange.ControlDisplayText;
                txtChangeFlow_UIDisplayID.Text = flowToChange.UserInterfaceDisplayIndicator;
                txtChangeFlow_Description.Text = flowToChange.Description;
                btnChangeFlow.Enabled = true;
            }
            else
            {
                btnChangeFlow.Enabled = false;
            }
        }

        protected void btnChangeFlow_Click(object sender, EventArgs e)
        {
            Flow flowToChange = new Flow();
            flowToChange.FlowID = cmbChangeFlow_FlowID.SelectedValue;
            flowToChange.System = cmbChangeFlow_System.SelectedValue;
            flowToChange.Description = txtChangeFlow_Description.Text;
            flowToChange.ControlDisplayText = txtChangeFlow_ControlDisplayText.Text;
            flowToChange.UserInterfaceDisplayIndicator = txtChangeFlow_UIDisplayID.Text;
            FlowControlDataAccess.ChangeFlow(flowToChange);
            lblChangeFlowResponse.Text = "The flow has been updated in the database.";
            lblChangeFlowResponse.Visible = true;
        }

        #endregion

        #region "Tab Navigation"

        protected void btnFlowManagement_Click(object sender, EventArgs e)
        {
            FlowMultiView.ActiveViewIndex = 0;
        }

        protected void btnStepManagement_Click(object sender, EventArgs e)
        {
            FlowMultiView.ActiveViewIndex = 1;
        }

        protected void btnResearchFlowSteps_Click(object sender, EventArgs e)
        {
            FlowMultiView.ActiveViewIndex = 2;
        }

        protected void btnStepStaffAssignment_Click(object sender, EventArgs e)
        {
            FlowMultiView.ActiveViewIndex = 3;
        }

        #endregion

        public void RefreshChangeStepRepeater()
        {
            rptSteps.DataSource = FlowControlDataAccess.GetAndTweakStepsForSpecifiedFlow(cmbStepManage_FlowID.SelectedValue);
            rptSteps.DataBind();
        }

        protected void cmbStepManage_FlowID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Flow flowToDisplay = FlowControlDataAccess.GetSpecifiedFlow(cmbStepManage_FlowID.SelectedValue);
            lblStepManage_System.Text = flowToDisplay.System;
            lblStepManage_Description.Text = flowToDisplay.Description;
            lblAddStepResponse.Visible = false;
            cmbStepManage_AccessKey.Items.Clear();
            cmbStepManage_NotificationKey.Items.Clear();
            cmbStepManage_AccessKey.Items.Add("Please Select...");
            cmbStepManage_NotificationKey.Items.Add("Please Select...");
            cmbStepManage_AccessKey.DataBind();
            cmbStepManage_NotificationKey.DataBind();
            lblStepManage_Description.Visible = true;
            lblStepManage_System.Visible = true;
            RefreshChangeStepRepeater();
        }

        protected void btnAddStep_Click(object sender, EventArgs e)
        {
			List<DisplayUser> users = new List<DisplayUser>(DataAccessBase.GetSystemUsers());
            FlowStep newFlowStep = new FlowStep();
            newFlowStep.FlowID = cmbStepManage_FlowID.SelectedValue;
            newFlowStep.AccessAlsoBasedOffBusinessUnit = bool.Parse(cmbStepManage_BusinessUnit.SelectedValue);
            newFlowStep.AccessKey = cmbStepManage_AccessKey.SelectedValue;
            newFlowStep.NotificationKey = cmbStepManage_NotificationKey.SelectedValue;
			newFlowStep.StaffAssignment = users.Single(p => p.ID == cmbStepManage_StaffAssignment.SelectedIndex).ID;
            newFlowStep.ControlDisplayText = txtStepManage_ControlDisplayText.Text;
            newFlowStep.Description = txtStepManage_Description.Text;
            newFlowStep.Status = txtStepManage_Status.Text;
            //create step in flow
            FlowControlDataAccess.AddStepToFlow(newFlowStep);
            //update label
            lblAddStepResponse.Text = "The step has been added to the flow.";
            lblAddStepResponse.Visible = true;
            //clear out values
            cmbStepManage_BusinessUnit.SelectedIndex = 0;
            cmbStepManage_AccessKey.SelectedIndex = 0;
            cmbStepManage_NotificationKey.SelectedIndex = 0;
            cmbStepManage_StaffAssignment.SelectedIndex = 0;
            txtStepManage_ControlDisplayText.Text = string.Empty;
            txtStepManage_Description.Text = string.Empty;
            RefreshChangeStepRepeater();
        }

        protected void cmbStepManage_StaffAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStepManage_StaffAssignment.SelectedValue == "Use Listed Calculation")
            {
                txtStepManage_StaffAssignmentCalculationID.Enabled = true;
                lblStaffAssignmentCalcID.Enabled = true;
            }
            else
            {
                txtStepManage_StaffAssignmentCalculationID.Enabled = false;
                lblStaffAssignmentCalcID.Enabled = false;
                txtStepManage_StaffAssignmentCalculationID.Text = string.Empty;
            }
        }


    }
}
