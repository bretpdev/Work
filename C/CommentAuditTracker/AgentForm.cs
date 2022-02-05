using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CommentAuditTracker
{
    public partial class AgentForm : Form
    {
        Panel loadingPanel;
        Panel noResultsPanel;
        public AgentForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            loadingPanel = new Panel();
            ResultsGroup.Controls.Add(loadingPanel);
            noResultsPanel = new Panel();
            ResultsGroup.Controls.Add(noResultsPanel);

            loadingPanel.Location = noResultsPanel.Location = AgentsPanel.Location;
            loadingPanel.Size = noResultsPanel.Size = AgentsPanel.Size;
            loadingPanel.Anchor = noResultsPanel.Anchor = AgentsPanel.Anchor;

            loadingPanel.BringToFront();
            loadingPanel.Hide();
            noResultsPanel.BringToFront();
            noResultsPanel.Hide();

            var loadingPicture = new PictureBox() { Image = Properties.Resources.loading_gif, SizeMode = PictureBoxSizeMode.CenterImage, Dock = DockStyle.Fill };
            loadingPanel.Controls.Add(loadingPicture);

            var label = new Label();
            label.Font = new Font("Arial", 16);
            label.Text = "No Results Found";
            label.AutoSize = true;
            noResultsPanel.Controls.Add(label);
        }

        private Thread currentSearchThread = null;
        /// <summary>
        /// Search the database with the given form parameters.
        /// </summary>
        private void Search()
        {
            if (currentSearchThread != null)
                currentSearchThread.Abort();
            currentSearchThread = new Thread(() =>
            {
                bool? active = null;
                if (ActiveInactiveFilterBox.SelectedValue == ActiveInactiveAll.Active)
                    active = true;
                else if (ActiveInactiveFilterBox.SelectedValue == ActiveInactiveAll.Inactive)
                    active = false;
                var results = Agent.SearchAgents(FullNameFilterBox.Text, UtIdFilterBox.Text, active);
                lock (this)
                {
                    DateTime start = DateTime.Now;
                    Invoke(() =>
                    {
                        loadingPanel.Show();
                        AgentsPanel.Controls.Clear();
                    });
                    foreach (var agent in results)
                    {
                        var agentControl = new AgentControl(agent);
                        Invoke(() => AddAgentControl(agentControl));
                    }
                    while ((DateTime.Now - start).TotalSeconds < 0.5)
                        Thread.Sleep(10);
                    Invoke(() =>
                    {
                        if (results.Any())
                            noResultsPanel.Hide();
                        else
                            noResultsPanel.Show();
                        loadingPanel.Hide();
                    });
                }
            });
            currentSearchThread.Start();
        }

        /// <summary>
        /// Simpler syntax for invocation.
        /// </summary>
        private void Invoke(Action a)
        {
            if (base.InvokeRequired)
                base.Invoke(a);
            else
                a();
        }

        private void Trigger_Search(object sender, EventArgs e)
        {
            Search();
        }

        private void ActiveInactiveFilterBox_Cycle(object sender)
        {
            Search();
        }

        private void NewAgentButton_Click(object sender, EventArgs e)
        {
            AddAgentControl(new AgentControl());
        }

        /// <summary>
        /// Add the given AgentControl to the agents panel with proper docking, etc.
        /// </summary>
        private void AddAgentControl(AgentControl ac)
        {
            ac.OnStateChanged += AgentStateChanged;
            ac.Width = AgentsPanel.Width;
            AgentsPanel.Controls.Add(ac);
            if (!ac.IsNew) //leave new controls alone so they appear at the top for easy editing
                AgentsPanel.Controls.SetChildIndex(ac, 0); //dockstyle top reverses their order
            ac.Dock = DockStyle.Top;

            SetButtons();
        }

        private void AgentStateChanged(AgentControl sender)
        {
            SetButtons();
        }

        /// <summary>
        /// Enable/disable Save and Cancel buttons where applicable
        /// </summary>
        private void SetButtons()
        {
            bool anyChanges = AgentsPanel.Controls.Cast<AgentControl>().Any(o => o.HasChanges);

            CancelButton.Enabled = SaveButton.Enabled = anyChanges;
            FilterBox.Enabled = !anyChanges; //no searching while changes are pending
            AdvancedMenuButton.Enabled = !anyChanges; //no deleting while changes are pending
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to abandon all changes you've made?"))
            {
                Search();
            }
            SetButtons();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to save these changes?"))
            {
                foreach (AgentControl ac in AgentsPanel.Controls.Cast<AgentControl>())
                    if (ac.HasChanges)
                        ac.Save();
            }
        }

        private void AgentForm_Shown(object sender, EventArgs e)
        {
            Search(); //perform a blank search immediately to load all agents
        }

        private void DeleteAgentsMenuButton_Click(object sender, EventArgs e)
        {
            new DeleteAgentForm().ShowDialog();
            Search(); //refresh if they deleted an entity
        }

        private void AgentForm_Load(object sender, EventArgs e)
        {
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
            {
                int height = MainMenu.Height - 10;
                MainMenu.Visible = false;
                Dictionary<Control, AnchorStyles> anchors = new Dictionary<Control, AnchorStyles>();
                foreach (var control in this.Controls.Cast<Control>())
                {
                    anchors[control] = control.Anchor;
                    control.Anchor = AnchorStyles.None;
                    control.Top -= height;
                }
                this.Height -= height;
                foreach (var control in this.Controls.Cast<Control>())
                    control.Anchor = anchors[control];
            }
        }
    }
}
