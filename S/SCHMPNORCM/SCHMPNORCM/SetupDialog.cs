using System;
using System.Windows.Forms;
using Q;

namespace SCHMPNORCM
{
	partial class SetupDialog : FormBase
	{
		private SetupDetails _details;
		private SchoolFile _schoolFile;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required for the Windows Forms Designer,
		/// but it will not work with the script.
		/// </summary>
		public SetupDialog()
		{
			InitializeComponent();
		}

		public SetupDialog(SchoolFile schoolFile)
		{
			InitializeComponent();
			_details = new SetupDetails();
			//TODO: Bind _details to the form fields.
			_schoolFile = schoolFile;
			SetRecordCount();
		}

		#region Event Handlers
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnDeleteFile_Click(object sender, EventArgs e)
		{
			if (_schoolFile.Delete())
			{
				MessageBox.Show("File Deleted.");
			}
			else
			{
				MessageBox.Show("File has already been Deleted.");
			}
			SetRecordCount();
		}

		private void btnProductionUpdate_Click(object sender, EventArgs e)
		{
			ReturnToScript(SetupDetails.Process.UpdateProductionRegion);
		}

		private void btnSaveInfo_Click(object sender, EventArgs e)
		{
			if (!IsValid()) { return; }

			_schoolFile.AddSchool(_details);

			radSerialMpn.Checked = false;
			radCommonline.Checked = false;
			radClearinghouse.Checked = false;
			radNslds.Checked = false;
			txtSchoolCode.Text = "";
			dtpEffectiveDate.Value = DateTime.Now;
			radStffrd.Checked = false;
			radPlus.Checked = false;
			chkClApp.Checked = false;
			chkClChange.Checked = false;
			chkClDisbursementRoster.Checked = false;
			chkModificationResponse.Checked = false;
			chkHoldAllDisb.Checked = false;
			chkServiceBureau.Checked = false;
			chkElmres.Checked = false;
			
			SetRecordCount();
		}

		private void btnTestUpdate_Click(object sender, EventArgs e)
		{
			ReturnToScript(SetupDetails.Process.UpdateTestRegion);
		}

		private void chkServiceBureau_CheckedChanged(object sender, EventArgs e)
		{
			chkElmres.Enabled = chkServiceBureau.Checked;
		}

		private void radCommonline_CheckedChanged(object sender, EventArgs e)
		{
			chkClDisbursementRoster.Enabled = radCommonline.Checked;
			chkClApp.Enabled = radCommonline.Checked;
			chkHoldAllDisb.Enabled = radCommonline.Checked;
			chkClChange.Enabled = radCommonline.Checked;
			chkModificationResponse.Enabled = radCommonline.Checked;
			chkServiceBureau.Enabled = radCommonline.Checked;
		}

		private void radSerialMpn_CheckedChanged(object sender, EventArgs e)
		{
			radStffrd.Enabled = radSerialMpn.Checked;
			radPlus.Enabled = radSerialMpn.Checked;
		}
		#endregion Event Handlers

		private bool IsValid()
		{
			if (txtSchoolCode.TextLength != 8)
			{
				MessageBox.Show("The School Code text box must have 8 characters in it.");
				txtSchoolCode.Focus();
				return false;
			}
			if (!radCommonline.Checked && !radSerialMpn.Checked && !radClearinghouse.Checked && !radNslds.Checked)
			{
				MessageBox.Show("You must choose how to setup the school.");
				grpSchoolSetTo.Focus();
				return false;
			}
			if (radCommonline.Checked && !chkClApp.Checked && !chkClChange.Checked && !chkClDisbursementRoster.Checked && !chkModificationResponse.Checked && !chkHoldAllDisb.Checked && !chkServiceBureau.Checked)
			{
				MessageBox.Show("One of the Required Options is not selected.");
				grpRequiredOptions.Focus();
				return false;
			}
			if (radSerialMpn.Checked && !radStffrd.Checked && !radPlus.Checked)
			{
				MessageBox.Show("If you have chosen to set up the school as a serial MPN then you must define a loan program.");
				grpRequiredOptions.Focus();
				return false;
			}
			return true;
		}//IsValid()

		private void ReturnToScript(SetupDetails.Process processingOption)
		{
			if (_schoolFile.Exists)
			{
				_details.ProcessingOption = processingOption;
				DialogResult = DialogResult.OK;
			}
			else
			{
				MessageBox.Show("The File does not exist. Click on Save Information to create a file.");
			}
		}//ReturnToScript()

		private void SetRecordCount()
		{
			lblRecordsInFile.Text = _schoolFile.RecordCount.ToString();
		}//SetRecordCount()
	}//class
}//namespace
