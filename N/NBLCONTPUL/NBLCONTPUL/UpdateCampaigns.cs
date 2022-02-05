using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace NBLCONTPUL
{
	public partial class UpdateCampaigns : Form
	{
		public string CallCampaign { get; set; }
		public string Group { get; set; }
		public string InitialDescription { get; set; }
		public bool DescriptionChanged { get; set; }
		public bool InitialActive { get; set; }
		public bool ActiveChanged { get; set; }
		public bool InitialInvalidate { get; set; }
		public bool InvalidateChanged { get; set; }
		public bool InitialCallType { get; set; }
		public bool CallTypeChanged { get; set; }

		public UpdateCampaigns()
		{
			InitializeComponent();
			GroupComboBox.Items.AddRange(DataAccess.GetGroups().ToArray());

			ClearFields();
		}

		/// <summary>
		/// Sets all form fields to their default values.
		/// </summary>
		private void ClearFields()
		{
			CallCampaignComboBox.Items.Clear();
			CallCampaignComboBox.SelectedItem = null;
			CampaignDescriptionTextBox.Clear();

			CallCampaignComboBox.Items.Add(string.Empty);
			CallCampaignComboBox.Enabled = false;

			CampaignDescriptionTextBox.SelectedText = string.Empty;
			CampaignDescriptionTextBox.Enabled = false;

			CallTypeInbound.Enabled = false;
			CallTypeOutbound.Enabled = false;

			ActiveCheckBox.Checked = false;
			ActiveCheckBox.Enabled = false;

			InvalidateCheckBox.Checked = false;
			InvalidateCheckBox.Enabled = false;
		}
		
		/// <summary>
		/// Set all other fields to enabled once a group is selected.  Get the CallCampaign list based on the group.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Group = GroupComboBox.SelectedItem.ToString();
			ClearFields();

			if (Group == "Cornerstone")
				CallCampaignComboBox.Items.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.CornerStone).ToArray());
			else if (Group == "Onelink")
				CallCampaignComboBox.Items.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.OneLink).ToArray());
			else if (Group == "UHEAA")
				CallCampaignComboBox.Items.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.Uheaa).ToArray());
			else if (Group == "Exclusion")
				CallCampaignComboBox.Items.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.Exclusion).ToArray());
			
			CallCampaignComboBox.Enabled = true;
			CallTypeInbound.Enabled = true;
			CallTypeOutbound.Enabled = true;
			CampaignDescriptionTextBox.Enabled = true;
			ActiveCheckBox.Enabled = true;
			InvalidateCheckBox.Enabled = true;
		}

		/// <summary>
		/// Gets all current data for the CallCampaign/Group Combo.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CallCampaignComboBox_Leave(object sender, EventArgs e)
		{
			CallCampaign = CallCampaignComboBox.Text;
			ActiveCheckBox.Checked = DataAccess.GetActiveFlag(CallCampaign, Group);
			InvalidateCheckBox.Checked = DataAccess.GetInvalidateFlag(CallCampaign, Group);
			CallTypeInbound.Checked = DataAccess.GetCallType(CallCampaign, Group);
			CallTypeOutbound.Checked = !CallTypeInbound.Checked;
			InitialDescription = CampaignDescriptionTextBox.Text = DataAccess.GetCampaignDescription(CallCampaign, Group);
			InitialCallType = CallTypeInbound.Checked;
			InitialActive = ActiveCheckBox.Checked;
			InitialInvalidate = InvalidateCheckBox.Checked;

			ActiveChanged = false;
			InvalidateChanged = false;
			CallTypeChanged = false;
			DescriptionChanged = false;
		}

		private void CampaignDescriptionTextBox_TextChanged(object sender, EventArgs e)
		{
            DescriptionChanged = InitialDescription == CampaignDescriptionTextBox.Text ? false : true;
		}

		private void ActiveCheckBox_CheckedChanged(object sender, EventArgs e)
		{
            ActiveChanged = InitialActive == ActiveCheckBox.Checked ? false : true;
		}

		private void InvalidateCheckBox_CheckedChanged(object sender, EventArgs e)
		{
            InvalidateChanged = InitialInvalidate == InvalidateCheckBox.Checked ? false : true;
		}

		private void CallTypeInbound_CheckedChanged(object sender, EventArgs e)
		{
			CallTypeChanged = InitialCallType == CallTypeInbound.Checked ? false : true;
			CallTypeOutbound.Checked = CallTypeInbound.Checked == true ? false : true;
		}

		private void CallTypeOutbound_CheckedChanged(object sender, EventArgs e)
		{
			CallTypeChanged = InitialCallType == CallTypeInbound.Checked ? false : true;
			CallTypeInbound.Checked = CallTypeOutbound.Checked == true ? false : true;
		}

		/// <summary>
		/// Adds new records to the database or changes an existing record based on the changes made on the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContinueButton_Click(object sender, EventArgs e)
		{
			List<string> campaigns = new List<string>();
			if (Group == "Cornerstone")
				campaigns.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.CornerStone).ToArray());
			else if (Group == "Onelink")
				campaigns.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.OneLink).ToArray());
			else if (Group == "UHEAA")
				campaigns.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.Uheaa).ToArray());
			else if (Group == "Exclusion")
				campaigns.AddRange(DataAccess.GetCallCampaigns(DataAccess.Region.Exclusion).ToArray());

			if (campaigns.Contains(CallCampaignComboBox.Text)) //Modify mode
			{
				if (Group.IsNullOrEmpty() || CallCampaign.IsNullOrEmpty())
				{
					MessageBox.Show("Group and Call Campaign must be populated in order to make updates.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else if (DescriptionChanged || ActiveChanged || InvalidateChanged || CallTypeChanged) //Something is new
				{
					DataAccess.UpdateCallCampaign(Group, CallCampaign, CampaignDescriptionTextBox.Text.ToUpper(), CallTypeInbound.Checked, ActiveCheckBox.Checked, InvalidateCheckBox.Checked);
				}
				else //Nothing changed
					MessageBox.Show("No data was changed for the original values.  Update not performed.", "No Changes Made", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else//Add mode
			{
				DataAccess.AddCampaign(Group, CallCampaign, CampaignDescriptionTextBox.Text.ToUpper(), CallTypeOutbound.Checked, ActiveCheckBox.Checked, InvalidateCheckBox.Checked);
			}
			//reset initial values for multiple modifications
			InitialDescription = CampaignDescriptionTextBox.Text;
			InitialCallType = CallTypeInbound.Checked;
			InitialActive = ActiveCheckBox.Checked;
			InitialInvalidate = InvalidateCheckBox.Checked;
		}
	}
}
