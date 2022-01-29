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
using Uheaa.Common.DataAccess;

namespace MD
{
    public partial class FeedbackForm : BaseForm
    {
        Image screenshot;
        Image screenshotOverlay;
        Image reflectionScreenshot;
        Image reflectionScreenshotOverlay;
        FeedbackTypeEnum type;
        public FeedbackForm()
        {
            InitializeComponent();
        }

        public void Initialize(Form calledFrom, FeedbackTypeEnum type)
        {
            this.type = type;
            if (type == FeedbackTypeEnum.Feature)
            {
                this.Text = "Feature Request";
                SubmitButton.Text = "Submit Feature Request";
            }
            else
            {
                this.Text = "Bug Report";
                SubmitButton.Text = "Submit Bug Report";
            }

            screenshot = new Bitmap(calledFrom.Width, calledFrom.Height);
            calledFrom.DrawToBitmap((Bitmap)screenshot, new Rectangle(0, 0, screenshot.Width, screenshot.Height));
            ScreenshotBox.BackgroundImage = screenshot;
            ScreenshotBox.BackgroundImageLayout = ImageLayout.Stretch;

            var handle = Hlpr.RH.ReflectionHandle;
            reflectionScreenshot = ImageHelper.GetImageFromHwnd(handle);

            ReflectionBox.BackgroundImage = reflectionScreenshot;
            ReflectionBox.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ScreenshotBox_Click(object sender, EventArgs e)
        {
            using (var annotator = new ImageAnnotator(screenshot, screenshotOverlay))
            {
                if (annotator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    screenshotOverlay = annotator.Overlay;
                    ScreenshotBox.Image = screenshotOverlay;
                    ScreenshotBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void ReflectionBox_Click(object sender, EventArgs e)
        {
            if (reflectionScreenshot == null) return;
            using (var annotator = new ImageAnnotator(reflectionScreenshot, reflectionScreenshotOverlay))
            {
                if (annotator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    reflectionScreenshotOverlay = annotator.Overlay;
                    ReflectionBox.Image = reflectionScreenshotOverlay;
                    ReflectionBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Info.YesNo("Are you sure you want to submit this request?"))
            {
                FeedbackSubmission fs = new FeedbackSubmission() { FeedbackDetails = NotesBox.Text, FeedbackType = type.ToString().ToLower() };
                fs.AppVersion = VersionHelper.RelevantVersion;
                fs.Screenshot = screenshot.ToBytes();
                if (screenshotOverlay != null)
                    fs.ScreenshotOverlay = screenshotOverlay.ToBytes();
                if (reflectionScreenshot != null)
                    fs.ReflectionScreenshot = reflectionScreenshot.ToBytes();
                if (reflectionScreenshotOverlay != null)
                    fs.ReflectionScreenshotOverlay = reflectionScreenshotOverlay.ToBytes();
                FeedbackSubmission.SubmitRequest(fs);
                this.Close();
            }
        }

    }
}
