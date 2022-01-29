using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DatabasePermissions
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			cmbDatabase.DataSource = DataAccess.DatabaseNames;
			cmbEntityType.DataSource = DataAccess.EntityTypes;
            cmbActiveDirectoryGroup.DataSource  = DataAccess.ActiveDirectoryGroups;
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			//Populate the entity list.
			IEnumerable<Entity> entities = DataAccess.GetEntities(cmbDatabase.Text, cmbEntityType.Text, cmbActiveDirectoryGroup.Text);
			if (!string.IsNullOrEmpty(txtFilter.Text)) { entities = entities.Where(p => Regex.IsMatch(p.Name, txtFilter.Text)); }
			pnlEntities.Controls.Clear();
			foreach (Entity entity in entities) { pnlEntities.Controls.Add(new EntityDisplay(entity)); }

			//Set the top check boxes to reflect the list contents.
			SetCheckState(chkInsert, entities.Count(p => p.Insert), entities.Count());
			SetCheckState(chkUpdate, entities.Count(p => p.Update), entities.Count());
			SetCheckState(chkDelete, entities.Count(p => p.Delete), entities.Count());
			SetCheckState(chkSelect, entities.Count(p => p.Select), entities.Count());
			SetCheckState(chkExecute, entities.Count(p => p.Execute), entities.Count());
		}

		private void SetCheckState(CheckBox checkBox, int checkedCount, int entityCount)
		{
			if (checkedCount == 0)
			{
				checkBox.CheckState = CheckState.Unchecked;
			}
			else if (checkedCount == entityCount)
			{
				checkBox.CheckState = CheckState.Checked;
			}
			else
			{
				checkBox.CheckState = CheckState.Indeterminate;
			}
		}

		private void chkInsert_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (chkInsert.CheckState == CheckState.Indeterminate) { return; }
			foreach (Control control in pnlEntities.Controls)
			{
				(control as EntityDisplay).Insert = (chkInsert.CheckState == CheckState.Checked);
			}
		}

		private void chkUpdate_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (chkUpdate.CheckState == CheckState.Indeterminate) { return; }
			foreach (Control control in pnlEntities.Controls)
			{
				(control as EntityDisplay).Update = (chkUpdate.CheckState == CheckState.Checked);
			}
		}

		private void chkDelete_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (chkDelete.CheckState == CheckState.Indeterminate) { return; }
			foreach (Control control in pnlEntities.Controls)
			{
				(control as EntityDisplay).Delete = (chkDelete.CheckState == CheckState.Checked);
			}
		}

		private void chkSelect_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (chkSelect.CheckState == CheckState.Indeterminate) { return; }
			foreach (Control control in pnlEntities.Controls)
			{
				(control as EntityDisplay).Select = (chkSelect.CheckState == CheckState.Checked);
			}
		}

		private void chkExecute_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (chkExecute.CheckState == CheckState.Indeterminate) { return; }
			foreach (Control control in pnlEntities.Controls)
			{
				(control as EntityDisplay).Execute = (chkExecute.CheckState == CheckState.Checked);
			}
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			List<Entity> entities = new List<Entity>();
			foreach (Control control in pnlEntities.Controls) { entities.Add((control as EntityDisplay).Entity); }
			//TODO: Write permissions back to the database.
		}
	}//class
}//namespace
