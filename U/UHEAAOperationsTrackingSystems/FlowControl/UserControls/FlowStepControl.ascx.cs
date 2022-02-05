using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;

namespace UHEAAOperationsTrackingSystems
{
    public partial class FlowStepControl : System.Web.UI.UserControl
    {

        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public string AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public string NotificationKey { get; set; }
        public string StaffAssignment { get; set; }
        public string StaffAssignmentCalculationID { get; set; }
        public string ControlDisplayText { get; set; }
        public string Description { get; set; }
        public string DataValidationID { get; set; }
        public string Status { get; set; }

        private FlowStep MovePropertiesIntoAFlowStep()
        {
			List<DisplayUser> users = new List<DisplayUser>(DataAccessBase.GetSystemUsers());
            FlowStep step = new FlowStep();
            step.FlowID = hfFlowID.Value;
            step.FlowStepSequenceNumber = int.Parse(lblFlowStepSequenceNumber.Text);
            step.AccessAlsoBasedOffBusinessUnit = bool.Parse(cmbBusinessUnit.SelectedValue);
            step.AccessKey = cmbAccessKey.SelectedValue;
            step.NotificationKey = cmbNotificationKey.SelectedValue;
			DisplayUser user = users.SingleOrDefault(p => p.LegalName == cmbStaffAssignment.SelectedItem.Text);
			step.StaffAssignment = user == null ? 0 : user.ID;
            step.StaffAssignmentCalculationID = txtStaffAssignmentCalculationID.Text;
            step.ControlDisplayText = txtControlDisplayText.Text;
            step.Description = txtDescription.Text;
            step.DataValidationID = txtDataValidationID.Text;
            step.Status = txtStatus.Text;
            return step;
        }

        protected void btnMoveUp_Click(object sender, ImageClickEventArgs e)
        {
            FlowStep stepToMove = MovePropertiesIntoAFlowStep();
            FlowControlDataAccess.MoveStep(stepToMove, FlowControlDataAccess.MoveDirection.Up);
            ((FlowControl)this.Page).RefreshChangeStepRepeater();
            lblStepChangeUpdateResponse.Text = "Step moved up.";
            lblStepChangeUpdateResponse.Visible = true;
        }

        protected void btnMoveDown_Click(object sender, ImageClickEventArgs e)
        {
            FlowStep stepToMove = MovePropertiesIntoAFlowStep();
            FlowControlDataAccess.MoveStep(stepToMove, FlowControlDataAccess.MoveDirection.Down);
            ((FlowControl)this.Page).RefreshChangeStepRepeater();
            lblStepChangeUpdateResponse.Text = "Step moved down.";
            lblStepChangeUpdateResponse.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            FlowStep stepToDelete = MovePropertiesIntoAFlowStep();
            FlowControlDataAccess.DeleteStep(stepToDelete);
            ((FlowControl)this.Page).RefreshChangeStepRepeater();
            lblStepChangeUpdateResponse.Visible = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            FlowStep stepToUpdate = MovePropertiesIntoAFlowStep();
            FlowControlDataAccess.UpdateStep(stepToUpdate);
            lblStepChangeUpdateResponse.Text = "Step was updated.";
            lblStepChangeUpdateResponse.Visible = true;
        }


        protected void cmbStaffAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStaffAssignment.SelectedValue == "Use Listed Calculation")
            {
                txtStaffAssignmentCalculationID.Enabled = true;
                lblStaffAssignmentCalculationID.Enabled = true;
            }
            else
            {
                txtStaffAssignmentCalculationID.Enabled = false;
                lblStaffAssignmentCalculationID.Enabled = false;
                txtStaffAssignmentCalculationID.Text = string.Empty;
            }
        }

        protected void cmbStaffAssignment_DataBound(object sender, EventArgs e)
        {
            if (cmbStaffAssignment.SelectedValue == "Use Listed Calculation")
            {
                txtStaffAssignmentCalculationID.Enabled = true;
                lblStaffAssignmentCalculationID.Enabled = true;
            }
            else
            {
                txtStaffAssignmentCalculationID.Enabled = false;
                lblStaffAssignmentCalculationID.Enabled = false;
            }
        }

    }
}