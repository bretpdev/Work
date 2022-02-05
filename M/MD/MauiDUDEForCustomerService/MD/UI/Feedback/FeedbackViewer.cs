using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace MD
{
    public partial class FeedbackViewer : BaseForm
    {
        public FeedbackViewer()
        {
            InitializeComponent();
            SyncData();
        }

        private List<FeedbackSubmission> submissions;
        private void SyncData()
        {
            SubmittedLabel.Visible = false;
            ProcessedButton.Enabled = false;
            DetailsBox.Text = "";
            ScreenshotBox.Image = null;


            MyQuestionsGrid.AutoGenerateColumns = false;
            MyQuestionsGrid.DataSource = submissions = FeedbackSubmission.GetPending();
            LoadSubmission();
        }

        private void MyQuestionsGrid_SelectionChanged(object sender, EventArgs e)
        {
            LoadSubmission();
        }

        private void LoadSubmission()
        {
            if (MyQuestionsGrid.SelectedRows.Count == 1)
            {
                var submission = submissions[MyQuestionsGrid.SelectedRows[0].Index];
                DetailsBox.Text = submission.FeedbackDetails;
                ScreenshotBox.BackgroundImage = null;
                ScreenshotBox.Image = null;
                ReflectionScreenshotBox.BackgroundImage = null;
                ReflectionScreenshotBox.Image = null;
                if (submission.Screenshot != null)
                {
                    ScreenshotBox.BackgroundImage = submission.Screenshot.ToImage();
                    if (submission.ScreenshotOverlay != null)
                        ScreenshotBox.Image = submission.ScreenshotOverlay.ToImage();
                }
                if (submission.ReflectionScreenshot != null)
                {
                    ReflectionScreenshotBox.BackgroundImage = submission.ReflectionScreenshot.ToImage();
                    if (submission.ReflectionScreenshotOverlay != null)
                        ReflectionScreenshotBox.Image = submission.ReflectionScreenshotOverlay.ToImage();
                }
                SubmittedLabel.Text = "Submitted on " + submission.RequestedOn.ToString("MM/dd/yyyy") + " by " + UsernameHelper.GetDisplayName(submission.RequestedBy);
                SubmittedLabel.Visible = true;
                ProcessedButton.Enabled = true;
                ProcessedButton.Tag = submission;
            }
        }
        private void ProcessedButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Info.YesNo("Are you sure you want to mark this submission as processed?  It will no longer be displayed in this form afterwards."))
            {
                var submission = (FeedbackSubmission)ProcessedButton.Tag;
                FeedbackSubmission.ProcessFeedback(submission);
                SyncData();
            }
        }

        private void ScreenshotBox_Click(object sender, EventArgs e)
        {
            if (ScreenshotBox.BackgroundImage != null)
            {
                new ImageAnnotator(ScreenshotBox.BackgroundImage, ScreenshotBox.Image, true).ShowDialog();
            }
        }

        private void ReflectionScreenshotBox_Click(object sender, EventArgs e)
        {
            if (ReflectionScreenshotBox.BackgroundImage != null)
            {
                new ImageAnnotator(ReflectionScreenshotBox.BackgroundImage, ReflectionScreenshotBox.Image, true).ShowDialog();
            }
        }
    }
}
