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

namespace CommentAuditTracker
{
    public partial class DeleteAgentForm : Form
    {
        public DeleteAgentForm()
        {
            InitializeComponent();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            FindStatusLabel.Visible = true;
            var existingAgent = Agent.GetByFullName(AgentNameBox.Text);
            if (existingAgent == null)
            {
                FindStatusLabel.Text = "No Agent Found";
            }
            else
            {
                FindStatusLabel.Text = "Agent Found";
                DeleteButton.Visible = true;
                AgentNameBox.Enabled = false;
                FindButton.Enabled = false;
                pendingDeletion = existingAgent;
            }
        }

        Agent pendingDeletion = null;

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Do you really want to permanently delete this agent?"))
            {
                pendingDeletion.Delete();
            }
            this.Close();
        }
    }
}
