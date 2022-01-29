using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;

namespace IMGCLIMPRT
{
    public partial class MainForm : Form, IRebindable
    {
        Log log;
        Locations locations;
        Coordinator coordinator;

        public MainForm(string scriptId)
        {
            InitializeComponent();

            log = new Log(scriptId, this);
            locations = new Locations();
            coordinator = new Coordinator(scriptId, locations, log, Rebind);
            coordinator.LoadPendingZips();
            if (DataAccessHelper.TestMode)
                this.Text = "[TEST MODE] " + this.Text;
        }

        /// <summary>
        /// Smoothes the refresh rate of the lists.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        /// <summary>
        /// Update the lists for all ListBoxes on the forms.
        /// </summary>
        public void Rebind()
        {
            Invoke(() =>
            {
                using (new ListBoxUpdater(this))
                {
                    //creating new objects via .ToList() seems to be the only thing that will trigger an update.
                    CompletedZipBox.DataSource = coordinator.CompletedZips.ToList();
                    FailedZipBox.DataSource = coordinator.FailedZips.ToList();
                    PendingZipBox.DataSource = coordinator.PendingZips.ToList();
                    LogBox.DataSource = log.LogItems.ToList();
                }
            });
        }

        /// <summary>
        /// Invoke the given action only if necessary.
        /// </summary>
        private void Invoke(Action a)
        {
            if (InvokeRequired)
                base.Invoke(a);
            else
                a();
        }

        /// <summary>
        /// Display the selected item in a message box if the list box is double-clicked.
        /// </summary>
        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            var list = sender as ListControl;
            if (list.SelectedValue != null)
                MessageBox.Show(list.SelectedValue.ToString());
        }

        private void BeginButton_Click(object sender, EventArgs e)
        {
            BeginButton.Enabled = false;
            coordinator.BeginProcessing(true);
        }

        private void PendingZipLabel_Click(object sender, EventArgs e)
        {
            if (DataAccessHelper.TestMode)
            {
                var ofd = new OpenFolderDialog();
                if (ofd.ShowDialog(this.Handle, false) == System.Windows.Forms.DialogResult.OK)
                {
                    coordinator.LoadPendingZips(ofd.Folder);
                }
            }
        }
    }
}
