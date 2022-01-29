using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Q;
using System.Text;
using ACDC;

namespace ACDCFlows
{
	public delegate void ResetControlsEventHandler(object sender, EventArgs e);

	public partial class FlowsUI : Form
	{
		public DataAccess _da;
		public List<SqlUser> _users;
		public DataAccessBase.ConfigurationMode _mode;

		public FlowsUI(DataAccessBase.ConfigurationMode mode, int sqlUserId, IEnumerable<string> userRoles)
		{
			InitializeComponent();
			_da = new DataAccess(mode);
			_mode = mode;
			LoadComboBoxes();
			txtAddFlowID.Focus();
		}

		/// <summary>
		/// Loads the combo boxes for all the controls in all tabs
		/// </summary>
		private void LoadComboBoxes()
		{
			cboFlowToChange.DataSource = null;
			cboFlowIDToChange.DataSource = null;
			cboAddSystem.DataSource = null;
			cboChangeSystem.DataSource = null;
			cboSystem.DataSource = null;
			cboUser.DataSource = null;

			try
			{
				List<FlowIDs> flows = _da.GetFlowIDs(string.Empty);
				flows.Insert(0, new FlowIDs() { FlowID = "", FlowName = "" });
				cboFlowToChange.DataSource = new List<FlowIDs>(flows);
				cboFlowToChange.DisplayMember = "FlowName";
				cboFlowToChange.ValueMember = "FlowID";

				cboFlowIDToChange.DataSource = new List<FlowIDs>(flows);
				cboFlowIDToChange.DisplayMember = "FlowName";
				cboFlowIDToChange.ValueMember = "FlowID";
			}
			catch (Exception ex)
			{
				CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Get Flow ID's", ex);
			}
			try
			{
				List<string> systems = _da.GetSystems();
				systems.Insert(0, string.Empty);
				cboAddSystem.DataSource = new List<string>(systems);
				cboChangeSystem.DataSource = new List<string>(systems);
				cboSystem.DataSource = new List<string>(systems);
			}
			catch (Exception ex)
			{
                CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Get System List", ex);
			}
			try
			{
				_users = _da.GetUsers().OrderBy(p => p.LegalName).ToList();
				_users.RemoveAll(p => p.WindowsUserName.Contains("training"));
				_users.Insert(0, new SqlUser() { LegalName = "", ID = 0 });
				cboUser.DataSource = _users;
				cboUser.DisplayMember = "LegalName";
				cboUser.ValueMember = "ID";

				cboStaffAssignment.DataSource = _users;
				cboStaffAssignment.DisplayMember = "LegalName";
				cboStaffAssignment.ValueMember = "ID";
			}
			catch (Exception ex)
			{
                CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Get List of Users", ex);
			}
        }

        #region Flow Management Tab

        /// <summary>
		/// Validates the data in the fields and adds the new flow to the FLOW_DAT_Flow table in CSYS
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddFlow_Click(object sender, EventArgs e)
		{
			if (AddFlowValidation() && CheckForSpecialCharacters(txtAddFlowID.Text) && CheckForSpecialCharacters(txtAddControlText.Text))
			{
				Flow flowToAdd = new Flow();
				flowToAdd.FlowID = txtAddFlowID.Text;
				flowToAdd.Description = txtAddDescription.Text;
				flowToAdd.ControlDisplayText = txtAddControlText.Text;
				flowToAdd.UserInterfaceDisplayIndicator = cboAddDisplayID.Text;
				flowToAdd.System = cboAddSystem.Text;
				try
				{
					_da.AddFlow(flowToAdd);
				}
				catch (FlowAlreadyExistsInDatabaseException)
				{
					MessageBox.Show("Flow already exists, ACDC Flows");
				}
				catch (Exception ex)
				{
                    CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Error Adding Flow", ex);
				}
				finally
				{
					lblAdded.Text = txtAddFlowID.Text + " Successfully Added";
					lblAdded.Visible = true;
					ClearAddFlow();
					LoadComboBoxes();
				}
			}
		}

