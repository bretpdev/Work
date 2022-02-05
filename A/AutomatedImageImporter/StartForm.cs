using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace AutomatedImageImporter
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            MonitorFolderText.Text = @"\\imgprodkofax\ascent$\FederalImport\Monitor";
            MonitorFolderText.Text = @"C:\Users\ewalker\Desktop\monitor";
            SourceZipText.Text = @"C:\Users\ewalker\Desktop\COLL_700502_898502_PFD96_20130219_20130215124800.ZIP";
            LineNumberPicker.Value = 50;
#endif
        }

        private void BeginButton_Click(object sender, EventArgs e)
        {
            if (MonitorFolderText.Text.ToLower() == @"\\imgprodkofax\ascent$\FederalImport\Monitor".ToLower())
            {
                if (MessageBox.Show("The monitor folder is set to production.  Are you sure you want to run this process against the PROD imaging system?",
                    "Prod Monitor Detected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            if (MonitorFolderText.Text.ToLower() == @"\\imgdevkofax\ascent$\FederalImport\Monitor".ToLower())
            {
                if (MessageBox.Show("The monitor folder is set to development.  Are you sure you want to run this process against the DEV imaging system?",
                    "Dev Monitor Detected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            if (FileSystemHelper.CheckFile(SourceZipText.Text, false))
                if (FileSystemHelper.CheckDirectory(MonitorFolderText.Text, false))
                {
                    if (Directory.GetFiles(MonitorFolderText.Text).Any())
                        if (MessageBox.Show("The Monitor folder isn't empty.  Are you sure you want to proceed?", "Monitor Check", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    MainForm mf = new MainForm(SourceZipText.Text, MonitorFolderText.Text, (int)LineNumberPicker.Value);
                    this.Hide();
                    mf.ShowDialog();
                    this.Close();
                }
        }

        private void SourceBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Zip Files (*.zip)|*.zip";
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SourceZipText.Text = diag.FileName;
        }

        private void MonitorBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFolderDialog diag = new OpenFolderDialog();
            if (diag.ShowDialog(this.Handle, false) == System.Windows.Forms.DialogResult.OK)
                MonitorFolderText.Text = diag.Folder;
        }
    }
}
