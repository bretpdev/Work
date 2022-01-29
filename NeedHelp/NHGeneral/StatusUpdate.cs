using System;
using System.Windows.Forms;
using SubSystemShared;
using System.Collections.Generic;
using System.Drawing;

namespace NHGeneral
{
    partial class StatusUpdate : Form
    {
        Ticket _activeTicket;

        public StatusUpdate(List<FlowStep> steps, Ticket activeTicket)
        {
            InitializeComponent();

            _activeTicket = activeTicket;
            int x = 0;
            int y = 0;

            foreach (FlowStep step in steps)
            {
                LinkLabel stepLabel = new LinkLabel();
                stepLabel.Text = step.Status;
                stepLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(stepLabel_Click);
                stepLabel.Location = new Point(x, y);
                stepLabel.LinkVisited = false;
                stepLabel.LinkColor = Color.Wheat;
                pnlStatus.Controls.Add(stepLabel);
                y += 23;
            }
        }

        public void stepLabel_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lnk = (LinkLabel)sender;
            _activeTicket.Data.TheTicketData.Status = lnk.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void StatusUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }
    }
}