		/// <summary>
		/// Sets up the controls for the Change Flow section according to the selected flow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboFlowToChange_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Clear out all the fields
			btnChangeFlow.Enabled = false;
			cboChangeSystem.Enabled = false;
			txtChangeDescription.Enabled = false;
			txtChangeDisplayText.Enabled = false;
			cboChangeSystem.Enabled = false;
			cboChangeDisplayID.Enabled = false;
			cboChangeSystem.SelectedIndex = -1;
			txtChangeDisplayText.Text = string.Empty;
			cboChangeDisplayID.SelectedIndex = -1;
			txtChangeDescription.Text = string.Empty;
			lblChangeOptional.Visible = true;
			lblChanged.Visible = false;

			//reload the fields with the items in the Flow object
			Flow flow = _da.GetFlowData(cboFlowToChange.Text);
			if (flow != null)
			{
				List<SystemInterfaces> interfaces = _da.GetInterfaces(flow.System);
				cboChangeDisplayID.DataSource = interfaces;
				cboChangeDisplayID.DisplayMember = "Interface";
				cboChangeDisplayID.ValueMember = "System";
				lblChangeOptional.Visible = interfaces[interfaces.Count - 1].System == string.Empty;
				cboChangeSystem.Text = flow.System;
				cboChangeDisplayID.Text = flow.UserInterfaceDisplayIndicator;
				flowBindingSource.DataSource = flow;
				btnChangeFlow.Enabled = true;
				txtChangeDescription.Enabled = true;
				txtChangeDisplayText.Enabled = true;
				cboChangeSystem.Enabled = true;
				cboChangeDisplayID.Enabled = true;
			}
		}

		/// <summary>
		/// Updated the flow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChangeFlow_Click(object sender, EventArgs e)
		{
			if (ChangeFlowValidation() && CheckForSpecialCharacters(txtChangeDisplayText.Text))
			{
				Flow flow = new Flow();
				flow.FlowID = cboFlowToChange.Text;
				flow.System = cboChangeSystem.Text;
				flow.ControlDisplayText = txtChangeDisplayText.Text;
				flow.UserInterfaceDisplayIndicator = cboChangeDisplayID.Text;
				flow.Description = txtChangeDescription.Text;

				try
				{
					_da.ChangeFlow(flow);
				}
				catch (Exception ex)
				{
                    CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Error Changing Flow", ex);
					lblChanged.Visible = true;
					lblChanged.Text = cboFlowToChange.Text + " Not Changed";
				}
				finally
				{
					lblChanged.Text = cboFlowToChange.Text + " Successfully Changed";
					lblChanged.Visible = true;
					ClearChangeFlow();
				}
			}
		}

