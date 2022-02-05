using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace WLCLETALGN
{
    public partial class ProcessingForm : Form
    {
        public AlignWelcomeLetters Letters;
        private string UserId;
        private int Max;
        private Thread LetterThread;
        private Thread CommentThread;
        public bool ShouldRecover = false;
        private int FadingSpeed = 20;
        public bool DidCancel = false;
        private bool IsClosing = false;

        public ProcessingForm(string userId, AlignWelcomeLetters letters)
        {
            InitializeComponent();
            Letters = letters;
            UserId = userId;
            UserIdDisplay.Text = "User ID: " + userId;
            Letters.Rec = new RecoveryLog(Letters.RecoveryFile);
        }

        /// <summary>
        /// Uses and OpenFileDialog to choose the file to process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            Process.Enabled = false;
            Counter.Text = "Loading Borrowers";
            if (!Letters.Rec.RecoveryValue.IsNullOrEmpty())
            {
                if (MessageBox.Show("A recovery file was found. Do you want to recover?", "Recovery Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    ShouldRecover = true;
                    CreateEOJReports();
                    Letters.GetBorrowerCount(Letters.Rec.RecoveryValue.SplitAndRemoveQuotes(",")[3]);
                    LoadBorrowers();
                    Process.Text = "Recover";
                    Letters.FileName = Letters.Rec.RecoveryValue.SplitAndRemoveQuotes(",")[3];
                    SetProgressValues(Letters.EndOfJob[AlignWelcomeLetters.EOJ_Total].Value);
                    return; //if in recovery, do not process like a new run.
                }
            }
            CreateEOJReports();
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = EnterpriseFileSystem.TempFolder;
            file.Filter = "All Files (*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                Letters.GetBorrowerCount(file.FileName);
                Letters.FileName = file.FileName;
                LoadBorrowers();
                Process.Text = "Process";
            }
            SetProgressValues(Letters.EndOfJob[AlignWelcomeLetters.EOJ_Total].Value);
        }

        /// <summary>
        /// Load the borrowers from the selected file.
        /// </summary>
        private void LoadBorrowers()
        {
            this.Invoke(new Action(() =>
                {
                    if (Letters.Borrowers.Count > 0)
                    {
                        Cancel.Text = "Cancel";
                        Process.Enabled = true;
                        Status.Text = "";
                    }
                }));
        }

        /// <summary>
        /// Creates a new thread for printing and comments and starts the threads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Click(object sender, EventArgs e)
        {
            UpdateStatusText();
            Process.Enabled = false;
            Open.Enabled = false;
            ArcProgress.Value = 0;
            Letters.RecoveryValues[3] = Letters.FileName;
            Letters.UpdateRecovery();

            //Printing thread
            LetterThread = new Thread(() =>
                {
                    if (!Status.Text.ToLower().Contains("printed"))
                    {
                        Letters.IsPrinting = true;
                        Letters.Print();
                    }
                    else
                    {
                        Letters.IsPrinting = false;
                        StatusTimer.Stop();
                        Status.ForeColor = Color.Black;
                    }
                });
            LetterThread.Start();

            //Comments thread
            CommentThread = new Thread(() =>
                {
                    Letters.IsCommenting = true;
                    Letters.AddComments();
                });
            CommentThread.Start();
        }

        /// <summary>
        /// Set the status text, assign the timer interval and start the timer.
        /// </summary>
        private void UpdateStatusText()
        {
            this.Invoke(new Action(() =>
                {
                    Status.Text = "Printing Letters";
                    StatusTimer.Interval = 100;
                    StatusTimer.Start();
                    Status.Text = Letters.Rec.RecoveryValue.SplitAndRemoveQuotes(",")[0].ToLower().Contains("printed") ? "Letters Printed" : "Printing Letters";
                }));
        }

        /// <summary>
        /// Create a custom end of job report
        /// </summary>
        private void CreateEOJReports()
        {
            //Check to see if recoverying. If not, delete the files and start new EOJ and ERR files.
            string eojFile = EnterpriseFileSystem.TempFolder + "EOJ_WLCLETALGN_" + UserId + ".txt";
            if (File.Exists(eojFile) && !ShouldRecover)
                File.Delete(eojFile);
            string errFile = EnterpriseFileSystem.TempFolder + "ERR_WLCLETALGN.txt";
            if (File.Exists(errFile) && !ShouldRecover)
                File.Delete(errFile);
            if (!ShouldRecover)
            {
                Letters.Rec.Delete();
                Letters.Rec = new RecoveryLog(Letters.RecoveryFile);
            }
            Letters.EndOfJob = new EndOfJobReport("WLCLETALGN", "EOJ_BU35", Letters.EOJ_Headers, UserId);
            Letters.ErrReport = new ErrorReport("WLCLETALGN", "ERR_BU35");
        }

        /// <summary>
        /// Sets the progress bar max value
        /// </summary>
        /// <param name="max">The number of borrowers in the file being processed</param>
        public void SetProgressValues(int max)
        {
            this.Invoke(new Action(() =>
                {
                    ArcProgress.Maximum = max;
                    Counter.Text = string.Format("0 of {0} arcs processed", max);
                    Max = max;
                }));
        }

        /// <summary>
        /// Increments the progress bar by 1.
        /// </summary>
        /// <param name="count">Default to 1 but can be more for recovery</param>
        public void IncrementProgress(int count = 1)
        {
            this.Invoke(new Action(() =>
                {
                    ArcProgress.Increment(count);
                    Counter.Text = string.Format("{0} of {1} arcs processed", ArcProgress.Value, Max);
                }));
        }

        /// <summary>
        /// Changes the status to Complete, aborts the thread and checks to see if the comments are done.
        /// </summary>
        public void FinishedPrinting()
        {
            this.Invoke(new Action(() =>
                {
                    Letters.IsPrinting = false;
                    StatusTimer.Stop();
                    Status.ForeColor = Color.Black;
                    Status.Text = "Printing Complete";
                    if (!Letters.IsCommenting)
                    {
                        if (!DidCancel) //Don't call EndProcessing if it was canceled.
                        {
                            Cancel.Enabled = true;
                            Cancel.Text = "Done";
                            Open.Enabled = true;
                            Letters.EndProcessLog();
                        }
                        this.Close();
                    }
                }));
        }

        /// <summary>
        /// Shows the number of arcs that were completed, aborts the thread and checks to see if the printing is done.
        /// </summary>
        public void FinishedComments()
        {
            this.Invoke(new Action(() =>
                {
                    if (Letters.EndOfJob != null)
                        Counter.Text = string.Format("Added {0} of {1} arcs", Letters.EndOfJob.Counts[AlignWelcomeLetters.EOJ_TotalArcProcessed].Value, Letters.EndOfJob.Counts[AlignWelcomeLetters.EOJ_Total].Value);

                    Letters.IsCommenting = false;
                    if (!Letters.IsPrinting)
                    {
                        if (!DidCancel) //Don't call EndProcessing if it was canceled.
                        {
                            Cancel.Enabled = true;
                            Cancel.Text = "Done";
                            Open.Enabled = true;
                            Letters.EndProcessLog();
                        }
                        this.Close();
                    }
                }));
        }

        /// <summary>
        /// Checks to see if the process has started and sets the processing to false. Closes the window if no processes are going.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            DidCancel = true;
            Cancel.Enabled = false;
            Letters.IsCommenting = false;
            if (Letters.IsPrinting)
                MessageBox.Show("The comments have been stopped but the printing portion has been sent to the printer. Please wait until the printing has completed."
                    + "\r\n\r\n If you need to cancel the printing, click the 'Cancel' button on the print job or end the process", "Printing in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (!Letters.IsPrinting && !Letters.IsCommenting)
                this.Close();
        }

        /// <summary>
        /// Timer to make the status label fade in and out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            Status.ForeColor = Color.FromArgb(Status.ForeColor.R + FadingSpeed,
                Status.ForeColor.G + FadingSpeed, Status.ForeColor.B + FadingSpeed);
            if (Status.ForeColor.R >= this.BackColor.R)
                FadingSpeed = -20;
            else if (Status.ForeColor.R == Color.Black.R)
                FadingSpeed = 20;
        }

        private void ProcessingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsClosing && (Letters.IsPrinting || Letters.IsCommenting))
            {
                IsClosing = true;
                e.Cancel = true;
                Cancel_Click(sender, e);
            }
        }
    }
}