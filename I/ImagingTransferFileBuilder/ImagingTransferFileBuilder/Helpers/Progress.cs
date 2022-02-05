using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImagingTransferFileBuilder
{
    public static class Progress
    {
        private static Label ProgressLabel { get; set; }
        private static Control Disabler { get; set; }
        private static decimal percentage;
        private static string ProcessName { get; set; }
        public static decimal Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                percentage = value;
                ProgressLabel.Text = ProcessName + " - " + string.Format("{0:0.##}%", percentage);
            }
        }
        public static void RegisterLabel(Label l)
        {
            ProgressLabel = l;
        }
        public static void RegisterDisabler(Control c)
        {
            Disabler = c;
        }
        public static void Start(string processName)
        {
            Results.Clear();
            ProcessName = processName;
            Increments = 1;
            Percentage = 0;
            if (Disabler != null)
                Disabler.Enabled = false;
        }
        public static void Finish()
        {
            ProgressLabel.Text = ProcessName + " - Complete";
            if (Disabler != null)
                Disabler.Enabled = true;
            Results.Finished();
        }
        public static void Failure()
        {
            ProgressLabel.Text = ProcessName + " - Failed";
            if (Disabler != null)
                Disabler.Enabled = true;
        }
        public static int Increments { get; set; }
        public static void Increment()
        {
            
            Percentage += 100m / (decimal)Increments;
            System.Windows.Forms.Application.DoEvents();
            if (Percentage >= 100)
            {
                Finish();
            }
        }
    }
}