		/// <summary>
		/// Loads the datasource for the User Interface Display ID
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboAddSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboAddDisplayID.Enabled = cboAddSystem.Text != string.Empty;
			List<SystemInterfaces> interfaces = _da.GetInterfaces(cboAddSystem.Text);
			cboAddDisplayID.DataSource = interfaces;
			cboAddDisplayID.DisplayMember = "Interface";
			cboAddDisplayID.ValueMember = "System";
			lblAddOptional.Visible = interfaces[interfaces.Count - 1].System == string.Empty;
		}

		/// <summary>
		/// Enables the User Interface Display ID combo box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboChangeSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboChangeDisplayID.Enabled = cboChangeSystem.Text != string.Empty;
        }

        #endregion

        #region Step Management Tab

        /// <summary>
		/// Gets all the flow information for the flow step chosen and load the panel with the ExistingFlowSteps control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboFlowIDToChange_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearAddStep();

			Flow flow = new Flow();
			flow = _da.GetFlowData(cboFlowIDToChange.Text);

			if (flow != null)
			{
				List<string> staffID = _da.GetStaffCalculationID(flow.System);
				staffID.Insert(0, string.Empty);
				cboStaffAssignID.DataSource = staffID;

				List<string> keys = new List<string>(Enum.GetNames(typeof(NotifyType.Type)));
				keys.Insert(0, string.Empty);
				cboNotificationType.DataSource = keys;

				List<string> dataValid = _da.GetDataValidation(flow.System);
				dataValid.Insert(0, string.Empty);
				cboDataValidation.DataSource = dataValid;

				lblSystem.Text = flow.System;
				lblDescription.Text = flow.Description;

				//Get list of the steps for the flow
				List<FlowStep> steps = new List<FlowStep>();
				steps.AddRange(_da.GetFlowSteps(flow.FlowID));

                cboAccessKey.Items.Clear();
				List<string> accessKey = _da.GetAccessKeys();
				accessKey.Insert(0, string.Empty);
				accessKey.Remove(null);
				cboAccessKey.Items.AddRange(accessKey.ToArray());

                cboStatus.Items.Clear();
				List<string> status = _da.GetStatus();
				status.Insert(0, string.Empty);
				status.Remove(null);
				cboStatus.Items.AddRange(status.ToArray());

				//Create a list of controls to add to the update panel
				List<Control> controls = new List<Control>();
				foreach (FlowStep step in steps)
				{
					ExistingFlowSteps existingFlow = new ExistingFlowSteps(_da, step, keys, staffID, _users, dataValid, accessKey, status, steps.Count);
					existingFlow.UserControlButtonUpClicked += new EventHandler<ExistingFlowSteps.ButtonUpClickedEventArgs>(MovePropertiesIntoAFlowStep);
					existingFlow.UserControlDeleteClicked += new EventHandler<ExistingFlowSteps.DeleteFlowStepEventArgs>(DeleteFlowStep);
					existingFlow.UserControlUpdateClicked += new EventHandler<ExistingFlowSteps.UpdateFlowStepEventArgs>(UpdateFlowStep);
					controls.Add(existingFlow);
				}
				//Add the controls to the panel and set the focus on the panel
				pnlSteps.Controls.AddRange(controls.ToArray());
				pnlSteps.Focus();
			}
		}

		private void MovePropertiesIntoAFlowStep(object sender, ExistingFlowSteps.ButtonUpClickedEventArgs e)
		{
			_da.MoveStep(e.Step, e.Direction);
			cboFlowIDToChange_SelectedIndexChanged(sender, new EventArgs());
		}

		public void DeleteFlowStep(object sender, ExistingFlowSteps.DeleteFlowStepEventArgs e)
		{
			_da.DeleteStep(e.Step);
			cboFlowIDToChange_SelectedIndexChanged(sender, new EventArgs());
		}

		public void UpdateFlowStep(object sender, ExistingFlowSteps.UpdateFlowStepEventArgs e)
		{
			_da.UpdateFlowStep(e.Step);
			cboFlowIDToChange_SelectedIndexChanged(sender, new EventArgs());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddStep_Click(object sender, EventArgs e)
		{
			if (AddFlowStepValidation())
			{
				int FlowStepSequenceNumber = (_da.GetNextFlowSequenceNumber(cboFlowIDToChange.Text)) + 1;

				//add step to end on the flow steps
				List<object> param = new List<object>();
				StringBuilder insertFlowStep = new StringBuilder();
				int placeHolderCount = 6;
				param.Add(cboFlowIDToChange.Text);
				param.Add(FlowStepSequenceNumber);
				param.Add(cboBusinessUnit.Text);
				param.Add(txtDisplayText.Text);
				param.Add(txtDescription.Text);
				param.Add(cboStatus.Text);
				insertFlowStep.Append("EXEC spFLOW_AddStepToFlow @FlowID = {0}, @FlowStepSequenceNumber = {1}, @AccessAlsoBasedOffBusinessUnit = {2}, @ControlDisplayText = {3}, @Description = {4}, @Status = {5}");
				if (cboAccessKey.Text != "")
				{
					insertFlowStep.AppendFormat(", @AccessKey = {0}{1}{2}", "{", placeHolderCount, "}");
					param.Add(cboAccessKey.Text);
					placeHolderCount++;
				}
				if (cboNotificationType.Text != "")
				{
					insertFlowStep.AppendFormat(", @NotificationType = {0}{1}{2}", "{", placeHolderCount, "}");
					param.Add(cboNotificationType.Text);
					placeHolderCount++;
				}
				if (cboStaffAssignment.SelectedItem != null)
				{
					insertFlowStep.AppendFormat(", @StaffAssignment = {0}{1}{2}", "{", placeHolderCount, "}");
					param.Add(((SqlUser)cboStaffAssignment.SelectedItem).ID);
					placeHolderCount++;
				}
				if (!string.IsNullOrEmpty(cboStaffAssignID.Text))
				{
					insertFlowStep.AppendFormat(", @StaffAssignmentCalculationID = {0}{1}{2}", "{", placeHolderCount, "}");
					param.Add(cboStaffAssignID.Text);
					placeHolderCount++;
				}
				if (!string.IsNullOrEmpty(cboDataValidation.Text))
				{
					insertFlowStep.AppendFormat(", @DataValidationID = {0}{1}{2}", "{", placeHolderCount, "}");
					param.Add(cboDataValidation.Text);
					placeHolderCount++;
				}

				try
				{
					_da.AddFlowStep(insertFlowStep.ToString(), param);
					cboFlowIDToChange_SelectedIndexChanged(sender, e);
				}
				catch (Exception ex)
				{
                    CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Error Adding Flow Step", ex);
				}
			}
        }

        #endregion

        #region System Flow Steps Tab

        /// <summary>
		/// Loads the flows available for the chosen system
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Clear out the Flow ID comobox before loading it.
			cboFlowId.Enabled = false;
			cboFlowId.SelectedIndex = -1;
			lblFlowID.Text = string.Empty;
			lblInterfaceText.Text = string.Empty;
			lblDesc.Text = string.Empty;
			lblDisplayText.Text = string.Empty;

			//Get list a FlowIDs according to the system
			List<FlowIDs> flows = _da.GetFlowIDs(cboSystem.Text);
			flows.Insert(0, new FlowIDs() { FlowID = "", FlowName = "", Description = "" });
			if (cboSystem.Text.Trim() != string.Empty)
			{
				foreach (FlowIDs flow in flows)
				{
					flow.FlowName += flow.Description == "" ? "" : " - " + flow.Description;
				}
				cboFlowId.DataSource = flows;
				cboFlowId.DisplayMember = "FlowName";
				cboFlowId.Enabled = true;
			}
		}

		/// <summary>
		/// Gets the flow data for the selected flow and displays the flow steps
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboFlowId_SelectedIndexChanged(object sender, EventArgs e)
		{
			pnlFlows.Controls.Clear();
			try
			{
				//Get the flow information for the selected flow
				FlowIDs flowID = (FlowIDs)cboFlowId.SelectedItem;
				Flow flow = null;
				if (flowID != null)
				{
					flow = _da.GetFlowData(flowID.FlowID);
				}

				if (flow != null)
				{
					flowBindingSource.DataSource = flow;

					//Get list of the steps for the flow
					List<FlowStep> steps = new List<FlowStep>();
					steps.AddRange(_da.GetFlowSteps(flow.FlowID));

					//Create a list of controls to add to the update panel
					List<Control> controls = new List<Control>();
					foreach (FlowStep step in steps)
					{
						Control ctrl = new FlowStepControl(step, _users);

						controls.Add(ctrl);
					}
					//Add the controls to the panel and set the focus on the panel
					pnlFlows.Controls.AddRange(controls.ToArray());
					pnlFlows.Focus();
				}
			}
			catch (Exception ex)
			{
                CommonException.CatchException(_mode != DataAccessBase.ConfigurationMode.Live ? true : false, "ACDC Flows", "Error Retrieving Flow ID's", ex);
			}
        }

        #endregion

        #region Staff Assignment Tab

        /// <summary>
		/// Gets the flow steps assigned to the user
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
		{
			pnlUsers.Controls.Clear();
			SqlUser user = (SqlUser)cboUser.SelectedItem;
			if (user != null)
			{
				List<Control> controls = new List<Control>();
				foreach (FlowStepInfoForUserSearch step in _da.GetUserFlowStep(user.ID))
				{
					Control ctrl = new StaffAssignment(step);
					controls.Add(ctrl);
				}
				pnlUsers.Controls.AddRange(controls.ToArray());
				pnlUsers.Focus();
			}
        }

        #endregion

        #region ChangeTab

        /// <summary>
		/// Finds the selected tab and calls the clear method to reset the fields in the tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabACDCFlows_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (tabACDCFlows.SelectedIndex)
			{
				case 0:
					ClearAddFlow();
					ClearChangeFlow();
					txtAddFlowID.Focus();
					break;
				case 1:
					ClearAddStep();
					cboFlowIDToChange.SelectedIndex = -1;
					cboFlowIDToChange.Focus();
					break;
				case 2:
					ClearSystemFlowStep();
					cboSystem.Focus();
					break;
				case 3:
					ClearUserFlows();
					cboUser.Focus();
					break;
			}
		}

		/// <summary>
		/// Clears all the data from the fields in the Add Flow section of the Flow Management tab
		/// </summary>
		private void ClearAddFlow()
		{
			txtAddFlowID.Text = string.Empty;
			cboAddSystem.SelectedIndex = -1;
			txtAddControlText.Text = string.Empty;
			cboAddDisplayID.SelectedIndex = -1;
			txtAddDescription.Text = string.Empty;
		}

		/// <summary>
		/// Clears all the data from the fields in the Change Flow section of the Flow Management tab
		/// </summary>
		private void ClearChangeFlow()
		{
			cboFlowToChange.SelectedIndex = -1;
			cboChangeSystem.SelectedIndex = -1;
			txtChangeDisplayText.Text = string.Empty;
			cboChangeDisplayID.SelectedIndex = -1;
			txtChangeDescription.Text = string.Empty;
		}

		/// <summary>
		/// Clears all the data from the Step Management tab
		/// </summary>
		private void ClearAddStep()
		{
			lblSystem.Text = string.Empty;
			lblDescription.Text = string.Empty;
			pnlSteps.Controls.Clear();
			cboBusinessUnit.SelectedIndex = -1;
			cboNotificationType.SelectedIndex = -1;
			txtDisplayText.Text = string.Empty;
			txtDescription.Text = string.Empty;
			cboDataValidation.SelectedIndex = -1;
			cboAccessKey.SelectedIndex = -1;
			cboStatus.Text = string.Empty;
			cboStaffAssignment.SelectedIndex = cboStaffAssignment.SelectedIndex == 0 ? 0 : -1;
			cboStaffAssignID.SelectedIndex = cboStaffAssignID.SelectedIndex == 0 ? 0 : -1;
		}

		/// <summary>
		/// Clears all the data from the System Flow Steps tab
		/// </summary>
		private void ClearSystemFlowStep()
		{
			cboSystem.SelectedIndex = -1;
			pnlFlows.Controls.Clear();
		}

		/// <summary>
		/// Clears all the data from the Staff Assignment tab
		/// </summary>
		private void ClearUserFlows()
		{
			cboUser.SelectedIndex = -1;
			pnlUsers.Controls.Clear();
		}

		#endregion

		#region Validation

		/// <summary>
		/// Check that all controls have text to add a flow
		/// </summary>
		/// <returns>True if all text provided / False if a field is missing</returns>
		public bool AddFlowValidation()
		{
			List<string> missingField = new List<string>();
			if (txtAddFlowID.Text.Trim() == string.Empty)
			{
				missingField.Add("Flow ID");
			}
			if (cboAddSystem.Text == string.Empty || cboAddSystem.SelectedIndex == -1)
			{
				missingField.Add("System");
			}
			if (txtAddControlText.Text.Trim() == string.Empty)
			{
				missingField.Add("Control Display Text");
			}
			if (txtAddDescription.Text.Trim() == string.Empty)
			{
				missingField.Add("Description");
			}
			if (missingField.Count > 0)
			{
				string message = string.Empty;
				foreach (string s in missingField)
				{
					message += s + "\r\n";
				}
				MessageBox.Show("The following fields are empty\r\n\r\n" + message + "\r\nPlease provide the missing fields and try again", "Missing fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Check that all controls have text to change a flow
		/// </summary>
		/// <returns></returns>
		public bool ChangeFlowValidation()
		{
			List<string> missingField = new List<string>();
			if (cboChangeSystem.Text.Trim() == string.Empty || cboChangeSystem.SelectedIndex == -1)
			{
				missingField.Add("System");
			}
			if (txtChangeDisplayText.Text.Trim() == string.Empty)
			{
				missingField.Add("Control Display Text");
			}
			if (txtChangeDescription.Text.Trim() == string.Empty)
			{
				missingField.Add("Description");
			}
			if (missingField.Count > 0)
			{
				string message = string.Empty;
				foreach (string s in missingField)
				{
					message += s + "\r\n";
				}
				MessageBox.Show("The following fields are empty\r\n\r\n" + message + "\r\nPlease provide the missing fields and try again", "Missing fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Check that all controls have text to add a flow step
		/// </summary>
		/// <returns></returns>
		private bool AddFlowStepValidation()
		{
			List<string> requiredField = new List<string>();
			List<string> optionalField = new List<string>();
			if (cboAccessKey.Text.Trim() == string.Empty || cboAccessKey.SelectedIndex == -1)
			{
				optionalField.Add("Access Key");
			}
			if (cboBusinessUnit.Text.Trim() == string.Empty || cboBusinessUnit.SelectedIndex == -1)
			{
				requiredField.Add("Business Unit Access Only");
			}
			if (cboDataValidation.Text.Trim() == string.Empty || cboDataValidation.SelectedIndex == -1)
			{
				optionalField.Add("Data Validation ID");
			}
			if (txtDescription.Text.Trim() == string.Empty)
			{
				optionalField.Add("Description");
			}
			if (txtDisplayText.Text.Trim() == string.Empty)
			{
				optionalField.Add("Display Text");
			}
			if (cboStaffAssignment.Text.Trim() == string.Empty || cboStaffAssignment.SelectedIndex == -1)
			{
				if (cboStaffAssignID.Text.Trim() == string.Empty || cboStaffAssignID.SelectedIndex == -1)
				{
					optionalField.Add("Staff Assignment");
				}
			}
			if (cboStaffAssignID.Text.Trim() == string.Empty || cboStaffAssignID.SelectedIndex == -1)
			{
				if (cboStaffAssignment.Text.Trim() == string.Empty || cboStaffAssignment.SelectedIndex == -1)
				{
					optionalField.Add("Staff Assignment ID");
				}
			}
			if (cboStatus.Text.Trim() == string.Empty || cboStatus.SelectedIndex == -1)
			{
				requiredField.Add("Status");
			}
			if (requiredField.Count > 0)
			{
				string message = string.Empty;
				foreach (string s in requiredField)
				{
					message += s + "\r\n";
				}
				MessageBox.Show("The following fields are empty\r\n\r\n" + message + "\r\nPlease provide the required missing fields and try again", "Missing fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			if (optionalField.Count > 0)
			{
				string message = string.Empty;
				foreach (string s in optionalField)
				{
					message += s + "\r\n";
				}
				if (MessageBox.Show("The following are optional fields\r\n\r\n" + message + "\r\n\r\nAre you sure you want to leave them blank?", "Missing fields", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return true;
		}

		private bool CheckForSpecialCharacters(string text)
		{
			List<string> characters = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "=", "+", "{", "}", "[", "]", @"\", "|", "<", ">", ",", ".", ":", ";", "`", "~" };

			bool isCorrect = true;
			string message = "You can not use any of the following characters\r\n\r\n";
			foreach (string str in characters)
			{
				if (text.Contains(str))
				{
					message += str + " ";
					isCorrect = false;
				}
			}

			if (!isCorrect)
			{
				message += "\r\n\r\nPlease remove the characters and try again";
				MessageBox.Show(message, "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return isCorrect;
		}

		#endregion

		#region FocusPanels

		/// <summary>
		/// Sets focus on Panel if clicked on scroll bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlSteps_Click(object sender, EventArgs e)
		{
			pnlSteps.Focus();
		}

		/// <summary>
		/// Sets focus on Panel if clicked on scroll bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlFlows_Click(object sender, EventArgs e)
		{
			pnlFlows.Focus();
		}

		/// <summary>
		/// Sets focus on Panel if clicked on scroll bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlUsers_Click(object sender, EventArgs e)
		{
			pnlUsers.Focus();
		}

		#endregion

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

	}
}
