using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Q;

namespace ACDCFlows
{
	public partial class ExistingFlowSteps : UserControl
	{
		private DataAccess _da;
		private List<SqlUser> _users;

		public FlowStep Step { get; set; }
		public DataAccess.MoveDirection MoveDirection { get; set; }

		public ExistingFlowSteps(DataAccess da, FlowStep step, List<string> keys, List<string> CalculationID, List<SqlUser> users, List<string> dataValid, List<string> accessKey, List<string> status, int flowSequenceNo)
		{
			InitializeComponent();
			_users = users;
			_da = da;

			Step = step;

			cboNotificationType.DataSource = keys;

			cboStaffAssignID.DataSource = CalculationID;

			cboStaffAssignment.DataSource = users;
			cboStaffAssignment.DisplayMember = "LegalName";
			cboStaffAssignment.ValueMember = "ID";

			cboBusinessUnit.SelectedIndex = step.AccessAlsoBasedOffBusinessUnit == true ? 1 : 2;
			cboDataValidation.DataSource = dataValid;
			cboAccessKey.DataSource = accessKey;
			cboStatus.DataSource = status;

			

			//The binding must happen last so all the datasources are set first
			flowStepBindingSource.DataSource = step;

			if (step.StaffAssignmentLegalName != string.Empty && !string.IsNullOrEmpty(step.StaffAssignmentLegalName))
			{
				cboStaffAssignID.Enabled = false;
				cboStaffAssignID.Text = string.Empty;
				cboStaffAssignment.Enabled = true;
				cboStaffAssignment.Text = step.StaffAssignmentLegalName;
			}
			else if (step.StaffAssignmentCalculationID != string.Empty)
			{
				cboStaffAssignment.Enabled = false;
				cboStaffAssignment.Text = string.Empty;
				cboStaffAssignID.Enabled = true;
				cboStaffAssignID.Text = step.StaffAssignmentCalculationID;
			}

			//Remove the Up button from the first step
			if (step.FlowStepSequenceNumber == 1)
			{
				btnUp.Visible = false;
			}
			else
			{
				btnUp.Visible = true;
			}

			//Remove the Down button from the last step
			if (step.FlowStepSequenceNumber == flowSequenceNo)
			{
				btnDown.Visible = false;
			}
			else
			{
				btnDown.Visible = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpdate_Click(object sender, EventArgs e)
		{
            if (cboStaffAssignment.Text != "")
                Step.StaffAssignment = ((SqlUser)_users.Where(p => p.LegalName == cboStaffAssignment.Text).First()).ID;
			UpdateFlowStep(sender, new UpdateFlowStepEventArgs(Step));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateFlowStep(object sender, UpdateFlowStepEventArgs e)
		{
			if (UserControlUpdateClicked != null)
			{
				UserControlUpdateClicked(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, EventArgs e)
		{
			DeleteFlowStep(sender, new DeleteFlowStepEventArgs(Step));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteFlowStep(object sender, DeleteFlowStepEventArgs e)
		{
			if (UserControlDeleteClicked != null)
			{
				UserControlDeleteClicked(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUp_Click(object sender, EventArgs e)
		{
			MoveUp(sender, new ButtonUpClickedEventArgs(Step, DataAccess.MoveDirection.Up));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDown_Click(object sender, EventArgs e)
		{
			MoveUp(sender, new ButtonUpClickedEventArgs(Step, DataAccess.MoveDirection.Down));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MoveUp(object sender, ButtonUpClickedEventArgs e)
		{
			if (UserControlButtonUpClicked != null)
			{
				UserControlButtonUpClicked(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboStaffAssignment_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboStaffAssignment.SelectedIndex == 0)
			{
				cboStaffAssignID.Enabled = true;
			}
			else
			{
				cboStaffAssignID.Text = string.Empty;
				cboStaffAssignID.Enabled = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboStaffAssignID_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboStaffAssignID.SelectedIndex == 0)
			{
				cboStaffAssignment.Enabled = true;
			}
			else
			{
				cboStaffAssignment.Text = string.Empty;
				cboStaffAssignment.Enabled = false;
			}
		}

		#region Custom Events

		public class ButtonUpClickedEventArgs : EventArgs
		{
			public readonly FlowStep Step;
			public readonly DataAccess.MoveDirection Direction;

			public ButtonUpClickedEventArgs(FlowStep step, DataAccess.MoveDirection direction)
			{
				Step = step;
				Direction = direction;
			}
		}

		public event EventHandler<ButtonUpClickedEventArgs> UserControlButtonUpClicked;
		private void OnUserControlButtonUpClicked(ButtonUpClickedEventArgs e)
		{
			if (UserControlButtonUpClicked != null)
			{
				UserControlButtonUpClicked(this, e);
			}
		}

		public class DeleteFlowStepEventArgs : EventArgs
		{
			public readonly FlowStep Step;

			public DeleteFlowStepEventArgs(FlowStep step)
			{
				Step = step;
			}
		}

		public event EventHandler<DeleteFlowStepEventArgs> UserControlDeleteClicked;
		private void OnUserControlDeleteClicked(DeleteFlowStepEventArgs e)
		{
			if (UserControlDeleteClicked != null)
			{
				UserControlDeleteClicked(this, e);
			}
		}

		public class UpdateFlowStepEventArgs : EventArgs
		{
			public readonly FlowStep Step;

			public UpdateFlowStepEventArgs(FlowStep step)
			{
				Step = step;
			}
		}

		public event EventHandler<UpdateFlowStepEventArgs> UserControlUpdateClicked;
		private void OnUserControlUpdateClicked(UpdateFlowStepEventArgs e)
		{
			if (UserControlUpdateClicked != null)
			{
				UserControlUpdateClicked(this, e);
			}
		}

		#endregion
	}
}
