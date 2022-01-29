using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommentAuditTracker
{
    public partial class AgentControl : UserControl
    {
        /// <summary>
        /// Returns true if the user has made any changes to the control.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                if (IsNew) return true; //new agent always has changes
                if (FullNameBox.Text != Agent.FullName) return true;
                if (ActiveInactiveButton.SelectedValue.ToBool() != Agent.Active) return true;
                if (AuditPercentageBox.Value != Agent.AuditPercentage) return true;
                if (this.UtIdsForm.HasChanges) return true;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the agent has never been committed to the database
        /// </summary>
        public bool IsNew { get { return Agent.AgentId == null; } }
        /// <summary>
        /// The backing object behind this control
        /// </summary>
        public Agent Agent { get; private set; }
        /// <summary>
        /// The form used to manage UT IDs within this control.
        /// </summary>
        public ManageUtIdsForm UtIdsForm { get; private set; }
        /// <summary>
        /// If no agent is supplied, this control is treated as a new agent.
        /// </summary>
        public AgentControl(Agent agent = null)
        {
            InitializeComponent();
            if (agent == null)
                agent = new Agent() { Active = true, AuditPercentage = 7.5m };
            SetAgent(agent);
            SetColor();
            TriggerStateChanged();
        }

        /// <summary>
        /// Load and display all relevant Agent data within this control.
        /// </summary>
        private void SetAgent(Agent agent)
        {
            this.Agent = agent;
            this.UtIdsForm = new ManageUtIdsForm(agent);
            this.FullNameBox.Text = agent.FullName;
            this.ActiveInactiveButton.SelectedValue = agent.Active ? ActiveInactive.Active : ActiveInactive.Inactive;
            this.AuditPercentageBox.Value = agent.AuditPercentage;
            SetManageButtonText(agent.UtIds.Count);
        }

        /// <summary>
        /// A message explaining any currently invalid data.
        /// </summary>
        public string ErrorMessage { get; private set; }
        private ToolTip errorToolTip = new ToolTip();
        /// <summary>
        /// Updates the background color of this control depending on its state (blue for changes made, green for a new agent, normal background for no changes).
        /// </summary>
        private void SetColor()
        {
            if (ErrorMessage != null)
            {
                this.BackColor = Color.LightPink;
                errorToolTip.SetToolTip(this, ErrorMessage);
                foreach (Control control in this.Controls)
                    errorToolTip.SetToolTip(control, ErrorMessage);
                ErrorMessage = null; //reset error message so any change reverts the color of the control
            }
            else
            {
                errorToolTip.RemoveAll();
                if (IsNew)
                    this.BackColor = Color.LightGreen;
                else if (HasChanges)
                    this.BackColor = Color.LightBlue;
                else
                    this.BackColor = SystemColors.Control;
            }
        }


        private void FullNameBox_TextChanged(object sender, EventArgs e)
        {
            UtIdsForm.SetAgentName(FullNameBox.Text);
            SetColor();
            TriggerStateChanged();
        }

        private void ActiveInactiveButton_Cycle(object sender)
        {
            SetColor();
            TriggerStateChanged();
        }

        private void AuditPercentageBox_ValueChanged(object sender, EventArgs e)
        {
            SetColor();
            TriggerStateChanged();
        }

        public delegate void StateChanged(AgentControl sender);
        public event StateChanged OnStateChanged;
        /// <summary>
        /// Let any consumers know that data may have changed on the control.
        /// </summary>
        private void TriggerStateChanged()
        {
            if (OnStateChanged != null)
                OnStateChanged(this);
        }

        /// <summary>
        /// Persist all changes to the database if possible.
        /// </summary>
        public void Save()
        {
            Agent updated = new Agent();
            updated.AgentId = Agent.AgentId;
            updated.FullName = FullNameBox.Text;
            updated.AuditPercentage = AuditPercentageBox.Value;
            updated.Active = ActiveInactiveButton.SelectedValue.ToBool();
            updated.UtIds = UtIdsForm.GetFormattedIds();
            string errorMessage = updated.ValidateChanges();
            if (errorMessage == null)
            {
                updated.UpdateDatabase();
                SetAgent(updated);
            }
            else
            {
                this.ErrorMessage = errorMessage;
            }
            SetColor();
            TriggerStateChanged();
        }

        private void ManageUtIdsButton_Click(object sender, EventArgs e)
        {
            UtIdsForm.ShowDialog();
            SetManageButtonText(UtIdsForm.GetFormattedIds().Count);
        }

        private void SetManageButtonText(int count)
        {
            ManageUtIdsButton.Text = "Manage UT IDs (" + count + ")";
            SetColor();
            TriggerStateChanged();
        }

        private void AuditPercentageBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AuditPercentageBox.Text))
                AuditPercentageBox.Text = AuditPercentageBox.Value.ToString();
        }
    }
}
