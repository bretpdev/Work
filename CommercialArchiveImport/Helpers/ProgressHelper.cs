using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommercialArchiveImport
{
    public static class ProgressHelper
    {
        private static Label ProgressLabel { get; set; }
        private static List<Control> Disablees { get; set; }
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
                SetProgress(ProcessName + " - " + string.Format("{0:0.00}%", percentage));
            }
        }

        static ProgressHelper()
        {
            Disablees = new List<Control>();
        }

        public static void RegisterLabel(Label l)
        {
            ProgressLabel = l;
        }
        public static void RegisterDisablee(Control c)
        {
            Disablees.Add(c);
        }
        public static void Start(string processName)
        {
            ResultsHelper.Clear();
            Next(processName);
        }
        public static void Next(string processName)
        {
            ProcessName = processName;
            Increments = 1;
            Percentage = 0;
            DisableDisablees();
        }
        public static void Finish()
        {
            SetProgress(ProcessName + " - Complete");
            EnableDisablees();
            ResultsHelper.Finished();
        }
        public static void Failure()
        {
            ProgressLabel.Text = ProcessName + " - Failed";
            foreach (Control c in Disablees)
                c.Enabled = true;
        }
        public static int Increments { get; set; }
        public static void Increment()
        {
            Percentage += 100m / (decimal)Increments;
        }

        public static void DisableDisablees() { SetDisablees(false); }
        public static void EnableDisablees() { SetDisablees(true); }
        public static void SetDisablees(bool enabled)
        {
            foreach (Control c in Disablees)
                Invoke(c, () =>
                {
                    c.Enabled = enabled;
                });
        }

        public static void SetProgress(string text)
        {
            Invoke(ProgressLabel, () =>
            {
                ProgressLabel.Text = text;
            });
        }

        public static void Invoke(Control c, Action a)
        {
            c.Invoke(a);
        }
    }
}
