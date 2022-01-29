using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NSLDSCONSO
{
    public partial class MainForm : Form
    {
        private DataAccess da;
        ProcessLogRun plr;
        string lastInput = "";
        string lastOutput = "";
        public MainForm(ProcessLogRun plr)
        {
            InitializeComponent();
            this.da = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, false));
            this.plr = plr;
            SetButtons();
            lastInput = Properties.Settings.Default.LastSelectedFileLocation;
            string genFile = "NSLDS_FILE_" + DateTime.Now.ToString("ddMMyy_hhmmss");
            lastOutput = Path.Combine(EnterpriseFileSystem.GetPath("NSLDSCONSO_OUTPUT"), genFile);
            FilePathBox.Text = lastInput;
            if (!DataAccessHelper.TestMode)
                CleanMenuButton.Visible = false;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (ImportButton.Checked)
            {
                using (var ofd = new OpenFileDialog())
                {
                    try
                    {
                        ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.LastSelectedFileLocation);
                    }
                    catch (ArgumentException)
                    {
                        //last file doesn't exist, don't worry about it
                    }
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        FilePathBox.Text = ofd.FileName;
                        Properties.Settings.Default.LastSelectedFileLocation = ofd.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            else
            {
                using (var sfd = new SaveFileDialog())
                {
                    try
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(FilePathBox.Text);
                    }
                    catch (ArgumentException)
                    {
                        //last file doesn't exist, don't worry about it
                    }
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        FilePathBox.Text = sfd.FileName;
                    }
                }
            }
            SetButtons();
        }

        private void SetButtons()
        {
            if (ImportButton.Checked)
                ProcessButton.Enabled = File.Exists(FilePathBox.Text);
            else
                ProcessButton.Enabled = true;
        }

        private void FilePathBox_TextChanged(object sender, EventArgs e)
        {
            SetButtons();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            Start(() =>
            {
                if (ImportButton.Checked)
                    using (var parser = new DataHistoryParser(da, FilePathBox.Text, LogItem, LogError))
                        ProcessParsing(parser);
                else
                {
                    EnableForm(false);
                    if (!File.Exists(FilePathBox.Text) || Dialog.Def.YesNo("File already exists.  Overwrite?"))
                        new NsldsBgGenerator(plr, da, LogItem, LogError, allowBorrowersWithoutReleasedLoansToolStripMenuItem.Checked, allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem.Checked).Generate(FilePathBox.Text);
                    EnableForm(true);
                }
            });
        }

        private void LogItem(string text)
        {
            Invoke(() =>
            {
                StatusBox.Text = DateTime.Now.ToShortTimeString() + " - " + text + Environment.NewLine + StatusBox.Text;
            });
        }

        private void CleanMenuButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Def.YesNo("This will remove all data from [nsldsconso] tables.  Do you want to continue?"))
            {
                if (da.CleanAndReseedTables())
                    LogItem("Clean and Re-seed complete.");
                else
                    LogError("Unable to Clean and Re-seed.  Please check Process Logger for more information.", null, NotificationSeverityType.Warning);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogItem("Using Process Log ID# " + plr.ProcessLogId);
            var existing = da.GetMostRecentDataLoadRun();
            if (existing != null && (!existing.EndedOn.HasValue || existing.BorrowerCount != existing.ActualBorrowerCount))
            {
                if (new RecoveryForm(existing).ShowDialog() == DialogResult.OK)
                {
                    this.FilePathBox.Text = existing.Filename;
                    Start(() =>
                    {
                        using (var parser = new DataHistoryParser(da, FilePathBox.Text, LogItem, LogError, existing))
                            ProcessParsing(parser);
                    });
                }
            }
        }

        private void ProcessParsing(DataHistoryParser parser)
        {
            EnableForm(false);
            new BorrowerUploader(plr, da, LogItem, LogError).Process(parser);
            EnableForm(true);
        }

        private void EnableForm(bool enable)
        {
            Invoke(() =>
            {
                ProcessGroup.Enabled = enable;
                InputGroup.Enabled = enable;
                MainMenu.Enabled = enable;
            });
        }

        private void LogError(string message, Exception e, NotificationSeverityType severity)
        {
            plr.AddNotification(message, NotificationType.ErrorReport, severity, e);
            LogItem(message);
        }

        private void Start(Action a)
        {
            Task.Run(a);
        }

        private void Invoke(Action a)
        {
            this.BeginInvoke(a);
        }

        private void SelectionButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (ImportButton.Checked)
            {
                InputGroup.Text = "Select an EA80 File";
                ProcessButton.Text = "Upload File";
                lastOutput = FilePathBox.Text;
                FilePathBox.Text = lastInput;
            }
            else if (GenerateButton.Checked)
            {
                InputGroup.Text = "Output File Path";
                ProcessButton.Text = "Generate File";
                lastInput = FilePathBox.Text;
                FilePathBox.Text = lastOutput;
            }
        }

        private void fixUnmappedLoansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MapperForm(da).ShowDialog();
        }
    }
}
