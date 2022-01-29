using System;
using System.Windows.Forms;
using Q;

namespace CORNTSTQUE
{
	partial class MainForm : FormBase
	{
		private ProcessingDetails _details;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required for the Windows Forms Designer,
		/// but it will not work with the script.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		public MainForm(ProcessingDetails details)
		{
			InitializeComponent();
			cmbUserType.SelectedIndex = 0;
			_details = details;
		}

		#region Convenience Handlers
		private void btnAddQueue_Click(object sender, EventArgs e)
		{
			AddQueue();
		}

		private void btnAddUser_Click(object sender, EventArgs e)
		{
			AddUser();
		}

		private void btnRemoveQueue_Click(object sender, EventArgs e)
		{
			dgvQueues.Rows.Remove(dgvQueues.SelectedRows[0]);
		}

		private void btnRemoveUser_Click(object sender, EventArgs e)
		{
			dgvUsers.Rows.Remove(dgvUsers.SelectedRows[0]);
		}

		private void cmbUserType_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { AddUser(); }
		}

		private void radAdd_CheckedChanged(object sender, EventArgs e)
		{
			if (radAdd.Checked) { _details.SelectedAction = ProcessingDetails.Action.Add; }
		}

		private void radRemove_CheckedChanged(object sender, EventArgs e)
		{
			if (radRemove.Checked) { _details.SelectedAction = ProcessingDetails.Action.Remove; }
		}

		private void txtQueue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtSubQueue.Focus();
				txtSubQueue.SelectAll();
			}
		}

		private void txtSubQueue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { AddQueue(); }
		}

		private void txtUserId_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { AddUser(); }
		}
		#endregion Convenience Handlers

		//Binding the DataGridViews to Lists proved to be too fickle,
		//so the Lists aren't populated until the user hits this button.
		private void btnProcess_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvUsers.Rows)
			{
				string userId = row.Cells["UserId"].Value.ToString();
				string type = row.Cells["Type"].Value.ToString();
				_details.Users.Add(new UserDetail(userId, type));
			}
			foreach (DataGridViewRow row in dgvQueues.Rows)
			{
				string queue = row.Cells["Queue"].Value.ToString();
				string subQueue = row.Cells["SubQueue"].Value.ToString();
				_details.Queues.Add(new QueueDetail(queue, subQueue));
			}
			DialogResult = DialogResult.OK;
		}

		private void AddQueue()
		{
			dgvQueues.Rows.Add(txtQueue.Text.ToUpper(), txtSubQueue.Text.ToUpper());
			txtQueue.Focus();
			txtQueue.SelectAll();
		}//AddQueue()

		private void AddUser()
		{
			dgvUsers.Rows.Add(txtUserId.Text.ToUpper(), cmbUserType.Text);
			txtUserId.Text = "UT00";
			txtUserId.Focus();
			txtUserId.Select(txtUserId.Text.Length, 0);
		}//AddUser()
	}//class
}//namespace
