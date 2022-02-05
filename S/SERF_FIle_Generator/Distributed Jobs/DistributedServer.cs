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
using Uheaa.Common.DataAccess;

namespace SERF_File_Generator
{
    public partial class DistributedServer : Form
    {
        const string TimeFormat = @"hh\:mm\:ss";
        public DistributedServer(DistributedJobInfo info, DateTime startTime, int totalFiles)
        {
            InitializeComponent();

            Job = info;
            WorkspaceText.Text = info.Workspace.WorkspaceDirectory;
            JobText.Text = Path.GetFileName(info.JobDirectory);
            StartTime = startTime;
            StartText.Text = StartTime.ToString(TimeFormat);
            TotalFiles = totalFiles;
            UpdateTimer.Enabled = true;
        }

        private DistributedJobInfo Job { get; set; }
        private DateTime StartTime { get; set; }
        private int TotalFiles { get; set; }

        private int lastFileCount = 0;
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            int fileCount = 0;
            foreach (string identifier in Job.FileIdentifiers)
            {
                string directory = Path.Combine(Job.CompleteDirectory, identifier);
                fileCount += Directory.GetFiles(directory).Length;
            }
            TimeSpan elapsed = (DateTime.Now - StartTime);
            ElapsedText.Text = elapsed.ToString(TimeFormat);
            if (fileCount != lastFileCount)
            {
                if (fileCount == 0)
                    EstimatedText.Text = "";
                else
                {
                    double ticksPerFile = elapsed.Ticks / fileCount;
                    double ticksLeft = ticksPerFile * (TotalFiles - fileCount);
                    EstimatedText.Text = new TimeSpan((long)ticksLeft).ToString(TimeFormat);
                }
            }
            lastFileCount = fileCount;
            ProgressText.Text = string.Format("{2}% ({0}/{1})", fileCount, TotalFiles, Math.Round((fileCount / (double)TotalFiles) * 100, 2));
            if (fileCount == TotalFiles)
            {
                UpdateTimer.Enabled = false;
                Finish();
            }
        }

        private void Finish()
        {
            List<string> files = new List<string>();
            foreach (string identifier in Job.FileIdentifiers)
            {
                string directory = Path.Combine(Job.CompleteDirectory, identifier);
                string serfFile = Program.GenerateSerfFileName(identifier);
                using (StreamWriter sw = new StreamWriter(serfFile))
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        using (StreamReader sr = new StreamReader(file))
                            while (!sr.EndOfStream)
                                sw.WriteLine(sr.ReadLine());
                    }
                files.Add(serfFile);
            }
            Job.Complete();
            MessageBox.Show("Finished Processing:" + string.Join(Environment.NewLine, files.ToArray()));
        }
    }
}

